using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace E.Deezer.Api
{
    public interface IGenre
    {
        ulong Id { get;  }
        string Name { get;  }
        IImages Images { get; }


        Task<IEnumerable<IArtist>> Artists(CancellationToken cancellationToken, uint start = 0, uint count = 25);

        Task<IEnumerable<IRadio>> Radio(CancellationToken cancellationToken, uint start = 0, uint count = 25);


        Task<IChart> Charts(CancellationToken cancellationToken, uint start = 0, uint count = 100);

        Task<IEnumerable<IAlbum>> AlbumChart(CancellationToken cancellatiomToken, uint start = 0, uint count = 100);
        Task<IEnumerable<IArtist>> ArtistChart(CancellationToken cancellationToken, uint start = 0, uint count = 100);
        Task<IEnumerable<IPlaylist>> PlaylistChart(CancellationToken cancellationToken, uint start = 0, uint count = 100);
        Task<IEnumerable<ITrack>> TrackChart(CancellationToken cancellationToken, uint start = 0, uint count = 0);

        Task<IEnumerable<IAlbum>> NewReleases(CancellationToken cancellationToken, uint start = 0, uint count = 0);
        Task<IEnumerable<IAlbum>> DeezerSelection(CancellationToken cancellationToken, uint start = 0, uint count = 0);

        //TODO
        //Task<IBook<IPodcast>> GetPodcasts();
    }

    internal class Genre : IGenre, IClientObject
    {
        public ulong Id { get; private set; }

        public string Name { get; private set; }

        public IImages Images { get; private set; }


        // IClientObject
        public IDeezerClient Client { get; private set; }


        public Task<IEnumerable<IArtist>> Artists(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.Genre.GetArtistsForGenre(this, cancellationToken, start, count);

        public Task<IEnumerable<IRadio>> Radio(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.Genre.GetRadioForGenre(this, cancellationToken, start, count);


        public Task<IChart> Charts(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.Charts.GetChartsForGenre(this, cancellationToken, start, count);


        public Task<IEnumerable<IAlbum>> AlbumChart(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.Charts.GetAlbumChartForGenre(this, cancellationToken, start, count);

        public Task<IEnumerable<IArtist>> ArtistChart(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.Charts.GetArtistChartForGenre(this, cancellationToken, start, count);

        public Task<IEnumerable<IPlaylist>> PlaylistChart(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.Charts.GetPlaylistChartForGenre(this, cancellationToken, start, count);

        public Task<IEnumerable<ITrack>> TrackChart(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.Charts.GetTrackChartForGenre(this, cancellationToken, start, count);


        public Task<IEnumerable<IAlbum>> NewReleases(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.Genre.GetNewReleasesForGenre(this, cancellationToken, start, count);

        public Task<IEnumerable<IAlbum>> DeezerSelection(CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.Client.Endpoints.Genre.GetDeezerSelectionForGenre(this, cancellationToken, start, count);


        public override string ToString()
        {
            return string.Format("E.Deezer: Genre({0} ({1}))", Name, Id);
        }


        // JSON
        internal const string ID_PROPERTY_NAME = "id";
        internal const string NAME_RROPERY_NAME = "name";


        public static IGenre FromJson(JToken json, IDeezerClient client)
        {
            ulong id = json.Value<ulong>(ID_PROPERTY_NAME);
            string name = json.Value<string>(NAME_RROPERY_NAME);

            return new Genre()
            {
                Id = id,
                Name = name,
                Images = Api.Images.FromJson(json),

                Client = client,
            };
        }
    }

}
