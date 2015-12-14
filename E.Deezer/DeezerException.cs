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
    public class DeezerException : Exception
    {
        private IError iError;
        private Exception iException;

        internal DeezerException(IError aError) 
        { 
            iError = aError;

            switch (iError.Code)
            {
                case 4:   { iException = new QuotaException(); break; }
                case 200: { iException = new OAuthException(OAuthException.PERMISSION_STRING); break; }
                case 300: { iException = new OAuthException(OAuthException.TOKEN_INVALID_STRING); break; }
                case 500: { iException = new ParamterException(); break; }
                case 501: { iException = new MissingParameterException(); break; }
                case 600: { iException = new InvalidQueryException(); break; }
                case 700: { iException = new ServiceBusyException(); break; }
                case 800: { iException = new DataNotFoundException(); break; }
                default:  { iException = new Exception("An unknown exception has occured with Deezer API"); break; }
            }
        }

        /// <summary>
        /// Gets the message returned from the Deezer API
        /// </summary>
        public override string Message  { get {  return iError.Message;  } }

        /// <summary>
        /// Gets a more detailed exception corripsonding to the given error code.
        /// Provides one of the exceptions found on: http://developers.deezer.com/api/errors
        /// </summary>
        /// <returns>An more detailed exception</returns>
        public override Exception GetBaseException() {  return iException; }
    }


    public class QuotaException : Exception
    {
        public QuotaException() : base("You have made too many calls to the Deezer API. Deezer responded with result 4 - QuotaException") { }
    }

    public class OAuthException : Exception
    {
        public OAuthException(string aMsg) : base(aMsg) { }

        internal const string PERMISSION_STRING = "You don't not have permission to access this information on Deezer. Deezer responded with result 200 - OAuthException";
        internal const string TOKEN_INVALID_STRING = "Your access token has expired. Deezer responded with result 300 - OAuthException";
    }

    public class ParamterException : Exception
    {
        public ParamterException() : base("Deezer didn't understand the given parameters. Deezer responded with result 500 - ParamterException") { }
    }

    public class MissingParameterException : Exception
    {
        public MissingParameterException() : base("Deezer was expecting another parameter on that request. Deezer responded with result 501 - MissingParameterException") { }
    }

    public class InvalidQueryException : Exception
    {
        public InvalidQueryException() : base("Deezer didn't understand your query. Deezer responded with result 600 - InvalidQueryException") { }
    }

    public class ServiceBusyException : Exception
    {
        public ServiceBusyException() : base("Deezer reported that it's service was busy. Deezer responded with result 700 - Exception") { }
    }

    public class DataNotFoundException : Exception
    {
        public DataNotFoundException() : base("Deezer was unable to find the requested resource. Deezer responded with result 800 - DataNotFoundException") { }
    }
}
