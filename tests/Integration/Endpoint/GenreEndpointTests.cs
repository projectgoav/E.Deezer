using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

using NUnit.Framework;

using E.Deezer.Api;
using NUnit.Framework.Legacy;

namespace E.Deezer.Tests.Integration.Endpoint
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class GenreEndpointTests : TestClassBase, IDisposable
    {
        private OfflineMessageHandler handler;
        private DeezerSession session;

        public GenreEndpointTests()
            : base("GenreEndpoint")
        {
            this.handler = new OfflineMessageHandler();
            this.session = new DeezerSession(this.handler);
        }
        

        // IDisposabe
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.session.Dispose();
            }
        }


        [Test]
        public void GetCommonGenre()
        {
            handler.Content = base.GetServerResponse("GetCommonGenre");

            IEnumerable<IGenre> actual = session.Genre.GetCommonGenre(CancellationToken.None)
                                                      .Result;

            var genres = actual.ToList();
            ClassicAssert.AreEqual(22, genres.Count, "Count");

            var secondGenre = genres[1];
            ClassicAssert.AreEqual(132, secondGenre.Id, nameof(secondGenre.Id));
            ClassicAssert.AreEqual("Pop", secondGenre.Name, nameof(secondGenre.Name));
        }
    }
}
