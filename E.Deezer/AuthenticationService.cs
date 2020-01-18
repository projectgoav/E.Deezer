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


    internal interface IAuthenticationServiceInternal
    {
        bool HasPermission(DeezerPermissions requestedPermission);

        string GetAccessTokenQueryString();

        bool LogoutIfAuthenticationError(IError error);
    }


    internal class AuthenticationService : IAuthenticationService, IAuthenticationServiceInternal
    {
        private const uint PERMISSION_ERROR_CODE = 200;
        private const uint INVALID_TOKEN_ERROR_CODE = 300;


        private readonly object lockObj;
        private readonly IDeezerClient client;
        private readonly ExecutorService executorService;


        private AuthenticationStatus currentAuthStatus;

        public AuthenticationService(IDeezerClient client)
        {
            this.client = client;

            this.lockObj = new object();
        }


        private CancellationToken CancellationToken => CancellationToken.None;


        // IAuthenticationService
        public IUserV2 CurrentUser { get; private set; }
        public string AccessToken { get; private set; }
        public IPermissions UserPermissions { get; private set; }

        public bool IsAuthenticated => this.CurrentUser != null;


        public event AuthenticationStateChangedHandler OnAuthenticationStateChanged;


        public Task<bool> Login(string accessToken, CancellationToken cancellationToken)
        {
            LogoutInternal();

            lock (this.lockObj)
            {
                this.AccessToken = accessToken;
            }

            string resource = $"user/me?{GetAccessTokenQueryString()}";

            //TODO: We should store this as an inflight task
            //      So, should a user request a method that requires permissions
            //      We can evalute it AFTER the permissions tasks have completed!

            //TODO: Raise new status changed for the Logging-In state?
            return this.client.Get(resource, this.CancellationToken, json => Api.UserV2.FromJson(json, this.client))
                              .ContinueWith(async t =>
                              {
                                  if (t.IsFaulted)
                                  {
                                      return false;
                                  }

                                  this.CurrentUser = t.Result;

                                  try
                                  {
                                      this.UserPermissions = await GetUserPermissions();

                                      if (this.UserPermissions == null)
                                      {
                                          return false;
                                      }
                                  }
                                  catch
                                  {
                                      // Something went wrong fetching user permissions
                                      // TODO: Should we log something to the terminal??
                                      // In theory, the underlying client will catch any issues when deserializing
                                      return false;
                                  }


                                  RaiseAuthenticationStateChanged(AuthenticationStatus.Authenticated);
                                  return true;

                              }, this.CancellationToken, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default)
                              .Unwrap();
        }

        public Task<bool> Logout(CancellationToken cancellationToken)
        {
            LogoutInternal();
            return Task.FromResult<bool>(true);
        }


        private Task<IPermissions> GetUserPermissions()
        {
            string resource = $"user/me/permissions?{GetAccessTokenQueryString()}";

            return this.client.Get(resource, this.CancellationToken, OAuthPermissions.FromJson)
                              .ContinueWith(t =>
                              {
                                  return t.IsFaulted ? null
                                                     : t.Result;
                              }, this.CancellationToken, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }


        // IAuthenticationServiceInternal
        public bool HasPermission(DeezerPermissions requestedPermission)
        {
            if (this.UserPermissions == null)
            {
                return false;
            }

            return this.UserPermissions.HasPermission(requestedPermission);
        }

        public string GetAccessTokenQueryString()
            => $"access_token={this.AccessToken}";

        // TODO: Audit this and make sure we do stuff correctly...
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
    }
}
