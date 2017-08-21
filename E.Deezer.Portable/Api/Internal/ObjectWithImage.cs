using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

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
        [JsonProperty(PropertyName = "picture_small")]
        public string SmallPicture { get; set; }

        [JsonProperty(PropertyName = "picture_medium")]
        public string MediumPicture { get; set; }

        [JsonProperty(PropertyName = "picture_big")]
        public string LargePicture { get; set; }

        [JsonProperty(PropertyName = "cover_small")]
        public string SmallCover { get; set; }

        [JsonProperty(PropertyName = "cover_medium")]
        public string MediumCover { get; set; }

        [JsonProperty(PropertyName = "cover_big")]
        public string LargeCover { get; set; }



        public virtual string GetPicture(PictureSize aSize)
        {
            switch (aSize)
            {
                case PictureSize.SMALL:     { return GetImage(SmallPicture, SmallCover); }
                case PictureSize.MEDIUM:    { return GetImage(MediumPicture, MediumCover); }
                case PictureSize.LARGE:     { return GetImage(LargePicture, LargeCover); }
                default:                    { return string.Empty; }
            }
        }

        public virtual bool HasPicture(PictureSize aSize)
        {
            switch (aSize)
            {
                case PictureSize.SMALL:     { return !string.IsNullOrEmpty(GetImage(SmallPicture, SmallCover)); }
                case PictureSize.MEDIUM:    { return !string.IsNullOrEmpty(GetImage(MediumPicture, MediumCover)); }
                case PictureSize.LARGE:     { return !string.IsNullOrEmpty(GetImage(LargePicture, LargeCover)); }
                default:                    { return false; }
            }
        }


        private string GetImage(string aPicture, string aCover)
        {
            bool isPictureEmpty = string.IsNullOrEmpty(aPicture);
            bool isCoverEmpty = string.IsNullOrEmpty(aCover);

            if (!isPictureEmpty && isCoverEmpty)        { return aPicture; }        //We have a picture but no cover
            else if (isPictureEmpty && !isCoverEmpty)   { return aCover; }          //We have a cover but no picture
            else                                        { return string.Empty; }    //We have neither...
        }
    }
}
