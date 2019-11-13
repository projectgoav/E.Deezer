using System.Net.Http;

namespace E.Deezer
{
    public class DeezerSession
    {
        public const string ENDPOINT = "https://api.deezer.com/";

        public DeezerSession()
        {
            AccessToken = string.Empty;
        }

        internal string AccessToken { get; private set; }

        internal bool Authenticated => AccessToken != string.Empty;

        internal void Login(string aAccessToken) => AccessToken = aAccessToken;

        internal void Logout() => AccessToken = string.Empty;

        //Generates a permission string which can be used to grant people
        //Access to features of the app
        public static string GeneratePermissionString(DeezerPermissions iPermissions)
        {
            string perms = null;

            if ((iPermissions & DeezerPermissions.BasicAccess) == DeezerPermissions.BasicAccess)
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

            return perms;
        }

        /// <summary>
        /// Adds the permissions in a comma seperated list.
        /// </summary>
        private static void AddToString(string container, string newValue)
        {
            if (string.IsNullOrEmpty(container))
            {
                container = newValue;
            }
            else
            {
                container += string.Format(",{0}", newValue);
            }
        }

        /// <summary>
        /// Starts a new session on the Deezer API.
        /// Setup internal workings of E.Deezer.
        /// </summary>
        public static Deezer CreateNew(HttpMessageHandler httpMessageHandler = null)
            => new Deezer(new DeezerSession(), httpMessageHandler);
    }
}
