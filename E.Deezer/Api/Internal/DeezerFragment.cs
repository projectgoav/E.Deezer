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
        public Error Error { get; set; }

        public IError TheError { get { return Error; } }
    }



    //ChartFragment is used to get all 4 charts at once
    internal class DeezerChartFragment
    {
        DeezerFragment<ITrack> Tracks { get; }

        DeezerFragment<IAlbum> Albums { get; }

        DeezerFragment<IArtist> Artists {get; }

        DeezerFragment<IPlaylist> Playlists {get; }
    }
}
