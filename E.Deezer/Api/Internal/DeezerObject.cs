using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E.Deezer.Api
{
    internal class DeezerObject<T> : IHasError
    {
        public T Data { get; set; }
        private Error Error { get; set; }

        public IError TheError => Error;
    }

}
