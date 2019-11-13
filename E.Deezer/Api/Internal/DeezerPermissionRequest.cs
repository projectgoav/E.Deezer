using System;

namespace E.Deezer.Api
{
    internal class DeezerPermissionRequest : IHasError
    {
        internal OAuthPermissions Permissions { get; set; }

        //IHasError
        internal Error Error { get; set; }

        public IError TheError =>  Error;
    }
}
