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
            Console.WriteLine("Starting up test server...");

            ts = new TestService(10024);

            ts.RegisterEndpoint("genre", TestServiceResponder.GenreAll);
            ts.Start();

            Assert.NotNull(ts);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Console.WriteLine("Stopping test server...");

            ts.Stop();
            ts = null;
        }
    }
}