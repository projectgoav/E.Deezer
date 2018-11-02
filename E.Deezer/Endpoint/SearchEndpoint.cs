using E.Deezer.Api;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace E.Deezer.Endpoint
{
    public interface ISearchEndpoint
    {
        Task<IEnumerable<IAlbum>> Albums(string aQuery, uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<IArtist>> Artists(string aQuery, uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<IPlaylist>> Playlists(string aQuery, uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<ITrack>> Tracks(string aQuery, uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<IRadio>> Radio(string aQuery, uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<IUser>> User(string aQuery, uint aStart = 0, uint aCount = 100);

        Task<IEnumerable<ITrack>> Advanced(string aArtist = "", string aAlbum = "", string aTrack = "", string aLabel = "",
            uint aDur_Min = 0, uint aDur_Max = 0, uint aBpm_Min = 0, uint aBpm_Max = 0,
            uint aStart = 0, uint aCount = 100);
    }

    internal class SearchEndpoint : ISearchEndpoint
    {
        private const string SEARCH_BASE = "search";

        private readonly DeezerClient iClient;

        public SearchEndpoint(DeezerClient aClient)
        {
            iClient = aClient;
        }


        public Task<IEnumerable<IAlbum>> Albums(string aQuery, uint aStart = 0, uint aCount = 100)
            => Get<Album, IAlbum>("album", aQuery, aStart, aCount);

        public Task<IEnumerable<IArtist>> Artists(string aQuery, uint aStart = 0, uint aCount = 100)
            => Get<Artist, IArtist>("artist", aQuery, aStart, aCount);

        public Task<IEnumerable<IPlaylist>> Playlists(string aQuery, uint aStart = 0, uint aCount = 100)
            => Get<Playlist, IPlaylist>("playlist", aQuery, aStart, aCount);

        public Task<IEnumerable<ITrack>> Tracks(string aQuery, uint aStart = 0, uint aCount = 100)
            => Get<Track, ITrack>("track", aQuery, aStart, aCount);

        public Task<IEnumerable<IRadio>> Radio(string aQuery, uint aStart = 0, uint aCount = 100)
            => Get<Radio, IRadio>("radio", aQuery, aStart, aCount);

        public Task<IEnumerable<IUser>> User(string aQuery, uint aStart = 0, uint aCount = 100)
            => Get<User, IUser>("user", aQuery, aStart, aCount);

        public Task<IEnumerable<ITrack>> Advanced(string aArtist = "", string aAlbum = "", string aTrack = "", string aLabel = "",
            uint aDur_Min = 0, uint aDur_Max = 0, uint aBpm_Min = 0, uint aBpm_Max = 0, 
            uint aStart = 0, uint aCount = 100)
        {
            StringBuilder sb = new StringBuilder("q=");
            if (aArtist.Length > 0) sb.Append($"artist:\"{aArtist}\" ");
            if (aAlbum.Length > 0) sb.Append($"album:\"{aAlbum}\" ");
            if (aTrack.Length > 0) sb.Append($"track:\"{aTrack}\" ");
            if (aLabel.Length > 0) sb.Append($"label:\"{aLabel}\" ");
            if (aDur_Min > 0) sb.Append($"dur_min:\"{aDur_Min}\" ");
            if (aDur_Max > 0) sb.Append($"dur_max:\"{aDur_Max}\" ");
            if (aBpm_Min > 0) sb.Append($"bpm_min:\"{aBpm_Min}\" ");
            if (aBpm_Max > 0) sb.Append($"bpm_max:\"{aBpm_Max}\"");
            var aQuery = sb.ToString();

            return Get<Track, ITrack>(string.Empty, aQuery, aStart, aCount);
        }
        
        private Task<IEnumerable<TDest>> Get<TSource, TDest>(string aSearchEndpoint, string aQuery, uint aStart, uint aCount) where TSource : TDest, IDeserializable<IDeezerClient>
        {
            string method = (aSearchEndpoint.Length == 0) ? 
                    SEARCH_BASE : 
                    string.Format("{0}/{1}", SEARCH_BASE, aSearchEndpoint);

            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("q", aQuery)
            };

            return iClient.Get<TSource>(method, parms, aStart, aCount)
                .ContinueWith<IEnumerable<TDest>>((aTask) =>
                {
                    return iClient.Transform<TSource, TDest>(aTask.Result);
                }, iClient.CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }

    }
}
