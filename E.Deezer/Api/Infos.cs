using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RestSharp.Deserializers;

namespace E.Deezer.Api
{
    public interface IInfos
    {
        string Country { get; set; }
        string Iso { get; set; }
        bool IsAvailable { get; set; }
    }

    internal class Infos : IInfos
    {
        public string Country { get; set; }

        [DeserializeAs(Name ="country_iso")]
        public string Iso { get; set; }

        [DeserializeAs(Name = "open")]
        public bool IsAvailable { get; set; }
    }
}
