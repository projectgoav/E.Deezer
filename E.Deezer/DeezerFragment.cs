using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RestSharp.Deserializers;

namespace E.Deezer
{
    internal interface IHasError
    {
        IError Error { get; }
    }

    internal interface IError
    {
        string Message { get; set; }
        uint Code { get; set; }
        string Type { get; set; }
    }

    //Grabs an error, if there was one, from the reply
    internal class Error : IError
    {
        public string Message { get; set; }
        public uint Code { get; set; }
        public string Type { get; set; }
    }

    
    //Retrun value of all Deezer API calls
    internal class DeezerFragmentV2<T> : IHasError
    {
        [DeserializeAs(Name="data")]
        public List<T> Items { get; set; }
        public uint Total { get; set; }

        [DeserializeAs(Name = "error")]
        private Error iError { get; set; }

        public IError Error { get { return iError; } }
    }

    internal class DeezerObject<T> : IHasError
    {
        public T Data { get; set; }

        [DeserializeAs(Name = "error")]
        private Error iError { get; set; }

        public IError Error { get { return iError; } }
    }

    internal class DeezerPermissionRequest : IHasError
    {
        public E.Deezer.Api.OAuthPermissions Permissions { get; set; }


        [DeserializeAs(Name = "error")]
        private Error iError { get; set; }

        public IError Error { get { return iError; } }
    }
}
