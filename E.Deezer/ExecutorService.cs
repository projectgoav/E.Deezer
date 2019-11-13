using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

using E.Deezer.Api;


namespace E.Deezer
{
    internal class ExecutorService : IDisposable
    {
        private const int DEFAULT_TIMEOUT = 30000; //30secs

        private readonly HttpClient _client;
        private readonly JsonSerializer _jsonSerializer;
        private readonly CancellationTokenSource _cancellationTokenSource;

        internal ExecutorService(HttpMessageHandler httpMessageHandler = null)
        {
            _jsonSerializer = CreateJsonSerializer();
            _cancellationTokenSource = new CancellationTokenSource();

            var handler = httpMessageHandler ?? new HttpClientHandler();
            _client = new HttpClient(handler, disposeHandler: true);

            ConfigureHttpClient();
        }

        internal CancellationToken CancellationToken => _cancellationTokenSource.Token;

        public Task<T> ExecuteGet<T>(string method, IEnumerable<IRequestParameter> parms)
        {
            string url = BuildUrl(method, parms);
            return _client.GetAsync(url, this.CancellationToken)
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
            return _client.PostAsync(url, null, this.CancellationToken)
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
            return _client.PostAsync(url, null, this.CancellationToken)
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

            return _client.DeleteAsync(url, CancellationToken)
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

            foreach (var p in parms)
            {
                switch (p.Type)
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

        private void ConfigureHttpClient()
        {
            _client.BaseAddress = new Uri("https://api.deezer.com/");
            _client.Timeout = TimeSpan.FromMilliseconds(DEFAULT_TIMEOUT);

            // Allow us to deal with compressed content, should Deezer support it.
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            _client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
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
                            return this._jsonSerializer.Deserialize<T>(jsonReader);
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
            if (!response.IsSuccessStatusCode)
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
                this._cancellationTokenSource.Cancel();
                this._cancellationTokenSource.Dispose();

                this._client.CancelPendingRequests();
                this._client.Dispose();
            }
        }
    }
}
