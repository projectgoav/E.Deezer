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
        ulong Id { get; set; }
        uint Tracks {get; set; }
        string Title { get; set; }
        string Link { get; set; }
        string ArtistName { get; }
        long Rating { get; }
        DateTime ReleaseDate {get; set; }
        IArtist Artist { get; }

        //Methods
        Task<IEnumerable<ITrack>> GetTracks();
        Task<bool> Rate(int aRating);

        string GetCover(PictureSize aSize);
        bool HasCover(PictureSize aSize);
    }

    internal class Album : ObjectWithImage, IAlbum, IDeserializable<DeezerClient>
    {
        public ulong Id { get; set; }
        public string Title { get; set; }
        public long Rating { get; set; }
        public DateTime ReleaseDate {get; set; }
        public IArtist Artist { get { return ArtistInternal; } }

        [DeserializeAs(Name = "artist")]
        public Artist ArtistInternal { get; set; }

        [DeserializeAs(Name = "Url")]
        public string Link { get; set; }

        [DeserializeAs(Name = "nb_tracks")]
        public uint Tracks {get; set; }

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
        public void Deserialize(DeezerClient aClient)
        {
            Client = aClient;

            if (ArtistInternal != null)
            {
                ArtistInternal.Deserialize(aClient);
            }
        }

        [Obsolete("Please use GetPicture instead.", true)]
        public string GetCover(PictureSize aSize) { return GetPicture(aSize); }

        [Obsolete("Please use HasPicture instead.", true)]
        public bool HasCover(PictureSize aSize) { return HasPicture(aSize); }


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
