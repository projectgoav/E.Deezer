using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

using E.Deezer.Api;
using E.Deezer.Util;
using E.Deezer.Api.Internal;

namespace E.Deezer.Endpoints
{
    public interface IArtistEndpoint
    {
        Task<IArtist> GetById(ulong artistId, CancellationToken cancellationToken);

        Task<IEnumerable<ITrack>> GetArtistsTopTracks(ulong artistId, CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<ITrack>> GetArtistsTopTracks(IArtist artist, CancellationToken cancellationToken, uint start = 0, uint count = 25);

        Task<IEnumerable<IAlbum>> GetArtistsAlbums(ulong artistId, CancellationToken cancellationToken, uint start = 0, uint count = 10);
        Task<IEnumerable<IAlbum>> GetArtistsAlbums(IArtist artist, CancellationToken cancellationToken, uint start = 0, uint count = 10);

        Task<IEnumerable<IUserProfile>> GetArtistsFans(ulong artistId, CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IUserProfile>> GetArtistsFans(IArtist artist, CancellationToken cancellationToken, uint start = 0, uint count = 25);

        Task<IEnumerable<IComment>> GetArtistsComments(ulong artistId, CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IComment>> GetArtistsComments(IArtist artist, CancellationToken cancellationToken, uint start = 0, uint count = 25);

        Task<IEnumerable<IArtist>> GetRelatedArtists(ulong artistId, CancellationToken cancellationToken, uint start = 0, uint count = 10);
        Task<IEnumerable<IArtist>> GetRelatedArtists(IArtist artist, CancellationToken cancellationToken, uint start = 0, uint count = 10);

        Task<IEnumerable<ITrack>> GetArtistsRadio(ulong artistId, CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<ITrack>> GetArtistsRadio(IArtist artist, CancellationToken cancellationToken, uint start = 0, uint count = 25);

        Task<IEnumerable<IPlaylist>> GetPlaylistsFeaturingArtist(ulong artistId, CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IPlaylist>> GetPlaylistsFeaturingArtist(IArtist artist, CancellationToken cancellationToken, uint start = 0, uint count = 25);

        Task<bool> RateArtist(IArtist artist, DeezerRating rating, CancellationToken cancellationToken);
        Task<bool> RateArtist(ulong artistId, DeezerRating rating, CancellationToken cancellationToken);

        Task<ulong> CommentOnArtist(IArtist artist, string commentText, CancellationToken cancellationToken);
        Task<ulong> CommentOnArtist(ulong artistId, string commentText, CancellationToken cancellationToken);
    }


    internal class ArtistEndpoint : IArtistEndpoint
    {
        private const string START_PARAM = "index";
        private const string COUNT_PARAM = "limit";

        private readonly IDeezerClient client;

        public ArtistEndpoint(IDeezerClient client)
        {
            this.client = client;
        }



        public Task<IArtist> GetById(ulong artistId, CancellationToken cancellationToken)
            => this.client.Get($"/artist/{artistId}", cancellationToken, json => Api.Artist.FromJson(json, this.client));


        public Task<IEnumerable<ITrack>> GetArtistsTopTracks(IArtist artist, CancellationToken cancellationToken, uint start = 0, uint count = 25)
        {
            artist.ThrowIfNull();

            return GetArtistsTopTracks(artist.Id, cancellationToken, start, count);
        }

        public Task<IEnumerable<ITrack>> GetArtistsTopTracks(ulong artistId, CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.client.Get($"/artist/{artistId}/top?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => FragmentOf<ITrack>.FromJson(json, x => Api.Track.FromJson(x, this.client)));


        public Task<IEnumerable<IAlbum>> GetArtistsAlbums(IArtist artist, CancellationToken cancellationToken, uint start = 0, uint count = 10)
        {
            artist.ThrowIfNull();

            return GetArtistsAlbums(artist.Id, cancellationToken, start, count);
        }

        public Task<IEnumerable<IAlbum>> GetArtistsAlbums(ulong artistId, CancellationToken cancellationToken, uint start = 0, uint count = 10)
            => this.client.Get($"/artist/{artistId}/albums?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => FragmentOf<IAlbum>.FromJson(json, x => Api.Album.FromJson(x, this.client)));



        public Task<IEnumerable<IUserProfile>> GetArtistsFans(IArtist artist, CancellationToken cancellationToken, uint start = 0, uint count = 10)
        {
            artist.ThrowIfNull();

            return GetArtistsFans(artist.Id, cancellationToken, start, count);
        }

        public Task<IEnumerable<IUserProfile>> GetArtistsFans(ulong artistId, CancellationToken cancellationToken, uint start = 0, uint count = 10)
            => this.client.Get($"/artist/{artistId}/fans?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => FragmentOf<IUserProfile>.FromJson(json, x => Api.UserProfile.FromJson(x, this.client)));


        public Task<IEnumerable<IComment>> GetArtistsComments(IArtist artist, CancellationToken cancellationToken, uint start = 0, uint count = 10)
        {
            artist.ThrowIfNull();

            return GetArtistsComments(artist.Id, cancellationToken, start, count);
        }

        public Task<IEnumerable<IComment>> GetArtistsComments(ulong artistId, CancellationToken cancellationToken, uint start = 0, uint count = 10)
            => this.client.Get($"/artist/{artistId}/comments?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => FragmentOf<IComment>.FromJson(json, x => Api.Comment.FromJson(x, this.client)));



        public Task<IEnumerable<IArtist>> GetRelatedArtists(IArtist artist, CancellationToken cancellationToken, uint start = 0, uint count = 10)
        {
            artist.ThrowIfNull();

            return GetRelatedArtists(artist.Id, cancellationToken, start, count);
        }

        public Task<IEnumerable<IArtist>> GetRelatedArtists(ulong artistId, CancellationToken cancellationToken, uint start = 0, uint count = 10)
            => this.client.Get($"/artist/{artistId}/related?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => FragmentOf<IArtist>.FromJson(json, x => Api.Artist.FromJson(x, this.client)));



        public Task<IEnumerable<ITrack>> GetArtistsRadio(IArtist artist, CancellationToken cancellationToken, uint start = 0, uint count = 25)
        {
            artist.ThrowIfNull();

            if (!artist.HasSmartRadio)
            {
                //TODO: Assert or throw exception here??
                //TODO: Is null a correct value, perhaps we should return an empty list?
                return Task.FromResult<IEnumerable<ITrack>>(null);
            }

            return GetArtistsRadio(artist.Id, cancellationToken, start, count);
        }

        public Task<IEnumerable<ITrack>> GetArtistsRadio(ulong artistId, CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.client.Get($"/artist/{artistId}/radio?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => FragmentOf<ITrack>.FromJson(json, x => Api.Track.FromJson(x, this.client)));


        public Task<IEnumerable<IPlaylist>> GetPlaylistsFeaturingArtist(IArtist artist, CancellationToken cancellationToken, uint start = 0, uint count = 10)
        {
            artist.ThrowIfNull();

            return GetPlaylistsFeaturingArtist(artist.Id, cancellationToken, start, count);
        }

        public Task<IEnumerable<IPlaylist>> GetPlaylistsFeaturingArtist(ulong artistId, CancellationToken cancellationToken, uint start = 0, uint count = 10)
            => this.client.Get($"/artist/{artistId}/playlists?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => FragmentOf<IPlaylist>.FromJson(json, x => Api.Playlist.FromJson(x, this.client)));




        public Task<bool> RateArtist(IArtist artist, DeezerRating rating, CancellationToken cancellationToken)
        {
            artist.ThrowIfNull();

            return RateArtist(artist.Id, rating, cancellationToken);
        }

        public Task<bool> RateArtist(ulong artistId, DeezerRating rating, CancellationToken cancellationToken)
            => this.client.Post($"/artist/{artistId}?{rating.AsRatingQueryParam()}",
                                DeezerPermissions.BasicAccess,
                                cancellationToken);


        public Task<ulong> CommentOnArtist(IArtist artist, string commentText, CancellationToken cancellationToken)
        {
            artist.ThrowIfNull();

            return CommentOnArtist(artist.Id, commentText, cancellationToken);
        }

        public Task<ulong> CommentOnArtist(ulong artistId, string commentText, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(commentText))
            {
                throw new ArgumentNullException(nameof(commentText));
            }

            var formattedComment = Uri.EscapeUriString(commentText);

            return this.client.Post<ulong>($"/artist/{artistId}/comments?comment={formattedComment}",
                                           DeezerPermissions.BasicAccess,
                                           cancellationToken,
                                           json => CommentCreationResponse.FromJson(json));
        }
    }
}
