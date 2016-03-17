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
        object Value       { get; }
    }


    internal class RequestParameter : IRequestParameter
    {
        private ParameterType iType;
        private string iId;
        private object iValue;

        public RequestParameter(string aId, object aValue, ParameterType aType)
        {
            iType = aType;
            iId = aId;
            iValue = aValue;
        }

        public ParameterType Type { get { return iType; } }
        public string Id          { get { return iId; } }
        public object Value       { get { return iValue; } }


        public static IRequestParameter GetAccessTokenParamter(string aAccessToken)
        {
            return GetNewQueryStringParameter("access_token", aAccessToken);
        }

        public static IRequestParameter GetNewQueryStringParameter(string aId, object aValue)
        {
            return new RequestParameter(aId, aValue, ParameterType.QueryString);
        }

        public static IRequestParameter GetNewUrlSegmentParamter(string aId, object aValue)
        {
            return new RequestParameter(aId, aValue, ParameterType.UrlSegment);
        }

        public static IList<IRequestParameter> EmptyList
        {
            get
            {
                return new List<IRequestParameter>();
            }
        }
    }

}
