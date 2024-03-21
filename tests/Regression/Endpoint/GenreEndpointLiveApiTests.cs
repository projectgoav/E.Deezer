using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using System.Net.Http;
using System.Threading;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace E.Deezer.Tests.Regression.Endpoint
{
#if LIVE_API_TEST
    [TestFixture]
#else
    [Ignore("Live API tests not enabled for this configuration")]
#endif
    public class GenreEndpointLiveApiTests : IDisposable
    {
        private readonly DeezerSession session;


        public GenreEndpointLiveApiTests()
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
        public void TestGetCommonGenre()
        {
            var result = this.session.Genre.GetCommonGenre(CancellationToken.None)
                                           .Result;

            ClassicAssert.NotNull(result);
            ClassicAssert.That(result.Count(), Is.GreaterThan(0));
        }


        [Test]
        public void TestGetGenreById()
        {
            var genre = this.session.Genre.GetById(0, CancellationToken.None)
                                          .Result;

            ClassicAssert.NotNull(genre);
            ClassicAssert.AreEqual(0, genre.Id);
        }


        [Test]
        public void TestGetGenreArtists()
        {
            var result = this.session.Genre.GetById(0, CancellationToken.None)
                                           .ContinueWith(async t => await t.Result.Artists(CancellationToken.None)
                                                                                  .ConfigureAwait(false))
                                           .Unwrap()
                                           .Result;

            ClassicAssert.NotNull(result);
            ClassicAssert.That(result.Count(), Is.GreaterThan(0));
        }


        [Test]
        public void TestGetGenreRadio()
        {
            var result = this.session.Genre.GetById(0, CancellationToken.None)
                                           .ContinueWith(async t => await t.Result.Radio(CancellationToken.None)
                                                                                  .ConfigureAwait(false))
                                           .Unwrap()
                                           .Result;

            ClassicAssert.NotNull(result);
            ClassicAssert.That(result.Count(), Is.GreaterThan(0));
        }


        [Test]
        public void TestGetDeezerSelection()
        {
            var result = this.session.Genre.GetDeezerSelectionForGenre(0, CancellationToken.None)
                                           .Result;

            ClassicAssert.NotNull(result);
            ClassicAssert.That(result.Count(), Is.GreaterThan(0));
        }


        [Test]
        public void TestNewReleases()
        {
            var result = this.session.Genre.GetNewReleasesForGenre(0, CancellationToken.None)
                                           .Result;

            ClassicAssert.NotNull(result);
            ClassicAssert.That(result.Count(), Is.GreaterThan(0));
        }


        //TODO: Need to add methods to get a genre's chart?
    }
}
