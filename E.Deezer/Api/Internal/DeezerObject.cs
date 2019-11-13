using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
#if NETSTANDARD11
using System.Linq;
using System.Reflection;
#endif

namespace E.Deezer.Api
{
    internal interface IDeezerObjectResponse
    {
        IError Error { get; }
    }

    internal interface IDeezerObjectResponse<TObject> : IDeezerObjectResponse
    {
        TObject Object { get; }
    }

    internal class AlbumObjectResponse : IDeezerObjectResponse<Album>
    {
        public Album Object{ get; private set; }
        public IError Error { get; private set; }

        public static AlbumObjectResponse CreateFrom(Album album, IError error)
        {
            var resp = new AlbumObjectResponse
            {
                Object = album,
                Error = error
            };

            return resp;
        }
    }

    internal class PlaylistObjectResponse : IDeezerObjectResponse<Playlist>
    {
        public Playlist Object{ get; private set; }
        public IError Error { get; private set; }

        public static PlaylistObjectResponse CreateFrom(Playlist playlist, IError error)
        {
            var resp = new PlaylistObjectResponse();

            resp.Object = playlist;
            resp.Error = error;

            return resp;
        }
    }

    internal class ArtistObjectResponse : IDeezerObjectResponse<Artist>
    {
        public Artist Object{ get; private set; }
        public IError Error { get; private set; }

        public static ArtistObjectResponse CreateFrom(Artist artist, IError error)
        {
            var resp = new ArtistObjectResponse();

            resp.Object = artist;
            resp.Error = error;

            return resp;
        }
    }

    internal class TrackObjectResponse : IDeezerObjectResponse<Track>
    {
        public Track Object{ get; private set; }
        public IError Error { get; private set; }

        public static TrackObjectResponse CreateFrom(Track track, IError error)
        {
            var resp = new TrackObjectResponse();

            resp.Object = track;
            resp.Error = error;

            return resp;
        }
    }

    internal class UserObjectResponse : IDeezerObjectResponse<User>
    {
        public User Object{ get; private set; }
        public IError Error { get; private set; }

        public static UserObjectResponse CreateFrom(User user, IError error)
        {
            var resp = new UserObjectResponse();

            resp.Object = user;
            resp.Error = error;

            return resp;
        }
    }

    internal class UserProfileObjectResponse : IDeezerObjectResponse<UserProfile>
    {
        public UserProfile Object{ get; private set; }
        public IError Error { get; private set; }

        public static UserProfileObjectResponse CreateFrom(UserProfile user, IError error)
        {
            var resp = new UserProfileObjectResponse
            {
                Object = user,
                Error = error
            };

            return resp;
        }
    }

    internal class RadioObjectResponse : IDeezerObjectResponse<Radio>
    {
        public Radio Object{ get; private set; }
        public IError Error { get; private set; }

        public static RadioObjectResponse CreateFrom(Radio radio, IError error)
        {
            var resp = new RadioObjectResponse();

            resp.Object = radio;
            resp.Error = error;

            return resp;
        }
    }

    internal class DeezerObjectResponseJsonDeserializer : JsonConverter
    {
        private static readonly Type _typeToConvert = typeof(IDeezerObjectResponse);

        public override bool CanRead => true;

        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
#if NETSTANDARD11
            return objectType.GetTypeInfo()
                             .ImplementedInterfaces
                             .Contains(_typeToConvert);
#else
            return _typeToConvert.IsAssignableFrom(objectType);
#endif
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject value = JObject.Load(reader);

            bool hasError = value.ContainsKey("error");

            if (hasError)
            {
                JObject errorObject = value.Value<JObject>("error");

                Error error = errorObject.ToObject<Error>();

                return CreateErrorResponse(objectType, error);
            }

            return CreateObjectResponse(objectType, value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private object CreateErrorResponse(Type objectType, Error error)
        {
            if (objectType == typeof(AlbumObjectResponse))
            {
                return AlbumObjectResponse.CreateFrom(null, error);
            }
            else if (objectType == typeof(ArtistObjectResponse))
            {
                return ArtistObjectResponse.CreateFrom(null, error);
            }
            else if (objectType == typeof(PlaylistObjectResponse))
            {
                return PlaylistObjectResponse.CreateFrom(null, error);
            }
            else if (objectType == typeof(RadioObjectResponse))
            {
                return RadioObjectResponse.CreateFrom(null, error);
            }
            else if (objectType == typeof(TrackObjectResponse))
            {
                return TrackObjectResponse.CreateFrom(null, error);
            }
            else if (objectType == typeof(UserObjectResponse))
            {
                return UserObjectResponse.CreateFrom(null, error);
            }
            else if (objectType == typeof(UserProfileObjectResponse))
            {
                return UserProfileObjectResponse.CreateFrom(null, error);
            }

            throw new Exception("Attempting to deserialize a response type unknown to E.Deezer.");
        }

        private object CreateObjectResponse(Type objectType, JObject valueObject)
        {
            if (objectType == typeof(AlbumObjectResponse))
            {
                var album = valueObject.ToObject<Album>();
                return AlbumObjectResponse.CreateFrom(album, null);
            }
            else if (objectType == typeof(ArtistObjectResponse))
            {
                var artist = valueObject.ToObject<Artist>();
                return ArtistObjectResponse.CreateFrom(artist, null);
            }
            else if (objectType == typeof(PlaylistObjectResponse))
            {
                var playlist = valueObject.ToObject<Playlist>();
                return PlaylistObjectResponse.CreateFrom(playlist, null);
            }
            else if (objectType == typeof(RadioObjectResponse))
            {
                var radio = valueObject.ToObject<Radio>();
                return RadioObjectResponse.CreateFrom(radio, null);
            }
            else if (objectType == typeof(TrackObjectResponse))
            {
                var track = valueObject.ToObject<Track>();
                return TrackObjectResponse.CreateFrom(track, null);
            }
            else if (objectType == typeof(UserObjectResponse))
            {
                var user = valueObject.ToObject<User>();
                return UserObjectResponse.CreateFrom(user, null);
            }
            else if (objectType == typeof(UserProfileObjectResponse))
            {
                var userProfile = valueObject.ToObject<UserProfile>();
                return UserProfileObjectResponse.CreateFrom(userProfile, null);
            }

            throw new Exception("Attempting to deserialize a response type unknown to E.Deezer.");
        }
    }
}
