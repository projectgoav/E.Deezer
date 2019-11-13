using E.Deezer.Api;
using E.Deezer.Endpoint;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace E.Deezer
{
    /// <summary>
    /// Provides access to the Deezer API
    /// </summary>
    public class Deezer : IDisposable
    {
        private readonly DeezerSession _session;
        private readonly DeezerClient _client;

        internal Deezer(DeezerSession session, HttpMessageHandler httpMessageHandler = null)
        {
            _session = session;
            _client = new DeezerClient(_session, httpMessageHandler);

            Browse = new BrowseEndpoint(_client);
            Search = new SearchEndpoint(_client);
            User   = new   UserEndpoint(_client);
            Radio  = new  RadioEndpoint(_client);
        }

        public IBrowseEndpoint Browse { get; }
        public ISearchEndpoint Search { get; }
        public IUserEndpoint User { get; }
        public IRadioEndpoint Radio { get; }

        public Task<IServceInfo> GetServiceInformation()
        {
           return _client.GetPlain<Infos>("infos")
                         .ContinueWith<IServceInfo>((aTask) => { return aTask.Result; }, _client.CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }

        //'OAuth'
        public bool IsAuthenticated => _session.Authenticated;

        public Task Login(string aAccessToken)
        {
            _session.Login(aAccessToken);
            return _client.Login(); //Obtaining the permissions this token grants E.Deezer
        }

        public void Logout() => _session.Logout();

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
