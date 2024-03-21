using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

using NUnit.Framework;

using E.Deezer.Api;
using NUnit.Framework.Legacy;

namespace E.Deezer.Tests.Integration.Endpoint
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class SearchEndpointTests : TestClassBase, IDisposable
    {
        private static readonly string DUMMY_TEXT = "search-term";


        private OfflineMessageHandler handler;
        private DeezerSession session;



        public SearchEndpointTests()
            : base("SearchEndpoint")
        {
            this.handler = new OfflineMessageHandler();
            this.session = new DeezerSession(this.handler);
        }

        
        //IDisposable
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
        public void Albums()
        {
            handler.Content = base.GetServerResponse("albums");

            IEnumerable<IAlbum> albums = session.Search.FindAlbums(DUMMY_TEXT, CancellationToken.None)
                                                       .Result;

            ClassicAssert.IsNotNull(albums, nameof(albums));
            ClassicAssert.AreEqual(100, albums.Count(), "Count");

            var firstAlbum = albums.First();
            ClassicAssert.IsNotNull(firstAlbum, nameof(firstAlbum));
            ClassicAssert.AreEqual(103248, firstAlbum.Id, nameof(firstAlbum.Id));
            ClassicAssert.AreEqual("The Eminem Show", firstAlbum.Title, nameof(firstAlbum.Title));
        }

        [Test]
        public void Artists()
        {
            handler.Content = base.GetServerResponse("artists");

            IEnumerable<IArtist> artists = session.Search.FindArtists(DUMMY_TEXT, CancellationToken.None)
                                                         .Result;

            ClassicAssert.IsNotNull(artists, nameof(artists));
            ClassicAssert.AreEqual(48, artists.Count(), "Count");

            var firstArtis = artists.First();
            ClassicAssert.IsNotNull(firstArtis, nameof(firstArtis));
            ClassicAssert.AreEqual(13, firstArtis.Id, nameof(firstArtis.Id));
            ClassicAssert.AreEqual("Eminem", firstArtis.Name, nameof(firstArtis.Name));
        }

        [Test]
        public void Playlists()
        {
            handler.Content = base.GetServerResponse("playlists");

            IEnumerable<IPlaylist> playlists = session.Search.FindPlaylists(DUMMY_TEXT, CancellationToken.None)
                                                             .Result;

            ClassicAssert.IsNotNull(playlists, nameof(playlists));
            ClassicAssert.AreEqual(100, playlists.Count(), "Count");

            var firstPlaylist = playlists.First();
            ClassicAssert.IsNotNull(firstPlaylist, nameof(firstPlaylist));
            ClassicAssert.AreEqual(3645740262, firstPlaylist.Id, nameof(firstPlaylist.Id));
            ClassicAssert.AreEqual("100% Eminem", firstPlaylist.Title, nameof(firstPlaylist.Title));
        }

        [Test]
        public void Tracks()
        {
            handler.Content = base.GetServerResponse("tracks");

            IEnumerable<ITrack> tracks = session.Search.FindTracks(DUMMY_TEXT, CancellationToken.None)
                                                       .Result;

            ClassicAssert.IsNotNull(tracks, nameof(tracks));
            ClassicAssert.AreEqual(100, tracks.Count(), "Count");

            var firstTrack = tracks.First();
            ClassicAssert.IsNotNull(firstTrack, nameof(firstTrack));
            ClassicAssert.AreEqual(1109731, firstTrack.Id, nameof(firstTrack.Id));
            ClassicAssert.AreEqual("Lose Yourself (From \"8 Mile\" Soundtrack)", firstTrack.Title, nameof(firstTrack.Title));
        }

        [Test]
        public void Radio()
        {
            handler.Content = base.GetServerResponse("radio");

            IEnumerable<IRadio> radios = session.Search.FindRadio(DUMMY_TEXT, CancellationToken.None)
                                                       .Result;

            ClassicAssert.IsNotNull(radios, nameof(radios));
            ClassicAssert.AreEqual(0, radios.Count(), "Count");
        }

        [Test]
        public void User()
        {
            handler.Content = base.GetServerResponse("user");

            IEnumerable<IUserProfile> users = session.Search.FindUsers(DUMMY_TEXT, CancellationToken.None)
                                                            .Result;

            ClassicAssert.IsNotNull(users, nameof(users));
            ClassicAssert.AreEqual(92, users.Count(), "Count");

            var firstUser = users.First();
            ClassicAssert.IsNotNull(firstUser, nameof(firstUser));
            ClassicAssert.AreEqual(7380218, firstUser.Id, nameof(firstUser.Id));
            ClassicAssert.AreEqual("eminem01", firstUser.Username, nameof(firstUser.Username));
        }

        /*
         * TODO: Reimplement 'Advanced' search
        [Test]
        public async Task Advanced()
        {
            handler.Content = base.GetServerResponse("advanced");


            IEnumerable<ITrack> tracks = await session.Advanced(DUMMY_TEXT);


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
        */


        /* TODO: Awaiting History support
        [Test]
        public void History()
        {
            Assert.Warn("This functionality not yet implemented!");
        }
        */
    }
}
