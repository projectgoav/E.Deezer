using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using Newtonsoft.Json;

namespace E.Deezer.Api
{
    public interface IGenre : IObjectWithImage
    {
        ulong Id { get;  }
        string Name { get;  }

        //METHODS
        Task<IEnumerable<IArtist>> GetArtists(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<IAlbum>> GetSelection(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<IAlbum>> GetReleases(uint aStart = 0 , uint aCount = 100);


        //Charts!
        Task<IChart> GetCharts(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<IAlbum>> GetAlbumChart(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<IArtist>> GetArtistChart(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<IPlaylist>> GetPlaylistChart(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<ITrack>> GetTrackChart(uint aStart = 0, uint aCount = 0);


        //TODO
        //Task<IBook<IPodcast>> GetPodcasts();

        //Task<IBook<IRadio>> GetRadios();

    }

    internal class Genre : ObjectWithImage, IGenre, IDeserializable<IDeezerClient>
    {
        public ulong Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }


        //IDeserializable
        public IDeezerClient Client
        {
            get;
            set;
        }

        public void Deserialize(IDeezerClient aClient)
            => Client = aClient;

        //Methods
        public Task<IEnumerable<IArtist>> GetArtists(uint aStart = 0, uint aCount = 100)
            => Get<Artist, IArtist>("genre/{id}/artists", aStart, aCount); 

        public Task<IEnumerable<IAlbum>> GetSelection(uint aStart = 0, uint aCount = 100) 
            => Get<Album, IAlbum>("editorial/{id}/selection", aStart, aCount); 

        public Task<IEnumerable<IAlbum>> GetReleases(uint aStart = 0, uint aCount = 100) 
            => Get<Album, IAlbum>("editorial/{id}/releases", aStart, aCount);

        //Charting
        public Task<IChart> GetCharts(uint aStart = 0, uint aCount = 100)
            => Client.GetChart(Id, aStart, aCount);

        public Task<IEnumerable<IAlbum>> GetAlbumChart(uint aStart = 0, uint aCount = 100) 
            => Get<Album, IAlbum>("chart/{id}/albums", aStart, aCount); 

        public Task<IEnumerable<IArtist>> GetArtistChart(uint aStart = 0, uint aCount = 100) 
            => Get<Artist, IArtist>("chart/{id}/artists", aStart, aCount); 

        public Task<IEnumerable<IPlaylist>> GetPlaylistChart(uint aStart = 0, uint aCount = 100) 
            => Get<Playlist, IPlaylist>("chart/{id}/playlists", aStart, aCount); 

        public Task<IEnumerable<ITrack>> GetTrackChart(uint aStart = 0, uint aCount = 100) 
            => Get<Track, ITrack>("chart/{id}/tracks", aStart, aCount); 


        //Internal wrapper around get for all genre methods :)
        private Task<IEnumerable<TDest>> Get<TSource, TDest>(string aMethod, uint aStart, uint aCount) where TSource : TDest, IDeserializable<IDeezerClient>
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", Id)
            };

            return Client.Get<TSource>(aMethod, parms, aStart, aCount)
                         .ContinueWith<IEnumerable<TDest>>((aTask) =>
                            {
                                return Client.Transform<TSource, TDest>(aTask.Result);
                            }, Client.CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }



        public override string ToString()
        {
            return string.Format("E.Deezer: Genre({0} ({1}))", Name, Id);
        }
    }

}
