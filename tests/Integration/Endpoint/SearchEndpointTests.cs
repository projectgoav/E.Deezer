using E.Deezer.Api;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E.Deezer.Tests.Integration.Endpoint
{
    [TestFixture]
    public class SearchEndpointTests //: TestClassBase
    {
        /*

        private static readonly string _dummyText = "";
        private OfflineMessageHandler _server;
        private ISearchEndpoint _search;

        public SearchEndpointTests()
            : base("SearchEndpoint") { }

        [SetUp]
        public void SetUp()
        {
            var session = OfflineDeezerSession.WithoutAuthentication();

            _search = session.Library.Search;
            _server = session.MessageHandler;
        }

        [Test]
        public async Task Albums()
        {
            _server.Content = base.GetServerResponse("albums");


            IEnumerable<IAlbum> albums = await _search.Albums(_dummyText);


            Assert.IsNotNull(albums, nameof(albums));
            Assert.AreEqual(100, albums.Count(), "Count");

            var firstAlbum = albums.First();
            Assert.IsNotNull(firstAlbum, nameof(firstAlbum));
            Assert.AreEqual(103248, firstAlbum.Id, nameof(firstAlbum.Id));
            Assert.AreEqual("The Eminem Show", firstAlbum.Title, nameof(firstAlbum.Title));
        }

        [Test]
        public async Task Artists()
        {
            _server.Content = base.GetServerResponse("artists");


            IEnumerable<IArtist> artists = await _search.Artists(_dummyText);


            Assert.IsNotNull(artists, nameof(artists));
            Assert.AreEqual(48, artists.Count(), "Count");

            var firstArtis = artists.First();
            Assert.IsNotNull(firstArtis, nameof(firstArtis));
            Assert.AreEqual(13, firstArtis.Id, nameof(firstArtis.Id));
            Assert.AreEqual("Eminem", firstArtis.Name, nameof(firstArtis.Name));
        }

        [Test]
        public async Task Playlists()
        {
            _server.Content = base.GetServerResponse("playlists");


            IEnumerable<IPlaylist> playlists = await _search.Playlists(_dummyText);


            Assert.IsNotNull(playlists, nameof(playlists));
            Assert.AreEqual(100, playlists.Count(), "Count");

            var firstPlaylist = playlists.First();
            Assert.IsNotNull(firstPlaylist, nameof(firstPlaylist));
            Assert.AreEqual(3645740262, firstPlaylist.Id, nameof(firstPlaylist.Id));
            Assert.AreEqual("100% Eminem", firstPlaylist.Title, nameof(firstPlaylist.Title));
        }

        [Test]
        public async Task Tracks()
        {
            _server.Content = base.GetServerResponse("tracks");


            IEnumerable<ITrack> tracks = await _search.Tracks(_dummyText);


            Assert.IsNotNull(tracks, nameof(tracks));
            Assert.AreEqual(100, tracks.Count(), "Count");

            var firstTrack = tracks.First();
            Assert.IsNotNull(firstTrack, nameof(firstTrack));
            Assert.AreEqual(1109731, firstTrack.Id, nameof(firstTrack.Id));
            Assert.AreEqual("Lose Yourself (From \"8 Mile\" Soundtrack)", firstTrack.Title, nameof(firstTrack.Title));
        }

        [Test]
        public async Task Radio()
        {
            _server.Content = base.GetServerResponse("radio");


            IEnumerable<IRadio> radios = await _search.Radio(_dummyText);


            Assert.IsNotNull(radios, nameof(radios));
            Assert.AreEqual(0, radios.Count(), "Count");
        }

        [Test]
        public async Task User()
        {
            _server.Content = base.GetServerResponse("user");


            IEnumerable<IUser> users = await _search.User(_dummyText);


            Assert.IsNotNull(users, nameof(users));
            Assert.AreEqual(92, users.Count(), "Count");

            var firstUser = users.First();
            Assert.IsNotNull(firstUser, nameof(firstUser));
            Assert.AreEqual(7380218, firstUser.Id, nameof(firstUser.Id));
            Assert.AreEqual("eminem01", firstUser.Name, nameof(firstUser.Name));
        }

        [Test]
        public async Task Advanced()
        {
            _server.Content = base.GetServerResponse("advanced");


            IEnumerable<ITrack> tracks = await _search.Advanced(_dummyText);


            Assert.IsNotNull(tracks, nameof(tracks));
            Assert.AreEqual(100, tracks.Count(), "Count");

            var firstTrack = tracks.First();
            Assert.IsNotNull(firstTrack, nameof(firstTrack));
            Assert.AreEqual(916424, firstTrack.Id, nameof(firstTrack.Id));
            Assert.AreEqual("Without Me", firstTrack.Title, nameof(firstTrack.Title));

            var artist = firstTrack.Artist;
            Assert.IsNotNull(artist, nameof(artist));
            Assert.AreEqual(13, artist.Id, nameof(artist.Id));
            Assert.AreEqual("Eminem", artist.Name, nameof(artist.Name));

            var album = firstTrack.Album;
            Assert.IsNotNull(album, nameof(album));
            Assert.AreEqual(103248, album.Id, nameof(album.Id));
            Assert.AreEqual("The Eminem Show", album.Title, nameof(album.Title));
        }

        [Test]
        public void History()
        {
            Assert.Warn("This functionality not yet implemented!");
        }

        */
    }
}
