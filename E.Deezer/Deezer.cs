using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using E.Deezer.Api;
using E.Deezer.Endpoint;

namespace E.Deezer
{
    /// <summary>
    /// Provides access to the Deezer API
    /// </summary>
    public class Deezer : IDisposable
    {
        private IDeezerSession iSession;
        private IBrowseEndpoint iBrowse;
        private ISearchEndpoint iSearch;
        private DeezerClientV2 iClient;

        internal Deezer(IDeezerSession aSession)
        {
            iSession = aSession;
            iClient = new DeezerClientV2(null);   //TODO FIX

            iBrowse = new BrowseEndpoint(iClient);
            iSearch = new SearchEndpoint(iClient);
        }

        public IBrowseEndpoint Browse { get { return iBrowse; } }
        public ISearchEndpoint Search { get { return iSearch; } }


        public Task<IInfos> GetServiceInformation()
        {
           return iClient.Get<Infos>("infos").ContinueWith<IInfos>((aTask) => { return aTask.Result; }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }
        

        public void Dispose() {  iClient.Dispose(); }
    }
}
