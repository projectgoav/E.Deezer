using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E.Deezer.Api
{
    public interface ITrack
    {
        uint Id { get; set;  }
        string Title { get; set;  }
        string Link { get; set;  }
        uint Duration { get; set;  }
        DateTime ReleaseDate { get; set; }
        bool Explicit { get; set;  }
        string Artwork { get; set;  }
        string ArtistName { get; }
        string AlbumName { get; }
    }


    internal class Track : ITrack
    {
        public uint Id { get; set; }
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


        public override string ToString()
        {
            return string.Format("{0} - {1} ({2})", Title, ArtistName, AlbumName);
        }
    }
}
