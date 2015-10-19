using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RestSharp.Deserializers;

namespace E.Deezer.Api
{
    public interface IAlbum
    {
        uint Id { get; set; }
        string Title { get; set; }
        string Url { get; set; }
        string Cover { get; set; }
        string Tracklist { get; set; }
        string ArtistName { get; }

        ISearchResult<ITrack> GetTracks();
    }

    internal class Album : IAlbum
    {
        public uint Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Cover { get; set; }
        public string Tracklist { get; set; }

        [DeserializeAs(Name = "artist")]
        public Artist ArtistInternal { get; set; }

        public string ArtistName
        {
            get
            {
                if (ArtistInternal == null) { return string.Empty; }
                else { return ArtistInternal.Name; }
            }
        }


        public ISearchResult<ITrack> GetTracks()
        {
            return null;
        }
    }
}
