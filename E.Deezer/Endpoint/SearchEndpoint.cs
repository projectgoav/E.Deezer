using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using E.Deezer.Api;

namespace E.Deezer.Endpoint
{
    public interface  ISearchEndpoint
    {
        Task<IEnumerable<IAlbum>> Albums(string aQuery);
        Task<IEnumerable<IAlbum>> Albums(string aQuery, uint aCount);
        Task<IEnumerable<IAlbum>> Albums(string aQuery, uint aStart, uint aCount);

        Task<IEnumerable<IArtist>> Artists(string aQuery);
        Task<IEnumerable<IArtist>> Artists(string aQuery, uint aCount);
        Task<IEnumerable<IArtist>> Artists(string aQuery, uint aStart, uint aCount);

        Task<IEnumerable<IPlaylist>> Playlists(string aQuery);
        Task<IEnumerable<IPlaylist>> Playlists(string aQuery, uint aCount);
        Task<IEnumerable<IPlaylist>> Playlists(string aQuery, uint aStart, uint aCount);

        Task<IEnumerable<ITrack>> Tracks(string aQuery);
        Task<IEnumerable<ITrack>> Tracks(string aQuery, uint aCount);
        Task<IEnumerable<ITrack>> Tracks(string aQuery, uint aStart, uint aCount);

        Task<IEnumerable<IRadio>> Radio(string aQuery);
        Task<IEnumerable<IRadio>> Radio(string aQuery, uint aCount);
        Task<IEnumerable<IRadio>> Radio(string aQuery, uint aStart, uint aCount);

    }

    internal class SearchEndpoint : ISearchEndpoint
    {
        private const string SEARCH_BASE = "search/";

        private DeezerClient iClient;

        public SearchEndpoint(DeezerClient aClient) { iClient = aClient; }


        public Task<IEnumerable<IAlbum>> Albums(string aQuery) { return Albums(aQuery, 0, iClient.ResultSize); }
        public Task<IEnumerable<IAlbum>> Albums(string aQuery, uint aCount) { return Albums(aQuery, 0, aCount); }
        public Task<IEnumerable<IAlbum>> Albums(string aQuery, uint aStart, uint aCount) { return Get<Album, IAlbum>("album", aQuery, aStart, aCount); }

        public Task<IEnumerable<IArtist>> Artists(string aQuery) { return Artists(aQuery, 0, iClient.ResultSize); }
        public Task<IEnumerable<IArtist>> Artists(string aQuery, uint aCount) { return Artists(aQuery, 0, aCount); }
        public Task<IEnumerable<IArtist>> Artists(string aQuery, uint aStart, uint aCount) { return Get<Artist, IArtist>("artist", aQuery, aStart, aCount); }


        public Task<IEnumerable<IPlaylist>> Playlists(string aQuery) { return Playlists(aQuery, 0, iClient.ResultSize); }
        public Task<IEnumerable<IPlaylist>> Playlists(string aQuery, uint aCount) { return Playlists(aQuery, 0, aCount); }
        public Task<IEnumerable<IPlaylist>> Playlists(string aQuery, uint aStart, uint aCount) { return Get<Playlist, IPlaylist>("playlist", aQuery, aStart, aCount); }

        public Task<IEnumerable<ITrack>> Tracks(string aQuery) { return Tracks(aQuery, 0, iClient.ResultSize); }
        public Task<IEnumerable<ITrack>> Tracks(string aQuery, uint aCount) { return Tracks(aQuery, 0, aCount); }
        public Task<IEnumerable<ITrack>> Tracks(string aQuery, uint aStart, uint aCount) { return Get<Track, ITrack>("track", aQuery, aStart, aCount);   }

        public Task<IEnumerable<IRadio>> Radio(string aQuery) { return Radio(aQuery, 0, iClient.ResultSize); }
        public Task<IEnumerable<IRadio>> Radio(string aQuery, uint aCount) { return Radio(aQuery, 0, aCount); }
        public Task<IEnumerable<IRadio>> Radio(string aQuery, uint aStart, uint aCount) { return Get<Radio, IRadio>("radio", aQuery, aStart, aCount); }


        private Task<IEnumerable<TDest>> Get<TSource, TDest>(string aSearchEndpoint, string aQuery, uint aStart, uint aCount) where TSource : TDest, IDeserializable<DeezerClient>
        {
            string method = string.Format("{0}{1}", SEARCH_BASE, aSearchEndpoint);

            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("q", aQuery)
            };

            return iClient.Get<TSource>(method, parms, aStart, aCount).ContinueWith<IEnumerable<TDest>>((aTask) =>
            {
                return iClient.Transform<TSource, TDest>(aTask.Result);
            }, iClient.CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }

    }
}
