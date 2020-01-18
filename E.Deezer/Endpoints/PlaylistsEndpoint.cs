using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using System.Threading;
using System.Threading.Tasks;

using E.Deezer.Api;
using E.Deezer.Util;
using E.Deezer.Api.Internal;

namespace E.Deezer.Endpoints
{
    public interface IPlaylistsEndpoint
    {
        Task<IPlaylist> GetById(ulong playlistId, CancellationToken cancellationToken);

        Task<IEnumerable<ITrack>> GetTracks(IPlaylist playlist, CancellationToken cancellationToken, uint start = 0, uint count = 50);
        Task<IEnumerable<ITrack>> GetTracks(ulong playlistId, CancellationToken cancellationToken, uint start = 0, uint count = 50);

        Task<IEnumerable<IComment>> GetComments(IPlaylist playlist, CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IComment>> GetComments(ulong playlistId, CancellationToken cancellationToken, uint start = 0, uint count = 25);

        Task<IEnumerable<IUserProfile>> GetFans(IPlaylist playlist, CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IUserProfile>> GetFans(ulong playlistId, CancellationToken cancellationToken, uint start = 0, uint count = 25);


        Task<bool> RatePlaylist(IPlaylist playlist, DeezerRating rating, CancellationToken cancellationToken);
        Task<bool> RatePlaylist(ulong playlistId, DeezerRating rating, CancellationToken cancellationToken);

        Task<ulong> CommentOnPlaylist(IPlaylist playlist, string commentText, CancellationToken cancellationToken);
        Task<ulong> CommentOnPlaylist(ulong playlistId, string commentText, CancellationToken cancellationToken);



        Task<bool> SetPlaylistSeen(IPlaylist playlist, CancellationToken cancellationToken);
        Task<bool> SetPlaylistSeen(ulong playlistId, CancellationToken cancellationToken);


        Task<ulong> CreatePlaylist(string playlistName, CancellationToken cancellationToken);

        Task<bool> DeletePlaylist(IPlaylist playlist, CancellationToken cancellationToken);
        Task<bool> DeletePlaylist(ulong playlistId, CancellationToken cancellationToken);



        Task<bool> AddTrackTo(IPlaylist playlist, ITrack track, CancellationToken cancellationToken);
        Task<bool> AddTrackTo(IPlaylist playlist, ulong trackId, CancellationToken cancellationToken);

        Task<bool> AddTrackTo(ulong playlistId, ITrack track, CancellationToken cancellationToken);
        Task<bool> AddTrackTo(ulong playlistId, ulong trackId, CancellationToken cancellationToken);


        Task<bool> AddTracksTo(IPlaylist playlist, IEnumerable<ITrack> tracks, CancellationToken cancellationToken);
        Task<bool> AddTracksTo(IPlaylist playlist, IEnumerable<ulong> trackIds, CancellationToken cancellationToken);

        Task<bool> AddTracksTo(ulong playlistId, IEnumerable<ITrack> tracks, CancellationToken cancellationToken);
        Task<bool> AddTracksTo(ulong playlistId, IEnumerable<ulong> trackIds, CancellationToken cancellationToken);



        Task<bool> RemoveTrackFrom(IPlaylist playlist, ITrack track, CancellationToken cancellationToken);
        Task<bool> RemoveTrackFrom(IPlaylist playlist, ulong trackId, CancellationToken cancellationToken);

        Task<bool> RemoveTrackFrom(ulong playlistId, ITrack track, CancellationToken cancellationToken);
        Task<bool> RemoveTrackFrom(ulong playlistId, ulong trackId, CancellationToken cancellationToken);


        Task<bool> RemoveTracksFrom(IPlaylist playlist, IEnumerable<ITrack> tracks, CancellationToken cancellationToken);
        Task<bool> RemoveTracksFrom(IPlaylist playlist, IEnumerable<ulong> trackIds, CancellationToken cancellationToken);

        Task<bool> RemoveTracksFrom(ulong playlistId, IEnumerable<ITrack> tracks, CancellationToken cancellationToken);
        Task<bool> RemoveTracksFrom(ulong playlistId, IEnumerable<ulong> trackIds, CancellationToken cancellationToken);

    }

    internal class PlaylistsEndpoint : IPlaylistsEndpoint
    {
        private const string START_PARAM = "index";
        private const string COUNT_PARAM = "limit";

        private readonly IDeezerClient client;

        public PlaylistsEndpoint(IDeezerClient client)
        {
            this.client = client;
        }


        public Task<IPlaylist> GetById(ulong playlistId, CancellationToken cancellationToken)
            => this.client.Get($"/playlist/{playlistId}",
                               cancellationToken,
                               json => Api.Playlist.FromJson(json, this.client));


        public Task<IEnumerable<ITrack>> GetTracks(IPlaylist playlist, CancellationToken cancellationToken, uint start = 0, uint count = 50)
        {
            playlist.ThrowIfNull();

            if (playlist is Playlist playlistImpl)
            {
                if (playlistImpl.TracklistInternal != null)
                {
                    var internalTracklist = playlistImpl.TracklistInternal.Skip((int)start)
                                                                          .Take((int)count)
                                                                          .ToList();
                    if (internalTracklist.Count == count)
                    {
                        return Task.FromResult<IEnumerable<ITrack>>(internalTracklist);
                    }
                }
            }

            return GetTracks(playlist.Id, cancellationToken, start, count);
        }

        public Task<IEnumerable<ITrack>> GetTracks(ulong playlistId, CancellationToken cancellationToken, uint start = 0, uint count = 50)
            => this.client.Get($"playlist/{playlistId}/tracks?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => FragmentOf<ITrack>.FromJson(json, x => Api.Track.FromJson(x, this.client)));


        public Task<IEnumerable<IComment>> GetComments(IPlaylist playlist, CancellationToken cancellationToken, uint start = 0, uint count = 25)
        {
            playlist.ThrowIfNull();

            return GetComments(playlist.Id, cancellationToken, start, count);
        }

        public Task<IEnumerable<IComment>> GetComments(ulong playlistId, CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.client.Get($"playlist/{playlistId}/comments?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => FragmentOf<IComment>.FromJson(json, x => Api.Comment.FromJson(x, this.client)));


        public Task<IEnumerable<IUserProfile>> GetFans(IPlaylist playlist, CancellationToken cancellationToken, uint start = 0, uint count = 25)
        {
            playlist.ThrowIfNull();

            return GetFans(playlist.Id, cancellationToken, start, count);
        }

        public Task<IEnumerable<IUserProfile>> GetFans(ulong playlistId, CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.client.Get($"playlist/{playlistId}/fans?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => FragmentOf<IUserProfile>.FromJson(json, x => Api.UserProfile.FromJson(x, this.client)));



        public Task<bool> RatePlaylist(IPlaylist playlist, DeezerRating rating, CancellationToken cancellationToken)
        {
            playlist.ThrowIfNull();

            return RatePlaylist(playlist.Id, rating, cancellationToken);
        }

        public Task<bool> RatePlaylist(ulong playlistId, DeezerRating rating, CancellationToken cancellationToken)
            => this.client.Post($"/playlist/{playlistId}?{rating.AsRatingQueryParam()}",
                                DeezerPermissions.BasicAccess,
                                cancellationToken);


        public Task<ulong> CommentOnPlaylist(IPlaylist playlist, string commentText, CancellationToken cancellationToken)
        {
            playlist.ThrowIfNull();

            return CommentOnPlaylist(playlist.Id, commentText, cancellationToken);
        }

        public Task<ulong> CommentOnPlaylist(ulong playlistId, string commentText, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(commentText))
            {
                throw new ArgumentNullException(nameof(commentText));
            }

            var formattedComment = Uri.EscapeUriString(commentText);

            return this.client.Post<ulong>($"/playlist/{playlistId}/comments?comment={formattedComment}",
                                           DeezerPermissions.BasicAccess,
                                           cancellationToken,
                                           json => CommentCreationResponse.FromJson(json));
        }


        // MANAGEMENT METHODS
        public Task<bool> SetPlaylistSeen(IPlaylist playlist, CancellationToken cancellationToken)
        {
            playlist.ThrowIfNull();

            return SetPlaylistSeen(playlist.Id, cancellationToken);
        }

        public Task<bool> SetPlaylistSeen(ulong playlistId, CancellationToken cancellationToken)
            => this.client.Post($"playlist/{playlistId}/seen",
                                DeezerPermissions.ManageLibrary,
                                cancellationToken);



        public Task<ulong> CreatePlaylist(string playlistName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(playlistName))
            {
                throw new ArgumentException("No playlist name given.", nameof(playlistName));
            }

            var formattedPlaylistTitle = Uri.EscapeUriString(playlistName);

            return this.client.Post<ulong>($"user/me/playlists?title={formattedPlaylistTitle}",
                                           DeezerPermissions.ManageLibrary,
                                           cancellationToken,
                                           json => PlaylistCreationResponse.FromJson(json));
        }



        public Task<bool> DeletePlaylist(IPlaylist playlist, CancellationToken cancellationToken)
        {
            playlist.ThrowIfNull();

            return DeletePlaylist(playlist.Id, cancellationToken);
        }

        public Task<bool> DeletePlaylist(ulong playlistId, CancellationToken cancellationToken)
            => this.client.Delete($"playlist{playlistId}",
                                  DeezerPermissions.DeleteLibrary,
                                  cancellationToken);



        public Task<bool> AddTrackTo(IPlaylist playlist, ITrack track, CancellationToken cancellationToken)
        {
            playlist.ThrowIfNull();

            track.ThrowIfNull();

            return AddTrackTo(playlist.Id, track.Id, cancellationToken);
        }


        public Task<bool> AddTrackTo(IPlaylist playlist, ulong trackId, CancellationToken cancellationToken)
        {
            playlist.ThrowIfNull();

            return AddTrackTo(playlist.Id, trackId, cancellationToken);
        }

        public Task<bool> AddTrackTo(ulong playlistId, ITrack track, CancellationToken cancellationToken)
        {
            track.ThrowIfNull();

            return AddTrackTo(playlistId, track.Id, cancellationToken);
        }

        public Task<bool> AddTrackTo(ulong playlistId, ulong trackId, CancellationToken cancellationToken)
            => AddTracksTo(playlistId, new ulong[] { trackId }, cancellationToken);


        public Task<bool> AddTracksTo(IPlaylist playlist, IEnumerable<ITrack> tracks, CancellationToken cancellationToken)
        {
            playlist.ThrowIfNull();

            if (tracks.Any(x => x == null))
            {
                throw new ArgumentException("Null track given.", nameof(tracks));
            }

            return AddTracksTo(playlist.Id, tracks.Select(x => x.Id), cancellationToken);
        }

        public Task<bool> AddTracksTo(IPlaylist playlist, IEnumerable<ulong> trackIds, CancellationToken cancellationToken)
        {
            playlist.ThrowIfNull();

            if (!trackIds.Any())
            {
                throw new ArgumentException("No tracks specified", nameof(trackIds));
            }

            return AddTracksTo(playlist.Id, trackIds, cancellationToken);
        }


        public Task<bool> AddTracksTo(ulong playlistId, IEnumerable<ITrack> tracks, CancellationToken cancellationToken)
        {
            if (tracks.Any(x => x == null))
            {
                throw new ArgumentException("Null track given.", nameof(tracks));
            }

            return AddTracksTo(playlistId, tracks.Select(x => x.Id), cancellationToken);
        }


        public Task<bool> AddTracksTo(ulong playlistId, IEnumerable<ulong> trackIds, CancellationToken cancellationToken)
        {
            if (!trackIds.Any())
            {
                throw new ArgumentException("No tracks specified", nameof(trackIds));
            }

            var combinedIds = string.Join(",", trackIds);
            var escapedCombinedIds = Uri.EscapeUriString(combinedIds);

            return this.client.Post($"playlist/{playlistId}/tracks?songs={escapedCombinedIds}",
                                    DeezerPermissions.ManageLibrary,
                                    cancellationToken);
        }




        public Task<bool> RemoveTrackFrom(IPlaylist playlist, ITrack track, CancellationToken cancellationToken)
        {
            playlist.ThrowIfNull();

            track.ThrowIfNull();

            return RemoveTrackFrom(playlist.Id, track.Id, cancellationToken);
        }


        public Task<bool> RemoveTrackFrom(IPlaylist playlist, ulong trackId, CancellationToken cancellationToken)
        {



            return RemoveTrackFrom(playlist.Id, trackId, cancellationToken);
        }

        public Task<bool> RemoveTrackFrom(ulong playlistId, ITrack track, CancellationToken cancellationToken)
        {
            track.ThrowIfNull();

            return RemoveTrackFrom(playlistId, track.Id, cancellationToken);
        }

        public Task<bool> RemoveTrackFrom(ulong playlistId, ulong trackId, CancellationToken cancellationToken)
            => RemoveTracksFrom(playlistId, new ulong[] { trackId }, cancellationToken);


        public Task<bool> RemoveTracksFrom(IPlaylist playlist, IEnumerable<ITrack> tracks, CancellationToken cancellationToken)
        {
            playlist.ThrowIfNull();

            if (tracks.Any(x => x == null))
            {
                throw new ArgumentException("Null track given.", nameof(tracks));
            }

            return RemoveTracksFrom(playlist.Id, tracks.Select(x => x.Id), cancellationToken);
        }

        public Task<bool> RemoveTracksFrom(IPlaylist playlist, IEnumerable<ulong> trackIds, CancellationToken cancellationToken)
        {
            playlist.ThrowIfNull();

            if (!trackIds.Any())
            {
                throw new ArgumentException("No tracks specified", nameof(trackIds));
            }

            return RemoveTracksFrom(playlist.Id, trackIds, cancellationToken);
        }


        public Task<bool> RemoveTracksFrom(ulong playlistId, IEnumerable<ITrack> tracks, CancellationToken cancellationToken)
        {
            if (tracks.Any(x => x == null))
            {
                throw new ArgumentException("Null track given.", nameof(tracks));
            }

            return RemoveTracksFrom(playlistId, tracks.Select(x => x.Id), cancellationToken);
        }


        public Task<bool> RemoveTracksFrom(ulong playlistId, IEnumerable<ulong> trackIds, CancellationToken cancellationToken)
        {
            if (!trackIds.Any())
            {
                throw new ArgumentException("No tracks specified", nameof(trackIds));
            }

            var combinedIds = string.Join(",", trackIds);
            var escapedCombinedIds = Uri.EscapeUriString(combinedIds);

            return this.client.Post($"playlist/{playlistId}/tracks?songs={escapedCombinedIds}",
                                    DeezerPermissions.ManageLibrary,
                                    cancellationToken);
        }

    }
}
