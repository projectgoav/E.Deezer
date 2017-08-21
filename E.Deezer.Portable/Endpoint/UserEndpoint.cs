﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using E.Deezer.Api;

namespace E.Deezer.Endpoint
{
    public interface IUserEndpoint
    {
        IUser Current { get; }

        Task<IEnumerable<ITrack>> GetHistory();
        Task<IEnumerable<ITrack>> GetHistory(uint aCount);
        Task<IEnumerable<ITrack>> GetHistory(uint aStart, uint aCount);

        Task<IEnumerable<ITrack>> GetFlow();
        Task<IEnumerable<ITrack>> GetFlow(uint aCount);
        Task<IEnumerable<ITrack>> GetFlow(uint aStart, uint aCount);

        Task<IEnumerable<ITrack>> GetPersonalTracks();
        Task<IEnumerable<ITrack>> GetPersonalTracks(uint aCount);
        Task<IEnumerable<ITrack>> GetPersonalTracks(uint aStart, uint aCount);

        Task<IEnumerable<IPlaylist>> GetPlaylists();
        Task<IEnumerable<IPlaylist>> GetPlaylists(uint aCount);
        Task<IEnumerable<IPlaylist>> GetPlaylists(uint aStart, uint aCount);

        //Favourite Wrappers
        Task<IEnumerable<IAlbum>> GetFavouriteAlbums();
        Task<IEnumerable<IAlbum>> GetFavouriteAlbums(uint aCount);
        Task<IEnumerable<IAlbum>> GetFavouriteAlbums(uint aStart, uint aCount);

        Task<IEnumerable<IArtist>> GetFavouriteArtists();
        Task<IEnumerable<IArtist>> GetFavouriteArtists(uint aCount);
        Task<IEnumerable<IArtist>> GetFavouriteArtists(uint aStart, uint aCount);

        Task<IEnumerable<ITrack>> GetFavouriteTracks();
        Task<IEnumerable<ITrack>> GetFavouriteTracks(uint aCount);
        Task<IEnumerable<ITrack>> GetFavouriteTracks(uint aStart, uint aCount);

        
        //Favourites Managers
        Task<bool> AddAlbumToFavourite(ulong albumId);
        Task<bool> AddAlbumToFavourite(IAlbum aAlbum);

        Task<bool> RemoveAlbumFromFavourite(ulong albumId);
        Task<bool> RemoveAlbumFromFavourite(IAlbum aAlbum);

        Task<bool> AddArtistToFavourite(ulong artistId);
        Task<bool> AddArtistToFavourite(IArtist artistId);

        Task<bool> RemoveArtistFromFavourite(ulong artistId);
        Task<bool> RemoveArtistFromFavourite(IArtist artistId);

        Task<bool> AddTrackToFavourite(ulong trackId);
        Task<bool> RemoveTrackFromFavourite(ulong trackId);

        Task<bool> AddPlaylistToFavourite(ulong playlistd);
        Task<bool> AddPlaylistToFavourite(IPlaylist playlistd);

        Task<bool> RemovePlaylistFromFavourite(ulong playlistId);
        Task<bool> RemovePlaylistFromFavourite(IPlaylist playlistId);

        Task<bool> AddRadioToFavourite(ulong radioId);
        Task<bool> AddRadioToFavourite(IRadio aRadio);

        Task<bool> RemoveRadioFromFavourite(ulong radioId);
        Task<bool> RemoveRadioFromFavourite(IRadio aRadio);

        //Recommendation Wrappers
        Task<IEnumerable<IAlbum>> GetRecommendedAlbums();
        Task<IEnumerable<IAlbum>> GetRecommendedAlbums(uint aCount);
        Task<IEnumerable<IAlbum>> GetRecommendedAlbums(uint aStart, uint aCount);

        Task<IEnumerable<IArtist>> GetRecommendedArtists();
        Task<IEnumerable<IArtist>> GetRecommendedArtists(uint aCount);
        Task<IEnumerable<IArtist>> GetRecommendedArtists(uint aStart, uint aCount);

