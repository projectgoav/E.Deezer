using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E.Deezer.Api
{
    internal interface IHasError
    {
        IError TheError { get; }
    }

    internal interface IError
    {
        string Message { get; set; }
        uint Code { get; set; }
        string Type { get; set; }
    }

    //Grabs an error, if there was one, from the reply
    internal class Error : IError
    {
        public string Message { get; set; }
        public uint Code { get; set; }
        public string Type { get; set; }
    }
}
