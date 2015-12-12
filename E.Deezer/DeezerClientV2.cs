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
        private readonly IDeezerSession iSession;
        private readonly CancellationTokenSource iCancellationTokenSource;

        internal DeezerClient(IDeezerSession aSession) 
        { 
            iClient = new RestClient(DeezerSession.ENDPOINT);
            iClient.Timeout = 2500;

            iSession = aSession;
            iCancellationTokenSource = new CancellationTokenSource();
        }

        internal CancellationToken Token { get { return iCancellationTokenSource.Token; } }


        //A nice wee copy of get, incase we want to limit users from picking the start/end points
        internal Task<DeezerFragmentV2<T>> Get<T>(string aMethod, string[] aParams) { return Get<T>(aMethod, aParams, uint.MaxValue, uint.MaxValue); }

        internal Task<DeezerFragmentV2<T>> Get<T>(string aMethod, string[] aParams, uint aStart, uint aCount)
        {
            IRestRequest request = new RestRequest(aMethod, Method.GET);

            for (int i = 0; i < aParams.Length; i+= 3)
            {
                switch(aParams[i])
                {
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
                //Is faulty?
                if(aTask.IsFaulted)
                {
                    if (aTask.Result.ErrorException != null) { throw aTask.Result.ErrorException; }
                    else { throw new Exception("The specified request did not complete successfully."); }       //TODO - wording
                }
                else
                {
                    //Did the Deezer API call fail?
                    if(aTask.Result.Data != null)
                    {
                        var r = aTask.Result.Data;
                        if (r.Error != null) { throw new DeezerException(r.Error); }
                    }
                }

                return aTask.Result.Data;
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
            task.SuppressExceptions();
            return task;
        }

        //Copy fo Get() for a single object result!
        internal Task<T> Get<T>(string aMethod)
        {
            IRestRequest request = new RestRequest(aMethod, Method.GET);

            var task = iClient.ExecuteGetTaskAsync<DeezerObject<T>>(request, Token).ContinueWith<T>((aTask) =>
            {
                //Is faulty?
                if (aTask.IsFaulted)
                {
                    if (aTask.Result.ErrorException != null) { throw aTask.Result.ErrorException; }
                    else { throw new Exception("The specified request did not complete successfully."); }       //TODO - wording
                }
                else
                {
                    //Did the Deezer API call fail?
                    if (aTask.Result.Data != null)
                    {
                        var r = aTask.Result.Data;
                        if (r.Error != null) { throw new DeezerException(r.Error); }
                    }
                }

                return aTask.Result.Data.Data;
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
            task.SuppressExceptions();
            return task;
        }

        public void Dispose()
        {
            iCancellationTokenSource.Cancel();
            iCancellationTokenSource.Dispose();
        }
    }
}
