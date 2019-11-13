using System;

namespace E.Deezer
{
    public class NotLoggedInException : Exception
    {
        private const string MSG = "There is no active access token. This operation couldn't be performed";
        public override string Message => MSG;
    }
}
