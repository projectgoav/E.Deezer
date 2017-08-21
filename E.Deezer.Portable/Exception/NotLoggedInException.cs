using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E.Deezer
{
    public class NotLoggedInException : Exception
    {
        private const string MSG = "There is no active access token. This operation couldn't be performed";
        public override string Message { get { return MSG; } }
    }
}
