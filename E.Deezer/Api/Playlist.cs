using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using RestSharp.Deserializers;

namespace E.Deezer.Api
{
	public interface IPlaylist
    {
		int Id { get; set; }
		string Title { get; set; }
		bool Public { get; set; }
		uint NumTracks { get; set; }
		string Link { get; set; }
		string Artwork { get; set; }
		string CreatorName { get; }
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
