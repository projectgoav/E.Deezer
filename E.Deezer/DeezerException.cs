using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace E.Deezer
{
    /// <summary>
    /// Defines an exception that may be returned from the Deezer API
    /// More information: http://developers.deezer.com/api/errors
    /// </summary>
    internal class DeezerException : Exception
    {
        private IError iError;
        private string iMessage;

        private const string QUOTA_EXCEPTION = "You have made too many calls to the Deezer API. Deezer responded with result 4 - QuotaException";
        private const string OAUTH_EXCEPTION_P = "You don't not have permission to access this information on Deezer. Deezer responded with result 200 - OAuthException";
        private const string OAUTH_EXCEPTION_T = "Your access token has expired or in invalid. Deezer responded with result 300 - OAuthException";
        private const string PARAM_EXCEPTION = "Deezer didn't understand the given parameters. Deezer responded with result 500 - ParamterException";
        private const string MISSING_EXCEPTION = "Deezer was expecting another parameter on that request. Deezer responded with result 501 - MissingParameterException";
        private const string INVALID_EXCEPTION = "Deezer didn't understand your query. Deezer responded with result 600 - InvalidQueryException";
        private const string SERVICE_EXCEPTION = "Deezer reported that it's service was busy. Deezer responded with result 700 - Exception";
        private const string DATA_EXCEPTION = "Deezer was unable to find the requested resource. Deezer responded with result 800 - DataNotFoundException";

        public DeezerException(IError aError)
        { 
            iError = aError;
            switch (aError.Code)
            {
                case 4:   { iMessage = QUOTA_EXCEPTION;   break; }
                case 200: { iMessage = OAUTH_EXCEPTION_P; break; }
                case 300: { iMessage = OAUTH_EXCEPTION_T; break; }
                case 500: { iMessage = PARAM_EXCEPTION;   break; }
                case 501: { iMessage = MISSING_EXCEPTION; break; }
                case 600: { iMessage = INVALID_EXCEPTION; break; }
                case 700: { iMessage = SERVICE_EXCEPTION; break; }
                case 800: { iMessage = DATA_EXCEPTION;    break; }
                default:  { iMessage = "An unknown exception has occured..."; break; }
            }
        }


        /// <summary>
        /// Gets the message returned from the Deezer API
        /// </summary>
        public override string Message  { get {  return iMessage;  } }

        internal IError DeezerError { get { return iError; } }
    }


    internal class DeezerPermissionsException : Exception
    {
        private string iMessage;

        private const string UNKNOWN = "The provided access token doesn't provide the required access rights to perform this operation.";
        private const string MSG_END = "Please ensure the token hasn't expired.";
        private const string BASIC = "The provided access token doesn't provide 'basic_access' rights for this user and so this operation can't be performed.";
        private const string HISTORY = "The provided access token doesn't provide 'lisntening_history' rights for this user and so this operation can't be performed"; 
        
        public DeezerPermissionsException(DeezerPermissions aPermission)
        {
            switch(aPermission)
            {
                case DeezerPermissions.BasicAccess:      { iMessage = BASIC; break; }
                case DeezerPermissions.ListeningHistory: {iMessage = HISTORY; break; }
                default: { iMessage = UNKNOWN; break; }
            }
        }


        public override string Message { get { return string.Format("{0}. {1}", iMessage, MSG_END); } }
    }


    public class NotLoggedInException : Exception
    {
        private const string MSG = "There is no active access token. This operation couldn't be performed";
        public override string Message { get { return MSG; } }
    }
}
