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
        ulong Id { get; }
        uint Fans { get; }
        string Name { get; }
        string Link { get; }
        uint AlbumCount { get; }
        string ShareLink { get; }
        bool HasSmartRadio { get; }

        [Obsolete("Use of GetSmartRadio is encouraged")]
        Task<IEnumerable<ITrack>> GetTracklist(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<ITrack>> GetSmartRadio(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<ITrack>> GetTopTracks(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<IAlbum>> GetAlbums(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<IArtist>> GetRelated(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<IPlaylist>> GetPlaylistsContaining(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<IUserProfile>> GetFans(uint aStart = 0, uint aCount = 25);

        Task<IEnumerable<IComment>> GetComments(uint aStart = 0, uint aCount = 10);


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

        public string Link
        {
            get;
            set;
        }

        public string ShareLink
        {
            get;
            set;
        }


        [JsonProperty(PropertyName = "radio")]
        public bool HasSmartRadio
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "nb_album")]
        public uint AlbumCount
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "nb_fan")]
        public uint Fans
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


        [Obsolete("Use of GetSmartRadio is encouraged")]
        public Task<IEnumerable<ITrack>> GetTracklist(uint aStart = 0, uint aCount = 100) 
            => Get<Track, ITrack>("artist/{id}/radio", aStart, aCount);

        public Task<IEnumerable<ITrack>> GetSmartRadio(uint aStart = 0, uint aCount = 100)
        {
            if(this.HasSmartRadio)
            {
                return Get<Track, ITrack>("artist/{id}/radio", aStart, aCount);
            }

            throw new InvalidOperationException("Unable to get SmartRadio for an aritst which doesn't support it. Please check 'HasSmartRadio' property before calling this method");
        }


        public Task<IEnumerable<ITrack>> GetTopTracks(uint aStart = 0, uint aCount = 100) 
            => Get<Track, ITrack>("artist/{id}/top", aStart, aCount);

        public Task<IEnumerable<IAlbum>> GetAlbums(uint aStart = 0, uint aCount = 100)
            => Get<Album, IAlbum>("artist/{id}/albums", aStart, aCount);


        public Task<IEnumerable<IArtist>> GetRelated(uint aStart = 0, uint aCount = 100)
            => Get<Artist, IArtist>("artist/{id}/related", aStart, aCount);

        public Task<IEnumerable<IPlaylist>> GetPlaylistsContaining(uint aStart = 0, uint aCount = 100) 
            => Get<Playlist, IPlaylist>("artist/{id}/playlists", aStart, aCount);


        public Task<IEnumerable<IUserProfile>> GetFans(uint aStart = 0, uint aCount = 25)
            => Get<UserProfile, IUserProfile>("artist/{id}/fans", aStart, aCount);

        public Task<IEnumerable<IComment>> GetComments(uint aStart = 0, uint aCount = 10)
            => Get<Comment, IComment>("artist/{id}/comments", aStart, aCount);


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
            => string.Format("E.Deezer: Artist({0})", Name);     
    }
}
