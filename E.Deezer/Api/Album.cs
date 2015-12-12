using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

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

        Task<IEnumerable<ITrack>> GetTracks();

        Task<IArtist> GetArtist();
    }

    internal class Album : IAlbum, IDeserializable<DeezerClientV2>
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
