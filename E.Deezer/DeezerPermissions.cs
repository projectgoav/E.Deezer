using System;
using System.Collections.Generic;

namespace E.Deezer
{
    public static class Permissions
    {
        internal const string BASIC_ACCESS      = "basic_access";
        internal const string EMAIL             = "email";
        internal const string OFFLINE_ACCESS    = "offline_access";
        internal const string MANAGE_LIBRARY    = "manage_library";
        internal const string MANAGE_COMMUNITY  = "manage_community";
        internal const string DELETE_LIBRARY    = "delete_library";
        internal const string LISTENING_HISTORY = "listening_history";

        internal static readonly IReadOnlyDictionary<DeezerPermissions, string> PERMISSION_NAME_LOOKUP = new Dictionary<DeezerPermissions, string>()
        {
            { DeezerPermissions.BasicAccess, BASIC_ACCESS },
            { DeezerPermissions.Email, EMAIL },
            { DeezerPermissions.OfflineAccess, OFFLINE_ACCESS },
            { DeezerPermissions.ManageLibrary, MANAGE_LIBRARY },
            { DeezerPermissions.ManageCommunity, MANAGE_COMMUNITY },
            { DeezerPermissions.DeleteLibrary, DELETE_LIBRARY },
            { DeezerPermissions.ListeningHistory, LISTENING_HISTORY },
        };

        public static string PermissionToString(this DeezerPermissions permissions)
        {
            List<string> perms = new List<string>(8);

            if (permissions.HasPermission(DeezerPermissions.BasicAccess))
                perms.Add(BASIC_ACCESS);

            if (permissions.HasPermission(DeezerPermissions.Email))
                perms.Add(EMAIL);

            if (permissions.HasPermission(DeezerPermissions.DeleteLibrary))
                perms.Add(DELETE_LIBRARY);

            if (permissions.HasPermission(DeezerPermissions.ListeningHistory))
                perms.Add(LISTENING_HISTORY);

            if (permissions.HasPermission(DeezerPermissions.ManageCommunity))
                perms.Add(MANAGE_COMMUNITY);

            if (permissions.HasPermission(DeezerPermissions.ManageLibrary))
                perms.Add(MANAGE_LIBRARY);

            if (permissions.HasPermission(DeezerPermissions.OfflineAccess))
                perms.Add(OFFLINE_ACCESS);

            return string.Join(", ", perms);
        }


        public static bool HasPermission(this DeezerPermissions permissions, DeezerPermissions expectedPermission)
            => (permissions & expectedPermission) > 0;
    }

    /// <summary>
    /// Deezer OAuth Permission requests.
    /// </summary>
    [Flags]
    public enum DeezerPermissions
    {
        /// <summary>
        /// Access users basic information.
        /// INFO: Include name, firstname, profile picture, gender.
        /// </summary>
        BasicAccess = 1 << 0,

        /// <summary>
        /// Get the user's email.
        /// </summary>
        Email = 1 << 1,

        /// <summary>
        /// Access user data any time.
        /// NOTE: Almost the same as all other permissions
        /// </summary>
        OfflineAccess = 1 << 2,

        /// <summary>
        /// Manage users' library.
        /// INFO: Add/rename a playlist. Add/order songs in the playlist.
        /// </summary>
        ManageLibrary = 1 << 3,

        /// <summary>
        /// Manage users' friends.
        /// INFO: Add/remove a following/follower.
        /// </summary>
        ManageCommunity = 1 << 4,

        /// <summary>
        /// Delete library items.
        /// INFO: Allow the application to delete items in the user's library.
        /// </summary>
        DeleteLibrary = 1 << 5,

        /// <summary>
        /// Allow the application to access the user's listening history.
        /// </summary>
        ListeningHistory = 1 << 6,
    }
}
