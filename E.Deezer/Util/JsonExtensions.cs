using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using E.Deezer.Api;

namespace E.Deezer.Util
{
    internal static class JsonExtensions
    {
        public static JObject JObjectFromStream(Stream stream)
        {
            using (stream)
            using (var streamReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                return JObject.Load(jsonReader);
            }
        }


        public static (IError error,
                       TResult result) DeserializeErrorOr<TResult>(this JObject json,
                                                                   Func<JObject, TResult> objectDeserializationFunc)
        {
            var error = Error.FromJson(json);
            if (error != null)
            {
                return (error, default(TResult));
            }

            return (null, objectDeserializationFunc(json));
        }




        public static TResult ValueOrDefault<TResult>(this JToken token, string key, TResult defaultValue)
        {
            try
            {
                return token.Value<TResult>(key);
            }
            catch (Exception ex)
            {
                //TODO: Log?
                return defaultValue;
            }
        }

    }
}
