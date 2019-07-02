using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace E.Deezer.Tests.Integration
{
    /// <summary>
    /// Class will help to return content like a live call
    /// to the Deezer API.
    /// </summary>
    class OfflineMessageHandler : HttpMessageHandler
    {
        public OfflineMessageHandler()
        {
            StatusCode = HttpStatusCode.OK;
        }

        public HttpStatusCode StatusCode { get; set; }

        public HttpContent Content { get; set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                new HttpResponseMessage()
                {
                    StatusCode = this.StatusCode,
                    Content = this.Content,
                });
        }
    }
}
