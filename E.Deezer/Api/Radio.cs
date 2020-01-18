using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace E.Deezer.Api
{
    public interface IRadio
    {
        ulong Id { get; }
        string Title { get; }
        string Description { get; }
        string ShareLink{ get; }
        IImages Images { get; }


        Task<IEnumerable<ITrack>> Tracks(CancellationToken cancellationToken, uint trackCount = 50);


        Task<bool> Favourite(CancellationToken cancellationToken);
        Task<bool> Unfavourite(CancellationToken cancellationToken);
    }


    internal class Radio : IRadio, IClientObject
    {
        public ulong Id { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public string ShareLink { get; private set; }

        public IImages Images { get; private set; }


        public IDeezerClient Client { get; set; }


        public Task<IEnumerable<ITrack>> Tracks(CancellationToken cancellationToken, uint trackCount = 50)
            => this.Client.Endpoints.Radio.GetTracks(this, cancellationToken, trackCount);


        public Task<bool> Favourite(CancellationToken cancellationToken)
            => this.Client.Endpoints.User.FavouriteRadio(this, cancellationToken);

        public Task<bool> Unfavourite(CancellationToken cancellationToken)
            => this.Client.Endpoints.User.UnfavouriteRadio(this, cancellationToken);



        public override string ToString()
        {
            return string.Format("E.Deezer: Radio({0} - ({1}))", Title, Id);
        }


        // JSON
        internal const string ID_PROPERTY_NAME = "id";
        internal const string TITLE_PROPERTY_NAME = "title";
        internal const string DESCRIPTION_PROPERTY_NAME = "description";
        internal const string SHARE_LINK_PROPERTY_NAME = "share";
        
        public static IRadio FromJson(JToken json, IDeezerClient client)
        {
            return new Radio()
            {
                Id = json.Value<ulong>(ID_PROPERTY_NAME),
                Title = json.Value<string>(TITLE_PROPERTY_NAME),
                Description = json.Value<string>(DESCRIPTION_PROPERTY_NAME),
                ShareLink = json.Value<string>(SHARE_LINK_PROPERTY_NAME),

                Images = Api.Images.FromJson(json),

                Client = client,
            };
        }
    }
}
