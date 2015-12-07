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
        public static void Deserialize<T>(this IEnumerable<T> aItems, DeezerClientV2 aClient) where T : IDeserializable<DeezerClientV2>
        {
            foreach (T item in aItems) { item.Deserialize(aClient); }
        }
    }
}
