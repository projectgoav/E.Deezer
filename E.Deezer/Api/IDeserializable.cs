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
}
