using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Deezer.Api
{
	/// <summary>
	/// Deezer user object
	/// </summary>
	public interface IUser
	{
		/// <summary>
		/// Deezer library ID number
		/// </summary>
		uint Id { get; set; }

		/// <summary>
		/// Username
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Deezer.com link to user
		/// </summary>
		string Link { get; set; }

		/// <summary>
		/// Country
		/// </summary>
		string Country { get; set; }


		//Methods


		/// <summary>
		/// Gets the user's favourite albums
		/// </summary>
		/// <returns>First page of user's favourite albums</returns>
		Task<IPagedResponse<IAlbum>> GetFavouriteAlbums();

		/// <summary>
		/// Gets the user's favourite artists
		/// </summary>
		/// <returns>First page of user's favourite artists</returns>
		Task<IPagedResponse<IArtist>> GetFavouriteArtists();

		/// <summary>
		/// Gets the user's favourite tracks
		/// </summary>
		/// <returns>First page of user's favourite tracks</returns>
		Task<IPagedResponse<ITrack>> GetFavouriteTracks();

		/// <summary>
		/// Gets the user's "flow"
		/// </summary>
		/// <returns>First page of user's "flow"</returns>
		Task<IPagedResponse<ITrack>> GetUserFlow();

		/// <summary>
		/// Gets the user;s listening history
		/// </summary>
		/// <returns>First page of user's litening history</returns>
		Task<IPagedResponse<ITrack>> GetListenHistory();

		/// <summary>
		/// Gets the user's favourite playlists
		/// </summary>
		/// <returns>First page of user's listening history</returns>
		Task<IPagedResponse<IPlaylist>> GetFavouritePlaylists();


		/// <summary>
		/// Gets recommended albums for the user
		/// </summary>
		/// <returns>First page of recommended albums</returns>
		Task<IPagedResponse<IAlbum>> GetRecommendedAlbums();

		/// <summary>
		/// Gets recommended artists for the user
		/// </summary>
		/// <returns>First page of recommended artists</returns>
		Task<IPagedResponse<IArtist>> GetRecommendedArtists();

		/// <summary>
		/// Gets recommended tracks for the user
		/// </summary>
		/// <returns>First page of recommended tracks</returns>
		Task<IPagedResponse<ITrack>> GetRecommendedTracks();

		/// <summary>
		/// Gets recommended playlists for the user
		/// </summary>
		/// <returns>First page of recommded playlists</returns>
		Task<IPagedResponse<IPlaylist>> GetRecommendedPlaylists();

	}

	public class User : IUser
	{
		public uint Id { get; set; }
		public string Name { get; set; }
		public string Link { get; set; }
		public string Country { get; set; }

		public string error { get; set; }

		//Local Serailization info
		private DeezerClient Client { get; set; }
		internal void Deserialize(DeezerClient aClient) { Client = aClient; }


		public Task<IPagedResponse<IAlbum>> GetFavouriteAlbums()
		{
			return null;
		}

		public Task<IPagedResponse<IArtist>> GetFavouriteArtists()
		{
			return null;
		}

		public Task<IPagedResponse<ITrack>> GetFavouriteTracks()
		{
			return null;
		}

		public Task<IPagedResponse<ITrack>> GetUserFlow()
		{
			return null;
		}

		public Task<IPagedResponse<ITrack>> GetListenHistory()
		{
			return null;
		}

		public Task<IPagedResponse<IPlaylist>> GetFavouritePlaylists()
		{
			return Client.GetUserFavouritePlaylists(Id);
		}




		public Task<IPagedResponse<IAlbum>> GetRecommendedAlbums()
		{
			return null;
		}

		public Task<IPagedResponse<IArtist>> GetRecommendedArtists()
		{
			return null;
		}

		public Task<IPagedResponse<ITrack>> GetRecommendedTracks()
		{
			return null;
		}

		public Task<IPagedResponse<IPlaylist>> GetRecommendedPlaylists()
		{
			return null;
		}



		public override string ToString()
		{
			return Name;
		}
	}
}
