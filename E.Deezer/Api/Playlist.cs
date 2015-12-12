using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using RestSharp.Deserializers;

namespace E.Deezer.Api
{
    /// <summary>
    /// Represents an Playlist in the Deezer Library
    /// </summary>
	public interface IPlaylist
    {
        /// <summary>
        /// Gets the Deezer ID of this Playlist
        /// </summary>
		int Id { get; set; }

        /// <summary>
        /// Gets the title of this Playlist
        /// </summary>
		string Title { get; set; }

        /// <summary>
        /// Gets if the Playlist is public or not
        /// </summary>
		bool Public { get; set; }
        
        /// <summary>
        /// Gets the number of tracks contained in Playlist
        /// </summary>
		uint NumTracks { get; set; }

        /// <summary>
        /// Gets the www.deezer.com link to this Playlist
        /// </summary>
		string Link { get; set; }

        /// <summary>
        /// Gets the link to the artwork for this Playlist
        /// </summary>
		string Artwork { get; set; }
        
        /// <summary>
        /// Gets the name of the aUthor of this Playlist
        /// </summary>
		string CreatorName { get; }
        
        /// <summary>
        /// Gets if this Playlist is the "loved tracks" playlist
        /// </summary>
		bool IsLovedTrack { get; set; }

        Task<IEnumerable<ITrack>> GetTracks();
        Task<IEnumerable<ITrack>> GetTracks(uint aCount);
		Task<IEnumerable<ITrack>> GetTracks(uint aStart, uint aCount);

	}

	internal class Playlist : IPlaylist, IDeserializable<DeezerClientV2>
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public bool Public { get; set; }

		[DeserializeAs(Name = "nb_tracks")]
		public uint NumTracks { get; set; }
		public string Link { get; set; }
        
        [DeserializeAs(Name="picture")]
		public string Artwork { get; set; }
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

		public DeezerClientV2 Client { get; set; }
		public void Deserialize(DeezerClientV2 aClient) { Client = aClient; }


        public Task<IEnumerable<ITrack>> GetTracks() { return GetTracks(0, DeezerSessionV2.DEFAULT_SIZE); }
        public Task<IEnumerable<ITrack>> GetTracks(uint aCount) {  return GetTracks(0, aCount); }
		public Task<IEnumerable<ITrack>> GetTracks(uint aStart, uint aCount)
		{
            string[] parms = new string[] { "URL", "id", Id.ToString() };

            return Client.Get<Track>("playlist/{id}/tracks", parms, aStart, aCount).ContinueWith<IEnumerable<ITrack>>((aTask) =>
            {
                List<ITrack> items = new List<ITrack>();

                foreach(var t in aTask.Result.Items)
                {
                    t.Deserialize(Client);
                    items.Add(t);
                }

                return items;
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
		}

		public override string ToString()
		{
			return string.Format("E.Deezer: Playlist({0} [{1}])", Title, CreatorName);
		}
	}
}
