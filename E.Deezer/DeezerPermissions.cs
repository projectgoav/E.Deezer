using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E.Deezer
{
    public static class Permissions
    {
        public static string PermissionToString(this DeezerPermissions aPermission)
        {
            switch (aPermission)
            {

                case DeezerPermissions.BasicAccess: { return "basic_access"; }
                case DeezerPermissions.Email: { return "email"; }
                case DeezerPermissions.OfflineAccess: { return "offline_access"; }
                case DeezerPermissions.ManageLibrary: { return "manage_library"; }
                case DeezerPermissions.ManageCommunity: { return "manage_community"; }
                case DeezerPermissions.DeleteLibrary: { return "delete_history"; }
                case DeezerPermissions.ListeningHistory: { return "listening_history"; }
                default: { throw new UnknownPermissionException(); }
            }
        }


        public class UnknownPermissionException : Exception
        {
            public UnknownPermissionException() : base("An unknown DeezerPermission value was given to PermissionToString()") { }
        }
    }

    /// <summary>
    /// Deezer OAuth Permission requests
    /// </summary>
    [Flags]
    public enum DeezerPermissions
    {
        /// <summary>
        /// Access users basic information.
        /// INFO: Include name, firstname, profile picture, gender
        /// </summary>
        BasicAccess = 1,

        /// <summary>
        /// Get the user's email
        /// </summary>
        Email = 2,

        /// <summary>
        /// Access user data any time
        /// NOTE: Almost the same as all other permissions
        /// </summary>
        OfflineAccess = 4,

        /// <summary>
        /// Manage users' library 
        /// INFO: Add/rename a playlist. Add/order songs in the playlist.
        /// </summary>
        ManageLibrary = 8,

        /// <summary>
        /// Manage users' friends
        /// INFO: Add/remove a following/follower.
        /// </summary>
        ManageCommunity = 16,

        /// <summary>
        /// Delete library items 	
        /// INFO: Allow the application to delete items in the user's library
        /// </summary>
        DeleteLibrary = 32,

        /// <summary>
        /// Allow the application to access the user's listening history
        /// </summary>
        ListeningHistory = 64,
    }
}
