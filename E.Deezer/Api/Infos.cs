using Newtonsoft.Json;

namespace E.Deezer.Api
{
    public interface IServceInfo
    {
        string Country { get; }
        string Iso { get; }
        bool IsAvailable { get; }
    }

    internal class Infos : IServceInfo
    {
        public string Country { get; set; }

        [JsonProperty(PropertyName = "country_iso")]
        public string Iso { get; set; }

        [JsonProperty(PropertyName = "open")]
        public bool IsAvailable { get; set; }

        public override string ToString()
        {
            return string.Format("E.Deezer: ServiceInfo");
        }
    }
}
