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

            // Will throw if Deezer error
            var response = await _client.GetDeezerObject<AlbumObjectResponse>("album/{id}", p)
                                        .ConfigureAwait(false);

            response.Object.Deserialize(_client);

            return response.Object;
        }

        public async Task<IArtist> GetArtistById(uint artistId)
        {
            var p = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", artistId),
            };

            // Will throw if Deezer error
            var response = await _client.GetDeezerObject<ArtistObjectResponse>("artist/{id}", p)
                                        .ConfigureAwait(false);

            response.Object.Deserialize(_client);

            return response.Object;
        }

        public async Task<IPlaylist> GetPlaylistById(uint playlistId)
        {
            var p = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", playlistId),
            };

            // Will throw if Deezer error
            var response = await _client.GetDeezerObject<PlaylistObjectResponse>("playlist/{id}", p)
                                        .ConfigureAwait(false);

            response.Object.Deserialize(_client);

            return response.Object;
        }

        public async Task<ITrack> GetTrackById(uint TrackId)
        {
            var p = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", TrackId),
            };

            // Will throw if Deezer error
            var response = await _client.GetDeezerObject<TrackObjectResponse>("track/{id}", p)
                                        .ConfigureAwait(false);

            response.Object.Deserialize(_client);

            return response.Object;
        }

        public async Task<IRadio> GetRadioById(uint RadioId)
        {
            var p = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", RadioId),
            };

            // Will throw if Deezer error
            var response = await _client.GetDeezerObject<RadioObjectResponse>("radio/{id}", p)
                                        .ConfigureAwait(false);

            response.Object.Deserialize(_client);

            return response.Object;
        }

        public async Task<IUserProfile> GetUserById(uint UserId)
        {
            var p = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", UserId),
            };

            // Will throw if Deezer error
            var response = await _client.GetDeezerObject<UserProfileObjectResponse>("user/{id}", p)
                                        .ConfigureAwait(false);

            response.Object.Deserialize(_client);

            return response.Object;
        }
    }
}
