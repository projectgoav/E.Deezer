using E.Deezer.Api;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E.Deezer.Tests.Integration.Endpoint
{
    [TestFixture]
    public class RadioEndpointTests //: TestClassBase
    {
        /*
        private OfflineMessageHandler _server;
        private IRadioEndpoint _radio;

        public RadioEndpointTests()
            : base("RadioEndpoint") { }

        [SetUp]
        public void SetUp()
        {
            var session = OfflineDeezerSession.WithoutAuthentication();

            _radio = session.Library.Radio;
            _server = session.MessageHandler;
        }

        [Test]
        public async Task GetTop5()
        {
            _server.Content = base.GetServerResponse("top");


            IEnumerable<IRadio> radios = await _radio.GetTop5();


            Assert.IsNotNull(radios, nameof(radios));
            Assert.AreEqual(25, radios.Count(), "Count");

            var firstRadio = radios.First();
            Assert.IsNotNull(firstRadio, nameof(firstRadio));
            Assert.AreEqual(36891, firstRadio.Id, nameof(firstRadio.Id));
            Assert.AreEqual("Deep House", firstRadio.Title, nameof(firstRadio.Title));
        }

        [Test]
        public async Task GetDeezerSelection()
        {
            _server.Content = base.GetServerResponse("list");


            IEnumerable<IRadio> radios = await _radio.GetDeezerSelection();


            Assert.IsNotNull(radios, nameof(radios));
            Assert.AreEqual(77, radios.Count(), "Count");

            var firstRadio = radios.First();
            Assert.IsNotNull(firstRadio, nameof(firstRadio));
            Assert.AreEqual(37151, firstRadio.Id, nameof(firstRadio.Id));
            Assert.AreEqual("Slágerek", firstRadio.Title, nameof(firstRadio.Title));
        }

        [Test]
        public async Task GetByGenres()
        {
            _server.Content = base.GetServerResponse("genres");


            IEnumerable<IGenreWithRadios> radios = await _radio.GetByGenres();


            Assert.IsNotNull(radios, nameof(radios));
            Assert.AreEqual(20, radios.Count(), "radios.Count");

            IGenreWithRadios firstGenre = radios.First();
            Assert.AreEqual(132, firstGenre.ID, nameof(firstGenre.ID));
            Assert.AreEqual("Pop", firstGenre.Title, nameof(firstGenre.Title));

            Assert.AreEqual(25, firstGenre.Radios.Count(), "firstGenre.Radios.Count");

            IRadio lastRadio = firstGenre.Radios.Last();
            Assert.AreEqual(32101, lastRadio.Id, $"{nameof(lastRadio)}.{nameof(lastRadio.Id)}");
            Assert.AreEqual("Phenom'enon", lastRadio.Title, $"{nameof(lastRadio)}.{nameof(lastRadio.Title)}");
        }

        [Test]
        public void GetByTracks()
        {
            Assert.Warn("This functionality not yet implemented!");
        }


        */
    }
}
