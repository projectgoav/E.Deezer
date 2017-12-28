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

        private Mock<IDeezerClient> client;

        [SetUp]
        public void SetUp()
        {
            client = new Mock<IDeezerClient>();
        }


        [Test]
        public void TestFavouriteCalls()
        {
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


        [Test]
        public void TestRate()
        {
            Album albumImpl = new Album()
            {
                Id = kAlbumId,
            };
            albumImpl.Deserialize(client.Object);

            IAlbum album = albumImpl;

            client.Setup(c => c.Post(It.IsAny<string>(), It.IsAny<IList<IRequestParameter>>(), It.IsAny<DeezerPermissions>()));

            Assert.Throws<ArgumentOutOfRangeException>(() => album.Rate(-100));
            Assert.Throws<ArgumentOutOfRangeException>(() => album.Rate(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => album.Rate(10240));

            album.Rate(3);

            client.Verify(c => c.Post(It.IsAny<string>(), It.IsAny<IList<IRequestParameter>>(), It.IsAny<DeezerPermissions>()), Times.Once());
        }
    }
}
