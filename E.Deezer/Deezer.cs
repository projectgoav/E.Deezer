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
        private string iVersion;

        private DeezerSession iSession;
        private readonly IBrowseEndpoint iBrowse;
        private readonly ISearchEndpoint iSearch;
        private readonly IUserEndpoint iUser;
        private readonly IRadioEndpoint iRadio;
        private readonly DeezerClient iClient;

        internal Deezer(DeezerSession aSession, bool underTest = false)
        {
            //iVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            iSession = aSession;
            if (underTest) { iClient = new DeezerClient(iSession, true); }
            else           { iClient = new DeezerClient(iSession); }
            

            iBrowse = new BrowseEndpoint(iClient);
            iSearch = new SearchEndpoint(iClient);
            iUser =   new UserEndpoint(iClient);
            iRadio = new RadioEndpoint(iClient);
        }

        public IBrowseEndpoint Browse { get { return iBrowse; } }
        public ISearchEndpoint Search { get { return iSearch; } }
        public IUserEndpoint   User   { get { return iUser; } }
        public IRadioEndpoint  Radio  { get { return iRadio; } }


        public Task<IInfos> GetServiceInformation()
        {
           return iClient.GetPlain<Infos>("infos").ContinueWith<IInfos>((aTask) => { return aTask.Result; }, iClient.CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }
        
        //'OAuth'
        public Task Login(string aAccessToken) 
        {
            iSession.Login(aAccessToken);
            return iClient.Login(); //Obtaining the permissions this token grants E.Deezer
        }
        public void Logout() { iSession.Logout(); }
        public bool IsAuthenticated { get { return iSession.Authenticated; } }

        public void Dispose() {  iClient.Dispose(); }
    }


}
