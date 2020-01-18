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
    public interface IAlbumEndpoint
    {
        Task<IAlbum> GetById(ulong albumId, CancellationToken cancellationToken);

        Task<IEnumerable<ITrack>> GetAlbumTracks(IAlbum album, CancellationToken cancellationToken);
        Task<IEnumerable<ITrack>> GetAlbumTracks(ulong albumId, CancellationToken cancellationToken);


        Task<IEnumerable<IUserProfile>> GetAlbumFans(IAlbum album, CancellationToken cancellationToken, uint start = 0, uint count = 10);
        Task<IEnumerable<IUserProfile>> GetAlbumFans(ulong albimId, CancellationToken cancellationToken, uint start = 0, uint count = 10);

        Task<IEnumerable<IComment>> GetAlbumComments(IAlbum album, CancellationToken cancellationToken, uint start = 0, uint count = 10);
        Task<IEnumerable<IComment>> GetAlbumComments(ulong albumId, CancellationToken cancellationToken, uint start = 0, uint count = 10);


        Task<bool> RateAlbum(IAlbum album, DeezerRating rating, CancellationToken cancellationToken);
        Task<bool> RateAlbum(ulong albumId, DeezerRating rating, CancellationToken cancellationToken);

        Task<ulong> AddComment(IAlbum album, string commentText, CancellationToken cancellationToken);
        Task<ulong> AddComment(ulong albumIf, string commentText, CancellationToken cancellationToken);
    }


    internal class AlbumEndpoint : IAlbumEndpoint
    {
        private const string kStartParam = "index";
        private const string kLimitParam = "limit";

        private readonly IDeezerClient client;

        public AlbumEndpoint(IDeezerClient client)
        {
            this.client = client;
        }


        // IAlbumEndpoint
        public Task<IAlbum> GetById(ulong albumId, CancellationToken cancellationToken)
            => this.client.Get($"/album/{albumId}", cancellationToken, json => Api.Album.FromJson(json, this.client));


        public Task<IEnumerable<ITrack>> GetAlbumTracks(IAlbum album, CancellationToken cancellationToken)
        {
            album.ThrowIfNull();

            if (album is Album albumImpl)
            {
                bool hasTracklist = albumImpl.TracklistInternal != null;
                bool tracklistPopulated = hasTracklist && albumImpl.TracklistInternal.Any();

                if (hasTracklist && tracklistPopulated)
                {
                    return Task.FromResult<IEnumerable<ITrack>>(albumImpl.TracklistInternal);
                }
            }

            return GetAlbumTracks(album.Id, cancellationToken);
        }

        public Task<IEnumerable<ITrack>> GetAlbumTracks(ulong albumId, CancellationToken cancellationToken)
            => this.client.Get($"/album/{albumId}/tracks",
                               cancellationToken,
                               json => FragmentOf<ITrack>.FromJson(json, x => Api.Track.FromJson(x, this.client)));



        public Task<IEnumerable<IUserProfile>> GetAlbumFans(IAlbum album, CancellationToken cancellationToken, uint start = 0, uint count = 10)
        {
            album.ThrowIfNull();

            return GetAlbumFans(album.Id, cancellationToken, start, count);
        }

        public Task<IEnumerable<IUserProfile>> GetAlbumFans(ulong albumId, CancellationToken cancellationToken, uint start = 0, uint count = 10)
            => this.client.Get($"/album/{albumId}/fans?{kStartParam}={start}&{kLimitParam}={count}",
                               cancellationToken,
                               json => FragmentOf<IUserProfile>.FromJson(json, x => Api.UserProfile.FromJson(x, this.client)));


        public Task<IEnumerable<IComment>> GetAlbumComments(IAlbum album, CancellationToken cancellationToken, uint start = 0, uint count = 10)
        {
            album.ThrowIfNull();

            return GetAlbumComments(album.Id, cancellationToken, start, count);
        }

        public Task<IEnumerable<IComment>> GetAlbumComments(ulong albumId, CancellationToken cancellationToken, uint start = 0, uint count = 10)
            => this.client.Get($"/album/{albumId}/comments?{kStartParam}={start}&{kLimitParam}={count}",
                               cancellationToken,
                               json => FragmentOf<IComment>.FromJson(json, x => Api.Comment.FromJson(x, this.client)));



        public Task<bool> RateAlbum(IAlbum album, DeezerRating rating, CancellationToken cancellationToken)
        {
            album.ThrowIfNull();

            return RateAlbum(album.Id, rating, cancellationToken);
        }



        public Task<bool> RateAlbum(ulong albumId, DeezerRating rating, CancellationToken cancellationToken)
            => this.client.Post($"/album/{albumId}?{rating.AsRatingQueryParam()}",
                                DeezerPermissions.BasicAccess,
                                cancellationToken);

        public Task<ulong> AddComment(IAlbum album, string commentText, CancellationToken cancellationToken)
        {
            album.ThrowIfNull();

            return AddComment(album.Id, commentText, cancellationToken);
        }

        public Task<ulong> AddComment(ulong albumId, string commentText, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(commentText))
            {
                throw new ArgumentNullException(nameof(commentText));
            }

            var formattedComment = Uri.EscapeUriString(commentText);

            return this.client.Post<ulong>($"/album/{albumId}/comments?comment={formattedComment}",
                                           DeezerPermissions.BasicAccess,
                                           cancellationToken,
                                           json => CommentCreationResponse.FromJson(json));
        }
    }
}
