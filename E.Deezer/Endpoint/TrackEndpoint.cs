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
        private DeezerClient iClient;


        public TrackEndpoint(DeezerClient aClient) {  iClient = aClient;  }
    }
}
