using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E.Deezer.Api
{
    public interface IUserProfile : IObjectWithImage
    {
        ulong Id { get; }
        string ShareLink { get; }
        string Username { get; }
        string Country { get; }

        Task<IEnumerable<ITrack>> GetFlow(uint aStart = 0, uint aCount = 100);
    }

    internal class UserProfile : ObjectWithImage, IUserProfile, IDeserializable<IDeezerClient>
    {
        public ulong Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "link")]
        public string ShareLink { get; set; }

        public string Country { get; set; }


        //IDeserializable
        public IDeezerClient Client { get; set; }

        public void Deserialize(IDeezerClient client)
            => this.Client = client;

        public Task<IEnumerable<ITrack>> GetFlow(uint aStart = 0, uint aCount = 100)
        {
            List<IRequestParameter> p = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", this.Id),
            };

            return Client.Get<Track>("user/{id}/flow", p, aStart, aCount)
                         .ContinueWith<IEnumerable<ITrack>>(task => Client.Transform<Track, ITrack>(task.Result),
                                                            Client.CancellationToken,
                                                            TaskContinuationOptions.NotOnCanceled,
                                                            TaskScheduler.Default);
        }

        public override string ToString()
            => string.Format("E.Deezer.UserProfile: {0} :: ({1})", this.Username, this.Id);
    }
}
