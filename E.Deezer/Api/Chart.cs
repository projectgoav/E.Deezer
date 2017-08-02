using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E.Deezer.Api
{
    public interface IChart
    {
        IEnumerable<IAlbum> Albums          { get; }
        IEnumerable<IArtist> Artists        { get; }
        IEnumerable<ITrack> Tracks          { get; }
        IEnumerable<IPlaylist> Playlists    { get; }
    }


    internal class Chart : IChart, IDeserializable<DeezerClient>
    {
        private readonly IEnumerable<IAlbum> iAlbums;
        private readonly IEnumerable<IArtist> iArtists;
        private readonly IEnumerable<ITrack> iTracks;
        private readonly IEnumerable<IPlaylist> iPlaylists;

        private DeezerClient iClient;

        public Chart(IEnumerable<IAlbum> aAlbums, IEnumerable<IArtist> aArtists, IEnumerable<ITrack> aTracks, IEnumerable<IPlaylist> aPlaylists)
        {
            iAlbums = aAlbums;
            iArtists = aArtists;
            iTracks = aTracks;
            iPlaylists = aPlaylists;
        }


        public IEnumerable<IAlbum> Albums
        {
            get { return iAlbums; }
        }

        public IEnumerable<IArtist> Artists
        {
            get { return iArtists; }
        }

        public IEnumerable<ITrack> Tracks
        {
            get { return iTracks; }
        }

        public IEnumerable<IPlaylist> Playlists
        {
            get { return iPlaylists; }
        }



        public void Deserialize(DeezerClient aClient)
        {
            iClient = aClient;

            DeserializeEnumerable(aClient, iAlbums.Select((v) => v as IDeserializable<DeezerClient>));
            DeserializeEnumerable(aClient, iArtists.Select((v) => v as IDeserializable<DeezerClient>));
            DeserializeEnumerable(aClient, iTracks.Select((v) => v as IDeserializable<DeezerClient>));
            DeserializeEnumerable(aClient, iPlaylists.Select((v) => v as IDeserializable<DeezerClient>));
        }

        public DeezerClient Client
        {
            get { return iClient; }
        }


        private void DeserializeEnumerable(DeezerClient aClient, IEnumerable<IDeserializable<DeezerClient>> aEnumerable)
        {
            foreach(var entry in aEnumerable)
            {
                entry.Deserialize(aClient);
            }
        }
    }
}
