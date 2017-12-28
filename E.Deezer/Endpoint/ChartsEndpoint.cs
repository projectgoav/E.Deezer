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
        //Complete charts
        Task<IChart> GetChart(uint aStart = 0, uint aCount = 100);
        Task<IChart> GetChart(ulong aGenreId, uint aStart = 0, uint aCount = 100);
        Task<IChart> GetChart(IGenre aGenre, uint aStart = 0, uint aCount = 100);

        //Album Charts
        Task<IEnumerable<IAlbum>> GetAlbumChart(uint aStart = 0, uint aCount = 100);
        Task<IEnumerable<IAlbum>> GetAlbumChartForGenre(ulong aGenreId, uint aStart = 0, uint aCount = 100);
        Task<IEnumerable<IAlbum>> GetAlbumChartForGenre(IGenre aGenre, uint aStart = 0, uint aCount = 100);


        //Artists
        Task<IEnumerable<IArtist>> GetArtistChart(uint aStart = 0, uint aCount = 100);
        Task<IEnumerable<IArtist>> GetArtistChartForGenre(ulong aGenreId, uint aStart = 0, uint aCount = 100);
        Task<IEnumerable<IArtist>> GetArtistChartForGenre(IGenre aGenre, uint aStart = 0, uint aCount = 100);

        //Playlists
        Task<IEnumerable<IPlaylist>> GetPlaylistChart(uint aStart = 0, uint aCount = 100);
        Task<IEnumerable<IPlaylist>> GetPlaylistChartForGenre(ulong aGenreId, uint aStart = 0, uint aCount = 100);
        Task<IEnumerable<IPlaylist>> GetPlaylistChartForGenre(IGenre aGenre, uint aStart = 0, uint aCount = 100);

        //Tracks
        Task<IEnumerable<ITrack>> GetTrackChart(uint aStart = 0, uint aCount = 100);
        Task<IEnumerable<ITrack>> GetTrackChartForGenre(ulong aGenreId, uint aStart = 0, uint aCount = 100);
        Task<IEnumerable<ITrack>> GetTrackChartForGenre(IGenre aGenre, uint aStart = 0, uint aCount = 100);
    }

    internal class ChartsEndpoint : IChartsEndpoint
    {
        private readonly DeezerClient iClient;

        public ChartsEndpoint(DeezerClient aClient)
        {
            iClient = aClient;
        }

        //-1 here means not to add any ID param, which will return the complete Deezer Chart
        public Task<IChart> GetChart(uint aStart = 0, uint aCount = 100)
            => iClient.GetChart(0, aStart, aCount);

        public Task<IChart> GetChart(ulong aGenreId, uint aStart = 0, uint aCount = 100) 
            => iClient.GetChart(aGenreId, aStart, aCount);

        public Task<IChart> GetChart(IGenre aGenre, uint aStart = 0, uint aCount = 100)
            => iClient.GetChart(aGenre.Id, aStart, aCount);

        /* ALBUMS */
        public Task<IEnumerable<IAlbum>> GetAlbumChart(uint aStart = 0, uint aCount = 100)                        
            => GetAlbumChartForGenre(0, aStart, aCount);

        public Task<IEnumerable<IAlbum>> GetAlbumChartForGenre(IGenre aGenre, uint aStart = 0, uint aCount = 100)
            => GetAlbumChartForGenre(aGenre.Id, aStart, aCount);

        public Task<IEnumerable<IAlbum>> GetAlbumChartForGenre(ulong aGenreId, uint aStart = 0, uint aCount = 100)
            => Get<Album, IAlbum>("chart/{id}/albums", aGenreId, aStart, aCount);


        /* ARTISTS */
        public Task<IEnumerable<IArtist>> GetArtistChart(uint aStart = 0, uint aCount = 100)                          
            => GetArtistChartForGenre(0, aStart, aCount);

        public Task<IEnumerable<IArtist>> GetArtistChartForGenre(IGenre aGenre, uint aStart = 0, uint aCount = 100)   
            => GetArtistChartForGenre(aGenre.Id, aStart, aCount);

        public Task<IEnumerable<IArtist>> GetArtistChartForGenre(ulong aGenreId, uint aStart = 0, uint aCount = 100)   
            => Get<Artist, IArtist>("chart/{id}/artists", aGenreId, aStart, aCount);



        /* PLAYLISTS */
        public Task<IEnumerable<IPlaylist>> GetPlaylistChart(uint aStart = 0, uint aCount = 100)                          
            => GetPlaylistChartForGenre(0, aStart, aCount);

        public Task<IEnumerable<IPlaylist>> GetPlaylistChartForGenre(IGenre aGenre, uint aStart = 0, uint aCount = 100)  
            => GetPlaylistChartForGenre(aGenre.Id, aStart, aCount);

        public Task<IEnumerable<IPlaylist>> GetPlaylistChartForGenre(ulong aGenreId, uint aStart = 0, uint aCount = 100)   
            => Get<Playlist, IPlaylist>("chart/{id}/playlists", aGenreId, aStart, aCount);


        /* TRACKS */
        public Task<IEnumerable<ITrack>> GetTrackChart(uint aStart = 0, uint aCount = 100)                            
            => GetTrackChartForGenre(0, aStart, aCount);

        public Task<IEnumerable<ITrack>> GetTrackChartForGenre(IGenre aGenre, uint aStart = 0, uint aCount = 100)     
            => GetTrackChartForGenre(aGenre.Id, aStart, aCount);

        public Task<IEnumerable<ITrack>> GetTrackChartForGenre(ulong aGenreId, uint aStart = 0, uint aCount = 100)      
            => Get<Track, ITrack>("chart/{id}/tracks", aGenreId, aStart, aCount);


        //Internal wrapper around get for all ChartEndpoint methods :)
        private Task<IEnumerable<TDest>> Get<TSource, TDest>(string aMethod, ulong aId, uint aStart, uint aCount) where TSource : TDest, IDeserializable<IDeezerClient>
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", aId)
            };

            return iClient.Get<TSource>(aMethod, parms, aStart, aCount)
                          .ContinueWith<IEnumerable<TDest>>((aTask) =>
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
