using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

using E.Deezer.Api;
using E.Deezer.Util;
using E.Deezer.Api.Internal;

namespace E.Deezer.Endpoints
{
    public interface IRadioEndpoint
    {
        Task<IRadio> GetById(ulong radioId, CancellationToken cancellationToken);

        Task<IEnumerable<IRadio>> ListRadio(CancellationToken cancellationToken);
        Task<IEnumerable<IRadio>> GetTopRadio(CancellationToken cancellationToken);


        //TODO: Group by genre??
        //      Something like the format Dictionary<IGenre, IEnumerable<IRadio>>
        //TODO: /lists endpoint for personalised radio list...

        Task<IEnumerable<ITrack>> GetTracks(IRadio radio, CancellationToken cancellationToken, uint trackCount = 50);
        Task<IEnumerable<ITrack>> GetTracks(ulong radioId, CancellationToken cancellationToken, uint trackCount = 50);
    }

    internal class RadioEndpoint : IRadioEndpoint
    {
        private const string COUNT_PARAM = "limit";

        private readonly IDeezerClient client;

        public RadioEndpoint(IDeezerClient client)
        {
            this.client = client;
        }


        public Task<IRadio> GetById(ulong radioId, CancellationToken cancellationToken)
            => this.client.Get($"radio/{radioId}",
                               cancellationToken,
                               json => Api.Radio.FromJson(json, this.client));


        public Task<IEnumerable<IRadio>> ListRadio(CancellationToken cancellationToken)
            => this.client.Get("radio",
                               cancellationToken,
                               json => FragmentOf<IRadio>.FromJson(json, x => Api.Radio.FromJson(x, this.client)));


        public Task<IEnumerable<IRadio>> GetTopRadio(CancellationToken cancellationToken)
            => this.client.Get("radio/top",
                               cancellationToken,
                               json => FragmentOf<IRadio>.FromJson(json, x => Api.Radio.FromJson(x, this.client)));


        
        public Task<IEnumerable<ITrack>> GetTracks(IRadio radio, CancellationToken cancellationToken, uint trackCount = 50)
        {
            radio.ThrowIfNull();

            return GetTracks(radio.Id, cancellationToken, trackCount);
        }

        public Task<IEnumerable<ITrack>> GetTracks(ulong radioId, CancellationToken cancellationToken, uint trackCount = 50)
            => this.client.Get($"radio/{radioId}/tracks?{COUNT_PARAM}={trackCount}",
                               cancellationToken,
                               json => FragmentOf<ITrack>.FromJson(json, x => Api.Track.FromJson(x, this.client)));

    }
}
