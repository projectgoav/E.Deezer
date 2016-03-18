using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using RestSharp.Deserializers;

namespace E.Deezer.Api
{
    public interface ITrack : IObjectWithImage
    {
        uint Id { get; set;  }
        string Title { get; set;  }
        string Link { get; set;  }
        uint Duration { get; set;  }
        DateTime ReleaseDate { get; set; }
        bool Explicit { get; set; }
        string Preview { get; set; }
        string ArtistName { get; }
        string AlbumName { get; }
        IArtist Artist { get; }
        IAlbum Album { get; }

        string GetCover(PictureSize aSize);
        bool HasCover(PictureSize aSize);
    }


    internal class Track : ObjectWithImage, ITrack, IDeserializable<DeezerClient>
    {
        public uint Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public uint Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Artwork { get; set; }
        public bool Explicit { get; set; }
        public string Preview { get; set; }
        public IArtist Artist { get { return ArtistInternal; } }
        public IAlbum Album { get { return AlbumInternal; } }

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

        public DeezerClient Client { get; set; }
        public void Deserialize(DeezerClient aClient) 
        { 
            Client = aClient;
        }

        [Obsolete("Please use GetPicture instead.")]
        public string GetCover(PictureSize aSize) {  return GetPicture(aSize); }

        [Obsolete("Please use HasPicture instead.")]
        public bool HasCover(PictureSize aSize) {  return HasPicture(aSize); }

        public override string ToString()
        {
            return string.Format("E.Deezer: Track({0} - ({1}))", Title, ArtistName);
        }
    }
}
