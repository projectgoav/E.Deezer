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
        ulong Id { get; }
        uint Fans { get; }
        string UPC { get; }
        uint Tracks { get; }
        long Rating { get; }
        string Link { get; }
        string Title { get; }
        string Label { get; }
        uint GenreId { get; }
        uint Duration { get; }
        bool HasRating { get; }
        IArtist Artist { get; }
        bool Available { get; }
        string ShareLink { get; }
        string ArtistName { get; }
        string RecordType { get; }
        DateTime ReleaseDate { get; }
        bool HasExplicitLyrics { get; }
        IAlbum AlternativeAlbum { get; }
        IEnumerable<IGenre> Genre { get; }
        IEnumerable<IArtist> Contributors { get; }


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

        public string UPC
        {
            get;
            set;
        }

        public string Link
        {
            get;
            set;
        }

        public long Rating
        {
            get;
            set;
        }

        public string Label
        {
            get;
            set;
        }

        public uint Duration
        {
            get;
            set;
        }

        public uint GenreId
        {
            get;
            set;
        }

        public uint Fans
        {
            get;
            set;    
        }

        public string RecordType
        {
            get;
            set;
        }

        public bool Available
        {
            get;
            set;
        }

        public DateTime ReleaseDate
        {
            get;
            set;
        }

        public bool HasRating => Rating > 0;

        public IArtist Artist => ArtistInternal;

        public IAlbum AlternativeAlbum => AlternativeInternal;

        public IEnumerable<IGenre> Genre => GenreInternal?.Items;

        public IEnumerable<IArtist> Contributors => ContributorsInternal;

        public string ArtistName => ArtistInternal?.Name ?? string.Empty;



        [JsonProperty(PropertyName = "share")]
        public string ShareLink
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "explicit_lyrics")]
        public bool HasExplicitLyrics
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "artist")]
        public Artist ArtistInternal
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "genre")]
        public DeezerFragment<Genre> GenreInternal
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

        [JsonProperty(PropertyName = "contributors")]
        public List<Artist> ContributorsInternal
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "alternative")]
        public Album AlternativeInternal
        {
            get;
            set;
        }

        //IDeserializable
        public IDeezerClient Client { get; set; }

        public void Deserialize(IDeezerClient aClient)
        {
            Client = aClient;

            ArtistInternal?.Deserialize(aClient);

            if (GenreInternal != null)
            {
                foreach (var genre in GenreInternal.Items)
                {
                    genre.Deserialize(aClient);
                }
            }

            if(this.ContributorsInternal != null)
            {
                foreach(var artist in ContributorsInternal)
                {
                    artist.Deserialize(aClient);
                }
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
