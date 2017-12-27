using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace E.Deezer.Api
{
    //Return value of many of the POST API Calls
    internal class DeezerCreateResponse
    {
        [JsonProperty(PropertyName = "id")]
        public uint Id { get; set; }
    }
}
