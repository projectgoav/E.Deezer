using E.Deezer.Api;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E.Deezer.Endpoint
{
    public interface IUserEndpoint
    {
        IUser User { get; }

        Task<IEnumerable<ITrack>> GetHistory(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<ITrack>> GetFlow(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<ITrack>> GetPersonalTracks(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<IPlaylist>> GetPlaylists(uint aStart = 0, uint aCount = 100);

        //Favourite Wrappers
        Task<IEnumerable<IAlbum>> GetFavouriteAlbums(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<IArtist>> GetFavouriteArtists(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<ITrack>> GetFavouriteTracks(uint aStart = 0, uint aCount = 100);

        
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
        Task<bool> AddTrackToFavourite(ITrack aTrack);

        Task<bool> RemoveTrackFromFavourite(ulong trackId);
        Task<bool> RemoveTrackFromFavourite(ITrack aTrack);

        Task<bool> AddPlaylistToFavourite(ulong playlistd);
        Task<bool> AddPlaylistToFavourite(IPlaylist playlistd);

        Task<bool> RemovePlaylistFromFavourite(ulong playlistId);
        Task<bool> RemovePlaylistFromFavourite(IPlaylist playlistId);

        Task<bool> AddRadioToFavourite(ulong radioId);
        Task<bool> AddRadioToFavourite(IRadio aRadio);

        Task<bool> RemoveRadioFromFavourite(ulong radioId);
        Task<bool> RemoveRadioFromFavourite(IRadio aRadio);

        //Recommendation Wrappers
        Task<IEnumerable<IAlbum>> GetRecommendedAlbums(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<IArtist>> GetRecommendedArtists(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<ITrack>> GetRecommendedTracks(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<IRadio>> GetRecommendedRadio(uint aStart = 0, uint aCount = 100);
    }

    internal class UserEndpoint : IUserEndpoint
    {
        private readonly DeezerClient iClient;

        public UserEndpoint(DeezerClient aClient)
        {
            iClient = aClient;
        }


        public IUser User => iClient.User;


        public Task<IEnumerable<ITrack>> GetHistory(uint aStart = 0, uint aCount = 0)
            => User.GetHistory(aStart, aCount);
        
        public Task<IEnumerable<ITrack>> GetFlow(uint aStart = 0, uint aCount = 100) 
            => User.GetFlow(aStart, aCount);
        
        public Task<IEnumerable<ITrack>> GetPersonalTracks(uint aStart = 0, uint aCount = 100) 
            => User.GetPersonalTracks(aStart, aCount);
        
        public Task<IEnumerable<IPlaylist>> GetPlaylists(uint aStart = 0, uint aCount = 100) 
            => User.GetPlaylists(aStart, aCount);
        


        //Favourites
        public Task<IEnumerable<IAlbum>> GetFavouriteAlbums(uint aStart = 0, uint aCount = 100) 
            => User.GetFavouriteAlbums(aStart, aCount); 

        public Task<IEnumerable<IArtist>> GetFavouriteArtists(uint aStart = 0, uint aCount = 100) 
            => User.GetFavouriteArtists(aStart, aCount); 

        public Task<IEnumerable<ITrack>> GetFavouriteTracks(uint aStart = 0, uint aCount = 100) 
            => User.GetFavouriteTracks(aStart, aCount); 

        //Favourites Managers
        public Task<bool> AddAlbumToFavourite(IAlbum aAlbum) 
            => AddAlbumToFavourite(aAlbum.Id);

        public Task<bool> AddAlbumToFavourite(ulong artistId) 
            => User.AddArtistToFavourite(artistId);

        public Task<bool> RemoveAlbumFromFavourite(IAlbum aAlbum) 
            => RemoveAlbumFromFavourite(aAlbum.Id);

        public Task<bool> RemoveAlbumFromFavourite(ulong AlbumId) 
            => User.RemoveAlbumFromFavourite(AlbumId);


        public Task<bool> AddArtistToFavourite(IArtist aArtist) 
            => AddArtistToFavourite(aArtist.Id);

        public Task<bool> AddArtistToFavourite(ulong artistId)
            => User.AddArtistToFavourite(artistId);

        public Task<bool> RemoveArtistFromFavourite(IArtist aArtist) 
            => RemoveArtistFromFavourite(aArtist.Id);

        public Task<bool> RemoveArtistFromFavourite(ulong artistId) 
            => User.RemoveArtistFromFavourite(artistId);


        public Task<bool> AddPlaylistToFavourite(IPlaylist aPlaylist) 
            => AddPlaylistToFavourite(aPlaylist.Id);

        public Task<bool> AddPlaylistToFavourite(ulong PlaylistId) 
            => User.AddPlaylistToFavourite(PlaylistId);

        public Task<bool> RemovePlaylistFromFavourite(IPlaylist aPlaylist) 
            => RemovePlaylistFromFavourite(aPlaylist.Id);

        public Task<bool> RemovePlaylistFromFavourite(ulong PlaylistId)
            => User.RemovePlaylistFromFavourite(PlaylistId);


        public Task<bool> AddTrackToFavourite(ITrack aTrack) 
            => AddTrackToFavourite(aTrack.Id);

        public Task<bool> AddTrackToFavourite(ulong TrackId)
            => User.AddTrackToFavourite(TrackId);

        public Task<bool> RemoveTrackFromFavourite(ITrack aTrack)
            => RemoveTrackFromFavourite(aTrack.Id);

        public Task<bool> RemoveTrackFromFavourite(ulong TrackId) 
            => User.RemoveTrackFromFavourite(TrackId);


        public Task<bool> AddRadioToFavourite(IRadio aRadio) 
            => AddRadioToFavourite(aRadio.Id);

        public Task<bool> AddRadioToFavourite(ulong RadioId) 
            => User.AddRadioToFavourite(RadioId);

        public Task<bool> RemoveRadioFromFavourite(IRadio aRadio)
            => RemoveRadioFromFavourite(aRadio.Id);

        public Task<bool> RemoveRadioFromFavourite(ulong RadioId) 
            => User.RemoveRadioFromFavourite(RadioId);


        //Recomends
        public Task<IEnumerable<IAlbum>> GetRecommendedAlbums(uint aStart = 0, uint aCount = 100) 
            => User.GetRecommendedAlbums(aStart, aCount); 

        public Task<IEnumerable<IArtist>> GetRecommendedArtists(uint aStart = 0, uint aCount = 100) 
            => User.GetRecommendedArtists(aStart, aCount); 

        public Task<IEnumerable<IPlaylist>> GetRecommendedPlaylists(uint aStart = 0, uint aCount = 100) 
            => User.GetRecommendedPlaylists(aStart, aCount); 

        public Task<IEnumerable<ITrack>> GetRecommendedTracks(uint aStart = 0, uint aCount = 100) 
            => User.GetRecommendedTracks(aStart, aCount);

        public Task<IEnumerable<IRadio>> GetRecommendedRadio(uint aStart = 0, uint aCount = 100)
            => User.GetRecommendedRadio(aStart, aCount);
       
    }
}
