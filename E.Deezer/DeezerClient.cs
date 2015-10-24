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
    public class DeezerClient
    {
        private readonly DeezerSession iSession;
        private readonly CancellationTokenSource iCancellationTokenSource;

        public DeezerClient(DeezerSession aSession)
        {
            iSession = aSession;
            iCancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Get Deezer service availablity
        /// </summary>
        /// <returns>Deezer service availiblity information. <see cref="E.Deezer.Api.IInfos"/>See IInfos</returns>
        public Task<IInfos> GetInfos()
        {
            IRestRequest request = new RestRequest("infos", Method.GET);
            return Execute<Infos>(request).ContinueWith<IInfos>((aTask) =>
            {
                if (aTask.Result != null)
                {
                    IInfos info = aTask.Result.Data;
                    return info;
                }
                else { return null; }
            });
        }


        
        private Task<IRestResponse> Execute(IRestRequest aRequest)
        {
            return iSession.Execute(aRequest, iCancellationTokenSource.Token);
        }

        private Task<IRestResponse<T>> Execute<T>(IRestRequest aRequest) 
        {  
            return iSession.Execute<T>(aRequest, iCancellationTokenSource.Token);
        }
    }
}
