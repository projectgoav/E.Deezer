using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Threading;
using System.Threading.Tasks;

using E.Deezer.Api;
using E.Deezer.Util;

namespace E.Deezer
{
    public enum AuthenticationStatus
    {
        Unauthenticated,
        LoggingIn,
        AuthenticationFailed,
        Authenticated,
    };


    public delegate void AuthenticationStateChangedHandler(AuthenticationStatus oldStatus,
                                                           AuthenticationStatus newStatus,
                                                           IUserV2 userInformation);

    // Handles authenticating the given access token provided by the 
    // user to the session/object containing this one.
    // Internals will be thread-safe, but callers shouldn't suspect that
    // event callbacks will always occur on the same thread.

    // TODO: Will need to check for authentication errors and handle logouts etc..
    // TODO: Relies on the clients above this to call something to check for Deezer
    //       errors. Can this be improved any?
    internal interface IAuthenticationService
    {
        IUserV2 CurrentUser { get; }
        string AccessToken { get; }
        bool IsAuthenticated { get; }
        IPermissions UserPermissions { get; }

        // Publically avaliable functions
        Task<bool> Login(string accessToken, CancellationToken cancellationToken);
        Task<bool> Logout(CancellationToken cancellationToken);

        event AuthenticationStateChangedHandler OnAuthenticationStateChanged;
    }


    internal interface IAuthenticationServiceInternal : IDisposable
    {
        bool HasPermission(DeezerPermissions requestedPermission);

        string GetAccessTokenQueryString();

        bool LogoutIfAuthenticationError(IError error);
    }


    internal class AuthenticationService : IAuthenticationService, IAuthenticationServiceInternal
    {
        private const uint PERMISSION_ERROR_CODE = 200;
        private const uint INVALID_TOKEN_ERROR_CODE = 300;

        private static readonly CancellationToken PRE_CANCELLED_TOKEN = new CancellationToken(canceled: true);


        private readonly object lockObj;
        private readonly IDeezerClient client;

        private Task<bool> inflightAuthTask;
        private AuthenticationStatus currentAuthStatus;
        private CancellationTokenSource cancellationTokenSource;


        public AuthenticationService(IDeezerClient client)
        {
            this.client = client;

            this.lockObj = new object();
            this.inflightAuthTask = Task.FromResult<bool>(false);
        }


        private CancellationToken CancellationToken => this.cancellationTokenSource.GetCancellationTokenSafe();

        // IAuthenticationService
        public IUserV2 CurrentUser { get; private set; }
        public string AccessToken { get; private set; }
        public IPermissions UserPermissions { get; private set; }

        public bool IsAuthenticated => this.CurrentUser != null;


        public event AuthenticationStateChangedHandler OnAuthenticationStateChanged;


        public Task<bool> Login(string accessToken, CancellationToken cancellationToken)
        {
            lock (this.lockObj)
            {
                // Tokens match so don't do anything
                if (this.AccessToken == accessToken)
                {
                    return Task.FromResult<bool>(true);
                }

                LogoutInternal();

                this.AccessToken = accessToken;

                CancelInflightTask();

                this.cancellationTokenSource = new CancellationTokenSource();
            }

            RaiseAuthenticationStateChanged(AuthenticationStatus.LoggingIn);

            string resource = $"user/me?{GetAccessTokenQueryString()}";

            this.inflightAuthTask = this.client.Get(resource, this.CancellationToken, json => Api.UserV2.FromJson(json, this.client))
                                               .ContinueWith(OnLoginComplete)
                                               .Unwrap();
                                             
            return this.inflightAuthTask;
        }

        public Task<bool> Logout(CancellationToken cancellationToken)
        {
            LogoutInternal();
            return Task.FromResult<bool>(true);
        }




        // IAuthenticationServiceInternal
        public bool HasPermission(DeezerPermissions requestedPermission)
            => this.UserPermissions != null && this.UserPermissions.HasPermission(requestedPermission);


        public string GetAccessTokenQueryString()
            => $"access_token={this.AccessToken}";


        public bool LogoutIfAuthenticationError(IError error)
        {
            switch(error.Code)
            {
                case INVALID_TOKEN_ERROR_CODE:
                    this.LogoutInternal();
                    return true;
            }

            return false;
        }


        // Private Impl
        public void LogoutInternal()
        {
            lock (this.lockObj)
            {
                this.AccessToken = null;
                this.CurrentUser = null;
                this.UserPermissions = null;
            }

            RaiseAuthenticationStateChanged(AuthenticationStatus.Unauthenticated);
        }


        private void RaiseAuthenticationStateChanged(AuthenticationStatus incomingStatus)
        {
            if (this.currentAuthStatus != incomingStatus)
            {
                var oldStatus = this.currentAuthStatus;

                lock (this.lockObj)
                {
                    this.currentAuthStatus = incomingStatus;
                }

                this.OnAuthenticationStateChanged?.Invoke(oldStatus, incomingStatus, this.CurrentUser);
            }
        }



        private Task<bool> OnLoginComplete(Task<IUserV2> userFetchTask)
        {
            if (userFetchTask.IsFaulted)
            {
                RaiseAuthenticationStateChanged(AuthenticationStatus.AuthenticationFailed);
                return Task.FromResult<bool>(false);
            }

            lock (lockObj)
            {
                this.CurrentUser = userFetchTask.Result;
            }


            string resource = $"user/me/permissions?{GetAccessTokenQueryString()}";

            return this.client.Get(resource, this.CancellationToken, OAuthPermissions.FromJson)
                              .ContinueWith(t =>
                              {
                                  if (t.IsFaulted)
                                  {
                                      LogoutInternal();
                                      RaiseAuthenticationStateChanged(AuthenticationStatus.AuthenticationFailed);

                                      return false;
                                  }

                                  this.UserPermissions = t.Result;

                                  RaiseAuthenticationStateChanged(AuthenticationStatus.Authenticated);
                                  return true;

                              }, this.CancellationToken, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }


        private void CancelInflightTask()
        {
            if (this.cancellationTokenSource != null)
            {
                this.cancellationTokenSource.Cancel();
                this.cancellationTokenSource.Dispose();
                this.cancellationTokenSource = null;
            }
        }



        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //TODO : Probs need to check or set something
                //       so future calls will fail.

                CancelInflightTask();
            }
        }
    }
}
