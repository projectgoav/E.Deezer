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
        private DeezerClientV2 iClient;


        public ArtistEndpoint(DeezerClientV2 aClient) {  iClient = aClient;  }
    }
}
