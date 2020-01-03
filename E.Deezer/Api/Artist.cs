using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace E.Deezer.Api
{
    public interface IArtist
    {
        ulong Id { get; }
        uint NumberOfFans { get; }
        string Name { get; }
        string Link { get; }
        uint NumberOfAlbums { get; }
        string ShareLink { get; }
        bool HasSmartRadio { get; }

        Task<IEnumerable<ITrack>> TopTracks(CancellationToken cancellationToken, uint start = 0, uint count = 25);

        Task<IEnumerable<IAlbum>> Albums(CancellationToken cancellationToken, uint start = 0, uint count = 10);

        Task<IEnumerable<IArtist>> RelatedArtists(CancellationToken cancellationToken, uint start = 0, uint count = 10);

        Task<IEnumerable<ITrack>> SmartRadio(CancellationToken cancellationToken, uint start = 0, uint count = 25);

        Task<IEnumerable<IPlaylist>> PlaylistsFeaturingArtist(CancellationToken cancellationToken, uint start = 0, uint count = 25);


        Task<IEnumerable<IUserProfile>> Fans(CancellationToken cancellationToken, uint start = 0, uint count = 10);

        Task<IEnumerable<IComment>> Comments(CancellationToken cancellationToken, uint start = 0, uint count = 10);


        Task<bool> Rate(DeezerRating rating, CancellationToken cancellationToken);

        Task<ulong> CommentOn(string commentText, CancellationToken cancellationToken);


        Task<bool> Favourite(CancellationToken cancellationToken);
        Task<bool> Unfavourite(CancellationToken cancellationToken);
    }

    internal class Artist : IArtist, IClientObject
    { 
        public ulong Id { get; private set; }

        public string Name { get; private set; }

        public string Link { get; private set; }

        public string ShareLink { get; private set; }

        public bool HasSmartRadio { get; private set; }
        
        public uint NumberOfAlbums { get; private set; }

        public uint NumberOfFans { get; private set; }

        public IImages Images { get; private set; }


        // IClientObject
        public IDeezerClient Client { get; private set; }


        public Task<IEnumerable<ITrack>> TopTracks(CancellationToken cancellationToken,
                                                   uint start = 0,
                                                   uint count = 25)
            => this.Client.Endpoints.Artists.GetArtistsTopTracks(this, cancellationToken, start, count);


        public Task<IEnumerable<IAlbum>> Albums(CancellationToken cancellationToken,
                                                uint start = 0,
                                                uint count = 10)
            => this.Client.Endpoints.Artists.GetArtistsAlbums(this, cancellationToken, start, count);


        public Task<IEnumerable<IArtist>> RelatedArtists(CancellationToken cancellationToken,
                                                         uint start = 0,
                                                         uint count = 10)
            => this.Client.Endpoints.Artists.GetRelatedArtists(this, cancellationToken, start, count);


        public Task<IEnumerable<ITrack>> SmartRadio(CancellationToken cancellationToken,
                                                    uint start = 0,
                                                    uint count = 10)
            => this.Client.Endpoints.Artists.GetArtistsRadio(this, cancellationToken, start, count);


        public Task<IEnumerable<IPlaylist>> PlaylistsFeaturingArtist(CancellationToken cancellationToken,
                                                                     uint start = 0,
                                                                     uint count = 10)
            => this.Client.Endpoints.Artists.GetPlaylistsFeaturingArtist(this, cancellationToken, start, count);


        public Task<IEnumerable<IUserProfile>> Fans(CancellationToken cancellationToken,
                                                    uint start = 0,
                                                    uint count = 10)
            => this.Client.Endpoints.Artists.GetArtistsFans(this, cancellationToken, start, count);

        public Task<IEnumerable<IComment>> Comments(CancellationToken cancellationToken,
                                                    uint start = 0,
                                                    uint count = 10)
            => this.Client.Endpoints.Artists.GetArtistsComments(this, cancellationToken, start, count);



        public Task<bool> Rate(DeezerRating rating, CancellationToken cancellationToken)
            => this.Client.Endpoints.Artists.RateArtist(this, rating, cancellationToken);

        public Task<ulong> CommentOn(string commentText, CancellationToken cancellationToken)
            => this.Client.Endpoints.Artists.CommentOnArtist(this, commentText, cancellationToken);



        public Task<bool> Favourite(CancellationToken cancellationToken)
            => this.Client.Endpoints.User.FavouriteArtist(this, cancellationToken);

        public Task<bool> Unfavourite(CancellationToken cancellationToken)
            => this.Client.Endpoints.User.UnfavouriteArtist(this, cancellationToken);


        public override string ToString()
            => string.Format("E.Deezer: Artist({0})", Name);


        //JSON
        internal const string ID_PROPERTY_NAME = "id";
        internal const string NAME_PROPERTY_NAME = "name";
        internal const string LINK_PROPERTY_NAME = "link";
        internal const string SHARE_LINK_PROPERTY_NAME = "share";
        internal const string ALBUM_COUNT_PROPERTY_NAME = "nb_album";
        internal const string FAN_COUNT_PROPERTY_NAME = "nb_fan";
        internal const string HAS_RADIO_PROPERTY_NAME = "radio";

        public static IArtist FromJson(JToken json, IDeezerClient client)
        {
            if (json == null)
            {
                return null;
            }

            return new Artist()
            {
                Id = json.Value<ulong>(ID_PROPERTY_NAME),
                Name = json.Value<string>(NAME_PROPERTY_NAME),
                Link = json.Value<string>(LINK_PROPERTY_NAME),
                ShareLink = json.Value<string>(SHARE_LINK_PROPERTY_NAME),

                NumberOfAlbums = json.Value<uint>(ALBUM_COUNT_PROPERTY_NAME),
                NumberOfFans = json.Value<uint>(FAN_COUNT_PROPERTY_NAME),

                Images = Api.Images.FromJson(json),

                HasSmartRadio = json.Value<bool>(HAS_RADIO_PROPERTY_NAME),


                Client = client,
            };
        }
    }
}
