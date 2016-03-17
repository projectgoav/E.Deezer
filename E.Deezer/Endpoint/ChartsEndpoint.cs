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
        Task<IEnumerable<IAlbum>> GetAlbumChart();
        Task<IEnumerable<IAlbum>> GetAlbumChart(uint aCount);
        Task<IEnumerable<IAlbum>> GetAlbumChart(uint aStart, uint aCount);

        Task<IEnumerable<IArtist>> GetArtistChart();
        Task<IEnumerable<IArtist>> GetArtistChart(uint aCount);
        Task<IEnumerable<IArtist>> GetArtistChart(uint aStart, uint aCount);

        Task<IEnumerable<IPlaylist>> GetPlaylistChart();
        Task<IEnumerable<IPlaylist>> GetPlaylistChart(uint aCount);
        Task<IEnumerable<IPlaylist>> GetPlaylistChart(uint aStart, uint aCount);

        Task<IEnumerable<ITrack>> GetTrackChart();
        Task<IEnumerable<ITrack>> GetTrackChart(uint aCount);
        Task<IEnumerable<ITrack>> GetTrackChart(uint aStart, uint aCount);
    }

    internal class ChartsEndpoint : IChartsEndpoint
    {
        private DeezerClient iClient;
        public ChartsEndpoint(DeezerClient aClient) { iClient = aClient; }


        public Task<IEnumerable<IAlbum>> GetAlbumChart() { return GetAlbumChart(0, iClient.ResultSize); }
        public Task<IEnumerable<IAlbum>> GetAlbumChart(uint aCount) { return GetAlbumChart(0, aCount); }
        public Task<IEnumerable<IAlbum>> GetAlbumChart(uint aStart, uint aCount) { return Get<Album, IAlbum>("charts/{id}/albums", 0, aStart, aCount); }

        public Task<IEnumerable<IArtist>> GetArtistChart() { return GetArtistChart(0, iClient.ResultSize); }
        public Task<IEnumerable<IArtist>> GetArtistChart(uint aCount) { return GetArtistChart(0, aCount); }
        public Task<IEnumerable<IArtist>> GetArtistChart(uint aStart, uint aCount) { return Get<Artist, IArtist>("charts/{id}/artists", 0, aStart, aCount); }

        public Task<IEnumerable<IPlaylist>> GetPlaylistChart() { return GetPlaylistChart(0, iClient.ResultSize); }
        public Task<IEnumerable<IPlaylist>> GetPlaylistChart(uint aCount) { return GetPlaylistChart(0, aCount); }
        public Task<IEnumerable<IPlaylist>> GetPlaylistChart(uint aStart, uint aCount) { return Get<Playlist, IPlaylist>("charts/{id}/playlists", 0, aStart, aCount); }

        public Task<IEnumerable<ITrack>> GetTrackChart() { return GetTrackChart(0, iClient.ResultSize); }
        public Task<IEnumerable<ITrack>> GetTrackChart(uint aCount) { return GetTrackChart(0, aCount); }
        public Task<IEnumerable<ITrack>> GetTrackChart(uint aStart, uint aCount) { return Get<Track, ITrack>("charts/{id}/track", 0, aStart, aCount); }


        //Internal wrapper around get for all ChartEndpoint methods :)
        private Task<IEnumerable<TDest>> Get<TSource, TDest>(string aMethod, uint aId, uint aStart, uint aCount) where TSource : TDest, IDeserializable<DeezerClient>
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", aId)
            };

            return iClient.Get<TSource>(aMethod, parms, aStart, aCount).ContinueWith<IEnumerable<TDest>>((aTask) =>
            {
                List<TDest> items = new List<TDest>();

                foreach (var item in aTask.Result.Items)
                {
                    item.Deserialize(iClient);
                    items.Add(item);
                }
                return items;
            }, iClient.CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }
    }
}
