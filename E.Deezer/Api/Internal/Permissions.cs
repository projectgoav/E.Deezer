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
        public bool HasEmail { get; set; }

        [JsonProperty(PropertyName="basic_access")]
        public bool HasBasicAccess { get; set; }

        [JsonProperty(PropertyName="offline_access")]
        public bool HasOfflineAccess { get; set; }

        [JsonProperty(PropertyName="manage_library")]
        public bool HasManageLibrary { get; set; }

        [JsonProperty(PropertyName="manage_community")]
        public bool HasManageCommunity { get; set; }

        [JsonProperty(PropertyName="delete_library")]
        public bool HasDeleteLibrary { get; set; }

        [JsonProperty(PropertyName="listening_history")]
        public bool HasListeningHistory { get; set; }

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
    }
}
