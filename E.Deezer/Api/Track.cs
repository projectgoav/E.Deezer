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
        ulong Id { get; set;  }
        string Title { get; set;  }
        string Link { get; set;  }
        uint Duration { get; set;  }
        DateTime ReleaseDate { get; set; }
        DateTime TimeAdd { get; set; }
        bool Explicit { get; set; }
        string Preview { get; set; }
        string ArtistName { get; }
        string AlbumName { get; }
        IArtist Artist { get; }
        IAlbum Album { get; }
        uint Number { get; }
        uint Disc { get; }

        string GetCover(PictureSize aSize);
        bool HasCover(PictureSize aSize);
    }


    internal class Track : ObjectWithImage, ITrack, IDeserializable<DeezerClient>
    {
        public ulong Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        [DeserializeAs(Name = "time_add")]
        public DateTime TimeAdd { get; set; }
        public uint Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Artwork { get; set; }
        public bool Explicit { get; set; }
        public string Preview { get; set; }
        public IArtist Artist { get { return ArtistInternal; } }
        public IAlbum Album { get { return AlbumInternal; } }

        [DeserializeAs(Name = "track_position")]
        public uint Number { get; set; }

        [DeserializeAs(Name = "disc_number")]
        public uint Disc { get; set; }

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

            if (ArtistInternal != null)
            {
                ArtistInternal.Deserialize(aClient);
            }

            if (AlbumInternal != null)
            {
                AlbumInternal.Deserialize(aClient);
            }
        }


        //Tracks don't often come with their own images so if there is none, we can use that from the album in which it belongs.
        public override string GetPicture(PictureSize aSize)
        {
            string url = base.GetPicture(aSize);
            return (url == string.Empty) ? AlbumInternal.GetPicture(aSize) : url;
        }

        public override bool HasPicture(PictureSize aSize)
        {
            bool baseResult = base.HasPicture(aSize);
            return (baseResult) ? baseResult : AlbumInternal.HasPicture(aSize);
        }




        [Obsolete("Please use GetPicture instead.", true)]
        public string GetCover(PictureSize aSize) {  return GetPicture(aSize); }

        [Obsolete("Please use HasPicture instead.", true)]
        public bool HasCover(PictureSize aSize) {  return HasPicture(aSize); }

        public override string ToString()
        {
            return string.Format("E.Deezer: Track({0} - ({1}))", Title, ArtistName);
        }
    }
}
