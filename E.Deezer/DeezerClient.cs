﻿using System;
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

		internal Task<IPagedResponse<T>> GetPage<T>(string aUrl)
		{
			throw new NotImplementedException();
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

		#region Content
		//Gaining content from the API

		#region Albums

		/// <summary>
		/// Gets the specificed albums tracks
		/// </summary>
		/// <param name="aId">Album Id</param>
		/// <returns>First page of Album tracks</returns>
		internal Task<IPagedResponse<ITrack>> GetAlbumTracks(uint aId, int aResultSize)
		{
			IRestRequest Request = new RestRequest("/album/{id}/tracks", Method.GET);
			Request.AddParameter("id", aId, ParameterType.UrlSegment);
			return Execute<Track>(Request, aResultSize).ContinueWith<IPagedResponse<ITrack>>((aTask) =>
			{
				List<ITrack> items = new List<ITrack>();
				foreach (var item in aTask.Result.Data.Data)
				{
					item.Deserialize(this);
					items.Add(item as ITrack);
				}

				aTask.Result.Data.Deserialize(this);

				return new PagedResponse<ITrack>()
				{
					Data = items,
					Total = aTask.Result.Data.Total,
					Next = aTask.Result.Data.Next,
					Previous = aTask.Result.Data.Previous,
				};
			});
		}

		#endregion //Albums

		#region Artists

		/// <summary>
		/// Gets the top tracks of an artist
		/// </summary>
		/// <param name="aId">Artist Id</param>
		/// <returns>First page of top tracks from artist</returns>
		internal Task<IPagedResponse<ITrack>> GetArtistTopTracks(uint aId, int aResultSize)
		{
			IRestRequest Request = new RestRequest("/artist/{id}/top", Method.GET);
			Request.AddParameter("id", aId, ParameterType.UrlSegment);
			return Execute<Track>(Request, aResultSize).ContinueWith<IPagedResponse<ITrack>>((aTask) =>
			{
				List<ITrack> items = new List<ITrack>();
				foreach (var item in aTask.Result.Data.Data)
				{
					item.Deserialize(this);
					items.Add(item as ITrack);
				}

				aTask.Result.Data.Deserialize(this);

				return new PagedResponse<ITrack>()
				{
					Data = items,
					Total = aTask.Result.Data.Total,
					Next = aTask.Result.Data.Next,
					Previous = aTask.Result.Data.Previous,
				};
			});
		}

		/// <summary>
		/// Gets albums by given artist
		/// </summary>
		/// <param name="aId">Artist Id</param>
		/// <returns>First page of albums by artist</returns>
		internal Task<IPagedResponse<IAlbum>> GetArtistAlbums(uint aId, int aResultSize)
		{
			IRestRequest Request = new RestRequest("/artist/{id}/albums", Method.GET);
			Request.AddParameter("id", aId, ParameterType.UrlSegment);
			return Execute<Album>(Request, aResultSize).ContinueWith<IPagedResponse<IAlbum>>((aTask) =>
			{
				List<IAlbum> items = new List<IAlbum>();
				foreach (var item in aTask.Result.Data.Data)
				{
					item.Deserialize(this);
					items.Add(item as IAlbum);
				}

				aTask.Result.Data.Deserialize(this);

				return new PagedResponse<IAlbum>()
				{
					Data = items,
					Total = aTask.Result.Data.Total,
					Next = aTask.Result.Data.Next,
					Previous = aTask.Result.Data.Previous,
				};
			});
		}

		/// <summary>
		/// Gets related artists
		/// </summary>
		/// <param name="aId">Artist Id</param>
		/// <returns>First page of artists related to given artist</returns>
		internal Task<IPagedResponse<IArtist>> GetArtistRelated(uint aId, int aResultSize)
		{
			IRestRequest Request = new RestRequest("/artist/{id}/related", Method.GET);
			Request.AddParameter("id", aId, ParameterType.UrlSegment);
			return Execute<Artist>(Request, aResultSize).ContinueWith<IPagedResponse<IArtist>>((aTask) =>
			{
				List<IArtist> items = new List<IArtist>();
				foreach (var item in aTask.Result.Data.Data)
				{
					item.Deserialize(this);
					items.Add(item as IArtist);
				}

				aTask.Result.Data.Deserialize(this);

				return new PagedResponse<IArtist>()
				{
					Data = items,
					Total = aTask.Result.Data.Total,
					Next = aTask.Result.Data.Next,
					Previous = aTask.Result.Data.Previous,
				};
			});
		}

		/// <summary>
		/// Gets an artist's tracklist
		/// </summary>
		/// <param name="aId">Artist Id</param>
		/// <returns>First page f artist's tracklist</returns>
		internal Task<IPagedResponse<ITrack>> GetArtistTracklist(uint aId, int aResultSize)
		{
			IRestRequest Request = new RestRequest("/artist/{id}/radio", Method.GET);
			Request.AddParameter("id", aId, ParameterType.UrlSegment);
			return Execute<Track>(Request, aResultSize).ContinueWith<IPagedResponse<ITrack>>((aTask) =>
			{
				List<ITrack> items = new List<ITrack>();
				foreach (var item in aTask.Result.Data.Data)
				{
					item.Deserialize(this);
					items.Add(item as ITrack);
				}

				aTask.Result.Data.Deserialize(this);

				return new PagedResponse<ITrack>()
				{
					Data = items,
					Total = aTask.Result.Data.Total,
					Next = aTask.Result.Data.Next,
					Previous = aTask.Result.Data.Previous,
				};
			});
		}

		/// <summary>
		/// Gets playlists featuring the given artist
		/// </summary>
		/// <param name="aId">Artist Id</param>
		/// <returns>First page of playlists containing the given artist</returns>
		internal Task<IPagedResponse<IPlaylist>> GetArtistPlaylists(uint aId, int aResultSize)
		{
			IRestRequest Request = new RestRequest("/artist/{id}/playlists", Method.GET);
			Request.AddParameter("id", aId, ParameterType.UrlSegment);
			return Execute<Playlist>(Request, aResultSize).ContinueWith<IPagedResponse<IPlaylist>>((aTask) =>
			{
				List<IPlaylist> items = new List<IPlaylist>();
				foreach (var item in aTask.Result.Data.Data)
				{
					item.Deserialize(this);
					items.Add(item as IPlaylist);
				}

				aTask.Result.Data.Deserialize(this);

				return new PagedResponse<IPlaylist>()
				{
					Data = items,
					Total = aTask.Result.Data.Total,
					Next = aTask.Result.Data.Next,
					Previous = aTask.Result.Data.Previous,
				};
			});
		}
		#endregion //Artists

		#region Tracks

		/// <summary>
		/// Gets a tracklist for a playlist
		/// </summary>
		/// <param name="aId">Playlist Id</param>
		/// <returns>FIrst page of tracks in playlist</returns>
		internal Task<IPagedResponse<ITrack>> GetPlaylistTracks(int aId, int aResultSize)
		{
			IRestRequest Request = new RestRequest("/playlist/{id}/tracks", Method.GET);
			Request.AddParameter("id", aId, ParameterType.UrlSegment);
			return Execute<Track>(Request, aResultSize).ContinueWith<IPagedResponse<ITrack>>((aTask) =>
			{
				List<ITrack> items = new List<ITrack>();
				foreach (var item in aTask.Result.Data.Data)
				{
					item.Deserialize(this);
					items.Add(item as ITrack);
				}

				aTask.Result.Data.Deserialize(this);

				return new PagedResponse<ITrack>()
				{
					Data = items,
					Total = aTask.Result.Data.Total,
					Next = aTask.Result.Data.Next,
					Previous = aTask.Result.Data.Previous,
				};
			});
		}

		#endregion

		#region Playlists

		internal Task<IPagedResponse<IPlaylist>> GetUserFavouritePlaylists(uint userId, int aResultSize)
		{
			IRestRequest Request = new RestRequest("/user/{id}/playlists", Method.GET);
			Request.AddParameter("id", userId, ParameterType.UrlSegment);
			return Execute<Playlist>(Request, aResultSize).ContinueWith<IPagedResponse<IPlaylist>>((aTask) =>
			{
				List<IPlaylist> items = new List<IPlaylist>();
				PagedResponse<Playlist> page = aTask.Result.Data;
				page.Deserialize(this);

				foreach (var item in page.Data)
				{
					item.Deserialize(this);
					items.Add(item as IPlaylist);
				}

				aTask.Result.Data.Deserialize(this);

				return new PagedResponse<IPlaylist>()
				{
					Data = items,
					Total = aTask.Result.Data.Total,
					Next = aTask.Result.Data.Next,
					Previous = aTask.Result.Data.Previous,
				};
			});
		}

		#endregion //Playlists

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

        private Task<IRestResponse<PagedResponse<T>>> Execute<T>(IRestRequest aRequest, int aResultSize)
        {
            if (aResultSize > 0) { aRequest.AddParameter("limit", aResultSize, ParameterType.QueryString); }
            var task = iSession.Execute<PagedResponse<T>>(aRequest, iCancellationTokenSource.Token).ContinueWith<IRestResponse<PagedResponse<T>>>((aTask) =>
            {
                if (aTask.IsFaulted) { throw aTask.Exception; }
                else { return aTask.Result; }
            });
            task.SuppressExceptions();
            return task;
        }	
	}
}
