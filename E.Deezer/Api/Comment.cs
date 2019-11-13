using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E.Deezer.Api
{
    public interface IComment
    {
        ulong Id { get; }
        string Text { get; }
        DateTime Posted { get; }
        IUserProfile Author { get; }

        bool IsUserComment { get; }

        Task<bool> DeleteComment();
    }

    internal class Comment : IComment, IDeserializable<IDeezerClient>
    {
        public ulong Id { get; set; }

        public string Text { get; set; }

        public DateTime Posted => new DateTime(PostedInternal);

        public IUserProfile Author => AuthorInternal;

        [JsonProperty(PropertyName = "date")]
        public long PostedInternal { get; set; }

        [JsonProperty(PropertyName = "author")]
        public UserProfile AuthorInternal { get; set; }

        public bool IsUserComment => this.Author != null                            //We've got an author object...
                                        && this.Client.User != null                 //And we're logged in...
                                        && this.Author.Id == this.Client.User.Id;   //And the ids match

        //IDeserializable
        public IDeezerClient Client { get; set; }

        public void Deserialize(IDeezerClient client)
        {
            this.Client = client;
            this.AuthorInternal?.Deserialize(client);
        }

        public Task<bool> DeleteComment()
        {
            if (!this.IsUserComment)
            {
                throw new InvalidOperationException("Attempting to delete a comment which the user did not create. Please check 'IsUserComment' property before calling this method.");
            }

            List<IRequestParameter> p = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", this.Id)
            };

            return Client.Delete("comment/{id}", p, DeezerPermissions.BasicAccess);
        }

        public override string ToString()
            => string.Format("E.Deezer.Comment:{0}", this.Id);
    }
}
