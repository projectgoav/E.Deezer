﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Net.Http; 
using System.Threading;

using NUnit.Framework;
using NUnit.Framework.Legacy;

using E.Deezer.Api;

namespace E.Deezer.Tests.Regression.Endpoint
{
#if LIVE_API_TEST
    [TestFixture]
#else
    [Ignore("Live API tests not enabled for this configuration")]
#endif
    public class ChartsEndpointLiveApiTests : IDisposable
    {
        private readonly DeezerSession session;


        public ChartsEndpointLiveApiTests()
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


        [TestCase(true)]
        [TestCase(false)]
        public void GetChart(bool useGenreFilter)
        {
            IChart chart = useGenreFilter ? this.session.Charts.GetChartsForGenre(0, CancellationToken.None).Result
                                          : this.session.Charts.GetCharts(CancellationToken.None).Result;
                
            ClassicAssert.IsNotNull(chart, nameof(chart));

            ClassicAssert.IsNotNull(chart.Tracks, nameof(chart.Tracks));
            ClassicAssert.That(chart.Tracks.Count(), Is.GreaterThan(10), "Tracks.Count");

            ClassicAssert.IsNotNull(chart.Albums, nameof(chart.Albums));
            ClassicAssert.That(chart.Albums.Count(), Is.GreaterThan(10), "Albums.Count");

            ClassicAssert.IsNotNull(chart.Artists, nameof(chart.Artists));
            ClassicAssert.That(chart.Artists.Count(), Is.GreaterThan(10), "Artists.Count");

            ClassicAssert.IsNotNull(chart.Playlists, nameof(chart.Playlists));
            ClassicAssert.That(chart.Playlists.Count(), Is.GreaterThan(10), "Playlists.Count");
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetAlbumChart(bool useGenreFilter)
        {
            IEnumerable<IAlbum> albums = useGenreFilter ? this.session.Charts.GetAlbumChartForGenre(0, CancellationToken.None).Result
                                                        : this.session.Charts.GetAlbumChart(CancellationToken.None).Result;
                
            ClassicAssert.IsNotNull(albums, nameof(albums));
            ClassicAssert.That(albums.Count(), Is.GreaterThan(10), "Count");
        }


        [TestCase(true)]
        [TestCase(false)]
        public void GetArtistChart(bool useGenreFilter)
        {
            IEnumerable<IArtist> artists = useGenreFilter ? this.session.Charts.GetArtistChartForGenre(0, CancellationToken.None).Result
                                                          : this.session.Charts.GetArtistChart(CancellationToken.None).Result;

            ClassicAssert.IsNotNull(artists, nameof(artists));
            ClassicAssert.That(artists.Count(), Is.GreaterThan(10), "Count");
        }


        [TestCase(true)]
        [TestCase(false)]
        public void GetPlaylistChart(bool useGenreFilter)
        {
            IEnumerable<IPlaylist> playlists = useGenreFilter ? this.session.Charts.GetPlaylistChartForGenre(0, CancellationToken.None).Result
                                                              : this.session.Charts.GetPlaylistChart(CancellationToken.None).Result;

            ClassicAssert.IsNotNull(playlists, nameof(playlists));
            ClassicAssert.That(playlists.Count(), Is.GreaterThan(10), "Count");
        }


        [TestCase(true)]
        [TestCase(false)]
        public void GetTrackChart(bool useGenreFilter)
        {
            IEnumerable<ITrack> tracks = useGenreFilter ? this.session.Charts.GetTrackChartForGenre(0, CancellationToken.None).Result
                                                        : this.session.Charts.GetTrackChart(CancellationToken.None).Result;

            ClassicAssert.IsNotNull(tracks, nameof(tracks));
            ClassicAssert.That(tracks.Count(), Is.GreaterThan(10), "Count");
        }
    }
}
