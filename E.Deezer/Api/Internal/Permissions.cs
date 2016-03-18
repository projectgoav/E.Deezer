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

        public bool HasPermission(DeezerPermissions aPermission)
        {
            switch (aPermission)
            {
                case DeezerPermissions.BasicAccess:         { return HasBasicAccess; }
                case DeezerPermissions.DeleteLibrary:       { return HasDeleteLibrary; }
                case DeezerPermissions.Email:               { return HasEmail; }
                case DeezerPermissions.ListeningHistory:    { return HasListeningHistory; }
                case DeezerPermissions.ManageCommunity:     { return HasManageCommunity; }
                case DeezerPermissions.ManageLibrary:       { return HasManageLibrary; }
                case DeezerPermissions.OfflineAccess:       { return HasOfflineAccess; }
                default: { return false; }
            }
        }
    }
}
