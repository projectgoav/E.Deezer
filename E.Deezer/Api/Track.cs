using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using RestSharp.Deserializers;

namespace E.Deezer.Api
{
    /// <summary>
    /// Deezer tack object
    /// </summary>
    public interface ITrack
    {
        /// <summary>
        /// Deezer libray ID number
        /// </summary>
        int Id { get; set;  }

        /// <summary>
        /// Track title
        /// </summary>
        string Title { get; set;  }

        /// <summary>
        /// Deezer.come link to track
        /// </summary>
        string Link { get; set;  }

        /// <summary>
        /// Length of track (in seconds)
        /// </summary>
        uint Duration { get; set;  }

        /// <summary>
        /// Track release date
        /// </summary>
        DateTime ReleaseDate { get; set; }

        /// <summary>
        /// Track Explicit rating
        /// </summary>
        bool Explicit { get; set;  }

        /// <summary>
        /// Link to track artwork
        /// </summary>
        string Artwork { get; set;  }

        /// <summary>
        /// Track artist name
        /// </summary>
        string ArtistName { get; }

        /// <summary>
        /// Track album name
        /// </summary>
        string AlbumName { get; }

        //Methods

        /// <summary>
        /// Gets track artist
        /// </summary>
        /// <returns>Track's artist</returns>
        Task<IArtist> GetArtist();

        /// <summary>
        /// Gets album that track belongs to
        /// </summary>
        /// <returns>Containing album</returns>
        Task<IAlbum> GetAlbum();
    }


    internal class Track : ITrack
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


        private DeezerClient Client { get; set; }
        internal void Deserialize(DeezerClient aClient) 
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
