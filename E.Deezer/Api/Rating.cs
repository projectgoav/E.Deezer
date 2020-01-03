using System;
using System.Collections.Generic;
using System.Text;

namespace E.Deezer.Api
{
    public enum DeezerRating : byte
    {
        OneStar     = 1,
        TwoStar     = 2,
        ThreeStar   = 3,
        FourStar    = 4,
        FiveStar    = 5,
    }


    internal static class DeezerRatingExtensions
    {
        public static string AsRatingQueryParam(this DeezerRating rating)
        {
            int numericalValue = (int)rating;

            if (numericalValue < 1 || numericalValue > 5)
                throw new ArgumentOutOfRangeException(nameof(rating));

            return $"note={numericalValue}";
        }
    }
}
