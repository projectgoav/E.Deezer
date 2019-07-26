using E.Deezer.Api;
using E.Deezer.Endpoint;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E.Deezer.Tests.Integration.Endpoint
{
    [TestFixture]
    class UserEndpointTests : TestClassBase
    {
        private OfflineMessageHandler _server;
        private IUserEndpoint _user;

        public UserEndpointTests()
            : base("UserEndpoint") { }

        [SetUp]
        public void SetUp()
        {
            var session = OfflineDeezerSession.WithAuthentication();

            _user = session.Library.User;
            _server = session.MessageHandler;
        }

        [Test]
        public async Task GetHistory()
        {
            _server.Content = base.GetServerResponse("history");


            IEnumerable<ITrack> tracks = await _user.GetHistory();


            Assert.IsNotNull(tracks, nameof(tracks));
            Assert.AreEqual(50, tracks.Count(), "Count");

            var firstTrack = tracks.First();
            Assert.IsNotNull(firstTrack, nameof(firstTrack));
            Assert.AreEqual(391727202, firstTrack.Id, nameof(firstTrack.Id));
            Assert.AreEqual("Silence", firstTrack.Title, nameof(firstTrack.Title));
        }

        [Test]
        public async Task GetFlow()
        {
            _server.Content = base.GetServerResponse("flow");


            IEnumerable<ITrack> tracks = await _user.GetFlow();


            Assert.IsNotNull(tracks, nameof(tracks));
            Assert.AreEqual(40, tracks.Count(), "Count");

            var firstTrack = tracks.First();
            Assert.IsNotNull(firstTrack, nameof(firstTrack));
            Assert.AreEqual(561756292, firstTrack.Id, nameof(firstTrack.Id));
            Assert.AreEqual("Without Me", firstTrack.Title, nameof(firstTrack.Title));
        }

        [Test]
        public async Task GetPersonalTracks()
        {
            _server.Content = base.GetServerResponse("personal_tracks");


            IEnumerable<ITrack> tracks = await _user.GetPersonalTracks();


            Assert.IsNotNull(tracks, nameof(tracks));
            Assert.AreEqual(0, tracks.Count(), "Count");
        }

        [Test]
        public async Task GetPlaylists()
        {
            _server.Content = base.GetServerResponse("playlist");


            IEnumerable<IPlaylist> playlists = await _user.GetPlaylists();


            Assert.IsNotNull(playlists, nameof(playlists));
            Assert.AreEqual(3, playlists.Count(), "Count");

            var firstPlaylist = playlists.First();
            Assert.IsNotNull(firstPlaylist, nameof(firstPlaylist));
            Assert.AreEqual(3093783442, firstPlaylist.Id, nameof(firstPlaylist.Id));
            Assert.AreEqual("Favourite tracks", firstPlaylist.Title, nameof(firstPlaylist.Title));
        }

        [Test]
        public async Task GetFavouriteAlbums()
        {
            _server.Content = base.GetServerResponse("favourite_albums");


            IEnumerable<IAlbum> albums = await _user.GetFavouriteAlbums();


            Assert.IsNotNull(albums, nameof(albums));
            Assert.AreEqual(2, albums.Count(), "Count");

            var firstAlbum = albums.First();
            Assert.IsNotNull(firstAlbum, nameof(firstAlbum));
            Assert.AreEqual(68913181, firstAlbum.Id, nameof(firstAlbum.Id));
            Assert.AreEqual("Arcade Mammoth", firstAlbum.Title, nameof(firstAlbum.Title));
        }

        [Test]
        public async Task GetFavouriteArtists()
        {
            _server.Content = base.GetServerResponse("favourite_artists");


            IEnumerable<IArtist> artists = await _user.GetFavouriteArtists();


            Assert.IsNotNull(artists, nameof(artists));
            Assert.AreEqual(7, artists.Count(), "Count");

            var firstArtist = artists.First();
            Assert.IsNotNull(firstArtist, nameof(firstArtist));
            Assert.AreEqual(310557, firstArtist.Id, nameof(firstArtist.Id));
            Assert.AreEqual("Steve Aoki", firstArtist.Name, nameof(firstArtist.Name));
        }

        [Test]
        public async Task GetFavouriteTracks()
        {
            _server.Content = base.GetServerResponse("favourite_tracks");


            IEnumerable<ITrack> tracks = await _user.GetFavouriteTracks();


            Assert.IsNotNull(tracks, nameof(tracks));
            Assert.AreEqual(58, tracks.Count(), "Count");

            var firstTrack = tracks.First();
            Assert.IsNotNull(firstTrack, nameof(firstTrack));
            Assert.AreEqual(565423972, firstTrack.Id, nameof(firstTrack.Id));
            Assert.AreEqual("MIA (feat. Drake)", firstTrack.Title, nameof(firstTrack.Title));
        }

        [Test]
        public async Task GetRecommendedAlbums()
        {
            _server.Content = base.GetServerResponse("recommended_albums");


            IEnumerable<IAlbum> albums = await _user.GetRecommendedAlbums();


            Assert.IsNotNull(albums, nameof(albums));
            Assert.AreEqual(99, albums.Count(), "Count");

            var firstAlbum = albums.First();
            Assert.IsNotNull(firstAlbum, nameof(firstAlbum));
            Assert.AreEqual(10174474, firstAlbum.Id, nameof(firstAlbum.Id));
            Assert.AreEqual("The Beginning", firstAlbum.Title, nameof(firstAlbum.Title));
        }

        [Test]
        public async Task GetRecommendedArtists()
        {
            _server.Content = base.GetServerResponse("recommended_artists");


            IEnumerable<IArtist> artists = await _user.GetRecommendedArtists();


            Assert.IsNotNull(artists, nameof(artists));
            Assert.AreEqual(30, artists.Count(), "Count");

            var firstArtist = artists.First();
            Assert.IsNotNull(firstArtist, nameof(firstArtist));
            Assert.AreEqual(1353625, firstArtist.Id, nameof(firstArtist.Id));
            Assert.AreEqual("R3hab", firstArtist.Name, nameof(firstArtist.Name));
        }

        [Test]
        public async Task GetRecommendedPlaylists()
        {
            _server.Content = base.GetServerResponse("recommended_playlists");


            IEnumerable<IPlaylist> playlists = await _user.GetRecommendedPlaylists();


            Assert.IsNotNull(playlists, nameof(playlists));
            Assert.AreEqual(29, playlists.Count(), "Count");

            var firstPlaylist = playlists.First();
            Assert.IsNotNull(firstPlaylist, nameof(firstPlaylist));
            Assert.AreEqual(706093725, firstPlaylist.Id, nameof(firstPlaylist.Id));
            Assert.AreEqual("Pulse", firstPlaylist.Title, nameof(firstPlaylist.Title));
        }

        [Test]
        public async Task GetRecommendedTracks()
        {
            _server.Content = base.GetServerResponse("recommended_tracks");


            IEnumerable<ITrack> tracks = await _user.GetRecommendedTracks();


            Assert.IsNotNull(tracks, nameof(tracks));
            Assert.AreEqual(40, tracks.Count(), "Count");

            var firstTrack = tracks.First();
            Assert.IsNotNull(firstTrack, nameof(firstTrack));
            Assert.AreEqual(485156332, firstTrack.Id, nameof(firstTrack.Id));
            Assert.AreEqual("Hold On Tight", firstTrack.Title, nameof(firstTrack.Title));
        }

        [Test]
        public async Task GetRecommendedRadio()
        {
            _server.Content = base.GetServerResponse("recommended_radio");


            IEnumerable<IRadio> radios = await _user.GetRecommendedRadio();


            Assert.IsNotNull(radios, nameof(radios));
            Assert.AreEqual(40, radios.Count(), "Count");

            var firstRadio = radios.First();
            Assert.IsNotNull(firstRadio, nameof(firstRadio));
            Assert.AreEqual(531582301, firstRadio.Id, nameof(firstRadio.Id));
            Assert.AreEqual("Arcade Mammoth", firstRadio.Title, nameof(firstRadio.Title));
        }
    }
}
