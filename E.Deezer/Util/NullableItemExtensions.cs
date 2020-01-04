using System;
using System.Collections.Generic;
using System.Text;

using E.Deezer.Api;

namespace E.Deezer.Util
{
    internal static class NullableItemExtensions
    {

        public static void ThrowIfNull(this IAlbum album)
        {
            if (album == null)
            {
                throw new ArgumentNullException(nameof(album));
            }
        }


        public static void ThrowIfNull(this IArtist artist)
        {
            if (artist == null)
            {
                throw new ArgumentNullException(nameof(artist));
            }
        }


        public static void ThrowIfNull(this IPlaylist playlist)
        {
            if (playlist == null)
            {
                throw new ArgumentNullException(nameof(playlist));
            }
        }


        public static void ThrowIfNull(this ITrack track)
        {
            if (track == null)
            {
                throw new ArgumentNullException(nameof(track));
            }
        }


        public static void ThrowIfNull(this IGenre genre)
        {
            if (genre == null)
            {
                throw new ArgumentNullException(nameof(genre));
            }
        }


        public static void ThrowIfNull(this IUserProfile user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
        }


        public static void ThrowIfNull(this IRadio radio)
        {
            if (radio == null)
            {
                throw new ArgumentNullException(nameof(radio));
            }
        }
    }
}
