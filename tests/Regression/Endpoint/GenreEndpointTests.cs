using E.Deezer.Api;
using E.Deezer.Endpoint;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E.Deezer.Tests.Regression.Endpoint
{
    [TestFixture]
    class GenreEndpointTests
    {
        private static IGenreEndpoint _genre;

        [OneTimeSetUp]
        public static void OneTimeSetUp()
        {
            _genre = DeezerSession.CreateNew().Browse.Genre;
        }

        [Test]
        public async Task GetCommonGenre()
        {
            var actual = (List<IGenre>)await _genre.GetCommonGenre();

            Assert.AreEqual(22, actual.Count);

            var actualPop = actual[1];
            Assert.AreEqual(132, actualPop.Id);
            Assert.AreEqual("Pop", actualPop.Name);
        }
    }
}
