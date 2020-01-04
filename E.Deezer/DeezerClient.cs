using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using E.Deezer.Api;
using E.Deezer.Util;

namespace E.Deezer
{
    internal interface IDeezerClient : IDisposable
    {
        Endpoints.Endpoints Endpoints { get; }

        Task<TItem> Get<TItem>(string resource, CancellationToken cancellationToken, Func<JToken, TItem> itemFactory);
        Task<TItem> Get<TItem>(string resource, DeezerPermissions requiredPermissons, CancellationToken cancellationToken, Func<JToken, TItem> itemFactory);

        Task<bool> Post(string resource, DeezerPermissions requiredPermissions, CancellationToken cancellationToken);
        Task<TResult> Post<TResult>(string resource, DeezerPermissions requiredPermissions, CancellationToken cancellationToken, Func<JToken, TResult> resultFactory);

        Task<bool> Delete(string resource, DeezerPermissions requiredPermissions, CancellationToken cancellationToken);
        Task<TResult> Delete<TResult>(string resource, DeezerPermissions requiredPermissions, CancellationToken cancellationToken, Func<JToken, TResult> resultFactory);

        // Authentication
        event AuthenticationStateChangedHandler OnAuthenticationStateChanged;

        ulong CurrentUserId { get; }
        bool IsAuthenticated { get; }
    }


    internal class DeezerClient : IDeezerClient
    {
        private readonly ExecutorService executor;
        private readonly AuthenticationService authService;

        public DeezerClient(HttpMessageHandler handler)
        {
            this.executor = new ExecutorService(handler);
            this.authService = new AuthenticationService(this.executor);

            this.Endpoints = new Endpoints.Endpoints(this);
        }

        public Endpoints.Endpoints Endpoints { get; }


        // FIX ME: There's a lot of duplication in this class.
        //         We should try condense some of this down.

