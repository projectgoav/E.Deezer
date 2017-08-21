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
    /// <summary>
    /// Performs requests on the GitHub API.
    /// </summary>
    public class DeezerClient : IDisposable
    {
        private readonly DeezerSession iSession;
        private readonly ExecutorService iExecutor;

        private IUser iUser;
        private IPermissions iPermissions;

        internal DeezerClient(DeezerSession aSession, bool isUnderTest = false) 
        { 
            iSession = aSession;

            if (isUnderTest)
            {
                iExecutor = new ExecutorService("http://localhost:10024");
            }
            else { iExecutor = new ExecutorService(); }
        }

        internal IUser User                          => iUser;
        internal uint ResultSize                     => iSession.ResultSize;
        internal string AccessToken                  => iSession.AccessToken;
        internal bool IsAuthenticated                => iSession.Authenticated;
        internal CancellationToken CancellationToken => iExecutor.CancellationToken;


        //Another copy for those without params!
        internal Task<DeezerFragment<T>> Get<T>(string aMethod, uint aStart, uint aCount)           => Get<T>(aMethod, RequestParameter.EmptyList, aStart, aCount);

        internal Task<DeezerFragment<T>> Get<T>(string aMethod, IList<IRequestParameter> aParams)   => Get<T>(aMethod, aParams, uint.MinValue, uint.MaxValue);

        internal Task<DeezerFragment<T>> Get<T>(string aMethod, IList<IRequestParameter> aParams, uint aStart, uint aCount)
        {
            AddToParamList(aParams, aStart, aCount);
            return DoGet<DeezerFragment<T>>(aMethod, aParams);
        }


        internal Task<T> Get<T>(string aMethod)
        {
            return DoGet<DeezerObject<T>>(aMethod, RequestParameter.EmptyList)
                        .ContinueWith<T>((aTask) => aTask.Result.Data, CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }


        internal Task<DeezerChartFragment> GetChart(string aMethod, uint aStart, uint aCount)           => GetChart(aMethod, RequestParameter.EmptyList, aStart, aCount);

        internal Task<DeezerChartFragment> GetChart(string aMethod, IList<IRequestParameter> aParams)   => GetChart(aMethod, aParams, uint.MinValue, uint.MaxValue);

        internal Task<DeezerChartFragment> GetChart(string aMethod, IList<IRequestParameter> aParams, uint aStart, uint aCount)
        {
            AddToParamList(aParams, aStart, aCount);
            return DoGet<DeezerChartFragment>(aMethod, aParams);
        }


        internal Task<T> GetPlain<T>(string aMethod)
        {
            return iExecutor.ExecuteGet(aMethod, RequestParameter.EmptyList)
                            .ContinueWith((aTask) =>
                            {
                                CheckHttpResponse(aTask);
                                T deserialized = DeserializeResponse<T>(aTask.Result.Content).Result;
                                return deserialized;
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

        //Performs a POST request
        internal Task<bool> Post(string aMethod, IList<IRequestParameter> aParams, DeezerPermissions aRequiredPermission)
        {
            CheckAuthentication();
            CheckPermissions(aRequiredPermission);

            AddDefaultsToParamList(aParams);

            return iExecutor.ExecutePost(aMethod, aParams)
                            .ContinueWith<bool>((aTask) => aTask.Result.IsSuccessStatusCode, CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }

        internal Task<T> Post<T>(string aMethod, IList<IRequestParameter> aParams, DeezerPermissions aRequiredPermission)
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
        internal Task<bool> Delete(string aMethod, IList<IRequestParameter> aParams, DeezerPermissions aRequiredPermission)
        {
            CheckAuthentication();
            CheckPermissions(aRequiredPermission);

            AddDefaultsToParamList(aParams);

            return iExecutor.ExecuteDelete(aMethod, aParams)
                            .ContinueWith((aTask) => aTask.Result.IsSuccessStatusCode, CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
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

        internal bool HasPermission(DeezerPermissions aRequiredPermissions)
        {
            if(IsAuthenticated && iPermissions != null)
            {
                return iPermissions.HasPermission(aRequiredPermissions);
            }

            return false;
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



        //Performs a transform from Deezer Fragment to IEnumerable.
        internal IEnumerable<TDest> Transform<TSource, TDest>(DeezerFragment<TSource> aFragment) where TSource : TDest, IDeserializable<DeezerClient>
        {
            List<TDest> items = new List<TDest>();

            foreach(var item in aFragment.Items)
            {
                item.Deserialize(this);
                items.Add(item);
            }

            return items;
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
