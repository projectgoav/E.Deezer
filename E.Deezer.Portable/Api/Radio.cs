using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using Newtonsoft.Json;

namespace E.Deezer.Api
{
    public interface IRadio : IObjectWithImage
    {
        ulong Id { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        string ShareLink{ get; set; }

        //Legacy
        Task<IEnumerable<ITack>> GetFirst40Tracks();

        //Methods
        Task<IEnumerable<ITrack>> GetTracks();
        Task<IEnumerable<ITrack>> GetTracks(uint aCount);
        Task<IEnumerable<ITrack>> GetTracks(uint aStart, uint aCount);

        Task<bool> AddRadioToFavorite();
        Task<bool> RemoveRadioFromFavorite();        
    }

    internal class Radio : ObjectWithImage, IRadio, IDeserializable<DeezerClient>
    {
        public ulong Id
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string ShareLink
        {
            get;
            set;
        }


        //IDeserializable
        public DeezerClient Client
        {
            get;
            set;
        }

        public void Deserialize(DeezerClient aClient) => Client = aClient;

        public Task<IEnumerable<ITrack>> GetFirst40Tracks() => GetTracks(0, 40);

        public Task<IEnumerable<ITrack>> GetTracks() => GetTracks(0, Client.ResultSize);

        public Task<IEnumerable<ITrack>> GetTracks(uint aCount) => GetTracks(0, aCount);

        public Task<IEnumerable<ITrack>> GetTracks(uint aStart, uint aCount)
        {
            if (!Client.HasPermission(DeezerPermissions.BasicAccess))
            {
                throw new DeezerPermissionsException(DeezerPermissions.BasicAccess);
            }

            IList<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", Id)
            };

            return Client.Get<Track>("radio/{id}/tracks", parms, aStart, aCount)
                         .ContinueWith<IEnumerable<ITrack>>((aTask) =>
                            {
                                return Client.Transform<Track, ITrack>(aTask.Result);
                            }, Client.CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default); 
        }


        public override string ToString()
        {
            return string.Format("E.Deezer: Radio({0} - ({1}))", Title, Description);
        }


        public Task<bool> AddRadioToFavorite() => Client.User.AddRadioToFavourite(Id);

        public Task<bool> RemoveRadioFromFavorite() => Client.User.RemoveRadioFromFavourite(Id);

    }
}
