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
        //Search Methods for Deezer.com

        /// <summary>
        /// Searches Deezer for Albums matching the query
        /// </summary>
        /// <param name="aQuery">Search query</param>
        /// <returns>First page of search results</returns>
        public Task<IPagedResponse<IAlbum>> SearchAlbums(string aQuery)
        {
            IRestRequest request = new RestRequest("/search/album", Method.GET);
            request.AddParameter("q", aQuery);
            return Execute<PagedResponse<Album>>(request).ContinueWith<IPagedResponse<IAlbum>>((aTask) =>
            {
                List<IAlbum> items = new List<IAlbum>();
                //Insert reference to client to get access to client
                foreach (var item in aTask.Result.Data.Data) 
                {
                    item.Deserialize(this);
                    items.Add(item as IAlbum);
                }

                aTask.Result.Data.Deserialize(this);

                IPagedResponse<IAlbum> result = new PagedResponse<IAlbum>()
                {
                    Data = items,
                    Total = aTask.Result.Data.Total,
                    Next = aTask.Result.Data.Next,
                    Previous = aTask.Result.Data.Previous
                };
                return result;
            });
        }

        /// <summary>
        /// Search deezer for artists matching query
        /// </summary>
        /// <param name="aQuery">Search query</param>
        /// <returns>First page of search results</returns>
        public Task<IPagedResponse<IArtist>> SearchArtists(string aQuery)
        {
            IRestRequest request = new RestRequest("/search/artist", Method.GET);
            request.AddParameter("q", aQuery);
            return Execute<PagedResponse<Artist>>(request).ContinueWith<IPagedResponse<IArtist>>((aTask) =>
            {
                List<IArtist> items = new List<IArtist>();
                //Insert reference to client to get access to client
                foreach (var item in aTask.Result.Data.Data)
                {
                    item.Deserialize(this);
                    items.Add(item as IArtist);
                }

                aTask.Result.Data.Deserialize(this);

                IPagedResponse<IArtist> result = new PagedResponse<IArtist>()
                {
                    Data = items,
                    Total = aTask.Result.Data.Total,
                    Next = aTask.Result.Data.Next,
                    Previous = aTask.Result.Data.Previous
                };
                return result;
            });
        }


        /// <summary>
        /// Searches Deezer for track matching query
        /// </summary>
        /// <param name="aQuery">Search query</param>
        /// <returns>First page of search results</returns>
        public Task<IPagedResponse<ITrack>> SearchTracks(string aQuery)
        {
            IRestRequest request = new RestRequest("/search/track", Method.GET);
            request.AddParameter("q", aQuery);
            return Execute<PagedResponse<Track>>(request).ContinueWith<IPagedResponse<ITrack>>((aTask) =>
            {
                List<ITrack> items = new List<ITrack>();
                //Insert reference to client to get access to client
                foreach (var item in aTask.Result.Data.Data)
                {
                    item.Deserialize(this);
                    items.Add(item as ITrack);
                }

                aTask.Result.Data.Deserialize(this);

                IPagedResponse<ITrack> result = new PagedResponse<ITrack>()
                {
                    Data = items,
                    Total = aTask.Result.Data.Total,
                    Next = aTask.Result.Data.Next,
                    Previous = aTask.Result.Data.Previous
                };
                return result;
            });
        }

        /// <summary>
        /// Searches Deezer for playlists
        /// </summary>
        /// <param name="aQuery">Search query</param>
        /// <returns>First page of search results</returns>
        public Task<IPagedResponse<IPlaylist>> SearchPlaylists(string aQuery)
        {
            IRestRequest request = new RestRequest("/search/playlist", Method.GET);
            request.AddParameter("q", aQuery);
            return Execute<PagedResponse<Playlist>>(request).ContinueWith<IPagedResponse<IPlaylist>>((aTask) =>
            {
                List<IPlaylist> items = new List<IPlaylist>();
                //Insert reference to client to get access to client
                foreach (var item in aTask.Result.Data.Data)
                {
                    item.Deserialize(this);
                    items.Add(item as IPlaylist);
                }

                aTask.Result.Data.Deserialize(this);

                IPagedResponse<IPlaylist> result = new PagedResponse<IPlaylist>()
                {
                    Data = items,
                    Total = aTask.Result.Data.Total,
                    Next = aTask.Result.Data.Next,
                    Previous = aTask.Result.Data.Previous
                };
                return result;
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
