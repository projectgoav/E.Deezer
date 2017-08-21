using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E.Deezer
{
    public static class Permissions
    {
        private const string dBasicAccess = "basic_access";
        private const string dEmail = "email";
        private const string dOffliceAccess = "offline_access";
        private const string dManageLibrary = "manage_library";
        private const string dManageCommunity = "manage_community";
        private const string dDeleteLibrary = "delete_library";
        private const string dListeningHistory = "listening_history";


        public static string PermissionToString(this DeezerPermissions aPermission)
        {
            List<string> perms = new List<string>(10);

            if (aPermission.HasFlag(DeezerPermissions.BasicAccess))
            {
                perms.Add(dBasicAccess);
            }

            if (aPermission.HasFlag(DeezerPermissions.DeleteLibrary))
            {
                perms.Add(dDeleteLibrary);
            }

            if (aPermission.HasFlag(DeezerPermissions.Email))
            {
                perms.Add(dEmail);
            }

            if (aPermission.HasFlag(DeezerPermissions.ListeningHistory))
            {
                perms.Add(dListeningHistory);
            }

            if (aPermission.HasFlag(DeezerPermissions.ManageCommunity))
            {
                perms.Add(dManageCommunity);
            }

            if (aPermission.HasFlag(DeezerPermissions.ManageLibrary))
            {
                perms.Add(dManageLibrary);
            }

            if (aPermission.HasFlag(DeezerPermissions.OfflineAccess))
            {
                perms.Add(dOffliceAccess);
            }

            return string.Join(", ", perms);
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
