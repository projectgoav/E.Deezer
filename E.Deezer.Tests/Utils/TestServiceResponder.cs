using System;
using System.Collections.Generic;

using E.Deezer.Tests.Properties;

namespace E.Deezer.Tests.Utils
{
    public static class TestServiceResponder
    {
        public static string GenreAll(Dictionary<string, string> queryString)
        {
            return Resources.GenreAll;
        }

    }
}
