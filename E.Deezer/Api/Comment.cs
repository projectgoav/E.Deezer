using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace E.Deezer.Api
{
    public interface IComment
    {
        ulong Id { get; }
        string Text { get; }
        DateTime Posted { get; }
        IUserProfile Author { get; }
    }

    internal class Comment : IComment, IDeserializable<IDeezerClient>
    {
        public ulong Id
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public DateTime Posted => new DateTime(PostedInternal);

        public IUserProfile Author => AuthorInternal;


        [JsonProperty(PropertyName = "date")]
        public long PostedInternal
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "author")]
        public UserProfile AuthorInternal
        {
            get;
            set;
        }


        //IDeserializable
        public IDeezerClient Client
        {
            get;
            set;
        }

        public void Deserialize(IDeezerClient client)
        {
            this.Client = client;
            this.AuthorInternal?.Deserialize(client);
        }


        public override string ToString()
            => string.Format("E.Deezer.Comment:{0}", this.Id);
    }
}
