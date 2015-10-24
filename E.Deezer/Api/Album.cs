using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RestSharp.Deserializers;

namespace E.Deezer.Api
{
    /// <summary>
    /// A Deezer Album object
    /// </summary>
    public interface IAlbum
    {
        /// <summary>
        /// Deezer library ID mumber
        /// </summary>
        uint Id { get; set; }
        
        /// <summary>
        /// Album Title
        /// </summary>
        string Title { get; set; }
        
        /// <summary>
        /// Deezer.com link to album
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// Link to album cover
        /// </summary>
        string Cover { get; set; }
        
        /// <summary>
        /// Link to album tracklist
        /// </summary>
        string Tracklist { get; set; }

        /// <summary>
        /// Album artist
        /// </summary>
        string ArtistName { get; }

        IPagedResponse<ITrack> GetTracks();
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


        //Local Serailization info
        private DeezerClient Client { get; set; }
        internal void Deserialize(DeezerClient aClient) { Client = aClient; }

        public IPagedResponse<ITrack> GetTracks()
        {
            return null;
        }
    }
}
