using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E.Deezer.Endpoint
{
    public interface IArtistEndpoint
    {

    }

    internal class ArtistEndpoint : IArtistEndpoint
    {
        private DeezerClient iClient;


        public ArtistEndpoint(DeezerClient aClient) {  iClient = aClient;  }
    }
}