        Task<IEnumerable<ITrack>> GetRecommendedTracks();
        Task<IEnumerable<ITrack>> GetRecommendedTracks(uint aCount);
        Task<IEnumerable<ITrack>> GetRecommendedTracks(uint aStart, uint aCount);

        Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists();
        Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists(uint aCount);
        Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists(uint aStart, uint aCount);
    }

    internal class UserEndpoint : IUserEndpoint
    {
        private DeezerClient iClient;
        public UserEndpoint(DeezerClient aClient) { iClient = aClient; }

        public IUser Current 
        { 
            get 
            {
                if (!iClient.IsAuthenticated) { throw new NotLoggedInException(); }
                return iClient.User;
            }
        }

        public Task<IEnumerable<ITrack>> GetHistory() { return GetHistory(0, iClient.ResultSize); }
        public Task<IEnumerable<ITrack>> GetHistory(uint aCount) { return GetHistory(0, aCount); }
        public Task<IEnumerable<ITrack>> GetHistory(uint aStart, uint aCount)
        {
            if (!iClient.IsAuthenticated) { throw new NotLoggedInException(); }
            return Current.GetHistory(aStart, aCount);
        }

        public Task<IEnumerable<ITrack>> GetFlow() { return GetFlow(0, iClient.ResultSize); }
        public Task<IEnumerable<ITrack>> GetFlow(uint aCount) { return GetFlow(0, aCount); }
        public Task<IEnumerable<ITrack>> GetFlow(uint aStart, uint aCount)
        {
            if (!iClient.IsAuthenticated) { throw new NotLoggedInException(); }
            return Current.GetFlow(aStart, aCount);
        }

        public Task<IEnumerable<ITrack>> GetPersonalTracks() { return GetPersonalTracks(0, iClient.ResultSize); }
        public Task<IEnumerable<ITrack>> GetPersonalTracks(uint aCount) { return GetPersonalTracks(0, aCount); }
        public Task<IEnumerable<ITrack>> GetPersonalTracks(uint aStart, uint aCount)
        {
            if (!iClient.IsAuthenticated) { throw new NotLoggedInException(); }
            return Current.GetPersonalTracks(aStart, aCount);
        }

        public Task<IEnumerable<IPlaylist>> GetPlaylists() { return GetPlaylists(0, iClient.ResultSize); }
        public Task<IEnumerable<IPlaylist>> GetPlaylists(uint aCount) { return GetPlaylists(0, aCount); }
        public Task<IEnumerable<IPlaylist>> GetPlaylists(uint aStart, uint aCount)
        {
            if (!iClient.IsAuthenticated) { throw new NotLoggedInException(); }
            return Current.GetPlaylists(aStart, aCount);
        }


        //Favourites
        public Task<IEnumerable<IAlbum>> GetFavouriteAlbums() { return GetFavouriteAlbums(0, iClient.ResultSize); }
        public Task<IEnumerable<IAlbum>> GetFavouriteAlbums(uint aCount) { return GetFavouriteAlbums(0, aCount); }
        public Task<IEnumerable<IAlbum>> GetFavouriteAlbums(uint aStart, uint aCount) { return Current.GetFavouriteAlbums(aStart, aCount); }

        public Task<IEnumerable<IArtist>> GetFavouriteArtists() { return GetFavouriteArtists(0, iClient.ResultSize); }
        public Task<IEnumerable<IArtist>> GetFavouriteArtists(uint aCount) { return GetFavouriteArtists(0, aCount); }
        public Task<IEnumerable<IArtist>> GetFavouriteArtists(uint aStart, uint aCount) { return Current.GetFavouriteArtists(aStart, aCount); }

        public Task<IEnumerable<ITrack>> GetFavouriteTracks() { return GetFavouriteTracks(0, iClient.ResultSize); }
        public Task<IEnumerable<ITrack>> GetFavouriteTracks(uint aCount) { return GetFavouriteTracks(0, aCount); }
        public Task<IEnumerable<ITrack>> GetFavouriteTracks(uint aStart, uint aCount) { return Current.GetFavouriteTracks(aStart, aCount); }

        //Favourites Managers
        public Task<bool> AddAlbumToFavourite(IAlbum aAlbum) => AddAlbumToFavourite(aAlbum.Id);
        public Task<bool> AddAlbumToFavourite(ulong artistId) => Current.AddArtistToFavourite(artistId);

