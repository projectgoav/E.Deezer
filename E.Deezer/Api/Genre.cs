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

        /// <summary>
        /// Returns a list of artists associated with this Genre
        /// </summary>
        /// <returns>A book of artists associated with this genre</returns>
        Task<IBook<IArtist>> GetArtists();

        /// <summary>
        /// Returns the Deezer Selection for this Genre
        /// </summary>
        /// <returns>A book of the Deezer Selection for this Genre</returns>
        Task<IBook<IAlbum>> GetSelection();

        /// <summary>
        /// Returns the new releases for this Genre
        /// </summary>
        /// <returns>A book of the new releases associated with this Genre</returns>
        Task<IBook<IAlbum>> GetReleases();


        //Charts!

        Task<IBook<IAlbum>> GetAlbumChart();

        Task<IBook<IArtist>> GetArtistChart();

        Task<IBook<IPlaylist>> GetPlaylistChart();

        Task<IBook<ITrack>> GetTrackChart(); 



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


        public Task<IBook<IArtist>> GetArtists() { throw new NotImplementedException(); }

        public Task<IBook<IAlbum>> GetSelection() { throw new NotImplementedException(); }

        public Task<IBook<IAlbum>> GetReleases() { throw new NotImplementedException();}


        //Charting

        public Task<IBook<IAlbum>> GetAlbumChart() { throw new NotImplementedException(); }

        public Task<IBook<IArtist>> GetArtistChart() { throw new NotImplementedException(); }

        public Task<IBook<IPlaylist>> GetPlaylistChart() { throw new NotImplementedException(); }

        public Task<IBook<ITrack>> GetTrackChart() { throw new NotImplementedException(); }


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
