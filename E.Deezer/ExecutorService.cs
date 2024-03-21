using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Threading;
using System.IO.Compression;
using System.Threading.Tasks;

using System.Net.Http;
using System.Net.Http.Headers;

using E.Deezer.Util;

namespace E.Deezer
{
    internal class ExecutorService : IDisposable
    {
        private const int DEFAULT_TIMEOUT = 30000; //30secs

        private const string GZIP_COMPRESSION = "gzip";
        private const string DEFLATE_COMPRESSION = "deflate";

        private readonly HttpClient client;
        private readonly CancellationTokenSource cancellationTokenSource;

        internal ExecutorService(HttpMessageHandler httpMessageHandler = null)
        {
            this.cancellationTokenSource = new CancellationTokenSource();

            var handler = httpMessageHandler ?? new HttpClientHandler();
            this.client = new HttpClient(handler, disposeHandler: true);

            ConfigureHttpClient(this.client);
        }

        internal CancellationToken CancellationToken => this.cancellationTokenSource.GetCancellationTokenSafe();


        public Task<Stream> ExecuteGet(string resource, CancellationToken cancellationToken)
            => ExecuteRequest(cancellationToken, (token) => this.client.GetAsync(resource, token));


        public Task<Stream> ExecutePost(string resource, CancellationToken cancellationToken)
            => ExecuteRequest(cancellationToken, (token) => this.client.PostAsync(resource, null,  token));


        public Task<Stream> ExecuteDelete(string resource, CancellationToken cancellationToken)
            => ExecuteRequest(cancellationToken, (token) => this.client.DeleteAsync(resource, token));

        

        private void ConfigureHttpClient(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("https://api.deezer.com/");
            httpClient.Timeout = TimeSpan.FromMilliseconds(DEFAULT_TIMEOUT);

            // Allow us to deal with compressed content, should Deezer support it.
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue(GZIP_COMPRESSION));
            httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue(DEFLATE_COMPRESSION));
        }


        private Task<Stream> ExecuteRequest(CancellationToken cancellationToken,
                                            Func<CancellationToken, Task<HttpResponseMessage>> requestFunc)
        {
            // Using a linkedtoken source allows both the calling code or this
            // executor's token to cancel the request.
            //
            // As tokenSources implement IDisposable, we chain on tasks for both
            // the happy and unhappy path to ensure we cleanup after ourselves.
            var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(this.CancellationToken, cancellationToken);

            if (linkedTokenSource.IsCancellationRequested)
            {
                linkedTokenSource.Dispose();

                return Task.FromCanceled<Stream>(cancellationToken);
            }

            var requestTask = requestFunc(linkedTokenSource.Token);
                
            var returnTask = requestTask.ContinueWith(async t =>
            {
                t.ThrowIfFaulted();

                var response = t.Result;

                CheckHttpResponseForError(response);

                return await GetDecompessionStreamForResponse(response)
                                            .ConfigureAwait(false);

            }, linkedTokenSource.Token, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);


            // Clean up the linked token source
            requestTask.ContinueWith(_ => linkedTokenSource.Dispose(), TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.ExecuteSynchronously);
            returnTask.ContinueWith(_ => linkedTokenSource.Dispose(), TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously);

            return returnTask.Unwrap();
        }


        private async Task<Stream> GetDecompessionStreamForResponse(HttpResponseMessage response)
        {
            if (response == null)
                return null;

            var contentHeaders = response.Content.Headers;
            var responseStream = await response.Content.ReadAsStreamAsync();
                                                
            // If header is not present the Deezer may not support this compression algorithm OR the
            // given HttpMessageHandler has support for automatic compression.
            if (contentHeaders != null && contentHeaders.ContentEncoding.Any())
            {
                foreach (var entry in contentHeaders.ContentEncoding)
                {
                    switch (entry.ToLowerInvariant())
                    {
                        case GZIP_COMPRESSION:
                            return new GZipStream(responseStream, CompressionMode.Decompress);

                        case DEFLATE_COMPRESSION:
                            return new DeflateStream(responseStream, CompressionMode.Decompress);
                    }
                }
            }

            return responseStream;
        }


        private void CheckHttpResponseForError(HttpResponseMessage response)
        {
            HttpRequestException exceptionToThrow = null;

            if(!response.IsSuccessStatusCode)
            {
                string msg = $"Status: {response.StatusCode} :: {response.ReasonPhrase}";
                exceptionToThrow = new HttpRequestException(msg);
            }

            if (response.Content == null)
            {
                exceptionToThrow = new HttpRequestException("Request returned but there was no content attached.");
            }


            // Ensure we cleanup should we be throwing
            if (exceptionToThrow != null)
            {
                response.Dispose();
                throw exceptionToThrow;
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
