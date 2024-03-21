using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

using NUnit.Framework;
using NUnit.Framework.Legacy;

using E.Deezer.Api;

namespace E.Deezer.Tests.Integration.Endpoint
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class RadioEndpointTests : TestClassBase, IDisposable 
    {
        private OfflineMessageHandler handler;
        private DeezerSession session;

        public RadioEndpointTests()
            : base("RadioEndpoint")
        {
            this.handler = new OfflineMessageHandler();
            this.session = new DeezerSession(this.handler);
        }


        // IDisposable
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
        public void GetTop5()
        {
            handler.Content = base.GetServerResponse("top");

            IEnumerable<IRadio> radios = session.Radio.GetTopRadio(CancellationToken.None)
                                                      .Result;

            ClassicAssert.IsNotNull(radios, nameof(radios));
            ClassicAssert.AreEqual(25, radios.Count(), "Count");

            var firstRadio = radios.First();
            ClassicAssert.IsNotNull(firstRadio, nameof(firstRadio));
            ClassicAssert.AreEqual(36891, firstRadio.Id, nameof(firstRadio.Id));
            ClassicAssert.AreEqual("Deep House", firstRadio.Title, nameof(firstRadio.Title));
        }

        /*
        [Test]
        public void GetDeezerSelection()
        {
            handler.Content = base.GetServerResponse("list");


            IEnumerable<IRadio> radios = session.Radio.GetDeezerSelection();


            ClassicAssert.IsNotNull(radios, nameof(radios));
            ClassicAssert.AreEqual(77, radios.Count(), "Count");

            var firstRadio = radios.First();
            ClassicAssert.IsNotNull(firstRadio, nameof(firstRadio));
            ClassicAssert.AreEqual(37151, firstRadio.Id, nameof(firstRadio.Id));
            ClassicAssert.AreEqual("Slágerek", firstRadio.Title, nameof(firstRadio.Title));
        }
        */

            /*
        [Test]
        public void GetByGenres()
        {
            handler.Content = base.GetServerResponse("genres");


            IEnumerable<IGenreWithRadios> radios = session.Radio.GetByGenres();
        

            ClassicAssert.IsNotNull(radios, nameof(radios));
            ClassicAssert.AreEqual(20, radios.Count(), "radios.Count");

            IGenreWithRadios firstGenre = radios.First();
            ClassicAssert.AreEqual(132, firstGenre.ID, nameof(firstGenre.ID));
            ClassicAssert.AreEqual("Pop", firstGenre.Title, nameof(firstGenre.Title));

            ClassicAssert.AreEqual(25, firstGenre.Radios.Count(), "firstGenre.Radios.Count");

            IRadio lastRadio = firstGenre.Radios.Last();
            ClassicAssert.AreEqual(32101, lastRadio.Id, $"{nameof(lastRadio)}.{nameof(lastRadio.Id)}");
            ClassicAssert.AreEqual("Phenom'enon", lastRadio.Title, $"{nameof(lastRadio)}.{nameof(lastRadio.Title)}");
        }
        */

        /* TODO This is implemented. We just need to hook up getting the
         *      tracks present on a radio. 
        [Test]
        public void GetByTracks()
        {
            ClassicAssert.Warn("This functionality not yet implemented!");
        }
        */
    }
}
