using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
 
using E.Deezer.Util;


namespace E.Deezer.Api
{
	public interface IUser
	{
        ulong Id { get; }
        string Username { get;  }
        string Lastname { get;  }
        string Firstname { get;  }
        string Email { get;  }
        int Status { get;  }
        DateTime? Birthday { get;  }
        DateTime? InscriptionDate { get;  }
        string Gender { get;  }
        string Link { get;  }

        IImages ProfilePictures { get; }

        string Country { get; }
        string Language { get; }
        bool IsKid { get; }


        // ** Methods **
        //Favourites
        /*

        Task<IEnumerable<ITrack>> GetPersonalTracks(uint aStart = 0 , uint aCount = 100);


        Task<IEnumerable<ITrack>> GetFlow(uint aStart = 0 , uint aCount = 100);

        Task<IEnumerable<ITrack>> GetHistory(uint aStart = 0, uint aCount = 100);

        //Recommendations

        Task<ulong> CreatePlaylist(string title);
 
        */
    }

	internal class User : IUser
	{
        public ulong Id { get; private set; }
        public string Username { get; private set; }
        public string Lastname { get; private set; }
        public string Firstname { get; private set; }
        public string Email { get; private set; }
        public int Status { get; private set; }
        public DateTime? Birthday { get; private set; }
        public DateTime? InscriptionDate { get; private set; }
        public string Gender { get; private set; }
        public string Link { get; private set; }
        public string Country { get; private set; }
        public string Language { get; private set; }
        public bool IsKid { get; private set; }
        public IImages ProfilePictures { get; private set; }


        public override string ToString() => Username; 

        /* METHODS
         * ~~~~~~~~

        public Task<IEnumerable<ITrack>> GetPersonalTracks(uint aStart = 0, uint aCount = 100)   
            => Get<Track, ITrack>("personal_songs", DeezerPermissions.BasicAccess, aStart, aCount); 

        */

        //JSON
        internal const string ID_PROPERTY_NAME = "id";
        internal const string USERNAME_PROPERTY_NAME = "username";
        internal const string FIRSTNAME_PROPERTY_NAME = "firstname";
        internal const string LASTNAME_PROPERTY_NAME = "lastname";
        internal const string EMAIL_PROPERTY_NAME = "email";
        internal const string GENDER_PROPERTY_NAME = "gender";
        internal const string STATUS_PROPERTY_NAME = "status";
        internal const string BIRTHDAY_PROPERTY_NAME = "birthday";
        internal const string JOINED_PROPERTY_NAME = "inscription_date";
        internal const string COUNTRY_PROPERTY_NAME = "country";
        internal const string LANGUAGE_PROPERTY_NAME = "language";
        internal const string LINK_PROPERTY_NAME = "link";
        internal const string CHILD_ACCOUNT_PROPERTY_NAME = "is_kid";

        //TODO: Explicit...

        public static IUser FromJson(JToken json)
        {
            string birthdayStr = json.Value<string>(BIRTHDAY_PROPERTY_NAME);
            DateTime? birthday = DateTimeExtensions.ParseApiDateTime(birthdayStr);

            string joinedStr = json.Value<string>(JOINED_PROPERTY_NAME);
            DateTime? joined = DateTimeExtensions.ParseApiDateTime(joinedStr);

            return new User()
            {
                Id = json.Value<ulong>(ID_PROPERTY_NAME),

                Username = json.Value<string>(USERNAME_PROPERTY_NAME),
                Firstname = json.Value<string>(FIRSTNAME_PROPERTY_NAME),
                Lastname = json.Value<string>(LASTNAME_PROPERTY_NAME),
                Email = json.Value<string>(EMAIL_PROPERTY_NAME),

                Status = json.Value<int>(STATUS_PROPERTY_NAME),

                Gender = json.Value<string>(GENDER_PROPERTY_NAME),

                Birthday = birthday,
                InscriptionDate = joined,

                Country = json.Value<string>(COUNTRY_PROPERTY_NAME),
                Language = json.Value<string>(LANGUAGE_PROPERTY_NAME),

                ProfilePictures = Api.Images.FromJson(json),

                Link = json.Value<string>(LINK_PROPERTY_NAME),

                IsKid = json.Value<bool>(CHILD_ACCOUNT_PROPERTY_NAME),
            };
        }
    }



