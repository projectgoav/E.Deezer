using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using NUnit.Framework;

namespace E.Deezer.Tests.Integration
{
    /// <summary>
    /// Class will take care the authentication
    /// process without calling the live API
    /// and after it will server the static
    /// file from the disk.
    /// </summary>
    class OfflineAuthenticationMessageHandler : OfflineMessageHandler
    {
        private readonly string dirPath;

        private byte requestCount;

        public OfflineAuthenticationMessageHandler()
        {
            dirPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "StaticResources");
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (requestCount == 0)
            {
                requestCount++;
                return GetUser();
            }
            else if (requestCount == 1)
            {
                requestCount++;
                return GetUserPermissions();
            }
            else if (requestCount == 2)
            {
                requestCount++;
                return base.SendAsync(request, cancellationToken);
            }
            else
            {
                throw new Exception("You call the HtppMessageHandler too much!");
            }
        }

        private Task<HttpResponseMessage> GetUser()
        {
            return Task.FromResult(FromFile("user.me"));
        }

        private Task<HttpResponseMessage> GetUserPermissions()
        {
            return Task.FromResult(FromFile("user.permissions"));
        }

        private HttpResponseMessage FromFile(string fileName)
        {
            string json = File.ReadAllText(
                Path.Combine(dirPath, $"{fileName}.json"));

            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json),
            };
        }
    }
}
