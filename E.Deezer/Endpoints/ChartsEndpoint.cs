using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

using E.Deezer.Api;
using E.Deezer.Util;

namespace E.Deezer.Endpoints
{
    public interface IChartsEndpoint
    {
        Task<IChart> GetCharts(CancellationToken cancellationToken, uint start = 0, uint count = 50);

        Task<IChart> GetChartsForGenre(IGenre genre, CancellationToken cancellationToken, uint start = 0, uint count = 50);
        Task<IChart> GetChartsForGenre(ulong genreId, CancellationToken cancellationToken, uint start = 0, uint count = 50);

        Task<IEnumerable<IAlbum>> GetAlbumChart(CancellationToken cancellationToken, uint start = 0, uint count = 50);
        Task<IEnumerable<IAlbum>> GetAlbumChartForGenre(IGenre genre, CancellationToken cancellationToken, uint start = 0, uint count = 50);
        Task<IEnumerable<IAlbum>> GetAlbumChartForGenre(ulong genreId, CancellationToken cancellationToken, uint start = 0, uint count = 50);

        Task<IEnumerable<IArtist>> GetArtistChart(CancellationToken cancellationToken, uint start = 0, uint count = 50);
        Task<IEnumerable<IArtist>> GetArtistChartForGenre(IGenre genre, CancellationToken cancellationToken, uint start = 0, uint count = 50);
        Task<IEnumerable<IArtist>> GetArtistChartForGenre(ulong genreId, CancellationToken cancellationToken, uint start = 0, uint count = 50);

        Task<IEnumerable<IPlaylist>> GetPlaylistChart(CancellationToken cancellationToken, uint start = 0, uint count = 50);
        Task<IEnumerable<IPlaylist>> GetPlaylistChartForGenre(IGenre genre, CancellationToken cancellationToken, uint start = 0, uint count = 50);
        Task<IEnumerable<IPlaylist>> GetPlaylistChartForGenre(ulong genreId, CancellationToken cancellationToken, uint start = 0, uint count = 50);

        Task<IEnumerable<ITrack>> GetTrackChart(CancellationToken cancellationToken, uint start = 0, uint count = 50);
        Task<IEnumerable<ITrack>> GetTrackChartForGenre(IGenre genre, CancellationToken cancellationToken, uint start = 0, uint count = 50);
        Task<IEnumerable<ITrack>> GetTrackChartForGenre(ulong genreId, CancellationToken cancellationToken, uint start = 0, uint count = 50);
    }


    internal class ChartsEndpoint : IChartsEndpoint
    {
        private const string START_PARAM = "index";
        private const string COUNT_PARAM = "limit";


        private readonly IDeezerClient client;

        public ChartsEndpoint(IDeezerClient client)
        {
            this.client = client;
        }


