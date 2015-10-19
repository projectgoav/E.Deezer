using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E.Deezer.Api
{
    public interface IUser
    {
        uint Id { get; set; }
        string Name { get; set; }
        string Link { get; set; }

        ISearchResult<IAlbum> GetFavouriteAlbums();
        ISearchResult<IArtist> GetFavouriteArtists();
        ISearchResult<ITrack> GetFavouriteTracks();
        ISearchResult<ITrack> GetUserFlow();
        ISearchResult<ITrack> GetListenHistory();
        ISearchResult<IPlaylist> GetFavouritePlaylists();

        ISearchResult<IAlbum> GetRecommendedAlbums();
        ISearchResult<IArtist> GetRecommendedArtists();
        ISearchResult<ITrack> GetRecommendedTracks();
        ISearchResult<IPlaylist> GetRecommendedPlaylists();

    }

    public class User : IUser
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }

        public string error { get; set; }


        public ISearchResult<IAlbum> GetFavouriteAlbums()
        {
            return null;
        }

        public ISearchResult<IArtist> GetFavouriteArtists()
        {
            return null;
        }

        public ISearchResult<ITrack> GetFavouriteTracks()
        {
            return null;
        }

        public ISearchResult<ITrack> GetUserFlow()
        {
            return null;
        }

        public ISearchResult<ITrack> GetListenHistory()
        {
            return null;
        }

        public ISearchResult<IPlaylist> GetFavouritePlaylists()
        {
            return null;
        }




        public ISearchResult<IAlbum> GetRecommendedAlbums()
        {
            return null;
        }

        public ISearchResult<IArtist> GetRecommendedArtists()
        {
            return null;
        }

        public ISearchResult<ITrack> GetRecommendedTracks()
        {
            return null;
        }

        public ISearchResult<IPlaylist> GetRecommendedPlaylists()
        {
            return null;
        }



        public override string ToString()
        {
            return Name;
        }
    }
}
