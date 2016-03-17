using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using E.Deezer.Api;

namespace E.Deezer.Endpoint
{
    public interface IAlbumEndpoint
    {
        //Task<IEnumerable<ITrack>> GetTracks(uint aAlbumId);
    }

    internal class AlbumEndpoint : IAlbumEndpoint
    {
        private DeezerClient iClient;

        public AlbumEndpoint(DeezerClient aClient) {  iClient = aClient;  }

        public Task<IEnumerable<ITrack>> GetTracks(uint aAlbumId)
        {
            return new Album()
            {
                Id = aAlbumId,
                Client = iClient,
            }.GetTracks();
        }

        public Task<bool> Rate(uint aAlbumId, int aRating)
        {
            return new Album()
            {
                Id = aAlbumId,
                Client = iClient
            }.Rate(aRating);
        }
    }
}
