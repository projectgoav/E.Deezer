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
        /// <returns>A book of tracklist</returns>
        Task<IBook<ITrack>> GetTracklist();

        /// <summary>
        /// Gets the artist's top track
        /// </summary>
        /// <returns>A book of artist's top tracks</returns>
        Task<IBook<ITrack>> GetTopTracks();

        /// <summary>
        /// Gets a list of artist's albums
        /// </summary>
        /// <returns>A book of artist's album collection</returns>
        Task<IBook<IAlbum>> GetAlbums();

        /// <summary>
        /// Geta  list of related artists
        /// </summary>
        /// <returns>A book of related artists</returns>
        Task<IBook<IArtist>> GetRelated();

        /// <summary>
        /// Gets a list of playlists containing this artist
        /// </summary>
        /// <returns>A book of playlists featuring artist</returns>
        Task<IBook<IPlaylist>> GetPlaylistsContaining();
    }

    public class Artist : IArtist, IDeserializable<DeezerClientV2>
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Picture { get; set; }
        public string Tracklist { get; set; }


        public DeezerClientV2 Client { get; set; }
        public void Deserialize(DeezerClientV2 aClient) { Client = aClient; }


        public Task<IBook<ITrack>> GetTopTracks()
        {
            throw new NotImplementedException();
            //return Client.GetArtistTopTracks(Id);
        }

        public Task<IBook<IAlbum>> GetAlbums()
        {
            throw new NotImplementedException();
            //return Client.GetArtistAlbums(Id);
        }

        public Task<IBook<IArtist>> GetRelated()
        {
            throw new NotImplementedException();
            //return Client.GetArtistRelated(Id);
        }

        public Task<IBook<ITrack>> GetTracklist()
        {
            throw new NotImplementedException();
            //return Client.GetArtistTracklist(Id);
        }

        public Task<IBook<IPlaylist>> GetPlaylistsContaining()
        {
            throw new NotImplementedException();
            //return Client.GetArtistPlaylists(Id);
        }


        public override string ToString()
        {
            return string.Format("E.Deezer: Artist({0})", Name);
        }
    }
}
