using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace E.Deezer.Api
{
    internal interface IPermissions
    {
        bool HasBasicAccess { get; set; }
        bool HasEmail { get; set; }
        bool HasOfflineAccess { get; set; }
        bool HasManageLibrary { get; set; }
        bool HasManageCommunity { get; set; }
        bool HasDeleteLibrary { get; set; }
        bool HasListeningHistory { get; set; }

        bool HasPermission(DeezerPermissions aPermission);
    }

    internal class OAuthPermissions : IPermissions
    {
        public bool HasEmail {get; set; }

        [JsonProperty(PropertyName="basic_access")]
        public bool HasBasicAccess {get; set; }

        [JsonProperty(PropertyName="offline_access")]
        public bool HasOfflineAccess {get; set; }

        [JsonProperty(PropertyName="manage_library")]
        public bool HasManageLibrary {get; set; }

        [JsonProperty(PropertyName="manage_community")]
        public bool HasManageCommunity {get; set; }

        [JsonProperty(PropertyName="delete_library")]
        public bool HasDeleteLibrary {get; set; }

        [JsonProperty(PropertyName="listening_history")]
        public bool HasListeningHistory {get; set; }

        //TODO - Check a method that has multiple permissions...
        public bool HasPermission(DeezerPermissions aPermission)
        {
            bool permission = true;

            if (aPermission.HasFlag(DeezerPermissions.BasicAccess))
            {
                permission &= HasBasicAccess;
            }

            if (aPermission.HasFlag(DeezerPermissions.DeleteLibrary))
            {
                permission &= HasDeleteLibrary;
            }

            if (aPermission.HasFlag(DeezerPermissions.Email))
            {
                permission &= HasEmail; 
            }

            if (aPermission.HasFlag(DeezerPermissions.ListeningHistory))
            {
                permission &= HasListeningHistory; 
            }

            if (aPermission.HasFlag(DeezerPermissions.ManageCommunity))
            {
                permission &= HasManageCommunity; 
            }

            if (aPermission.HasFlag(DeezerPermissions.ManageLibrary))
            {
                permission &= HasManageLibrary;
            }

            if (aPermission.HasFlag(DeezerPermissions.OfflineAccess))
            {
                permission &= HasOfflineAccess;
            }

            return permission;
        }
    }
}