        public Task<bool> RemoveAlbumFromFavourite(IAlbum aAlbum) => RemoveAlbumFromFavourite(aAlbum.Id);
        public Task<bool> RemoveAlbumFromFavourite(ulong AlbumId) => Current.RemoveAlbumFromFavourite(AlbumId);


        public Task<bool> AddArtistToFavourite(IArtist aArtist) => AddArtistToFavourite(aArtist.Id);
        public Task<bool> AddArtistToFavourite(ulong artistId) => Current.AddArtistToFavourite(artistId);

        public Task<bool> RemoveArtistFromFavourite(IArtist aArtist) => RemoveArtistFromFavourite(aArtist.Id);
        public Task<bool> RemoveArtistFromFavourite(ulong artistId) => Current.RemoveArtistFromFavourite(artistId);


        public Task<bool> AddPlaylistToFavourite(IPlaylist aPlaylist) => AddPlaylistToFavourite(aPlaylist.Id);
        public Task<bool> AddPlaylistToFavourite(ulong PlaylistId) => Current.AddPlaylistToFavourite(PlaylistId);

        public Task<bool> RemovePlaylistFromFavourite(IPlaylist aPlaylist) => RemovePlaylistFromFavourite(aPlaylist.Id);
        public Task<bool> RemovePlaylistFromFavourite(ulong PlaylistId) => Current.RemovePlaylistFromFavourite(PlaylistId);


        public Task<bool> AddTrackToFavourite(ITrack aTrack) => AddTrackToFavourite(aTrack.Id);
        public Task<bool> AddTrackToFavourite(ulong TrackId) => Current.AddTrackToFavourite(TrackId);

        public Task<bool> RemoveTrackFromFavourite(ITrack aTrack) => RemoveTrackFromFavourite(aTrack.Id);
        public Task<bool> RemoveTrackFromFavourite(ulong TrackId) => Current.RemoveTrackFromFavourite(TrackId);


        public Task<bool> AddRadioToFavourite(IRadio aRadio) => AddRadioToFavourite(aRadio.Id);
        public Task<bool> AddRadioToFavourite(ulong RadioId) => Current.AddRadioToFavourite(RadioId);

        public Task<bool> RemoveRadioFromFavourite(IRadio aRadio) => RemoveRadioFromFavourite(aRadio.Id);
        public Task<bool> RemoveRadioFromFavourite(ulong RadioId) => Current.RemoveRadioFromFavourite(RadioId);


        //Recomends
        public Task<IEnumerable<IAlbum>> GetRecommendedAlbums() { return GetRecommendedAlbums(0, iClient.ResultSize); }
        public Task<IEnumerable<IAlbum>> GetRecommendedAlbums(uint aCount) { return GetRecommendedAlbums(0, aCount); }
        public Task<IEnumerable<IAlbum>> GetRecommendedAlbums(uint aStart, uint aCount) { return Current.GetRecommendedAlbums(aStart, aCount); }

        public Task<IEnumerable<IArtist>> GetRecommendedArtists() { return GetRecommendedArtists(0, iClient.ResultSize); }
        public Task<IEnumerable<IArtist>> GetRecommendedArtists(uint aCount) { return GetRecommendedArtists(0, aCount); }
        public Task<IEnumerable<IArtist>> GetRecommendedArtists(uint aStart, uint aCount) { return Current.GetRecommendedArtists(aStart, aCount); }

        public Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists() { return GetRecommendedPlaylists(0, iClient.ResultSize); }
        public Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists(uint aCount) { return GetRecommendedPlaylists(0, aCount); }
        public Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists(uint aStart, uint aCount) { return Current.GetRecommendedPlaylists(aStart, aCount); }

        public Task<IEnumerable<ITrack>> GetRecommendedTracks() { return GetRecommendedTracks(0, iClient.ResultSize); }
        public Task<IEnumerable<ITrack>> GetRecommendedTracks(uint aCount) { return GetRecommendedTracks(0, aCount); }
        public Task<IEnumerable<ITrack>> GetRecommendedTracks(uint aStart, uint aCount) { return Current.GetRecommendedTracks(aStart, aCount); }

                
    }
}
