using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace E.Deezer.Api
{
    /// <summary>
    /// A Deezer artist object
    /// </summary>
    public interface IArtist
    {
        /// <summary>
        /// Deezer library ID number
        /// </summary>
        uint Id { get; set; }

        /// <summary>
        /// Artist's name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Deezer.com link to artist
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// Link to artist's image
        /// </summary>
        string Picture { get; set; }

        /// <summary>
        /// Link to artist tracklist
        /// </summary>
        string Tracklist { get; set; }

        //Methods

        /// <summary>
        /// Gets Album tracklist
        /// </summary>
        /// <returns>First page of tracklist</returns>
        Task<IPagedResponse<ITrack>> GetTracklist(int aResultSize);

        /// <summary>
        /// Gets the artist's top track
        /// </summary>
        /// <returns>First page of artist's top tracks</returns>
        Task<IPagedResponse<ITrack>> GetTopTracks(int aResultSize);

        /// <summary>
        /// Gets a list of artist's albums
        /// </summary>
        /// <returns>First page of artist's album collection</returns>
        Task<IPagedResponse<IAlbum>> GetAlbums(int aResultSize);

        /// <summary>
        /// Geta  list of related artists
        /// </summary>
        /// <returns>First page of related artists</returns>
        Task<IPagedResponse<IArtist>> GetRelated(int aResultSize);

        /// <summary>
        /// Gets a list of playlists containing this artist
        /// </summary>
        /// <returns>First page of playlists featuring artist</returns>
        Task<IPagedResponse<IPlaylist>> GetPlaylistsContaining(int aResultSize);
    }

    public class Artist : IArtist
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Picture { get; set; }
        public string Tracklist { get; set; }


        private DeezerClient Client { get; set; }
        internal void Deserialize(DeezerClient aClient)
        {
            Client = aClient;
        }


        public Task<IPagedResponse<ITrack>> GetTopTracks(int aResultSize)
        {
            return Client.GetArtistTopTracks(Id, aResultSize);
        }

        public Task<IPagedResponse<IAlbum>> GetAlbums(int aResultSize)
        {
            return Client.GetArtistAlbums(Id, aResultSize);
        }

        public Task<IPagedResponse<IArtist>> GetRelated(int aResultSize)
        {
            return Client.GetArtistRelated(Id, aResultSize);
        }

        public Task<IPagedResponse<ITrack>> GetTracklist(int aResultSize)
        {
            return Client.GetArtistTracklist(Id, aResultSize);
        }

        public Task<IPagedResponse<IPlaylist>> GetPlaylistsContaining(int aResultSize)
        {
            return Client.GetArtistPlaylists(Id, aResultSize);
        }


        public override string ToString()
        {
            return string.Format("E.Deezer: Artist({0})", Name);
        }
    }
}
