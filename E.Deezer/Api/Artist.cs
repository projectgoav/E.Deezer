using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E.Deezer.Api
{
    public interface IArtist
    {
        uint Id { get; set; }
        string Name { get; set; }
        string Url { get; set; }
        string Picture { get; set; }
        string Tracklist { get; set; }

        ISearchResult<ITrack> GetTopTracks();
        ISearchResult<IAlbum> GetAlbums();
        ISearchResult<IArtist> GetRelated();
    }

    public class Artist : IArtist
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Picture { get; set; }
        public string Tracklist { get; set; }

        public ISearchResult<ITrack> GetTopTracks()
        {
            return null;
        }

        public ISearchResult<IAlbum> GetAlbums()
        {
            return null;
        }

        public ISearchResult<IArtist> GetRelated()
        {
            return null;
        }


        public override string ToString()
        {
            return Name;
        }
    }
}
