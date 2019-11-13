using System.Collections.Generic;

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
        public RequestParameter(string aId, object aValue, ParameterType aType)
        {
            Type = aType;
            Id = aId;
            Value = aValue;
        }

        public ParameterType Type { get; }
        public string Id          { get; }
        public object Value       { get; }

        public static IRequestParameter GetAccessTokenParamter(string aAccessToken)
            => GetNewQueryStringParameter("access_token", aAccessToken);

        public static IRequestParameter GetNewQueryStringParameter(string aId, object aValue)
            => new RequestParameter(aId, aValue, ParameterType.QueryString);

        public static IRequestParameter GetNewUrlSegmentParamter(string aId, object aValue)
            => new RequestParameter(aId, aValue, ParameterType.UrlSegment);

        public static IList<IRequestParameter> EmptyList => new List<IRequestParameter>();
    }
}
