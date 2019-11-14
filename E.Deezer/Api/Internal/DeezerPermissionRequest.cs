using Newtonsoft.Json;

namespace E.Deezer.Api
{
    internal class DeezerPermissionRequest : IHasError
    {
        [JsonProperty("permissions")]
        internal OAuthPermissions Permissions { get; set; }

        //IHasError
        internal Error Error { get; set; }

        public IError TheError =>  Error;
    }
}
