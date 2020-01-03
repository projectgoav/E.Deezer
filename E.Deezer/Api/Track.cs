using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using E.Deezer.Util;
using E.Deezer.Api.Internal;

namespace E.Deezer.Api
{
    public interface ITrack
    {
        ulong Id { get; }
        uint DiscNumber { get; }
        uint Rank { get; }
        float BPM { get; }
        float Gain { get; }
        string Link { get; }
        uint TrackNumber { get; }
        string ISRC { get; }
        string Title { get; }
        IAlbum Album { get; }
        uint Duration { get; }
        string PreviewLink { get; }
        IArtist Artist { get; }
        bool IsExplicit { get; }
        string AlbumName { get; }
        //DateTime? TimeAdd { get; }
        string ShareLink { get; }
        string ShortTitle { get; }
        string ArtistName { get; }
        DateTime? ReleaseDate { get; }
        ITrack AlternativeTrack { get; }
        IEnumerable<string> AvailableIn { get; }
        //IEnumerable<IArtist> Contributors { get; }

        [Obsolete("Use of IsExplicit is encouraged")]
        bool Explicit { get; }


        Task<bool> Favourite(CancellationToken cancellationToken);
        Task<bool> Unfavourite(CancellationToken cancelllationToken);

    }


    internal class Track : ITrack, IClientObject
    {
        public ulong Id { get; private set; }

        public string Title { get; private set; }

        public string Link { get; private set; }

        public uint Duration { get; private set; }

        public string Artwork { get; private set; }

        public string PreviewLink { get; private set; }

        // TODO, Merge some of these properties into a 'Playback Info' object / class??
        public float BPM { get; private set; }

        public float Gain { get; private set; }

        public uint Rank { get; private set; }

        public string ISRC { get; private set; }

        public IAlbum Album { get; private set; }

        public IArtist Artist { get; private set; }

        public DateTime? AvailableFrom { get; private set; }

        public ITrack AlternativeTrack { get; private set; }
        
        public IEnumerable<string> AvailableIn { get; private set; }

        public IEnumerable<IArtist> Conrtibutors { get; private set; }

        public string ShareLink { get; private set; }

        public bool IsExplicit { get; private set; }

        public string ShortTitle { get; private set; }

        public uint TrackNumber { get; private set; }

        public uint DiscNumber { get; private set; }

        public DateTime? ReleaseDate { get; private set; }


        // IClientObject
        public IDeezerClient Client { get; private set; }


        // TODO? 
        public string ArtistName => Artist?.Name ?? string.Empty;

        public string AlbumName => Album?.Title ?? string.Empty;


        [Obsolete("Use of IsExplicit is encouraged")]
        public bool Explicit => IsExplicit;



        public Task<bool> Favourite(CancellationToken cancellationToken)
            => this.Client.Endpoints.User.FavouriteTrack(this, cancellationToken);

        public Task<bool> Unfavourite(CancellationToken cancellationToken)
            => this.Client.Endpoints.User.UnfavouriteTrack(this, cancellationToken);


        /*
        //Tracks don't often come with their own images so if there is none, we can use that from the album in which it belongs.
        //TODO -> Need to port this across to do this with the internal album :)
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

        */
   
        public override string ToString()
            => string.Format("E.Deezer: Track({0} - ({1}))", Title, ArtistName);


        //JSON

        internal const string ID_PROPERTY_NAME = "id";
        internal const string TITLE_PROPERTY_NAME = "title";
        internal const string SHORT_TITLE_PROPERTY_NAME = "short_title";
        internal const string ISRC_PROPERTY_NAME = "isrc";
        internal const string LINK_PROPERTY_NAME = "link";
        internal const string SHARE_LINK_PROPERTY_NAME = "share";
        internal const string DURATION_PROPERTY_NAME = "duration";
        internal const string TRACK_NUMBER_PROPERTY_NAME = "track_position";
        internal const string DISC_NUMBER_PROPERTY_NAME = "disk_number";
        internal const string RANK_PROPERTY_NAME = "rank";
        internal const string RELEASE_PROPERTY_NAME = "release_date";
        internal const string PREVIEW_PROPERTY_NAME = "preview";
        internal const string BPM_PROPERTY_NAME = "bpm";
        internal const string GAIN_PROPERTY_NAME = "gain";
        internal const string AVAIlABLE_COUNTRY_PROPERTY_NAME = "available_countries";
        internal const string CONTRIBUTORS_PROPERTY_NAME = "contributors";
        internal const string ARTIST_PROPERTY_NAME = "artist";
        internal const string ALBUM_PROPERTY_NAME = "album";

        //TODO Readable
        //TODO Title_version
        //TODO Explicit


        public static ITrack FromJson(JToken json, IDeezerClient client)
        {
            var releaseDateString = json.Value<string>(RELEASE_PROPERTY_NAME);
            DateTime? releaseDate = DateTimeExtensions.ParseApiDateTime(releaseDateString);

            //TODO -> AvailableFrom property??
            //var availabilityDateString = json.Value<String>(TimeAdd)

            return new Track()
            {
                Id = json.Value<ulong>(ID_PROPERTY_NAME),

                Title = json.Value<string>(TITLE_PROPERTY_NAME),
                ShortTitle = json.Value<string>(SHORT_TITLE_PROPERTY_NAME),

                ISRC = json.Value<string>(ISRC_PROPERTY_NAME),
                Link = json.Value<string>(LINK_PROPERTY_NAME),
                ShareLink = json.Value<string>(SHARE_LINK_PROPERTY_NAME),
                PreviewLink = json.Value<string>(PREVIEW_PROPERTY_NAME),

                Duration = json.Value<uint>(DURATION_PROPERTY_NAME),
                BPM = json.Value<float>(BPM_PROPERTY_NAME),
                Gain = json.Value<float>(GAIN_PROPERTY_NAME),

                TrackNumber = json.Value<uint>(TRACK_NUMBER_PROPERTY_NAME),
                DiscNumber = json.Value<uint>(DISC_NUMBER_PROPERTY_NAME),

                Rank = json.Value<uint>(RANK_PROPERTY_NAME),

                ReleaseDate = releaseDate,

                //TODO: Will need to check for null values here...
                //AvailableIn = json.Values<string>(AVAIlABLE_COUNTRY_PROPERTY_NAME),

                Artist = Api.Artist.FromJson(json[ARTIST_PROPERTY_NAME], client),
                Album = Api.Album.FromJson(json[ALBUM_PROPERTY_NAME], client),
                Conrtibutors = CollectionOf<IArtist>.FromJson(json[CONTRIBUTORS_PROPERTY_NAME], x => Api.Artist.FromJson(x, client)),


                Client = client,
            };
        }
    }
}
