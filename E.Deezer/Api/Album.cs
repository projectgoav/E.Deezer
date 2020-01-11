using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using E.Deezer.Util;
using E.Deezer.Api.Internal;

namespace E.Deezer.Api
{
    public interface IAlbum
    {
        ulong Id { get; }
        uint Fans { get; }
        string UPC { get; }
        uint TrackCount { get; }
        int Rating { get; }
        string Link { get; }
        string Title { get; }
        string Label { get; }
        ulong GenreId { get; }
        uint Duration { get; }
        bool HasRating { get; }
        IArtist Artist { get; }
        bool Available { get; }
        string ShareLink { get; }
        string ArtistName { get; }
        string RecordType { get; }
        DateTime? ReleaseDate { get; }
        bool HasExplicitLyrics { get; }
        IImages CoverArtwork { get; }
        IAlbum AlternativeAlbum { get; }
        IEnumerable<IGenre> Genre { get; }
        IEnumerable<IArtist> Contributors { get; }


        //Methods
        Task<IEnumerable<ITrack>> GetTracks(CancellationToken cancellationToken);

        Task<IEnumerable<IUserProfile>> GetFans(CancellationToken cancellationToken, uint start = 0, uint count = 10);

        Task<IEnumerable<IComment>> GetComments(CancellationToken cancellationToken, uint start = 0, uint count = 10);


        Task<bool> Rate(DeezerRating rating, CancellationToken cancellationToken);

        Task<ulong> CommentOn(string commentText, CancellationToken cancellationToken);


        Task<bool> Favourite(CancellationToken cancellationToken);
        Task<bool> Unfavourite(CancellationToken cancellationToken);
    }

    internal class Album : IAlbum, IClientObject
    {
        public ulong Id { get; private set;  }

        public string Title { get; private set; }

        public string UPC { get; private set; }

        public string Link { get; private set; } 

        public int Rating { get; private set; }

        public string Label { get; private set; }

        public uint Duration { get; private set; }

        public uint Fans { get; private set; }

        public string RecordType { get; private set; }

        public bool Available { get; private set; }

        public DateTime? ReleaseDate { get; private set; }

        public IImages CoverArtwork { get; private set; }

        public IArtist Artist { get; private set; }

        public IAlbum AlternativeAlbum { get; private set; }

        public IEnumerable<IGenre> Genre { get; private set; }

        public IEnumerable<IArtist> Contributors { get; private set; }


        public bool HasRating => Rating > 0;

        public string ArtistName => Artist?.Name ?? string.Empty;


        public string ShareLink { get; private set; }

        public bool HasExplicitLyrics { get; private set; }

        public ulong GenreId { get; private set; }

        public uint TrackCount { get; private set; }

        internal IEnumerable<ITrack> TracklistInternal { get; set; }


        //IClientObject
        public IDeezerClient Client { get; private set; }


        public Task<IEnumerable<ITrack>> GetTracks(CancellationToken cancellationToken)
            => this.Client.Endpoints.Albums.GetAlbumTracks(this, cancellationToken);


        public Task<IEnumerable<IUserProfile>> GetFans(CancellationToken cancellationToken,
                                                       uint start = 0,
                                                       uint count = 10)
            => this.Client.Endpoints.Albums.GetAlbumFans(this, cancellationToken, start, count);

        public Task<IEnumerable<IComment>> GetComments(CancellationToken cancellationToken,
                                                       uint start = 0,
                                                       uint count = 10)
            => this.Client.Endpoints.Albums.GetAlbumComments(this, cancellationToken, start, count);


        public Task<bool> Rate(DeezerRating rating, CancellationToken cancellationToken)
            => this.Client.Endpoints.Albums.RateAlbum(this, rating, cancellationToken);


        public Task<ulong> CommentOn(string commentText, CancellationToken cancellationToken)
            => this.Client.Endpoints.Albums.AddComment(this, commentText, cancellationToken);


        public Task<bool> Favourite(CancellationToken cancellationToken)
            => this.Client.Endpoints.User.FavouriteAlbum(this, cancellationToken);

