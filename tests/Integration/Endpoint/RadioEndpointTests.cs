using E.Deezer.Api;
using E.Deezer.Endpoint;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E.Deezer.Tests.Integration.Endpoint
{
    [TestFixture]
    class RadioEndpointTests : TestClassBase
    {
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


            IEnumerable<IRadio> radios = await _radio.GetByGenres();


            Assert.IsNotNull(radios, nameof(radios));
            Assert.Fail("GetByGenres has a wrong return type! Radios nested inside a List of Genre class!");
        }

        [Test]
        public void GetByTracks()
        {
            Assert.Warn("This functionality not yet implemented!");
        }
    }
}
