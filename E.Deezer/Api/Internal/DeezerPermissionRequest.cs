using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace E.Deezer.Api
{
    internal class DeezerPermissionRequest : IHasError
    {
        public OAuthPermissions Permissions { get; set; }

        //IHasError
        public Error Error { get; set; }

        public IError TheError =>  Error;
    }
}
