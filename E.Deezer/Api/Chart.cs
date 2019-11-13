using System.Collections.Generic;
using System.Linq;

namespace E.Deezer.Api
{
    public interface IChart
    {
        IEnumerable<IAlbum> Albums          { get; }
        IEnumerable<IArtist> Artists        { get; }
        IEnumerable<ITrack> Tracks          { get; }
        IEnumerable<IPlaylist> Playlists    { get; }
    }

    internal class Chart : IChart, IDeserializable<IDeezerClient>
    {
        public Chart(IEnumerable<IAlbum> aAlbums, IEnumerable<IArtist> aArtists, IEnumerable<ITrack> aTracks, IEnumerable<IPlaylist> aPlaylists)
        {
            Albums = aAlbums;
            Artists = aArtists;
            Tracks = aTracks;
            Playlists = aPlaylists;
        }

        public IEnumerable<IAlbum> Albums { get; }

        public IEnumerable<IArtist> Artists { get; }

        public IEnumerable<ITrack> Tracks { get; }

        public IEnumerable<IPlaylist> Playlists { get; }

        //IDeserializable
        public IDeezerClient Client { get; set; }

        public void Deserialize(IDeezerClient aClient)
        {
            Client = aClient;

            DeserializeEnumerable(aClient, Albums.Select((v) => v as IDeserializable<IDeezerClient>));
            DeserializeEnumerable(aClient, Artists.Select((v) => v as IDeserializable<IDeezerClient>));
            DeserializeEnumerable(aClient, Tracks.Select((v) => v as IDeserializable<IDeezerClient>));
            DeserializeEnumerable(aClient, Playlists.Select((v) => v as IDeserializable<IDeezerClient>));
        }

        private void DeserializeEnumerable(IDeezerClient aClient, IEnumerable<IDeserializable<IDeezerClient>> aEnumerable)
        {
            foreach (var entry in aEnumerable)
            {
                entry.Deserialize(aClient);
            }
        }

        public override string ToString()
        {
            return string.Format("E.Deezer.Chart : \n Albums :: {0} \n Artists :: {1} \n Tracks :: {2} \n Playlists :: {3}", Albums.Count(),
                                                                                                                             Artists.Count(),
                                                                                                                             Tracks.Count(),
                                                                                                                             Playlists.Count());
        }
    }
}
