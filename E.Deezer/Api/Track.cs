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
        bool Explicit { get; set; }
        string ArtistName { get; }
        string AlbumName { get; }

        //Methods
        Task<IArtist> GetArtist();
        Task<IAlbum> GetAlbum();

        string GetCover(PictureSize aSize);
        bool HasCover(PictureSize aSize);
    }


    internal class Track : ITrack, IDeserializable<DeezerClient>
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

        //Pictures
        [DeserializeAs(Name = "picture_small")]
        private string SMPicture { get; set; }

        [DeserializeAs(Name = "picture_medium")]
        private string MDPicture { get; set; }

        [DeserializeAs(Name = "picture_big")]
        private string BGPicture { get; set; }

        public DeezerClient Client { get; set; }
        public void Deserialize(DeezerClient aClient) 
        { 
            Client = aClient;
        }


        public string GetCover(PictureSize aSize)
        {
            switch (aSize)
            {
                case PictureSize.SMALL: { return string.IsNullOrEmpty(SMPicture) ? string.Empty : SMPicture; }
                case PictureSize.MEDIUM: { return string.IsNullOrEmpty(MDPicture) ? string.Empty : MDPicture; }
                case PictureSize.LARGE: { return string.IsNullOrEmpty(BGPicture) ? string.Empty : BGPicture; }
                default: { return string.Empty; }
            }
        }

        public bool HasCover(PictureSize aSize)
        {
            switch (aSize)
            {
                case PictureSize.SMALL: { return string.IsNullOrEmpty(SMPicture); }
                case PictureSize.MEDIUM: { return string.IsNullOrEmpty(MDPicture); }
                case PictureSize.LARGE: { return string.IsNullOrEmpty(BGPicture); }
                default: { return false; }
            }
        }


        public Task<IArtist> GetArtist() { return Task.Factory.StartNew<IArtist>(() => ArtistInternal); }

        public Task<IAlbum> GetAlbum() { return Task.Factory.StartNew<IAlbum>(() => AlbumInternal); }


        public override string ToString()
        {
            return string.Format("E.Deezer: Track({0} - ({1}))", Title, ArtistName);
        }
    }
}
