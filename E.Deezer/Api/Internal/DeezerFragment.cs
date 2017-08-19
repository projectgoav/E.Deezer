using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RestSharp.Deserializers;

namespace E.Deezer.Api
{    
    //Retrun value of all Deezer API calls
    internal class DeezerFragment<T> : IHasError
    {
        [DeserializeAs(Name="data")]
        public List<T> Items { get; set; }
        public uint Total { get; set; }

        //IHasErro
        public Error Error { get; set; }
        public IError TheError => Error;
    }



    //ChartFragment is used to get all 4 charts at once
    internal class DeezerChartFragment : IHasError
    {
        public DeezerFragment<ITrack> Tracks { get; set; }

        public DeezerFragment<IAlbum> Albums { get; set; }

        public DeezerFragment<IArtist> Artists { get; set; }

        public DeezerFragment<IPlaylist> Playlists { get; set; }

        //IHasError
        public Error Error { get; set; }

        public IError TheError => Error;
    }
}
