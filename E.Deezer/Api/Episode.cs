using E.Deezer.Util;
using Newtonsoft.Json.Linq;
using System;

namespace E.Deezer.Api
{
    public interface IEpisode
    {
        ulong Id { get; }
        string Title { get; }
        DateTime? ReleaseDate { get; }
        int Duration { get;}
        bool Available { get; }
        string Link { get; }
        string ShareLink { get; }
        string Picture { get; }
        string Description { get; }
    }

    internal class Episode : IEpisode, IClientObject
    {
        public ulong Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime? ReleaseDate { get; private set; }
        public int Duration { get; private set; }
        public bool Available { get; private set; }
        public string Picture { get; private set; }
        public string Link { get; private set; }
        public string ShareLink { get; private set; }

        // IClientObject
        public IDeezerClient Client { get; private set; }

        public override string ToString()
        {
            return string.Format("E.Deezer: Episode({0} - ({1}))", Title, Id);
        }

        // JSON
        internal const string ID_PROPERTY_NAME = "id";
        internal const string TITLE_PROPERTY_NAME = "title";
        internal const string DESCRIPTION_PROPERTY_NAME = "description";
        internal const string SHARE_LINK_PROPERTY_NAME = "share";
        internal const string AVAILABLE_PROPERTY_NAME = "available";
        internal const string LINK_PROPERTY_NAME = "link";
        internal const string DURATION_PROPERTY_NAME = "duration";
        internal const string RELEASE_DATE_PROPERTY_NAME = "release_date";

        internal static IEpisode FromJson(JToken json, IDeezerClient client)
        {
            return new Episode
            {
                Id = json.Value<ulong>(ID_PROPERTY_NAME),
                Title = json.Value<string>(TITLE_PROPERTY_NAME),
                Description = json.Value<string>(DESCRIPTION_PROPERTY_NAME),
                ShareLink = json.Value<string>(SHARE_LINK_PROPERTY_NAME),
                Link = json.Value<string>(LINK_PROPERTY_NAME),
                Available = json.Value<bool>(AVAILABLE_PROPERTY_NAME),
                Duration = json.Value<int>(DURATION_PROPERTY_NAME),
                ReleaseDate = json.ParseApiDateTime(RELEASE_DATE_PROPERTY_NAME),

                Client = client,
            };
        }
    }
}
