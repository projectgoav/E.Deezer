using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using RestSharp.Deserializers;

namespace E.Deezer.Api
{
	/// <summary>
	/// Deezer Playlist object
	/// </summary>
	public interface IPlaylist
	{
		/// <summary>
		/// Deezer library ID number
		/// </summary>
		int Id { get; set; }

		/// <summary>
		/// Playlist title
		/// </summary>
		string Title { get; set; }

		/// <summary>
		/// Public availablity of playlist
		/// </summary>
		bool Public { get; set; }

		/// <summary>
		/// Number of tracks in playlist
		/// </summary>
		uint NumTracks { get; set; }

		/// <summary>
		/// Deezer.com link to playlist
		/// </summary>
		string Link { get; set; }

		/// <summary>
		/// Link to playlist picture
		/// </summary>
		string Picture { get; set; }

		/// <summary>
		/// Link to playlist tracklist
		/// </summary>
		string Tracklist { get; set; }

		/// <summary>
		/// Username of playlist creator
		/// </summary>
		string CreatorName { get; }

		/// <summary>
		/// If true, then playlist is "loved tracks"
		/// </summary>
		bool IsLovedTrack { get; set; }

		/// <summary>
		/// Gets the tracks in the playlist
		/// </summary>
		/// <returns>First page of tracks in playlist.</returns>
		Task<IPagedResponse<ITrack>> GetTracks(int aResultSize = DeezerSession.RESULT_SIZE);

	}

	internal class Playlist : IPlaylist, IDeserializable<DeezerClient>
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public bool Public { get; set; }

		[DeserializeAs(Name = "nb_tracks")]
		public uint NumTracks { get; set; }

		public string Link { get; set; }
		public string Picture { get; set; }
		public string Tracklist { get; set; }
		[DeserializeAs(Name = "is_loved_track")]
		public bool IsLovedTrack { get; set; }
		public string CreatorName
		{
			//Required as sometime playlist creator is references as Creator and sometimes references as User
			get
			{
				if (UserInternal == null && CreatorInternal == null) { return string.Empty; }
				return (UserInternal == null) ? CreatorInternal.Name : UserInternal.Name;
			}
		}

		[DeserializeAs(Name = "user")]
		public User UserInternal { get; set; }

		[DeserializeAs(Name = "creator")]
		public User CreatorInternal { get; set; }

		public DeezerClient Client { get; set; }
		public void Deserialize(DeezerClient aClient) { Client = aClient; }



		public Task<IPagedResponse<ITrack>> GetTracks(int aResultSize = DeezerSession.RESULT_SIZE)
		{
			return Client.GetPlaylistTracks(Id, aResultSize);
		}


		public override string ToString()
		{
			return string.Format("E.Deezer: Playlist({0} [{1}])", Title, CreatorName);
		}
	}
}
