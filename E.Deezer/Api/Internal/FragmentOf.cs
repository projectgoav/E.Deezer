using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json.Linq;

namespace E.Deezer.Api.Internal
{
    /* Internal class to support deserializing a 'DeezerFragment'
     * 
     * Fragment responses come in the form:
     * {
     *     'data' : [
     *         ...
     *      ]
     *  }
     *  
     *  Callers will pass in a factory function for each item in 
     *  the fragment. */
    internal static class FragmentOf<TItem>
    {
        internal const string DATA_PROPERTY_NAME = "data";


        public static IEnumerable<TItem> FromJson(JToken json,
                                                  Func<JToken, TItem> itemFactoryFunc)
        {
            if (json == null)
            {
                return Array.Empty<TItem>();
            }

            var fragmentContents = json[DATA_PROPERTY_NAME];

            if (fragmentContents == null)
            {
                // Nothing to parse
                return new TItem[0];
            }

            return CollectionOf<TItem>.FromJson(fragmentContents,
                                                itemFactoryFunc);
        }
    }
}
