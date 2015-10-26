using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RestSharp.Deserializers;

namespace E.Deezer.Api
{
    /// <summary>
    /// Deezer service information object
    /// </summary>
    public interface IInfos
    {
        /// <summary>
        /// Country assoicated with user's IP
        /// </summary>
        string Country { get; set; }

        /// <summary>
        /// Country ISO code associated with user's IP
        /// </summary>
        string Iso { get; set; }

        /// <summary>
        /// Deezer's service availablility in given country
        /// </summary>
        bool IsAvailable { get; set; }
    }

    internal class Infos : IInfos
    {
        public string Country { get; set; }

        [DeserializeAs(Name ="country_iso")]
        public string Iso { get; set; }

        [DeserializeAs(Name = "open")]
        public bool IsAvailable { get; set; }


        public override string ToString()
        {
            return string.Format("E.Deezer: Info");
        }
    }
}
