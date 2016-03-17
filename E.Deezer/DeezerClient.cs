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

        internal DeezerClient(DeezerSession aSession) 
        { 
            iSession = aSession;

            iExecutor = new ExecutorService();
        }

        internal CancellationToken CancellationToken { get { return iExecutor.CancellationToken; } }
        internal uint ResultSize { get { return iSession.ResultSize; } }

        internal string AccessToken { get { return iSession.AccessToken; } }
        internal bool IsAuthenticated { get { return iSession.Authenticated; } }
        internal IUser User { get { return iUser; } }

        //Another copy for those without params!
        internal Task<DeezerFragmentV2<T>> Get<T>(string aMethod, uint aStart, uint aCount) {  return Get<T>(aMethod, RequestParameter.EmptyList, aStart, aCount); }
        internal Task<DeezerFragmentV2<T>> Get<T>(string aMethod, IList<IRequestParameter> aParams) { return Get<T>(aMethod, aParams, uint.MaxValue, uint.MaxValue); }
        internal Task<DeezerFragmentV2<T>> Get<T>(string aMethod, IList<IRequestParameter> aParams, uint aStart, uint aCount)
        {
            AppendToParameterList(aParams, aStart, aCount);

            return DoGet<DeezerFragmentV2<T>>(aMethod, aParams);
        }
        internal Task<T> Get<T>(string aMethod)
        {
            return DoGet<DeezerObject<T>>(aMethod, RequestParameter.EmptyList).ContinueWith<T>((aTask) => aTask.Result.Data, iExecutor.CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }

        private Task<T> DoGet<T>(string aMethod, IEnumerable<IRequestParameter> aParams) where T : IHasError
        {
            var task = iExecutor.ExecuteGet<T>(aMethod, aParams).ContinueWith<T>((aTask) =>
            {
                CheckResponse<T>(aTask);

                return aTask.Result.Data;
            }, CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
            task.SuppressExceptions();
            return task;
        }

        //Performs a POST request
        internal Task<bool> Post(string aMethod, IList<IRequestParameter> aParams, DeezerPermissions aRequiredPermission)
        {
            if (!IsAuthenticated) { throw new NotLoggedInException(); }
            if (!HasPermission(aRequiredPermission)) { throw new DeezerPermissionsException(aRequiredPermission); }

            AppendDefaultToParamterList(aParams);

            return iExecutor.ExecutePost<bool>(aMethod, aParams).ContinueWith<bool>((aTask) => aTask.Result.Data, CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }


        //'OAuth' Stuff

        //Grabs the user's permissions when the user Logs into the library.
        internal Task Login()
        {
            IList<IRequestParameter> parms = RequestParameter.EmptyList;
            AppendDefaultToParamterList(parms);

            var loginTask = DoGet<DeezerPermissionRequest>("user/me/permissions", parms).ContinueWith((aTask) => iPermissions = aTask.Result.Permissions, CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);

            //Create a local copy of the user so access by the User Endpoint
            var usrTask = Task.Factory.StartNew(() =>
            {
                iExecutor.ExecuteGet<User>("user/me", parms).ContinueWith((aTask) =>
                {
                    if (aTask.Result.ErrorException == null)
                    {
                        aTask.Result.Data.Deserialize(this);
                        iUser = aTask.Result.Data;        
                    }
                }, CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
            }, CancellationToken);

            usrTask.Wait();
            return loginTask;
        }

        //Wrapper around permissions, matching to DeezerPermissions Enum
        internal bool HasPermission(DeezerPermissions aPermission)
        {
            if (IsAuthenticated)
            {
                if (iPermissions != null)
                {
                    switch (aPermission)
                    {
                        case DeezerPermissions.BasicAccess: { return iPermissions.BasicAccess; }
                        case DeezerPermissions.DeleteLibrary: { return iPermissions.DeleteLibrary; }
                        case DeezerPermissions.Email: { return iPermissions.Email; }
                        case DeezerPermissions.ListeningHistory: { return iPermissions.ListeningHistory; }
                        case DeezerPermissions.ManageCommunity: { return iPermissions.ManageCommunity; }
                        case DeezerPermissions.ManageLibrary: { return iPermissions.ManageLibrary; }
                        case DeezerPermissions.OfflineAccess: { return iPermissions.OfflineAccess; }
                        default: { return false; }
                    }
                }
            }
            return false;
        }


        //Checks a response for errors and exceptions
        private void CheckResponse<T>(Task<IRestResponse<T>> aResponse) where T : IHasError
        {
            //Is faulty?
            if (aResponse.IsFaulted)
            {
                throw aResponse.Exception;
            }
            else if (aResponse.Result.ErrorException != null)
            {
                if (aResponse.Result.ErrorException != null) { throw aResponse.Result.ErrorException; }
            }
            else
            {
                //Did the Deezer API call fail?
                if (aResponse.Result.Data != null)
                {
                    var r = aResponse.Result.Data;
                    if (r.TheError != null) 
                    { 
                        //If we've got an invalid code, we auto logout + clear internals
                        if(r.TheError.Code == 300) 
                        { 
                            iSession.Logout();
                            iPermissions = null;
                            iUser = null;
                        }
                        throw new DeezerException(r.TheError); 
                    }
                }
            }
        }


        //Performs a transform from Deezer Fragment to IEnumerable.
        internal IEnumerable<TDest> Transform<TSource, TDest>(DeezerFragmentV2<TSource> aFragment) where TSource : TDest, IDeserializable<DeezerClient>
        {
            List<TDest> items = new List<TDest>();

            foreach(var item in aFragment.Items)
            {
                item.Deserialize(this);
                items.Add(item);
            }

            return items;
        }


        private void AppendDefaultToParamterList(IList<IRequestParameter> aParams) { AppendToParameterList(aParams, uint.MaxValue, uint.MaxValue);  }
        private void AppendToParameterList(IList<IRequestParameter> aParams, uint aStart, uint aCount)
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
