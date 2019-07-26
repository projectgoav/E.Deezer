using E.Deezer.Api;
using E.Deezer.Endpoint;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E.Deezer.Tests.Integration.Endpoint
{
    [TestFixture]
    class ChartsEndpointTests : TestClassBase
    {
        private OfflineMessageHandler _server;
        private IChartsEndpoint _charts;

        public ChartsEndpointTests()
            : base("ChartsEndpoint") { }

        [SetUp]
        public void SetUp()
        {
            var session = OfflineDeezerSession.WithoutAuthentication();

            _charts = session.Library.Browse.Charts;
            _server = session.MessageHandler;
        }

        [Test]
        public async Task GetChart()
        {
            _server.Content = base.GetServerResponse("chart");


            IChart chart = await _charts.GetChart();


            Assert.IsNotNull(chart, nameof(chart));

            Assert.IsNotNull(chart.Tracks, nameof(chart.Tracks));
            Assert.AreEqual(100, chart.Tracks.Count(), "Tracks.Count");

            Assert.IsNotNull(chart.Albums, nameof(chart.Albums));
            Assert.AreEqual(99, chart.Albums.Count(), "Albums.Count");

            Assert.IsNotNull(chart.Artists, nameof(chart.Artists));
            Assert.AreEqual(100, chart.Artists.Count(), "Artists.Count");

            Assert.IsNotNull(chart.Playlists, nameof(chart.Playlists));
            Assert.AreEqual(100, chart.Playlists.Count(), "Playlists.Count");
        }

        [Test]
        public async Task GetAlbumChart()
        {
            _server.Content = base.GetServerResponse("album");


            IEnumerable<IAlbum> albums = await _charts.GetAlbumChart();


            Assert.IsNotNull(albums, nameof(albums));
            Assert.AreEqual(99, albums.Count(), "Count");

            var firstAlbum = albums.First();
            Assert.IsNotNull(firstAlbum, nameof(firstAlbum));
            Assert.AreEqual(97400042, firstAlbum.Id, nameof(firstAlbum.Id));
            Assert.AreEqual("My Songs (Deluxe)", firstAlbum.Title, nameof(firstAlbum.Title));
        }

        [Test]
        public async Task GetArtistChart()
        {
            _server.Content = base.GetServerResponse("artist");


            IEnumerable<IArtist> artists = await _charts.GetArtistChart();


            Assert.IsNotNull(artists, nameof(artists));
            Assert.AreEqual(100, artists.Count(), "Count");

            var firstArtist = artists.First();
            Assert.IsNotNull(firstArtist, nameof(firstArtist));
            Assert.AreEqual(11800683, firstArtist.Id, nameof(firstArtist.Id));
            Assert.AreEqual("Follow The Flow", firstArtist.Name, nameof(firstArtist.Name));
        }

        [Test]
        public async Task GetPlaylistChart()
        {
            _server.Content = base.GetServerResponse("playlist");


            IEnumerable<IPlaylist> playlists = await _charts.GetPlaylistChart();


            Assert.IsNotNull(playlists, nameof(playlists));
            Assert.AreEqual(100, playlists.Count(), "Count");

            var firstPlaylist = playlists.First();
            Assert.IsNotNull(firstPlaylist, nameof(firstPlaylist));
            Assert.AreEqual(5522964682, firstPlaylist.Id, nameof(firstPlaylist.Id));
            Assert.AreEqual("Nyári slágerek 2019", firstPlaylist.Title, nameof(firstPlaylist.Title));
        }

        [Test]
        public async Task GetTrackChart()
        {
            _server.Content = base.GetServerResponse("track");


            IEnumerable<ITrack> tracks = await _charts.GetTrackChart();


            Assert.IsNotNull(tracks, nameof(tracks));
            Assert.AreEqual(100, tracks.Count(), "Count");

            var firstTrack = tracks.First();
            Assert.IsNotNull(firstTrack, nameof(firstTrack));
            Assert.AreEqual(698905582, firstTrack.Id, nameof(firstTrack.Id));
            Assert.AreEqual("Señorita", firstTrack.Title, nameof(firstTrack.Title));
        }

        [Test]
        public void GetPodcastChart()
        {
            Assert.Warn("This functionality not yet implemented!");
        }
    }
}