        public Task<IChart> GetCharts(CancellationToken cancellationToken, uint start = 0, uint count = 50)
            => this.client.Get($"/chart?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => Api.Chart.FromJson(json, this.client));


        public Task<IChart> GetChartsForGenre(IGenre genre, CancellationToken cancellationToken, uint start = 0, uint count = 50)
        {
            if (genre == null)
            {
                throw new ArgumentNullException(nameof(genre));
            }

            return GetChartsForGenre(genre.Id, cancellationToken, start, count);
        }

        public Task<IChart> GetChartsForGenre(ulong genreId, CancellationToken cancellationToken, uint start = 0, uint count = 50)
            => this.client.Get($"chart/{genreId}?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => Api.Chart.FromJson(json, this.client));



        public Task<IEnumerable<IAlbum>> GetAlbumChart(CancellationToken cancellationToken, uint start = 0, uint count = 50)
            => GetCharts(cancellationToken, start, count)
                    .ContinueWith<IEnumerable<IAlbum>>(t =>
                    {
                        t.ThrowIfFaulted();

                        return t.Result.Albums;
                    }, cancellationToken, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);


        public Task<IEnumerable<IAlbum>> GetAlbumChartForGenre(IGenre genre, CancellationToken cancellationToken, uint start = 0, uint count = 50)
        {
            if (genre == null)
            {
                throw new ArgumentNullException(nameof(genre));
            }

            return GetAlbumChartForGenre(genre.Id, cancellationToken, start, count);
        }

        public Task<IEnumerable<IAlbum>> GetAlbumChartForGenre(ulong genreId, CancellationToken cancellationToken, uint start = 0, uint count = 50)
            => this.GetChartsForGenre(genreId, cancellationToken, start, count)
                    .ContinueWith(t =>
                    {
                        t.ThrowIfFaulted();

                        return t.Result.Albums;
                    }, cancellationToken, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);



        public Task<IEnumerable<IArtist>> GetArtistChart(CancellationToken cancellationToken, uint start = 0, uint count = 50)
            => GetCharts(cancellationToken, start, count)
                    .ContinueWith<IEnumerable<IArtist>>(t =>
                    {
                        t.ThrowIfFaulted();

                        return t.Result.Artists;
                    }, cancellationToken, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);


        public Task<IEnumerable<IArtist>> GetArtistChartForGenre(IGenre genre, CancellationToken cancellationToken, uint start = 0, uint count = 50)
        {
            if (genre == null)
            {
                throw new ArgumentNullException(nameof(genre));
            }

            return GetArtistChartForGenre(genre.Id, cancellationToken, start, count);
        }

        public Task<IEnumerable<IArtist>> GetArtistChartForGenre(ulong genreId, CancellationToken cancellationToken, uint start = 0, uint count = 50)
            => this.GetChartsForGenre(genreId, cancellationToken, start, count)
                    .ContinueWith(t =>
                    {
                        t.ThrowIfFaulted();

                        return t.Result.Artists;
                    }, cancellationToken, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);



        public Task<IEnumerable<IPlaylist>> GetPlaylistChart(CancellationToken cancellationToken, uint start = 0, uint count = 50)
            => GetCharts(cancellationToken, start, count)
                    .ContinueWith<IEnumerable<IPlaylist>>(t =>
                    {
                        t.ThrowIfFaulted();

                        return t.Result.Playlists;
                    }, cancellationToken, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);


        public Task<IEnumerable<IPlaylist>> GetPlaylistChartForGenre(IGenre genre, CancellationToken cancellationToken, uint start = 0, uint count = 50)
        {
            if (genre == null)
            {
                throw new ArgumentNullException(nameof(genre));
            }

            return GetPlaylistChartForGenre(genre.Id, cancellationToken, start, count);
        }

        public Task<IEnumerable<IPlaylist>> GetPlaylistChartForGenre(ulong genreId, CancellationToken cancellationToken, uint start = 0, uint count = 50)
            => this.GetChartsForGenre(genreId, cancellationToken, start, count)
                    .ContinueWith(t =>
                    {
                        t.ThrowIfFaulted();

                        return t.Result.Playlists;
                    }, cancellationToken, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);



        public Task<IEnumerable<ITrack>> GetTrackChart(CancellationToken cancellationToken, uint start = 0, uint count = 50)
            => GetCharts(cancellationToken, start, count)
                    .ContinueWith<IEnumerable<ITrack>>(t =>
                    {
                        t.ThrowIfFaulted();

                        return t.Result.Tracks;
                    }, cancellationToken, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);


        public Task<IEnumerable<ITrack>> GetTrackChartForGenre(IGenre genre, CancellationToken cancellationToken, uint start = 0, uint count = 50)
        {
            if (genre == null)
            {
                throw new ArgumentNullException(nameof(genre));
            }

            return GetTrackChartForGenre(genre.Id, cancellationToken, start, count);
        }

        public Task<IEnumerable<ITrack>> GetTrackChartForGenre(ulong genreId, CancellationToken cancellationToken, uint start = 0, uint count = 50)
            => this.GetChartsForGenre(genreId, cancellationToken, start, count)
                    .ContinueWith(t =>
                    {
                        t.ThrowIfFaulted();

                        return t.Result.Tracks;
                    }, cancellationToken, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
    }
}
