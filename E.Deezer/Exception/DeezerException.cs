using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using E.Deezer.Api;

namespace E.Deezer
{
    public enum EDeezerApiError
    {
        Quota               = 4,
        ItemLimit           = 100,
        InvalidPermissions  = 200,
        InvalidToken        = 300,
        InvalidParameter    = 500,
        MissingParameter    = 501,
        InvalidQuery        = 600,
        ServiceBusy         = 700,
        NotFound            = 800
    }


    /// <summary>
    /// Defines an exception that may be returned from the Deezer API
    /// More information: http://developers.deezer.com/api/errors
    /// </summary>
    /// 
    public class DeezerException : Exception
    {
        internal const string QUOTA_MESSAGE = "You have made too many calls to the Deezer API.Deezer responded with result 4 - QuotaException";
        internal const string ITEM_LIMIT_MESSAGE = "You have requested too many items from Deezer.Responded with resullt 100 - ItemLimitException";
        internal const string INVALID_PERMISSION_MESSAGE = "You don't have the correct permission to access this information. Deezer responded with result 200 - OAuthException";
        internal const string INVALID_TOKEN_MESSAGE = "Your access token has expired or is invalid. Deezer responded with result 300 - OAuthException";
        internal const string INVALID_PARAMETER_MESSAGE = "Deezer didn't understand the given parameters on your request. Responded with result 500 - ParameterException";
        internal const string MISSING_PARAMETER_MESSAGE = "Request was missing a parameter. Responded with result 501 - MissingParameterException";
        internal const string INVALID_QUERY_MESSAGE = "Deezer didn't understand query. Responded with result 600 - InvalidQueryException";
        internal const string SERVICE_BUSY_MESSAGE = "Deezer API is busy or unavailabe. Responded with result 700 - Exception";
        internal const string NOT_FOUND_MESSAGE = "Unable to find the requested resource.Deezer responded with result 800 - DataNotFoundException";


        private static readonly IReadOnlyDictionary<EDeezerApiError, string> ERROR_MSG_LOOKUP = new Dictionary<EDeezerApiError, string>()
        {
            { EDeezerApiError.Quota,                QUOTA_MESSAGE },
            { EDeezerApiError.ItemLimit,            ITEM_LIMIT_MESSAGE },
            { EDeezerApiError.InvalidPermissions,   INVALID_PERMISSION_MESSAGE },
            { EDeezerApiError.InvalidToken,         INVALID_TOKEN_MESSAGE },
            { EDeezerApiError.InvalidParameter,     INVALID_PARAMETER_MESSAGE },
            { EDeezerApiError.MissingParameter,     MISSING_PARAMETER_MESSAGE },
            { EDeezerApiError.InvalidQuery,         INVALID_QUERY_MESSAGE },
            { EDeezerApiError.ServiceBusy,          SERVICE_BUSY_MESSAGE },
            { EDeezerApiError.NotFound,             NOT_FOUND_MESSAGE },
        };

        internal const string DEFAULT_EXCEPTION_MESSAGE = "An unknown exception has occured...";


        internal DeezerException(IError deezerError)
        { 
            this.Error = deezerError;
        }


        /// <summary>
        /// Gets the message returned from the Deezer API
        /// </summary>
        public override string Message
        {
            get
            {
                EDeezerApiError errorCode = (EDeezerApiError)this.Error.Code;

                return ERROR_MSG_LOOKUP.ContainsKey(errorCode) ? ERROR_MSG_LOOKUP[errorCode]
                                                               : DEFAULT_EXCEPTION_MESSAGE;
            }
        }

        internal IError Error { get; }
    }
}
