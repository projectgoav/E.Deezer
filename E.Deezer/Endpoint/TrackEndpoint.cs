using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E.Deezer.Endpoint
{
    public interface ITrackEndpoint
    {

    }

    internal class TrackEndpoint : ITrackEndpoint
    {
        private DeezerClientV2 iClient;


        public TrackEndpoint(DeezerClientV2 aClient) {  iClient = aClient;  }
    }
}
