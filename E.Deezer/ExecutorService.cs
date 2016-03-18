using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

using RestSharp;

namespace E.Deezer
{
    internal class ExecutorService : IDisposable
    {
        private const int DEFAULT_TIMEOUT = 2500;

        private readonly RestClient iClient;
        private readonly CancellationTokenSource iCancellationTokenSource;

        internal ExecutorService()
        {
            iCancellationTokenSource = new CancellationTokenSource();

            iClient = new RestClient(DeezerSession.ENDPOINT);
            iClient.Timeout = DEFAULT_TIMEOUT;
        }

        internal CancellationToken CancellationToken { get { return iCancellationTokenSource.Token; } }


        public Task<IRestResponse<T>> ExecuteGet<T>(string aMethod, IEnumerable<IRequestParameter> aParams)
        {
            return Execute<T>(aMethod, Method.GET, aParams);
        }

        public Task<IRestResponse<T>> ExecutePost<T>(string aMethod, IEnumerable<IRequestParameter> aParams)
        {
            return Execute<T>(aMethod, Method.POST, aParams);
        }



        private Task<IRestResponse<T>> Execute<T>(string aMethodUrl, Method aMethodType, IEnumerable<IRequestParameter> aParams)
        {
            IRestRequest request = new RestRequest(aMethodUrl, aMethodType);
            AddParamsToRequest(request, aParams);

            var task = iClient.ExecuteTaskAsync<T>(request, CancellationToken);
            task.SuppressExceptions();
            return task;
        }

        private void AddParamsToRequest(IRestRequest aRequest, IEnumerable<IRequestParameter> aParams)
        {
            foreach(IRequestParameter param in aParams)
            {
                aRequest.AddParameter(param.Id, param.Value, param.Type);
            }

            //Always add the correct output format
            aRequest.AddParameter("output", "json", ParameterType.QueryString);
        }


        public void Dispose()
        {
            iCancellationTokenSource.Cancel();
            iCancellationTokenSource.Dispose();
        }
    }
}
