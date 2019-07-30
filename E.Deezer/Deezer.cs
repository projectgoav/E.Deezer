﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;

using E.Deezer.Api;
using E.Deezer.Endpoint;

namespace E.Deezer
{
    /// <summary>
    /// Provides access to the Deezer API
    /// </summary>
    public class Deezer : IDisposable
    {
        private readonly IBrowseEndpoint iBrowse;
        private readonly ISearchEndpoint iSearch;
        private readonly IUserEndpoint iUser;
        private readonly IRadioEndpoint iRadio;
        private readonly DeezerSession iSession;
        private readonly DeezerClient iClient;

        internal Deezer(DeezerSession aSession, HttpMessageHandler httpMessageHandler = null)
        {
            iSession = aSession;
            iClient = new DeezerClient(iSession, httpMessageHandler);

            iBrowse = new BrowseEndpoint(iClient);
            iSearch = new SearchEndpoint(iClient);
            iUser =   new UserEndpoint(iClient);
            iRadio = new RadioEndpoint(iClient);
        }

        public IBrowseEndpoint Browse =>iBrowse; 
        public ISearchEndpoint Search => iSearch;
        public IUserEndpoint   User   => iUser;
        public IRadioEndpoint  Radio  => iRadio; 


        public Task<IServceInfo> GetServiceInformation()
        {
           return iClient.GetPlain<Infos>("infos")
                         .ContinueWith<IServceInfo>((aTask) => { return aTask.Result; }, iClient.CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }

        //'OAuth'
        public bool IsAuthenticated => iSession.Authenticated;

        public Task Login(string aAccessToken) 
        {
            iSession.Login(aAccessToken);
            return iClient.Login(); //Obtaining the permissions this token grants E.Deezer
        }

        public void Logout() => iSession.Logout();



        public void Dispose()
        {
            iClient.Dispose();
        }
    }


}
