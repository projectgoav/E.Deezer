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
				if (aTask.Result.Data != null)
				{
					IInfos info = aTask.Result.Data;
					return info;
				}
                else { return new Infos(); }
			});
		}

		/// <summary>
		/// Get User information (like in the User tab of API explorer at https://developers.deezer.com/api/explorer)
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public Task<IUser> GetUser(int userId)
		{
			IRestRequest request = new RestRequest("user/{id}", Method.GET);
			request.AddParameter("id", userId, ParameterType.UrlSegment);
			return Execute<User>(request).ContinueWith<IUser>((aTask) =>
			{
				if (aTask.Result != null)
				{
					//Insert reference to client to get access to client
					aTask.Result.Data.Deserialize(this);

					IUser user = aTask.Result.Data;
					return user;
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
		/// <returns>A book of search results</returns>
		public Task<IBook<IAlbum>> SearchAlbums(string aQuery)
		{
            return GetBook<Album, IAlbum>(() =>
            {
                IRestRequest request = new RestRequest("/search/album", Method.GET);
                request.AddParameter("q", aQuery);
                return request;
            }, aItem => aItem);	
		}

		/// <summary>
		/// Search deezer for artists matching query
		/// </summary>
		/// <param name="aQuery">Search query</param>
		/// <returns>A book of search results</returns>
		public Task<IBook<IArtist>> SearchArtists(string aQuery)
		{
            return GetBook<Artist, IArtist>(() =>
            {
                IRestRequest request = new RestRequest("/search/artist", Method.GET);
                request.AddParameter("q", aQuery);
                return request;
            }, aItem => aItem);
		}


		/// <summary>
		/// Searches Deezer for track matching query
		/// </summary>
		/// <param name="aQuery">Search query</param>
		/// <returns>A book of search results</returns>
		public Task<IBook<ITrack>> SearchTracks(string aQuery)
		{
            return GetBook<Track, ITrack>(() =>
            {
                IRestRequest request = new RestRequest("/search/track", Method.GET);
                request.AddParameter("q", aQuery);
                return request;
            }, aItem => aItem);
	
		}

		/// <summary>
		/// Searches Deezer for playlists
		/// </summary>
		/// <param name="aQuery">Search query</param>
		/// <returns>A book of search results</returns>
		public Task<IBook<IPlaylist>> SearchPlaylists(string aQuery)
		{
            return GetBook<Playlist, IPlaylist>(() =>
            {
                IRestRequest request = new RestRequest("/search/playlist", Method.GET);
                request.AddParameter("q", aQuery);
                return request;
            }, aItem => aItem);
		}
		#endregion

		#region Content
		//Gaining content from the API

		#region Albums

		/// <summary>
		/// Gets the specificed albums tracks
		/// </summary>
		/// <param name="aId">Album Id</param>
		/// <returns>A book of Album tracks</returns>
		internal Task<IBook<ITrack>> GetAlbumTracks(uint aId)
		{
            return GetBook<Track, ITrack>(() =>
            {
                IRestRequest Request = new RestRequest("/album/{id}/tracks", Method.GET);
                Request.AddParameter("id", aId, ParameterType.UrlSegment);
                return Request;
            }, aItem => aItem);
		}

		#endregion //Albums

		#region Artists

		/// <summary>
		/// Gets the top tracks of an artist
		/// </summary>
		/// <param name="aId">Artist Id</param>
		/// <returns>A book of top tracks from artist</returns>
		internal Task<IBook<ITrack>> GetArtistTopTracks(uint aId)
		{
            return GetBook<Track, ITrack>(() =>
            {
                IRestRequest request = new RestRequest("/artist/{id}/top", Method.GET);
                request.AddParameter("id", aId, ParameterType.UrlSegment);
                return request;
            }, aItem => aItem);
		}

		/// <summary>
		/// Gets albums by given artist
		/// </summary>
		/// <param name="aId">Artist Id</param>
		/// <returns>A book of albums by artist</returns>
		internal Task<IBook<IAlbum>> GetArtistAlbums(uint aId)
		{
            return GetBook<Album, IAlbum>(() =>
            {
                IRestRequest request = new RestRequest("/artist/{id}/albums", Method.GET);
                request.AddParameter("id", aId, ParameterType.UrlSegment);
                return request;
            }, aItem => aItem);
		}

		/// <summary>
		/// Gets related artists
		/// </summary>
		/// <param name="aId">Artist Id</param>
		/// <returns>A book of artists related to given artist</returns>
		internal Task<IBook<IArtist>> GetArtistRelated(uint aId)
		{
            return GetBook<Artist, IArtist>(() =>
            {
                IRestRequest request = new RestRequest("/artist/{id}/related", Method.GET);
                request.AddParameter("id", aId, ParameterType.UrlSegment);
                return request;
            }, aItem => aItem);
		}

		/// <summary>
		/// Gets an artist's tracklist
		/// </summary>
		/// <param name="aId">Artist Id</param>
		/// <returns>First page f artist's tracklist</returns>
		internal Task<IBook<ITrack>> GetArtistTracklist(uint aId)
		{
            return GetBook<Track, ITrack>(() =>
            {
                IRestRequest request = new RestRequest("/artist/{id}/radio", Method.GET);
                request.AddParameter("id", aId, ParameterType.UrlSegment);
                return request;
            }, aItem => aItem);
		}

		/// <summary>
		/// Gets playlists featuring the given artist
		/// </summary>
		/// <param name="aId">Artist Id</param>
		/// <returns>A book of playlists containing the given artist</returns>
		internal Task<IBook<IPlaylist>> GetArtistPlaylists(uint aId)
		{
            return GetBook<Playlist, IPlaylist>(() =>
            {
                IRestRequest request = new RestRequest("/artist/{id}/playlists", Method.GET);
                request.AddParameter("id", aId, ParameterType.UrlSegment);
                return request;
            }, aItem => aItem);
		}
		#endregion //Artists

		#region Tracks

		/// <summary>
		/// Gets a tracklist for a playlist
		/// </summary>
		/// <param name="aId">Playlist Id</param>
		/// <returns>A book of tracks in playlist</returns>
		internal Task<IBook<ITrack>> GetPlaylistTracks(int aId)
		{
            return GetBook<Track, ITrack>(() =>
            {
                IRestRequest request = new RestRequest("/playlist/{id}/tracks", Method.GET);
                request.AddParameter("id", aId, ParameterType.UrlSegment);
                return request;
            }, aItem => aItem);
		}

		#endregion

		#region Playlists

		internal Task<IBook<IPlaylist>> GetUserFavouritePlaylists(uint userId)
		{
            return GetBook<Playlist, IPlaylist>(() =>
            {
                IRestRequest request = new RestRequest("/user/{id}/playlists", Method.GET);
                request.AddParameter("id", userId, ParameterType.UrlSegment);
                return request;
            }, aItem => aItem);	
		}

		#endregion //Playlists

        #region Genre

        /// <summary>
        /// Gets the common genre from Deezer
        /// </summary>
        /// <returns>List of the common genre in the Deezer Library.</returns>
        public Task<IGenreList> GetCommonGenre()
        {
            IRestRequest request = new RestRequest("/genre", Method.GET);
            return Execute<GenreList>(request).ContinueWith<IGenreList>((aTask) =>
            {
                if(aTask.Result != null)
                {
                    aTask.Result.Data.Deserialize(this);
                    return aTask.Result.Data;
                }
                return new GenreList();
            });
        }


        /// <summary>
        /// Gets artists associated with the given genre
        /// </summary>
        /// <param name="aGenreId">Genre to search</param>
        /// <returns></returns>
        internal Task<IBook<IArtist>> GetGenreArtists(uint aGenreId)
        {
            return GetBook<Artist, IArtist>(() =>
            {
                IRestRequest request = new RestRequest("/genre/{id}/artists", Method.GET);
                request.AddParameter("id", aGenreId, ParameterType.UrlSegment);
                return request;
            }, aItem => aItem);
        }

        #endregion //Genre


        #endregion //Content

        #endregion //Deezer Methods



        private Task<IRestResponse> Execute(IRestRequest aRequest)
        {
            return iSession.Execute(aRequest, iCancellationTokenSource.Token);
        }

		private Task<IRestResponse<T>> Execute<T>(IRestRequest aRequest)
		{
            var task = iSession.Execute<T>(aRequest, iCancellationTokenSource.Token).ContinueWith<IRestResponse<T>>((aTask) =>
            {
                if (aTask.IsFaulted) { throw aTask.Exception; }
                else { return aTask.Result; }
            });
            task.SuppressExceptions();
            return task;
		}


        /* Gets a book object
         * --------------------
         * Creates a quick network call to poll the API for a total number of results.
         * Adds a reference to a ReadPage() function so we can get a particular number
         * of items from a specified location in a book */
        private Task<IBook<TDest>> GetBook<TSource, TDest>(Func<IRestRequest> aRequestFn, Func<TSource, TDest> aCastFn) where TSource : IDeserializable<DeezerClient>
        {
            var request = aRequestFn();
            request.AddParameter("limit", 0);
            request.AddParameter("index", 0);
            var result = Execute<DeezerFragment<TSource>>(request).ContinueWith<IBook<TDest>>((aTask) =>
            {
                uint total = (aTask.Result.Data.Total == 0) ?  uint.MaxValue : aTask.Result.Data.Total;
                return new Book<TSource, TDest>(total, (aIndex, aCount, aCallback) =>
                {
                    ReadPage<TSource, TDest>(aRequestFn, aIndex, aCount, iCancellationTokenSource.Token,  aCallback, aCastFn);
                });
            }, iCancellationTokenSource.Token, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
            result.SuppressExceptions();
            return result;
        }

        /* ReadPage
         * Gets the specified number of items (aCount) starting from a given place (aIndex) in a Book */
        private void ReadPage<TSource, TDest>(Func<IRestRequest> aRequestFn, uint aIndex, uint aCount,CancellationToken aCancellationToken, Action<IPage<TDest>> aCallback, Func<TSource, TDest> aCastFn) where TSource : IDeserializable<DeezerClient>
        {
            var request = aRequestFn();
            request.AddParameter("limit", aCount);
            request.AddParameter("index", aIndex);
            Execute<DeezerFragment<TSource>>(request).ContinueWith((aTask) =>
            {
                try
                {
                    foreach (var item in aTask.Result.Data.Data) {  item.Deserialize(this); }

                    var fragment = new Page<TDest>(aIndex, (from i in aTask.Result.Data.Data select aCastFn(i)));
                    aCallback(fragment);
                }
                catch (AggregateException ex)
                {
                    Console.WriteLine(ex.GetBaseException().Message);
                    //TODO, do we want to do something with an Error Callback here?
                }
            }, aCancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }
	}
}
