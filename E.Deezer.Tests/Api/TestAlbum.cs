using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Moq;
using NUnit.Framework;

using E.Deezer.Api;

namespace E.Deezer.Tests.Api
{
    [TestFixture]
    public class TestAlbum
    {
        private const ulong kAlbumId = 13034431;

        [Test]
        public void TestFavouriteCalls()
        {
            var client = new Mock<IDeezerClient>();

            Album albumImpl = new Album()
            {
                Id = kAlbumId,
            };

            albumImpl.Deserialize(client.Object);

            IAlbum album = albumImpl;

            client.Setup(c => c.User)
                  .Returns(() => new Mock<IUser>().Object);


            album.AddAlbumToFavorite();
            album.RemoveAlbumFromFavorite();


            client.Verify(c => c.User, Times.Exactly(2));
        }

    }
}
