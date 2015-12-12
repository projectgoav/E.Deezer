using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using RestSharp.Deserializers;

namespace E.Deezer.Api
{
    public interface ITrack
    {
        int Id { get; set;  }
        string Title { get; set;  }
        string Link { get; set;  }
        uint Duration { get; set;  }
        DateTime ReleaseDate { get; set; }
        bool Explicit { get; set;  }
        string Artwork { get; set;  }
        string ArtistName { get; }
        string AlbumName { get; }

        //Methods

        Task<IArtist> GetArtist();
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
