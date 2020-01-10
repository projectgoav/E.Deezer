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

        private readonly DeezerSession session;

        private IUser user;
        private IRadio radio;
        private IAlbum album;
        private ITrack track;
        private IArtist artist;
        private IPlaylist playlist;

        public UserEndpointLiveApiTests()
        {
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

        /*
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

            var firstPlaylist = playlists.First();
            Assert.IsNotNull(firstPlaylist, nameof(firstPlaylist));
            Assert.That(firstPlaylist.Id, Is.GreaterThan(0), nameof(firstPlaylist.Id));
            Assert.IsNotNull(firstPlaylist.Title, nameof(firstPlaylist.Title));
        }

        [Test]
        public void GetFavouriteAlbums()
        {
            IEnumerable<IAlbum> albums = this.session.User.GetFavouriteAlbums(this.session.CurrentUserId, CancellationToken.None)
                                                          .Result;

            Assert.IsNotNull(albums, nameof(albums));
            Assert.AreEqual(11, albums.Count(), "Count");

            var firstAlbum = albums.First();
            Assert.IsNotNull(firstAlbum, nameof(firstAlbum));
            Assert.AreEqual(6063443, firstAlbum.Id, nameof(firstAlbum.Id));
            Assert.AreEqual("Mainstage, Vol. 1", firstAlbum.Title, nameof(firstAlbum.Title));
        }

        [Test]
        public void GetFavouriteArtists()
        {
            IEnumerable<IArtist> artists = this.session.User.GetFavouriteArtists(this.session.CurrentUserId, CancellationToken.None)
                                                            .Result;

            Assert.IsNotNull(artists, nameof(artists));
            Assert.That(artists.Count(), Is.GreaterThan(0), "Count");

            var firstArtist = artists.First();
            Assert.IsNotNull(firstArtist, nameof(firstArtist));
            Assert.That(firstArtist.Id, Is.GreaterThan(0), nameof(firstArtist.Id));
            Assert.IsNotNull(firstArtist.Name, nameof(firstArtist.Name));
        }

        [Test]
        public void GetFavouriteTracks()
        {
            IEnumerable<ITrack> tracks = this.session.User.GetFavouriteTracks(this.session.CurrentUserId, CancellationToken.None)
                                                          .Result;
            
            Assert.IsNotNull(tracks, nameof(tracks));
            Assert.AreEqual(7, tracks.Count(), "Count");

            var firstTrack = tracks.First();
            Assert.IsNotNull(firstTrack, nameof(firstTrack));
            Assert.AreEqual(137745477, firstTrack.Id, nameof(firstTrack.Id));
            Assert.AreEqual("Arcade", firstTrack.Title, nameof(firstTrack.Title));
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
        

        /*
        [Test, Order(1)]
        public async Task AddAlbumToFavourite()
        {
            bool response = await _user.AddAlbumToFavourite(_album);

            Assert.IsTrue(response);
        }

        [Test, Order(2)]
        public async Task AddAlreadyAddedAlbumToFavourites()
        {
            bool response = await _album.AddAlbumToFavorite();

            Assert.IsTrue(response);
        }

        [Test, Order(3)]
        public async Task RemoveAlbumFromFavourite()
        {
            bool response = await _user.RemoveAlbumFromFavourite(_album);

            Assert.IsTrue(response);
        }

        [Test, Order(4)]
        public async Task RemoveAlreadyRemovedAlbumFromFavourites()
        {
            bool response = await _album.RemoveAlbumFromFavorite();

            Assert.IsTrue(response);
        }


        [Test, Order(5)]
        public async Task AddArtistToFavourite()
        {
            bool response = await _user.AddArtistToFavourite(_artist);

            Assert.IsTrue(response);
        }

        [Test, Order(6)]
        public async Task AddAlreadyAddedArtistToFavourites()
        {
            bool response = await _artist.AddArtistToFavorite();

            Assert.IsTrue(response);
        }

        [Test, Order(7)]
        public async Task RemoveArtistFromFavourite()
        {
            bool response = await _user.RemoveArtistFromFavourite(_artist);

            Assert.IsTrue(response);
        }

        [Test, Order(8)]
        public async Task RemoveAlreadyRemovedArtistFromFavourites()
        {
            bool response = await _artist.RemoveArtistFromFavorite();

            Assert.IsTrue(response);
        }


        [Test, Order(9)]
        public async Task AddTrackToFavourite()
        {
            bool response = await _user.AddTrackToFavourite(_track);

            Assert.IsTrue(response);
        }

        [Test, Order(10)]
        public async Task AddAlreadyAddedTrackToFavourites()
        {
            bool response = await _track.AddTrackToFavorite();

            Assert.IsTrue(response);
        }

        [Test, Order(11)]
        public async Task RemoveTrackFromFavourite()
        {
            bool response = await _user.RemoveTrackFromFavourite(_track);

            Assert.IsTrue(response);
        }

        [Test, Order(12)]
        public async Task RemoveAlreadyRemovedTrackFromFavourites()
        {
            bool response = await _track.RemoveTrackFromFavorite();

            Assert.IsTrue(response);
        }


        [Test, Order(13)]
        public async Task AddPlaylistToFavourite()
        {
            bool response = await _user.AddPlaylistToFavourite(_playlist);

            Assert.IsTrue(response);
        }

        [Test, Order(14)]
        public async Task AddAlreadyAddedPlaylistToFavourites()
        {
            bool response = await _playlist.AddPlaylistToFavorite();

            Assert.IsTrue(response);
        }

        [Test, Order(15)]
        public async Task RemovePlaylistFromFavourite()
        {
            bool response = await _user.RemovePlaylistFromFavourite(_playlist);

            Assert.IsTrue(response);
        }

        [Test, Order(16)]
        public async Task RemoveAlreadyRemovedPlaylistFromFavourites()
        {
            bool response = await _playlist.RemovePlaylistFromFavorite();

            Assert.IsTrue(response);
        }


        [Test, Order(17)]
        public async Task AddRadioToFavourite()
        {
            bool response = await _user.AddRadioToFavourite(_radio);

            Assert.IsTrue(response);
        }

        [Test, Order(18)]
        public async Task AddAlreadyAddedRadioToFavourites()
        {
            bool response = await _radio.AddRadioToFavorite();

            Assert.IsTrue(response);
        }

        [Test, Order(19)]
        public async Task RemoveRadioFromFavourite()
        {
            bool response = await _user.RemoveRadioFromFavourite(_radio);

            Assert.IsTrue(response);
        }

        [Test, Order(20)]
        public async Task RemoveAlreadyRemovedRadioFromFavourites()
        {
            bool response = await _radio.RemoveRadioFromFavorite();

            Assert.IsTrue(response);
        }
        */
    }
}
