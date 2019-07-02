using E.Deezer.Api;
using E.Deezer.Endpoint;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E.Deezer.Tests.Integration.Endpoint
{
    [TestFixture]
    class GenreEndpointTests : TestClassBase
    {
        private OfflineMessageHandler _server;
        private IGenreEndpoint _genre;

        public GenreEndpointTests()
            : base("GenreEndpoint") { }

        [SetUp]
        public void SetUp()
        {
            var session = OfflineDeezerSession.WithoutAuthentication();

            _genre = session.Library.Browse.Genre;
            _server = session.MessageHandler;
        }

        [Test]
        public async Task GetCommonGenre()
        {
            _server.Content = base.GetServerResponse("GetCommonGenre");

            IEnumerable<IGenre> actual = await _genre.GetCommonGenre();

            var genres = actual.ToList();
            Assert.AreEqual(22, genres.Count, "Count");

            var secondGenre = genres[1];
            Assert.AreEqual(132, secondGenre.Id, nameof(secondGenre.Id));
            Assert.AreEqual("Pop", secondGenre.Name, nameof(secondGenre.Name));
        }
    }
}
