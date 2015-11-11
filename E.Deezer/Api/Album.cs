using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

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

        /// <summary>
        /// Gets the album tracklist
        /// </summary>
        /// <returns>First page of album tracks</returns>
        Task<IPage<ITrack>> GetTracks();

        /// <summary>
        /// Gets album artist
        /// </summary>
        /// <returns>Album Artist</returns>
        Task<IArtist> GetArtist();
    }

    internal class Album : IAlbum, IDeserializable<DeezerClient>
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
        public DeezerClient Client { get; set; }
        public void Deserialize(DeezerClient aClient) { Client = aClient; }


        public Task<IPage<ITrack>> GetTracks()
        {
            return Client.GetAlbumTracks(Id);
        }

        public Task<IArtist> GetArtist()
        {
            return Task.Factory.StartNew<IArtist>(() => ArtistInternal);
        }


        public override string ToString()
        {
            return string.Format("E.Deezer: Album({0})", Title);
        }
    }
}
