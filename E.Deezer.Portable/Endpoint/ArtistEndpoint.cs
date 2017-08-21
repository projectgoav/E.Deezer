using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using E.Deezer.Api;

namespace E.Deezer.Endpoint
{
    public interface IArtistEndpoint
    {
        //Task<IEnumerable<ITrack>> GetTracklist();
        //Task<IEnumerable<ITrack>> GetTracklist(uint aCount);
        //Task<IEnumerable<ITrack>> GetTracklist(uint aStart, uint aCount);

        //Task<IEnumerable<ITrack>> GetTopTracks();
        //Task<IEnumerable<ITrack>> GetTopTracks(uint aCount);
        //Task<IEnumerable<ITrack>> GetTopTracks(uint aStart, uint aCount);

        //Task<IEnumerable<IAlbum>> GetAlbums();
        //Task<IEnumerable<IAlbum>> GetAlbums(uint aCount);
        //Task<IEnumerable<IAlbum>> GetAlbums(uint aStart, uint aCount);

        //Task<IEnumerable<IArtist>> GetRelated();
        //Task<IEnumerable<IArtist>> GetRelated(uint aCount);
        //Task<IEnumerable<IArtist>> GetRelated(uint aStart, uint aCount);

        //Task<IEnumerable<IPlaylist>> GetPlaylistsContaining();
        //Task<IEnumerable<IPlaylist>> GetPlaylistsContaining(uint aCount);
        //Task<IEnumerable<IPlaylist>> GetPlaylistsContaining(int aStart, uint aCount);
    }

    internal class ArtistEndpoint : IArtistEndpoint
    {
        private DeezerClient iClient;


        public ArtistEndpoint(DeezerClient aClient) {  iClient = aClient;  }
    }
}
