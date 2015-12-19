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
        private DeezerSession iSession;
        private IBrowseEndpoint iBrowse;
        private ISearchEndpoint iSearch;
        private IUserEndpoint iUser;
        private DeezerClient iClient;

        internal Deezer(DeezerSession aSession)
        {
            iSession = aSession;
            iClient = new DeezerClient(iSession);

            iBrowse = new BrowseEndpoint(iClient);
            iSearch = new SearchEndpoint(iClient);
            iUser =   new UserEndpoint(iClient);
        }

        public IBrowseEndpoint Browse { get { return iBrowse; } }
        public ISearchEndpoint Search { get { return iSearch; } }
        public IUserEndpoint   User   { get { return iUser; } }


        public Task<IInfos> GetServiceInformation()
        {
           return iClient.Get<Infos>("infos").ContinueWith<IInfos>((aTask) => { return aTask.Result; }, iClient.Token, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
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

    /// <summary>
    /// Defines the Picutre size you can obtain from Deezer
    /// </summary>
    public enum PictureSize
    {
        SMALL,
        MEDIUM,
        LARGE,
    };


}
