using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

using E.Deezer.Api;

namespace E.Deezer.Endpoints
{
    public interface ICommentsEndpoint
    {
        Task<IComment> GetCommentById(ulong commentId, CancellationToken cancellationToken);


        Task<bool> DeleteComment(IComment comment, CancellationToken cancellationToken);
        Task<bool> DeleteComment(ulong commentId, CancellationToken cancellationToken);
    }

    internal class CommentsEndpoint : ICommentsEndpoint
    {
        private readonly IDeezerClient client;

        public CommentsEndpoint(IDeezerClient client)
        {
            this.client = client;
        }


        public Task<IComment> GetCommentById(ulong commentId, CancellationToken cancellationToken)
            => this.client.Get($"comment/{commentId}",
                               cancellationToken,
                               json => Api.Comment.FromJson(json, this.client));


        public Task<bool> DeleteComment(IComment comment, CancellationToken cancellationToken)
        {
            if (!comment.IsUserComment)
            {
                throw new ArgumentException("Comment must be a user comment.", nameof(comment));
            }

            return DeleteComment(comment.Id, cancellationToken);
        }

        public Task<bool> DeleteComment(ulong commentId, CancellationToken cancellationToken)
            => this.client.Delete($"comment/{commentId}",
                                  DeezerPermissions.DeleteLibrary,
                                  cancellationToken);
    }
}
