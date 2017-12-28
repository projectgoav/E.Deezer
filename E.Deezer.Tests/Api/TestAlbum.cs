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

        [Test]
        public void TestTracklistWillUseInternal()
        {
            Album albumImpl = new Album()
            {
                Id = kAlbumId,
                TracklistInternal = new DeezerFragment<Track>()
                {
                    Items = new List<Track>()
                    {
                        new Track(),
                        new Track(),
                        new Track(),
                    },
                },
            };
            albumImpl.Deserialize(client.Object);

            IAlbum album = albumImpl;

            client.Setup(c => c.Get<Track>(It.IsAny<string>(), It.IsAny<IList<IRequestParameter>>()));

            var tracklist = album.GetTracks()
                                 .Result;

            Assert.NotNull(tracklist);

            client.Verify(c => c.Get<Track>(It.IsAny<string>(), It.IsAny<IList<IRequestParameter>>()), Times.Never());
        }

        [Test]
        public void TestArtistInternalDeserialised()
        {
            Artist artistImpl = new Artist();

            Album albumImpl = new Album()
            {
                Id = kAlbumId,
                ArtistInternal = artistImpl,
            };

            albumImpl.Deserialize(client.Object);

            Assert.That(albumImpl.ArtistInternal.Client != null);
        }
    }
}
