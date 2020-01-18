using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json.Linq;

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
#if NETSTANDARD20
                return Array.Empty<TItem>();
#else
                return new TItem[0];
#endif


            if (json.Type != JTokenType.Array)
            {
                // TODO: Better exception handling here.
                // TODO: Do we even want to throw an exception?
                throw new InvalidOperationException("Something went wrong.");
            }


            var jsonArray = (json as JArray);

            int numItems = jsonArray.Count;
            var resultingContents = new List<TItem>(numItems);

            for (int i = 0; i < numItems; ++i)
            {
                resultingContents.Add(itemFactoryFunc(jsonArray[i]));
            }

            return resultingContents;
        }


    }
}
