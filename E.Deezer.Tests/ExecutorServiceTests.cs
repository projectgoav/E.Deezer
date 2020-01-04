using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using E.Deezer;

namespace E.Deezer.Tests
{
    [TestFixture]
    public class ExecutorServiceTests
    {
        private ExecutorService executor;

        [SetUp]
        public void SetUp()
        {
            executor = new ExecutorService();
            Assert.NotNull(executor);
        }

        [TearDown]
        public void TearDown()
        {
            executor.Dispose();
            executor = null;
        }

       
        //TODO: Add some tests :)

    }
}
