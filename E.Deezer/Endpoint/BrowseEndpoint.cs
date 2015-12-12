using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E.Deezer.Endpoint
{
    public interface IBrowseEndpoint
    {
        IAlbumEndpoint Albums { get; }
        IArtistEndpoint Artists { get; }
        IPlaylistEndpoint Playlists { get; }
        ITrackEndpoint Tracks { get; }

        //TODO
        //IUserEndpoint User { get; }


    }

    internal class BrowseEndpoint : IBrowseEndpoint
    {
        private readonly IAlbumEndpoint iAlbums;
        private readonly IArtistEndpoint iArtists;
        private readonly IPlaylistEndpoint iPlaylists;
        private readonly ITrackEndpoint iTracks;

        private DeezerClientV2 iClient;

        public IAlbumEndpoint Albums { get { return iAlbums; } }
        public IArtistEndpoint Artists { get { return iArtists; } }
        public IPlaylistEndpoint Playlists { get { return iPlaylists; } }
        public ITrackEndpoint Tracks { get { return iTracks; } }

        public BrowseEndpoint(DeezerClientV2 aClient)
        {
            iClient = aClient;

            iAlbums = new AlbumEndpoint(iClient);
            iArtists = new ArtistEndpoint(iClient);
            iPlaylists = new PlaylistEndpoint(iClient);
            iTracks = new TrackEndpoint(iClient);
        }
    }
}
