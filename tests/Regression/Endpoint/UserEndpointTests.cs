using E.Deezer.Api;
using E.Deezer.Endpoint;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E.Deezer.Tests.Regression.Endpoint
{
    [TestFixture]
    class UserEndpointTests
    {
        private static IAlbum _album;
        private static IArtist _artist;
        private static ITrack _track;
        private static IPlaylist _playlist;
        private static IRadio _radio;
        private static IUserEndpoint _user;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var deezer = DeezerSession.CreateNew();
            string token = "frmFoXgyyO1ATzluA6gZIFIoWAf8b8G4tGWHaoxtDN9oCKMghM";

            if (string.IsNullOrEmpty(token))
                throw new NotLoggedInException();

            deezer.Login(token)
                .GetAwaiter().GetResult();

            _user = deezer.User;

            _album = deezer.Browse.GetAlbumById(302127)
                .GetAwaiter().GetResult();

            _artist = deezer.Browse.GetArtistById(27)
                .GetAwaiter().GetResult();

            _track = deezer.Browse.GetTrackById(3135556)
                .GetAwaiter().GetResult();

            _playlist = deezer.Browse.GetPlaylistById(908622995)
                .GetAwaiter().GetResult();

            _radio = deezer.Browse.GetRadioById(6)
                .GetAwaiter().GetResult();
        }

        [Test]
        public async Task GetHistory()
        {
            IEnumerable<ITrack> tracks = await _user.GetHistory();


            Assert.IsNotNull(tracks, nameof(tracks));
            Assert.AreEqual(13, tracks.Count(), "Count");

            var firstTrack = tracks.First();
            Assert.IsNotNull(firstTrack, nameof(firstTrack));
            Assert.AreEqual(85963055, firstTrack.Id, nameof(firstTrack.Id));
            Assert.AreEqual("Waves (Tomorrowland 2014 Anthem) (Radio Edit)", firstTrack.Title, nameof(firstTrack.Title));
        }

        [Test]
        public async Task GetFlow()
        {
            IEnumerable<ITrack> tracks = await _user.GetFlow();


            Assert.IsNotNull(tracks, nameof(tracks));
            Assert.That(tracks.Count(), Is.GreaterThan(0), "Count");

            var firstTrack = tracks.First();
            Assert.IsNotNull(firstTrack, nameof(firstTrack));
            Assert.That(firstTrack.Id, Is.GreaterThan(0), nameof(firstTrack.Id));
            Assert.IsNotNull(firstTrack.Title, nameof(firstTrack.Title));

        }

        [Test]
        public async Task GetPersonalTracks()
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

        [Test]
        public async Task GetPlaylists()
        {
            IEnumerable<IPlaylist> playlists = await _user.GetPlaylists();


            Assert.IsNotNull(playlists, nameof(playlists));
            Assert.That(playlists.Count(), Is.GreaterThan(0), "Count");

            var firstPlaylist = playlists.First();
            Assert.IsNotNull(firstPlaylist, nameof(firstPlaylist));
            Assert.That(firstPlaylist.Id, Is.GreaterThan(0), nameof(firstPlaylist.Id));
            Assert.IsNotNull(firstPlaylist.Title, nameof(firstPlaylist.Title));
        }

        [Test]
        public async Task GetFavouriteAlbums()
        {
            IEnumerable<IAlbum> albums = await _user.GetFavouriteAlbums();


            Assert.IsNotNull(albums, nameof(albums));
            Assert.AreEqual(11, albums.Count(), "Count");

            var firstAlbum = albums.First();
            Assert.IsNotNull(firstAlbum, nameof(firstAlbum));
            Assert.AreEqual(6063443, firstAlbum.Id, nameof(firstAlbum.Id));
            Assert.AreEqual("Mainstage, Vol. 1", firstAlbum.Title, nameof(firstAlbum.Title));
        }

        [Test]
        public async Task GetFavouriteArtists()
        {
            IEnumerable<IArtist> artists = await _user.GetFavouriteArtists();


            Assert.IsNotNull(artists, nameof(artists));
            Assert.That(artists.Count(), Is.GreaterThan(0), "Count");

            var firstArtist = artists.First();
            Assert.IsNotNull(firstArtist, nameof(firstArtist));
            Assert.That(firstArtist.Id, Is.GreaterThan(0), nameof(firstArtist.Id));
            Assert.IsNotNull(firstArtist.Name, nameof(firstArtist.Name));
        }

        [Test]
        public async Task GetFavouriteTracks()
        {
            var actual = await _user.GetFavouriteTracks();
            IEnumerable<ITrack> tracks = await _user.GetFavouriteTracks();


            Assert.IsNotNull(tracks, nameof(tracks));
            Assert.AreEqual(7, tracks.Count(), "Count");

            var firstTrack = tracks.First();
            Assert.IsNotNull(firstTrack, nameof(firstTrack));
            Assert.AreEqual(137745477, firstTrack.Id, nameof(firstTrack.Id));
            Assert.AreEqual("Arcade", firstTrack.Title, nameof(firstTrack.Title));
        }

        [Test]
        public async Task GetRecommendedAlbums()
        {
            IEnumerable<IAlbum> albums = await _user.GetRecommendedAlbums();


            Assert.IsNotNull(albums, nameof(albums));
            Assert.AreEqual(9, albums.Count(), "Count");

            var firstAlbum = albums.First();
            Assert.IsNotNull(firstAlbum, nameof(firstAlbum));
            Assert.AreEqual(75783062, firstAlbum.Id, nameof(firstAlbum.Id));
            Assert.AreEqual("BYLAW EP", firstAlbum.Title, nameof(firstAlbum.Title));
        }

        [Test]
        public async Task GetRecommendedArtists()
        {
            IEnumerable<IArtist> artists = await _user.GetRecommendedArtists();


            Assert.IsNotNull(artists, nameof(artists));
            Assert.That(artists.Count(), Is.GreaterThan(0), "Count");

            var firstArtist = artists.First();
            Assert.IsNotNull(firstArtist, nameof(firstArtist));
            Assert.That(firstArtist.Id, Is.GreaterThan(0), nameof(firstArtist.Id));
            Assert.IsNotNull(firstArtist.Name, nameof(firstArtist.Name));
        }

        [Test]
        public async Task GetRecommendedPlaylists()
        {
            IEnumerable<IPlaylist> playlists = await _user.GetRecommendedPlaylists();


            Assert.IsNotNull(playlists, nameof(playlists));
            Assert.That(playlists.Count(), Is.GreaterThan(0), "Count");

            var firstPlaylist = playlists.First();
            Assert.IsNotNull(firstPlaylist, nameof(firstPlaylist));
            Assert.That(firstPlaylist.Id, Is.GreaterThan(0), nameof(firstPlaylist.Id));
            Assert.IsNotNull(firstPlaylist.Title, nameof(firstPlaylist.Title));
        }

        [Test]
        public async Task GetRecommendedTracks()
        {
            IEnumerable<ITrack> tracks = await _user.GetRecommendedTracks();


            Assert.IsNotNull(tracks, nameof(tracks));
            Assert.That(tracks.Count(), Is.GreaterThan(0), "Count");

            var firstTrack = tracks.First();
            Assert.IsNotNull(firstTrack, nameof(firstTrack));
            Assert.That(firstTrack.Id, Is.GreaterThan(0), nameof(firstTrack.Id));
            Assert.IsNotNull(firstTrack.Title, nameof(firstTrack.Title));
        }

        [Test]
        public async Task GetRecommendedRadio()
        {
            IEnumerable<IRadio> radios = await _user.GetRecommendedRadio();


            Assert.IsNotNull(radios, nameof(radios));
            Assert.That(radios.Count(), Is.GreaterThan(0), "Count");

            var firstRadio = radios.First();
            Assert.IsNotNull(firstRadio, nameof(firstRadio));
            Assert.That(firstRadio.Id, Is.GreaterThan(0), nameof(firstRadio.Id));
            Assert.IsNotNull(firstRadio.Title, nameof(firstRadio.Title));
        }
        

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
            bool response = await _user.AddTrackToFavourite(_track.Id);

            Assert.IsTrue(response);
            Assert.Warn($"Task<bool> AddTrackToFavourite(ITrack aTrack); method is missing from the interface IUserEndpoint!");
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
            bool response = await _user.RemoveTrackFromFavourite(_track.Id);

            Assert.IsTrue(response);
            Assert.Warn($"Task<bool> RemoveTrackFromFavourite(ITrack aTrack); method is missing from the interface IUserEndpoint!");
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
    }
}
