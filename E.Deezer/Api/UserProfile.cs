using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using E.Deezer.Api.Internal;

namespace E.Deezer.Api
{
    public interface IUserProfile
    {
        ulong Id { get; }
        string Link { get; }
        string Country { get; }
        string Username { get; }
        IImages ProfilePictures { get; }

        
        Task<IEnumerable<ITrack>> Flow(CancellationToken cancellationToken, uint start = 0, uint count = 50);

        Task<IEnumerable<IUserProfile>> Followers(CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IUserProfile>> Followings(CancellationToken cancellationToken, uint start = 0, uint count = 25);

        Task<IEnumerable<IAlbum>> FavouriteAlbums(CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IArtist>> FavouriteArtists(CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IPlaylist>> FavouritePlaylists(CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<ITrack>> FavouriteTracks(CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IRadio>> FavouriteRadio(CancellationToken cancellation, uint start = 0, uint count = 25);
    }

    internal class UserProfile : IUserProfile, IClientObject
    {
        public ulong Id { get; private set; }

        public string Username { get; private set; }

        public string Link { get; private set; }

        public string Country { get; private set; }

        public IImages ProfilePictures { get; private set; }

        
        //IUserProfile
        public IDeezerClient Client { get; private set; }



        public Task<IEnumerable<ITrack>> Flow(CancellationToken cancellationToken, uint start = 0, uint count = 50)
            => this.Client.Endpoints.User.GetFlow(this, cancellationToken, start, count);


        public Task<IEnumerable<IUserProfile>> Followers(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.User.GetFollowers(this.Id, cancellationToken, start, count);

        public Task<IEnumerable<IUserProfile>> Followings(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.User.GetFollowings(this.Id, cancellationToken, start, count);


        public Task<IEnumerable<IAlbum>> FavouriteAlbums(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.User.GetFavouriteAlbums(this, cancellationToken, start, count);

        public Task<IEnumerable<IArtist>> FavouriteArtists(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.User.GetFavouriteArtists(this, cancellationToken, start, count);

        public Task<IEnumerable<IPlaylist>> FavouritePlaylists(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.User.GetFavouritePlaylists(this, cancellationToken, start, count);

        public Task<IEnumerable<ITrack>> FavouriteTracks(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.User.GetFavouriteTracks(this, cancellationToken, start, count);

        public Task<IEnumerable<IRadio>> FavouriteRadio(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.User.GetFavouriteRadio(this, cancellationToken, start, count);


        public override string ToString()
            => string.Format("E.Deezer.UserProfile: {0} :: ({1})", this.Username, this.Id);


        //JSON
        internal const string ID_PROPERTY_NAME = "id";
        internal const string USERNAME_PROPERTY_NAME = "name";
        internal const string LINK_PROPERTY_NAME = "link";
        internal const string COUNTRY_PROPERTY_NAME = "country";


        public static IUserProfile FromJson(JToken json, IDeezerClient client)
        {
            if (json == null)
            {
                return null;
            }

            return new UserProfile()
            {
                Id = ulong.Parse(json.Value<string>(ID_PROPERTY_NAME)),

                Username = json.Value<string>(USERNAME_PROPERTY_NAME),

                Link = json.Value<string>(LINK_PROPERTY_NAME),

                Country = json.Value<string>(COUNTRY_PROPERTY_NAME),

                ProfilePictures = Api.Images.FromJson(json),


                Client = client,
            };
        }
    }
}
