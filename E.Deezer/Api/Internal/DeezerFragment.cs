using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace E.Deezer.Api
{    
    //Retrun value of all Deezer API calls
    internal class DeezerFragment<T> : IHasError
    {
        [JsonProperty(PropertyName="data")]
        public List<T> Items { get; set; }

        public uint Total { get; set; }

        //IHasError
        public Error Error { get; set; }

        public IError TheError => Error;
    }



    //ChartFragment is used to get all 4 charts at once
    internal class DeezerChartFragment : IHasError
    {
        public DeezerFragment<Track> Tracks { get; set; }

        public DeezerFragment<Album> Albums { get; set; }

        public DeezerFragment<Artist> Artists { get; set; }

        public DeezerFragment<Playlist> Playlists { get; set; }

        //IHasError
        public Error Error { get; set; }

        public IError TheError => Error;
    }
}
