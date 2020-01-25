using System;
using System.Collections.Generic;
using System.Text;

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using E.Deezer.Endpoints;

namespace E.Deezer
{
    public interface IDeezerSession : IDisposable
    {
        // Authentication stuff...
        ulong CurrentUserId { get; }
        bool IsAuthenticated { get; }

        Task<bool> Login(string accessToken, CancellationToken cancellationToken);
        Task<bool> Logout(CancellationToken cancellationToken);



        // References to the Client...
        IAlbumEndpoint Albums { get; }
        IArtistEndpoint Artists { get; }
        IChartsEndpoint Charts { get; }
        ICommentsEndpoint Comments { get; }
        IGenreEndpoint Genre { get; }
        IPlaylistsEndpoint Playlists { get; }
        IRadioEndpoint Radio { get; }
        ITrackEndpoint Tracks { get; }
        IUserEndpoint User { get; }
        ISearchEndpoint Search { get; }
    }


    // Internal interface used by internal objects to allow a session 
    // reference to be held by individual objects and therefore expose
    // methods on that object without duplicating code!
    internal interface IClientObject
    {
        IDeezerClient Client { get; }
    }


    //TODO: If we made all the client methods extensions from it with 
    //      namespace opt-in, then we could intheory get rid of this class
    //      as it really only forwards methods onto other class(es)....
    public class DeezerSession : IDeezerSession
    {
        private readonly DeezerClient client;

        public DeezerSession(HttpMessageHandler handler)
        {
            this.client = new DeezerClient(handler);
        }

        //Readonly references to all the various endpoints.
        //Might create a wrapper class to house them all and have this class
        //forward all it's properties onto that object instead!


        internal IDeezerClient Client => this.client;

        // IDeezerSession
        // Endpoints
        public IAlbumEndpoint Albums => this.client.Endpoints.Albums;

        public IArtistEndpoint Artists => this.client.Endpoints.Artists;

        public IChartsEndpoint Charts => this.client.Endpoints.Charts;

        public ICommentsEndpoint Comments => this.client.Endpoints.Comments;

        public IGenreEndpoint Genre => this.client.Endpoints.Genre;

        public IPlaylistsEndpoint Playlists => this.client.Endpoints.Playlists;

        public IRadioEndpoint Radio => this.client.Endpoints.Radio;

        public ITrackEndpoint Tracks => this.client.Endpoints.Track;

        public IUserEndpoint User => this.client.Endpoints.User;

        public ISearchEndpoint Search => this.client.Endpoints.Search;


        // Authentication
        public bool IsAuthenticated => this.client.IsAuthenticated;

        public ulong CurrentUserId => this.client.CurrentUserId;


        public Task<bool> Login(string accessToken, CancellationToken cancellationToken)
            => this.client.Login(accessToken, cancellationToken);

        public Task<bool> Logout(CancellationToken cancellationToken)
            => this.client.Logout(cancellationToken);



        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.client.Dispose();
            }
        }
    }
}
