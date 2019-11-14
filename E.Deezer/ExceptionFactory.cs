using E.Deezer.Api;
using Newtonsoft.Json.Linq;

namespace E.Deezer
{
    /// <summary>
    /// Class to convert API Exceptions to C# Exceptions if neccessary.
    /// </summary>
    internal class ExceptionFactory
    {
        /// <summary>
        /// Throws an Exception if the server response contains
        /// any kind of Exception.
        /// </summary>
        /// <param name="reader">Container of the HTTP response.</param>
        /// <exception cref="DeezerException">Occurs when
        /// the API response is not data but an Exception.</exception>
        internal void ThrowIfNeeded(string json)
        {
            var jToken = JToken.Parse(json);

            if (jToken.Type == JTokenType.Object)
            {
                var value = JObject.FromObject(jToken);

                bool hasError = value.ContainsKey("error");

                if (hasError)
                {
                    JObject errorObject = value.Value<JObject>("error");

                    var error = errorObject.ToObject<Error>();

                    throw new DeezerException(error);
                }
            }
        }
    }
}
