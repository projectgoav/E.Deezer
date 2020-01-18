using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json.Linq;

using E.Deezer.Api.Internal;

namespace E.Deezer.Api
{
    public interface IChart
    {
        IEnumerable<IAlbum> Albums          { get; }
        IEnumerable<IArtist> Artists        { get; }
        IEnumerable<ITrack> Tracks          { get; }
        IEnumerable<IPlaylist> Playlists    { get; }
    }


    internal class Chart : IChart
    {
        public IEnumerable<IAlbum> Albums { get; private set; }

        public IEnumerable<IArtist> Artists { get; private set; }

        public IEnumerable<ITrack> Tracks { get; private set; }

        public IEnumerable<IPlaylist> Playlists { get; private set; }


        public override string ToString()
        {
            return string.Format("E.Deezer.Chart : \n Albums :: {0} \n Artists :: {1} \n Tracks :: {2} \n Playlists :: {3}", this.Albums.Count(),
                                                                                                                             this.Artists.Count(),
                                                                                                                             this.Tracks.Count(),
                                                                                                                             this.Playlists.Count());
        }


        //JSON
        internal const string ALBUMS_PROPERTY_NAME = "albums";
        internal const string ARTISTS_PROPERTY_NAME = "artists";
        internal const string TRACKS_PROPERTY_NAME = "tracks";
        internal const string PLAYLISTS_PROPERTY_NAME = "playlists";

        public static IChart FromJson(JToken json, IDeezerClient client)
        {
            return new Chart()
            {
                Albums = FragmentOf<IAlbum>.FromJson(json[ALBUMS_PROPERTY_NAME], x => Api.Album.FromJson(x, client)),
                Artists = FragmentOf<IArtist>.FromJson(json[ARTISTS_PROPERTY_NAME], x => Api.Artist.FromJson(x, client)),
                Tracks = FragmentOf<ITrack>.FromJson(json[TRACKS_PROPERTY_NAME], x => Api.Track.FromJson(x, client)),
                Playlists = FragmentOf<IPlaylist>.FromJson(json[PLAYLISTS_PROPERTY_NAME], x => Api.Playlist.FromJson(x, client)),
            };
        }
    }
}
