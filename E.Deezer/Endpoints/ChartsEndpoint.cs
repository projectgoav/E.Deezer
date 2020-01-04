using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

using E.Deezer.Api;
using E.Deezer.Util;
using E.Deezer.Api.Internal;

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
            genre.ThrowIfNull();

            return GetChartsForGenre(genre.Id, cancellationToken, start, count);
        }

        public Task<IChart> GetChartsForGenre(ulong genreId, CancellationToken cancellationToken, uint start = 0, uint count = 50)
            => this.client.Get($"chart/{genreId}?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => Api.Chart.FromJson(json, this.client));



        public Task<IEnumerable<IAlbum>> GetAlbumChart(CancellationToken cancellationToken, uint start = 0, uint count = 50)
            => this.client.Get($"/chart/albums?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => FragmentOf<IAlbum>.FromJson(json, x => Api.Album.FromJson(x, this.client)));


        public Task<IEnumerable<IAlbum>> GetAlbumChartForGenre(IGenre genre, CancellationToken cancellationToken, uint start = 0, uint count = 50)
        {
            genre.ThrowIfNull();

            return GetAlbumChartForGenre(genre.Id, cancellationToken, start, count);
        }

        public Task<IEnumerable<IAlbum>> GetAlbumChartForGenre(ulong genreId, CancellationToken cancellationToken, uint start = 0, uint count = 50)
            => this.client.Get($"/chart/{genreId}/albums?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => FragmentOf<IAlbum>.FromJson(json, x => Api.Album.FromJson(x, this.client)));



        public Task<IEnumerable<IArtist>> GetArtistChart(CancellationToken cancellationToken, uint start = 0, uint count = 50)
            => this.client.Get($"/chart/artists?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => FragmentOf<IArtist>.FromJson(json, x => Api.Artist.FromJson(x, this.client)));


        public Task<IEnumerable<IArtist>> GetArtistChartForGenre(IGenre genre, CancellationToken cancellationToken, uint start = 0, uint count = 50)
        {
            genre.ThrowIfNull();

            return GetArtistChartForGenre(genre.Id, cancellationToken, start, count);
        }

        public Task<IEnumerable<IArtist>> GetArtistChartForGenre(ulong genreId, CancellationToken cancellationToken, uint start = 0, uint count = 50)
            => this.client.Get($"/chart/{genreId}/artists?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => FragmentOf<IArtist>.FromJson(json, x => Api.Artist.FromJson(x, this.client)));



        public Task<IEnumerable<IPlaylist>> GetPlaylistChart(CancellationToken cancellationToken, uint start = 0, uint count = 50)
            => this.client.Get($"/chart/playlists?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => FragmentOf<IPlaylist>.FromJson(json, x => Api.Playlist.FromJson(x, this.client)));


        public Task<IEnumerable<IPlaylist>> GetPlaylistChartForGenre(IGenre genre, CancellationToken cancellationToken, uint start = 0, uint count = 50)
        {
            genre.ThrowIfNull();

            return GetPlaylistChartForGenre(genre.Id, cancellationToken, start, count);
        }

        public Task<IEnumerable<IPlaylist>> GetPlaylistChartForGenre(ulong genreId, CancellationToken cancellationToken, uint start = 0, uint count = 50)
            => this.client.Get($"/chart/{genreId}/playlists?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => FragmentOf<IPlaylist>.FromJson(json, x => Api.Playlist.FromJson(x, this.client)));



        public Task<IEnumerable<ITrack>> GetTrackChart(CancellationToken cancellationToken, uint start = 0, uint count = 50)
            => this.client.Get($"/chart/tracks?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => FragmentOf<ITrack>.FromJson(json, x => Api.Track.FromJson(x, this.client)));


        public Task<IEnumerable<ITrack>> GetTrackChartForGenre(IGenre genre, CancellationToken cancellationToken, uint start = 0, uint count = 50)
        {
            genre.ThrowIfNull();

            return GetTrackChartForGenre(genre.Id, cancellationToken, start, count);
        }

        public Task<IEnumerable<ITrack>> GetTrackChartForGenre(ulong genreId, CancellationToken cancellationToken, uint start = 0, uint count = 50)
            => this.client.Get($"/chart/{genreId}/tracks?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => FragmentOf<ITrack>.FromJson(json, x => Api.Track.FromJson(x, this.client)));
    }
}
