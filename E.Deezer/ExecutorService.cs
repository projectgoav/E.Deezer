using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace E.Deezer
{
    /// <summary>
    /// Class to directly interact with the internet.
    /// </summary>
    internal class ExecutorService : IDisposable
    {
        private const int DEFAULT_TIMEOUT = 30000; //30secs
        private readonly HttpClient _client;
        private readonly CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// Initialize a new instance with a custom <see cref="HttpMessageHandler"/>.
        /// </summary>
        internal ExecutorService(HttpMessageHandler httpMessageHandler = null)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            var handler = httpMessageHandler ?? new HttpClientHandler();
            _client = new HttpClient(handler, disposeHandler: true);
            ConfigureHttpClient();
        }

        internal CancellationToken CancellationToken => _cancellationTokenSource.Token;

        /// <summary>
        /// Sends a GET request to the API.
        /// </summary>
        /// <param name="methodName">Additional parts of the URL.</param>
        /// <param name="params">Additional parameters for the URL.</param>
        /// <returns>Server response as <see cref="Stream"/>. Do not forget to Dispose() it!</returns>
        /// <exception cref="HttpRequestException">Occurs
        /// when the response was not HTTP 200 or when the response
        /// content is null.</exception>
        /// <exception cref="InvalidOperationException">Error with one
        /// or more URL segment(s).</exception>
        internal virtual async Task<Stream> GetAsync(string methodName, IList<IRequestParameter> @params)
        {
            string url = BuildUrl(methodName, @params);

            var response = await _client.GetAsync(url, CancellationToken).ConfigureAwait(false);

            Check(response);

            var responseStream = await response.Content
                .ReadAsStreamAsync()
                .ConfigureAwait(false);

            return GetDecompessionStreamForResponse(responseStream, response.Content.Headers);
        }

        /// <summary>
        /// Sends a POST request to the API.
        /// </summary>
        /// <param name="methodName">Additional parts of the URL.</param>
        /// <param name="params">Additional parameters for the URL.</param>
        /// <returns>Server response as <see cref="Stream"/>. Do not forget to Dispose() it!</returns>
        /// <exception cref="HttpRequestException">Occurs
        /// when the response was not HTTP 200 or when the response
        /// content is null.</exception>
        /// <exception cref="InvalidOperationException">Error with one
        /// or more URL segment(s).</exception>
        internal virtual Task<Stream> PostAsync(string methodName, IList<IRequestParameter> @params)
        {
            @params.Add(RequestParameter.GetNewQueryStringParameter("request_method", "post"));

            return GetAsync(methodName, @params);
        }

        /// <summary>
        /// Sends a DELETE request to the API.
        /// </summary>
        /// <param name="methodName">Additional parts of the URL.</param>
        /// <param name="params">Additional parameters for the URL.</param>
        /// <returns>Server response as <see cref="Stream"/>. Do not forget to Dispose() it!</returns>
        /// <exception cref="HttpRequestException">Occurs
        /// when the response was not HTTP 200 or when the response
        /// content is null.</exception>
        /// <exception cref="InvalidOperationException">Error with one
        /// or more URL segment(s).</exception>
        internal virtual Task<Stream> DeleteAsync(string methodName, IList<IRequestParameter> @params)
        {
            @params.Add(RequestParameter.GetNewQueryStringParameter("request_method", "delete"));

            return GetAsync(methodName, @params);
        }

        /// <summary>
        /// Crafts the final URL which will be added to the base URL.
        /// </summary>
        /// <returns>Non null string.</returns>
        /// <exception cref="InvalidOperationException">Error with one
        /// or more URL segment(s).</exception>
        private string BuildUrl(string url, IEnumerable<IRequestParameter> parms)
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

            // Make sure we've filled all URL segments
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

        /// <summary>
        /// Checks the response for HTTP errors and Exceptions.
        /// </summary>
        /// <exception cref="HttpRequestException">Occurs
        /// when the response was not HTTP 200 or when the response
        /// content is null.</exception>
        private void Check(HttpResponseMessage response)
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
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();

                _client.CancelPendingRequests();
                _client.Dispose();
            }
        }
    }
}
