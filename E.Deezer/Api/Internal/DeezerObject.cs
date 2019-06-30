using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if NETSTANDARD11
using System.Reflection;
#endif

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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


    internal class AlbumObjectResponse : IDeezerObjectResponse<IAlbum>
    {
        public IAlbum Object { get; private set; }
        public IError Error { get; private set; }

        public static AlbumObjectResponse CreateFrom(IAlbum album, IError error)
        {
            var resp = new AlbumObjectResponse();

            resp.Object = album;
            resp.Error = error;

            return resp;
        }
    }

    internal class PlaylistObjectResponse : IDeezerObjectResponse<IPlaylist>
    {
        public IPlaylist Object { get; private set; }
        public IError Error { get; private set; }

        public static PlaylistObjectResponse CreateFrom(IPlaylist playlist, IError error)
        {
            var resp = new PlaylistObjectResponse();

            resp.Object = playlist;
            resp.Error = error;

            return resp;
        }
    }

    internal class ArtistObjectResponse : IDeezerObjectResponse<IArtist>
    {
        public IArtist Object { get; private set; }
        public IError Error { get; private set; }

        public static ArtistObjectResponse CreateFrom(IArtist artist, IError error)
        {
            var resp = new ArtistObjectResponse();

            resp.Object = artist;
            resp.Error = error;

            return resp;
        }
    }

    internal class TrackObjectResponse : IDeezerObjectResponse<ITrack>
    {
        public ITrack Object { get; private set; }
        public IError Error { get; private set; }

        public static TrackObjectResponse CreateFrom(ITrack track, IError error)
        {
            var resp = new TrackObjectResponse();

            resp.Object = track;
            resp.Error = error;

            return resp;
        }
    }

    internal class UserObjectResponse : IDeezerObjectResponse<IUser>
    {
        public IUser Object { get; private set; }
        public IError Error { get; private set; }

        public static UserObjectResponse CreateFrom(IUser user, IError error)
        {
            var resp = new UserObjectResponse();

            resp.Object = user;
            resp.Error = error;

            return resp;
        }
    }

    internal class RadioObjectResponse : IDeezerObjectResponse<IRadio>
    {
        public IRadio Object { get; private set; }
        public IError Error { get; private set; }

        public static RadioObjectResponse CreateFrom(IRadio radio, IError error)
        {
            var resp = new RadioObjectResponse();

            resp.Object = radio;
            resp.Error = error;

            return resp;
        }
    }


    internal class DeezerObjectResponseJsonDeserializer : JsonConverter
    {
        private static readonly Type typeToConvert = typeof(IDeezerObjectResponse);

        public override bool CanRead => true;

        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
#if NETSTANDARD11
            return objectType.GetTypeInfo()
                             .ImplementedInterfaces
                             .Contains(typeToConvert);
#else
            return typeToConvert.IsAssignableFrom(objectType);
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

            throw new Exception("Attempting to deserialize a response type unknown to E.Deezer.");
        }
    }
}
