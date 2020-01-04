using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

using E.Deezer.Api;
using E.Deezer.Api.Internal;

namespace E.Deezer.Endpoints
{
    public interface ISearchEndpoint
    {
        Task<IEnumerable<IAlbum>> FindAlbums(string searchTerm, CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IArtist>> FindArtists(string searchTerm, CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IPlaylist>> FindPlaylists(string searchTerm, CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<ITrack>> FindTracks(string searchTerm, CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IRadio>> FindRadio(string searchTerm, CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IUserProfile>> FindUsers(string searchTerm, CancellationToken cancellationToken, uint start = 0, uint count = 25);
    }

    internal class SearchEndpoint : ISearchEndpoint
    {
        private const string START_PARAM = "index";
        private const string COUNT_PARAM = "limit";

        private readonly IDeezerClient client;

        public SearchEndpoint(IDeezerClient client)
        {
            this.client = client;
        }


        public Task<IEnumerable<IAlbum>> FindAlbums(string searchTerm, CancellationToken cancellationToken, uint start = 0, uint count = 25)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                throw new ArgumentException("No search term given.", nameof(searchTerm));
            }

            var escapedSearchTerm = Uri.EscapeUriString(searchTerm);

            return this.client.Get($"search/album?q={escapedSearchTerm}&{START_PARAM}={start}&{COUNT_PARAM}={count}",
                                   cancellationToken,
                                   json => FragmentOf<IAlbum>.FromJson(json, x => Api.Album.FromJson(x, this.client)));
        }


        public Task<IEnumerable<IArtist>> FindArtists(string searchTerm, CancellationToken cancellationToken, uint start = 0, uint count = 25)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                throw new ArgumentException("No search term given.", nameof(searchTerm));
            }

            var escapedSearchTerm = Uri.EscapeUriString(searchTerm);

            return this.client.Get($"search/artist?q={escapedSearchTerm}&{START_PARAM}={start}&{COUNT_PARAM}={count}",
                                   cancellationToken,
                                   json => FragmentOf<IArtist>.FromJson(json, x => Api.Artist.FromJson(x, this.client)));
        }


        public Task<IEnumerable<IPlaylist>> FindPlaylists(string searchTerm, CancellationToken cancellationToken, uint start = 0, uint count = 25)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                throw new ArgumentException("No search term given.", nameof(searchTerm));
            }

            var escapedSearchTerm = Uri.EscapeUriString(searchTerm);

            return this.client.Get($"search/playlist?q={escapedSearchTerm}&{START_PARAM}={start}&{COUNT_PARAM}={count}",
                                   cancellationToken,
                                   json => FragmentOf<IPlaylist>.FromJson(json, x => Api.Playlist.FromJson(x, this.client)));
        }


        public Task<IEnumerable<ITrack>> FindTracks(string searchTerm, CancellationToken cancellationToken, uint start = 0, uint count = 25)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                throw new ArgumentException("No search term given.", nameof(searchTerm));
            }

            var escapedSearchTerm = Uri.EscapeUriString(searchTerm);

            return this.client.Get($"search/track?q={escapedSearchTerm}&{START_PARAM}={start}&{COUNT_PARAM}={count}",
                                   cancellationToken,
                                   json => FragmentOf<ITrack>.FromJson(json, x => Api.Track.FromJson(x, this.client)));
        }


        public Task<IEnumerable<IRadio>> FindRadio(string searchTerm, CancellationToken cancellationToken, uint start = 0, uint count = 25)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                throw new ArgumentException("No search term given.", nameof(searchTerm));
            }

            var escapedSearchTerm = Uri.EscapeUriString(searchTerm);

            return this.client.Get($"search/radio?q={escapedSearchTerm}&{START_PARAM}={start}&{COUNT_PARAM}={count}",
                                   cancellationToken,
                                   json => FragmentOf<IRadio>.FromJson(json, x => Api.Radio.FromJson(x, this.client)));
        }

        public Task<IEnumerable<IUserProfile>> FindUsers(string searchTerm, CancellationToken cancellationToken, uint start = 0, uint count = 25)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                throw new ArgumentException("No search term given.", nameof(searchTerm));
            }

            var escapedSearchTerm = Uri.EscapeUriString(searchTerm);

            return this.client.Get($"search/user?q={escapedSearchTerm}&{START_PARAM}={start}&{COUNT_PARAM}={count}",
                                   cancellationToken,
                                   json => FragmentOf<IUserProfile>.FromJson(json, x => Api.UserProfile.FromJson(x, this.client)));
        }

    }
}
