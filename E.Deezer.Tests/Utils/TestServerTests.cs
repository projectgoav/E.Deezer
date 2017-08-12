using System;

using NUnit.Framework;

namespace E.Deezer.Tests.Utils
{
    [TestFixture]
    public class TestServerTests
    {
        [Test]
        public void Test_Service()
        {
            var d = DeezerSession.CreateNew(true);
            var ret = d.Browse.Genre.GetCommonGenre().Result;

            Assert.NotNull(ret);
            
            foreach(var item in ret)
            {
                Assert.NotNull(item);
                Assert.NotNull(item.Name);
            }
        }
    }
}
