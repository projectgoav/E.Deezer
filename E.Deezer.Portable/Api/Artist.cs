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
        Task<IEnumerable<ITrack>> GetTracklist();
        Task<IEnumerable<ITrack>> GetTracklist(uint aCount);
        Task<IEnumerable<ITrack>> GetTracklist(uint aStart, uint aCount);

        Task<IEnumerable<ITrack>> GetTopTracks();
        Task<IEnumerable<ITrack>> GetTopTracks(uint aCount);
        Task<IEnumerable<ITrack>> GetTopTracks(uint aStart, uint aCount);

        Task<IEnumerable<IAlbum>> GetAlbums();
        Task<IEnumerable<IAlbum>> GetAlbums(uint aCount);
        Task<IEnumerable<IAlbum>> GetAlbums(uint aStart, uint aCount);

        Task<IEnumerable<IArtist>> GetRelated();
        Task<IEnumerable<IArtist>> GetRelated(uint aCount);
        Task<IEnumerable<IArtist>> GetRelated(uint aStart, uint aCount);

        Task<IEnumerable<IPlaylist>> GetPlaylistsContaining();
        Task<IEnumerable<IPlaylist>> GetPlaylistsContaining(uint aCount);
        Task<IEnumerable<IPlaylist>> GetPlaylistsContaining(uint aStart, uint aCount);

        Task<bool> AddArtistToFavorite();
        Task<bool> RemoveArtistFromFavorite();
    }

    internal class Artist : ObjectWithImage, IArtist, IDeserializable<DeezerClient>
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
        public DeezerClient Client
        {
            get;
            set;
        }

        public void Deserialize(DeezerClient aClient) => Client = aClient;


        public Task<IEnumerable<ITrack>> GetTracklist() => GetTracklist(0, Client.ResultSize); 
        public Task<IEnumerable<ITrack>> GetTracklist(uint aCount)  => GetTracklist(0, aCount); 
        public Task<IEnumerable<ITrack>> GetTracklist(uint aStart, uint aCount) => Get<Track, ITrack>("artist/{id}/radio", aStart, aCount);


        public Task<IEnumerable<ITrack>> GetTopTracks() => GetTopTracks(0, Client.ResultSize);
        public Task<IEnumerable<ITrack>> GetTopTracks(uint aCount) => GetTopTracks(0, aCount); 
        public Task<IEnumerable<ITrack>> GetTopTracks(uint aStart, uint aCount) => Get<Track, ITrack>("artist/{id}/top", aStart, aCount);


        public Task<IEnumerable<IAlbum>> GetAlbums() => GetAlbums(0, Client.ResultSize);
        public Task<IEnumerable<IAlbum>> GetAlbums(uint aCount)  => GetAlbums(0, aCount);
        public Task<IEnumerable<IAlbum>> GetAlbums(uint aStart, uint aCount) => Get<Album, IAlbum>("/artist/{id}/albums", aStart, aCount);


        public Task<IEnumerable<IArtist>> GetRelated() => GetRelated(0, Client.ResultSize);
        public Task<IEnumerable<IArtist>> GetRelated(uint aCount) => GetRelated(0, aCount);
        public Task<IEnumerable<IArtist>> GetRelated(uint aStart, uint aCount) => Get<Artist, IArtist>("artist/{id}/related", aStart, aCount);

        public Task<IEnumerable<IPlaylist>> GetPlaylistsContaining() => GetPlaylistsContaining(0, DeezerSession.DEFAULT_SIZE);
        public Task<IEnumerable<IPlaylist>> GetPlaylistsContaining(uint aCount) => GetPlaylistsContaining(0, aCount);
        public Task<IEnumerable<IPlaylist>> GetPlaylistsContaining(uint aStart, uint aCount) => Get<Playlist, IPlaylist>("artist/{id}/playlists", aStart, aCount);


        //Internal wrapper around get for all artist methods :)
        private Task<IEnumerable<TDest>> Get<TSource, TDest>(string aMethod, uint aStart, uint aCount) where TSource : TDest, IDeserializable<DeezerClient>
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


        public Task<bool> AddArtistToFavorite() => Client.User.AddArtistToFavourite(Id);

        public Task<bool> RemoveArtistFromFavorite() => Client.User.RemoveArtistFromFavourite(Id);       


        public override string ToString()
        {
            return string.Format("E.Deezer: Artist({0})", Name);
        }        
    }
}
