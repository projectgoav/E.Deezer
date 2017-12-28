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
        ulong Id { get;  }
        string Title { get;  }
        string Description { get;  }
        string ShareLink{ get;  }

        //Legacy
        Task<IEnumerable<ITrack>> GetFirst40Tracks();

        //Methods
        Task<IEnumerable<ITrack>> GetTracks(uint aStart = 0, uint aCount = 100);

        Task<bool> AddRadioToFavorite();
        Task<bool> RemoveRadioFromFavorite();        
    }

    internal class Radio : ObjectWithImage, IRadio, IDeserializable<IDeezerClient>
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
        public IDeezerClient Client
        {
            get;
            set;
        }

        public void Deserialize(IDeezerClient aClient) 
            => Client = aClient;

        public Task<IEnumerable<ITrack>> GetFirst40Tracks() 
            => GetTracks(0, 40);

        public Task<IEnumerable<ITrack>> GetTracks(uint aStart = 0, uint aCount = 100)
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

        public Task<bool> AddRadioToFavorite()
            => Client.User.AddRadioToFavourite(Id);

        public Task<bool> RemoveRadioFromFavorite()
            => Client.User.RemoveRadioFromFavourite(Id);


        public override string ToString()
        {
            return string.Format("E.Deezer: Radio({0} - ({1}))", Title, Description);
        }

    }
}
