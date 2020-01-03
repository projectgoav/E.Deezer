using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using E.Deezer.Api.Internal;

namespace E.Deezer.Api
{
	public interface IPlaylist
    {
		ulong Id { get; }
        uint NumberOfFans { get; }
        int Rating { get; }
        string Link { get; }
        string Title { get; }
        bool IsPublic { get; }
        uint Duration { get; }
        uint NumberOfTracks { get; }
        string ShareLink { get; }
        bool IsLovedTrack { get; }
        string Description { get; }
        IImages Images { get; }
        IUserProfile Creator { get; }
        bool IsCollaborative { get; }

        // TODO
        uint UnseenTrackCount { get; }


        Task<IEnumerable<ITrack>> Tracks(CancellationToken cancellationToken, uint start = 0, uint count = 50);

        Task<IEnumerable<IUserProfile>> Fans(CancellationToken cancellationToken, uint start = 0, uint count = 10);

        Task<IEnumerable<IComment>> Comments(CancellationToken cancellationToken, uint start = 0, uint count = 10);


        Task<bool> Rate(DeezerRating rating, CancellationToken cancellationToken);

        Task<ulong> CommentOn(string commentText, CancellationToken cancellationToken);


        Task<bool> Favourite(CancellationToken cancellationToken);
        Task<bool> Unfavourite(CancellationToken cancellationToken);

        Task<bool> SetSeen(CancellationToken cancellationToken);


        Task<bool> AddTrack(ITrack track, CancellationToken cancellationToken);
        Task<bool> AddTrack(ulong trackId, CancellationToken cancellationToken);

        Task<bool> AddTracks(IEnumerable<ITrack> tracks, CancellationToken cancellationToken);
        Task<bool> AddTracks(IEnumerable<ulong> trackIds, CancellationToken cancellationToken);


        Task<bool> RemoveTrack(ITrack track, CancellationToken cancellationToken);
        Task<bool> RemoveTrack(ulong trackId, CancellationToken cancellationToken);

        Task<bool> RemoveTracks(IEnumerable<ITrack> tracks, CancellationToken cancellationToken);
        Task<bool> RemoveTracks(IEnumerable<ulong> trackIds, CancellationToken cancellationToken);

    }

