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
    public class UserEndpointTests : TestClassBase
    {
        private OfflineAuthenticationMessageHandler handler;
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

            ClassicAssert.IsTrue(didLogin);
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

            ClassicAssert.IsNotNull(tracks, nameof(tracks));
            ClassicAssert.AreEqual(50, tracks.Count(), "Count");

            var firstTrack = tracks.First();
            ClassicAssert.IsNotNull(firstTrack, nameof(firstTrack));
            ClassicAssert.AreEqual(391727202, firstTrack.Id, nameof(firstTrack.Id));
            ClassicAssert.AreEqual("Silence", firstTrack.Title, nameof(firstTrack.Title));
        }

        [Test]
        public void GetFlow()
        {
            handler.Content = base.GetServerResponse("flow");

            IEnumerable<ITrack> tracks = session.User.GetFlow(this.session.CurrentUserId, CancellationToken.None)
                                                     .Result;

            ClassicAssert.IsNotNull(tracks, nameof(tracks));
            ClassicAssert.AreEqual(40, tracks.Count(), "Count");

            var firstTrack = tracks.First();
            ClassicAssert.IsNotNull(firstTrack, nameof(firstTrack));
            ClassicAssert.AreEqual(561756292, firstTrack.Id, nameof(firstTrack.Id));
            ClassicAssert.AreEqual("Without Me", firstTrack.Title, nameof(firstTrack.Title));
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

            ClassicAssert.IsNotNull(playlists, nameof(playlists));
            ClassicAssert.AreEqual(3, playlists.Count(), "Count");

            var firstPlaylist = playlists.First();
            ClassicAssert.IsNotNull(firstPlaylist, nameof(firstPlaylist));
            ClassicAssert.AreEqual(3093783442, firstPlaylist.Id, nameof(firstPlaylist.Id));
            ClassicAssert.AreEqual("Favourite tracks", firstPlaylist.Title, nameof(firstPlaylist.Title));
        }

        [Test]
        public void GetFavouriteAlbums()
        {
            handler.Content = base.GetServerResponse("favourite_albums");

            IEnumerable<IAlbum> albums = session.User.GetFavouriteAlbums(this.session.CurrentUserId, CancellationToken.None)
                                                     .Result;

            ClassicAssert.IsNotNull(albums, nameof(albums));
            ClassicAssert.AreEqual(2, albums.Count(), "Count");

            var firstAlbum = albums.First();
            ClassicAssert.IsNotNull(firstAlbum, nameof(firstAlbum));
            ClassicAssert.AreEqual(68913181, firstAlbum.Id, nameof(firstAlbum.Id));
            ClassicAssert.AreEqual("Arcade Mammoth", firstAlbum.Title, nameof(firstAlbum.Title));
        }

        [Test]
        public void GetFavouriteArtists()
        {
            handler.Content = base.GetServerResponse("favourite_artists");

            IEnumerable<IArtist> artists = session.User.GetFavouriteArtists(this.session.CurrentUserId, CancellationToken.None)
                                                       .Result;

            ClassicAssert.IsNotNull(artists, nameof(artists));
            ClassicAssert.AreEqual(7, artists.Count(), "Count");

            var firstArtist = artists.First();
            ClassicAssert.IsNotNull(firstArtist, nameof(firstArtist));
            ClassicAssert.AreEqual(310557, firstArtist.Id, nameof(firstArtist.Id));
            ClassicAssert.AreEqual("Steve Aoki", firstArtist.Name, nameof(firstArtist.Name));
        }

        [Test]
        public void GetFavouriteTracks()
        {
            handler.Content = base.GetServerResponse("favourite_tracks");

            IEnumerable<ITrack> tracks = session.User.GetFavouriteTracks(this.session.CurrentUserId, CancellationToken.None)
                                                     .Result;

            ClassicAssert.IsNotNull(tracks, nameof(tracks));
            ClassicAssert.AreEqual(58, tracks.Count(), "Count");

            var firstTrack = tracks.First();
            ClassicAssert.IsNotNull(firstTrack, nameof(firstTrack));
            ClassicAssert.AreEqual(565423972, firstTrack.Id, nameof(firstTrack.Id));
            ClassicAssert.AreEqual("MIA (feat. Drake)", firstTrack.Title, nameof(firstTrack.Title));
        }

        [Test]
        public void GetRecommendedAlbums()
        {
            handler.Content = base.GetServerResponse("recommended_albums");

            IEnumerable<IAlbum> albums = session.User.GetRecommendedAlbums(CancellationToken.None)
                                                     .Result;

            ClassicAssert.IsNotNull(albums, nameof(albums));
            ClassicAssert.AreEqual(99, albums.Count(), "Count");

            var firstAlbum = albums.First();
            ClassicAssert.IsNotNull(firstAlbum, nameof(firstAlbum));
            ClassicAssert.AreEqual(10174474, firstAlbum.Id, nameof(firstAlbum.Id));
            ClassicAssert.AreEqual("The Beginning", firstAlbum.Title, nameof(firstAlbum.Title));
        }

        [Test]
        public void GetRecommendedArtists()
        {
            handler.Content = base.GetServerResponse("recommended_artists");

            IEnumerable<IArtist> artists = session.User.GetRecommendedArtists(CancellationToken.None)
                                                       .Result;

            ClassicAssert.IsNotNull(artists, nameof(artists));
            ClassicAssert.AreEqual(30, artists.Count(), "Count");

            var firstArtist = artists.First();
            ClassicAssert.IsNotNull(firstArtist, nameof(firstArtist));
            ClassicAssert.AreEqual(1353625, firstArtist.Id, nameof(firstArtist.Id));
            ClassicAssert.AreEqual("R3hab", firstArtist.Name, nameof(firstArtist.Name));
        }

        [Test]
        public void GetRecommendedPlaylists()
        {
            handler.Content = base.GetServerResponse("recommended_playlists");

            IEnumerable<IPlaylist> playlists = session.User.GetRecommendedPlaylists(CancellationToken.None)
                                                           .Result;

            ClassicAssert.IsNotNull(playlists, nameof(playlists));
            ClassicAssert.AreEqual(29, playlists.Count(), "Count");

            var firstPlaylist = playlists.First();
            ClassicAssert.IsNotNull(firstPlaylist, nameof(firstPlaylist));
            ClassicAssert.AreEqual(706093725, firstPlaylist.Id, nameof(firstPlaylist.Id));
            ClassicAssert.AreEqual("Pulse", firstPlaylist.Title, nameof(firstPlaylist.Title));
        }

        [Test]
        public void GetRecommendedTracks()
        {
            handler.Content = base.GetServerResponse("recommended_tracks");

            IEnumerable<ITrack> tracks = session.User.GetRecommendedTracks(CancellationToken.None)
                                                     .Result;

            ClassicAssert.IsNotNull(tracks, nameof(tracks));
            ClassicAssert.AreEqual(40, tracks.Count(), "Count");

            var firstTrack = tracks.First();
            ClassicAssert.IsNotNull(firstTrack, nameof(firstTrack));
            ClassicAssert.AreEqual(485156332, firstTrack.Id, nameof(firstTrack.Id));
            ClassicAssert.AreEqual("Hold On Tight", firstTrack.Title, nameof(firstTrack.Title));
        }

        [Test]
        public void GetRecommendedRadio()
        {
            handler.Content = base.GetServerResponse("recommended_radio");

            IEnumerable<IRadio> radios = session.User.GetRecommendedRadio(CancellationToken.None)
                                                     .Result;

            ClassicAssert.IsNotNull(radios, nameof(radios));
            ClassicAssert.AreEqual(40, radios.Count(), "Count");

            var firstRadio = radios.First();
            ClassicAssert.IsNotNull(firstRadio, nameof(firstRadio));
            ClassicAssert.AreEqual(531582301, firstRadio.Id, nameof(firstRadio.Id));
            ClassicAssert.AreEqual("Arcade Mammoth", firstRadio.Title, nameof(firstRadio.Title));
        }

    }
}
