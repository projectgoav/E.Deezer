using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E.Deezer.Endpoint
{
    public interface IDeezerEndpoint
    {
        IArtistEndpoint Artists { get; }
        IAlbumEndpoint Albums { get; }
        IPlaylistEndpoint Playlists { get; }
        ITrackEndpoint Tracks { get; }

        IGenreEndpoint Genre { get; }

        ISearchEndpoint Search { get; }

        //TODO
        //IUserEndpoint User { get; }

    }

    class DeezerEndpoint
    {
    }
}
