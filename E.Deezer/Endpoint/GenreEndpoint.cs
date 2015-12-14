using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using E.Deezer.Api;

namespace E.Deezer.Endpoint
{
    public interface IGenreEndpoint
    {
        Task<IEnumerable<IGenre>> GetCommonGenre();
    }

    internal class GenreEndpoint : IGenreEndpoint
    {
        private DeezerClient iClient;

        public Task<IEnumerable<IGenre>> GetCommonGenre()
        {
            return iClient.Get<Genre>("genre", new string[] {}).ContinueWith<IEnumerable<IGenre>>((aTask) =>
            {
                List<IGenre> items = new List<IGenre>();

                foreach(var g in aTask.Result.Items)
                {
                    g.Deserialize(iClient);
                    items.Add(g);
                }

                return items;
            }, iClient.Token, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }


        public GenreEndpoint(DeezerClient aClient) {  iClient = aClient;  }
    }
}
