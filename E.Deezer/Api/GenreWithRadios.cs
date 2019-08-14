using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace E.Deezer.Api
{
    public class GenreWithRadios : IGenreWithRadios
    {
        [JsonProperty("radios")]
        internal readonly IEnumerable<Radio> InternalRadios;

        public int ID { get; set; }
        public string Title { get; set; }
        public IEnumerable<IRadio> Radios => InternalRadios;
    }

    public interface IGenreWithRadios
    {
        int ID { get; }
        string Title { get; }
        IEnumerable<IRadio> Radios { get; }
    }
}
