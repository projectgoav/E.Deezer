using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using RestSharp.Deserializers;

namespace E.Deezer.Api
{
    /// <summary>
    /// Represents an Track in the Deezer Library
    /// </summary>
    public interface ITrack
    {
        /// <summary>
        /// Gets the Deezer ID of this Track
        /// </summary>
        int Id { get; set;  }

        /// <summary>
        /// Gets the Title of Track
        /// </summary>
        string Title { get; set;  }

        /// <summary>
        /// Gets the www.deezer.com link to this Track
        /// </summary>
        string Link { get; set;  }
        
        /// <summary>
        /// Gets the duration of Track, in seconds
        /// </summary>
        uint Duration { get; set;  }

        /// <summary>
        /// Gets the Track's release date
        /// </summary>
        DateTime ReleaseDate { get; set; }

        /// <summary>
        /// Gets if the Track has explicit content
        /// </summary>
        bool Explicit { get; set; }

        /// <summary>
        /// Gets the link to the artwork for this Track
        /// </summary>
        string Artwork { get; set; }

        /// <summary>
        /// Gets the name of the Track's artist
        /// </summary>
        string ArtistName { get; }

        /// <summary>
        /// Gets the name of the album to which this Track belongs
        /// </summary>
        string AlbumName { get; }

        //Methods

        /// <summary>
        /// Get the Track's artist object
        /// </summary>
        /// <returns>Artist object who is the author of this Track</returns>
        Task<IArtist> GetArtist();

        /// <summary>
        /// Gets the Track's album object
        /// </summary>
        /// <returns>Album object to which the Track belongs</returns>
        Task<IAlbum> GetAlbum();
    }


    internal class Track : ITrack, IDeserializable<DeezerClientV2>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public uint Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Artwork { get; set; }
        public bool Explicit { get; set; }

        public string ArtistName
        {
            get
            {
                if (ArtistInternal == null) { return string.Empty; }
                else { return ArtistInternal.Name; }
            }
        }

        public string AlbumName
        {
            get
            {
                if (AlbumInternal == null) { return string.Empty; }
                else { return AlbumInternal.Title; }
            }
        }


        [DeserializeAs(Name = "artist")]
        public Artist ArtistInternal { get; set; }

        [DeserializeAs(Name = "album")]
        public Album AlbumInternal { get; set; }


        public DeezerClientV2 Client { get; set; }
        public void Deserialize(DeezerClientV2 aClient) 
        { 
            Client = aClient;
        }


        public Task<IArtist> GetArtist() { return Task.Factory.StartNew<IArtist>(() => ArtistInternal); }

        public Task<IAlbum> GetAlbum() { return Task.Factory.StartNew<IAlbum>(() => AlbumInternal); }


        public override string ToString()
        {
            return string.Format("E.Deezer: Track({0} - ({1}))", Title, ArtistName);
        }
    }
}
