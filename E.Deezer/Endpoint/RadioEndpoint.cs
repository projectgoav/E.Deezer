using E.Deezer.Api;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E.Deezer.Endpoint
{
    public interface IRadioEndpoint
    {
        Task<IEnumerable<IRadio>> GetTop5();

        Task<IEnumerable<IRadio>> GetDeezerSelection(uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<IGenreWithRadios>> GetByGenres();
    }

    //TODO needs refactor for the Get<T> with permissions param
    internal class RadioEndpoint : IRadioEndpoint
    {
        private readonly DeezerClient _client;

        public RadioEndpoint(DeezerClient client)
        {
            _client = client;
        }

        public Task<IEnumerable<IRadio>> GetTop5()
        {
            return _client.Get<Radio>("radio/top", RequestParameter.EmptyList)
                            .ContinueWith<IEnumerable<IRadio>>((aTask) =>
                                {
                                    return _client.Transform<Radio, IRadio>(aTask.Result);
                                }, _client.CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }

        public Task<IEnumerable<IRadio>> GetDeezerSelection(uint aStart = 0, uint aCount = 100)
        {
            return _client.Get<Radio>("radio/lists", aStart, aCount).ContinueWith<IEnumerable<IRadio>>((aTask) =>
            {
                return _client.Transform<Radio, IRadio>(aTask.Result);
            }, _client.CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }

        public async Task<IEnumerable<IGenreWithRadios>> GetByGenres()
        {
            var response = await _client.Get<GenreWithRadios>("radio/genres", RequestParameter.EmptyList)
                .ConfigureAwait(false);

            foreach (var genre in response.Items)
            {
                foreach (var radio in genre.InternalRadios)
                {
                    radio.Deserialize(_client);
                }
            }

            return response.Items;
        }
    }
}
