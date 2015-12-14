using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using RestSharp.Deserializers;

namespace E.Deezer.Api
{
    public interface IArtist
    {
        uint Id { get; set; }
        string Name { get; set; }
        string Link { get; set; }

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

        string GetPicture(PictureSize aSize);
        bool HasPicture(PictureSize aSize);
    }

    public class Artist : IArtist, IDeserializable<DeezerClient>
    {
        public uint Id { get; set; }
        public string Name { get; set; }

        [DeserializeAs(Name="url")]
        public string Link { get; set; }

        //Pictures
        [DeserializeAs(Name = "picture_small")]
        private string SMPicture { get; set; }

        [DeserializeAs(Name = "picture_medium")]
        private string MDPicture { get; set; }

        [DeserializeAs(Name = "picture_big")]
        private string BGPicture { get; set; }

        public string GetPicture(PictureSize aSize)
        {
            switch (aSize)
            {
                case PictureSize.SMALL: { return string.IsNullOrEmpty(SMPicture) ? string.Empty : SMPicture; }
                case PictureSize.MEDIUM: { return string.IsNullOrEmpty(MDPicture) ? string.Empty : MDPicture; }
                case PictureSize.LARGE: { return string.IsNullOrEmpty(BGPicture) ? string.Empty : BGPicture; }
                default: { return string.Empty; }
            }
        }

        public bool HasPicture(PictureSize aSize)
        {
            switch (aSize)
            {
                case PictureSize.SMALL: { return string.IsNullOrEmpty(SMPicture); }
                case PictureSize.MEDIUM: { return string.IsNullOrEmpty(MDPicture); }
                case PictureSize.LARGE: { return string.IsNullOrEmpty(BGPicture); }
                default: { return false; }
            }
        }

        public DeezerClient Client { get; set; }
        public void Deserialize(DeezerClient aClient) { Client = aClient; }


        public Task<IEnumerable<ITrack>> GetTracklist() {  return GetTracklist(0, Client.ResultSize); }
        public Task<IEnumerable<ITrack>> GetTracklist(uint aCount)  { return GetTracklist(0, aCount); }
        public Task<IEnumerable<ITrack>> GetTracklist(uint aStart, uint aCount) { return Get<Track, ITrack>("artist/{id}/radio", aStart, aCount); }


        public Task<IEnumerable<ITrack>> GetTopTracks() { return GetTopTracks(0, Client.ResultSize); }
        public Task<IEnumerable<ITrack>> GetTopTracks(uint aCount) { return GetTopTracks(0, aCount); }
        public Task<IEnumerable<ITrack>> GetTopTracks(uint aStart, uint aCount) { return Get<Track, ITrack>("artist/{id}/top", aStart, aCount); }


        public Task<IEnumerable<IAlbum>> GetAlbums() { return GetAlbums(0, Client.ResultSize); }
        public Task<IEnumerable<IAlbum>> GetAlbums(uint aCount)  {  return GetAlbums(0, aCount); }
        public Task<IEnumerable<IAlbum>> GetAlbums(uint aStart, uint aCount) { return Get<Album, IAlbum>("/artist/{id}/albums", aStart, aCount); }


        public Task<IEnumerable<IArtist>> GetRelated() { return GetRelated(0, Client.ResultSize); }
        public Task<IEnumerable<IArtist>> GetRelated(uint aCount) { return GetRelated(0, aCount); }
        public Task<IEnumerable<IArtist>> GetRelated(uint aStart, uint aCount) { return Get<Artist, IArtist>("artist/{id}/related", aStart, aCount); }

        public Task<IEnumerable<IPlaylist>> GetPlaylistsContaining() {  return GetPlaylistsContaining(0, DeezerSession.DEFAULT_SIZE); }
        public Task<IEnumerable<IPlaylist>> GetPlaylistsContaining(uint aCount) {  return GetPlaylistsContaining(0, aCount); }
        public Task<IEnumerable<IPlaylist>> GetPlaylistsContaining(uint aStart, uint aCount) {  return Get<Playlist, IPlaylist>("artist/{id}/playlists", aStart, aCount); }


        //Internal wrapper around get for all artist methods :)
        private Task<IEnumerable<TDest>> Get<TSource, TDest>(string aMethod, uint aStart, uint aCount) where TSource : TDest, IDeserializable<DeezerClient>
        {     
            string[] parms = new string[] { "URL", "id", Id.ToString() };

            return Client.Get<TSource>(aMethod, parms, aStart, aCount).ContinueWith<IEnumerable<TDest>>((aTask) =>
            {
                return Client.Transform<TSource, TDest>(aTask.Result);
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }


        public override string ToString()
        {
            return string.Format("E.Deezer: Artist({0})", Name);
        }
    }
}
