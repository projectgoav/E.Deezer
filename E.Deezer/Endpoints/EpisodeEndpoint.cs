using E.Deezer.Api;
using System.Threading;
using System.Threading.Tasks;

namespace E.Deezer.Endpoints
{
    public interface IEpisodeEndpoint
    {
        Task<IEpisode> GetById(ulong episodeId, CancellationToken cancellationToken);
    }

    internal class EpisodeEndpoint : IEpisodeEndpoint
    {
        private readonly IDeezerClient client;

        public EpisodeEndpoint(IDeezerClient client)
        {
            this.client = client;
        }


        public Task<IEpisode> GetById(ulong episodeId, CancellationToken cancellationToken)
            => this.client.Get($"episode/{episodeId}",
                               cancellationToken,
                               json => Api.Episode.FromJson(json, this.client));
    }
}
