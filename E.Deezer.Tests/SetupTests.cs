using System;
using System.Net;

using NUnit.Framework;

using E.Deezer.Tests.Utils;
using E.Deezer.Tests.Properties;

namespace E.Deezer.Tests
{
    [SetUpFixture]
    public class SetupTests
    {
        TestService ts;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ts = new TestService(10024);

            ts.RegisterEndpoint("genre", GenreTest);
            ts.Start();

            Assert.NotNull(ts);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            ts.Stop();
            ts = null;
        }

        private string GenreTest(System.Collections.Generic.Dictionary<string, string> queryString)
        {
            return Resources.GenreAll;
        }
    }
}