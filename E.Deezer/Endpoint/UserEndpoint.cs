using System;
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

        Task<bool> AddAlbumToFavorite(ulong albumId);
        Task<bool> RemoveAlbumFromFavorite(ulong albumId);

        Task<IEnumerable<IArtist>> GetFavouriteArtists();
        Task<IEnumerable<IArtist>> GetFavouriteArtists(uint aCount);
        Task<IEnumerable<IArtist>> GetFavouriteArtists(uint aStart, uint aCount);

        Task<bool> AddArtistToFavorite(ulong artistId);
        Task<bool> RemoveArtistFromFavorite(ulong artistId);

        Task<IEnumerable<ITrack>> GetFavouriteTracks();
        Task<IEnumerable<ITrack>> GetFavouriteTracks(uint aCount);
        Task<IEnumerable<ITrack>> GetFavouriteTracks(uint aStart, uint aCount);

        Task<bool> AddTrackToFavorite(ulong trackId);
        Task<bool> RemoveTrackFromFavorite(ulong trackId);

        Task<bool> AddPlaylistToFavorite(ulong playlistd);
        Task<bool> RemovePlaylistFromFavorite(ulong playlistId);

        Task<bool> AddRadioToFavorite(ulong radioId);
        Task<bool> RemoveRadioFromFavorite(ulong radioId);

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

        public Task<bool> AddAlbumToFavorite(ulong albumId) => Current.AddAlbumToFavorite(albumId);
        public Task<bool> RemoveAlbumFromFavorite(ulong albumId) => Current.RemoveAlbumFromFavorite(albumId);

        public Task<IEnumerable<IArtist>> GetFavouriteArtists() { return GetFavouriteArtists(0, iClient.ResultSize); }
        public Task<IEnumerable<IArtist>> GetFavouriteArtists(uint aCount) { return GetFavouriteArtists(0, aCount); }
        public Task<IEnumerable<IArtist>> GetFavouriteArtists(uint aStart, uint aCount) { return Current.GetFavouriteArtists(aStart, aCount); }

        public Task<bool> AddArtistToFavorite(ulong artistId) => Current.AddArtistToFavorite(artistId);
        public Task<bool> RemoveArtistFromFavorite(ulong artistId) => Current.RemoveArtistFromFavorite(artistId);

        public Task<IEnumerable<ITrack>> GetFavouriteTracks() { return GetFavouriteTracks(0, iClient.ResultSize); }
        public Task<IEnumerable<ITrack>> GetFavouriteTracks(uint aCount) { return GetFavouriteTracks(0, aCount); }
        public Task<IEnumerable<ITrack>> GetFavouriteTracks(uint aStart, uint aCount) { return Current.GetFavouriteTracks(aStart, aCount); }

        public Task<bool> AddTrackToFavorite(ulong trackId) => Current.AddTrackToFavorite(trackId);
        public Task<bool> RemoveTrackFromFavorite(ulong trackId) => Current.RemoveTrackFromFavorite(trackId);

        public Task<bool> AddRadioToFavorite(ulong radioId) => Current.AddRadioToFavorite(radioId);
        public Task<bool> RemoveRadioFromFavorite(ulong radioId) => Current.RemoveRadioFromFavorite(radioId);

        public Task<bool> AddPlaylistToFavorite(ulong playlistd) => Current.AddPlaylistToFavorite(playlistd);
        public Task<bool> RemovePlaylistFromFavorite(ulong playlistId) => Current.RemovePlaylistFromFavorite(playlistId);

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
