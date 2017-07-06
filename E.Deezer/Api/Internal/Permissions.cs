using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RestSharp.Deserializers;

namespace E.Deezer.Api
{
    internal interface IPermissions
    {
        bool HasBasicAccess {get; set; }
        bool HasEmail {get; set; }
        bool HasOfflineAccess {get; set; }
        bool HasManageLibrary {get; set; }
        bool HasManageCommunity {get; set; }
        bool HasDeleteLibrary {get; set; }
        bool HasListeningHistory {get; set; }

        bool HasPermission(DeezerPermissions aPermission);
    }

    internal class OAuthPermissions : IPermissions
    {
        public bool HasEmail {get; set; }

        [DeserializeAs(Name="basic_access")]
        public bool HasBasicAccess {get; set; }

        [DeserializeAs(Name="offline_access")]
        public bool HasOfflineAccess {get; set; }

        [DeserializeAs(Name="manage_library")]
        public bool HasManageLibrary {get; set; }

        [DeserializeAs(Name="manage_community")]
        public bool HasManageCommunity {get; set; }

        [DeserializeAs(Name="delete_library")]
        public bool HasDeleteLibrary {get; set; }

        [DeserializeAs(Name="listening_history")]
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
