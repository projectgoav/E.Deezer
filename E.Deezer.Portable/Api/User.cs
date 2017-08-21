using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Deezer.Api
{

	public interface IUser : IObjectWithImage
	{
		ulong Id { get; set; }
		string Name { get; set; }
		string Link { get; set; }
		string Country { get; set; }


		// ** Methods **
        //Favourites
		Task<IEnumerable<IAlbum>> GetFavouriteAlbums();
        Task<IEnumerable<IAlbum>> GetFavouriteAlbums(uint aCount);
        Task<IEnumerable<IAlbum>> GetFavouriteAlbums(uint aStart, uint aCount);

		Task<IEnumerable<IArtist>> GetFavouriteArtists();
        Task<IEnumerable<IArtist>> GetFavouriteArtists(uint aCount);
        Task<IEnumerable<IArtist>> GetFavouriteArtists(uint aStart, uint aCount);

		Task<IEnumerable<ITrack>> GetFavouriteTracks();
        Task<IEnumerable<ITrack>> GetFavouriteTracks(uint aCount);
        Task<IEnumerable<ITrack>> GetFavouriteTracks(uint aStart, uint aCount);


        Task<IEnumerable<ITrack>> GetPersonalTracks();
        Task<IEnumerable<ITrack>> GetPersonalTracks(uint aCount);
        Task<IEnumerable<ITrack>> GetPersonalTracks(uint aStart, uint aCount);

        Task<IEnumerable<IPlaylist>> GetPlaylists();
        Task<IEnumerable<IPlaylist>> GetPlaylists(uint aCount);
        Task<IEnumerable<IPlaylist>> GetPlaylists(uint aStart, uint aCount);


		Task<IEnumerable<ITrack>> GetFlow();
        Task<IEnumerable<ITrack>> GetFlow(uint aCount);
        Task<IEnumerable<ITrack>> GetFlow(uint aStart, uint aCount);

		Task<IEnumerable<ITrack>> GetHistory();
        Task<IEnumerable<ITrack>> GetHistory(uint aCount);
        Task<IEnumerable<ITrack>> GetHistory(uint aStart, uint aCount);

        //Recommendations
		Task<IEnumerable<IAlbum>> GetRecommendedAlbums();
        Task<IEnumerable<IAlbum>> GetRecommendedAlbums(uint aCount);
        Task<IEnumerable<IAlbum>> GetRecommendedAlbums(uint aStart, uint aCount);

		Task<IEnumerable<IArtist>> GetRecommendedArtists();
        Task<IEnumerable<IArtist>> GetRecommendedArtists(uint aCount);
        Task<IEnumerable<IArtist>> GetRecommendedArtists(uint aStart, uint aCount);

		Task<IEnumerable<ITrack>> GetRecommendedTracks();
        Task<IEnumerable<ITrack>> GetRecommendedTracks(uint aCount);
        Task<IEnumerable<ITrack>> GetRecommendedTracks(uint aStart, uint aCount);

		Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists();
        Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists(uint aCount);
        Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists(uint aStart, uint aCount);

        Task<uint> CreatePlaylist(string title);
        Task<bool> AddToPlaylist(ulong playlistId, string songids);

        Task<bool> AddRadioToFavourite(ulong radioId);
        Task<bool> RemoveRadioFromFavourite(ulong radioId);

        Task<bool> AddTrackToFavourite(ulong trackId);
        Task<bool> RemoveTrackFromFavourite(ulong trackId);

        Task<bool> AddPlaylistToFavourite(ulong playlistId);
        Task<bool> RemovePlaylistFromFavourite(ulong playlistId);

        Task<bool> AddArtistToFavourite(ulong artistId);
        Task<bool> RemoveArtistFromFavourite(ulong artistId);

        Task<bool> AddAlbumToFavourite(ulong albumId);
        Task<bool> RemoveAlbumFromFavourite(ulong albumId);
    }

	internal class User : ObjectWithImage, IUser, IDeserializable<DeezerClient>
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

		public string Country
        {
            get;
            set;
        }


		//IDeserializable
		public DeezerClient Client
        {
            get;
            private set;
        }

        public void Deserialize(DeezerClient aClient) => Client = aClient;


        public override string ToString() => Name;


        /* METHODS */
        public Task<IEnumerable<IAlbum>> GetFavouriteAlbums()                           => GetFavouriteAlbums(0, Client.ResultSize);
        public Task<IEnumerable<IAlbum>> GetFavouriteAlbums(uint aCount)                => GetFavouriteAlbums(0, aCount);
        public Task<IEnumerable<IAlbum>> GetFavouriteAlbums(uint aStart, uint aCount)   => Get<Album, IAlbum>("albums", DeezerPermissions.BasicAccess, aStart, aCount);

        public Task<IEnumerable<IArtist>> GetFavouriteArtists()                         => GetFavouriteArtists(0, Client.ResultSize);
        public Task<IEnumerable<IArtist>> GetFavouriteArtists(uint aCount)              => GetFavouriteArtists(0, aCount);
        public Task<IEnumerable<IArtist>> GetFavouriteArtists(uint aStart, uint aCount) => Get<Artist, IArtist>("artists", DeezerPermissions.BasicAccess, aStart, aCount); 

        public Task<IEnumerable<ITrack>> GetFavouriteTracks()                           => GetFavouriteTracks(0, Client.ResultSize); 
        public Task<IEnumerable<ITrack>> GetFavouriteTracks(uint aCount)                => GetFavouriteTracks(0, aCount);
        public Task<IEnumerable<ITrack>> GetFavouriteTracks(uint aStart, uint aCount)   => Get<Track, ITrack>("tracks", DeezerPermissions.BasicAccess, aStart, aCount);


        public Task<IEnumerable<ITrack>> GetPersonalTracks()                            => GetPersonalTracks(0, Client.ResultSize); 
        public Task<IEnumerable<ITrack>> GetPersonalTracks(uint aCount)                 => GetPersonalTracks(0, aCount); 
        public Task<IEnumerable<ITrack>> GetPersonalTracks(uint aStart, uint aCount)    => Get<Track, ITrack>("personal_songs", DeezerPermissions.BasicAccess, aStart, aCount); 


        public Task<IEnumerable<IPlaylist>> GetPlaylists()                              => GetPlaylists(0, Client.ResultSize); 
        public Task<IEnumerable<IPlaylist>> GetPlaylists(uint aCount)                   => GetPlaylists(0, aCount); 
        public Task<IEnumerable<IPlaylist>> GetPlaylists(uint aStart, uint aCount)      => Get<Playlist, IPlaylist>("playlists", DeezerPermissions.BasicAccess, aStart, aCount); 


        public Task<IEnumerable<ITrack>> GetFlow()                          => GetFlow(0, Client.ResultSize); 
        public Task<IEnumerable<ITrack>> GetFlow(uint aCount)               => GetFlow(0, aCount);
        public Task<IEnumerable<ITrack>> GetFlow(uint aStart, uint aCount)  => Get<Track, ITrack>("flow", DeezerPermissions.BasicAccess, aStart, aCount);


        public Task<IEnumerable<ITrack>> GetHistory()                           => GetHistory(0, Client.ResultSize); 
        public Task<IEnumerable<ITrack>> GetHistory(uint aCount)                => GetHistory(0, aCount); 
        public Task<IEnumerable<ITrack>> GetHistory(uint aStart, uint aCount)   => Get<Track, ITrack>("history", DeezerPermissions.ListeningHistory, aStart, aCount); 


        public Task<IEnumerable<IAlbum>> GetRecommendedAlbums()                         => GetRecommendedAlbums(0, Client.ResultSize); 
        public Task<IEnumerable<IAlbum>> GetRecommendedAlbums(uint aCount)              => GetRecommendedAlbums(0, aCount); 
        public Task<IEnumerable<IAlbum>> GetRecommendedAlbums(uint aStart, uint aCount) => Get<Album, IAlbum>("recommendations/albums", DeezerPermissions.BasicAccess, aStart, aCount); 

        public Task<IEnumerable<IArtist>> GetRecommendedArtists()                           => GetRecommendedArtists(0, Client.ResultSize); 
        public Task<IEnumerable<IArtist>> GetRecommendedArtists(uint aCount)                => GetRecommendedArtists(0, aCount); 
        public Task<IEnumerable<IArtist>> GetRecommendedArtists(uint aStart, uint aCount)   => Get<Artist, IArtist>("recommendations/artists", DeezerPermissions.BasicAccess, aStart, aCount); 
    
        public Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists()                           => GetRecommendedPlaylists(0, Client.ResultSize); 
        public Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists(uint aCount)                => GetRecommendedPlaylists(0, aCount); 
        public Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists(uint aStart, uint aCount)   => Get<Playlist, IPlaylist>("recommendations/playlists",DeezerPermissions.BasicAccess, aStart, aCount);

        public Task<IEnumerable<ITrack>> GetRecommendedTracks()                         => GetRecommendedTracks(0, Client.ResultSize); 
        public Task<IEnumerable<ITrack>> GetRecommendedTracks(uint aCount)              => GetRecommendedTracks(0, aCount); 
        public Task<IEnumerable<ITrack>> GetRecommendedTracks(uint aStart, uint aCount) => Get<Track, ITrack>("recommendations/tracks", DeezerPermissions.BasicAccess, aStart, aCount); 


        public Task<uint> CreatePlaylist(string title)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", Id),
                RequestParameter.GetNewQueryStringParameter("title", title)
            };

            return Client.Post<DeezerCreateResponse>("user/{id}/playlists", parms, DeezerPermissions.ManageLibrary)
                            .ContinueWith(t => t.Result.Id);                        
        }

        [Obsolete("Preferable to use IPlaylist.AddTrack(s) methods instead")]
        public Task<bool> AddToPlaylist(ulong playlistId, string songids)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("playlist_id", playlistId),
                RequestParameter.GetNewQueryStringParameter("songs", songids)
            };

            return Client.Post("playlist/{playlist_id}/tracks", parms, DeezerPermissions.ManageLibrary);
        }

        public Task<bool> AddRadioToFavourite(ulong radioId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("radio_id", radioId),
            };

            return Client.Post("user/me/radios", parms, DeezerPermissions.ManageLibrary);
        }

        public Task<bool> RemoveRadioFromFavourite(ulong radioId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("radio_id", radioId),
            };

            return Client.Delete("user/me/radios", parms, DeezerPermissions.ManageLibrary | DeezerPermissions.DeleteLibrary);
        }        

        public Task<bool> AddTrackToFavourite(ulong trackId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("track_id", trackId),                
            };

            return Client.Post("user/me/tracks", parms, DeezerPermissions.ManageLibrary);
        }

        public Task<bool> RemoveTrackFromFavourite(ulong trackId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("track_id", trackId),
            };

            return Client.Delete("user/me/tracks", parms, DeezerPermissions.ManageLibrary | DeezerPermissions.DeleteLibrary);
        }

        public Task<bool> AddPlaylistToFavourite(ulong playlistId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("playlist_id", playlistId),
            };

            return Client.Post("user/me/playlists", parms, DeezerPermissions.ManageLibrary);
        }

        public Task<bool> RemovePlaylistFromFavourite(ulong playlistId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("playlist_id", playlistId),
            };

            return Client.Delete("user/me/playlists", parms, DeezerPermissions.ManageLibrary | DeezerPermissions.DeleteLibrary);
        }

        public Task<bool> AddArtistToFavourite(ulong artistId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("artist_id", artistId),
            };

            return Client.Post("user/me/artists", parms, DeezerPermissions.ManageLibrary);
        }

        public Task<bool> RemoveArtistFromFavourite(ulong artistId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("artist_id", artistId),
            };

            return Client.Delete("user/me/artists", parms, DeezerPermissions.ManageLibrary | DeezerPermissions.DeleteLibrary);
        }

        public Task<bool> AddAlbumToFavourite(ulong albumId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("album_id", albumId),
            };

            return Client.Post("user/me/albums", parms, DeezerPermissions.ManageLibrary);
        }

        public Task<bool> RemoveAlbumFromFavourite(ulong albumId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("album_id", albumId),
            };

            return Client.Delete("user/me/albums", parms, DeezerPermissions.ManageLibrary | DeezerPermissions.DeleteLibrary);
        }




        private Task<IEnumerable<TDest>> Get<TSource, TDest>(string aMethod, DeezerPermissions aPermisisons, uint aStart, uint aCount) where TSource : TDest, IDeserializable<DeezerClient>
        {

            //TODO -> We should really have a Client.GET<T> that accepts a permissions check....
            if (!Client.IsAuthenticated)
            {
                throw new NotLoggedInException();
            }
            if (!Client.HasPermission(aPermisisons)) { throw new DeezerPermissionsException(aPermisisons); }

            string method = string.Format("user/me/{0}", aMethod);
            return Client.Get<TSource>(method, aStart, aCount).ContinueWith<IEnumerable<TDest>>((aTask) =>
            {
                return Client.Transform<TSource, TDest>(aTask.Result);
            }, Client.CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }

    }
}
