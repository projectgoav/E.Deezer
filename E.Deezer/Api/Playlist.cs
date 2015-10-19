using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RestSharp.Deserializers;

namespace E.Deezer.Api
{
    public interface IPlaylist
    {
        uint Id { get; set; }
        string Title { get; set; }
        bool Public { get; set; }
        uint NumTracks { get; set; }
        string Link { get; set; }
        string Picture { get; set; }
        string Tracklist { get; set; }
        string CreatorName { get; }

        ISearchResult<ITrack> GetTracks();

    }

    internal class Playlist : IPlaylist
    {
        public uint Id { get; set; }
        public string Title { get; set; }
        public bool Public { get; set; }

        [DeserializeAs(Name = "nb_tracks")]
        public uint NumTracks { get; set; }

        public string Link { get; set; }
        public string Picture { get; set; }
        public string Tracklist { get; set; }
        public string CreatorName
        {
            //Required as sometime playlist creator is references as Creator and sometimes references as User
            get
            {
                if (UserInternal == null && CreatorInternal == null) { return string.Empty; }
                return (UserInternal == null) ? CreatorInternal.Name : UserInternal.Name;
            }
        }

        [DeserializeAs(Name = "user")]
        public User UserInternal { get; set; }

        [DeserializeAs(Name = "creator")]
        public User CreatorInternal { get; set; }

        public ISearchResult<ITrack> GetTracks()
        {
            return null;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Title, CreatorName);
        }
    }
}
