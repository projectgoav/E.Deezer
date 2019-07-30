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
        private static readonly IReadOnlyDictionary<EDeezerApiError, string> ERROR_MSG_LOOKUP = new Dictionary<EDeezerApiError, string>()
        {
            { EDeezerApiError.Quota,                "You have made too many calls to the Deezer API. Deezer responded with result 4 - QuotaException" },
            { EDeezerApiError.ItemLimit,            "You have requested too many items from Deezer. Responded with resullt 100 - ItemLimitException" },
            { EDeezerApiError.InvalidPermissions,   "You don't have the correct permission to access this information. Deezer responded with result 200 - OAuthException" },
            { EDeezerApiError.InvalidToken,         "Your access token has expired or is invalid. Deezer responded with result 300 - OAuthException" },
            { EDeezerApiError.InvalidParameter,     "Deezer didn't understand the given parameters on your request. Responded with result 500 - ParameterException" },
            { EDeezerApiError.MissingParameter,     "Request was missing a parameter. Responded with result 501 - MissingParameterException" },
            { EDeezerApiError.InvalidQuery,         "Deezer didn't understand query. Responded with result 600 - InvalidQueryException" },
            { EDeezerApiError.ServiceBusy,          "Deezer API is busy or unavailabe. Responded with result 700 - Exception" },
            { EDeezerApiError.NotFound,             "Unable to find the requested resource. Deezer responded with result 800 - DataNotFoundException" },
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
