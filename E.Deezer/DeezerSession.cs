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

        /// <summary>
        /// Default result size
        /// </summary>
		public const int RESULT_SIZE = 25;

        public string Username { get; private set; }
        public string ApplicationId { get; private set; }
        public string ApplicationSecret { get; private set; }

        /// <summary>
        /// The reference to a fixed size for pages.
        /// If unset will be set to RESULT_SIZE
        /// </summary>
        public int DefaultPageSize { get; private set; }
        internal string Permissions { get; private set; }

        private RestClient iClient;

        /// <summary>
        /// Creates a new Deezer session with the following options.
        /// Throws ArgumentOutOfRangeException if aResultSize < 0
        /// </summary>
        /// <param name="aUsername">User's account name</param>
        /// <param name="aAppId">Your Deezer application ID</param>
        /// <param name="aAppSecret">Your Deezer application secret</param>
        /// <param name="aPermissions">Requested permissions for Deezer API</param>
        /// <param name="aDefaultPageSize">(OPTIONAL)A referene to a default number of items in a Page</param>
        /// <param name="aTimeout">(OPTIONAL)How long to wait for a reponse before giving up. Default 2.5seconds.</param>
        public DeezerSession(string aUsername, string aAppId, string aAppSecret, DeezerPermissions aPermissions, int aDefaultPageSize = RESULT_SIZE, int aTimeout = 2500 )
        {
            Username = aUsername;
            ApplicationId = aAppId;
            ApplicationSecret = aAppSecret;

            if (aDefaultPageSize < 0) {  throw new ArgumentOutOfRangeException("Result Size must be greater than, or equal to, 0"); }
            DefaultPageSize = aDefaultPageSize;

            GeneratePermissionString(aPermissions);

            iClient = new RestClient(ENDPOINT);
            iClient.Timeout = aTimeout;
        }


        #region API Requests
        internal Task<IRestResponse<T>> Execute<T>(IRestRequest aRequest, CancellationToken aToken)
        {
            AppendParams(aRequest);
            var task = iClient.ExecuteGetTaskAsync<T>(aRequest, aToken).ContinueWith<IRestResponse<T>>((aTask) =>
            {
                if(aTask.Result.ErrorException != null) { throw aTask.Result.ErrorException; }
                else { return aTask.Result; }
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
            task.SuppressExceptions();
            return task;
        }

        internal Task<IRestResponse> Execute(IRestRequest aRequest, CancellationToken aToken)
        {
            AppendParams(aRequest);
            var task = iClient.ExecuteGetTaskAsync(aRequest, aToken).ContinueWith<IRestResponse>((aTask) =>
            {
                if (aTask.IsFaulted) { throw aTask.Exception; }
                else { return aTask.Result; }
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
            task.SuppressExceptions();
            return task;
        }
        #endregion

        //Adding any addition params we'd like to the requests
        private void AppendParams(IRestRequest aRequest)
        {
            aRequest.AddParameter("output", "json", ParameterType.QueryString);
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

    /// <summary>
    /// Task Extensions to stop tasks throwing exceptions straight away.
    /// </summary>
    public static class TaskExtensions
    {
        public static Task SuppressExceptions(this Task aTask)
        {
            aTask.ContinueWith((t) => { var ignored = t.Exception; },
                TaskContinuationOptions.OnlyOnFaulted |
                TaskContinuationOptions.ExecuteSynchronously);
            return aTask;
        }
        public static Task<T> SuppressExceptions<T>(this Task<T> aTask)
        {
            aTask.ContinueWith((t) => { var ignored = t.Exception; },
                TaskContinuationOptions.OnlyOnFaulted |
                TaskContinuationOptions.ExecuteSynchronously);
            return aTask;
        }
    }
}
