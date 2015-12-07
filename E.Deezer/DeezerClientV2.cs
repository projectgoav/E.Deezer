﻿using System;
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
    internal class DeezerClientV2
    {
        private readonly RestClient iClient;
        private readonly IDeezerSession iSession;
        private readonly CancellationTokenSource iCancellationTokenSource;

        internal DeezerClientV2(IDeezerSession aSession) 
        { 
            iClient = new RestClient(DeezerSessionV2.ENDPOINT);
            iSession = aSession;
            iCancellationTokenSource = new CancellationTokenSource();
        }

        internal CancellationToken Token { get { return iCancellationTokenSource.Token; } }


        internal Task<DeezerFragmentV2<T>> Get<T>(string aMethod, Dictionary<string, string> aParams, uint aStart, uint aCount)
        {
            IRestRequest request = new RestRequest(aMethod, Method.GET);

            foreach(string key in aParams.Keys) { request.AddParameter(key, aParams[key], ParameterType.UrlSegment); }

            request.AddParameter("index", aStart, ParameterType.QueryString);
            request.AddParameter("limit", aCount);
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


    }
}