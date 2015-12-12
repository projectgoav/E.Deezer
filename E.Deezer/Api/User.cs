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
		Task<IEnumerable<IAlbum>> GetFavouriteAlbums();

		/// <summary>
		/// Gets the user's favourite artists
		/// </summary>
		/// <returns>A book of user's favourite artists</returns>
		Task<IEnumerable<IArtist>> GetFavouriteArtists();

		/// <summary>
		/// Gets the user's favourite tracks
		/// </summary>
		/// <returns>A book of user's favourite tracks</returns>
		Task<IEnumerable<ITrack>> GetFavouriteTracks();

		/// <summary>
		/// Gets the user's "flow"
		/// </summary>
		/// <returns>A book of user's "flow"</returns>
		Task<IEnumerable<ITrack>> GetUserFlow();

		/// <summary>
		/// Gets the user;s listening history
		/// </summary>
		/// <returns>A book of user's litening history</returns>
		Task<IEnumerable<ITrack>> GetListenHistory();

		/// <summary>
		/// Gets the user's favourite playlists
		/// </summary>
		/// <returns>A book of user's listening history</returns>
		Task<IEnumerable<IPlaylist>> GetFavouritePlaylists();


		/// <summary>
		/// Gets recommended albums for the user
		/// </summary>
		/// <returns>A book of recommended albums</returns>
		Task<IEnumerable<IAlbum>> GetRecommendedAlbums();

		/// <summary>
		/// Gets recommended artists for the user
		/// </summary>
		/// <returns>A book of recommended artists</returns>
		Task<IEnumerable<IArtist>> GetRecommendedArtists();

		/// <summary>
		/// Gets recommended tracks for the user
		/// </summary>
		/// <returns>A book of recommended tracks</returns>
		Task<IEnumerable<ITrack>> GetRecommendedTracks();

		/// <summary>
		/// Gets recommended playlists for the user
		/// </summary>
		/// <returns>A book of recommded playlists</returns>
		Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists();

	}

	public class User : IUser, IDeserializable<DeezerClientV2>
	{
		public uint Id { get; set; }
		public string Name { get; set; }
		public string Link { get; set; }
		public string Country { get; set; }

		public string error { get; set; }

		//Local Serailization info
		public DeezerClientV2 Client { get; set; }
		public void Deserialize(DeezerClientV2 aClient) { Client = aClient; }


		public Task<IEnumerable<IAlbum>> GetFavouriteAlbums()
		{
            throw new NotImplementedException();
		}

		public Task<IEnumerable<IArtist>> GetFavouriteArtists()
		{
            throw new NotImplementedException();
		}

		public Task<IEnumerable<ITrack>> GetFavouriteTracks()
		{
            throw new NotImplementedException();
		}

		public Task<IEnumerable<ITrack>> GetUserFlow()
		{
            throw new NotImplementedException();
		}

		public Task<IEnumerable<ITrack>> GetListenHistory()
		{
            throw new NotImplementedException();
		}

		public Task<IEnumerable<IPlaylist>> GetFavouritePlaylists()
		{
            throw new NotImplementedException();
		}




		public Task<IEnumerable<IAlbum>> GetRecommendedAlbums()
		{
            throw new NotImplementedException();
		}

		public Task<IEnumerable<IArtist>> GetRecommendedArtists()
		{
            throw new NotImplementedException();
		}

		public Task<IEnumerable<ITrack>> GetRecommendedTracks()
		{
            throw new NotImplementedException();
		}

		public Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists()
		{
            throw new NotImplementedException();
		}



		public override string ToString()
		{
			return Name;
		}
	}
}
