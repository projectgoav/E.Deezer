using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using E.Deezer.Endpoint;

namespace E.Deezer
{
    public class DeezerSession
    {
        //Base Deezer API endpoint
        public const string ENDPOINT = "https://api.deezer.com/";

        //Private default response size.
        public const uint DEFAULT_SIZE = 25;

        //Internal access to this size.
        private uint iSize = 0;
        internal uint ResultSize { get { return iSize; } }

        public DeezerSession(uint aResultSize) { iSize = aResultSize; }


        //Generates a permission string which can be used to grant people
        //Access to features of the app
        private void GeneratePermissionString(DeezerPermissions iPermissions)
        {
            string perms = null;

            if((iPermissions & DeezerPermissions.BasicAccess) == DeezerPermissions.BasicAccess)
            { 
                AddToString(perms, DeezerPermissions.BasicAccess.PermissionToString());
            }

            if ((iPermissions & DeezerPermissions.DeleteLibrary) == DeezerPermissions.DeleteLibrary)
            {
                AddToString(perms, DeezerPermissions.DeleteLibrary.PermissionToString());
            }

            if ((iPermissions & DeezerPermissions.Email) == DeezerPermissions.Email)
            {
                AddToString(perms, DeezerPermissions.Email.PermissionToString());
            }

            if ((iPermissions & DeezerPermissions.ListeningHistory) == DeezerPermissions.ListeningHistory)
            {
                AddToString(perms, DeezerPermissions.ListeningHistory.PermissionToString());
            }

            if ((iPermissions & DeezerPermissions.ManageCommunity) == DeezerPermissions.ManageCommunity)
            {
                AddToString(perms, DeezerPermissions.ManageCommunity.PermissionToString());
            }

            if ((iPermissions & DeezerPermissions.ManageLibrary) == DeezerPermissions.ManageLibrary)
            {
                AddToString(perms, DeezerPermissions.ManageLibrary.PermissionToString());
            }

            if ((iPermissions & DeezerPermissions.OfflineAccess) == DeezerPermissions.OfflineAccess)
            {
                AddToString(perms, DeezerPermissions.OfflineAccess.PermissionToString());
            }
        }

        //Adds the permissions in a comma seperated list
        private void AddToString(string aString, string aAdd)
        {
            if(string.IsNullOrEmpty(aString)) {  aString = aAdd; }
            else {  aString += string.Format(",{0}", aAdd); }
        }


        /// <summary>
        /// Starts a new session on the Deezer API.
        /// Setup internal workings of E.Deezer
        /// </summary>
        public static Deezer CreateNew() { return CreateNew(DEFAULT_SIZE); }

        public static Deezer CreateNew(uint aDefaultResponseSize)
        {
            return new Deezer(new DeezerSession(aDefaultResponseSize)); 
        }


    }
}
