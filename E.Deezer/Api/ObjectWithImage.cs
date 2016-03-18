using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RestSharp.Deserializers;

namespace E.Deezer.Api
{
    public enum PictureSize
    {
        SMALL,
        MEDIUM,
        LARGE,
    };

    public interface IObjectWithImage
    {
        string GetPicture(PictureSize aSize);
        bool HasPicture(PictureSize aSize);
    }

    internal class ObjectWithImage : IObjectWithImage
    {
        [DeserializeAs(Name = "picture_small")]
        private string SmallPicture { get; set; }

        [DeserializeAs(Name = "picture_medium")]
        private string MediumPicture { get; set; }

        [DeserializeAs(Name = "picture_big")]
        private string LargePicture { get; set; }



        public string GetPicture(PictureSize aSize)
        {
            switch (aSize)
            {
                case PictureSize.SMALL: { return string.IsNullOrEmpty(SmallPicture) ? string.Empty : SmallPicture; }
                case PictureSize.MEDIUM: { return string.IsNullOrEmpty(MediumPicture) ? string.Empty : MediumPicture; }
                case PictureSize.LARGE: { return string.IsNullOrEmpty(LargePicture) ? string.Empty : LargePicture; }
                default: { return string.Empty; }
            }
        }

        public bool HasPicture(PictureSize aSize)
        {
            switch (aSize)
            {
                case PictureSize.SMALL: { return string.IsNullOrEmpty(SmallPicture); }
                case PictureSize.MEDIUM: { return string.IsNullOrEmpty(MediumPicture); }
                case PictureSize.LARGE: { return string.IsNullOrEmpty(LargePicture); }
                default: { return false; }
            }
        }
    }
}
