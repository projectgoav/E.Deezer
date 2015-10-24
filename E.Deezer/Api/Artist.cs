using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// Gets the artist's top track
        /// </summary>
        /// <returns>First page of artist's top tracks</returns>
        IPagedResponse<ITrack> GetTopTracks();

        /// <summary>
        /// Gets a list of artist's albums
        /// </summary>
        /// <returns>First page of artist's album collection</returns>
        IPagedResponse<IAlbum> GetAlbums();

        /// <summary>
        /// Geta  list of related artists
        /// </summary>
        /// <returns>First page of related artists</returns>
        IPagedResponse<IArtist> GetRelated();
    }

    public class Artist : IArtist
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Picture { get; set; }
        public string Tracklist { get; set; }

        public IPagedResponse<ITrack> GetTopTracks()
        {
            throw new NotImplementedException();
        }

        public IPagedResponse<IAlbum> GetAlbums()
        {
            throw new NotImplementedException();
        }

        public IPagedResponse<IArtist> GetRelated()
        {
            throw new NotImplementedException();
        }


        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
