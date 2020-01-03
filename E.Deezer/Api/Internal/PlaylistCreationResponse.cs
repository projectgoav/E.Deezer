using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json.Linq;

namespace E.Deezer.Api.Internal
{
    internal static class PlaylistCreationResponse
    {
        public const ulong DEFAULT_ID = 0;

        internal const string ID_PROPERTY_NAME = "id";


        public static ulong FromJson(JToken json)
        {
            if (json == null)
                return DEFAULT_ID;

            return json.Value<ulong>(ID_PROPERTY_NAME);
        }

    }
}
