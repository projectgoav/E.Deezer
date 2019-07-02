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
        private static IUserEndpoint _user;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var deezer = DeezerSession.CreateNew();
            string token = "";

            if (string.IsNullOrEmpty(token))
                throw new NotLoggedInException();

            deezer.Login(token)
                .GetAwaiter().GetResult();

            _user = deezer.User;
        }

        [Test]
        public async Task GetHistory()
        {
            IEnumerable<ITrack> tracks = await _user.GetHistory();


            Assert.IsNotNull(tracks, nameof(tracks));
            Assert.That(tracks.Count(), Is.GreaterThan(0), "Count");

            var firstTrack = tracks.First();
            Assert.IsNotNull(firstTrack, nameof(firstTrack));
            Assert.That(firstTrack.Id, Is.GreaterThan(0), nameof(firstTrack.Id));
            Assert.IsNotNull(firstTrack.Title, nameof(firstTrack.Title));
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
            Assert.That(tracks.Count(), Is.GreaterThan(0), "Count");

            var firstTrack = tracks.First();
            Assert.IsNotNull(firstTrack, nameof(firstTrack));
            Assert.That(firstTrack.Id, Is.GreaterThan(0), nameof(firstTrack.Id));
            Assert.IsNotNull(firstTrack.Title, nameof(firstTrack.Title));
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
            Assert.That(albums.Count(), Is.GreaterThan(0), "Count");

            var firstAlbum = albums.First();
            Assert.IsNotNull(firstAlbum, nameof(firstAlbum));
            Assert.That(firstAlbum.Id, Is.GreaterThan(0), nameof(firstAlbum.Id));
            Assert.IsNotNull(firstAlbum.Title, nameof(firstAlbum.Title));
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
            Assert.That(tracks.Count(), Is.GreaterThan(0), "Count");

            var firstTrack = tracks.First();
            Assert.IsNotNull(firstTrack, nameof(firstTrack));
            Assert.That(firstTrack.Id, Is.GreaterThan(0), nameof(firstTrack.Id));
            Assert.IsNotNull(firstTrack.Title, nameof(firstTrack.Title));
        }

        [Test]
        public async Task GetRecommendedAlbums()
        {
            IEnumerable<IAlbum> albums = await _user.GetRecommendedAlbums();


            Assert.IsNotNull(albums, nameof(albums));
            Assert.That(albums.Count(), Is.GreaterThan(0), "Count");

            var firstAlbum = albums.First();
            Assert.IsNotNull(firstAlbum, nameof(firstAlbum));
            Assert.That(firstAlbum.Id, Is.GreaterThan(0), nameof(firstAlbum.Id));
            Assert.IsNotNull(firstAlbum.Title, nameof(firstAlbum.Title));
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
    }
}
