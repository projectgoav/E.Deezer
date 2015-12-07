using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace E.Deezer.Endpoint
{
    public interface  ISearchEndpoint
    {
        Task All(string aQuery);
        Task Albums(string aQuery);
        Task Artists(string aQuery);
        Task Playlists(string aQuery);
        Task Tracks(string aQuery);
    }

    internal class SearchEndpoint : ISearchEndpoint
    {
        private DeezerClientV2 iClient;

        public Task All(string aQuery)
        {
            return Task.Factory.StartNew(()=>
            {
                Task[] tasks = new Task[]
                {
                    Albums(aQuery),
                    Artists(aQuery),
                    Playlists(aQuery),
                    Tracks(aQuery),
                };
                Task.WaitAll(tasks);

                /* Process */

            });
        }

        public Task Albums(string aQuery)   { throw new NotImplementedException();  }
       
        public Task Artists(string aQuery)  { throw new NotImplementedException();  }

        public Task Playlists(string aQuery) { throw new NotImplementedException(); }

        public Task Tracks(string aQuery) {  throw new NotImplementedException(); }

        public SearchEndpoint(DeezerClientV2 aClient) { iClient = aClient; }
    }
}
