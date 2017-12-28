using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Album albumImpl = new Album()
            {
                Id = kAlbumId,
            };

            //albumImpl.Deserialize();

            IAlbum album = albumImpl;
            
        }

    }
}
