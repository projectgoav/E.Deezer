using E.Deezer.Api;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace E.Deezer
{
    internal interface IDeezerClient
    {
        IUser User { get; }

        bool IsAuthenticated { get; }

        CancellationToken CancellationToken { get; }
        
        Task<DeezerFragment<T>> Get<T>(string aMethod, uint aStart, uint aCount);

        Task<DeezerFragment<T>> Get<T>(string aMethod, IList<IRequestParameter> aParams);

        Task<DeezerFragment<T>> Get<T>(string aMethod, IList<IRequestParameter> aParams, uint aStart, uint aCount);

        Task<IChart> GetChart(long aId, uint aStart, uint aCount);

        Task<T> GetPlain<T>(string aMethod, IList<IRequestParameter> aParams);
        Task<T> GetPlainWithError<T>(string aMethod, IList<IRequestParameter> aParams) where T : IHasError;
        
        Task<bool> Post(string aMethod, IList<IRequestParameter> aParams, DeezerPermissions aRequiredPermission);

        Task<T> Post<T>(string aMethod, IList<IRequestParameter> aParams, DeezerPermissions aRequiredPermission);
        
        Task<bool> Delete(string aMethod, IList<IRequestParameter> aParams, DeezerPermissions aRequiredPermission);

        //Helpers
        IEnumerable<TDest> Transform<TSource, TDest>(DeezerFragment<TSource> aFragment) where TSource : TDest, IDeserializable<IDeezerClient>;

        bool HasPermission(DeezerPermissions aRequiredPermissions);
    }

    internal class DeezerClient : IDeezerClient, IDisposable
    {
        private readonly DeezerSession _session;
        private readonly ExecutorService _executor;
        private readonly ExceptionFactory _exceptionFactory;

        private IPermissions _permissions;

        internal DeezerClient(DeezerSession session, HttpMessageHandler httpMessageHandler = null)
        {
            _session = session;
            _executor = new ExecutorService(httpMessageHandler);
            _exceptionFactory = new ExceptionFactory();
        }

        public IUser User { get; private set; }

        public bool IsAuthenticated => _session.Authenticated;

        public CancellationToken CancellationToken => _executor.CancellationToken;

        internal string AccessToken => _session.AccessToken;

        // TODO: Another copy for those without params!
        public Task<DeezerFragment<T>> Get<T>(string aMethod, uint aStart, uint aCount)
            => Get<T>(aMethod, RequestParameter.EmptyList, aStart, aCount);

        public Task<DeezerFragment<T>> Get<T>(string aMethod, IList<IRequestParameter> aParams)
            => Get<T>(aMethod, aParams, uint.MinValue, uint.MaxValue);

        public Task<DeezerFragment<T>> Get<T>(string aMethod, IList<IRequestParameter> aParams, uint aStart, uint aCount)
        {
            AddToParamList(aParams, aStart, aCount);
            return DoGet<DeezerFragment<T>>(aMethod, aParams);
        }

        public Task<IChart> GetChart(long aId, uint aStart, uint aCount)
        {
            string method = "chart/{id}";
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", aId),
            };

            return GetChart(method, parms, aStart, aCount);
        }

        public async Task<T> GetPlainWithError<T>(string method, IList<IRequestParameter> parms = null) where T : IHasError
        {
            if (parms == null)
            {
                parms = RequestParameter.EmptyList;
            }

            return await GetPlain<T>(method, parms).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a GET request response as a T object.
        /// </summary>
        /// <typeparam name="T">Server response type.</typeparam>
        /// <param name="method">Additional parts after the base URL.</param>
        /// <param name="parms">Additional parameters for the URL.</param>
        /// <exception cref="HttpRequestException">Occurs
        /// when the response was not HTTP 200 or when the response
        /// content is null.</exception>
        /// <exception cref="InvalidOperationException">Error with one
        /// or more URL segment(s).</exception>
        /// <exception cref="DeezerException">Occurs when
        /// the API response is not data but an Exception.</exception>
        public async Task<T> GetPlain<T>(string method, IList<IRequestParameter> parms = null)
        {
            if (parms == null)
            {
                parms = RequestParameter.EmptyList;
            }

            string response = await _executor.GetAsync(method, parms).ConfigureAwait(false);
            _exceptionFactory.ThrowIfNeeded(response);
            return await Deserialize<T>(response).ConfigureAwait(false);
        }

        public async Task<bool> Post(string method, IList<IRequestParameter> parms, DeezerPermissions requiredPermissions)
        {
            CheckAuthentication();
            CheckPermissions(requiredPermissions);

            AddDefaultsToParamList(parms);

            string response = await _executor.PostAsync(method, parms).ConfigureAwait(false);
            _exceptionFactory.ThrowIfNeeded(response);
            return await Deserialize<bool>(response).ConfigureAwait(false);
        }

        public async Task<T> Post<T>(string method, IList<IRequestParameter> parms, DeezerPermissions requiredPermissions)
        {
            CheckAuthentication();
            CheckPermissions(requiredPermissions);

            AddDefaultsToParamList(parms);

            string response = await _executor.PostAsync(method, parms).ConfigureAwait(false);
            _exceptionFactory.ThrowIfNeeded(response);
            return await Deserialize<T>(response).ConfigureAwait(false);
        }

        public async Task<bool> Delete(string method, IList<IRequestParameter> parms, DeezerPermissions aRequiredPermission)
        {
            CheckAuthentication();
            CheckPermissions(aRequiredPermission);

            AddDefaultsToParamList(parms);

            string response = await _executor.DeleteAsync(method, parms).ConfigureAwait(false);
            _exceptionFactory.ThrowIfNeeded(response);
            return await Deserialize<bool>(response).ConfigureAwait(false);
        }

        //Performs a transform from Deezer Fragment to IEnumerable.
        public IEnumerable<TDest> Transform<TSource, TDest>(DeezerFragment<TSource> fragment) where TSource : TDest, IDeserializable<IDeezerClient>
            => fragment.Items.Select<TSource, TDest>(x =>
                {
                    x.Deserialize(this);
                    return x;
                })
                .ToList();

        public bool HasPermission(DeezerPermissions requiredPermissions)
        {
            if (IsAuthenticated && _permissions != null)
            {
                return _permissions.HasPermission(requiredPermissions);
            }

            return false;
        }

        //'OAuth' Stuff

        //Grabs the user's permissions when the user Logs into the library.
        internal Task Login()
        {
            IList<IRequestParameter> parms = RequestParameter.EmptyList;
            AddDefaultsToParamList(parms);

            return GetPlainWithError<User>("user/me", parms)
                    .ContinueWith((aTask) =>
                    {
                        var userInstance = aTask.Result;
                        userInstance.Deserialize(this);

                        User = userInstance;

                        IList<IRequestParameter> permissionParams = RequestParameter.EmptyList;
                        AddDefaultsToParamList(permissionParams);

                        DoGet<DeezerPermissionRequest>("user/me/permissions", permissionParams)
                                .ContinueWith((aPermissionTask) => _permissions = aPermissionTask.Result.Permissions, CancellationToken, TaskContinuationOptions.NotOnFaulted, TaskScheduler.Default)
                                .Wait();

                    }, CancellationToken, TaskContinuationOptions.NotOnFaulted, TaskScheduler.Default);
        }

        private async Task<T> DoGet<T>(string method, IList<IRequestParameter> parm) where T : IHasError
        {
            string response = await _executor.GetAsync(method, parm).ConfigureAwait(false);
            _exceptionFactory.ThrowIfNeeded(response);
            return await Deserialize<T>(response)
                                .ContinueWith(t =>
                                {
                                    if (t.IsFaulted)
                                    {
                                        throw t.Exception.GetBaseException();
                                    }

                                    return t.Result;

                                }, CancellationToken, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }

        private Task<IChart> GetChart(string method, IList<IRequestParameter> parm, uint start, uint count)
        {
            AddToParamList(parm, start, count);

            return DoGet<DeezerChartFragment>(method, parm)
                        .ContinueWith<IChart>(t =>
                        {
                            if (t.IsFaulted)
                            {
                                throw t.Exception.GetBaseException();
                            }

                            Chart chart = new Chart(t.Result.Albums.Items,
                                                    t.Result.Artists.Items,
                                                    t.Result.Tracks.Items,
                                                    t.Result.Playlists.Items);

                            chart.Deserialize(this);
                            return chart;

                        }, CancellationToken, TaskContinuationOptions.NotOnFaulted, TaskScheduler.Default);
        }

        private async Task<T> Deserialize<T>(string json)
        {
            return await Task.FromResult(JsonConvert.DeserializeObject<T>(json));
        }

        private void CheckForDeezerError<T>(T deezerObject) where T : IHasError
        {
            if (deezerObject == null)
            {
                throw new InvalidOperationException("JSON response failed to be parsed into suitable object.");
            }

            //Make sure our API call didn't fail...
            if (deezerObject.TheError != null)
            {
                ThrowDeezerError(deezerObject.TheError);
            }
        }

        private void ThrowDeezerError(IError error)
        {
            if (error.Code == 200          //200 == Logout fail
                || error.Code == 300)      //300 == Authentication error
            {
                //We've got an invalid/expired auth code -> auto logout + clear internals
                _session.Logout();
                _permissions = null;
                User = null;
            }

            throw new DeezerException(error);
        }

        private void CheckAuthentication()
        {
            if (!IsAuthenticated)
            {
                throw new NotLoggedInException();
            }
        }

        /// <summary>
        /// Runs a check to determinate the current User has
        /// the required permission(s).
        /// </summary>
        /// <param name="requiredPermissions">User should own these.</param>
        /// <exception cref="NotLoggedInException" />
        /// <exception cref="DeezerPermissionsException">Occurs
        /// when the current User does not have the required permission(s)
        /// to execute the request.</exception>
        private void CheckPermissions(DeezerPermissions requiredPermissions)
        {
            if (_permissions == null)
            {
                throw new NotLoggedInException();
            }

            if (!HasPermission(requiredPermissions))
            {
                throw new DeezerPermissionsException(requiredPermissions);
            }
        }

        private void AddDefaultsToParamList(IList<IRequestParameter> parms)
            => AddToParamList(parms, uint.MinValue, uint.MaxValue);

        private void AddToParamList(IList<IRequestParameter> parms, uint start, uint count)
        {
            if (count <= uint.MaxValue && start <= uint.MaxValue)
            {
                parms.Add(RequestParameter.GetNewQueryStringParameter("index", start));
                parms.Add(RequestParameter.GetNewQueryStringParameter("limit", count));
            }

            if (IsAuthenticated)
            {
                parms.Add(RequestParameter.GetAccessTokenParamter(AccessToken));
            }
        }

        public void Dispose()
        {
            _executor.Dispose();
        }
    }
}
