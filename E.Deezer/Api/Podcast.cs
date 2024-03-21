using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace E.Deezer.Api
{
    public interface IPodcast
    {
        ulong Id { get; }
        string Title { get; }
        string Description { get; }
        bool Available { get; }
        int Fans { get; }
        string Link { get; }
        string ShareLink { get; }

        Task<IEnumerable<IEpisode>> GetEpisodes(CancellationToken cancellationToken);
    }

    internal class Podcast : IPodcast, IClientObject
    {
        public ulong Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public bool Available { get; private set; }
        public int Fans { get; private set; }
        public string Link { get; private set; }
        public string ShareLink { get; private set; }

        public IDeezerClient Client { get; private set; }

        public override string ToString()
        {
            return string.Format("E.Deezer: Podcast({0} - ({1}))", Title, Id);
        }

        public Task<IEnumerable<IEpisode>> GetEpisodes(CancellationToken cancellationToken)
            => Client.Endpoints.Podcasts.GetPodcastEpisodes(this, cancellationToken);



        // JSON
        internal const string ID_PROPERTY_NAME = "id";
        internal const string TITLE_PROPERTY_NAME = "title";
        internal const string DESCRIPTION_PROPERTY_NAME = "description";
        internal const string SHARE_LINK_PROPERTY_NAME = "share";
        internal const string AVAILABLE_PROPERTY_NAME = "available";
        internal const string FANS_PROPERTY_NAME = "fans";
        internal const string LINK_PROPERTY_NAME = "link";

        internal static IPodcast FromJson(JToken json, IDeezerClient client)
        {
            return new Podcast
            {
                Id = json.Value<ulong>(ID_PROPERTY_NAME),
                Title = json.Value<string>(TITLE_PROPERTY_NAME),
                Description = json.Value<string>(DESCRIPTION_PROPERTY_NAME),
                ShareLink = json.Value<string>(SHARE_LINK_PROPERTY_NAME),
                Fans = json.Value<int>(FANS_PROPERTY_NAME),
                Link = json.Value<string>(LINK_PROPERTY_NAME),
                Available = json.Value<bool>(AVAILABLE_PROPERTY_NAME),
                Client = client,
            };
        }
    }
}
