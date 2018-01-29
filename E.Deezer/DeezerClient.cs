using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.Net.Http;

using E.Deezer.Api;

namespace E.Deezer
{
    internal interface IDeezerClient
    {
        IUser User { get; }

        bool IsAuthenticated { get; }

        CancellationToken CancellationToken { get; }
        

        //GET
        Task<DeezerFragment<T>> Get<T>(string aMethod, uint aStart, uint aCount);

        Task<DeezerFragment<T>> Get<T>(string aMethod, IList<IRequestParameter> aParams);

        Task<DeezerFragment<T>> Get<T>(string aMethod, IList<IRequestParameter> aParams, uint aStart, uint aCount);

        Task<T> Get<T>(string aMethod);

        Task<IChart> GetChart(ulong aId, uint aStart, uint aCount);

        Task<T> GetPlain<T>(string aMethod);


        //POST
        Task<bool> Post(string aMethod, IList<IRequestParameter> aParams, DeezerPermissions aRequiredPermission);

        Task<T> Post<T>(string aMethod, IList<IRequestParameter> aParams, DeezerPermissions aRequiredPermission);


        //DELETE
        Task<bool> Delete(string aMethod, IList<IRequestParameter> aParams, DeezerPermissions aRequiredPermission);


        //Helpers
        IEnumerable<TDest> Transform<TSource, TDest>(DeezerFragment<TSource> aFragment) where TSource : TDest, IDeserializable<IDeezerClient>;

        bool HasPermission(DeezerPermissions aRequiredPermissions);
    }
    
    
    internal class DeezerClient : IDeezerClient, IDisposable
    {
        private readonly DeezerSession iSession;
        private readonly ExecutorService iExecutor;

        private IUser iUser;
        private IPermissions iPermissions;

        internal DeezerClient(DeezerSession aSession, HttpMessageHandler httpMessageHandler = null, bool isUnderTest = false) 
        { 
            iSession = aSession;

            if (isUnderTest)
            {
                iExecutor = new ExecutorService("http://localhost:10024", httpMessageHandler);
            }
            else { iExecutor = new ExecutorService(httpMessageHandler); }
        }


        public IUser User => iUser;

        public bool IsAuthenticated => iSession.Authenticated;

        public CancellationToken CancellationToken => iExecutor.CancellationToken;


        internal string AccessToken => iSession.AccessToken;


        //Another copy for those without params!
        public Task<DeezerFragment<T>> Get<T>(string aMethod, uint aStart, uint aCount)           
            => Get<T>(aMethod, RequestParameter.EmptyList, aStart, aCount);

        public Task<DeezerFragment<T>> Get<T>(string aMethod, IList<IRequestParameter> aParams)   
            => Get<T>(aMethod, aParams, uint.MinValue, uint.MaxValue);

        public Task<DeezerFragment<T>> Get<T>(string aMethod, IList<IRequestParameter> aParams, uint aStart, uint aCount)
        {
            AddToParamList(aParams, aStart, aCount);
            return DoGet<DeezerFragment<T>>(aMethod, aParams);
        }

        public Task<T> Get<T>(string aMethod)
        {
            return DoGet<DeezerObject<T>>(aMethod, RequestParameter.EmptyList)
                        .ContinueWith<T>((aTask) => aTask.Result.Data, CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }


        public Task<IChart> GetChart(ulong aId, uint aStart, uint aCount)
        {
            string method = "chart/{id}";
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", aId),
            };

            return GetChart(method, parms, aStart, aCount);

        }

        public Task<T> GetPlain<T>(string aMethod)
        {
            return iExecutor.ExecuteGet(aMethod, RequestParameter.EmptyList)
                            .ContinueWith((aTask) =>
                            {
                                CheckHttpResponse(aTask);
                                T deserialized = DeserializeResponse<T>(aTask.Result.Content).Result;
                                return deserialized;
                            }, CancellationToken, TaskContinuationOptions.NotOnFaulted, TaskScheduler.Default);
        }

