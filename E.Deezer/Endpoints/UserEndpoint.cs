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
    public interface IUserEndpoint
    {
        Task<IUserProfile> GetById(ulong userId, CancellationToken cancellationToken);

        Task<IEnumerable<ITrack>> GetFlow(IUserProfile user, CancellationToken cancellationToken, uint start = 0, uint count = 50);
        Task<IEnumerable<ITrack>> GetFlow(ulong userId, CancellationToken cancellationToken, uint start = 0, uint count = 50);


        Task<IEnumerable<IUserProfile>> GetFollowers(IUserProfile user, CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IUserProfile>> GetFollowers(ulong userId, CancellationToken cancellationToken, uint start = 0, uint count = 25);

        Task<IEnumerable<IUserProfile>> GetFollowings(IUserProfile user, CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IUserProfile>> GetFollowings(ulong userId, CancellationToken cancellationToken, uint start = 0, uint count = 25);


        Task<IEnumerable<ITrack>> GetListeningHistory(CancellationToken cancellationToken, uint start = 0, uint count = 25);


        Task<IEnumerable<IAlbum>> GetFavouriteAlbums(IUserProfile user, CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IAlbum>> GetFavouriteAlbums(ulong userId, CancellationToken cancellationToken, uint start = 0, uint count = 25);

        Task<IEnumerable<IArtist>> GetFavouriteArtists(IUserProfile user, CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IArtist>> GetFavouriteArtists(ulong userId, CancellationToken cancellationToken, uint start = 0, uint count = 25);

        Task<IEnumerable<ITrack>> GetFavouriteTracks(IUserProfile user, CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<ITrack>> GetFavouriteTracks(ulong userId, CancellationToken cancellationToken, uint start = 0, uint count = 25);

        Task<IEnumerable<IRadio>> GetFavouriteRadio(IUserProfile user, CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IRadio>> GetFavouriteRadio(ulong userId, CancellationToken cancellationToken, uint start = 0, uint count = 25);

        Task<IEnumerable<IPlaylist>> GetFavouritePlaylists(IUserProfile user, CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IPlaylist>> GetFavouritePlaylists(ulong userId, CancellationToken cancellationToken, uint start = 0, uint count = 25);

        Task<IEnumerable<IAlbum>> GetRecommendedAlbums(CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IArtist>> GetRecommendedArtists(CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists(CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<ITrack>> GetRecommendedTracks(CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IRadio>> GetRecommendedRadio(CancellationToken cancellationToken, uint start = 0, uint count = 25);

        /* Deezer playlist collection contains both user created playlists & favourited playlists.
         * Filters can be achieved by using filters on the properties exposed by IPlaylist to determin
         * if the playlist is a personal or favourite. */
        Task<IEnumerable<IPlaylist>> GetPlaylists(IUserProfile user, CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IPlaylist>> GetPlaylists(ulong userId, CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IPlaylist>> GetPlaylists(CancellationToken cancellationToken, uint start = 0, uint count = 25);



        Task<bool> FavouriteAlbum(IAlbum album, CancellationToken cancellationToken);
        Task<bool> FavouriteAlbum(ulong albumId, CancellationToken cancellationToken);

        Task<bool> UnfavouriteAlbum(IAlbum album, CancellationToken cancellationToken);
        Task<bool> UnfavouriteAlbum(ulong albumId, CancellationToken cancellationToken);


        Task<bool> FavouriteArtist(IArtist artist, CancellationToken cancellationToken);
        Task<bool> FavouriteArtist(ulong artistId, CancellationToken cancellationToken);

        Task<bool> UnfavouriteArtist(IArtist artist, CancellationToken cancellationToken);
        Task<bool> UnfavouriteArtist(ulong artistId, CancellationToken cancellationToken);


        Task<bool> FavouriteTrack(ITrack track, CancellationToken cancellationToken);
        Task<bool> FavouriteTrack(ulong trackId, CancellationToken cancellationToken);

        Task<bool> UnfavouriteTrack(ITrack track, CancellationToken cancellationToken);
        Task<bool> UnfavouriteTrack(ulong trackId, CancellationToken cancellationToken);


        Task<bool> FavouritePlaylist(IPlaylist playlist, CancellationToken cancellationToken);
        Task<bool> FavouritePlaylist(ulong playlistId, CancellationToken cancellationToken);

        Task<bool> UnfavouritePlaylist(IPlaylist playlist, CancellationToken cancellationToken);
        Task<bool> UnfavouritePlaylist(ulong playlistId, CancellationToken cancellationToken);


        Task<bool> FavouriteRadio(IRadio radio, CancellationToken cancellationToken);
        Task<bool> FavouriteRadio(ulong radioId, CancellationToken cancellationToken);

        Task<bool> UnfavouriteRadio(IRadio radio, CancellationToken cancellationToken);
        Task<bool> UnfavouriteRadio(ulong radioId, CancellationToken cancellationToken);
    }

    internal class UserEndpoint : IUserEndpoint
    {
        private const string START_PARAM = "index";
        private const string COUNT_PARAM = "limit";


        private readonly IDeezerClient client;

        public UserEndpoint(IDeezerClient client)
        {
            this.client = client;
        }



        public Task<IUserProfile> GetById(ulong userId, CancellationToken cancellationToken)
            => this.client.Get($"user/{userId}",
                               cancellationToken,
                               json => Api.UserProfile.FromJson(json, this.client));


        public Task<IEnumerable<ITrack>> GetFlow(IUserProfile user, CancellationToken cancellationToken, uint start = 0, uint count = 50)
        {
            user.ThrowIfNull();

            return GetFlow(user.Id, cancellationToken, start, count);
        }

        public Task<IEnumerable<ITrack>> GetFlow(ulong userId, CancellationToken cancellationToken, uint start = 0, uint count = 50)
            => this.client.Get($"user/{userId}/flow?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => FragmentOf<ITrack>.FromJson(json, x => Api.Track.FromJson(x, this.client)));



        public Task<IEnumerable<ITrack>> GetListeningHistory(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.client.Get($"user/me/history?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               DeezerPermissions.ListeningHistory,
                               cancellationToken,
                               json => FragmentOf<ITrack>.FromJson(json, x => Api.Track.FromJson(x, this.client)));



        public Task<IEnumerable<IAlbum>> GetFavouriteAlbums(IUserProfile user, CancellationToken cancellationToken, uint start = 0, uint count = 25)
        {
            user.ThrowIfNull();

            return GetFavouriteAlbums(user.Id, cancellationToken, start, count);
        }

        public Task<IEnumerable<IAlbum>> GetFavouriteAlbums(ulong userId, CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.client.Get($"user/{userId}/albums?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => FragmentOf<IAlbum>.FromJson(json, x => Api.Album.FromJson(x, this.client)));


        public Task<IEnumerable<IArtist>> GetFavouriteArtists(IUserProfile user, CancellationToken cancellationToken, uint start = 0, uint count = 25)
        {
            user.ThrowIfNull();

            return GetFavouriteArtists(user.Id, cancellationToken, start, count);
        }

        public Task<IEnumerable<IArtist>> GetFavouriteArtists(ulong userId, CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.client.Get($"user/{userId}/artists?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => FragmentOf<IArtist>.FromJson(json, x => Api.Artist.FromJson(x, this.client)));


        public Task<IEnumerable<IPlaylist>> GetFavouritePlaylists(IUserProfile user, CancellationToken cancellationToken, uint start = 0, uint count = 25)
        {
            user.ThrowIfNull();

            return GetFavouritePlaylists(user.Id, cancellationToken, start, count);
        }

        public Task<IEnumerable<IPlaylist>> GetFavouritePlaylists(ulong userId, CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.client.Get($"user/{userId}/playlists?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => FragmentOf<IPlaylist>.FromJson(json, x => Api.Playlist.FromJson(x, this.client)));


        public Task<IEnumerable<ITrack>> GetFavouriteTracks(IUserProfile user, CancellationToken cancellationToken, uint start = 0, uint count = 25)
        {
            user.ThrowIfNull();

            return GetFavouriteTracks(user.Id, cancellationToken, start, count);
        }

        public Task<IEnumerable<ITrack>> GetFavouriteTracks(ulong userId, CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.client.Get($"user/{userId}/tracks?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => FragmentOf<ITrack>.FromJson(json, x => Api.Track.FromJson(x, this.client)));


        public Task<IEnumerable<IRadio>> GetFavouriteRadio(IUserProfile user, CancellationToken cancellationToken, uint start = 0, uint count = 25)
        {
            user.ThrowIfNull();

            return GetFavouriteRadio(user.Id, cancellationToken, start, count);
        }

        public Task<IEnumerable<IRadio>> GetFavouriteRadio(ulong userId, CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.client.Get($"user/{userId}/radios?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => FragmentOf<IRadio>.FromJson(json, x => Api.Radio.FromJson(x, this.client)));


        public Task<IEnumerable<IUserProfile>> GetFollowers(IUserProfile user, CancellationToken cancellationToken, uint start = 0, uint count = 25)
        {
            user.ThrowIfNull();

            return GetFollowers(user.Id, cancellationToken, start, count);
        }

        public Task<IEnumerable<IUserProfile>> GetFollowers(ulong userId, CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.client.Get($"user/{userId}/followers?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => FragmentOf<IUserProfile>.FromJson(json, x => Api.UserProfile.FromJson(x, this.client)));

        public Task<IEnumerable<IUserProfile>> GetFollowings(IUserProfile user, CancellationToken cancellationToken, uint start = 0, uint count = 25)
        {
            user.ThrowIfNull();

            return GetFollowings(user.Id, cancellationToken, start, count);
        }

        public Task<IEnumerable<IUserProfile>> GetFollowings(ulong userId, CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.client.Get($"user/{userId}/followings?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => FragmentOf<IUserProfile>.FromJson(json, x => Api.UserProfile.FromJson(x, this.client)));


        // RECOMMENDS
        // FIX ME: I don't think the API methods actually listen to the paging
        //         params that are passed...
        public Task<IEnumerable<IAlbum>> GetRecommendedAlbums(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.client.Get($"user/me/recommendations/albums?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               DeezerPermissions.BasicAccess,
                               cancellationToken,
                               json => FragmentOf<IAlbum>.FromJson(json, x => Api.Album.FromJson(x, this.client)));

        public Task<IEnumerable<IArtist>> GetRecommendedArtists(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.client.Get($"user/me/recommendations/artists?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               DeezerPermissions.BasicAccess,
                               cancellationToken,
                               json => FragmentOf<IArtist>.FromJson(json, x => Api.Artist.FromJson(x, this.client)));

        public Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.client.Get($"user/me/recommendations/playlists?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               DeezerPermissions.BasicAccess,
                               cancellationToken,
                               json => FragmentOf<IPlaylist>.FromJson(json, x => Api.Playlist.FromJson(x, this.client)));

        public Task<IEnumerable<ITrack>> GetRecommendedTracks(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.client.Get($"user/me/recommendations/tracks?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               DeezerPermissions.BasicAccess,
                               cancellationToken,
                               json => FragmentOf<ITrack>.FromJson(json, x => Api.Track.FromJson(x, this.client)));

        public Task<IEnumerable<IRadio>> GetRecommendedRadio(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.client.Get($"user/me/recommendations/radios?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               DeezerPermissions.BasicAccess,
                               cancellationToken,
                               json => FragmentOf<IRadio>.FromJson(json, x => Api.Radio.FromJson(x, this.client)));


        public Task<IEnumerable<IPlaylist>> GetPlaylists(IUserProfile userProfile, CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => GetPlaylists(userProfile.Id, cancellationToken, start, count);

        public Task<IEnumerable<IPlaylist>> GetPlaylists(ulong userId, CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.client.Get($"user/{userId}/playlists?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               DeezerPermissions.ManageLibrary,
                               cancellationToken,
                               json => FragmentOf<IPlaylist>.FromJson(json, x => Api.Playlist.FromJson(x, this.client)));

        public Task<IEnumerable<IPlaylist>> GetPlaylists(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.client.Get($"user/me/playlists?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               DeezerPermissions.ManageLibrary,
                               cancellationToken,
                               json => FragmentOf<IPlaylist>.FromJson(json, x => Api.Playlist.FromJson(x, this.client)));




        // FAVOURITING
        public Task<bool> FavouriteAlbum(IAlbum album, CancellationToken cancellationToken)
        {
            album.ThrowIfNull();

            return FavouriteAlbum(album.Id, cancellationToken);
        }

        public Task<bool> FavouriteAlbum(ulong albumId, CancellationToken cancellationToken)
            => this.client.Post($"user/me/albums?album_id={albumId}",
                                DeezerPermissions.ManageLibrary,
                                cancellationToken);


        public Task<bool> UnfavouriteAlbum(IAlbum album, CancellationToken cancellationToken)
        {
            album.ThrowIfNull();

            return UnfavouriteAlbum(album.Id, cancellationToken);
        }

        public Task<bool> UnfavouriteAlbum(ulong albumId, CancellationToken cancellationToken)
            => this.client.Delete($"user/me/albums?album_id={albumId}",
                                 DeezerPermissions.ManageLibrary | DeezerPermissions.DeleteLibrary,
                                 cancellationToken);



        public Task<bool> FavouriteArtist(IArtist artist, CancellationToken cancellationToken)
        {
            artist.ThrowIfNull();

            return FavouriteArtist(artist.Id, cancellationToken);
        }

        public Task<bool> FavouriteArtist(ulong artistId, CancellationToken cancellationToken)
            => this.client.Post($"user/me/artists?artist_id={artistId}",
                                DeezerPermissions.ManageLibrary,
                                cancellationToken);


        public Task<bool> UnfavouriteArtist(IArtist artist, CancellationToken cancellationToken)
        {
            artist.ThrowIfNull();

            return UnfavouriteArtist(artist.Id, cancellationToken);
        }

        public Task<bool> UnfavouriteArtist(ulong artistId, CancellationToken cancellationToken)
            => this.client.Delete($"user/me/artists?artist_id={artistId}",
                                 DeezerPermissions.ManageLibrary | DeezerPermissions.DeleteLibrary,
                                 cancellationToken);



        public Task<bool> FavouritePlaylist(IPlaylist playlist, CancellationToken cancellationToken)
        {
            playlist.ThrowIfNull();

            return FavouritePlaylist(playlist.Id, cancellationToken);
        }

        public Task<bool> FavouritePlaylist(ulong playlistId, CancellationToken cancellationToken)
            => this.client.Post($"user/me/playlists?playlist_id={playlistId}",
                                DeezerPermissions.ManageLibrary,
                                cancellationToken);


        public Task<bool> UnfavouritePlaylist(IPlaylist playlist, CancellationToken cancellationToken)
        {
            playlist.ThrowIfNull();

            return UnfavouritePlaylist(playlist.Id, cancellationToken);
        }

        public Task<bool> UnfavouritePlaylist(ulong playlistId, CancellationToken cancellationToken)
            => this.client.Delete($"user/me/playlists?playlist_id={playlistId}",
                                 DeezerPermissions.ManageLibrary | DeezerPermissions.DeleteLibrary,
                                 cancellationToken);



        public Task<bool> FavouriteTrack(ITrack track, CancellationToken cancellationToken)
        {
            track.ThrowIfNull();

            return FavouriteTrack(track.Id, cancellationToken);
        }

        public Task<bool> FavouriteTrack(ulong trackId, CancellationToken cancellationToken)
            => this.client.Post($"user/me/tracks?track_id={trackId}",
                                DeezerPermissions.ManageLibrary,
                                cancellationToken);


        public Task<bool> UnfavouriteTrack(ITrack track, CancellationToken cancellationToken)
        {
            track.ThrowIfNull();

            return UnfavouriteTrack(track.Id, cancellationToken);
        }

        public Task<bool> UnfavouriteTrack(ulong trackId, CancellationToken cancellationToken)
            => this.client.Delete($"user/me/tracks?track_id={trackId}",
                                 DeezerPermissions.ManageLibrary | DeezerPermissions.DeleteLibrary,
                                 cancellationToken);




        public Task<bool> FavouriteRadio(IRadio radio, CancellationToken cancellationToken)
        {
            radio.ThrowIfNull();

            return FavouriteRadio(radio.Id, cancellationToken);
        }

        public Task<bool> FavouriteRadio(ulong radioId, CancellationToken cancellationToken)
            => this.client.Post($"user/me/radios?radio_id={radioId}",
                                DeezerPermissions.ManageLibrary,
                                cancellationToken);


        public Task<bool> UnfavouriteRadio(IRadio radio, CancellationToken cancellationToken)
        {
            radio.ThrowIfNull();

            return UnfavouriteRadio(radio.Id, cancellationToken);
        }

        public Task<bool> UnfavouriteRadio(ulong radioId, CancellationToken cancellationToken)
            => this.client.Delete($"user/me/radios?radio_id={radioId}",
                                 DeezerPermissions.ManageLibrary | DeezerPermissions.DeleteLibrary,
                                 cancellationToken);
    }
}
