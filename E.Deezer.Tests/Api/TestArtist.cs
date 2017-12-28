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
    public class TestArtist
    {
        private const ulong kArtistId = 1024u;

        private Mock<IDeezerClient> client;

        [SetUp]
        public void SetUp()
        {
            client = new Mock<IDeezerClient>();
        }


        [Test]
        public void TestTracklist()
        {
            string method = null;
            uint start = uint.MaxValue;
            uint count = uint.MaxValue;
            IList<IRequestParameter> parameters = null;

            client.Setup(c => c.Get<Track>(It.IsAny<string>(), It.IsAny<IList<IRequestParameter>>(), It.IsAny<uint>(), It.IsAny<uint>()))
                  .Callback<string, IList<IRequestParameter>, uint, uint>((m, p, s, c) =>
                  {
                      method = m;
                      parameters = p;
                      start = s;
                      count = c;
                  })
                  .Returns(new TaskCompletionSource<DeezerFragment<Track>>().Task);


            Artist artist = new Artist()
            {
                Id = kArtistId,
            };

            artist.Deserialize(client.Object);
            artist.GetTracklist();

            Assert.That(start != uint.MaxValue);
            Assert.That(count != uint.MaxValue);

            Assert.AreEqual("artist/{id}/radio", method);
            Assert.That(parameters != null);

            Assert.That(parameters.Any(p => p.Id == "id"));
            Assert.That(parameters.Any(p => p.Value is ulong && ((ulong)p.Value) == kArtistId));
        }

        [Test]
        public void TestTop()
        {
            string method = null;
            uint start = uint.MaxValue;
            uint count = uint.MaxValue;
            IList<IRequestParameter> parameters = null;

            client.Setup(c => c.Get<Track>(It.IsAny<string>(), It.IsAny<IList<IRequestParameter>>(), It.IsAny<uint>(), It.IsAny<uint>()))
                  .Callback<string, IList<IRequestParameter>, uint, uint>((m, p, s, c) =>
                  {
                      method = m;
                      parameters = p;
                      start = s;
                      count = c;
                  })
                  .Returns(new TaskCompletionSource<DeezerFragment<Track>>().Task);


            Artist artist = new Artist()
            {
                Id = kArtistId,
            };

            artist.Deserialize(client.Object);
            artist.GetTopTracks();

            Assert.That(start != uint.MaxValue);
            Assert.That(count != uint.MaxValue);

            Assert.AreEqual("artist/{id}/top", method);
            Assert.That(parameters != null);

            Assert.That(parameters.Any(p => p.Id == "id"));
            Assert.That(parameters.Any(p => p.Value is ulong && ((ulong)p.Value) == kArtistId));
        }

        [Test]
        public void TestGetAlbums()
        {
            string method = null;
            uint start = uint.MaxValue;
            uint count = uint.MaxValue;
            IList<IRequestParameter> parameters = null;

            client.Setup(c => c.Get<Album>(It.IsAny<string>(), It.IsAny<IList<IRequestParameter>>(), It.IsAny<uint>(), It.IsAny<uint>()))
                  .Callback<string, IList<IRequestParameter>, uint, uint>((m, p, s, c) =>
                  {
                      method = m;
                      parameters = p;
                      start = s;
                      count = c;
                  })
                  .Returns(new TaskCompletionSource<DeezerFragment<Album>>().Task);


            Artist artist = new Artist()
            {
                Id = kArtistId,
            };

            artist.Deserialize(client.Object);
            artist.GetAlbums();

            Assert.That(start != uint.MaxValue);
            Assert.That(count != uint.MaxValue);

            Assert.AreEqual("artist/{id}/albums", method);
            Assert.That(parameters != null);

            Assert.That(parameters.Any(p => p.Id == "id"));
            Assert.That(parameters.Any(p => p.Value is ulong && ((ulong)p.Value) == kArtistId));
        }

        [Test]
        public void TestRelated()
        {
            string method = null;
            uint start = uint.MaxValue;
            uint count = uint.MaxValue;
            IList<IRequestParameter> parameters = null;

            client.Setup(c => c.Get<Artist>(It.IsAny<string>(), It.IsAny<IList<IRequestParameter>>(), It.IsAny<uint>(), It.IsAny<uint>()))
                  .Callback<string, IList<IRequestParameter>, uint, uint>((m, p, s, c) =>
                  {
                      method = m;
                      parameters = p;
                      start = s;
                      count = c;
                  })
                  .Returns(new TaskCompletionSource<DeezerFragment<Artist>>().Task);


            Artist artist = new Artist()
            {
                Id = kArtistId,
            };

            artist.Deserialize(client.Object);
            artist.GetRelated();

            Assert.That(start != uint.MaxValue);
            Assert.That(count != uint.MaxValue);

            Assert.AreEqual("artist/{id}/related", method);
            Assert.That(parameters != null);

            Assert.That(parameters.Any(p => p.Id == "id"));
            Assert.That(parameters.Any(p => p.Value is ulong && ((ulong)p.Value) == kArtistId));
        }

        [Test]
        public void TestPlaylistsContainingArtist()
        {
            string method = null;
            uint start = uint.MaxValue;
            uint count = uint.MaxValue;
            IList<IRequestParameter> parameters = null;

            client.Setup(c => c.Get<Playlist>(It.IsAny<string>(), It.IsAny<IList<IRequestParameter>>(), It.IsAny<uint>(), It.IsAny<uint>()))
                  .Callback<string, IList<IRequestParameter>, uint, uint>((m, p, s, c) =>
                  {
                      method = m;
                      parameters = p;
                      start = s;
                      count = c;
                  })
                  .Returns(new TaskCompletionSource<DeezerFragment<Playlist>>().Task);


            Artist artist = new Artist()
            {
                Id = kArtistId,
            };

            artist.Deserialize(client.Object);
            artist.GetPlaylistsContaining();

            Assert.That(start != uint.MaxValue);
            Assert.That(count != uint.MaxValue);

            Assert.AreEqual("artist/{id}/playlists", method);
            Assert.That(parameters != null);

            Assert.That(parameters.Any(p => p.Id == "id"));
            Assert.That(parameters.Any(p => p.Value is ulong && ((ulong)p.Value) == kArtistId));
        }


        [Test]
        public void TestFavouriting()
        {
            client.Setup(c => c.User)
                  .Returns(() => new Mock<IUser>().Object);

            Artist artist = new Artist()
            {
                Id = kArtistId,
            };
            artist.Deserialize(client.Object);

            artist.AddArtistToFavorite();
            artist.RemoveArtistFromFavorite();


            client.Verify(c => c.User, Times.Exactly(2));
        }
    }
}
