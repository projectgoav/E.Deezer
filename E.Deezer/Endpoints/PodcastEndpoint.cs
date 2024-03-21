using E.Deezer.Api;
using E.Deezer.Api.Internal;
using E.Deezer.Util;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace E.Deezer.Endpoints
{
    public interface IPodcastEndpoint
    {
        Task<IPodcast> GetById(ulong podcastId, CancellationToken cancellationToken);
        Task<IEnumerable<IEpisode>> GetPodcastEpisodes(IPodcast podcast, CancellationToken cancellationToken, uint start = 0, uint count = 50);
        Task<IEnumerable<IEpisode>> GetPodcastEpisodes(ulong podcastId, CancellationToken cancellationToken, uint start = 0, uint count = 50);
    }

    internal class PodcastEndpoint : IPodcastEndpoint
    {
        private const string kStartParam = "index";
        private const string kLimitParam = "limit";

        private readonly IDeezerClient client;

        public PodcastEndpoint(IDeezerClient client)
        {
            this.client = client;
        }

        public Task<IPodcast> GetById(ulong podcastId, CancellationToken cancellationToken)
            => this.client.Get($"podcast/{podcastId}",
                               cancellationToken,
                               json => Api.Podcast.FromJson(json, this.client));

        public Task<IEnumerable<IEpisode>> GetPodcastEpisodes(IPodcast podcast, CancellationToken cancellationToken, uint start = 0, uint count = 50)
        {
            podcast.ThrowIfNull();
            return GetPodcastEpisodes(podcast.Id, cancellationToken, start, count);
        }


        public Task<IEnumerable<IEpisode>> GetPodcastEpisodes(ulong podcastId, CancellationToken cancellationToken, uint start = 0, uint count = 50)
            => this.client.Get(
                $"/podcast/{podcastId}/episodes?{kStartParam}={start}&{kLimitParam}={count}",
                cancellationToken,
                json => FragmentOf<IEpisode>.FromJson(json, x => Api.Episode.FromJson(x, this.client)));
    }
}
