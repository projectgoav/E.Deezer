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


		//Methods
		Task<IEnumerable<IAlbum>> GetFavouriteAlbums();
		Task<IEnumerable<IArtist>> GetFavouriteArtists();
		Task<IEnumerable<ITrack>> GetFavouriteTracks();
		Task<IEnumerable<ITrack>> GetUserFlow();
		Task<IEnumerable<ITrack>> GetListenHistory();
		Task<IEnumerable<IPlaylist>> GetFavouritePlaylists();
		Task<IEnumerable<IAlbum>> GetRecommendedAlbums();
		Task<IEnumerable<IArtist>> GetRecommendedArtists();
		Task<IEnumerable<ITrack>> GetRecommendedTracks();
		Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists();

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
