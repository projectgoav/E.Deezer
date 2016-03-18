using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using RestSharp.Deserializers;

namespace E.Deezer.Api
{
    public interface IAlbum : IObjectWithImage
    {
        uint Id { get; set; }
        string Title { get; set; }
        string Link { get; set; }
        string ArtistName { get; }
        int Rating { get; }
        IArtist Artist { get; }

        //Methods
        Task<IEnumerable<ITrack>> GetTracks();
        Task<bool> Rate(int aRating);
    }

    internal class Album : ObjectWithImage, IAlbum, IDeserializable<DeezerClient>
    {
        public uint Id { get; set; }
        public string Title { get; set; }
        public int Rating { get; set; }
        public IArtist Artist { get { return ArtistInternal; } }

        [DeserializeAs(Name = "artist")]
        public Artist ArtistInternal { get; set; }

        [DeserializeAs(Name = "Url")]
        public string Link { get; set; }

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


        public Task<IEnumerable<ITrack>> GetTracks()
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", Id)
            };

            return Client.Get<Track>("album/{id}/tracks", parms).ContinueWith<IEnumerable<ITrack>>((aTask) =>
            {
                return Client.Transform<Track, ITrack>(aTask.Result);
            }, Client.CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);  
        }

        public Task<bool> Rate(int aRating)
        {
            if (aRating < 1 || aRating > 5) { throw new ArgumentOutOfRangeException("aRating", "Rating value should be between 1 and 5 (inclusive)"); }

            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", Id),
                RequestParameter.GetNewQueryStringParameter("note", aRating)
            };

            return Client.Post("album/{id}", parms, DeezerPermissions.BasicAccess);
        }


        public override string ToString()
        {
            return string.Format("E.Deezer: Album({0})", Title);
        }
    }
}
