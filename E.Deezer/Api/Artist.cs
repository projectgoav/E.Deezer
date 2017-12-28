using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using Newtonsoft.Json;

namespace E.Deezer.Api
{
    public interface IArtist : IObjectWithImage
    {
        ulong Id { get;  }
        string Name { get;  }
        string Link { get;  }

        //Methods
        Task<IEnumerable<ITrack>> GetTracklist(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<ITrack>> GetTopTracks(uint aStart = 0 , uint aCount = 100);

        Task<IEnumerable<IAlbum>> GetAlbums(uint aStart = 0 , uint aCount = 100);

        Task<IEnumerable<IArtist>> GetRelated(uint aStart =0 , uint aCount = 100);

        Task<IEnumerable<IPlaylist>> GetPlaylistsContaining(uint aStart = 0, uint aCount = 100);

        Task<bool> AddArtistToFavorite();
        Task<bool> RemoveArtistFromFavorite();
    }

    internal class Artist : ObjectWithImage, IArtist, IDeserializable<IDeezerClient>
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

        [JsonProperty(PropertyName="url")]
        public string Link
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


        public Task<IEnumerable<ITrack>> GetTracklist(uint aStart = 0, uint aCount = 100) 
            => Get<Track, ITrack>("artist/{id}/radio", aStart, aCount);


        public Task<IEnumerable<ITrack>> GetTopTracks(uint aStart = 0, uint aCount = 100) 
            => Get<Track, ITrack>("artist/{id}/top", aStart, aCount);

        public Task<IEnumerable<IAlbum>> GetAlbums(uint aStart = 0, uint aCount = 100)
            => Get<Album, IAlbum>("artist/{id}/albums", aStart, aCount);


        public Task<IEnumerable<IArtist>> GetRelated(uint aStart = 0, uint aCount = 100)
            => Get<Artist, IArtist>("artist/{id}/related", aStart, aCount);

        public Task<IEnumerable<IPlaylist>> GetPlaylistsContaining(uint aStart = 0, uint aCount = 100) 
            => Get<Playlist, IPlaylist>("artist/{id}/playlists", aStart, aCount);


        //Internal wrapper around get for all artist methods :)
        private Task<IEnumerable<TDest>> Get<TSource, TDest>(string aMethod, uint aStart, uint aCount) where TSource : TDest, IDeserializable<IDeezerClient>
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", Id)
            };

            return Client.Get<TSource>(aMethod, parms, aStart, aCount).ContinueWith<IEnumerable<TDest>>((aTask) =>
            {
                return Client.Transform<TSource, TDest>(aTask.Result);
            }, Client.CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }


        public Task<bool> AddArtistToFavorite() 
            => Client.User.AddArtistToFavourite(Id);

        public Task<bool> RemoveArtistFromFavorite() 
            => Client.User.RemoveArtistFromFavourite(Id);       


        public override string ToString()
        {
            return string.Format("E.Deezer: Artist({0})", Name);
        }        
    }
}
