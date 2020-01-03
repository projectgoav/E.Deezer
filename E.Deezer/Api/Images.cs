using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Newtonsoft.Json.Linq;

namespace E.Deezer.Api
{
    public enum PictureSize
    {
        Small,
        Medium,
        Large,
        ExtraLarge,
    };


    public interface IImages
    {
        string Small { get; }
        string Medium { get; }
        string Large { get; }
        string ExtraLarge { get; }

        IEnumerable<PictureSize> AvailableSizes { get; }

        bool HasPictureOfSize(PictureSize pictureSize);
    }


    internal class Images : IImages
    {
        internal Images(string small,
                        string medium,
                        string large,
                        string extraLarge)
        {
            this.Small = small;
            this.Medium = medium;
            this.Large = large;
            this.ExtraLarge = extraLarge;

            this.AvailableSizes = CalculateAvailableSizes();
        }


        public string Small { get; }
        public string Medium { get; }
        public string Large { get; }
        public string ExtraLarge { get; }

        public IEnumerable<PictureSize> AvailableSizes { get; }

        public bool HasPictureOfSize(PictureSize size)
            => this.AvailableSizes.Contains(size);

        private IEnumerable<PictureSize> CalculateAvailableSizes()
        {
            var sizes = new List<PictureSize>(4);

            if (!string.IsNullOrEmpty(this.Small))
                sizes.Add(PictureSize.Small);

            if (!string.IsNullOrEmpty(this.Medium))
                sizes.Add(PictureSize.Medium);

            if (!string.IsNullOrEmpty(this.Large))
                sizes.Add(PictureSize.Large);

            if (!string.IsNullOrEmpty(this.ExtraLarge))
                sizes.Add(PictureSize.ExtraLarge);

            return sizes;
        }


        // JSON
        internal const string PICTURE_SMALL_PROPERTY_NAME = "picture_small";
        internal const string PICTURE_MEDIUM_PROPERTY_NAME = "picture_medium";
        internal const string PICTURE_LARGE_PROPERTY_NAME = "picture_big";
        internal const string PICTURE_EXTRALARGE_PROPERTY_NAME = "picture_xl";

        internal const string COVER_SMALL_PROPERTY_NAME = "cover_small";
        internal const string COVER_MEDIUM_PROPERTY_NAME = "cover_medium";
        internal const string COVER_LARGE_PROPERTY_NAME = "cover_big";
        internal const string COVER_EXTRALARGE_PROPERTY_NAME = "cover_xl";


        public static IImages FromJson(JToken json)
        {
            // Prefer 'picture_XXX' values over 'cover_XXX' as they are more
            // likely to occur in API responses
            string small = json.Value<string>(PICTURE_SMALL_PROPERTY_NAME) ?? json.Value<string>(COVER_SMALL_PROPERTY_NAME);
            string medium = json.Value<string>(PICTURE_MEDIUM_PROPERTY_NAME) ?? json.Value<string>(COVER_MEDIUM_PROPERTY_NAME);
            string large = json.Value<string>(PICTURE_LARGE_PROPERTY_NAME) ?? json.Value<string>(COVER_LARGE_PROPERTY_NAME);
            string extraLarge = json.Value<string>(PICTURE_EXTRALARGE_PROPERTY_NAME) ?? json.Value<string>(COVER_EXTRALARGE_PROPERTY_NAME);

            return new Images(small,
                              medium,
                              large,
                              extraLarge);
        }

    }
}
