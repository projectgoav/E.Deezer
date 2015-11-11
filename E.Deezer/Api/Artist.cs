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
        Task<IPage<ITrack>> GetTracklist();

        /// <summary>
        /// Gets the artist's top track
        /// </summary>
        /// <returns>First page of artist's top tracks</returns>
        Task<IPage<ITrack>> GetTopTracks();

        /// <summary>
        /// Gets a list of artist's albums
        /// </summary>
        /// <returns>First page of artist's album collection</returns>
        Task<IPage<IAlbum>> GetAlbums();

        /// <summary>
        /// Geta  list of related artists
        /// </summary>
        /// <returns>First page of related artists</returns>
        Task<IPage<IArtist>> GetRelated();

        /// <summary>
        /// Gets a list of playlists containing this artist
        /// </summary>
        /// <returns>First page of playlists featuring artist</returns>
        Task<IPage<IPlaylist>> GetPlaylistsContaining();
    }

    public class Artist : IArtist, IDeserializable<DeezerClient>
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Picture { get; set; }
        public string Tracklist { get; set; }


        public DeezerClient Client { get; set; }
        public void Deserialize(DeezerClient aClient) { Client = aClient; }


        public Task<IPage<ITrack>> GetTopTracks()
        {
            return Client.GetArtistTopTracks(Id);
        }

        public Task<IPage<IAlbum>> GetAlbums()
        {
            return Client.GetArtistAlbums(Id);
        }

        public Task<IPage<IArtist>> GetRelated()
        {
            return Client.GetArtistRelated(Id);
        }

        public Task<IPage<ITrack>> GetTracklist()
        {
            return Client.GetArtistTracklist(Id);
        }

        public Task<IPage<IPlaylist>> GetPlaylistsContaining()
        {
            return Client.GetArtistPlaylists(Id);
        }


        public override string ToString()
        {
            return string.Format("E.Deezer: Artist({0})", Name);
        }
    }
}
