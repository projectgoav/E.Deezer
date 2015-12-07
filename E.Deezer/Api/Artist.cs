using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace E.Deezer.Api
{
    /// <summary>
    /// A Deezer artist object
    /// </summary>
    public interface IArtist
    {
        /// <summary>
        /// Deezer library ID number
        /// </summary>
        uint Id { get; set; }

        /// <summary>
        /// Artist's name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Deezer.com link to artist
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// Link to artist's image
        /// </summary>
        string Picture { get; set; }

        /// <summary>
        /// Link to artist tracklist
        /// </summary>
        string Tracklist { get; set; }

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
        Task<IEnumerable<IPlaylist>> GetPlaylistsContaining(int aStart, uint aCount);

    }

    public class Artist : IArtist, IDeserializable<DeezerClientV2>
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Picture { get; set; }
        public string Tracklist { get; set; }


        public DeezerClientV2 Client { get; set; }
        public void Deserialize(DeezerClientV2 aClient) { Client = aClient; }


        public Task<IEnumerable<ITrack>> GetTracklist() {  return GetTracklist(0, DeezerSessionV2.DEFAULT_SIZE); }
        public Task<IEnumerable<ITrack>> GetTracklist(uint aCount)  { return GetTracklist(0, aCount); }

        public Task<IEnumerable<ITrack>> GetTracklist(uint aStart, uint aCount)
        {
            string[] parms = new string[] { "URL", "id", Id.ToString() };

            return Client.Get<Track>("artist/{id}/radio", parms, aStart, aCount).ContinueWith<IEnumerable<ITrack>>((aTask) =>
            {
               return aTask.Result.Items;
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }





        public Task<IEnumerable<ITrack>> GetTopTracks()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ITrack>> GetTopTracks(uint aCount)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ITrack>> GetTopTracks(uint aStart, uint aCount)
        {
            throw new NotImplementedException();
        }





        public Task<IEnumerable<IAlbum>> GetAlbums()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IAlbum>> GetAlbums(uint aCount)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IAlbum>> GetAlbums(uint aStart, uint aCount)
        {
            throw new NotImplementedException();
        }




        public Task<IEnumerable<IArtist>> GetRelated()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IArtist>> GetRelated(uint aCount)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IArtist>> GetRelated(uint aStart, uint aCount)
        {
            throw new NotImplementedException();
        }




        public Task<IEnumerable<IPlaylist>> GetPlaylistsContaining()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IPlaylist>> GetPlaylistsContaining(uint aCount)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IPlaylist>> GetPlaylistsContaining(int aStart, uint aCount)
        {
            throw new NotImplementedException();
        }


        public override string ToString()
        {
            return string.Format("E.Deezer: Artist({0})", Name);
        }
    }
}
