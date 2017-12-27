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

        public Chart(IEnumerable<IAlbum> aAlbums, IEnumerable<IArtist> aArtists, IEnumerable<ITrack> aTracks, IEnumerable<IPlaylist> aPlaylists)
        {
            iAlbums = aAlbums;
            iArtists = aArtists;
            iTracks = aTracks;
            iPlaylists = aPlaylists;
        }


        public IEnumerable<IAlbum> Albums => iAlbums;

        public IEnumerable<IArtist> Artists => iArtists;

        public IEnumerable<ITrack> Tracks => iTracks;

        public IEnumerable<IPlaylist> Playlists => iPlaylists;


        //IDeserializable
        public DeezerClient Client
        {
            get;
            set;
        }

        public void Deserialize(DeezerClient aClient)
        {
            Client = aClient;

            DeserializeEnumerable(aClient, iAlbums.Select((v) => v as IDeserializable<DeezerClient>));
            DeserializeEnumerable(aClient, iArtists.Select((v) => v as IDeserializable<DeezerClient>));
            DeserializeEnumerable(aClient, iTracks.Select((v) => v as IDeserializable<DeezerClient>));
            DeserializeEnumerable(aClient, iPlaylists.Select((v) => v as IDeserializable<DeezerClient>));
        }

        private void DeserializeEnumerable(DeezerClient aClient, IEnumerable<IDeserializable<DeezerClient>> aEnumerable)
        {
            foreach(var entry in aEnumerable)
            {
                entry.Deserialize(aClient);
            }
        }



        public override string ToString()
        {
            return string.Format("E.Deezer.Chart : \n Albums :: {0} \n Artists :: {1} \n Tracks :: {2} \n Playlists :: {3}", iAlbums.Count(),
                                                                                                                             iArtists.Count(),
                                                                                                                             iTracks.Count(),
                                                                                                                             iPlaylists.Count());
        }
    }
}
