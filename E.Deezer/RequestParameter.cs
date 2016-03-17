using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RestSharp;

namespace E.Deezer
{
    internal interface IRequestParameter
    {
        ParameterType Type { get; }
        string Id          { get; }
        string Value       { get; }
    }


    internal class RequestParameter
    {
        private ParameterType iType;
        private string iId;
        private string iValue;

        public RequestParameter(string aId, string aValue, ParameterType aType)
        {
            iType = aType;
            iId = aId;
            iValue = aValue;
        }

        public ParameterType Type { get { return iType; } }
        public string Id          { get { return iId; } }
        public string Value       { get { return iValue; } }
    }
}
