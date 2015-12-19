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
        private readonly RestClient iClient;
        private readonly DeezerSession iSession;
        private readonly CancellationTokenSource iCancellationTokenSource;
        private IPermissions iPermissions;
        private IUser iUser;

        internal DeezerClient(DeezerSession aSession) 
        { 
            iClient = new RestClient(DeezerSession.ENDPOINT);
            iClient.Timeout = 2500;

            iSession = aSession;
            iCancellationTokenSource = new CancellationTokenSource();
        }

        internal CancellationToken Token { get { return iCancellationTokenSource.Token; } }
        internal uint ResultSize { get { return iSession.ResultSize; } }

        internal string AccessToken { get { return iSession.AccessToken; } }
        internal bool IsAuthenticated { get { return iSession.Authenticated; } }
        internal IUser User { get { return iUser; } }

        //Another copy for those without params!
        internal Task<DeezerFragmentV2<T>> Get<T>(string aMethod, uint aStart, uint aCount) {  return Get<T>(aMethod, new string[] { "QRY", "access_token", AccessToken} , aStart, aCount); }

        //A nice wee copy of get, incase we want to limit users from picking the start/end points
        internal Task<DeezerFragmentV2<T>> Get<T>(string aMethod, string[] aParams) { return Get<T>(aMethod, aParams, uint.MaxValue, uint.MaxValue); }

        internal Task<DeezerFragmentV2<T>> Get<T>(string aMethod, string[] aParams, uint aStart, uint aCount)
        {
            IRestRequest request = new RestRequest(aMethod, Method.GET);

            for (int i = 0; i < aParams.Length; i+= 3)
            {
                switch(aParams[i])
                {
                    case "QRY": { request.AddParameter(aParams[i + 1], aParams[i + 2], ParameterType.QueryString); break; }
                    case "URL": { request.AddParameter(aParams[i + 1], aParams[i + 2], ParameterType.UrlSegment); break; }
                    default:    { request.AddParameter(aParams[i + 1], aParams[i + 2]); break;  }
                }           
            }

            if (aCount < uint.MaxValue && aStart < uint.MaxValue)
            {
                request.AddParameter("index", aStart, ParameterType.QueryString);
                request.AddParameter("limit", aCount, ParameterType.QueryString);
            }

            request.AddParameter("output", "json", ParameterType.QueryString);

            var task = iClient.ExecuteGetTaskAsync<DeezerFragmentV2<T>>(request, Token).ContinueWith<DeezerFragmentV2<T>>((aTask) =>
            {
                CheckResponse<DeezerFragmentV2<T>>(aTask);

                return aTask.Result.Data;
            },Token, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
            task.SuppressExceptions();
            return task;
        }

        //Copy of Get() for a single object result!
        public Task<T> Get<T>(string aMethod)
        {
            IRestRequest request = new RestRequest(aMethod, Method.GET);

            var task = iClient.ExecuteGetTaskAsync<DeezerObject<T>>(request, Token).ContinueWith<T>((aTask) =>
            {
                CheckResponse<DeezerObject<T>>(aTask);

                return aTask.Result.Data.Data;
            }, Token, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
            task.SuppressExceptions();
            return task;
        }




        //'OAuth' Stuff

        //Grabs the user's permissions when the user Logs into the library.
        internal Task Login()
        {
            IRestRequest request = new RestRequest("user/me/permissions", Method.GET);
            request.AddParameter("access_token", AccessToken, ParameterType.QueryString);

            var loginTask = iClient.ExecuteGetTaskAsync<DeezerPermissionRequest>(request, Token).ContinueWith((aTask) =>
            {
                CheckResponse<DeezerPermissionRequest>(aTask);

                iPermissions = aTask.Result.Data.Permissions;
            }, Token, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);

            //Create a local copy of the user so access by the User Endpoint
            Task.Factory.StartNew(() =>
            {
                IRestRequest uRequest = new RestRequest("user/me", Method.GET);
                uRequest.AddParameter("access_token", AccessToken, ParameterType.QueryString);

                iClient.ExecuteGetTaskAsync<DeezerObject<User>>(uRequest, Token).ContinueWith((aTask) =>
                {
                    try { CheckResponse<DeezerObject<User>>(aTask); }
                    catch { iUser = null; }
                    iUser = aTask.Result.Data.Data;
                }, Token, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
            }, Token);

            return loginTask;
        }

        //Wrapper around permissions, matching to DeezerPermissions Enum
        internal bool HasPermission(DeezerPermissions aPermission)
        {
            if (!IsAuthenticated)
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


        public void Dispose()
        {
            iCancellationTokenSource.Cancel();
            iCancellationTokenSource.Dispose();
        }
    }
}
