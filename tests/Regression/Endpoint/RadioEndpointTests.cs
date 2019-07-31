using E.Deezer.Api;
using E.Deezer.Endpoint;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E.Deezer.Tests.Regression.Endpoint
{
    [TestFixture]
    class RadioEndpointTests
    {
        private static IRadioEndpoint _radio;

        [OneTimeSetUp]
        public static void OneTimeSetUp()
        {
            _radio = DeezerSession.CreateNew().Radio;
        }

        [Test]
        public async Task GetTop5()
        {
            IEnumerable<IRadio> radios = await _radio.GetTop5();


            Assert.IsNotNull(radios, nameof(radios));
            Assert.AreEqual(25, radios.Count(), "Count");

            var firstRadio = radios.First();
            Assert.IsNotNull(firstRadio, nameof(firstRadio));
            Assert.That(firstRadio.Id, Is.GreaterThan(0), nameof(firstRadio.Id));
            Assert.IsNotNull(firstRadio.Title, nameof(firstRadio.Title));
        }

        [Test]
        public async Task GetDeezerSelection()
        {
            IEnumerable<IRadio> radios = await _radio.GetDeezerSelection();


            Assert.IsNotNull(radios, nameof(radios));
            Assert.That(radios.Count(), Is.GreaterThan(1), "Count");

            var firstRadio = radios.First();
            Assert.IsNotNull(firstRadio, nameof(firstRadio));
            Assert.That(firstRadio.Id, Is.GreaterThan(0), nameof(firstRadio.Id));
            Assert.IsNotNull(firstRadio.Title, nameof(firstRadio.Title));
        }

        [Test]
        public async Task GetByGenres()
        {
            IEnumerable<IGenreWithRadios> genres = await _radio.GetByGenres();


            Assert.IsNotNull(genres, nameof(genres));

            IRadio oneRadio = genres.First().Radios.First();
            IEnumerable<ITrack> tracks = oneRadio.GetFirst40Tracks()
                .GetAwaiter().GetResult();

            Assert.AreEqual(40, tracks.Count(), "Count");
        }
    }
}
