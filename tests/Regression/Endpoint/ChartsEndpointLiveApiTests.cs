using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Net.Http; 
using System.Threading;

using NUnit.Framework;

using E.Deezer.Api;

namespace E.Deezer.Tests.Regression.Endpoint
{
#if LIVE_API_TEST
    [TestFixture]
#else
    [Ignore("Live API tests not enabled for this configuration")]
#endif
    public class ChartsEndpointLiveApiTests : IDisposable
    {
        private readonly DeezerSession session;


        public ChartsEndpointLiveApiTests()
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


        [TestCase(true)]
        [TestCase(false)]
        public void GetChart(bool useGenreFilter)
        {
            IChart chart = useGenreFilter ? this.session.Charts.GetChartsForGenre(0, CancellationToken.None).Result
                                          : this.session.Charts.GetCharts(CancellationToken.None).Result;
                
            Assert.IsNotNull(chart, nameof(chart));

            Assert.IsNotNull(chart.Tracks, nameof(chart.Tracks));
            Assert.That(chart.Tracks.Count(), Is.GreaterThan(10), "Tracks.Count");

            Assert.IsNotNull(chart.Albums, nameof(chart.Albums));
            Assert.That(chart.Albums.Count(), Is.GreaterThan(10), "Albums.Count");

            Assert.IsNotNull(chart.Artists, nameof(chart.Artists));
            Assert.That(chart.Artists.Count(), Is.GreaterThan(10), "Artists.Count");

            Assert.IsNotNull(chart.Playlists, nameof(chart.Playlists));
            Assert.That(chart.Playlists.Count(), Is.GreaterThan(10), "Playlists.Count");
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetAlbumChart(bool useGenreFilter)
        {
            IEnumerable<IAlbum> albums = useGenreFilter ? this.session.Charts.GetAlbumChartForGenre(0, CancellationToken.None).Result
                                                        : this.session.Charts.GetAlbumChart(CancellationToken.None).Result;
                
            Assert.IsNotNull(albums, nameof(albums));
            Assert.That(albums.Count(), Is.GreaterThan(10), "Count");
        }


        [TestCase(true)]
        [TestCase(false)]
        public void GetArtistChart(bool useGenreFilter)
        {
            IEnumerable<IArtist> artists = useGenreFilter ? this.session.Charts.GetArtistChartForGenre(0, CancellationToken.None).Result
                                                          : this.session.Charts.GetArtistChart(CancellationToken.None).Result;

            Assert.IsNotNull(artists, nameof(artists));
            Assert.That(artists.Count(), Is.GreaterThan(10), "Count");
        }


        [TestCase(true)]
        [TestCase(false)]
        public void GetPlaylistChart(bool useGenreFilter)
        {
            IEnumerable<IPlaylist> playlists = useGenreFilter ? this.session.Charts.GetPlaylistChartForGenre(0, CancellationToken.None).Result
                                                              : this.session.Charts.GetPlaylistChart(CancellationToken.None).Result;

            Assert.IsNotNull(playlists, nameof(playlists));
            Assert.That(playlists.Count(), Is.GreaterThan(10), "Count");
        }


        [TestCase(true)]
        [TestCase(false)]
        public void GetTrackChart(bool useGenreFilter)
        {
            IEnumerable<ITrack> tracks = useGenreFilter ? this.session.Charts.GetTrackChartForGenre(0, CancellationToken.None).Result
                                                        : this.session.Charts.GetTrackChart(CancellationToken.None).Result;

            Assert.IsNotNull(tracks, nameof(tracks));
            Assert.That(tracks.Count(), Is.GreaterThan(10), "Count");
        }
    }
}
