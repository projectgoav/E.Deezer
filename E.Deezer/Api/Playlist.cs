using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using Newtonsoft.Json;

namespace E.Deezer.Api
{
	public interface IPlaylist : IObjectWithImage
    {
		ulong Id { get; }
        uint Fans { get; }
        int Rating { get; }
        string Link { get; }
        string Title { get; }
        bool IsPublic { get; }
        uint Duration { get; }
        uint TrackCount { get; }
        string ShareLink { get; }
        bool IsLovedTrack { get; }
        string CreatorName { get; }
        string Description { get; }
        IUserProfile Creator { get; }
        bool IsCollaborative { get; }
        uint UnseenTrackCount { get; }


        [Obsolete("Use of IsPublic is encouraged")]
        bool Public { get; }

        [Obsolete("Use of TrackCount is encouraged")]
        uint NumTracks { get; }


        Task<IEnumerable<ITrack>> GetTracks(uint aStart = 0, uint aCount = uint.MaxValue);

        Task<bool> Rate(int aRating);

        //Manage Tracks
        Task<bool> AddTrack(ITrack aTrack);
        Task<bool> AddTrack(ulong aTrackId);

        Task<bool> AddTracks(IEnumerable<ITrack> aTracks);
        Task<bool> AddTracks(IEnumerable<ulong> aTrackIds);
        Task<bool> AddTracks(string aTrackIds);

        Task<bool> RemoveTrack(ITrack aTrack);
        Task<bool> RemoveTrack(ulong aTrackId);

        Task<bool> RemoveTracks(IEnumerable<ITrack> aTracks);
        Task<bool> RemoveTracks(IEnumerable<ulong> aTrackIds);
        Task<bool> RemoveTracks(string aTrackIds);

        Task<bool> AddPlaylistToFavorite();
        Task<bool> RemovePlaylistFromFavorite();
    }

	internal class Playlist : ObjectWithImage, IPlaylist, IDeserializable<IDeezerClient>
	{
		public ulong Id
        {
            get;
            set;
        }

		public string Title
        {
            get;
            set;
        }

        [Obsolete("Use of IsPublic is encouraged")]
        public bool Public => IsPublic;

        public bool IsPublic
        {
            get;
            set;
        }

		public string Link
        {
            get;
            set;
        }

        public int Rating
        {
            get;
            set;
        }

		public string CreatorName
		{
			//Required as sometime playlist creator is references as Creator and sometimes references as User
			get
			{
				if (UserInternal == null && CreatorInternal == null) { return string.Empty; }
				return (UserInternal == null) ? CreatorInternal.Name : UserInternal.Name;
			}
		}

        [JsonProperty(PropertyName = "nb_tracks")]
        public uint NumTracks
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "is_loved_track")]
        public bool IsLovedTrack
        {
            get;
            set;
        }

		[JsonProperty(PropertyName = "user")]
		public User UserInternal
        {
            get;
            set;
        }

		[JsonProperty(PropertyName = "creator")]
		public User CreatorInternal
        {
            get; 
            set;
        }

        
        //IDeserializable
    	public IDeezerClient Client
        {
            get;
            set;
        }

		public void Deserialize(IDeezerClient aClient)
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


        public Task<bool> AddTrack(ITrack aTrack)
            => AddTrack(aTrack.Id);

        public Task<bool> AddTrack(ulong aTrackId)
            => AddTracks(aTrackId.ToString());


        public Task<bool> AddTracks(IEnumerable<ulong> aTrackIds)
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


        public Task<bool> RemoveTrack(ITrack aTrack) 
            => RemoveTrack(aTrack.Id);

        public Task<bool> RemoveTrack(ulong aTrackId)
            => RemoveTracks(aTrackId.ToString());


        public Task<bool> RemoveTracks(IEnumerable<ulong> aTrackIds)
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

        public Task<bool> AddPlaylistToFavorite() 
            => Client.User.AddPlaylistToFavourite(Id);

        public Task<bool> RemovePlaylistFromFavorite()
            => Client.User.RemovePlaylistFromFavourite(Id);
        

        public override string ToString()
		{
			return string.Format("E.Deezer: Playlist({0} [{1}])", Title, CreatorName);
		}        
    }
}
