using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using RestSharp.Deserializers;

namespace E.Deezer.Api
{
    public interface IRadio : IObjectWithImage
    {
        int Id { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        string ShareLink{ get; set; }

        //Methods
        Task<IEnumerable<ITrack>> GetFirst40Tracks();

        Task<bool> AddRadioToFavorite();
        Task<bool> RemoveRadioFromFavorite();        
    }

    internal class Radio : ObjectWithImage, IRadio, IDeserializable<DeezerClient>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ShareLink { get; set; }

        public DeezerClient Client { get; set; }

        public Task<bool> AddRadioToFavorite() => Client.User.AddRadioToFavorite(Id);
        public Task<bool> RemoveRadioFromFavorite() => Client.User.RemoveRadioFromFavorite(Id);        

        public void Deserialize(DeezerClient aClient) { Client = aClient; }

        public Task<IEnumerable<ITrack>> GetFirst40Tracks()
        {
            if (!Client.HasPermission(DeezerPermissions.BasicAccess)) { throw new DeezerPermissionsException(DeezerPermissions.BasicAccess); }

            IList<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", Id)
            };

            return Client.Get<Track>("radio/{id}/tracks", parms).ContinueWith<IEnumerable<ITrack>>((aTask) =>
            {
                return Client.Transform<Track, ITrack>(aTask.Result);
            }, Client.CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default); 
        }

        public override string ToString()
        {
            return string.Format("E.Deezer: Radio({0} - ({1}))", Title, Description);
        }
    }
}
