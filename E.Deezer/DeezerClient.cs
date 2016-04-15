using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

using RestSharp;

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

        internal CancellationToken CancellationToken { get { return iExecutor.CancellationToken; } }
        internal uint ResultSize { get { return iSession.ResultSize; } }

        internal string AccessToken { get { return iSession.AccessToken; } }
        internal bool IsAuthenticated { get { return iSession.Authenticated; } }
        internal IUser User { get { return iUser; } }

        //Another copy for those without params!
        internal Task<DeezerFragment<T>> Get<T>(string aMethod, uint aStart, uint aCount) {  return Get<T>(aMethod, RequestParameter.EmptyList, aStart, aCount); }
        internal Task<DeezerFragment<T>> Get<T>(string aMethod, IList<IRequestParameter> aParams) { return Get<T>(aMethod, aParams, uint.MaxValue, uint.MaxValue); }
        internal Task<DeezerFragment<T>> Get<T>(string aMethod, IList<IRequestParameter> aParams, uint aStart, uint aCount)
        {
            AddToParamList(aParams, aStart, aCount);

            return DoGet<DeezerFragment<T>>(aMethod, aParams);
        }
        internal Task<T> Get<T>(string aMethod)
        {
            return DoGet<DeezerObject<T>>(aMethod, RequestParameter.EmptyList).ContinueWith<T>((aTask) => aTask.Result.Data, iExecutor.CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }

        private Task<T> DoGet<T>(string aMethod, IEnumerable<IRequestParameter> aParams) where T : IHasError
        {
            return iExecutor.ExecuteGet<T>(aMethod, aParams).ContinueWith<T>((aTask) =>
            {
                CheckResponse<T>(aTask);
                return aTask.Result.Data;
            }, CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }

        //Performs a POST request
        internal Task<bool> Post(string aMethod, IList<IRequestParameter> aParams, DeezerPermissions aRequiredPermission)
        {
            if (!IsAuthenticated) { throw new NotLoggedInException(); }
            if (!HasPermission(aRequiredPermission)) { throw new DeezerPermissionsException(aRequiredPermission); }

            AddDefaultsToParamList(aParams);

            return iExecutor.ExecutePost<bool>(aMethod, aParams).ContinueWith<bool>((aTask) => aTask.Result.Data, CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }


        //'OAuth' Stuff

        //Grabs the user's permissions when the user Logs into the library.
        internal Task Login()
        {
            var loginTask = GetLoginTask();
            GetFetchUserTask().Wait();
            return loginTask;
        }

        private Task<IPermissions> GetLoginTask()
        {
            return DoGet<DeezerPermissionRequest>("user/me/permissions", GetLoginParams()).ContinueWith((aTask) => iPermissions = aTask.Result.Permissions, CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }
        private Task GetFetchUserTask()
        {
            return iExecutor.ExecuteGet<User>("user/me", GetLoginParams()).ContinueWith((aTask) =>
            {
                if (aTask.Result.ErrorException == null)
                {
                    aTask.Result.Data.Deserialize(this);
                    iUser = aTask.Result.Data;
                }
            }, CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }

        private IList<IRequestParameter> GetLoginParams()
        {
            IList<IRequestParameter> parms = RequestParameter.EmptyList;
            AddDefaultsToParamList(parms);
            return parms;
        }



        //Wrapper around permissions, matching to DeezerPermissions Enum
        internal bool HasPermission(DeezerPermissions aPermission)
        {
            if (IsAuthenticated && iPermissions != null)
            {
                return iPermissions.HasPermission(aPermission);
            }
            return false;
        }


        //Checks a response for errors and exceptions
        //TODO Refactor?
        private void CheckResponse<T>(Task<IRestResponse<T>> aResponse) where T : IHasError
        {
            //Is Task Faulty
            ThrowIFaulyTask<T>(aResponse);

            //Did the Deezer API call fail?
            if (aResponse.Result.Data != null)
            {
                ThrowIfDeezerExceptionAndLogout<T>(aResponse);
            }
        }

        private void ThrowIFaulyTask<T>(Task<IRestResponse<T>> aTask)
        {
            if(aTask.IsFaulted) {  throw aTask.Exception; }

            if (aTask.Result.ErrorException != null) {  throw aTask.Result.ErrorException; }
        }
        private void ThrowIfDeezerExceptionAndLogout<T>(Task<IRestResponse<T>> aTask) where T : IHasError
        {
            var r = aTask.Result.Data;
            if (r.TheError != null)
            {
                //If we've got an invalid code, we auto logout + clear internals
                if (r.TheError.Code == 300)
                {
                    iSession.Logout();
                    iPermissions = null;
                    iUser = null;
                }
                throw new DeezerException(r.TheError);
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


        private void AddDefaultsToParamList(IList<IRequestParameter> aParams) { AddToParamList(aParams, uint.MaxValue, uint.MaxValue);  }
        private void AddToParamList(IList<IRequestParameter> aParams, uint aStart, uint aCount)
        {
            if (aCount < uint.MaxValue && aStart < uint.MaxValue)
            {
                aParams.Add(RequestParameter.GetNewQueryStringParameter("index", aStart));
                aParams.Add(RequestParameter.GetNewQueryStringParameter("limit", aCount));
            }

            if (IsAuthenticated) { aParams.Add(RequestParameter.GetAccessTokenParamter(AccessToken)); }
        }


        public void Dispose() { iExecutor.Dispose();  }
    }
}
