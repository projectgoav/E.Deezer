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
    public interface IGenreEndpoint
    {
        Task<IGenre> GetById(ulong genreId, CancellationToken cancellationToken);

        Task<IEnumerable<IGenre>> GetCommonGenre(CancellationToken cancellationToken);

        Task<IEnumerable<IArtist>> GetArtistsForGenre(IGenre genre, CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IArtist>> GetArtistsForGenre(ulong genreId, CancellationToken cancellationToken, uint start = 0, uint count = 25);

        Task<IEnumerable<IRadio>> GetRadioForGenre(IGenre genre, CancellationToken cancellationToken, uint start = 0, uint count = 25);
        Task<IEnumerable<IRadio>> GetRadioForGenre(ulong genreId, CancellationToken cancellationToken, uint start = 0, uint count = 25);
    }


    internal class GenreEndpoint : IGenreEndpoint
    {
        private const string START_PARAM = "index";
        private const string COUNT_PARAM = "limit";

        private readonly IDeezerClient client;


        public GenreEndpoint(IDeezerClient client)
        {
            this.client = client;
        }



        public Task<IGenre> GetById(ulong genreId, CancellationToken cancellationToken)
            => this.client.Get($"genre/{genreId}",
                               cancellationToken,
                               json => Api.Genre.FromJson(json, this.client));

        public Task<IEnumerable<IGenre>> GetCommonGenre(CancellationToken cancellationToken)
            => this.client.Get("genre",
                               cancellationToken,
                               json => FragmentOf<IGenre>.FromJson(json, x => Api.Genre.FromJson(x, this.client)));



        // TODO: Endpoint accepts paging but doesn't take note of it
        public Task<IEnumerable<IArtist>> GetArtistsForGenre(IGenre genre, CancellationToken cancellationToken, uint start = 0, uint count = 25)
        {
            genre.ThrowIfNull();

            return GetArtistsForGenre(genre.Id, cancellationToken, start, count);
        }

        // TODO: Endpoint accepts paging but doesn't take note of it
        public Task<IEnumerable<IArtist>> GetArtistsForGenre(ulong genreId, CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.client.Get($"genre/{genreId}/artists?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => FragmentOf<IArtist>.FromJson(json, x => Api.Artist.FromJson(x, this.client)));


        // TODO: Endpoint accepts paging but doesn't take note of it
        public Task<IEnumerable<IRadio>> GetRadioForGenre(IGenre genre, CancellationToken cancellationToken, uint start = 0, uint count = 25)
        {
            genre.ThrowIfNull();

            return GetRadioForGenre(genre.Id, cancellationToken, start, count);
        }


        // TODO: Endpoint accepts paging but doesn't take note of it
        public Task<IEnumerable<IRadio>> GetRadioForGenre(ulong genreId, CancellationToken cancellationToken, uint start = 0, uint count = 25)
            => this.client.Get($"genre/{genreId}/radios?{START_PARAM}={start}&{COUNT_PARAM}={count}",
                               cancellationToken,
                               json => FragmentOf<IRadio>.FromJson(json, x => Api.Radio.FromJson(x, this.client)));
    }
}