        public Task<bool> Unfavourite(CancellationToken cancellationToken)
            => this.Client.Endpoints.User.UnfavouriteAlbum(this, cancellationToken);

        
        public override string ToString()
            => string.Format("E.Deezer: Album({0})", Title);



        // JSON
        internal const string ID_PROPERTY_NAME = "id";
        internal const string TITLE_PROPERTY_NAME = "title";
        internal const string UPC_PROPERTY_NAME = "upc";
        internal const string LINK_PROPERTY_NAME = "link";
        internal const string SHARE_LINK_PROPERTY_NAME = "share";
        internal const string GENRE_LIST_PROPERTY_NAME = "genres";
        internal const string GENRE_ID_PROPERTY_NAME = "genre_id";
        internal const string LABEL_PROPERTY_NAME = "label";
        internal const string TRACK_COUNT_PROEPRTY_NAME = "nb_tracks";
        internal const string DURATION_PROPERTY_NAME = "duration";
        internal const string FANS_PROPERTY_NAME = "fans";
        internal const string RATING_PROPERTY_NAME = "rating";
        internal const string RELEASE_DATE_PROPERTY_NAME = "release_date";
        internal const string RECORD_TYPE_PROPERTY_NAME = "record_type";
        internal const string AVAILABLE_PROPERTY_NAME = "available";
        internal const string TRACKLIST_PROPERTY_NAME = "tracklist"; //TODO: We could ignore this as we get it? AND/OR don't expose it...

        // TODO: Explicit values. 
        // TODO: Need to re-read the docs on these

        internal const string CONTRIBUTORS_PROPERTY_NAME = "contributors";
        internal const string ARTIST_PROPERTY_NAME = "artist";

        internal const string TRACKS_PROPERTY_NAME = "tracks";


        public static IAlbum FromJson(JToken json, IDeezerClient client)
        {
            if (json == null)
            {
                return null;
            }

            string apiDateString = json.Value<string>(RELEASE_DATE_PROPERTY_NAME);
            DateTime? releaseDate = DateTimeExtensions.ParseApiDateTime(apiDateString);

            return new Album()
            {
                Id = json.Value<ulong>(ID_PROPERTY_NAME),
                Title = json.Value<string>(TITLE_PROPERTY_NAME),

                UPC = json.Value<string>(UPC_PROPERTY_NAME),

                Link = json.Value<string>(LINK_PROPERTY_NAME),
                ShareLink = json.Value<string>(SHARE_LINK_PROPERTY_NAME),

                CoverArtwork = Images.FromJson(json),

                GenreId = json.ValueOrDefault<ulong>(GENRE_ID_PROPERTY_NAME, 0),
                Genre = FragmentOf<IGenre>.FromJson(json[GENRE_LIST_PROPERTY_NAME],
                                                    x => Api.Genre.FromJson(x, client)),

                Fans = json.Value<uint>(FANS_PROPERTY_NAME),
               
                Rating = json.Value<int>(RATING_PROPERTY_NAME),

                Label = json.Value<string>(LABEL_PROPERTY_NAME),
                TrackCount = json.Value<uint>(TRACK_COUNT_PROEPRTY_NAME),
                Duration = json.Value<uint>(DURATION_PROPERTY_NAME),

                ReleaseDate = releaseDate,

                Available = json.Value<bool>(AVAILABLE_PROPERTY_NAME),

                RecordType = json.Value<string>(RECORD_TYPE_PROPERTY_NAME),

                Contributors = CollectionOf<IArtist>.FromJson(json[CONTRIBUTORS_PROPERTY_NAME],
                                                              x => Api.Artist.FromJson(x, client)),

                Artist = Api.Artist.FromJson(json[ARTIST_PROPERTY_NAME], client),

                TracklistInternal = FragmentOf<ITrack>.FromJson(json[TRACKS_PROPERTY_NAME],
                                                                x => Api.Track.FromJson(x, client)),


                // ISessionObject
                Client = client,                                   
            };
        }
    }
}