        //Performs a POST request
        public Task<bool> Post(string aMethod, IList<IRequestParameter> aParams, DeezerPermissions aRequiredPermission)
        {
            CheckAuthentication();
            CheckPermissions(aRequiredPermission);

            AddDefaultsToParamList(aParams);

            return iExecutor.ExecutePost(aMethod, aParams)
                            .ContinueWith<bool>((aTask) => aTask.Result.IsSuccessStatusCode, CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }

        public Task<T> Post<T>(string aMethod, IList<IRequestParameter> aParams, DeezerPermissions aRequiredPermission)
        {
            CheckAuthentication();
            CheckPermissions(aRequiredPermission);

            AddDefaultsToParamList(aParams);

            return iExecutor.ExecutePost(aMethod, aParams)
                            .ContinueWith<T>((aTask) =>
                            {
                                CheckHttpResponse(aTask);
                                T deserialized = DeserializeResponse<T>(aTask.Result.Content).Result;
                                return deserialized;
                            }, CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }

        //Performs a DELETE request
        public Task<bool> Delete(string aMethod, IList<IRequestParameter> aParams, DeezerPermissions aRequiredPermission)
        {
            CheckAuthentication();
            CheckPermissions(aRequiredPermission);

            AddDefaultsToParamList(aParams);

            return iExecutor.ExecuteDelete(aMethod, aParams)
                            .ContinueWith((aTask) => aTask.Result.IsSuccessStatusCode, CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }


        //Performs a transform from Deezer Fragment to IEnumerable.
        public IEnumerable<TDest> Transform<TSource, TDest>(DeezerFragment<TSource> aFragment) where TSource : TDest, IDeserializable<IDeezerClient>
        {
            List<TDest> items = new List<TDest>();

            foreach (var item in aFragment.Items)
            {
                item.Deserialize(this);
                items.Add(item);
            }

            return items;
        }


        public bool HasPermission(DeezerPermissions aRequiredPermissions)
        {
            if (IsAuthenticated && iPermissions != null)
            {
                return iPermissions.HasPermission(aRequiredPermissions);
            }

            return false;
        }



        //'OAuth' Stuff

        //Grabs the user's permissions when the user Logs into the library.
        internal Task Login()
        {
            IList<IRequestParameter> parms = RequestParameter.EmptyList;
            AddDefaultsToParamList(parms);

            return GetPlain<User>("user/me")
                    .ContinueWith((aTask) =>
                    {
                        iUser = aTask.Result;

                        IList<IRequestParameter> permissionParams = RequestParameter.EmptyList;
                        AddDefaultsToParamList(permissionParams);

                        DoGet<DeezerPermissionRequest>("user/me/permissions", permissionParams)
                                .ContinueWith((aPermissionTask) => iPermissions = aPermissionTask.Result.Permissions, CancellationToken, TaskContinuationOptions.NotOnFaulted, TaskScheduler.Default)
                                .Wait();

                    }, CancellationToken, TaskContinuationOptions.NotOnFaulted, TaskScheduler.Default);
        }


        private Task<T> DoGet<T>(string aMethod, IEnumerable<IRequestParameter> aParams) where T : IHasError
        {
            return iExecutor.ExecuteGet(aMethod, aParams)
                            .ContinueWith((aTask) =>
                            {
                                CheckHttpResponse(aTask);
                                T deserialized = DeserializeResponse<T>(aTask.Result.Content).Result;
                                CheckForDeezerError(deserialized);
                                return deserialized;
                            }, CancellationToken, TaskContinuationOptions.NotOnFaulted, TaskScheduler.Default);
        }


        private Task<IChart> GetChart(string aMethod, IList<IRequestParameter> aParams, uint aStart, uint aCount)
        {
            AddToParamList(aParams, aStart, aCount);
            return DoGet<DeezerChartFragment>(aMethod, aParams)
                    .ContinueWith((aTask) =>
                    {
                        Chart chart = new Chart(aTask.Result.Albums.Items,
                                aTask.Result.Artists.Items,
                                aTask.Result.Tracks.Items,
                                aTask.Result.Playlists.Items);

                        chart.Deserialize(this);
                        return chart as IChart;
                    }, CancellationToken, TaskContinuationOptions.NotOnFaulted, TaskScheduler.Default);
        }

        //Checks a response for errors and exceptions
        private void CheckHttpResponse(Task<HttpResponseMessage> aResponse)
        {
            //Is Task Faulty
            if(aResponse.IsFaulted)
            {
                throw aResponse.Exception;
            }

            if(!aResponse.Result.IsSuccessStatusCode)
            {
                string msg = $"Status: {aResponse.Result.StatusCode} :: {aResponse.Result.ReasonPhrase}";
                throw new HttpRequestException(msg);
            }
        }

        private async Task<T> DeserializeResponse<T>(HttpContent aContent)
        {
            string json = await aContent.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        private void CheckForDeezerError<T>(T aObject) where T : IHasError
        {
            if(aObject == null)
            {
                throw new InvalidOperationException("JSON response failed to be parsed into suitable object.");
            }

            //Make sure our API call didn't fail...
            if(aObject.TheError != null)
            {
                if(aObject.TheError.Code == 300)
                {
                    //We've got an invalid/expired auth code -> auto logout + clear internals
                    iSession.Logout();
                    iPermissions = null;
                    iUser = null;
                }

                throw new DeezerException(aObject.TheError);

            }
        }


        private void CheckAuthentication()
        {
            if (!IsAuthenticated)
            {
                throw new NotLoggedInException();
            }
        }

        private void CheckPermissions(DeezerPermissions aRequiredPermissions)
        {
            if(iPermissions == null)
            {
                throw new NotLoggedInException();
            }

            if(!HasPermission(aRequiredPermissions))
            {
                throw new DeezerPermissionsException(aRequiredPermissions);
            }
        }


        private void AddDefaultsToParamList(IList<IRequestParameter> aParams) => AddToParamList(aParams, uint.MinValue, uint.MaxValue);

        private void AddToParamList(IList<IRequestParameter> aParams, uint aStart, uint aCount)
        {
            if (aCount <= uint.MaxValue && aStart <= uint.MaxValue)
            {
                aParams.Add(RequestParameter.GetNewQueryStringParameter("index", aStart));
                aParams.Add(RequestParameter.GetNewQueryStringParameter("limit", aCount));
            }

            if (IsAuthenticated) { aParams.Add(RequestParameter.GetAccessTokenParamter(AccessToken)); }
        }


        public void Dispose()
        {
            iExecutor.Dispose();
        }
    }
}
