using Newtonsoft.Json;

namespace E.Deezer.Api
{
    public enum PictureSize
    {
        Small,
        Medium,
        Large,
        ExtraLarge,
    };

    public interface IObjectWithImage
    {
        string GetPicture(PictureSize aSize);
        bool HasPicture(PictureSize aSize);
    }

    internal class ObjectWithImage : IObjectWithImage
    {
        [JsonProperty(PropertyName = "picture_small")]
        internal string SmallPicture { get; set; }

        [JsonProperty(PropertyName = "picture_medium")]
        internal string MediumPicture { get; set; }

        [JsonProperty(PropertyName = "picture_big")]
        internal string LargePicture { get; set; }

        [JsonProperty(PropertyName = "picture_xl")]
        internal string ExtraLargePicture { get; set; }

        [JsonProperty(PropertyName = "cover_small")]
        internal string SmallCover { get; set; }

        [JsonProperty(PropertyName = "cover_medium")]
        internal string MediumCover { get; set; }

        [JsonProperty(PropertyName = "cover_big")]
        internal string LargeCover { get; set; }

        [JsonProperty(PropertyName = "cover_xl")]
        internal string ExtraLargeCover { get; set; }

        internal Error Error { get; set; }

        public virtual string GetPicture(PictureSize aSize)
        {
            switch (aSize)
            {
                case PictureSize.Small:
                    {
                        return GetImage(SmallPicture, SmallCover);
                    }
                case PictureSize.Medium:
                    {
                        return GetImage(MediumPicture, MediumCover);
                    }
                case PictureSize.Large:
                    {
                        return GetImage(LargePicture, LargeCover);
                    }
                case PictureSize.ExtraLarge:
                    {
                        return GetImage(ExtraLargePicture, ExtraLargeCover);
                    }
                default:
                    {
                        return string.Empty;
                    }
            }
        }

        public virtual bool HasPicture(PictureSize aSize)
            => !string.IsNullOrEmpty(GetPicture(aSize));

        private string GetImage(string picture, string cover)
        {
            bool isPictureEmpty = string.IsNullOrEmpty(picture);
            bool isCoverEmpty = string.IsNullOrEmpty(cover);

            if (!isPictureEmpty && isCoverEmpty)        { return picture; }        //We have a picture but no cover
            else if (isPictureEmpty && !isCoverEmpty)   { return cover; }          //We have a cover but no picture
            else                                        { return string.Empty; }    //We have neither...
        }
    }
}
