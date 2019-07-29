using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Threading;
using System.IO.Compression;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

using E.Deezer.Api;


namespace E.Deezer
{
    internal class ExecutorService : IDisposable
    {
        private const int DEFAULT_TIMEOUT = 30000; //30secs

        private readonly HttpClient client;
        private readonly JsonSerializer jsonSerializer;
        private readonly CancellationTokenSource cancellationTokenSource;

        internal ExecutorService(HttpMessageHandler httpMessageHandler = null)
            : this(DeezerSession.ENDPOINT, httpMessageHandler)
        { }

        internal ExecutorService(string testUrl, HttpMessageHandler httpMessageHandler = null)
        {
            this.jsonSerializer = CreateJsonSerializer();
            this.cancellationTokenSource = new CancellationTokenSource();

            var handler = httpMessageHandler ?? new HttpClientHandler();
            this.client = new HttpClient(handler, disposeHandler: true);

            ConfigureHttpClient(this.client);
        }

        internal CancellationToken CancellationToken { get { return cancellationTokenSource.Token; } }


        public Task<T> ExecuteGet<T>(string method, IEnumerable<IRequestParameter> parms)
        {
            string url = BuildUrl(method, parms);
            return client.GetAsync(url, this.CancellationToken)
                         .ContinueWith(async t =>
                         {
                             if (t.IsFaulted)
                             {
                                 throw t.Exception.GetBaseException();
                             }

                             // Ensure we dispose of stuff should things go bad
                             using (t.Result)
                             {
                                 CheckHttpResponse(t.Result);

                                 return await GetJsonObjectFromResponse<T>(t.Result)
                                                .ConfigureAwait(false);
                             }

                         }, this.CancellationToken, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default)
                        .Unwrap();
        }

        public Task<bool> ExecutePost(string method, IEnumerable<IRequestParameter> parms)
        {
            string url = BuildUrl(method, parms);
            return client.PostAsync(url, null, this.CancellationToken)
                         .ContinueWith<bool>(t =>
                         {
                             if (t.IsFaulted)
                             {
                                 throw t.Exception.GetBaseException();
                             }

                             using (t.Result)
                             {
                                 // TODO -> This isn't entirely correct, as there can often be a Deezer error hidden in here...
                                 return t.Result.IsSuccessStatusCode;
                             }

                         }, this.CancellationToken, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }

        public Task<T> ExecutePost<T>(string method, IEnumerable<IRequestParameter> parms)
        {
            string url = BuildUrl(method, parms);
            return client.PostAsync(url, null, this.CancellationToken)
                         .ContinueWith(async t =>
                         {
                             if (t.IsFaulted)
                             {
                                 throw t.Exception.GetBaseException();
                             }

                             using (t.Result)
                             {
                                 CheckHttpResponse(t.Result);

                                 // TODO -> This isn't entirely correct, as there can often be a Deezer error hidden in here...
                                 return await GetJsonObjectFromResponse<T>(t.Result)
                                                .ConfigureAwait(false);
                             }

                         }, this.CancellationToken, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default)
                        .Unwrap();
        }

        public Task<bool> ExecuteDelete(string method, IEnumerable<IRequestParameter> parms)
        {
            string url = BuildUrl(method, parms);

            return client.DeleteAsync(url, CancellationToken)
                         .ContinueWith(t =>
                         {
                             if (t.IsFaulted)
                             {
                                 throw t.Exception.GetBaseException();
                             }

                             using (t.Result)
                             {
                                 //TODO => This isn't entirely correct, as there can often be a Deezer error hidden in here..
                                 return t.Result.IsSuccessStatusCode;
                             }

                         }, this.CancellationToken, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }


        internal string BuildUrl(string url, IEnumerable<IRequestParameter> parms)
        {
            string trueUrl = url;
            var queryStrings = new List<string>();

            foreach(var p in parms)
            {
                switch(p.Type)
                {
                    case ParameterType.UrlSegment:
                        {
                            string idToReplace = $"{{{p.Id}}}";
                            trueUrl = trueUrl.Replace(idToReplace, p.Value.ToString());
                            break;
                        }
                    case ParameterType.QueryString:
                        {
                            string escapedValue = Uri.EscapeDataString(p.Value.ToString());
                            queryStrings.Add($"{p.Id}={escapedValue}");
                            break;
                        }
                }
            }

            //Make sure we've filled all url segments...
            if (trueUrl.Contains("{") || trueUrl.Contains("}"))
            {
                throw new InvalidOperationException("Failed to fill out all url segment parameters. Perhaps they weren't all provided?");
            }

            // Ensure we request json output (not default)
            queryStrings.Add("output=json");

            return string.Format("{0}?{1}", trueUrl, string.Join("&", queryStrings));
        }


        private void ConfigureHttpClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri(DeezerSession.ENDPOINT);
            httpClient.Timeout = TimeSpan.FromMilliseconds(DEFAULT_TIMEOUT);

            // Allow us to deal with compressed content, should Deezer support it.
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
        }

        private JsonSerializer CreateJsonSerializer()
        {
            var customConverters = new List<JsonConverter>()
            {
                // TODO add any customer converters we might end up with..
                new DeezerObjectResponseJsonDeserializer(),
            };

            var jsonSerailizerSettings = new JsonSerializerSettings()
            {
                Converters = customConverters,
            };

            return JsonSerializer.Create(jsonSerailizerSettings);
        }

        private Stream GetDecompessionStreamForResponse(Stream responseStream, HttpContentHeaders contentHeaders)
        {
            // If header is not present the Deezer may not support this compression algorithm OR the
            // given HttpMessageHandler has support for automatic compression.
            if (contentHeaders != null && contentHeaders.ContentEncoding.Any())
            {
                foreach (var entry in contentHeaders.ContentEncoding)
                {
                    switch (entry.ToLowerInvariant())
                    {
                        case "gzip":
                            {
                                return new GZipStream(responseStream, CompressionMode.Decompress);
                            }
                        case "deflate":
                            {
                                return new DeflateStream(responseStream, CompressionMode.Decompress);
                            }
                    }
                }
            }

            return responseStream;
        }

        private async Task<T> DeserializeResponseStream<T>(HttpResponseMessage response)
        {
            using (var responseStream = await response.Content.ReadAsStreamAsync()
                                                              .ConfigureAwait(false))
            {
                using (var compressedStream = GetDecompessionStreamForResponse(responseStream, response.Content.Headers))
                {
                    using (var reader = new StreamReader(compressedStream))
                    {
                        using (var jsonReader = new JsonTextReader(reader))
                        {
                            return this.jsonSerializer.Deserialize<T>(jsonReader);
                        }
                    }
                }
            }
        }

        private async Task<T> GetJsonObjectFromResponse<T>(HttpResponseMessage response)
        {
            using (response)
            {
                CheckHttpResponse(response);

                return await DeserializeResponseStream<T>(response)
                                .ConfigureAwait(false);
            }
        }

        //Checks a response for errors and exceptions
        private void CheckHttpResponse(HttpResponseMessage response)
        {
            if(!response.IsSuccessStatusCode)
            {
                string msg = $"Status: {response.StatusCode} :: {response.ReasonPhrase}";
                throw new HttpRequestException(msg);
            }

            if (response.Content == null)
            {
                throw new HttpRequestException("Request returned but there was no content attached.");
            }
        }



        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.cancellationTokenSource.Cancel();
                this.cancellationTokenSource.Dispose();

                this.client.CancelPendingRequests();
                this.client.Dispose();
            }
        }
    }
}
