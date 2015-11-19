using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RestSharp.Deserializers;

namespace E.Deezer.Api
{
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


    internal class Genre : IGenre
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


        public override string ToString()
        {
            return string.Format("E.Deezer: Genre({0} ({1}))", Name, Id);
        }
    }
}
