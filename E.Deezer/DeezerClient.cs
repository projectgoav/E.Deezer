using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

using RestSharp;

using E.Deezer.Api;

namespace E.Deezer
{
    public class DeezerClient
    {
        private readonly DeezerSession iSession;
        private readonly CancellationTokenSource iCancellationTokenSource;

        public DeezerClient(DeezerSession aSession)
        {
            iSession = aSession;
            iCancellationTokenSource = new CancellationTokenSource();
        }

        #region Deezer Methods

        /// <summary>
        /// Get Deezer service availablity
        /// </summary>
        /// <returns>Deezer service availiblity information. <see cref="E.Deezer.Api.IInfos"/>See IInfos</returns>
        public Task<IInfos> GetInfos()
        {
            IRestRequest request = new RestRequest("infos", Method.GET);
            return Execute<Infos>(request).ContinueWith<IInfos>((aTask) =>
            {
                if (aTask.Result != null)
                {
                    IInfos info = aTask.Result.Data;
                    return info;
                }
                else { return null; }
            });
        }


        #region Search
        public Task<IPagedResponse<IAlbum>> SearchAlbums(string aQuery)
        {
            IRestRequest request = new RestRequest("/search/album", Method.GET);
            request.AddParameter("q", aQuery);
            return Execute<PagedResponse<Album>>(request).ContinueWith<IPagedResponse<IAlbum>>((aTask) =>
            {
                //Insert reference to client to get access to client
                foreach (var item in aTask.Result.Data.Data) 
                {
                    item.Deserialize(this); 
                }
                return aTask.Result.Data as IPagedResponse<IAlbum>;
            });
        }

        public Task<IPagedResponse<IArtist>> SearchArtists(string aQuery)
        {
            IRestRequest request = new RestRequest("/search/artist", Method.GET);
            request.AddParameter("q", aQuery);
            return Execute<PagedResponse<Artist>>(request).ContinueWith<IPagedResponse<IArtist>>((aTask) =>
            {
                //Insert reference to client to get access to client
                foreach (var item in aTask.Result.Data.Data)
                {
                    item.Deserialize(this);
                }
                return aTask.Result.Data as IPagedResponse<IArtist>;
            });
        }


        public Task<IPagedResponse<ITrack>> SearchTracks(string aQuery)
        {
            IRestRequest request = new RestRequest("/search/track", Method.GET);
            request.AddParameter("q", aQuery);
            return Execute<PagedResponse<Track>>(request).ContinueWith<IPagedResponse<ITrack>>((aTask) =>
            {
                //Insert reference to client to get access to client
                foreach (var item in aTask.Result.Data.Data)
                {
                    item.Deserialize(this);
                }
                return aTask.Result.Data as IPagedResponse<ITrack>;
            });
        }

        public Task<IPagedResponse<IPlaylist>> SearchPlaylists(string aQuery)
        {
            IRestRequest request = new RestRequest("/search/playlist", Method.GET);
            request.AddParameter("q", aQuery);
            return Execute<PagedResponse<Playlist>>(request).ContinueWith<IPagedResponse<IPlaylist>>((aTask) =>
            {
                //Insert reference to client to get access to client
                foreach (var item in aTask.Result.Data.Data)
                {
                    item.Deserialize(this);
                }

                return aTask.Result.Data as IPagedResponse<IPlaylist>;
            });
        }

        #endregion



        #endregion

        private Task<IRestResponse> Execute(IRestRequest aRequest)
        {
            return iSession.Execute(aRequest, iCancellationTokenSource.Token);
        }

        private Task<IRestResponse<T>> Execute<T>(IRestRequest aRequest) 
        {  
            return iSession.Execute<T>(aRequest, iCancellationTokenSource.Token);
        }
    }
}
