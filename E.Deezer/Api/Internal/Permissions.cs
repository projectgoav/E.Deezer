using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace E.Deezer.Api
{
    public interface IPermissions
    {
        bool HasBasicAccess { get; }
        bool HasEmail { get; }
        bool HasOfflineAccess { get; }
        bool HasManageLibrary { get; }
        bool HasManageCommunity { get; }
        bool HasDeleteLibrary { get; }
        bool HasListeningHistory { get; }

        bool HasPermission(DeezerPermissions requestedPermission);
    }

    internal class OAuthPermissions : IPermissions
    {
        public bool HasEmail { get; private set; }

        public bool HasBasicAccess { get; private set; }

        public bool HasOfflineAccess { get; private set; }

        public bool HasManageLibrary { get; private set; }

        public bool HasManageCommunity { get; private set; }

        public bool HasDeleteLibrary { get; private set; }

        public bool HasListeningHistory { get; private set; }


        public bool HasPermission(DeezerPermissions desiredPermission)
        {
            bool permission = true;

            if (desiredPermission.HasPermission(DeezerPermissions.BasicAccess))
                permission &= HasBasicAccess;

            if (desiredPermission.HasPermission(DeezerPermissions.DeleteLibrary))
                permission &= HasDeleteLibrary;

            if (desiredPermission.HasPermission(DeezerPermissions.Email))
                permission &= HasEmail; 

            if (desiredPermission.HasPermission(DeezerPermissions.ListeningHistory))
                permission &= HasListeningHistory; 

            if (desiredPermission.HasPermission(DeezerPermissions.ManageCommunity))
                permission &= HasManageCommunity; 

            if (desiredPermission.HasPermission(DeezerPermissions.ManageLibrary))
                permission &= HasManageLibrary;

            if (desiredPermission.HasPermission(DeezerPermissions.OfflineAccess))
                permission &= HasOfflineAccess;


            return permission;
        }


        // JSON
        internal const string PERMISSION_OBJECT_PROPERTY_NAME = "permissions";


        public static IPermissions FromJson(JToken json)
        {
            var permissionJson = json[PERMISSION_OBJECT_PROPERTY_NAME];

            return new OAuthPermissions()
            {
                HasBasicAccess = permissionJson.Value<bool>(Permissions.BASIC_ACCESS),
                HasOfflineAccess = permissionJson.Value<bool>(Permissions.OFFLINE_ACCESS),
                HasManageLibrary = permissionJson.Value<bool>(Permissions.MANAGE_LIBRARY),
                HasManageCommunity = permissionJson.Value<bool>(Permissions.MANAGE_COMMUNITY),
                HasDeleteLibrary = permissionJson.Value<bool>(Permissions.DELETE_LIBRARY),
                HasListeningHistory = permissionJson.Value<bool>(Permissions.LISTENING_HISTORY),
                HasEmail = permissionJson.Value<bool>(Permissions.EMAIL),
            };
        }


        // Useful for unit tests :)
        internal static IPermissions FromValues(bool hasEmail = false,
                                                bool hasBasicAccess = false,
                                                bool hasOfflineAccess = false,
                                                bool hasManageLibrary = false,
                                                bool hasManageCommunity = false,
                                                bool hasDeleteLibrary = false,
                                                bool hasListeningHistory = false)
            => new OAuthPermissions()
                {
                    HasEmail = hasEmail,
                    HasBasicAccess = hasBasicAccess,
                    HasOfflineAccess = hasOfflineAccess,
                    HasManageCommunity = hasManageCommunity,
                    HasManageLibrary = hasManageLibrary,
                    HasDeleteLibrary = hasDeleteLibrary,
                    HasListeningHistory = hasListeningHistory
                };

    }
}
