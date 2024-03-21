using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using NUnit.Framework;

using E.Deezer.Api;
using NUnit.Framework.Legacy;

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

            ClassicAssert.IsNotNull(chart, nameof(chart));

            ClassicAssert.IsNotNull(chart.Tracks, nameof(chart.Tracks));
            ClassicAssert.AreEqual(100, chart.Tracks.Count(), "Tracks.Count");

            ClassicAssert.IsNotNull(chart.Albums, nameof(chart.Albums));
            ClassicAssert.AreEqual(99, chart.Albums.Count(), "Albums.Count");

            ClassicAssert.IsNotNull(chart.Artists, nameof(chart.Artists));
            ClassicAssert.AreEqual(100, chart.Artists.Count(), "Artists.Count");

            ClassicAssert.IsNotNull(chart.Playlists, nameof(chart.Playlists));
            ClassicAssert.AreEqual(100, chart.Playlists.Count(), "Playlists.Count");
        }

        [Test]
        public void GetAlbumChart()
        {
            handler.Content = base.GetServerResponse("album");

            IEnumerable<IAlbum> albums = session.Charts.GetAlbumChart(CancellationToken.None)
                                                       .Result;

            ClassicAssert.IsNotNull(albums, nameof(albums));
            ClassicAssert.AreEqual(99, albums.Count(), "Count");

            var firstAlbum = albums.First();
            ClassicAssert.IsNotNull(firstAlbum, nameof(firstAlbum));
            ClassicAssert.AreEqual(97400042, firstAlbum.Id, nameof(firstAlbum.Id));
            ClassicAssert.AreEqual("My Songs (Deluxe)", firstAlbum.Title, nameof(firstAlbum.Title));
        }

        [Test]
        public void GetArtistChart()
        {
            handler.Content = base.GetServerResponse("artist");

            IEnumerable<IArtist> artists = session.Charts.GetArtistChart(CancellationToken.None)
                                                         .Result;

            ClassicAssert.IsNotNull(artists, nameof(artists));
            ClassicAssert.AreEqual(100, artists.Count(), "Count");

            var firstArtist = artists.First();
            ClassicAssert.IsNotNull(firstArtist, nameof(firstArtist));
            ClassicAssert.AreEqual(11800683, firstArtist.Id, nameof(firstArtist.Id));
            ClassicAssert.AreEqual("Follow The Flow", firstArtist.Name, nameof(firstArtist.Name));
        }

        [Test]
        public void GetPlaylistChart()
        {
            handler.Content = base.GetServerResponse("playlist");

            IEnumerable<IPlaylist> playlists = session.Charts.GetPlaylistChart(CancellationToken.None)
                                                             .Result;

            ClassicAssert.IsNotNull(playlists, nameof(playlists));
            ClassicAssert.AreEqual(100, playlists.Count(), "Count");

            var firstPlaylist = playlists.First();
            ClassicAssert.IsNotNull(firstPlaylist, nameof(firstPlaylist));
            ClassicAssert.AreEqual(5522964682, firstPlaylist.Id, nameof(firstPlaylist.Id));
            ClassicAssert.AreEqual("Nyári slágerek 2019", firstPlaylist.Title, nameof(firstPlaylist.Title));
        }

        [Test]
        public void GetTrackChart()
        {
            handler.Content = base.GetServerResponse("track");

            IEnumerable<ITrack> tracks = session.Charts.GetTrackChart(CancellationToken.None)
                                                       .Result;

            ClassicAssert.IsNotNull(tracks, nameof(tracks));
            ClassicAssert.AreEqual(100, tracks.Count(), "Count");

            var firstTrack = tracks.First();
            ClassicAssert.IsNotNull(firstTrack, nameof(firstTrack));
            ClassicAssert.AreEqual(698905582, firstTrack.Id, nameof(firstTrack.Id));
            ClassicAssert.AreEqual("Señorita", firstTrack.Title, nameof(firstTrack.Title));
        }

        [Test]
        public void GetPodcastChart()
        {
            handler.Content = base.GetServerResponse("podcast");

            var podcasts = session.Charts.GetPodcastChart(CancellationToken.None)
                                                       .Result;

            ClassicAssert.IsNotNull(podcasts, nameof(podcasts));
            ClassicAssert.AreEqual(10, podcasts.Count(), "Count");

            var firstPodcast = podcasts.First();
            ClassicAssert.IsNotNull(firstPodcast, nameof(firstPodcast));
            ClassicAssert.AreEqual(2888112, firstPodcast.Id, nameof(firstPodcast.Id));
            ClassicAssert.AreEqual("The Rest Is History", firstPodcast.Title, nameof(firstPodcast.Title));
        }
    }
}
