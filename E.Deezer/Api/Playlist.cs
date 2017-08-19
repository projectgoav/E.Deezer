using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using RestSharp.Deserializers;

namespace E.Deezer.Api
{
	public interface IPlaylist : IObjectWithImage
    {
		int Id { get; set; }
		string Title { get; set; }
		bool Public { get; set; }
		uint NumTracks { get; set; }
		string Link { get; set; }
		string CreatorName { get; }
        int Rating { get; }
		bool IsLovedTrack { get; set; }

        Task<IEnumerable<ITrack>> GetTracks();
        Task<IEnumerable<ITrack>> GetTracks(uint aCount);
		Task<IEnumerable<ITrack>> GetTracks(uint aStart, uint aCount);

        Task<bool> Rate(int aRating);

        //Manage Tracks
        Task<bool> AddTrack(ITrack aTrack);
        Task<bool> AddTrack(Int64 aTrackId);

        Task<bool> AddTracks(IEnumerable<ITrack> aTracks);
        Task<bool> AddTracks(IEnumerable<Int64> aTrackIds);
        Task<bool> AddTracks(string aTrackIds);

        Task<bool> RemoveTrack(ITrack aTrack);
        Task<bool> RemoveTrack(Int64 aTrackId);

        Task<bool> RemoveTracks(IEnumerable<ITrack> aTracks);
        Task<bool> RemoveTracks(IEnumerable<Int64> aTrackIds);
        Task<bool> RemoveTracks(string aTrackIds);

        Task<bool> AddPlaylistToFavorite();
        Task<bool> RemovePlaylistFromFavorite();
    }

	internal class Playlist : ObjectWithImage, IPlaylist, IDeserializable<DeezerClient>
	{
		public int Id { get; set; }
		public string Title { get; set; }
        public bool Public { get; set; }
		public string Link { get; set; }
        public int Rating { get; set; }
		public string CreatorName
		{
			//Required as sometime playlist creator is references as Creator and sometimes references as User
			get
			{
				if (UserInternal == null && CreatorInternal == null) { return string.Empty; }
				return (UserInternal == null) ? CreatorInternal.Name : UserInternal.Name;
			}
		}

        [DeserializeAs(Name = "nb_tracks")]
        public uint NumTracks { get; set; }

        [DeserializeAs(Name = "is_loved_track")]
        public bool IsLovedTrack { get; set; }

		[DeserializeAs(Name = "user")]
		public User UserInternal { get; set; }

		[DeserializeAs(Name = "creator")]
		public User CreatorInternal { get; set; }


    	public DeezerClient Client { get; set; }
		public void Deserialize(DeezerClient aClient)
        {
            Client = aClient;

            if (UserInternal != null)
            {
                UserInternal.Deserialize(aClient);
            }

            if (CreatorInternal != null)
            {
                CreatorInternal.Deserialize(aClient);
            }
        }


        public Task<IEnumerable<ITrack>> GetTracks() { return GetTracks(0, Client.ResultSize); }
        public Task<IEnumerable<ITrack>> GetTracks(uint aCount) {  return GetTracks(0, aCount); }
		public Task<IEnumerable<ITrack>> GetTracks(uint aStart, uint aCount)
		{
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", Id)
            };

            return Client.Get<Track>("playlist/{id}/tracks", parms, aStart, aCount).ContinueWith<IEnumerable<ITrack>>((aTask) =>
            {
                return Client.Transform<Track, ITrack>(aTask.Result);
            }, Client.CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
		}

        public Task<bool> Rate(int aRating)
        {
            if (aRating < 1 || aRating > 5) { throw new ArgumentOutOfRangeException("aRating", "Rating value should be between 1 and 5 (inclusive)"); }

            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", Id),
                RequestParameter.GetNewQueryStringParameter("note", aRating)
            };

            return Client.Post("playlist/{id}", parms, DeezerPermissions.BasicAccess);
        }


        public Task<bool> AddTrack(ITrack aTrack) { return AddTrack(aTrack.Id); }
        public Task<bool> AddTrack(Int64 aTrackId) { return AddTracks(aTrackId.ToString()); }

        public Task<bool> AddTracks(IEnumerable<Int64> aTrackIds)
        {
            if (aTrackIds.Count() > 0)
            {
                return AddTracks(string.Join(",", aTrackIds.Select((v) => v.ToString())));
            }
            else
            {
                throw new ArgumentException("Must provide at least one track ID", "aTrackIds");
            }

        }

        public Task<bool> AddTracks(IEnumerable<ITrack> aTracks)
        {
            if (aTracks.Count() > 0)
            {
                return AddTracks(string.Join(",", aTracks.Select((v) => v.Id.ToString())));
            }
            else
            {
                throw new ArgumentException("Must provide at least one track ID", "aTrackIds");
            }
        }

        public Task<bool> AddTracks(string aTrackIds)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("playlist_id", Id),
                RequestParameter.GetNewQueryStringParameter("songs", aTrackIds)
            };

            return Client.Post("playlist/{playlist_id}/tracks", parms, DeezerPermissions.ManageLibrary);
        }


        public Task<bool> RemoveTrack(ITrack aTrack) { return RemoveTrack(aTrack.Id); }
        public Task<bool> RemoveTrack(Int64 aTrackId) { return RemoveTracks(aTrackId.ToString()); }

        public Task<bool> RemoveTracks(IEnumerable<Int64> aTrackIds)
        {
            if (aTrackIds.Count() > 0)
            {
                return RemoveTracks(string.Join(",", aTrackIds.Select((v) => v.ToString())));
            }
            else
            {
                throw new ArgumentException("Must provide at least one track ID", "aTrackIds");
            }

        }

        public Task<bool> RemoveTracks(IEnumerable<ITrack> aTracks)
        {
            if (aTracks.Count() > 0)
            {
                return RemoveTracks(string.Join(",", aTracks.Select((v) => v.Id.ToString())));
            }
            else
            {
                throw new ArgumentException("Must provide at least one track ID", "aTrackIds");
            }
        }

        public Task<bool> RemoveTracks(string aTrackIds)
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("playlist_id", Id),
                RequestParameter.GetNewQueryStringParameter("songs", aTrackIds)
            };

            return Client.Delete("playlist/{playlist_id}/tracks", parms, DeezerPermissions.ManageLibrary | DeezerPermissions.DeleteLibrary);
        }

        public Task<bool> AddPlaylistToFavorite() => Client.User.AddPlaylistToFavorite(Id);

        public Task<bool> RemovePlaylistFromFavorite() => Client.User.RemovePlaylistFromFavorite(Id);
        
        public override string ToString()
		{
			return string.Format("E.Deezer: Playlist({0} [{1}])", Title, CreatorName);
		}        
    }
}
