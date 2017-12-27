using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using Newtonsoft.Json;

namespace E.Deezer.Api
{
    public interface ITrack : IObjectWithImage
    {
        ulong Id { get;   }
        string Title { get;   }
        string Link { get;   }
        uint Duration { get;   }
        DateTime ReleaseDate { get;  }
        DateTime TimeAdd { get;  }
        bool Explicit { get;  }
        string Preview { get;  }
        string ArtistName { get; }
        string AlbumName { get; }
        IArtist Artist { get; }
        IAlbum Album { get; }
        uint Number { get; }
        uint Disc { get; }

        Task<bool> AddTrackToFavorite();
        Task<bool> RemoveTrackFromFavorite();
    }


    internal class Track : ObjectWithImage, ITrack, IDeserializable<DeezerClient>
    {
        public ulong Id
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Link
        {
            get;
            set;
        }

        public uint Duration
        {
            get;
            set;
        }
        public DateTime ReleaseDate
        {
            get;
            set;
        }

        public string Artwork
        {
            get;
            set;
        }

        public bool Explicit
        {
            get;
            set;
        }

        public string Preview
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "time_add")]
        public long TimeAddInternal
        {
            get;
            set;
        }

        public IArtist Artist => ArtistInternal;

        public IAlbum Album => AlbumInternal;

        [JsonProperty(PropertyName = "track_position")]
        public uint Number
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "disc_number")]
        public uint Disc
        {
            get;
            set;
        }

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


        [JsonProperty(PropertyName = "artist")]
        public Artist ArtistInternal
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "album")]
        public Album AlbumInternal
        {
            get;
            set;
        }

        public DateTime TimeAdd => new DateTime(TimeAddInternal);


        //IDeserializable
        public DeezerClient Client
        {
            get;
            set;
        }

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


        public Task<bool> AddTrackToFavorite() 
            => Client.User.AddTrackToFavourite(Id);

        public Task<bool> RemoveTrackFromFavorite()
            => Client.User.RemoveTrackFromFavourite(Id);


        public override string ToString()
        {
            return string.Format("E.Deezer: Track({0} - ({1}))", Title, ArtistName);
        }

    }
}
