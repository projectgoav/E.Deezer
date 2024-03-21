using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Net.Http;
using System.Threading;

using NUnit.Framework;
using NUnit.Framework.Legacy;

using E.Deezer.Api;


namespace E.Deezer.Tests.Regression.Endpoint
{
#if LIVE_API_TEST
    [TestFixture]
#else
    [Ignore("Live API tests not enabled for this configuration")]
#endif
    public class RadioEndpointLiveApiTests : IDisposable
    {
        private readonly DeezerSession session;


        public RadioEndpointLiveApiTests()
        {
            this.session = new DeezerSession(new HttpClientHandler());
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
                this.session.Dispose();
            }
        }



        [Test]
        public void TestGetTop5()
        {
            IEnumerable<IRadio> radios = this.session.Radio.GetTopRadio(CancellationToken.None)
                                                           .Result;


            ClassicAssert.IsNotNull(radios, nameof(radios));
            ClassicAssert.AreEqual(25, radios.Count(), "Count");

            var firstRadio = radios.First();
            ClassicAssert.IsNotNull(firstRadio, nameof(firstRadio));
            ClassicAssert.That(firstRadio.Id, Is.GreaterThan(0), nameof(firstRadio.Id));
            ClassicAssert.IsNotNull(firstRadio.Title, nameof(firstRadio.Title));
        }

        /* TODO: On Selection support
         * 
        [Test]
        public void GetDeezerSelection()
        {
            IEnumerable<IRadio> radios = this.session.Radio.GetSelection(CancellationToken.None)
                                                           .Result;


            Assert.IsNotNull(radios, nameof(radios));
            Assert.That(radios.Count(), Is.GreaterThan(1), "Count");

            var firstRadio = radios.First();
            Assert.IsNotNull(firstRadio, nameof(firstRadio));
            Assert.That(firstRadio.Id, Is.GreaterThan(0), nameof(firstRadio.Id));
            Assert.IsNotNull(firstRadio.Title, nameof(firstRadio.Title));
        }
        */


        /* TODO: IMplement me!!
        [Test]
        public void GetByGenres()
        {
            IEnumerable<IGenreWithRadios> genres = await _radio.GetByGenres();


            Assert.IsNotNull(genres, nameof(genres));

            IRadio oneRadio = genres.First().Radios.First();
            IEnumerable<ITrack> tracks = oneRadio.GetFirst40Tracks()
                .GetAwaiter().GetResult();

            Assert.AreEqual(40, tracks.Count(), "Count");
        }
        */
    }
}
