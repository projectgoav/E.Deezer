using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E.Deezer
{
    public class DeezerPermissionsException : Exception
    {
        private string iMessage;

        private const string UNKNOWN = "The provided access token doesn't provide the required access rights to perform this operation.";
        private const string MSG_END = "Please ensure the token hasn't expired.";
        private const string BASIC = "The provided access token doesn't provide 'basic_access' rights for this user and so this operation can't be performed.";
        private const string HISTORY = "The provided access token doesn't provide 'listening_history' rights for this user and so this operation can't be performed";
        private const string EMAIL = "The provided access token doesn't provide 'email' access rights for this user and so this operation can't be performed.";
        private const string OFFLINE = "The provided access token doesn't provide 'offline_access' rights for this user and so this operaiton can't be performed.";
        private const string COMMUNITY = "The provided access token doesn't provide 'manage_community' rights for this user and so this operation can't be performed.";
        private const string DELETE = "The provided access token doesn't provide 'delete' rights for this user and so this operation can't be performem.";

        public DeezerPermissionsException(DeezerPermissions aPermission)
        {
            switch (aPermission)
            {
                case DeezerPermissions.BasicAccess: { iMessage = BASIC; break; }
                case DeezerPermissions.ListeningHistory: { iMessage = HISTORY; break; }
                case DeezerPermissions.Email: { iMessage = EMAIL; break; }
                case DeezerPermissions.OfflineAccess: { iMessage = OFFLINE; break; }
                case DeezerPermissions.ManageCommunity: { iMessage = COMMUNITY; break; }
                case DeezerPermissions.DeleteLibrary: { iMessage = DELETE; break; }
                default: { iMessage = UNKNOWN; break; }
            }
        }


        public override string Message => string.Format("{0}. {1}", iMessage, MSG_END);
    }
}
