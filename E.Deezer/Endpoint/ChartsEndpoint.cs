using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using E.Deezer.Api;

namespace E.Deezer.Endpoint
{
    public interface IChartsEndpoint
    {
        Task<IEnumerable<IAlbum>> GetAlbumsChart();
        Task<IEnumerable<IAlbum>> GetAlbumsChart(uint aCount);
        Task<IEnumerable<IAlbum>> GetAlbumsChart(uint aStart, uint aCount);

        Task<IEnumerable<IArtist>> GetArtistsChart();
        Task<IEnumerable<IArtist>> GetArtistsChart(uint aCount);
        Task<IEnumerable<IArtist>> GetArtistsChart(uint aStart, uint aCount);

        Task<IEnumerable<IPlaylist>> GetPlaylistsChart();
        Task<IEnumerable<IPlaylist>> GetPlaylistsChart(uint aCount);
        Task<IEnumerable<IPlaylist>> GetPlaylistsChart(uint aStart, uint aCount);

        Task<IEnumerable<ITrack>> GetTracksChart();
        Task<IEnumerable<ITrack>> GetTracksChart(uint aCount);
        Task<IEnumerable<ITrack>> GetTracksChart(uint aStart, uint aCount);
    }

    internal class ChartsEndpoint : IChartsEndpoint
    {
        private DeezerClientV2 iClient;
        public ChartsEndpoint(DeezerClientV2 aClient) { iClient = aClient; }


        public Task<IEnumerable<IAlbum>> GetAlbumsChart() { return GetAlbumsChart(0, DeezerSessionV2.DEFAULT_SIZE); }
        public Task<IEnumerable<IAlbum>> GetAlbumsChart(uint aCount) { return GetAlbumsChart(0, aCount); }
        public Task<IEnumerable<IAlbum>> GetAlbumsChart(uint aStart, uint aCount) { return Get<Album, IAlbum>("charts/{id}/albums", 0, aStart, aCount); }

        public Task<IEnumerable<IArtist>> GetArtistsChart() { return GetArtistsChart(0, DeezerSessionV2.DEFAULT_SIZE); }
        public Task<IEnumerable<IArtist>> GetArtistsChart(uint aCount) { return GetArtistsChart(0, aCount); }
        public Task<IEnumerable<IArtist>> GetArtistsChart(uint aStart, uint aCount) { return Get<Artist, IArtist>("charts/{id}/artists", 0, aStart, aCount); }

        public Task<IEnumerable<IPlaylist>> GetPlaylistsChart() { return GetPlaylistsChart(0, DeezerSessionV2.DEFAULT_SIZE); }
        public Task<IEnumerable<IPlaylist>> GetPlaylistsChart(uint aCount) { return GetPlaylistsChart(0, aCount); }
        public Task<IEnumerable<IPlaylist>> GetPlaylistsChart(uint aStart, uint aCount) { return Get<Playlist, IPlaylist>("charts/{id}/playlists", 0, aStart, aCount); }

        public Task<IEnumerable<ITrack>> GetTracksChart() { return GetTracksChart(0, DeezerSessionV2.DEFAULT_SIZE); }
        public Task<IEnumerable<ITrack>> GetTracksChart(uint aCount) { return GetTracksChart(0, aCount); }
        public Task<IEnumerable<ITrack>> GetTracksChart(uint aStart, uint aCount) { return Get<Track, ITrack>("charts/{id}/track", 0, aStart, aCount); }


        //Internal wrapper around get for all ChartEndpoint methods :)
        private Task<IEnumerable<TDest>> Get<TSource, TDest>(string aMethod, uint aId, uint aStart, uint aCount) where TSource : TDest, IDeserializable<DeezerClientV2>
        {
            string[] parms = new string[] { "URL", "id", aId.ToString() };
            return iClient.Get<TSource>(aMethod, parms, aStart, aCount).ContinueWith<IEnumerable<TDest>>((aTask) =>
            {
                List<TDest> items = new List<TDest>();

                foreach (var item in aTask.Result.Items)
                {
                    item.Deserialize(iClient);
                    items.Add(item);
                }
                return items;
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }
    }
}
