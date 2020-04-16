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
    public class SearchEndpointLiveApiTests : IDisposable
    {
        private readonly DeezerSession session;

        
        public SearchEndpointLiveApiTests()
        {
            this.session = new DeezerSession(new HttpClientHandler());
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



        [Test]
        public void Albums()
        {
            const string SEARCH_TERM = "eminem";

            IEnumerable<IAlbum> albums = this.session.Search.FindAlbums(SEARCH_TERM, CancellationToken.None)
                                                            .Result;


            Assert.IsNotNull(albums, nameof(albums));
            Assert.That(albums.Count(), Is.GreaterThan(0), "Count");

            var firstAlbum = albums.First();
            Assert.IsNotNull(firstAlbum, nameof(firstAlbum));
            Assert.That(firstAlbum.Id, Is.GreaterThan(0), nameof(firstAlbum.Id));
            Assert.IsNotNull(firstAlbum.Title, nameof(firstAlbum.Title));
        }

        [Test]
        public void Artists()
        {
            const string SEARCH_TERM = "eminem";

            IEnumerable<IArtist> artists = this.session.Search.FindArtists(SEARCH_TERM, CancellationToken.None)
                                                              .Result;

            Assert.IsNotNull(artists, nameof(artists));
            Assert.That(artists.Count(), Is.GreaterThan(0), "Count");

            var firstArtist = artists.First();
            Assert.IsNotNull(firstArtist, nameof(firstArtist));
            Assert.That(firstArtist.Id, Is.GreaterThan(0), nameof(firstArtist.Id));
            Assert.IsNotNull(firstArtist.Name, nameof(firstArtist.Name));
        }

        [Test]
        public void Playlists()
        {
            const string SEARCH_TERM = "eminem";

            IEnumerable<IPlaylist> playlists = this.session.Search.FindPlaylists(SEARCH_TERM, CancellationToken.None)
                                                                  .Result;

            Assert.IsNotNull(playlists, nameof(playlists));
            Assert.That(playlists.Count(), Is.GreaterThan(0), "Count");

            var firstPlaylist = playlists.First();
            Assert.IsNotNull(firstPlaylist, nameof(firstPlaylist));
            Assert.That(firstPlaylist.Id, Is.GreaterThan(0), nameof(firstPlaylist.Id));
            Assert.IsNotNull(firstPlaylist.Title, nameof(firstPlaylist.Title));
        }

        [Test]
        public void Tracks()
        {
            const string SEARCH_TERM = "eminem";

            IEnumerable<ITrack> tracks = this.session.Search.FindTracks(SEARCH_TERM, CancellationToken.None)
                                                            .Result;

            Assert.IsNotNull(tracks, nameof(tracks));
            Assert.That(tracks.Count(), Is.GreaterThan(0), "Count");

            var firstTrack = tracks.First();
            Assert.IsNotNull(firstTrack, nameof(firstTrack));
            Assert.That(firstTrack.Id, Is.GreaterThan(0), nameof(firstTrack.Id));
            Assert.IsNotNull(firstTrack.Title, nameof(firstTrack.Title));
        }

        [Test]
        public void Radio()
        {
            const string SEARCH_TERM = "electro";

            IEnumerable<IRadio> radios = this.session.Search.FindRadio(SEARCH_TERM, CancellationToken.None)
                                                            .Result;
            
            Assert.IsNotNull(radios, nameof(radios));
            Assert.That(radios.Count(), Is.GreaterThan(0), "Count");

            var firstRadio = radios.First();
            Assert.IsNotNull(firstRadio, nameof(firstRadio));
            Assert.That(firstRadio.Id, Is.GreaterThan(0), nameof(firstRadio.Id));
            Assert.IsNotNull(firstRadio.Title, nameof(firstRadio.Title));
        }

        [Test]
        public void User()
        {
            const string SEARCH_TERM = "ddgh";

            IEnumerable<IUserProfile> users = this.session.Search.FindUsers(SEARCH_TERM, CancellationToken.None)
                                                                 .Result;
            
            Assert.IsNotNull(users, nameof(users));
            Assert.That(users.Count(), Is.GreaterThan(0), "Count");

            var firstUser = users.First();
            Assert.IsNotNull(firstUser, nameof(firstUser));
            Assert.That(firstUser.Id, Is.GreaterThan(0), nameof(firstUser.Id));
            Assert.IsNotNull(firstUser.Username, nameof(firstUser.Username));
        }

        /*
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
        */

        [Test]
        public void TestGithubIssue79()
        {
            const string SEARCH_TERM = "Impronta";

            // If there is a regression then this search result will fail to parse.
            // For further information see the Github issue.
            var result = this.session.Search.FindAlbums(SEARCH_TERM, CancellationToken.None, start: 200, count: 10)
                                            .Result;
        }
    }
}
