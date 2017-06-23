using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RestSharp.Deserializers;

namespace E.Deezer.Api
{
    //Retrun value of all Deezer API calls
    internal class DeezerCreateResponse
    {
        [DeserializeAs(Name = "id")]
        public uint Id { get; set; }
    }
}
