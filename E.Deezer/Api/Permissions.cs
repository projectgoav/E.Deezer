using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RestSharp.Deserializers;

namespace E.Deezer.Api
{
    internal interface IPermissions
    {
        bool BasicAccess {get; set; }
        bool Email {get; set; }
        bool OfflineAccess {get; set; }
        bool ManageLibrary {get; set; }
        bool ManageCommunity {get; set; }
        bool DeleteLibrary {get; set; }
        bool ListeningHistory {get; set; }
    }

    internal class OAuthPermissions : IPermissions
    {
        public bool Email {get; set; }

        [DeserializeAs(Name="basic_access")]
        public bool BasicAccess {get; set; }

        [DeserializeAs(Name="offline_access")]
        public bool OfflineAccess {get; set; }

        [DeserializeAs(Name="manage_library")]
        public bool ManageLibrary {get; set; }

        [DeserializeAs(Name="manage_community")]
        public bool ManageCommunity {get; set; }

        [DeserializeAs(Name="delete_library")]
        public bool DeleteLibrary {get; set; }

        [DeserializeAs(Name="listening_history")]
        public bool ListeningHistory {get; set; }
    }
}
