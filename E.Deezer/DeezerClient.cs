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
            this.authService = new AuthenticationService(this);

            this.Endpoints = new Endpoints.Endpoints(this);
        }

        public Endpoints.Endpoints Endpoints { get; }


        // GET

        public Task<TItem> Get<TItem>(string resource, DeezerPermissions requiredPermissions, CancellationToken cancellationToken, Func<JToken, TItem> itemFactory)
        {
            AssertAuthenticated(requiredPermissions);

            string actualResource = AppendAccessTokenToResourceIfRequired(resource);

            return Get<TItem>(actualResource, cancellationToken, itemFactory);
        }


        public Task<TItem> Get<TItem>(string resource, CancellationToken cancellationToken, Func<JToken, TItem> itemFactory)
        {
            return executor.ExecuteGet(resource, cancellationToken)
                           .ContinueWith(t =>
                           {
                               t.ThrowIfFaulted();

                               var json = JObjectFromStream(t.Result);
         
                               return DeserializeErrorOr<TItem>(json as JObject, itemFactory);

                           }, cancellationToken, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }

        // POST

        public Task<bool> Post(string resource,
                               DeezerPermissions requiredPermissions,
                               CancellationToken cancellationToken)
            => Post<bool>(resource,
                          requiredPermissions,
                          cancellationToken,
                          ParseBoolOrError);
       

        public Task<TResult> Post<TResult>(string resource,
                                           DeezerPermissions requiredPermissions,
                                           CancellationToken cancellationToken,
                                           Func<JToken, TResult> resultFactory)
            => Post<TResult>(resource,
                             requiredPermissions,
                             cancellationToken,
                             stream =>
                             {
                                 var json = JObjectFromStream(stream);
                                 return DeserializeErrorOr<TResult>(json, resultFactory);
                             });
        

        private Task<TResult> Post<TResult>(string resource,
                                            DeezerPermissions requiredPermissions,
                                            CancellationToken cancellationToken,
                                            Func<Stream, TResult> resultFactory)
        {
            AssertAuthenticated(requiredPermissions);

            string actualResource = AppendAccessTokenToResourceIfRequired(resource);

            return executor.ExecutePost(actualResource, cancellationToken)
                           .ContinueWith(t =>
                           {
                               t.ThrowIfFaulted();

                               return resultFactory(t.Result);

                           }, cancellationToken, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }


        // DELETE

        public Task<bool> Delete(string resource,
                       DeezerPermissions requiredPermissions,
                       CancellationToken cancellationToken)
            => Delete(resource,
                      requiredPermissions,
                      cancellationToken,
                      ParseBoolOrError);

        public Task<TResult> Delete<TResult>(string resource,
                                             DeezerPermissions requiredPermissions,
                                             CancellationToken cancellationToken,
                                             Func<JToken, TResult> resultFactory)
            => Delete<TResult>(resource,
                               requiredPermissions,
                               cancellationToken,
                               stream =>
                               {
                                   var json = JObjectFromStream(stream);
                                   return DeserializeErrorOr<TResult>(json, resultFactory);
                               });
        

        private Task<TResult> Delete<TResult>(string resource,
                                              DeezerPermissions requiredPermissions,
                                              CancellationToken cancellationToken,
                                              Func<Stream, TResult> resultFactory)
        {
            AssertAuthenticated(requiredPermissions);

            string actualResource = AppendAccessTokenToResourceIfRequired(resource);

            return executor.ExecuteDelete(actualResource, cancellationToken)
                           .ContinueWith(t =>
                           {
                               t.ThrowIfFaulted();

                               return resultFactory(t.Result);

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


        //TODO: Do we want to throw if not logged in??
        //TODO: What happens if authentication is inflight??
        public ulong CurrentUserId => this.authService.CurrentUser.Id; 

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


        private bool ParseBoolOrError(Stream stream)
        {
            using (stream)
            using (var streamReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                var token = JToken.Load(jsonReader);

                switch (token.Type)
                {
                    // Deezer sometimes just responds with 'true' or 'false'
                    // which Newtonsoft can't parse into a JObject. 
                    case JTokenType.Boolean:
                        return (bool)((token as JValue).Value);

                    case JTokenType.Object:
                        return DeserializeErrorOr<bool>(token as JObject, j => j.Value<bool>());

                    default:
                        throw new Exception("Unable to parse response");
                }
            }
        }

        private void AssertAuthenticated(DeezerPermissions requiredPermissions)
        {
            if (!this.IsAuthenticated)
            {
                throw new Exception("Not authenticated");
            }

            if (!this.authService.HasPermission(requiredPermissions))
            {
                throw new DeezerPermissionsException(requiredPermissions);
            }
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
                this.authService.LogoutIfAuthenticationError(error);
                throw new DeezerException(error);
            }

            // Any JSON parsing issues will be thrown here.
            // This causes any of the upstream tasks to enter the 
            // 'Failed' state chain.
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
                this.authService.Dispose();
                this.executor.Dispose();
            }
        }
    }
}
