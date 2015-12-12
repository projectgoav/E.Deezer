using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RestSharp.Deserializers;

namespace E.Deezer.Api
{
    /// <summary>
    /// Represents Deezer Service Information for the user's country
    /// </summary>
    public interface IInfos
    {
        /// <summary>
        /// Gets the country name that Deezer thinks the user is from
        /// </summary>
        string Country { get; set; }

        /// <summary>
        /// Gets the country ISO code that Deezer thinks the user is from
        /// <example>GB</example>
        /// </summary>
        string Iso { get; set; }

        /// <summary>
        /// Gets if Deezer is available in the country Deezer thinks the user is from
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
