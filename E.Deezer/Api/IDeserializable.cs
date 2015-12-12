using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E.Deezer.Api
{
    public interface IDeserializable<T>
    {
        T Client { get; }

        void Deserialize(T aClient);
    }

    public static class DeserializeableExtensions
    {
        // Extension to aid deserialization of lists :)
        public static void Deserialize<T>(this IEnumerable<T> aItems, DeezerClient aClient) where T : IDeserializable<DeezerClient>
        {
            foreach (T item in aItems) { item.Deserialize(aClient); }
        }
    }
}
