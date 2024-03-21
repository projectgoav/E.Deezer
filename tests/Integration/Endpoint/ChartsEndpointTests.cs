using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using NUnit.Framework;

using E.Deezer.Api;

namespace E.Deezer.Tests.Integration.Endpoint
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ChartsEndpointTests : TestClassBase, IDisposable
    {
        private readonly OfflineMessageHandler handler;
        private readonly DeezerSession session;

        public ChartsEndpointTests()
            : base("ChartsEndpoint")
        {
            this.handler = new OfflineMessageHandler();

            this.session = new DeezerSession(this.handler);

        }


        // IDisposable
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
        public void GetChart()
        {
            handler.Content = base.GetServerResponse("chart");

            IChart chart = session.Charts.GetCharts(CancellationToken.None)
                                         .Result;

            Assert.IsNotNull(chart, nameof(chart));

            Assert.IsNotNull(chart.Tracks, nameof(chart.Tracks));
            Assert.AreEqual(100, chart.Tracks.Count(), "Tracks.Count");

            Assert.IsNotNull(chart.Albums, nameof(chart.Albums));
            Assert.AreEqual(99, chart.Albums.Count(), "Albums.Count");

            Assert.IsNotNull(chart.Artists, nameof(chart.Artists));
            Assert.AreEqual(100, chart.Artists.Count(), "Artists.Count");

            Assert.IsNotNull(chart.Playlists, nameof(chart.Playlists));
            Assert.AreEqual(100, chart.Playlists.Count(), "Playlists.Count");
        }

        [Test]
        public void GetAlbumChart()
        {
            handler.Content = base.GetServerResponse("album");

            IEnumerable<IAlbum> albums = session.Charts.GetAlbumChart(CancellationToken.None)
                                                       .Result;

            Assert.IsNotNull(albums, nameof(albums));
            Assert.AreEqual(99, albums.Count(), "Count");

            var firstAlbum = albums.First();
            Assert.IsNotNull(firstAlbum, nameof(firstAlbum));
            Assert.AreEqual(97400042, firstAlbum.Id, nameof(firstAlbum.Id));
            Assert.AreEqual("My Songs (Deluxe)", firstAlbum.Title, nameof(firstAlbum.Title));
        }

        [Test]
        public void GetArtistChart()
        {
            handler.Content = base.GetServerResponse("artist");

            IEnumerable<IArtist> artists = session.Charts.GetArtistChart(CancellationToken.None)
                                                         .Result;

            Assert.IsNotNull(artists, nameof(artists));
            Assert.AreEqual(100, artists.Count(), "Count");

            var firstArtist = artists.First();
            Assert.IsNotNull(firstArtist, nameof(firstArtist));
            Assert.AreEqual(11800683, firstArtist.Id, nameof(firstArtist.Id));
            Assert.AreEqual("Follow The Flow", firstArtist.Name, nameof(firstArtist.Name));
        }

        [Test]
        public void GetPlaylistChart()
        {
            handler.Content = base.GetServerResponse("playlist");

            IEnumerable<IPlaylist> playlists = session.Charts.GetPlaylistChart(CancellationToken.None)
                                                             .Result;

            Assert.IsNotNull(playlists, nameof(playlists));
            Assert.AreEqual(100, playlists.Count(), "Count");

            var firstPlaylist = playlists.First();
            Assert.IsNotNull(firstPlaylist, nameof(firstPlaylist));
            Assert.AreEqual(5522964682, firstPlaylist.Id, nameof(firstPlaylist.Id));
            Assert.AreEqual("Nyári slágerek 2019", firstPlaylist.Title, nameof(firstPlaylist.Title));
        }

        [Test]
        public void GetTrackChart()
        {
            handler.Content = base.GetServerResponse("track");

            IEnumerable<ITrack> tracks = session.Charts.GetTrackChart(CancellationToken.None)
                                                       .Result;

            Assert.IsNotNull(tracks, nameof(tracks));
            Assert.AreEqual(100, tracks.Count(), "Count");

            var firstTrack = tracks.First();
            Assert.IsNotNull(firstTrack, nameof(firstTrack));
            Assert.AreEqual(698905582, firstTrack.Id, nameof(firstTrack.Id));
            Assert.AreEqual("Señorita", firstTrack.Title, nameof(firstTrack.Title));
        }

        [Test]
        public void GetPodcastChart()
        {
            handler.Content = base.GetServerResponse("podcast");

            var podcasts = session.Charts.GetPodcastChart(CancellationToken.None)
                                                       .Result;

            Assert.IsNotNull(podcasts, nameof(podcasts));
            Assert.AreEqual(10, podcasts.Count(), "Count");

            var firstPodcast = podcasts.First();
            Assert.IsNotNull(firstPodcast, nameof(firstPodcast));
            Assert.AreEqual(2888112, firstPodcast.Id, nameof(firstPodcast.Id));
            Assert.AreEqual("The Rest Is History", firstPodcast.Title, nameof(firstPodcast.Title));
        }
    }
}
