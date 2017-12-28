using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using Newtonsoft.Json;

namespace E.Deezer.Api
{
    public interface IAlbum : IObjectWithImage
    {
        ulong Id { get;  }
        uint Tracks {get;  }
        string Title { get;  }
        string Link { get;  }
        string ArtistName { get; }
        long Rating { get; }
        DateTime ReleaseDate {get;  }
        IArtist Artist { get; }

        //Methods
        Task<IEnumerable<ITrack>> GetTracks();
        Task<bool> Rate(int aRating);

        Task<bool> AddAlbumToFavorite();
        Task<bool> RemoveAlbumFromFavorite();
    }

    internal class Album : ObjectWithImage, IAlbum, IDeserializable<IDeezerClient>
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

        public long Rating
        {
            get;
            set;
        }

        public DateTime ReleaseDate
        {
            get;
            set;
        }

        public IArtist Artist => ArtistInternal;

        [JsonProperty(PropertyName = "artist")]
        public Artist ArtistInternal
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "Url")]
        public string Link
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "nb_tracks")]
        public uint Tracks
        {
            get;
            set;
        }

        [JsonProperty(PropertyName ="tracks")]
        public DeezerFragment<Track> TracklistInternal
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


        //IDeserializable
        public IDeezerClient Client { get; set; }

        public void Deserialize(IDeezerClient aClient)
        {
            Client = aClient;

            if (ArtistInternal != null)
            {
                ArtistInternal.Deserialize(aClient);
            }
        }


        public Task<IEnumerable<ITrack>> GetTracks()
        {
            if(TracklistInternal != null)
            {
                return Task.Run(() =>
                {
                    List<ITrack> tracks = new List<ITrack>();

                    foreach(Track t in TracklistInternal.Items)
                    {
                        t.Deserialize(Client);
                        tracks.Add(t);
                    }

                    return tracks as IEnumerable<ITrack>;

                }, Client.CancellationToken);
            }


            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", Id)
            };

            return Client.Get<Track>("album/{id}/tracks", parms)
                         .ContinueWith<IEnumerable<ITrack>>((aTask) => Client.Transform<Track, ITrack>(aTask.Result), Client.CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);  
        }


        public Task<bool> Rate(int aRating)
        {
            if (aRating < 1 || aRating > 5) { throw new ArgumentOutOfRangeException("aRating", "Rating value should be between 1 and 5 (inclusive)"); }

            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", Id),
                RequestParameter.GetNewQueryStringParameter("note", aRating)
            };

            return Client.Post("album/{id}", parms, DeezerPermissions.BasicAccess);
        }


        public Task<bool> AddAlbumToFavorite() 
            => Client.User.AddAlbumToFavourite(Id);

        public Task<bool> RemoveAlbumFromFavorite() 
            => Client.User.RemoveAlbumFromFavourite(Id);  
              

        public override string ToString()
        {
            return string.Format("E.Deezer: Album({0})", Title);
        }        
    }
}
