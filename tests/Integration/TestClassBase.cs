using System.IO;
using System.Net.Http;

using NUnit.Framework;

namespace E.Deezer.Tests.Integration
{
    public abstract class TestClassBase
    {
        private readonly string _directory;

        protected TestClassBase(string moduleName)
        {
            _directory = Path.Combine(TestContext.CurrentContext.TestDirectory, "StaticResources", moduleName);
        }

        /// <summary>
        /// Loads a .json file into a <see cref="StreamContent"/>.
        /// </summary>
        /// <param name="fileName">Only the name of the file
        /// without it's extension.</param>
        /// <returns>Non null <see cref="StreamContent"/>.</returns>
        protected StreamContent GetServerResponse(string fileName)
        {
            return new StreamContent(
                File.OpenRead(
                    Path.Combine(_directory, $"{fileName}.json")));
        }
    }
}
