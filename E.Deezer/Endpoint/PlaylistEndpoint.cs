using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using E.Deezer.Api;

namespace E.Deezer.Endpoint
{
    public interface IPlaylistEndpoint
    {

    }

    internal class PlaylistEndpoint : IPlaylistEndpoint
    {
        private DeezerClient iClient;


        public PlaylistEndpoint(DeezerClient aClient) {  iClient = aClient;  }


        public Task<bool> Rate(int aPlaylistId, int aRating)
        {
            return new Playlist()
            {
                Id = aPlaylistId,
                Client = iClient
            }.Rate(aRating);
        }
    }
}
