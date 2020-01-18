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
    public class UserEndpointLiveApiTests : IDisposable
    {
        private const ulong RADIO_ID = 6L;
        private const ulong ARTIST_ID = 27L;
        private const ulong ALBUM_ID = 302127L;
        private const ulong TRACK_ID = 3135556L;
        private const ulong PLAYLIST_ID = 908622995L;
        private const string ACCESS_TOKEN = "frmFoXgyyO1ATzluA6gZIFIoWAf8b8G4tGWHaoxtDN9oCKMghM";

        private readonly Random random;
        private readonly DeezerSession session;

        private IRadio radio;
        private IAlbum album;
        private ITrack track;
        private IArtist artist;
        private IPlaylist playlist;

        public UserEndpointLiveApiTests()
        {
            this.random = new Random((int)DateTime.UtcNow.Ticks);
            this.session = new DeezerSession(new HttpClientHandler());

            LoginSession();

            FetchObjects();
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


        private void LoginSession()
        {
            bool didLogin = this.session.Login(ACCESS_TOKEN, CancellationToken.None)
                                        .Result;

            Assert.IsTrue(didLogin);

            //TODO: Access and assert we have enough permissions to run thses tests?
        }

        private void FetchObjects()
        {
            this.album = this.session.Albums.GetById(ALBUM_ID, CancellationToken.None)
                                            .Result;

            this.artist = this.session.Artists.GetById(ARTIST_ID, CancellationToken.None)
                                              .Result;

            this.playlist = this.session.Playlists.GetById(PLAYLIST_ID, CancellationToken.None)
                                                  .Result;

            this.track = this.session.Tracks.GetById(TRACK_ID, CancellationToken.None)
                                            .Result;

            this.radio = this.session.Radio.GetById(RADIO_ID, CancellationToken.None)
                                           .Result;

            Assert.NotNull(album);
            Assert.NotNull(artist);
            Assert.NotNull(playlist);
            Assert.NotNull(track);
            Assert.NotNull(radio);
        }


        [Test]
        public void GetHistory()
        {
            IEnumerable<ITrack> tracks = this.session.User.GetListeningHistory(CancellationToken.None)
                                                          .Result;

            Assert.IsNotNull(tracks, nameof(tracks));
            Assert.That(tracks.Count(), Is.GreaterThan(0), nameof(tracks));
        }


        /* NOTE: To prevent getting API limited by making the many requests contained in some of these tests
         *       we add a small delay to the beginning and the end of the tests. */
        [SetUp]
        public void SetUp()
        {
            var waitTime = this.random.Next(100, 1500);
            Task.Delay(waitTime)
                .Wait();
        }

        [TearDown]
        public void TearDown()
        {
            var waitTime = this.random.Next(50, 100);
            Task.Delay(waitTime)
                .Wait();
        }



        [Test]
        public void GetFlow()
        {
            IEnumerable<ITrack> tracks = this.session.User.GetFlow(this.session.CurrentUserId, CancellationToken.None)
                                                          .Result;

            Assert.IsNotNull(tracks, nameof(tracks));
            Assert.That(tracks.Count(), Is.GreaterThan(0), "Count");

            var firstTrack = tracks.First();
            Assert.IsNotNull(firstTrack, nameof(firstTrack));
            Assert.That(firstTrack.Id, Is.GreaterThan(0), nameof(firstTrack.Id));
            Assert.IsNotNull(firstTrack.Title, nameof(firstTrack.Title));

        }

        /* TODO: Implement
        [Test]
        public void GetPersonalTracks()
        {
            IEnumerable<ITrack> tracks = await _user.GetPersonalTracks();


            Assert.IsNotNull(tracks, nameof(tracks));
            

            if (tracks.Count() == 0)
            {
                Assert.Warn("User doesn't have any personal track.");
            }
            else
            {
                Assert.Fail($"User has {tracks.Count()} personal tracks.");
            }
        }
        */

        [Test]
        public void GetPlaylists()
        {
            IEnumerable<IPlaylist> playlists = this.session.User.GetPlaylists(CancellationToken.None)
                                                                .Result;

            Assert.IsNotNull(playlists, nameof(playlists));
            Assert.That(playlists.Count(), Is.GreaterThan(0), "Count");
        }

        [Test]
        public void GetFavouriteAlbums()
        {
            IEnumerable<IAlbum> albums = this.session.User.GetFavouriteAlbums(this.session.CurrentUserId, CancellationToken.None)
                                                          .Result;

            Assert.IsNotNull(albums, nameof(albums));
            Assert.That(albums.Count(), Is.GreaterThan(0), "Count");
        }

        [Test]
        public void GetFavouriteArtists()
        {
            IEnumerable<IArtist> artists = this.session.User.GetFavouriteArtists(this.session.CurrentUserId, CancellationToken.None)
                                                            .Result;

            Assert.IsNotNull(artists, nameof(artists));
            Assert.That(artists.Count(), Is.GreaterThan(0), "Count");
        }

        [Test]
        public void GetFavouriteTracks()
        {
            IEnumerable<ITrack> tracks = this.session.User.GetFavouriteTracks(this.session.CurrentUserId, CancellationToken.None)
                                                          .Result;
            
            Assert.IsNotNull(tracks, nameof(tracks));
            Assert.That(tracks.Count(), Is.GreaterThan(0), "Count");
        }

        [Test]
        public void GetRecommendedAlbums()
        {
            IEnumerable<IAlbum> albums = this.session.User.GetRecommendedAlbums(CancellationToken.None)
                                                          .Result;

            Assert.IsNotNull(albums, nameof(albums));
            Assert.AreEqual(9, albums.Count(), "Count");

            var firstAlbum = albums.First();
            Assert.IsNotNull(firstAlbum, nameof(firstAlbum));
            Assert.AreEqual(75783062, firstAlbum.Id, nameof(firstAlbum.Id));
            Assert.AreEqual("BYLAW EP", firstAlbum.Title, nameof(firstAlbum.Title));
        }

        [Test]
        public void GetRecommendedArtists()
        {
            IEnumerable<IArtist> artists = this.session.User.GetRecommendedArtists(CancellationToken.None)
                                                            .Result;

            Assert.IsNotNull(artists, nameof(artists));
            Assert.That(artists.Count(), Is.GreaterThan(0), "Count");

            var firstArtist = artists.First();
            Assert.IsNotNull(firstArtist, nameof(firstArtist));
            Assert.That(firstArtist.Id, Is.GreaterThan(0), nameof(firstArtist.Id));
            Assert.IsNotNull(firstArtist.Name, nameof(firstArtist.Name));
        }

        [Test]
        public void GetRecommendedPlaylists()
        {
            IEnumerable<IPlaylist> playlists = this.session.User.GetRecommendedPlaylists(CancellationToken.None)
                                                                .Result;

            Assert.IsNotNull(playlists, nameof(playlists));
            Assert.That(playlists.Count(), Is.GreaterThan(0), "Count");

            var firstPlaylist = playlists.First();
            Assert.IsNotNull(firstPlaylist, nameof(firstPlaylist));
            Assert.That(firstPlaylist.Id, Is.GreaterThan(0), nameof(firstPlaylist.Id));
            Assert.IsNotNull(firstPlaylist.Title, nameof(firstPlaylist.Title));
        }

        [Test]
        public void GetRecommendedTracks()
        {
            IEnumerable<ITrack> tracks = this.session.User.GetRecommendedTracks(CancellationToken.None)
                                                          .Result;
            
            Assert.IsNotNull(tracks, nameof(tracks));
            Assert.That(tracks.Count(), Is.GreaterThan(0), "Count");

            var firstTrack = tracks.First();
            Assert.IsNotNull(firstTrack, nameof(firstTrack));
            Assert.That(firstTrack.Id, Is.GreaterThan(0), nameof(firstTrack.Id));
            Assert.IsNotNull(firstTrack.Title, nameof(firstTrack.Title));
        }

        [Test]
        public void GetRecommendedRadio()
        {
            IEnumerable<IRadio> radios = this.session.User.GetRecommendedRadio(CancellationToken.None)
                                                          .Result;
            
            Assert.IsNotNull(radios, nameof(radios));
            Assert.That(radios.Count(), Is.GreaterThan(0), "Count");

            var firstRadio = radios.First();
            Assert.IsNotNull(firstRadio, nameof(firstRadio));
            Assert.That(firstRadio.Id, Is.GreaterThan(0), nameof(firstRadio.Id));
            Assert.IsNotNull(firstRadio.Title, nameof(firstRadio.Title));
        }
        


        [Test]
        public void TestFavouritingAlbum()
        {
            // Ensure album isn't already favourited..
            this.session.User.UnfavouriteAlbum(this.album, CancellationToken.None)
                             .Wait();

            // Favourite
            Assert.True(this.session.User.FavouriteAlbum(this.album, CancellationToken.None)
                                         .Result);

            var favouriteAlbums = this.session.User.GetFavouriteAlbums(this.session.CurrentUserId, CancellationToken.None, 0, 100)
                                                   .Result;

            Assert.That(favouriteAlbums.Select(x => x.Id)
                                       .Any(x => x == this.album.Id));

            // Test when already added to favourites
            Assert.True(this.session.User.FavouriteAlbum(this.album, CancellationToken.None)
                                         .Result);

            // Then unfavourite
            Assert.True(this.session.User.UnfavouriteAlbum(this.album, CancellationToken.None)
                                         .Result);

            var updatedFavouriteAlbums = this.session.User.GetFavouriteAlbums(this.session.CurrentUserId, CancellationToken.None, 0, 100)
                                                          .Result;

            Assert.AreEqual(0, updatedFavouriteAlbums.Select(x => x.Id)
                                                     .Where(x => x == this.album.Id)
                                                     .Count());
                
            // And test when not already a favorite
            Assert.True(this.session.User.UnfavouriteAlbum(this.album, CancellationToken.None)
                                         .Result);
        }


        [Test]
        public void TestFavouritingArtist()
        {
            // Ensure artist isn't already favourited..
            this.session.User.UnfavouriteArtist(this.artist, CancellationToken.None)
                             .Wait();

            // Favourite
            Assert.True(this.session.User.FavouriteArtist(this.artist, CancellationToken.None)
                                         .Result);

            var favouriteArtists = this.session.User.GetFavouriteArtists(this.session.CurrentUserId, CancellationToken.None, 0, 100)
                                                   .Result;

            Assert.That(favouriteArtists.Select(x => x.Id)
                                       .Any(x => x == this.artist.Id));

            // Test when already added to favourites
            Assert.True(this.session.User.FavouriteArtist(this.artist, CancellationToken.None)
                                         .Result);

            // Then unfavourite
            Assert.True(this.session.User.UnfavouriteArtist(this.artist, CancellationToken.None)
                                         .Result);

            var updatedFavouriteArtists = this.session.User.GetFavouriteArtists(this.session.CurrentUserId, CancellationToken.None, 0, 100)
                                                          .Result;

            Assert.AreEqual(0, updatedFavouriteArtists.Select(x => x.Id)
                                                     .Where(x => x == this.artist.Id)
                                                     .Count());

            // And test when not already a favorite
            Assert.True(this.session.User.UnfavouriteArtist(this.artist, CancellationToken.None)
                                         .Result);
        }


        [Test]
        public void TestFavouritingTrack()
        {
            // Ensure track isn't already favourited..
            this.session.User.UnfavouriteTrack(this.track, CancellationToken.None)
                             .Wait();

            // Favourite
            Assert.True(this.session.User.FavouriteTrack(this.track, CancellationToken.None)
                                         .Result);

            var favouriteTracks = this.session.User.GetFavouriteTracks(this.session.CurrentUserId, CancellationToken.None, 0, 100)
                                                   .Result;

            Assert.That(favouriteTracks.Select(x => x.Id)
                                       .Any(x => x == this.track.Id));

            // Test when already added to favourites
            /* NOTE: Interestingly, this API appears to give back a Deezer exception
             *       if the item already is favourited. None of the other favourite API
             *       calls appear to do this... */

            try
            {
                this.session.User.FavouriteTrack(this.track, CancellationToken.None)
                                 .Wait();

                Assert.Fail("This API should throw if the track is already present in favourite list.");
            }
            catch (AggregateException ex)
            {
                var innerEx = ex.GetBaseException();

                if (innerEx is DeezerException deezerEx)
                    Assert.That(deezerEx.Error.Code == (uint)EDeezerApiError.DataException);

                else
                    throw;           
            }

            // Then unfavourite
            Assert.True(this.session.User.UnfavouriteTrack(this.track, CancellationToken.None)
                                         .Result);

            var updatedFavouriteTracks = this.session.User.GetFavouriteTracks(this.session.CurrentUserId, CancellationToken.None, 0, 100)
                                                          .Result;

            Assert.AreEqual(0, updatedFavouriteTracks.Select(x => x.Id)
                                                     .Where(x => x == this.track.Id)
                                                     .Count());

            // And test when not already a favorite
            Assert.True(this.session.User.UnfavouriteTrack(this.track, CancellationToken.None)
                                         .Result);
        }


        [Test]
        public void TestFavouritingPlaylist()
        {
            // Ensure playlist isn't already favourited..
            this.session.User.UnfavouritePlaylist(this.playlist, CancellationToken.None)
                             .Wait();

            // Favourite
            Assert.True(this.session.User.FavouritePlaylist(this.playlist, CancellationToken.None)
                                         .Result);

            var favouritePlaylists = this.session.User.GetFavouritePlaylists(this.session.CurrentUserId, CancellationToken.None, 0, 100)
                                                   .Result;

            Assert.That(favouritePlaylists.Select(x => x.Id)
                                       .Any(x => x == this.playlist.Id));

            // Test when already added to favourites
            Assert.True(this.session.User.FavouritePlaylist(this.playlist, CancellationToken.None)
                                         .Result);

            // Then unfavourite
            Assert.True(this.session.User.UnfavouritePlaylist(this.playlist, CancellationToken.None)
                                         .Result);

            var updatedFavouritePlaylists = this.session.User.GetFavouritePlaylists(this.session.CurrentUserId, CancellationToken.None, 0, 100)
                                                          .Result;

            Assert.AreEqual(0, updatedFavouritePlaylists.Select(x => x.Id)
                                                     .Where(x => x == this.playlist.Id)
                                                     .Count());

            // And test when not already a favorite
            Assert.True(this.session.User.UnfavouritePlaylist(this.playlist, CancellationToken.None)
                                         .Result);
        }


        [Test]
        public void TestFavouritingRadio()
        {
            // Ensure radio isn't already favourited..
            this.session.User.UnfavouriteRadio(this.radio, CancellationToken.None)
                             .Wait();

            // Favourite
            Assert.True(this.session.User.FavouriteRadio(this.radio, CancellationToken.None)
                                         .Result);

            var favouriteRadios = this.session.User.GetFavouriteRadio(this.session.CurrentUserId, CancellationToken.None, 0, 100)
                                                   .Result;

            Assert.That(favouriteRadios.Select(x => x.Id)
                                       .Any(x => x == this.radio.Id));

            // Test when already added to favourites
            Assert.True(this.session.User.FavouriteRadio(this.radio, CancellationToken.None)
                                         .Result);

            // Then unfavourite
            Assert.True(this.session.User.UnfavouriteRadio(this.radio, CancellationToken.None)
                                         .Result);

            var updatedFavouriteRadios = this.session.User.GetFavouriteRadio(this.session.CurrentUserId, CancellationToken.None, 0, 100)
                                                          .Result;

            Assert.AreEqual(0, updatedFavouriteRadios.Select(x => x.Id)
                                                     .Where(x => x == this.radio.Id)
                                                     .Count());

            // And test when not already a favorite
            Assert.True(this.session.User.UnfavouriteRadio(this.radio, CancellationToken.None)
                                         .Result);
        }
    }
}
