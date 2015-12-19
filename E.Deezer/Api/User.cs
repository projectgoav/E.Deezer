using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Deezer.Api
{

	public interface IUser
	{
		uint Id { get; set; }
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


		Task<IEnumerable<ITrack>> GetUserFlow();
        Task<IEnumerable<ITrack>> GetUserFlow(uint aCount);
        Task<IEnumerable<ITrack>> GetUserFlow(uint aStart, uint aCount);

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
	}

	public class User : IUser, IDeserializable<DeezerClient>
	{
		public uint Id { get; set; }
		public string Name { get; set; }
		public string Link { get; set; }
		public string Country { get; set; }

		public string error { get; set; }

		//Local Serailization info
		public DeezerClient Client { get; set; }
		public void Deserialize(DeezerClient aClient) { Client = aClient; }

		public override string ToString()
		{
			return Name;
		}

        //Internal wrapper around get for all user methods :)
        private Task<IEnumerable<TDest>> Get<TSource, TDest>(string aMethod, uint aStart, uint aCount) where TSource : TDest, IDeserializable<DeezerClient>
        {
            string method = string.Format("user/me/{0}", aMethod);
            return Client.Get<TSource>(method, aStart, aCount).ContinueWith<IEnumerable<TDest>>((aTask) =>
            {
                return Client.Transform<TSource, TDest>(aTask.Result);
            }, Client.Token, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }


        public Task<IEnumerable<IAlbum>> GetFavouriteAlbums() { return GetFavouriteAlbums(0, Client.ResultSize);  }
        public Task<IEnumerable<IAlbum>> GetFavouriteAlbums(uint aCount) { return GetFavouriteAlbums(0, aCount); }
        public Task<IEnumerable<IAlbum>> GetFavouriteAlbums(uint aStart, uint aCount)
        {
            if (!Client.HasPermission(DeezerPermissions.BasicAccess)) { throw new DeezerPermissionsException(DeezerPermissions.BasicAccess); }
            else { return Get<Album, IAlbum>("albums", aStart, aCount); }
        }

        public Task<IEnumerable<IArtist>> GetFavouriteArtists() { return GetFavouriteArtists(0, Client.ResultSize); }
        public Task<IEnumerable<IArtist>> GetFavouriteArtists(uint aCount) {  return GetFavouriteArtists(0, aCount); }
        public Task<IEnumerable<IArtist>> GetFavouriteArtists(uint aStart, uint aCount)
        {
            if (!Client.HasPermission(DeezerPermissions.BasicAccess)) { throw new DeezerPermissionsException(DeezerPermissions.BasicAccess); }
            else { return Get<Artist, IArtist>("artists", aStart, aCount); }
        }

        public Task<IEnumerable<ITrack>> GetFavouriteTracks() { return GetFavouriteTracks(0, Client.ResultSize); }
        public Task<IEnumerable<ITrack>> GetFavouriteTracks(uint aCount) { return GetFavouriteTracks(0, aCount); }
        public Task<IEnumerable<ITrack>> GetFavouriteTracks(uint aStart, uint aCount)
        {
            if (!Client.HasPermission(DeezerPermissions.BasicAccess)) { throw new DeezerPermissionsException(DeezerPermissions.BasicAccess); }
            else { return Get<Track, ITrack>("tracks", aStart, aCount); }
        }


        public Task<IEnumerable<ITrack>> GetPersonalTracks() {  return GetPersonalTracks(0, Client.ResultSize); }
        public Task<IEnumerable<ITrack>> GetPersonalTracks(uint aCount) {  return GetPersonalTracks(0, aCount); }
        public Task<IEnumerable<ITrack>> GetPersonalTracks(uint aStart, uint aCount)
        {
            if (!Client.HasPermission(DeezerPermissions.BasicAccess)) { throw new DeezerPermissionsException(DeezerPermissions.BasicAccess); }
            else { return Get<Track, ITrack>("personal_songs", aStart, aCount); }
        }


        public Task<IEnumerable<IPlaylist>> GetPlaylists() { return GetPlaylists(0, Client.ResultSize); }
        public Task<IEnumerable<IPlaylist>> GetPlaylists(uint aCount) { return GetPlaylists(0, aCount); }
        public Task<IEnumerable<IPlaylist>> GetPlaylists(uint aStart, uint aCount)
        {
            if (!Client.HasPermission(DeezerPermissions.BasicAccess)) { throw new DeezerPermissionsException(DeezerPermissions.BasicAccess); }
            else { return Get<Playlist, IPlaylist>("[playlists", aStart, aCount); }
        }


        public Task<IEnumerable<ITrack>> GetUserFlow() { return GetUserFlow(0, Client.ResultSize);  }
        public Task<IEnumerable<ITrack>> GetUserFlow(uint aCount) { return GetUserFlow(0, aCount); }
        public Task<IEnumerable<ITrack>> GetUserFlow(uint aStart, uint aCount)
        {
            if (!Client.HasPermission(DeezerPermissions.BasicAccess)) { throw new DeezerPermissionsException(DeezerPermissions.BasicAccess); }
            else { return Get<Track, ITrack>("flow", aStart, aCount); }
        }


        public Task<IEnumerable<ITrack>> GetHistory() { return GetHistory(0, Client.ResultSize); }
        public Task<IEnumerable<ITrack>> GetHistory(uint aCount) { return GetHistory(0, aCount); }
        public Task<IEnumerable<ITrack>> GetHistory(uint aStart, uint aCount)
        {
            if (!Client.HasPermission(DeezerPermissions.ListeningHistory)) { throw new DeezerPermissionsException(DeezerPermissions.ListeningHistory); }
            else { return Get<Track, ITrack>("history", aStart, aCount); }
        }


        public Task<IEnumerable<IAlbum>> GetRecommendedAlbums() { return GetRecommendedAlbums(0, Client.ResultSize); }
        public Task<IEnumerable<IAlbum>> GetRecommendedAlbums(uint aCount) { return GetRecommendedAlbums(0, aCount); }
        public Task<IEnumerable<IAlbum>> GetRecommendedAlbums(uint aStart, uint aCount)
        {
            if (!Client.HasPermission(DeezerPermissions.BasicAccess)) { throw new DeezerPermissionsException(DeezerPermissions.BasicAccess); }
            else { return Get<Album, IAlbum>("recommendations/albums", aStart, aCount); }
        }

        public Task<IEnumerable<IArtist>> GetRecommendedArtists() { return GetRecommendedArtists(0, Client.ResultSize); }
        public Task<IEnumerable<IArtist>> GetRecommendedArtists(uint aCount) { return GetRecommendedArtists(0, aCount); }
        public Task<IEnumerable<IArtist>> GetRecommendedArtists(uint aStart, uint aCount)
        {
            if (!Client.HasPermission(DeezerPermissions.BasicAccess)) { throw new DeezerPermissionsException(DeezerPermissions.BasicAccess); }
            else { return Get<Artist, IArtist>("recommendations/artists", aStart, aCount); }
        }

        public Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists() { return GetRecommendedPlaylists(0, Client.ResultSize); }
        public Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists(uint aCount) { return GetRecommendedPlaylists(0, aCount); }
        public Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists(uint aStart, uint aCount)
        {
            if (!Client.HasPermission(DeezerPermissions.BasicAccess)) { throw new DeezerPermissionsException(DeezerPermissions.BasicAccess); }
            else { return Get<Playlist, IPlaylist>("recommendations/playlists", aStart, aCount); }
        }

        public Task<IEnumerable<ITrack>> GetRecommendedTracks() { return GetRecommendedTracks(0, Client.ResultSize); }
        public Task<IEnumerable<ITrack>> GetRecommendedTracks(uint aCount) { return GetRecommendedTracks(0, aCount); }
        public Task<IEnumerable<ITrack>> GetRecommendedTracks(uint aStart, uint aCount)
        {
            if (!Client.HasPermission(DeezerPermissions.BasicAccess)) { throw new DeezerPermissionsException(DeezerPermissions.BasicAccess); }
            else { return Get<Track, ITrack>("recommendations/tracks", aStart, aCount); }
        }


    }
}