    internal class Playlist : IPlaylist, IClientObject
    {
        public ulong Id { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public string Link { get; private set; }

        public uint Duration { get; private set; }

        public int Rating { get; private set; }

        public uint NumberOfFans { get; private set; }

        public IUserProfile Creator { get; private set; }

        public bool IsPublic { get; private set; }

        public bool IsCollaborative { get; private set; }

        public uint UnseenTrackCount { get; private set; }

        public string ShareLink { get; private set; }

        public uint NumberOfTracks { get; private set; }

        public bool IsLovedTrack { get; private set; }

        public IImages Images { get; private set; }

        internal IEnumerable<ITrack> TracklistInternal { get; private set; }


        // IClientObject
        public IDeezerClient Client { get; private set; }


        public Task<IEnumerable<ITrack>> Tracks(CancellationToken cancellationToken, uint start = 0, uint count = 50)
            => this.Client.Endpoints.Playlists.GetTracks(this, cancellationToken, start, count);

        public Task<IEnumerable<IUserProfile>> Fans(CancellationToken cancellationToken, uint start = 0, uint count = 10)
            => this.Client.Endpoints.Playlists.GetFans(this, cancellationToken, start, count);

        public Task<IEnumerable<IComment>> Comments(CancellationToken cancellationToken, uint start = 0, uint count = 10)
            => this.Client.Endpoints.Playlists.GetComments(this, cancellationToken, start, count);


        public Task<bool> Rate(DeezerRating rating, CancellationToken cancellationToken)
            => this.Client.Endpoints.Playlists.RatePlaylist(this, rating, cancellationToken);

        public Task<ulong> CommentOn(string commentText, CancellationToken cancellationToken)
            => this.Client.Endpoints.Playlists.CommentOnPlaylist(this, commentText, cancellationToken);



        public Task<bool> Favourite(CancellationToken cancellationToken)
            => this.Client.Endpoints.User.FavouritePlaylist(this, cancellationToken);

        public Task<bool> Unfavourite(CancellationToken cancellationToken)
            => this.Client.Endpoints.User.UnfavouritePlaylist(this, cancellationToken);



        public Task<bool> SetSeen(CancellationToken cancellationToken)
            => this.Client.Endpoints.Playlists.SetPlaylistSeen(this, cancellationToken);


        public Task<bool> AddTrack(ITrack track, CancellationToken cancellationToken)
            => this.Client.Endpoints.Playlists.AddTrackTo(this, track, cancellationToken);

        public Task<bool> AddTrack(ulong trackId, CancellationToken cancellationToken)
            => this.Client.Endpoints.Playlists.AddTrackTo(this, trackId, cancellationToken);


        public Task<bool> AddTracks(IEnumerable<ITrack> tracks, CancellationToken cancellationToken)
            => this.Client.Endpoints.Playlists.AddTracksTo(this, tracks, cancellationToken);

        public Task<bool> AddTracks(IEnumerable<ulong> trackIds, CancellationToken cancellationToken)
            => this.Client.Endpoints.Playlists.AddTracksTo(this, trackIds, cancellationToken);



        public Task<bool> RemoveTrack(ITrack track, CancellationToken cancellationToken)
            => this.Client.Endpoints.Playlists.RemoveTrackFrom(this, track, cancellationToken);

        public Task<bool> RemoveTrack(ulong trackId, CancellationToken cancellationToken)
            => this.Client.Endpoints.Playlists.RemoveTrackFrom(this, trackId, cancellationToken);


        public Task<bool> RemoveTracks(IEnumerable<ITrack> tracks, CancellationToken cancellationToken)
            => this.Client.Endpoints.Playlists.RemoveTracksFrom(this, tracks, cancellationToken);

        public Task<bool> RemoveTracks(IEnumerable<ulong> trackIds, CancellationToken cancellationToken)
            => this.Client.Endpoints.Playlists.RemoveTracksFrom(this, trackIds, cancellationToken);


        public override string ToString()
            => string.Format("E.Deezer: Playlist({0} [{1}])", Title, Creator?.Username ?? "<UNKOWN>");


        //JSON
        internal const string ID_PROPERTY_NAME = "id";
        internal const string TITLE_PROPERTY_NAME = "title";
        internal const string DESCRIPTION_PROPERTY_NAME = "description";
        internal const string DURATION_PROPERTY_NAME = "duration";
        internal const string PUBLIC_PROPERTY_NAME = "public";
        internal const string LOVED_TRACKS_PROPERTY_NAME = "is_loved_track";
        internal const string COLLABORATIVE_PROPERTY_NAME = "collaborative";
        internal const string TRACK_COUNT_PROPERTY_NAME = "nb_tracks";
        internal const string FANS_PROPERTY_NAME = "fans";
        internal const string LINK_PROPERTY_NAME = "link";
        internal const string SHARE_LINK_PROPERTY_NAME = "share";
        internal const string CREATOR_PROPERTY_NAME = "creator";
        internal const string USER_PROPERTY_NAME = "user";
        internal const string TRACKS_PROPERTY_NAME = "tracks";


        // TODO: UnseenTrackCount
        // TODO: CreationDate
        // TODO: Rating

        public static IPlaylist FromJson(JToken json, IDeezerClient client)
        {
            return new Playlist()
            {
                Id = ulong.Parse(json.Value<string>(ID_PROPERTY_NAME)),

                Title = json.Value<string>(TITLE_PROPERTY_NAME),
                Description = json.Value<string>(DESCRIPTION_PROPERTY_NAME),

                IsPublic = json.Value<bool>(PUBLIC_PROPERTY_NAME),
                IsCollaborative = json.Value<bool>(COLLABORATIVE_PROPERTY_NAME),
                IsLovedTrack = json.Value<bool>(LOVED_TRACKS_PROPERTY_NAME),

                Duration = json.Value<uint>(DURATION_PROPERTY_NAME),

                NumberOfFans = json.Value<uint>(FANS_PROPERTY_NAME),
                NumberOfTracks = json.Value<uint>(TRACK_COUNT_PROPERTY_NAME),

                Link = json.Value<string>(LINK_PROPERTY_NAME),
                ShareLink = json.Value<string>(SHARE_LINK_PROPERTY_NAME),

                Images = Api.Images.FromJson(json),

                Creator = Api.UserProfile.FromJson(json[CREATOR_PROPERTY_NAME], client) ?? Api.UserProfile.FromJson(json[USER_PROPERTY_NAME], client),
                TracklistInternal = FragmentOf<ITrack>.FromJson(json[TRACKS_PROPERTY_NAME], x => Api.Track.FromJson(x, client)),

                Client = client,
            };
        }
    }
}
