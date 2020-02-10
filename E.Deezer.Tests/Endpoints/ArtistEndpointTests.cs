using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

using Moq;
using NUnit.Framework;

using E.Deezer.Api;
using E.Deezer.Endpoints;

namespace E.Deezer.Tests.Endpoints
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ArtistEndpointTests
    {
        private readonly ArtistEndpoint endpoint;


        public ArtistEndpointTests()
        {
            var client = new Mock<IDeezerClient>(MockBehavior.Strict);

            endpoint = new ArtistEndpoint(client.Object);
        }



        [Test]
        public void TestThrowsIfGivenArtistWithoutSmartRadio()
        {
            var artist = new Mock<IArtist>(MockBehavior.Strict);

            artist.Setup(a => a.HasSmartRadio)
                  .Returns(false);

            Assert.Throws<InvalidOperationException>(() => this.endpoint.GetArtistsRadio(artist.Object, CancellationToken.None)
                                                                        .Wait());

        }

        [TestCase(null)]
        [TestCase("")]      // String.Empty
        public void TestThrowsOnCommentValidation(string comment)
        {
            Assert.Throws<ArgumentNullException>(() => this.endpoint.CommentOnArtist(0L, comment, CancellationToken.None)
                                                                    .Wait());
        }
    }
}
