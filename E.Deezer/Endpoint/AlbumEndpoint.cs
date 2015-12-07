using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E.Deezer.Endpoint
{
    public interface IAlbumEndpoint
    {

    }

    internal class AlbumEndpoint : IAlbumEndpoint
    {
        private DeezerClientV2 iClient;


        public AlbumEndpoint(DeezerClientV2 aClient) {  iClient = aClient;  }
    }
}
