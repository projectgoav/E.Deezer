using Newtonsoft.Json;

namespace E.Deezer.Api
{
    //Return value of many of the POST API Calls
    internal class DeezerCreateResponse
    {
        [JsonProperty(PropertyName = "id")]
        public ulong Id { get; set; }
    }
}
