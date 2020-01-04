using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

using NUnit.Framework;

using E.Deezer.Api;

namespace E.Deezer.Tests.Integration.Endpoint
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class UserEndpointTests : TestClassBase
    {
        private OfflineMessageHandler handler;
        private DeezerSession session;

        public UserEndpointTests()
            : base("UserEndpoint")
        { }

        [SetUp]
        public void SetUp()
        {
            this.handler = new OfflineAuthenticationMessageHandler();

            this.session = new DeezerSession(this.handler);

            bool didLogin = this.session.Login("access_token", CancellationToken.None)
                                        .Result;

            Assert.IsTrue(didLogin);
        }
        
        [TearDown]
        public void TearDown()
        {
            this.session.Dispose();
        }



        [Test]
        public void GetHistory()
        {
            handler.Content = base.GetServerResponse("history");

            IEnumerable<ITrack> tracks = session.User.GetListeningHistory(CancellationToken.None)
                                                     .Result;

            Assert.IsNotNull(tracks, nameof(tracks));
            Assert.AreEqual(50, tracks.Count(), "Count");

            var firstTrack = tracks.First();
            Assert.IsNotNull(firstTrack, nameof(firstTrack));
            Assert.AreEqual(391727202, firstTrack.Id, nameof(firstTrack.Id));
            Assert.AreEqual("Silence", firstTrack.Title, nameof(firstTrack.Title));
        }

        [Test]
        public void GetFlow()
        {
            handler.Content = base.GetServerResponse("flow");

            IEnumerable<ITrack> tracks = session.User.GetFlow(this.session.CurrentUserId, CancellationToken.None)
                                                     .Result;

            Assert.IsNotNull(tracks, nameof(tracks));
            Assert.AreEqual(40, tracks.Count(), "Count");

            var firstTrack = tracks.First();
            Assert.IsNotNull(firstTrack, nameof(firstTrack));
            Assert.AreEqual(561756292, firstTrack.Id, nameof(firstTrack.Id));
            Assert.AreEqual("Without Me", firstTrack.Title, nameof(firstTrack.Title));
        }

        /*
         * TODO: Awaiting personal track support
        [Test]
        public void GetPersonalTracks()
        {
            handler.Content = base.GetServerResponse("personal_tracks");

            IEnumerable<ITrack> tracks = session.User.GetPersonalTracks(CancellationToken.None);

            Assert.IsNotNull(tracks, nameof(tracks));
            Assert.AreEqual(0, tracks.Count(), "Count");
        }
        */


        [Test]
        public void GetPlaylists()
        {
            handler.Content = base.GetServerResponse("playlist");

            IEnumerable<IPlaylist> playlists = session.User.GetPlaylists(CancellationToken.None)
                                                           .Result;

            Assert.IsNotNull(playlists, nameof(playlists));
            Assert.AreEqual(3, playlists.Count(), "Count");

            var firstPlaylist = playlists.First();
            Assert.IsNotNull(firstPlaylist, nameof(firstPlaylist));
            Assert.AreEqual(3093783442, firstPlaylist.Id, nameof(firstPlaylist.Id));
            Assert.AreEqual("Favourite tracks", firstPlaylist.Title, nameof(firstPlaylist.Title));
        }

        [Test]
        public void GetFavouriteAlbums()
        {
            handler.Content = base.GetServerResponse("favourite_albums");

            IEnumerable<IAlbum> albums = session.User.GetFavouriteAlbums(this.session.CurrentUserId, CancellationToken.None)
                                                     .Result;

            Assert.IsNotNull(albums, nameof(albums));
            Assert.AreEqual(2, albums.Count(), "Count");

            var firstAlbum = albums.First();
            Assert.IsNotNull(firstAlbum, nameof(firstAlbum));
            Assert.AreEqual(68913181, firstAlbum.Id, nameof(firstAlbum.Id));
            Assert.AreEqual("Arcade Mammoth", firstAlbum.Title, nameof(firstAlbum.Title));
        }

        [Test]
        public void GetFavouriteArtists()
        {
            handler.Content = base.GetServerResponse("favourite_artists");

            IEnumerable<IArtist> artists = session.User.GetFavouriteArtists(this.session.CurrentUserId, CancellationToken.None)
                                                       .Result;

            Assert.IsNotNull(artists, nameof(artists));
            Assert.AreEqual(7, artists.Count(), "Count");

            var firstArtist = artists.First();
            Assert.IsNotNull(firstArtist, nameof(firstArtist));
            Assert.AreEqual(310557, firstArtist.Id, nameof(firstArtist.Id));
            Assert.AreEqual("Steve Aoki", firstArtist.Name, nameof(firstArtist.Name));
        }

        [Test]
        public void GetFavouriteTracks()
        {
            handler.Content = base.GetServerResponse("favourite_tracks");

            IEnumerable<ITrack> tracks = session.User.GetFavouriteTracks(this.session.CurrentUserId, CancellationToken.None)
                                                     .Result;

            Assert.IsNotNull(tracks, nameof(tracks));
            Assert.AreEqual(58, tracks.Count(), "Count");

            var firstTrack = tracks.First();
            Assert.IsNotNull(firstTrack, nameof(firstTrack));
            Assert.AreEqual(565423972, firstTrack.Id, nameof(firstTrack.Id));
            Assert.AreEqual("MIA (feat. Drake)", firstTrack.Title, nameof(firstTrack.Title));
        }

        [Test]
        public void GetRecommendedAlbums()
        {
            handler.Content = base.GetServerResponse("recommended_albums");

            IEnumerable<IAlbum> albums = session.User.GetRecommendedAlbums(CancellationToken.None)
                                                     .Result;

            Assert.IsNotNull(albums, nameof(albums));
            Assert.AreEqual(99, albums.Count(), "Count");

            var firstAlbum = albums.First();
            Assert.IsNotNull(firstAlbum, nameof(firstAlbum));
            Assert.AreEqual(10174474, firstAlbum.Id, nameof(firstAlbum.Id));
            Assert.AreEqual("The Beginning", firstAlbum.Title, nameof(firstAlbum.Title));
        }

        [Test]
        public void GetRecommendedArtists()
        {
            handler.Content = base.GetServerResponse("recommended_artists");

            IEnumerable<IArtist> artists = session.User.GetRecommendedArtists(CancellationToken.None)
                                                       .Result;

            Assert.IsNotNull(artists, nameof(artists));
            Assert.AreEqual(30, artists.Count(), "Count");

            var firstArtist = artists.First();
            Assert.IsNotNull(firstArtist, nameof(firstArtist));
            Assert.AreEqual(1353625, firstArtist.Id, nameof(firstArtist.Id));
            Assert.AreEqual("R3hab", firstArtist.Name, nameof(firstArtist.Name));
        }

        [Test]
        public void GetRecommendedPlaylists()
        {
            handler.Content = base.GetServerResponse("recommended_playlists");

            IEnumerable<IPlaylist> playlists = session.User.GetRecommendedPlaylists(CancellationToken.None)
                                                           .Result;

            Assert.IsNotNull(playlists, nameof(playlists));
            Assert.AreEqual(29, playlists.Count(), "Count");

            var firstPlaylist = playlists.First();
            Assert.IsNotNull(firstPlaylist, nameof(firstPlaylist));
            Assert.AreEqual(706093725, firstPlaylist.Id, nameof(firstPlaylist.Id));
            Assert.AreEqual("Pulse", firstPlaylist.Title, nameof(firstPlaylist.Title));
        }

        [Test]
        public void GetRecommendedTracks()
        {
            handler.Content = base.GetServerResponse("recommended_tracks");

            IEnumerable<ITrack> tracks = session.User.GetRecommendedTracks(CancellationToken.None)
                                                     .Result;

            Assert.IsNotNull(tracks, nameof(tracks));
            Assert.AreEqual(40, tracks.Count(), "Count");

            var firstTrack = tracks.First();
            Assert.IsNotNull(firstTrack, nameof(firstTrack));
            Assert.AreEqual(485156332, firstTrack.Id, nameof(firstTrack.Id));
            Assert.AreEqual("Hold On Tight", firstTrack.Title, nameof(firstTrack.Title));
        }

        [Test]
        public void GetRecommendedRadio()
        {
            handler.Content = base.GetServerResponse("recommended_radio");

            IEnumerable<IRadio> radios = session.User.GetRecommendedRadio(CancellationToken.None)
                                                     .Result;

            Assert.IsNotNull(radios, nameof(radios));
            Assert.AreEqual(40, radios.Count(), "Count");

            var firstRadio = radios.First();
            Assert.IsNotNull(firstRadio, nameof(firstRadio));
            Assert.AreEqual(531582301, firstRadio.Id, nameof(firstRadio.Id));
            Assert.AreEqual("Arcade Mammoth", firstRadio.Title, nameof(firstRadio.Title));
        }

    }
}
