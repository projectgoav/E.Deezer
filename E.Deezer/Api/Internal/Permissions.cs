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

        //TODO - Check a method that has multiple permissions...
        public bool HasPermission(DeezerPermissions aPermission)
        {
            bool permission = true;

            if (aPermission.HasPermission(DeezerPermissions.BasicAccess))
                permission &= HasBasicAccess;

            if (aPermission.HasPermission(DeezerPermissions.DeleteLibrary))
                permission &= HasDeleteLibrary;

            if (aPermission.HasPermission(DeezerPermissions.Email))
                permission &= HasEmail; 

            if (aPermission.HasPermission(DeezerPermissions.ListeningHistory))
                permission &= HasListeningHistory; 

            if (aPermission.HasPermission(DeezerPermissions.ManageCommunity))
                permission &= HasManageCommunity; 

            if (aPermission.HasPermission(DeezerPermissions.ManageLibrary))
                permission &= HasManageLibrary;

            if (aPermission.HasPermission(DeezerPermissions.OfflineAccess))
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

    }
}
