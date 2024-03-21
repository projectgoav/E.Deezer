using E.Deezer.Api;
using System.Threading;
using System.Threading.Tasks;

namespace E.Deezer.Endpoints
{
    public interface ITrackEndpoint
    {
        Task<ITrack> GetById(ulong trackId, CancellationToken cancellationToken);
    }

    internal class TrackEndpoint : ITrackEndpoint
    {
        private readonly IDeezerClient client;

        public TrackEndpoint(IDeezerClient client)
        {
            this.client = client;
        }


        public Task<ITrack> GetById(ulong trackId, CancellationToken cancellationToken)
            => this.client.Get($"track/{trackId}",
                               cancellationToken,
                               json => Api.Track.FromJson(json, this.client));
    }
}
