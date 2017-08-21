using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace E.Deezer
{
    public enum ParameterType
    {
        QueryString,
        UrlSegment,
    }


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

        public ParameterType Type => iType;
        public string Id          => iId;
        public object Value       => iValue;


        public static IRequestParameter GetAccessTokenParamter(string aAccessToken)
            => GetNewQueryStringParameter("access_token", aAccessToken);

        public static IRequestParameter GetNewQueryStringParameter(string aId, object aValue) 
            => new RequestParameter(aId, aValue, ParameterType.QueryString);


        public static IRequestParameter GetNewUrlSegmentParamter(string aId, object aValue)
            => new RequestParameter(aId, aValue, ParameterType.UrlSegment);

        public static IList<IRequestParameter> EmptyList => new List<IRequestParameter>();
    }

}
