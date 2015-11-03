using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E.Deezer.Api
{
    /// <summary>
    /// Deezer user object
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// Deezer library ID number
        /// </summary>
        uint Id { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Deezer.com link to user
        /// </summary>
        string Link { get; set; }

				/// <summary>
				/// Country
				/// </summary>
				string Country { get; set; }


        //Methods


        /// <summary>
        /// Gets the user's favourite albums
        /// </summary>
        /// <returns>First page of user's favourite albums</returns>
        IPagedResponse<IAlbum> GetFavouriteAlbums();

        /// <summary>
        /// Gets the user's favourite artists
        /// </summary>
        /// <returns>First page of user's favourite artists</returns>
        IPagedResponse<IArtist> GetFavouriteArtists();

        /// <summary>
        /// Gets the user's favourite tracks
        /// </summary>
        /// <returns>First page of user's favourite tracks</returns>
        IPagedResponse<ITrack> GetFavouriteTracks();

        /// <summary>
        /// Gets the user's "flow"
        /// </summary>
        /// <returns>First page of user's "flow"</returns>
        IPagedResponse<ITrack> GetUserFlow();

        /// <summary>
        /// Gets the user;s listening history
        /// </summary>
        /// <returns>First page of user's litening history</returns>
        IPagedResponse<ITrack> GetListenHistory();
        
        /// <summary>
        /// Gets the user's favourite playlists
        /// </summary>
        /// <returns>First page of user's listening history</returns>
        IPagedResponse<IPlaylist> GetFavouritePlaylists();


        /// <summary>
        /// Gets recommended albums for the user
        /// </summary>
        /// <returns>First page of recommended albums</returns>
        IPagedResponse<IAlbum> GetRecommendedAlbums();

        /// <summary>
        /// Gets recommended artists for the user
        /// </summary>
        /// <returns>First page of recommended artists</returns>
        IPagedResponse<IArtist> GetRecommendedArtists();

        /// <summary>
        /// Gets recommended tracks for the user
        /// </summary>
        /// <returns>First page of recommended tracks</returns>
        IPagedResponse<ITrack> GetRecommendedTracks();

        /// <summary>
        /// Gets recommended playlists for the user
        /// </summary>
        /// <returns>First page of recommded playlists</returns>
        IPagedResponse<IPlaylist> GetRecommendedPlaylists();

    }

    public class User : IUser
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
				public string Country { get; set; }

        public string error { get; set; }


        public IPagedResponse<IAlbum> GetFavouriteAlbums()
        {
            return null;
        }

        public IPagedResponse<IArtist> GetFavouriteArtists()
        {
            return null;
        }

        public IPagedResponse<ITrack> GetFavouriteTracks()
        {
            return null;
        }

        public IPagedResponse<ITrack> GetUserFlow()
        {
            return null;
        }

        public IPagedResponse<ITrack> GetListenHistory()
        {
            return null;
        }

        public IPagedResponse<IPlaylist> GetFavouritePlaylists()
        {
            return null;
        }




        public IPagedResponse<IAlbum> GetRecommendedAlbums()
        {
            return null;
        }

        public IPagedResponse<IArtist> GetRecommendedArtists()
        {
            return null;
        }

        public IPagedResponse<ITrack> GetRecommendedTracks()
        {
            return null;
        }

        public IPagedResponse<IPlaylist> GetRecommendedPlaylists()
        {
            return null;
        }



        public override string ToString()
        {
            return Name;
        }
    }
}
