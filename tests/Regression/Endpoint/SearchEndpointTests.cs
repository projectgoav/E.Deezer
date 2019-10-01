using E.Deezer.Api;
using E.Deezer.Endpoint;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E.Deezer.Tests.Regression.Endpoint
{
    [TestFixture]
    class SearchEndpointTests
    {
        private static readonly string _searchQuery = "eminem";
        private static ISearchEndpoint _search;

        [OneTimeSetUp]
        public static void OneTimeSetUp()
        {
            _search = DeezerSession.CreateNew().Search;
        }

        [Test]
        public async Task Albums()
        {
            IEnumerable<IAlbum> albums = await _search.Albums(_searchQuery);


            Assert.IsNotNull(albums, nameof(albums));
            Assert.That(albums.Count(), Is.GreaterThan(0), "Count");

            var firstAlbum = albums.First();
            Assert.IsNotNull(firstAlbum, nameof(firstAlbum));
            Assert.That(firstAlbum.Id, Is.GreaterThan(0), nameof(firstAlbum.Id));
            Assert.IsNotNull(firstAlbum.Title, nameof(firstAlbum.Title));
        }

        [Test]
        public async Task Artists()
        {
            IEnumerable<IArtist> artists = await _search.Artists(_searchQuery);


            Assert.IsNotNull(artists, nameof(artists));
            Assert.That(artists.Count(), Is.GreaterThan(0), "Count");

            var firstArtist = artists.First();
            Assert.IsNotNull(firstArtist, nameof(firstArtist));
            Assert.That(firstArtist.Id, Is.GreaterThan(0), nameof(firstArtist.Id));
            Assert.IsNotNull(firstArtist.Name, nameof(firstArtist.Name));
        }

        [Test]
        public async Task Playlists()
        {
            IEnumerable<IPlaylist> playlists = await _search.Playlists(_searchQuery);


            Assert.IsNotNull(playlists, nameof(playlists));
            Assert.That(playlists.Count(), Is.GreaterThan(0), "Count");

            var firstPlaylist = playlists.First();
            Assert.IsNotNull(firstPlaylist, nameof(firstPlaylist));
            Assert.That(firstPlaylist.Id, Is.GreaterThan(0), nameof(firstPlaylist.Id));
            Assert.IsNotNull(firstPlaylist.Title, nameof(firstPlaylist.Title));
        }

        [Test]
        public async Task Tracks()
        {
            IEnumerable<ITrack> tracks = await _search.Tracks(_searchQuery);


            Assert.IsNotNull(tracks, nameof(tracks));
            Assert.That(tracks.Count(), Is.GreaterThan(0), "Count");

            var firstTrack = tracks.First();
            Assert.IsNotNull(firstTrack, nameof(firstTrack));
            Assert.That(firstTrack.Id, Is.GreaterThan(0), nameof(firstTrack.Id));
            Assert.IsNotNull(firstTrack.Title, nameof(firstTrack.Title));
        }

        [Test]
        public async Task Radio()
        {
            IEnumerable<IRadio> radios = await _search.Radio("Electro");


            Assert.IsNotNull(radios, nameof(radios));
            Assert.That(radios.Count(), Is.GreaterThan(0), "Count");

            var firstRadio = radios.First();
            Assert.IsNotNull(firstRadio, nameof(firstRadio));
            Assert.That(firstRadio.Id, Is.GreaterThan(0), nameof(firstRadio.Id));
            Assert.IsNotNull(firstRadio.Title, nameof(firstRadio.Title));
        }

        [Test]
        public async Task User()
        {
            IEnumerable<IUser> users = await _search.User(_searchQuery);


            Assert.IsNotNull(users, nameof(users));
            Assert.That(users.Count(), Is.GreaterThan(0), "Count");

            var firstUser = users.First();
            Assert.IsNotNull(firstUser, nameof(firstUser));
            Assert.That(firstUser.Id, Is.GreaterThan(0), nameof(firstUser.Id));
            Assert.IsNotNull(firstUser.Name, nameof(firstUser.Name));
        }

        [Test]
        public async Task Advanced()
        {
            IEnumerable<ITrack> tracks = await _search.Advanced(_searchQuery);


            Assert.IsNotNull(tracks, nameof(tracks));
            Assert.That(tracks.Count(), Is.GreaterThan(0), "Count");

            var firstTrack = tracks.First();
            Assert.IsNotNull(firstTrack, nameof(firstTrack));
            Assert.That(firstTrack.Id, Is.GreaterThan(0), nameof(firstTrack.Id));
            Assert.IsNotNull(firstTrack.Title, nameof(firstTrack.Title));
        }

        [Test]
        public void TestGithubIssue79()
        {
            /* If there is a regression, then this should fail. See github comments. */
            var result = _search.Albums("Impronta", 200).Result;
        }
    }
}
