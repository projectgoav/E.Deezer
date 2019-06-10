﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using E.Deezer.Api;

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
        Task<IUser> GetUserById(uint userId);
    }

    internal class BrowseEndpoint : IBrowseEndpoint
    {
        private readonly IGenreEndpoint iGenre;
        private readonly IChartsEndpoint iCharts;
        private readonly IUserEndpoint iUserEndpoint;

        private readonly DeezerClient iClient;

        public BrowseEndpoint(DeezerClient aClient)
        {
            iClient = aClient;

            iGenre = new GenreEndpoint(iClient);
            iCharts = new ChartsEndpoint(iClient);
            iUserEndpoint = new UserEndpoint(aClient);
        }


        public IGenreEndpoint Genre => iGenre;

        public IChartsEndpoint Charts => iCharts;

        public IUserEndpoint CurrentUser
        {
            get
            {
                if(!iClient.IsAuthenticated)
                {
                    throw new NotLoggedInException();
                }

                return iUserEndpoint;
            }
        }


        public async Task<IAlbum> GetAlbumById(uint albumId)
        {
            var p = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", albumId),
            };

            // Will throw if Deezer error
            var response = await iClient.GetDeezerObject<AlbumObjectResponse>("album/{id}", p)
                                        .ConfigureAwait(false);

            return response.Object;
        }

        public async Task<IArtist> GetArtistById(uint artistId)
        {
            var p = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", artistId),
            };

            // Will throw if Deezer error
            var response = await iClient.GetDeezerObject<ArtistObjectResponse>("artist/{id}", p)
                                        .ConfigureAwait(false);

            return response.Object;
        }

        public async Task<IPlaylist> GetPlaylistById(uint playlistId)
        {
            var p = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", playlistId),
            };

            // Will throw if Deezer error
            var response = await iClient.GetDeezerObject<PlaylistObjectResponse>("playlist/{id}", p)
                                        .ConfigureAwait(false);

            return response.Object;
        }

        public async Task<ITrack> GetTrackById(uint TrackId)
        {
            var p = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", TrackId),
            };

            // Will throw if Deezer error
            var response = await iClient.GetDeezerObject<TrackObjectResponse>("Track/{id}", p)
                                        .ConfigureAwait(false);

            return response.Object;
        }

        public async Task<IRadio> GetRadioById(uint RadioId)
        {
            var p = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", RadioId),
            };

            // Will throw if Deezer error
            var response = await iClient.GetDeezerObject<RadioObjectResponse>("Radio/{id}", p)
                                        .ConfigureAwait(false);

            return response.Object;
        }

        public async Task<IUser> GetUserById(uint UserId)
        {
            var p = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", UserId),
            };

            // Will throw if Deezer error
            var response = await iClient.GetDeezerObject<UserObjectResponse>("User/{id}", p)
                                        .ConfigureAwait(false);

            return response.Object;
        }
    }
}