    public interface IUserV2
    {
        ulong Id { get; }
        string Username { get; }
        string Firstname { get; }
        string Lastname { get; }
        string EmailAddress { get; }
        
        DateTime? Birthday { get; }
        DateTime? Inscription { get; }

        string Link { get; }

        IImages ProfilePicture { get; }

        string Country { get; }
        string Language { get; }


        Task<IEnumerable<ITrack>> Flow(CancellationToken cancellationToken, uint start = 0, uint count = 50);

        Task<IEnumerable<IUserProfile>> Followers(CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IUserProfile>> Followings(CancellationToken cancellationToken, uint start = 0, uint count = 25);

        Task<IEnumerable<ITrack>> ListeningHistory(CancellationToken cancellationToken, uint start = 0, uint count = 25);

        Task<IEnumerable<IAlbum>> FavouriteAlbums(CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IArtist>> FavouriteArtists(CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IPlaylist>> FavouritePlaylists(CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<ITrack>> FavouriteTracks(CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IRadio>> FavouriteRadio(CancellationToken cancellationToken, uint start = 0, uint count = 25);

        Task<IEnumerable<IAlbum>> RecommendedAlbums(CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IArtist>> RecommendedArtists(CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IPlaylist>> RecommendedPlaylists(CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<ITrack>> RecommendedTracks(CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IRadio>> RecommendedRadio(CancellationToken cancellationToken, uint start = 0, uint count = 25);


        Task<IEnumerable<IPlaylist>> Playlists(CancellationToken cancellationToken, uint start = 0, uint count = 25);

        Task<ulong> CreatePlaylist(string playlistName, CancellationToken cancellationToken);
    }


    internal class UserV2 : IUserV2, IClientObject
    {

        public ulong Id { get; private set; }
        public string Username { get; private set; }
        public string Firstname { get; private set; }
        public string Lastname { get; private set; }
        public string EmailAddress { get; private set; }

        public DateTime? Birthday { get; private set; }
        public DateTime? Inscription { get; private set; }

        public string Link { get; private set; }

        public IImages ProfilePicture { get; private set; }

        public string Country { get; private set; }
        public string Language { get; private set; }


        // IClientObject
        public IDeezerClient Client { get; private set; }


        // Methods
        public Task<IEnumerable<ITrack>> Flow(CancellationToken cancellationToken, uint start = 0, uint count = 50)
            => this.Client.Endpoints.User.GetFlow(this.Id, cancellationToken, start, count);


        public Task<IEnumerable<IUserProfile>> Followers(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.User.GetFollowers(this.Id, cancellationToken, start, count);

        public Task<IEnumerable<IUserProfile>> Followings(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.User.GetFollowings(this.Id, cancellationToken, start, count);


        public Task<IEnumerable<ITrack>> ListeningHistory(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.User.GetListeningHistory(cancellationToken, start, count);


        public Task<IEnumerable<IAlbum>> FavouriteAlbums(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.User.GetFavouriteAlbums(this.Id, cancellationToken, start, count);

        public Task<IEnumerable<IArtist>> FavouriteArtists(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.User.GetFavouriteArtists(this.Id, cancellationToken, start, count);

        public Task<IEnumerable<IPlaylist>> FavouritePlaylists(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.User.GetFavouritePlaylists(this.Id, cancellationToken, start, count);

        public Task<IEnumerable<ITrack>> FavouriteTracks(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.User.GetFavouriteTracks(this.Id, cancellationToken, start, count);

        public Task<IEnumerable<IRadio>> FavouriteRadio(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.User.GetFavouriteRadio(this.Id, cancellationToken, start, count);


        public Task<IEnumerable<IAlbum>> RecommendedAlbums(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.User.GetRecommendedAlbums(cancellationToken, start, count);

        public Task<IEnumerable<IArtist>> RecommendedArtists(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.User.GetRecommendedArtists(cancellationToken, start, count);

        public Task<IEnumerable<IPlaylist>> RecommendedPlaylists(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.User.GetRecommendedPlaylists(cancellationToken, start, count);

        public Task<IEnumerable<ITrack>> RecommendedTracks(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.User.GetRecommendedTracks(cancellationToken, start, count);

        public Task<IEnumerable<IRadio>> RecommendedRadio(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.User.GetRecommendedRadio(cancellationToken, start, count);


        public Task<IEnumerable<IPlaylist>> Playlists(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.User.GetPlaylists(cancellationToken, start, count);


        public Task<ulong> CreatePlaylist(string playlistName, CancellationToken cancellationToken)
            => this.Client.Endpoints.Playlists.CreatePlaylist(playlistName, cancellationToken);



        // JSON
        internal const string ID_PROPERTY_NAME = "id";
        internal const string USERNAME_PROPERTY_NAME = "name";
        internal const string FIRSTNAME_PROPERTY_NAME = "firstname";
        internal const string LASTNAME_PROPERTY_NAME = "lastname";
        internal const string EMAIL_PROPERTY_NAME = "email";
        internal const string BIRTHDAY_PROPERTY_NAME = "birthday";
        internal const string INSCRIPTION_PROPERTY_NAME = "inscription";
        internal const string LINK_PROPERTY_NAME = "link";
        internal const string COUNTRY_PROPERTY_NAME = "country";
        internal const string LANGUAGE_PROPERTY_NAME = "lang";

        // TODO: Make this a public available thing? 
        //       OR at least, available to the rest of the library
        internal const string API_DATE_FORMAT = "YYYY-MM-DD";
        internal const string DEFAULT_BIRTHDAY = "0000-00-00";

        // TODO IsKid
        // TODO: Gender. Would likely be parsed into an enum (M, F, Unknown/Not Specified
        // TODO: status. Returns int, but docs don't specify anything about it's values
        // TODO: explicit status ATM. Would need to generate ENUM from returned values to make it easier to user


        public static IUserV2 FromJson(JToken json, IDeezerClient client)
        {
            var birthdayDate = json.Value<string>(BIRTHDAY_PROPERTY_NAME);

            DateTime? birthdayAsDateTime = birthdayDate == DEFAULT_BIRTHDAY ? (DateTime?)null
                                                                            : DateTime.ParseExact(birthdayDate, API_DATE_FORMAT, CultureInfo.InvariantCulture);

            var inscriptionDateStr = json.Value<string>(INSCRIPTION_PROPERTY_NAME);
            DateTime? inscriptionDate = null;

            if (inscriptionDateStr != null)
            {
                inscriptionDate = DateTime.ParseExact(json.Value<string>(INSCRIPTION_PROPERTY_NAME), API_DATE_FORMAT, CultureInfo.InvariantCulture);
            }

            return new UserV2()
            {
                Id = json.Value<ulong>(ID_PROPERTY_NAME),
                Username = json.Value<string>(USERNAME_PROPERTY_NAME),
                Firstname = json.Value<string>(FIRSTNAME_PROPERTY_NAME),
                Lastname = json.Value<string>(LASTNAME_PROPERTY_NAME),
                EmailAddress = json.Value<string>(EMAIL_PROPERTY_NAME),

                Birthday = birthdayAsDateTime,
                Inscription = inscriptionDate,

                ProfilePicture = Images.FromJson(json),

                Country = json.Value<string>(COUNTRY_PROPERTY_NAME),
                Language = json.Value<string>(LANGUAGE_PROPERTY_NAME),

                Link = json.Value<string>(LINK_PROPERTY_NAME),


                Client = client,
            };
        }
    }
}
