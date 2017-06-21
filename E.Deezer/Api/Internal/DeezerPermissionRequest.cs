using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E.Deezer.Api
{
    internal class DeezerPermissionRequest : IHasError
    {
        public E.Deezer.Api.OAuthPermissions Permissions { get; set; }
        public Error Error { get; set; }

        public IError TheError { get { return Error; } }
    }
}
