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
        string Lastname { get; set; }
        string Firstname { get; set; }
        string Email { get; set; }
        int Status { get; set; }
        string Birthday { get; set; }
        string Inscription_Date { get; set; }
        string Gender { get; set; }
        string Link { get; set; }
        string Picture { get; set; }
        string Picture_Small { get; set; }
        string Picture_Medium { get; set; }
        string Picture_Big { get; set; }
        string Picture_XL { get; set; }
        string Country { get; set; }
        string Lang { get; set; }
        bool Is_Kid { get; set; }
        string Tracklist { get; set; }
        string Type { get; set; }

        // ** Methods **
        //Favourites
        Task<IEnumerable<IAlbum>> GetFavouriteAlbums(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<IArtist>> GetFavouriteArtists(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<ITrack>> GetFavouriteTracks(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<ITrack>> GetPersonalTracks(uint aStart = 0 , uint aCount = 100);

        Task<IEnumerable<IPlaylist>> GetPlaylists(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<ITrack>> GetFlow(uint aStart = 0 , uint aCount = 100);

        Task<IEnumerable<ITrack>> GetHistory(uint aStart = 0, uint aCount = 100);

        //Recommendations
        Task<IEnumerable<IAlbum>> GetRecommendedAlbums(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<IArtist>> GetRecommendedArtists(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<ITrack>> GetRecommendedTracks(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<IRadio>> GetRecommendedRadio(uint aStart = 0, uint aCount = 100);

        Task<uint> CreatePlaylist(string title);

        Task<bool> AddRadioToFavourite(IRadio aRadio);
        Task<bool> AddRadioToFavourite(ulong radioId);

        Task<bool> RemoveRadioFromFavourite(IRadio aRadio);
        Task<bool> RemoveRadioFromFavourite(ulong radioId);

        Task<bool> AddTrackToFavourite(ITrack aTrack);
        Task<bool> AddTrackToFavourite(ulong trackId);

        Task<bool> RemoveTrackFromFavourite(ITrack aTrack);
        Task<bool> RemoveTrackFromFavourite(ulong trackId);

        Task<bool> AddPlaylistToFavourite(IPlaylist aPlaylist);
        Task<bool> AddPlaylistToFavourite(ulong playlistId);

        Task<bool> RemovePlaylistFromFavourite(IPlaylist aPlaylist);
        Task<bool> RemovePlaylistFromFavourite(ulong playlistId);

        Task<bool> AddArtistToFavourite(IArtist aArtist);
        Task<bool> AddArtistToFavourite(ulong artistId);

        Task<bool> RemoveArtistFromFavourite(IArtist aArtist);
        Task<bool> RemoveArtistFromFavourite(ulong artistId);

        Task<bool> AddAlbumToFavourite(IAlbum aAlbum);
        Task<bool> AddAlbumToFavourite(ulong albumId);

        Task<bool> RemoveAlbumFromFavourite(IAlbum aAlbum);
        Task<bool> RemoveAlbumFromFavourite(ulong albumId);
    }

	internal class User : ObjectWithImage, IUser, IHasError, IDeserializable<IDeezerClient>
	{

        public ulong Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
        public string Birthday { get; set; }
        public string Inscription_Date { get; set; }
        public string Gender { get; set; }
        public string Link { get; set; }
        public string Picture { get; set; }
        public string Picture_Small { get; set; }
        public string Picture_Medium { get; set; }
        public string Picture_Big { get; set; }
        public string Picture_XL { get; set; }
        public string Country { get; set; }
        public string Lang { get; set; }
        public bool Is_Kid { get; set; }
        public string Tracklist { get; set; }
        public string Type { get; set; }

		//IDeserializable
		public IDeezerClient Client
        {
            get;
            private set;
        }

        public void Deserialize(IDeezerClient aClient) 
            => Client = aClient;


        public override string ToString() => Name;

        public IError TheError => Error;


        /* METHODS */
        public Task<IEnumerable<IAlbum>> GetFavouriteAlbums(uint aStart = 0, uint aCount = 100)   
            => Get<Album, IAlbum>("albums", DeezerPermissions.BasicAccess, aStart, aCount);

        public Task<IEnumerable<IArtist>> GetFavouriteArtists(uint aStart = 0, uint aCount = 100) 
            => Get<Artist, IArtist>("artists", DeezerPermissions.BasicAccess, aStart, aCount); 

        public Task<IEnumerable<ITrack>> GetFavouriteTracks(uint aStart = 0, uint aCount = 100)   
            => Get<Track, ITrack>("tracks", DeezerPermissions.BasicAccess, aStart, aCount);

        public Task<IEnumerable<ITrack>> GetPersonalTracks(uint aStart = 0, uint aCount = 100)   
            => Get<Track, ITrack>("personal_songs", DeezerPermissions.BasicAccess, aStart, aCount); 

        public Task<IEnumerable<IPlaylist>> GetPlaylists(uint aStart = 0, uint aCount = 100)      
            => Get<Playlist, IPlaylist>("playlists", DeezerPermissions.BasicAccess, aStart, aCount); 

        public Task<IEnumerable<ITrack>> GetFlow(uint aStart = 0, uint aCount = 100)  
            => Get<Track, ITrack>("flow", DeezerPermissions.BasicAccess, aStart, aCount);

        public Task<IEnumerable<ITrack>> GetHistory(uint aStart = 0, uint aCount = 100)   
            => Get<Track, ITrack>("history", DeezerPermissions.ListeningHistory, aStart, aCount); 

        public Task<IEnumerable<IAlbum>> GetRecommendedAlbums(uint aStart = 0, uint aCount = 100) 
            => Get<Album, IAlbum>("recommendations/albums", DeezerPermissions.BasicAccess, aStart, aCount); 

        public Task<IEnumerable<IArtist>> GetRecommendedArtists(uint aStart = 0, uint aCount = 100)   
            => Get<Artist, IArtist>("recommendations/artists", DeezerPermissions.BasicAccess, aStart, aCount); 
    
        public Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists(uint aStart = 0, uint aCount = 100)   
            => Get<Playlist, IPlaylist>("recommendations/playlists",DeezerPermissions.BasicAccess, aStart, aCount);

        public Task<IEnumerable<ITrack>> GetRecommendedTracks(uint aStart = 0, uint aCount = 100) 
            => Get<Track, ITrack>("recommendations/tracks", DeezerPermissions.BasicAccess, aStart, aCount);

        public Task<IEnumerable<IRadio>> GetRecommendedRadio(uint aStart = 0, uint aCount = 100)
            => Get<Radio, IRadio>("recommendations/radios", DeezerPermissions.BasicAccess, aStart, aCount);


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


        public Task<bool> AddRadioToFavourite(IRadio aRadio)
            => AddRadioToFavourite(aRadio.Id);

        public Task<bool> AddRadioToFavourite(ulong radioId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("radio_id", radioId),
            };

            return Client.Post("user/me/radios", parms, DeezerPermissions.ManageLibrary);
        }

        public Task<bool> RemoveRadioFromFavourite(IRadio aRadio)
            => RemoveRadioFromFavourite(aRadio.Id);

        public Task<bool> RemoveRadioFromFavourite(ulong radioId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("radio_id", radioId),
            };

            return Client.Delete("user/me/radios", parms, DeezerPermissions.ManageLibrary | DeezerPermissions.DeleteLibrary);
        }

        public Task<bool> AddTrackToFavourite(ITrack aTrack)
            => AddTrackToFavourite(aTrack.Id);

        public Task<bool> AddTrackToFavourite(ulong trackId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("track_id", trackId),                
            };

            return Client.Post("user/me/tracks", parms, DeezerPermissions.ManageLibrary);
        }

        public Task<bool> RemoveTrackFromFavourite(ITrack aTrack)
            => RemoveTrackFromFavourite(aTrack.Id);

        public Task<bool> RemoveTrackFromFavourite(ulong trackId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("track_id", trackId),
            };

            return Client.Delete("user/me/tracks", parms, DeezerPermissions.ManageLibrary | DeezerPermissions.DeleteLibrary);
        }

        public Task<bool> AddPlaylistToFavourite(IPlaylist aPlaylist)
            => AddPlaylistToFavourite(aPlaylist.Id);

        public Task<bool> AddPlaylistToFavourite(ulong playlistId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("playlist_id", playlistId),
            };

            return Client.Post("user/me/playlists", parms, DeezerPermissions.ManageLibrary);
        }

        public Task<bool> RemovePlaylistFromFavourite(IPlaylist aPlaylist)
            => RemovePlaylistFromFavourite(aPlaylist.Id);

        public Task<bool> RemovePlaylistFromFavourite(ulong playlistId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("playlist_id", playlistId),
            };

            return Client.Delete("user/me/playlists", parms, DeezerPermissions.ManageLibrary | DeezerPermissions.DeleteLibrary);
        }

        public Task<bool> AddArtistToFavourite(IArtist aArtist)
            => AddAlbumToFavourite(aArtist.Id);
        
        public Task<bool> AddArtistToFavourite(ulong artistId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("artist_id", artistId),
            };

            return Client.Post("user/me/artists", parms, DeezerPermissions.ManageLibrary);
        }

        public Task<bool> RemoveArtistFromFavourite(IArtist aArtist)
            => RemoveArtistFromFavourite(aArtist.Id);

        public Task<bool> RemoveArtistFromFavourite(ulong artistId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("artist_id", artistId),
            };

            return Client.Delete("user/me/artists", parms, DeezerPermissions.ManageLibrary | DeezerPermissions.DeleteLibrary);
        }

        public Task<bool> AddAlbumToFavourite(IAlbum aAlbum)
            => AddAlbumToFavourite(aAlbum.Id);

        public Task<bool> AddAlbumToFavourite(ulong albumId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("album_id", albumId),
            };

            return Client.Post("user/me/albums", parms, DeezerPermissions.ManageLibrary);
        }

        public Task<bool> RemoveAlbumFromFavourite(IAlbum aAlbum)
            => RemoveArtistFromFavourite(aAlbum.Id);

        public Task<bool> RemoveAlbumFromFavourite(ulong albumId)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("album_id", albumId),
            };

            return Client.Delete("user/me/albums", parms, DeezerPermissions.ManageLibrary | DeezerPermissions.DeleteLibrary);
        }



        private Task<IEnumerable<TDest>> Get<TSource, TDest>(string aMethod, DeezerPermissions aPermisisons, uint aStart, uint aCount) where TSource : TDest, IDeserializable<IDeezerClient>
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
