using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using RestSharp.Deserializers;

namespace E.Deezer.Api
{
    /// <summary>
    /// Represents an Artist in the Deezer Library
    /// </summary>
    public interface IArtist
    {
        /// <summary>
        /// Deezer ID of this Artist
        /// </summary>
        uint Id { get; set; }

        /// <summary>
        /// Gets the name of this Artist
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// www.deezer.com link to this Artist
        /// </summary>
        string Link { get; set; }

        /// <summary>
        /// Gets the link to the artwork of this Artist
        /// </summary>
        string Artwork { get; set; }

        //Methods
        Task<IEnumerable<ITrack>> GetTracklist();
        Task<IEnumerable<ITrack>> GetTracklist(uint aCount);
        Task<IEnumerable<ITrack>> GetTracklist(uint aStart, uint aCount);

        Task<IEnumerable<ITrack>> GetTopTracks();
        Task<IEnumerable<ITrack>> GetTopTracks(uint aCount);
        Task<IEnumerable<ITrack>> GetTopTracks(uint aStart, uint aCount);

        Task<IEnumerable<IAlbum>> GetAlbums();
        Task<IEnumerable<IAlbum>> GetAlbums(uint aCount);
        Task<IEnumerable<IAlbum>> GetAlbums(uint aStart, uint aCount);

        Task<IEnumerable<IArtist>> GetRelated();
        Task<IEnumerable<IArtist>> GetRelated(uint aCount);
        Task<IEnumerable<IArtist>> GetRelated(uint aStart, uint aCount);

        Task<IEnumerable<IPlaylist>> GetPlaylistsContaining();
        Task<IEnumerable<IPlaylist>> GetPlaylistsContaining(uint aCount);
        Task<IEnumerable<IPlaylist>> GetPlaylistsContaining(uint aStart, uint aCount);

    }

    public class Artist : IArtist, IDeserializable<DeezerClientV2>
    {
        public uint Id { get; set; }
        public string Name { get; set; }

        [DeserializeAs(Name="url")]
        public string Link { get; set; }

        [DeserializeAs(Name="picture")]
        public string Artwork { get; set; }


        public DeezerClientV2 Client { get; set; }
        public void Deserialize(DeezerClientV2 aClient) { Client = aClient; }


        public Task<IEnumerable<ITrack>> GetTracklist() {  return GetTracklist(0, DeezerSessionV2.DEFAULT_SIZE); }
        public Task<IEnumerable<ITrack>> GetTracklist(uint aCount)  { return GetTracklist(0, aCount); }
        public Task<IEnumerable<ITrack>> GetTracklist(uint aStart, uint aCount) { return Get<Track, ITrack>("artist/{id}/radio", aStart, aCount); }


        public Task<IEnumerable<ITrack>> GetTopTracks() { return GetTopTracks(0, DeezerSessionV2.DEFAULT_SIZE); }
        public Task<IEnumerable<ITrack>> GetTopTracks(uint aCount) { return GetTopTracks(0, aCount); }
        public Task<IEnumerable<ITrack>> GetTopTracks(uint aStart, uint aCount) { return Get<Track, ITrack>("artist/{id}/top", aStart, aCount); }


        public Task<IEnumerable<IAlbum>> GetAlbums() { return GetAlbums(0, DeezerSessionV2.DEFAULT_SIZE); }
        public Task<IEnumerable<IAlbum>> GetAlbums(uint aCount)  {  return GetAlbums(0, aCount); }
        public Task<IEnumerable<IAlbum>> GetAlbums(uint aStart, uint aCount) { return Get<Album, IAlbum>("/artist/{id}/albums", aStart, aCount); }


        public Task<IEnumerable<IArtist>> GetRelated() {  return GetRelated(0, DeezerSessionV2.DEFAULT_SIZE); }
        public Task<IEnumerable<IArtist>> GetRelated(uint aCount) { return GetRelated(0, aCount); }
        public Task<IEnumerable<IArtist>> GetRelated(uint aStart, uint aCount) { return Get<Artist, IArtist>("artist/{id}/related", aStart, aCount); }

        public Task<IEnumerable<IPlaylist>> GetPlaylistsContaining() {  return GetPlaylistsContaining(0, DeezerSessionV2.DEFAULT_SIZE); }
        public Task<IEnumerable<IPlaylist>> GetPlaylistsContaining(uint aCount) {  return GetPlaylistsContaining(0, aCount); }
        public Task<IEnumerable<IPlaylist>> GetPlaylistsContaining(uint aStart, uint aCount) {  return Get<Playlist, IPlaylist>("artist/{id}/playlists", aStart, aCount); }


        //Internal wrapper around get for all artist methods :)
        private Task<IEnumerable<TDest>> Get<TSource, TDest>(string aMethod, uint aStart, uint aCount) where TSource : TDest, IDeserializable<DeezerClientV2>
        {
            string[] parms = new string[] { "URL", "id", Id.ToString() };
            return Client.Get<TSource>(aMethod, parms, aStart, aCount).ContinueWith<IEnumerable<TDest>>((aTask) =>
            {
                List<TDest> items = new List<TDest>();

                foreach(var item in aTask.Result.Items)
                {
                    item.Deserialize(Client);
                    items.Add(item);
                }
                return items;
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }


        public override string ToString()
        {
            return string.Format("E.Deezer: Artist({0})", Name);
        }
    }
}