        public Task<TItem> Get<TItem>(string resource, CancellationToken cancellationToken, Func<JToken, TItem> itemFactory)
        {
            return executor.ExecuteGet(resource, cancellationToken)
                           .ContinueWith(t =>
                           {
                               t.ThrowIfFaulted();

                               var json = JObjectFromStream(t.Result);

                               //TODO: Maybe we should handle the error in here?
                               //      And only return on the happy path??            
                               return DeserializeErrorOr<TItem>(json, itemFactory);

                           }, cancellationToken, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }

        public Task<TItem> Get<TItem>(string resource, DeezerPermissions requiredPermissions, CancellationToken cancellationToken, Func<JToken, TItem> itemFactory)
        {
            var authenticationException = AssertAuthenticated(requiredPermissions);
            if (authenticationException != null)
            {
                throw authenticationException;
            }

            string actualResource = AppendAccessTokenToResourceIfRequired(resource);

            return Get<TItem>(actualResource, cancellationToken, itemFactory);
        }


        public Task<bool> Post(string resource,
                               DeezerPermissions requiredPermissions,
                               CancellationToken cancellationToken)
        {
            var authenticationException = AssertAuthenticated(requiredPermissions);
            if (authenticationException != null)
            {
                throw authenticationException;
            }

            string actualResource = AppendAccessTokenToResourceIfRequired(resource);

            return executor.ExecutePost(actualResource, cancellationToken)
                            .ContinueWith(t =>
                            {
                                t.ThrowIfFaulted();

                                using (var stream = t.Result)
                                using (var reader = new StreamReader(stream))
                                {
                                    // TODO: As the streams might be zipped we can't rely on stream length.
                                    //       Instead, read everything, compare the 2 values or fallback to 
                                    //       parsing json instead.
                                    var possibleJson = reader.ReadToEnd();

                                    if (possibleJson == "true")
                                        return true;

                                    if (possibleJson == "false")
                                        return false;

                                    var json = JObjectFromString(possibleJson);

                                    //TODO: Maybe we should handle the error in here?
                                    //      And only return on the happy path??            
                                    return DeserializeErrorOr<bool>(json, j => j.Value<bool>());
                                }

                            }, cancellationToken, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }


        public Task<TResult> Post<TResult>(string resource, 
                                           DeezerPermissions requiredPermissions, 
                                           CancellationToken cancellationToken,
                                           Func<JToken, TResult> resultFactory)
        {
            var authenticationException = AssertAuthenticated(requiredPermissions);
            if (authenticationException != null)
            {
                throw authenticationException;
            }

            string actualResource = AppendAccessTokenToResourceIfRequired(resource);

            return executor.ExecutePost(actualResource, cancellationToken)
                           .ContinueWith(t =>
                           {
                               t.ThrowIfFaulted();

                               var json = JObjectFromStream(t.Result);

                               //TODO: Maybe we should handle the error in here?
                               //      And only return on the happy path??            
                               return DeserializeErrorOr<TResult>(json, resultFactory);

                           }, cancellationToken, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

        }

        public Task<bool> Delete(string resource,
                       DeezerPermissions requiredPermissions,
                       CancellationToken cancellationToken)
        {
            var authenticationException = AssertAuthenticated(requiredPermissions);
            if (authenticationException != null)
            {
                throw authenticationException;
            }

            string actualResource = AppendAccessTokenToResourceIfRequired(resource);

            return executor.ExecutePost(actualResource, cancellationToken)
                            .ContinueWith(t =>
                            {
                                t.ThrowIfFaulted();

                                using (var stream = t.Result)
                                using (var reader = new StreamReader(stream))
                                {
                                    // TODO: As the streams might be zipped we can't rely on stream length.
                                    //       Instead, read everything, compare the 2 values or fallback to 
                                    //       parsing json instead.
                                    var possibleJson = reader.ReadToEnd();

                                    if (possibleJson == "true")
                                        return true;

                                    if (possibleJson == "false")
                                        return false;

                                    var json = JObjectFromString(possibleJson);

                                    //TODO: Maybe we should handle the error in here?
                                    //      And only return on the happy path??            
                                    return DeserializeErrorOr<bool>(json, j => j.Value<bool>());
                                }

                            }, cancellationToken, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }


        public Task<TResult> Delete<TResult>(string resource,
                                             DeezerPermissions requiredPermissions,
                                             CancellationToken cancellationToken,
                                             Func<JToken, TResult> resultFactory)
        {
            var authenticationException = AssertAuthenticated(requiredPermissions);
            if (authenticationException != null)
            {
                throw authenticationException;
            }

            string actualResource = AppendAccessTokenToResourceIfRequired(resource);

            return executor.ExecuteDelete(actualResource, cancellationToken)
                           .ContinueWith(t =>
                           {
                               t.ThrowIfFaulted();

                               var json = JObjectFromStream(t.Result);

                               //TODO: Maybe we should handle the error in here?
                               //      And only return on the happy path??            
                               return DeserializeErrorOr<TResult>(json, resultFactory);

                           }, cancellationToken, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

        }


        // Authentication
        public event AuthenticationStateChangedHandler OnAuthenticationStateChanged
        {
            add => this.authService.OnAuthenticationStateChanged += value;
            remove => this.authService.OnAuthenticationStateChanged -= value;
        }

        public Task<bool> Login(string accessToken, CancellationToken cancellationToken)
            => this.authService.Login(accessToken, cancellationToken);


        public Task<bool> Logout(CancellationToken cancellationToken)
            => this.authService.Logout(cancellationToken);


        public ulong CurrentUserId => this.authService.CurrentUser.Id; //TODO: Do we want to throw if not logged in??

        public bool IsAuthenticated => this.authService.IsAuthenticated;



        private JObject JObjectFromStream(Stream stream)
        {
            using (stream)
            using (var streamReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                return JObject.Load(jsonReader);
            }
        }

        private JObject JObjectFromString(string json)
        {
            using (var stringReader = new StringReader(json))
            using (var jsonReader = new JsonTextReader(stringReader))
            {
                return JObject.Load(jsonReader);
            }
        }

        private Exception AssertAuthenticated(DeezerPermissions requiredPermissions)
        {
            if (!this.IsAuthenticated)
            {
                return new Exception("Not authenticated");
            }

            if (!this.authService.HasPermission(requiredPermissions))
            {
                return new DeezerPermissionsException(requiredPermissions);
            }

            return null;
        }


        private string AppendAccessTokenToResourceIfRequired(string resource)
        {
            var accessTokenQuery = this.authService.GetAccessTokenQueryString();

            return resource.Contains(accessTokenQuery) ? resource
                                                       : $"{resource}&{accessTokenQuery}";
        }


        private TItem DeserializeErrorOr<TItem>(JObject json, Func<JObject, TItem> itemFactory)
        {
            var error = Error.FromJson(json);
            if (error != null)
            {
                //TODO: Translate this error into a DeezerException
                //      Will likely need a hook for the AuthService to 
                //      auto logout on authentication issues...
                throw new Exception("Something went wrong...");
            }

            //TODO: Handle Json Parsing issues
            //      Wont they just be thrown automagically??
            return itemFactory(json);
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
                this.executor.Dispose();
            }
        }
    }
}
