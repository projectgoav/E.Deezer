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
        IImages Artwork { get; }
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
        IEnumerable<IArtist> Contributors { get; }

        [Obsolete("Use of IsExplicit is encouraged")]
        bool Explicit { get; }


        Task<bool> Favourite(CancellationToken cancellationToken);
        Task<bool> Unfavourite(CancellationToken cancelllationToken);

    }


    internal class Track : ITrack, IClientObject
    {
        private class TrackImages : IImages
        {
            private const string DEFAULT_IMAGE = "";

            public TrackImages(IAlbum containingAlbum,
                               string smallImage,
                               string mediumImage,
                               string largeImage,
                               string extraLargeImage)
            {
                this.Small = string.IsNullOrEmpty(smallImage) ? containingAlbum?.CoverArtwork?.Small ?? DEFAULT_IMAGE
                                                              : smallImage;

                this.Medium = string.IsNullOrEmpty(mediumImage) ? containingAlbum?.CoverArtwork?.Medium ?? DEFAULT_IMAGE
                                                                : mediumImage;

                this.Large = string.IsNullOrEmpty(largeImage) ? containingAlbum?.CoverArtwork?.Large ?? DEFAULT_IMAGE
                                                              : largeImage;

                this.ExtraLarge = string.IsNullOrEmpty(extraLargeImage) ? containingAlbum?.CoverArtwork?.ExtraLarge ?? DEFAULT_IMAGE
                                                                        : extraLargeImage;
            }


            //IImages
            public string Small { get; }
            public string Medium { get; }
            public string Large { get; }
            public string ExtraLarge { get; }
        }


        public ulong Id { get; private set; }

        public string Title { get; private set; }

        public string Link { get; private set; }

        public uint Duration { get; private set; }

        public string PreviewLink { get; private set; }

        /* In the future we could have a ITrackBasic, which has the populated information
         * from most of the API calls and then an ITrackFull (or better names) for the full
         * information.
         * 
         * See Github issue: https://github.com/projectgoav/E.Deezer/issues/89
         * for some more background on this. */
        public float BPM { get; private set; }

        public float Gain { get; private set; }

        public uint Rank { get; private set; }

        public string ISRC { get; private set; }

        public IAlbum Album { get; private set; }

        public IArtist Artist { get; private set; }

        public IImages Artwork { get; private set; }

        public DateTime? AvailableFrom { get; private set; }

        public ITrack AlternativeTrack { get; private set; }
        
        public IEnumerable<string> AvailableIn { get; private set; }

        public IEnumerable<IArtist> Contributors { get; private set; }

        public string ShareLink { get; private set; }

        public bool IsExplicit { get; private set; }

        public string ShortTitle { get; private set; }

        public uint TrackNumber { get; private set; }

        public uint DiscNumber { get; private set; }

        public DateTime? ReleaseDate { get; private set; }


        // IClientObject
        public IDeezerClient Client { get; private set; }


        public string ArtistName => Artist?.Name ?? string.Empty;

        public string AlbumName => Album?.Title ?? string.Empty;


        [Obsolete("Use of IsExplicit is encouraged")]
        public bool Explicit => IsExplicit;



        public Task<bool> Favourite(CancellationToken cancellationToken)
            => this.Client.Endpoints.User.FavouriteTrack(this, cancellationToken);

        public Task<bool> Unfavourite(CancellationToken cancellationToken)
            => this.Client.Endpoints.User.UnfavouriteTrack(this, cancellationToken);

   
        public override string ToString()
            => string.Format("E.Deezer: Track({0} - ({1}))", Title, ArtistName);


        //JSON
        internal const string ID_PROPERTY_NAME = "id";
        internal const string TITLE_PROPERTY_NAME = "title";
        internal const string SHORT_TITLE_PROPERTY_NAME = "title_short";
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
        internal const string AVAILABLE_COUNTRY_PROPERTY_NAME = "available_countries";
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

            /* Tracks don't always come with their own artwork.
             * Instead, we'll pinch the artwork from the 'Album' property
             * if this is available in the returned JSON. */
        var containedInAlbum = Api.Album.FromJson(json[ALBUM_PROPERTY_NAME], client);

            var internalArtwork = Api.Images.FromJson(json);

            var actualArtwork = new TrackImages(containedInAlbum, 
                                                internalArtwork?.Small, 
                                                internalArtwork?.Medium, 
                                                internalArtwork?.Large, 
                                                internalArtwork?.ExtraLarge);

            bool hasAvailabiltyList = (json as JObject)?.ContainsKey(AVAILABLE_COUNTRY_PROPERTY_NAME) ?? false;

            // Json.Values<string>() is lazy, need to wrap and eval into a list at this point
            // as the underlying json is disposed as soon as the parsing has been completed.
            IEnumerable<string> availableInList = hasAvailabiltyList ? new List<string>(json[AVAILABLE_COUNTRY_PROPERTY_NAME].Values<string>())
                                                                     : new List<string>(0);

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

                Artwork = actualArtwork,

                AvailableIn = availableInList,

                Artist = Api.Artist.FromJson(json[ARTIST_PROPERTY_NAME], client),
                Album = containedInAlbum,
                Contributors = CollectionOf<IArtist>.FromJson(json[CONTRIBUTORS_PROPERTY_NAME], x => Api.Artist.FromJson(x, client)),

                // ISssionObject
                Client = client,
            };
        }
    }
}
