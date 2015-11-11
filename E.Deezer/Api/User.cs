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
		/// <returns>A book of user's favourite albums</returns>
		Task<IBook<IAlbum>> GetFavouriteAlbums();

		/// <summary>
		/// Gets the user's favourite artists
		/// </summary>
		/// <returns>A book of user's favourite artists</returns>
		Task<IBook<IArtist>> GetFavouriteArtists();

		/// <summary>
		/// Gets the user's favourite tracks
		/// </summary>
		/// <returns>A book of user's favourite tracks</returns>
		Task<IBook<ITrack>> GetFavouriteTracks();

		/// <summary>
		/// Gets the user's "flow"
		/// </summary>
		/// <returns>A book of user's "flow"</returns>
		Task<IBook<ITrack>> GetUserFlow();

		/// <summary>
		/// Gets the user;s listening history
		/// </summary>
		/// <returns>A book of user's litening history</returns>
		Task<IBook<ITrack>> GetListenHistory();

		/// <summary>
		/// Gets the user's favourite playlists
		/// </summary>
		/// <returns>A book of user's listening history</returns>
		Task<IBook<IPlaylist>> GetFavouritePlaylists();


		/// <summary>
		/// Gets recommended albums for the user
		/// </summary>
		/// <returns>A book of recommended albums</returns>
		Task<IBook<IAlbum>> GetRecommendedAlbums();

		/// <summary>
		/// Gets recommended artists for the user
		/// </summary>
		/// <returns>A book of recommended artists</returns>
		Task<IBook<IArtist>> GetRecommendedArtists();

		/// <summary>
		/// Gets recommended tracks for the user
		/// </summary>
		/// <returns>A book of recommended tracks</returns>
		Task<IBook<ITrack>> GetRecommendedTracks();

		/// <summary>
		/// Gets recommended playlists for the user
		/// </summary>
		/// <returns>A book of recommded playlists</returns>
		Task<IBook<IPlaylist>> GetRecommendedPlaylists();

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


		public Task<IBook<IAlbum>> GetFavouriteAlbums()
		{
            throw new NotImplementedException();
		}

		public Task<IBook<IArtist>> GetFavouriteArtists()
		{
            throw new NotImplementedException();
		}

		public Task<IBook<ITrack>> GetFavouriteTracks()
		{
            throw new NotImplementedException();
		}

		public Task<IBook<ITrack>> GetUserFlow()
		{
            throw new NotImplementedException();
		}

		public Task<IBook<ITrack>> GetListenHistory()
		{
            throw new NotImplementedException();
		}

		public Task<IBook<IPlaylist>> GetFavouritePlaylists()
		{
			return Client.GetUserFavouritePlaylists(Id);
		}




		public Task<IBook<IAlbum>> GetRecommendedAlbums()
		{
            throw new NotImplementedException();
		}

		public Task<IBook<IArtist>> GetRecommendedArtists()
		{
            throw new NotImplementedException();
		}

		public Task<IBook<ITrack>> GetRecommendedTracks()
		{
            throw new NotImplementedException();
		}

		public Task<IBook<IPlaylist>> GetRecommendedPlaylists()
		{
            throw new NotImplementedException();
		}



		public override string ToString()
		{
			return Name;
		}
	}
}
