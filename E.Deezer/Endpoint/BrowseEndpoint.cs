using E.Deezer.Api;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E.Deezer.Endpoint
{
    public interface IBrowseEndpoint
    {
        IGenreEndpoint Genre { get; }
        IChartsEndpoint Charts { get; }

        IUserEndpoint CurrentUser { get; }

        Task<IAlbum> GetAlbumById(uint albumId);
        Task<IArtist> GetArtistById(uint artistId);
        Task<IPlaylist> GetPlaylistById(uint playlistId);
        Task<IRadio> GetRadioById(uint radioId);
        Task<ITrack> GetTrackById(uint trackId);
        Task<IUserProfile> GetUserById(uint userId);
    }

    internal class BrowseEndpoint : IBrowseEndpoint
    {
        private readonly IUserEndpoint _userEndpoint;

        private readonly DeezerClient _client;

        public BrowseEndpoint(DeezerClient client)
        {
            _client = client;

            Genre = new GenreEndpoint(_client);
            Charts = new ChartsEndpoint(_client);
            _userEndpoint = new UserEndpoint(client);
        }

        public IGenreEndpoint Genre { get; }

        public IChartsEndpoint Charts { get; }

        public IUserEndpoint CurrentUser
        {
            get
            {
                if (!_client.IsAuthenticated)
                {
                    throw new NotLoggedInException();
                }

                return _userEndpoint;
            }
        }

        public async Task<IAlbum> GetAlbumById(uint albumId)
        {
            var p = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", albumId),
            };

            var response = await _client.GetPlain<Album>("album/{id}", p)
                                        .ConfigureAwait(false);

            response.Deserialize(_client);
            return response;
        }

        public async Task<IArtist> GetArtistById(uint artistId)
        {
            var p = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", artistId),
            };

            var response = await _client.GetPlain<Artist>("artist/{id}", p)
                                        .ConfigureAwait(false);

            response.Deserialize(_client);
            return response;
        }

        public async Task<IPlaylist> GetPlaylistById(uint playlistId)
        {
            var p = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", playlistId),
            };

            var response = await _client.GetPlain<Playlist>("playlist/{id}", p)
                                        .ConfigureAwait(false);

            response.Deserialize(_client);
            return response;
        }

        public async Task<ITrack> GetTrackById(uint TrackId)
        {
            var p = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", TrackId),
            };

            var response = await _client.GetPlain<Track>("track/{id}", p)
                                        .ConfigureAwait(false);

            response.Deserialize(_client);
            return response;
        }

        public async Task<IRadio> GetRadioById(uint RadioId)
        {
            var p = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", RadioId),
            };

            var response = await _client.GetPlain<Radio>("radio/{id}", p)
                                        .ConfigureAwait(false);

            response.Deserialize(_client);
            return response;
        }

        public async Task<IUserProfile> GetUserById(uint UserId)
        {
            var p = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", UserId),
            };

            var response = await _client.GetPlain<UserProfile>("user/{id}", p)
                                        .ConfigureAwait(false);

            response.Deserialize(_client);
            return response;
        }
    }
}
