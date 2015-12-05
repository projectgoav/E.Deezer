using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using E.Deezer.Endpoint;

namespace E.Deezer
{
    /// <summary>
    /// Provides access to the Deezer API
    /// </summary>
    public class Deezer
    {
        private IDeezerSession iSession;
        private IBrowseEndpoint iBrowse;
        private ISearchEndpoint iSearch;
        private DeezerClient iClient;

        internal Deezer(IDeezerSession aSession)
        {
            iSession = aSession;
            iClient = new DeezerClient(null);   //TODO FIX

            iBrowse = new BrowseEndpoint(iClient);
            iSearch = new SearchEndpoint(iClient);
        }

        public IBrowseEndpoint Browse { get { return iBrowse; } }
        public ISearchEndpoint Search { get { return iSearch; } }
    }
}
