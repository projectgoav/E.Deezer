using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

using RestSharp;

namespace E.Deezer
{
    /// <summary>
    /// This class will become useful once there's support for getting the user's permission
    /// from Deezer API with (or without) throwing up a web window to get a code :(
    /// </summary>
    public class DeezerSession
    {
        /// <summary>
        /// Base Deezer API endpoint
        /// </summary>
        public const string ENDPOINT = "https://api.deezer.com/";

        public string Username { get; private set; }
        public string ApplicationId { get; private set; }
        public string ApplicationSecret { get; private set; }
        internal string Permissions { get; private set; }

        private RestClient iClient;

        public DeezerSession(string aUsername, string aAppId, string aAppSecret, DeezerPermissions iPermissions )
        {
            Username = aUsername;
            ApplicationId = aAppId;
            ApplicationSecret = aAppSecret;

            GeneratePermissionString(iPermissions);

            iClient = new RestClient(ENDPOINT);
        }

        internal Task<IRestResponse<T>> Execute<T>(IRestRequest aRequest, CancellationToken aToken) { return iClient.ExecuteGetTaskAsync<T>(aRequest, aToken); }
        internal Task<IRestResponse> Execute(IRestRequest aRequest, CancellationToken aToken) { return iClient.ExecuteGetTaskAsync(aRequest, aToken); }


        //Adding any addition params we'd like to the requests
        private void AppendParams(IRestRequest aRequest)
        {
            aRequest.AddParameter("output", "json");
        }

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
    }
}
