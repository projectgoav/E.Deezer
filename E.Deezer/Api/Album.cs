using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using RestSharp.Deserializers;

namespace E.Deezer.Api
{
    /// <summary>
    /// Represents an Album in the Deezer Library
    /// </summary>
    public interface IAlbum
    {
        /// <summary>
        /// Deezer ID of this Album
        /// </summary>
        uint Id { get; set; }

        /// <summary>
        /// Gets the name of the Album
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// www.deezer.com link to this Album
        /// </summary>
        string Link { get; set; }

        /// <summary>
        /// Gets the link to the artwork for this Album
        /// </summary>
        string Artwork { get; set; }
        string Tracklist { get; set; }
        string ArtistName { get; }

        Task<IEnumerable<ITrack>> GetTracks();

        Task<IArtist> GetArtist();
    }

    internal class Album : IAlbum, IDeserializable<DeezerClientV2>
    {
        public uint Id { get; set; }
        public string Title { get; set; }

        [DeserializeAs(Name="Url")]
        public string Link { get; set; }

        [DeserializeAs(Name="cover")]
        public string Artwork { get; set; }
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
        public DeezerClientV2 Client { get; set; }
        public void Deserialize(DeezerClientV2 aClient) { Client = aClient; }


        public Task<IEnumerable<ITrack>> GetTracks()
        {
            string[] parms = new string[] { "URL", "id", Id.ToString() };

            return Client.Get<Track>("album/{id}/tracks", parms).ContinueWith<IEnumerable<ITrack>>((aTask) =>
            {
                aTask.Result.Items.Deserialize(Client);
                return aTask.Result.Items;
            }, TaskContinuationOptions.OnlyOnRanToCompletion);  
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
