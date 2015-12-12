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
        string Link { get; set; }
        string ArtistName { get; }
        IArtist Artist { get; }

        //Methods
        Task<IEnumerable<ITrack>> GetTracks();

        string GetCover(PictureSize aSize);
        bool HasCover(PictureSize aSize);
    }

    internal class Album : IAlbum, IDeserializable<DeezerClient>
    {
        public uint Id { get; set; }
        public string Title { get; set; }
        public IArtist Artist { get { return ArtistInternal; } }

        [DeserializeAs(Name = "artist")]
        public Artist ArtistInternal { get; set; }

        [DeserializeAs(Name = "Url")]
        public string Link { get; set; }

        //Pictures
        [DeserializeAs(Name = "cover_small")]
        private string SMPicture { get; set; }

        [DeserializeAs(Name = "cover_medium")]
        private string MDPicture { get; set; }

        [DeserializeAs(Name = "cover_big")]
        private string BGPicture { get; set; }


        public string ArtistName
        {
            get
            {
                if (ArtistInternal == null) { return string.Empty; }
                else { return ArtistInternal.Name; }
            }
        }

        public string GetCover(PictureSize aSize)
        {
            switch (aSize)
            {
                case PictureSize.SMALL: { return string.IsNullOrEmpty(SMPicture) ? string.Empty : SMPicture; }
                case PictureSize.MEDIUM: { return string.IsNullOrEmpty(MDPicture) ? string.Empty : MDPicture; }
                case PictureSize.LARGE: { return string.IsNullOrEmpty(BGPicture) ? string.Empty : BGPicture; }
                default: { return string.Empty; }
            }
        }

        public bool HasCover(PictureSize aSize)
        {
            switch (aSize)
            {
                case PictureSize.SMALL: { return string.IsNullOrEmpty(SMPicture); }
                case PictureSize.MEDIUM: { return string.IsNullOrEmpty(MDPicture); }
                case PictureSize.LARGE: { return string.IsNullOrEmpty(BGPicture); }
                default: { return false; }
            }
        }

        //Local Serailization info
        public DeezerClient Client { get; set; }
        public void Deserialize(DeezerClient aClient) { Client = aClient; }


        public Task<IEnumerable<ITrack>> GetTracks()
        {
            string[] parms = new string[] { "URL", "id", Id.ToString() };

            return Client.Get<Track>("album/{id}/tracks", parms).ContinueWith<IEnumerable<ITrack>>((aTask) =>
            {
                aTask.Result.Items.Deserialize(Client);
                return aTask.Result.Items;
            }, TaskContinuationOptions.OnlyOnRanToCompletion);  
        }



        public override string ToString()
        {
            return string.Format("E.Deezer: Album({0})", Title);
        }
    }
}
