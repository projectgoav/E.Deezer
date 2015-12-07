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
        Task<IEnumerable<ITrack>> GetTracks(IAlbum aAlbum);
        Task<IEnumerable<ITrack>> GetTracks(IAlbum aAlbum, uint aCount);
        Task<IEnumerable<ITrack>> GetTracks(IAlbum aAlbum, uint aStart, uint aCount);

        Task<IEnumerable<ITrack>> GetTracks(uint aAlbumId);
        Task<IEnumerable<ITrack>> GetTracks(uint aAlbumId, uint aCount);
        Task<IEnumerable<ITrack>> GetTracks(uint aAlbumId, uint aStart, uint aCount);
    }

    internal class AlbumEndpoint : IAlbumEndpoint
    {
        private DeezerClientV2 iClient;

        public AlbumEndpoint(DeezerClientV2 aClient) {  iClient = aClient;  }

        #region WithGenre 
        public Task<IEnumerable<ITrack>> GetTracks(IAlbum aAlbum) { return GetTracks(aAlbum.Id, 0, DeezerSessionV2.DEFAULT_SIZE); }
        public Task<IEnumerable<ITrack>> GetTracks(IAlbum aAlbum, uint aCount) { return GetTracks(aAlbum, 0, aCount); }
        public Task<IEnumerable<ITrack>> GetTracks(IAlbum aAlbum, uint aStart, uint aCount) {  return GetTracks(aAlbum, aStart, aCount); }
        #endregion


        #region With ID Number
        public Task<IEnumerable<ITrack>> GetTracks(uint aAlbumId) { return GetTracks(aAlbumId, 0, DeezerSessionV2.DEFAULT_SIZE); }
        public Task<IEnumerable<ITrack>> GetTracks(uint aAlbumId, uint aCount) { return GetTracks(aAlbumId, 0, aCount); }

        public Task<IEnumerable<ITrack>> GetTracks(uint aAlbumId, uint aStart, uint aCount)
        {
            string[] parms = new string[] { "URL", "id", aAlbumId.ToString() };

            return iClient.Get<Track>("album/{id}/tracks", parms, aStart, aCount).ContinueWith<IEnumerable<ITrack>>((aTask) =>
            {
                return aTask.Result.Items;
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        #endregion
    }
}
