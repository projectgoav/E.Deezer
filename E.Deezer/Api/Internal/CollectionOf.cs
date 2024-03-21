using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json.Linq;

using E.Deezer.Util;

namespace E.Deezer.Api.Internal
{
    /* Helper class to deserialize a JSON array of a given type.
     * Given a JToken or type ARRAY, will look contents to return
     * a collection of items. */
    internal static class CollectionOf<TItem>
    {

        public static IEnumerable<TItem> FromJson(JToken json,
                                                  Func<JToken, TItem> itemFactoryFunc)
        {
            if (json == null)
                return Array.Empty<TItem>();

            if (json.Type != JTokenType.Array)
                throw new InvalidOperationException($"Attempting to deserialize a json object of type '{json.Type}' as an array. This can't be done.");


            var jsonArray = json as JArray;

            int numItems = jsonArray.Count;
            var resultingContents = new List<TItem>(jsonArray.Count);

            for (int i = 0; i < numItems; ++i)
            {
                resultingContents.Add(itemFactoryFunc(jsonArray[i]));
            }

            return resultingContents;
        }

    }
}
