using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using E.Deezer.Util;

namespace E.Deezer.Api
{
    public interface IComment
    {
        ulong Id { get; }
        string Text { get; }
        DateTime? Posted { get; }
        IUserProfile Author { get; }

        bool IsUserComment { get; }

        /*
        Task<bool> DeleteComment();
        */
    }

    internal class Comment : IComment, IClientObject
    {
        public ulong Id { get; private set; }

        public string Text { get; private set; }

        public DateTime? Posted { get; private set; }

        public IUserProfile Author { get; private set; }

        
        // IClientObject
        public IDeezerClient Client { get; private set; }


        public bool IsUserComment => this.Author != null
                                        && this.Client.IsAuthenticated
                                        && this.Client.CurrentUserId == Author.Id;



        public override string ToString()
            => string.Format("E.Deezer.Comment:{0}", this.Id);


        //JSON
        internal const string ID_PROPERTY_NAME = "id";
        internal const string TEXT_PROPERTY_NAME = "text";
        internal const string POSTED_PROPERTY_NAME = "date";
        internal const string AUTHOR_PROPERTY_NAME = "author";

        public static IComment FromJson(JToken json, IDeezerClient client)
        {
            uint dateAsUnixSeconds = json.Value<uint>(POSTED_PROPERTY_NAME);
            DateTime? postedDate = DateTimeExtensions.ParseUnixTimeFromSeconds(dateAsUnixSeconds);

            return new Comment()
            {
                Id = ulong.Parse(json.Value<string>(ID_PROPERTY_NAME)),
                Text = json.Value<string>(TEXT_PROPERTY_NAME),

                Posted = postedDate,

                Author = Api.UserProfile.FromJson(json[AUTHOR_PROPERTY_NAME], client),


                Client = client,
            };
        }
    }
}
