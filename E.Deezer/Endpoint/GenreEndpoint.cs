using E.Deezer.Api;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E.Deezer.Endpoint
{
    public interface IGenreEndpoint
    {
        Task<IEnumerable<IGenre>> GetCommonGenre();
    }

    internal class GenreEndpoint : IGenreEndpoint
    {
        private readonly DeezerClient _client;

        public GenreEndpoint(DeezerClient client)
        {
            _client = client;
        }

        public Task<IEnumerable<IGenre>> GetCommonGenre()
        {
            return _client.Get<Genre>("genre", RequestParameter.EmptyList)
                          .ContinueWith<IEnumerable<IGenre>>((aTask) =>
                            {
                                List<IGenre> items = new List<IGenre>();

                                foreach (var g in aTask.Result.Items)
                                {
                                    g.Deserialize(_client);
                                    items.Add(g);
                                }

                                return items;
                            }, _client.CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }
    }
}
