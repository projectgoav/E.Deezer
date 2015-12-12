using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using RestSharp.Deserializers;

namespace E.Deezer.Api
{
    #region Genre

    /// <summary>
    /// A Deezer Genre Object
    /// </summary>
    public interface IGenre
    {
        /// <summary>
        /// Deezer library ID number
        /// </summary>
        uint Id { get; set; }

        /// <summary>
        /// Genre name
        /// </summary>
        string Name { get; set; }

        //METHODS

        /// <summary>
        /// Gets Genre's image
        /// </summary>
        /// <param name="aSize">Requested image size</param>
        /// <returns>Url to image, or empty string if no such image exists.</returns>
        string GetPicture(PictureSize aSize);

        /// <summary>
        /// Gets if the Genre has the specified image
        /// </summary>
        /// <param name="aSize">Requested image size</param>
        /// <returns>True if picture exists</returns>
        bool HasPicture(PictureSize aSize);



        Task<IEnumerable<IArtist>> GetArtists(uint aCount);
        Task<IEnumerable<IArtist>> GetArtists(uint aStart, uint aCount);

        Task<IEnumerable<IAlbum>> GetSelection(uint aCount);
        Task<IEnumerable<IAlbum>> GetSelection(uint aStart, uint aCount);

        Task<IEnumerable<IAlbum>> GetReleases(uint aCount);
        Task<IEnumerable<IAlbum>> GetReleases(uint aStart, uint aCount);


        //Charts!

        Task<IEnumerable<IAlbum>> GetAlbumChart(uint aCount);
        Task<IEnumerable<IAlbum>> GetAlbumChart(uint aStart, uint aCount);

        Task<IEnumerable<IArtist>> GetArtistChart(uint aCount);
        Task<IEnumerable<IArtist>> GetArtistChart(uint aStart, uint aCount);

        Task<IEnumerable<IPlaylist>> GetPlaylistChart(uint aCount);
        Task<IEnumerable<IPlaylist>> GetPlaylistChart(uint aStart, uint aCount);

        Task<IEnumerable<ITrack>> GetTrackChart(uint aCount);
        Task<IEnumerable<ITrack>> GetTrackChart(uint aStart, uint aCount); 


        //TODO
        //Task<IBook<IPodcast>> GetPodcasts();

        //Task<IBook<IRadio>> GetRadios();

    }

    /// <summary>
    /// Defines a picture size for Deezer Genre
    /// </summary>
    public enum PictureSize
    {
        /// <summary>
        /// Small picture size -> 56x56 pixels
        /// </summary>
        SMALL,
 
        /// <summary>
        /// Medium picture size -> 250x250 pixels
        /// </summary>
        MEDIUM,
        
        /// <summary>
        /// Large picture size -> 500x500 pixels
        /// </summary>
        LARGE,
    };


    internal class Genre : IGenre, IDeserializable<DeezerClientV2>
    {
        public uint Id { get; set; }
        public string Name { get; set; }

        //Pictures
        [DeserializeAs(Name="picture_small")]
        private string SMPicture { get; set; }

        [DeserializeAs(Name = "picture_medium")]
        private string MDPicture { get; set; }

        [DeserializeAs(Name = "picture_big")]
        private string BGPicture { get; set; }


        public DeezerClientV2 Client { get; set; }
        public void Deserialize(DeezerClientV2 aClient) { Client = aClient; }


        //Methods

        public string GetPicture(PictureSize aSize)
        {
            switch(aSize)
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


        public Task<IEnumerable<IArtist>> GetArtists(uint aCount) { return GetArtists(0, aCount); }
        public Task<IEnumerable<IArtist>> GetArtists(uint aStart, uint aCount) { return Get<Artist, IArtist>("genre/{id}/artists", aStart, aCount); }

        public Task<IEnumerable<IAlbum>> GetSelection(uint aCount) { return GetSelection(0, aCount); }
        public Task<IEnumerable<IAlbum>> GetSelection(uint aStart, uint aCount) { return Get<Album, IAlbum>("genre/{id}/selection", aStart, aCount); }

        public Task<IEnumerable<IAlbum>> GetReleases(uint aCount) { return GetReleases(0, aCount); }
        public Task<IEnumerable<IAlbum>> GetReleases(uint aStart, uint aCount) { return Get<Album , IAlbum>("genre/{id}/releases", aStart, aCount); }

        //Charting

        public Task<IEnumerable<IAlbum>> GetAlbumChart(uint aCount) { return GetAlbumChart(0, aCount); }
        public Task<IEnumerable<IAlbum>> GetAlbumChart(uint aStart, uint aCount) { return Get<Album, IAlbum>("chart/{id}/albums", aStart, aCount); }

        public Task<IEnumerable<IArtist>> GetArtistChart(uint aCount) { return GetArtistChart(0, aCount); }
        public Task<IEnumerable<IArtist>> GetArtistChart(uint aStart, uint aCount) { return Get<Artist, IArtist>("chart/{id}/artists", aStart, aCount); }

        public Task<IEnumerable<IPlaylist>> GetPlaylistChart(uint aCount) { return GetPlaylistChart(0, aCount); }
        public Task<IEnumerable<IPlaylist>> GetPlaylistChart(uint aStart, uint aCount) { return Get<Playlist, IPlaylist>("chart/{id}/playlists", aStart, aCount); }

        public Task<IEnumerable<ITrack>> GetTrackChart(uint aCount) { return GetTrackChart(0, aCount); }
        public Task<IEnumerable<ITrack>> GetTrackChart(uint aStart, uint aCount) { return Get<Track, ITrack>("chart/{id}/tracks", aStart, aCount); }


        //Internal wrapper around get for all genre methods :)
        private Task<IEnumerable<TDest>> Get<TSource, TDest>(string aMethod, uint aStart, uint aCount) where TSource : TDest, IDeserializable<DeezerClientV2>
        {
            string[] parms = new string[] { "URL", "id", Id.ToString() };
            return Client.Get<TSource>(aMethod, parms, aStart, aCount).ContinueWith<IEnumerable<TDest>>((aTask) =>
            {
                List<TDest> items = new List<TDest>();

                foreach (var item in aTask.Result.Items)
                {
                    item.Deserialize(Client);
                    items.Add(item);
                }
                return items;
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }



        public override string ToString()
        {
            return string.Format("E.Deezer: Genre({0} ({1}))", Name, Id);
        }
    }

    #endregion

    #region GenreList

    /// <summary>
    /// Contains a list of Genre objects
    /// </summary>
    public interface IGenreList
    {
        /// <summary>
        /// List of the common genres in Deezer
        /// </summary>
        List<IGenre> Genre { get; }
    }

    internal class GenreList : IGenreList, IDeserializable<DeezerClient>
    {
        public List<IGenre> Genre
        {
            get { return new List<IGenre>(data);  }
        }

        //Since we need concreate classes for API call
        //We store them locally and only return the interfaces
        //Wont be exposed by interface, despite public.
        public List<Genre> data { get; set; }


        public DeezerClient Client { get; set; }

        public void Deserialize(DeezerClient aClient)
        { 
            Client = aClient; 

            //Make sure to put client referne in the Genre so users
            //can access genre methods.
            //foreach(IGenre g in Genre) { (g as Genre).Deserialize(aClient); }
        }
    }



    #endregion
}
