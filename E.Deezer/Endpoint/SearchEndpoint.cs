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

    }

    internal class SearchEndpoint : ISearchEndpoint
    {
        private DeezerClient iClient;

        private const string ENDPOINT = "search/";

        public Task<IEnumerable<IAlbum>> Albums(string aQuery) { return Albums(aQuery, 0, iClient.ResultSize); }
        public Task<IEnumerable<IAlbum>> Albums(string aQuery, uint aCount) { return Albums(aQuery, 0, aCount); }
        public Task<IEnumerable<IAlbum>> Albums(string aQuery, uint aStart, uint aCount)
        {
            string[] parms = new string[] { string.Empty, "q", aQuery };

            var t = iClient.Get<Album>(ENDPOINT + "album", parms, aStart, aCount).ContinueWith<IEnumerable<IAlbum>>((aTask) =>
            {
                    aTask.Result.Items.Deserialize(iClient);
                    return aTask.Result.Items;
            }, iClient.Token, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
            t.SuppressExceptions();
            return t;
        }

        public Task<IEnumerable<IArtist>> Artists(string aQuery) { return Artists(aQuery, 0, iClient.ResultSize); }
        public Task<IEnumerable<IArtist>> Artists(string aQuery, uint aCount) { return Artists(aQuery, 0, aCount); }
        public Task<IEnumerable<IArtist>> Artists(string aQuery, uint aStart, uint aCount)
        {
            string[] parms = new string[] { string.Empty, "q", aQuery };

            return iClient.Get<Artist>(ENDPOINT + "artist", parms, aStart, aCount).ContinueWith<IEnumerable<IArtist>>((aTask) =>
            {
                aTask.Result.Items.Deserialize(iClient);
                return aTask.Result.Items;
            }, iClient.Token, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }


        public Task<IEnumerable<IPlaylist>> Playlists(string aQuery) { return Playlists(aQuery, 0, iClient.ResultSize); }
        public Task<IEnumerable<IPlaylist>> Playlists(string aQuery, uint aCount) { return Playlists(aQuery, 0, aCount); }
        public Task<IEnumerable<IPlaylist>> Playlists(string aQuery, uint aStart, uint aCount)
        {
            string[] parms = new string[] { string.Empty, "q", aQuery };

            return iClient.Get<Playlist>(ENDPOINT + "playlist", parms, aStart, aCount).ContinueWith<IEnumerable<IPlaylist>>((aTask) =>
            {
                aTask.Result.Items.Deserialize(iClient);
                return aTask.Result.Items;
            }, iClient.Token, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }

        public Task<IEnumerable<ITrack>> Tracks(string aQuery) { return Tracks(aQuery, 0, iClient.ResultSize); }
        public Task<IEnumerable<ITrack>> Tracks(string aQuery, uint aCount) { return Tracks(aQuery, 0, aCount); }
        public Task<IEnumerable<ITrack>> Tracks(string aQuery, uint aStart, uint aCount)
        {
            string[] parms = new string[] { string.Empty, "q", aQuery };

            return iClient.Get<Track>(ENDPOINT + "track", parms, aStart, aCount).ContinueWith<IEnumerable<ITrack>>((aTask) =>
            {
                aTask.Result.Items.Deserialize(iClient);
                return aTask.Result.Items;
            }, iClient.Token, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }


        public SearchEndpoint(DeezerClient aClient) { iClient = aClient; }
    }
}
