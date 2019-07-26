using E.Deezer.Api;
using E.Deezer.Endpoint;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E.Deezer.Tests.Regression.Endpoint
{
    [TestFixture]
    class ChartsEndpointTests
    {
        private static IChartsEndpoint _charts;

        [OneTimeSetUp]
        public static void OneTimeSetUp()
        {
            _charts = DeezerSession.CreateNew().Browse.Charts;
        }

        [Test]
        public async Task GetChart()
        {
            IChart chart = await _charts.GetChart();


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

        [Test]
        public async Task GetAlbumChart()
        {
            IEnumerable<IAlbum> albums = await _charts.GetAlbumChart();


            Assert.IsNotNull(albums, nameof(albums));
            Assert.That(albums.Count(), Is.GreaterThan(10), "Count");
        }

        [Test]
        public async Task GetArtistChart()
        {
            IEnumerable<IArtist> artists = await _charts.GetArtistChart();


            Assert.IsNotNull(artists, nameof(artists));
            Assert.That(artists.Count(), Is.GreaterThan(10), "Count");
        }

        [Test]
        public async Task GetPlaylistChart()
        {
            IEnumerable<IPlaylist> playlists = await _charts.GetPlaylistChart();


            Assert.IsNotNull(playlists, nameof(playlists));
            Assert.That(playlists.Count(), Is.GreaterThan(10), "Count");
        }

        [Test]
        public async Task GetTrackChart()
        {
            IEnumerable<ITrack> tracks = await _charts.GetTrackChart();


            Assert.IsNotNull(tracks, nameof(tracks));
            Assert.That(tracks.Count(), Is.GreaterThan(10), "Count");
        }
    }
}
