using System;
using System.Threading.Tasks;

using System.Net.Http;
using System.Threading;

using NUnit.Framework;

using Moq;
using Moq.Protected;
using NUnit.Framework.Legacy;


namespace E.Deezer.Tests
{
    [TestFixture]
    public class ExecutorServiceTests
    {
        private const string SEND_ASYNC_FUNC = "SendAsync";

        private ExecutorService executor;
        private Mock<HttpMessageHandler> handler;

        [SetUp]
        public void SetUp()
        {

            this.handler = new Mock<HttpMessageHandler>();

            this.handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                                                  ItExpr.IsAny<HttpRequestMessage>(),
                                                  ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromCanceled<HttpResponseMessage>(new CancellationToken(true)))    // Return a cancelled task so we don't do anything :)
                .Verifiable();

            executor = new ExecutorService(handler.Object);
            ClassicAssert.NotNull(executor);
        }

        [TearDown]
        public void TearDown()
        {
            executor.Dispose();
            executor = null;
        }

       

        [Test]
        public void TestAccessingDisposedCancellationTokenIsSafe()
        {
            var e = new ExecutorService(this.handler.Object);
            e.Dispose();

            
            /* Accessing the token here should not throw.
             * The tasks themselves will not execute anything as
             * should be automatically in the cancelled state. */
            e.ExecuteGet("resource", CancellationToken.None);
            e.ExecutePost("resource", CancellationToken.None);
            e.ExecuteDelete("resource", CancellationToken.None);

            this.handler.Protected()
                       .Verify(SEND_ASYNC_FUNC,
                               Times.Never(),
                               ItExpr.IsAny<HttpRequestMessage>(),
                               ItExpr.IsAny<CancellationToken>());
        }

        

        [Test]
        public void TestGet()
        {
            try
            {
                this.executor.ExecuteGet("resource", CancellationToken.None)
                                .Wait();
            }
            catch (AggregateException ex)
            {
                if (!(ex.GetBaseException() is TaskCanceledException))
                {
                    throw;
                }
            }

            this.handler.Protected()
                        .Verify("SendAsync",
                               Times.Once(),
                               ItExpr.Is<HttpRequestMessage>(msg => msg.Method == HttpMethod.Get && msg.RequestUri.AbsoluteUri == "https://api.deezer.com/resource"),
                               ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public void TestPost()
        {
            try
            {
                this.executor.ExecutePost("resource", CancellationToken.None)
                                .Wait();
            }
            catch (AggregateException ex)
            {
                if (!(ex.GetBaseException() is TaskCanceledException))
                {
                    throw;
                }
            }

            this.handler.Protected()
                        .Verify("SendAsync",
                               Times.Once(),
                               ItExpr.Is<HttpRequestMessage>(msg => msg.Method == HttpMethod.Post && msg.RequestUri.AbsoluteUri == "https://api.deezer.com/resource"),
                               ItExpr.IsAny<CancellationToken>());
        }


        [Test]
        public void TestDelete()
        {
            try
            {
                this.executor.ExecuteDelete("resource", CancellationToken.None)
                             .Wait();
            }
            catch (AggregateException ex)
            {
                if (!(ex.GetBaseException() is TaskCanceledException))
                {
                    throw;
                }
            }


            this.handler.Protected()
                        .Verify("SendAsync",
                               Times.Once(),
                               ItExpr.Is<HttpRequestMessage>(msg => msg.Method == HttpMethod.Delete && msg.RequestUri.AbsoluteUri == "https://api.deezer.com/resource"),
                               ItExpr.IsAny<CancellationToken>());
        }



        [Test]
        public void TestRequestsAreCancelledCorrectly()
        {
            this.handler.Protected()
                        .Setup<Task<HttpResponseMessage>>("SendAsync",
                                                          ItExpr.IsAny<HttpRequestMessage>(),
                                                          ItExpr.IsAny<CancellationToken>())
                        .Returns<HttpRequestMessage, CancellationToken>((_, tkn) => Task.Delay(500, tkn)
                                                                                        .ContinueWith(__ => Task.FromResult(new HttpResponseMessage()), tkn)
                                                                                        .Unwrap())
                        .Verifiable();


            using (var cts = new CancellationTokenSource())
            {
                var requestTask = this.executor.ExecuteGet("resource", cts.Token);

                cts.Cancel();

                try
                {
                    requestTask.Wait();
                }
                catch(AggregateException ex)
                {
                    if (!(ex.GetBaseException() is TaskCanceledException))
                    {
                        throw;
                    }

                    return;
                }
            }

            Assert.Fail("Task didn't appear to be cancelled.");

        }

    }
}
