using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E.Deezer.Endpoint
{
    public interface IBrowseEndpoint
    {
        //IAlbumEndpoint Albums { get; }
        //IArtistEndpoint Artists { get; }
        //IPlaylistEndpoint Playlists { get; }
        //ITrackEndpoint Tracks { get; }

        IGenreEndpoint Genre { get; }
        IChartsEndpoint Charts { get; }

        //TODO
        //IUserEndpoint User { get; }


    }

    internal class BrowseEndpoint : IBrowseEndpoint
    {
        private readonly IAlbumEndpoint iAlbums;
        private readonly IArtistEndpoint iArtists;
        private readonly IPlaylistEndpoint iPlaylists;
        private readonly ITrackEndpoint iTracks;
        private readonly IGenreEndpoint iGenre;
        private readonly IChartsEndpoint iCharts;

        private DeezerClient iClient;

        public IAlbumEndpoint Albums => iAlbums;

        public IArtistEndpoint Artists => iArtists;

        public IPlaylistEndpoint Playlists => iPlaylists;

        public ITrackEndpoint Tracks =>  iTracks;

        public IGenreEndpoint Genre => iGenre;

        public IChartsEndpoint Charts => iCharts;


        public BrowseEndpoint(DeezerClient aClient)
        {
            iClient = aClient;

            iAlbums = new AlbumEndpoint(iClient);
            iArtists = new ArtistEndpoint(iClient);
            iPlaylists = new PlaylistEndpoint(iClient);
            iTracks = new TrackEndpoint(iClient);
            iGenre = new GenreEndpoint(iClient);
            iCharts = new ChartsEndpoint(iClient);
        }
    }
}
