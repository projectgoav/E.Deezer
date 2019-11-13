using System;

namespace E.Deezer.Api
{
    internal interface IDeserializable<T>
    {
        T Client { get; }

        void Deserialize(T aClient);
    }
}
