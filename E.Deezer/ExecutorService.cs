using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

using System.Net.Http;

namespace E.Deezer
{
    internal class ExecutorService : IDisposable
    {
        private const int DEFAULT_TIMEOUT = 30000; //30secs

        private readonly HttpClient iClient;
        private readonly CancellationTokenSource iCancellationTokenSource;

        internal ExecutorService(HttpMessageHandler httpMessageHandler = null)
            : this(DeezerSession.ENDPOINT, httpMessageHandler)
        { }

        internal ExecutorService(string testUrl, HttpMessageHandler httpMessageHandler = null)
        {
            iCancellationTokenSource = new CancellationTokenSource();

            if(httpMessageHandler != null)
            {
                iClient = new HttpClient(httpMessageHandler);
            }
            else
            {
                iClient = new HttpClient();
            }

            iClient.BaseAddress = new Uri(DeezerSession.ENDPOINT);
            iClient.Timeout = TimeSpan.FromMilliseconds(DEFAULT_TIMEOUT);
        }

        internal CancellationToken CancellationToken { get { return iCancellationTokenSource.Token; } }


        public Task<HttpResponseMessage> ExecuteGet(string aMethod, IEnumerable<IRequestParameter> aParams)
        {
            return iClient.GetAsync(BuildUrl(aMethod, aParams), CancellationToken);
        }

    
        public Task<HttpResponseMessage> ExecutePost(string aMethod, IEnumerable<IRequestParameter> aParams)
        {
            return iClient.PostAsync(BuildUrl(aMethod, aParams), null, CancellationToken);
        }

        public Task<HttpResponseMessage> ExecuteDelete(string aMethod, IEnumerable<IRequestParameter> aParams)
        {
            return iClient.DeleteAsync(BuildUrl(aMethod, aParams), CancellationToken);
        }


        internal string BuildUrl(string aUrl, IEnumerable<IRequestParameter> aParams)
        {
            IEnumerable<IRequestParameter> urlSegements = aParams.Where((v) => v.Type == ParameterType.UrlSegment);
            IEnumerable<IRequestParameter> queryStrings = aParams.Where((v) => v.Type == ParameterType.QueryString);

            string trueUrl = aUrl;

            //Fill out the params 
            foreach(IRequestParameter param in urlSegements)
            {
                string endpointId = $"{{{param.Id}}}";
                trueUrl = trueUrl.Replace(endpointId, param.Value.ToString());
            }

            //Make sure we've filled them all...
            if(trueUrl.Contains("{") || trueUrl.Contains("}"))
            {
                throw new InvalidOperationException("Failed to fill out all url segment parameters. Perhaps they weren't all provided?");
            }


            //BUild up querystrings and append to the url
            List<string> queryStringParams = new List<string>();

            foreach(IRequestParameter param in queryStrings)
            {
                queryStringParams.Add($"{param.Id}={param.Value}");
            }

            queryStringParams.Add("output=json");

            trueUrl = string.Format("{0}?{1}", trueUrl, string.Join("&", queryStringParams));

            return trueUrl;
        }


        public void Dispose()
        {
            iCancellationTokenSource.Cancel();
            iCancellationTokenSource.Dispose();
        }
    }
}
