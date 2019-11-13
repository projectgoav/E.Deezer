using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        Task<bool> SetSeen();

        Task<IEnumerable<IUserProfile>> GetFans(uint aStart = 0, uint aCount = 25);

        Task<IEnumerable<IComment>> GetComments(uint aStart = 0, uint aCount = 10);

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

        Task<bool> AddComment(string commentText);
    }

    internal class Playlist : ObjectWithImage, IPlaylist, IDeserializable<IDeezerClient>
    {
        public ulong Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }

        public uint Duration { get; set; }

        public int Rating { get; set; }

        public uint Fans { get; set; }

        public IUserProfile Creator => CreatorInternal;

        public string CreatorName => CreatorInternal?.Username;

        [Obsolete("Use of IsPublic is encouraged")]
        public bool Public => IsPublic;

        [Obsolete("User of TrackCount is encouraged")]
        public uint NumTracks => TrackCount;

        [JsonProperty(PropertyName = "public")]
        public bool IsPublic { get; set; }

        [JsonProperty(PropertyName = "collaborative")]
        public bool IsCollaborative { get; set; }

        [JsonProperty(PropertyName = "unseen_track_count")]
        public uint UnseenTrackCount { get; set; }

        [JsonProperty(PropertyName = "share")]
        public string ShareLink { get; set; }

        [JsonProperty(PropertyName = "nb_tracks")]
        public uint TrackCount { get; set; }

        [JsonProperty(PropertyName = "is_loved_track")]
        public bool IsLovedTrack { get; set; }

        [JsonProperty(PropertyName = "creator")]
        public UserProfile CreatorInternal { get; set; }

        //IDeserializable
        public IDeezerClient Client { get; set; }

        public void Deserialize(IDeezerClient aClient)
        {
            Client = aClient;
            CreatorInternal?.Deserialize(aClient);
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

        public Task<bool> AddComment(string commentText)
        {
            if (string.IsNullOrEmpty(commentText))
            {
                throw new ArgumentException("A comment is required.");
            }

            List<IRequestParameter> p = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", this.Id),
                RequestParameter.GetNewQueryStringParameter("comment", commentText),
            };

            return Client.Post("playlist/{id}/comments", p, DeezerPermissions.BasicAccess);
        }

        public Task<bool> SetSeen()
        {
            List<IRequestParameter> p = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", this.Id),
            };

            return Client.Post("playlist/{id}/seen", p, DeezerPermissions.BasicAccess);
        }

        public Task<IEnumerable<IUserProfile>> GetFans(uint aStart = 0, uint aCount = 25)
        {
            List<IRequestParameter> p = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", this.Id)
            };

            return Client.Get<UserProfile>("playlist/{id}/fans", p, aStart, aCount)
                         .ContinueWith<IEnumerable<IUserProfile>>(task => Client.Transform<UserProfile, IUserProfile>(task.Result),
                                                                  Client.CancellationToken,
                                                                  TaskContinuationOptions.NotOnCanceled,
                                                                  TaskScheduler.Default);
        }

        public Task<IEnumerable<IComment>> GetComments(uint aStart = 0, uint aCount = 10)
        {
            List<IRequestParameter> p = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", this.Id)
            };

            return Client.Get<Comment>("playlist/{id}/comments", p, aStart, aCount)
                         .ContinueWith<IEnumerable<IComment>>(task => Client.Transform<Comment, IComment>(task.Result),
                                                                  Client.CancellationToken,
                                                                  TaskContinuationOptions.NotOnCanceled,
                                                                  TaskScheduler.Default);
        }

        public override string ToString()
            => string.Format("E.Deezer: Playlist({0} [{1}])", Title, CreatorName);
    }
}
