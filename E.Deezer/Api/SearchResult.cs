using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E.Deezer.Api
{
    public interface ISearchResult<T>
    {
        List<T> Data { get; set; }
        uint Total { get; set; }
        string Next { get; set; }
        string Previous { get; set; }
    }

    internal class SearchResult<T> : ISearchResult<T> where T : new()
    {
        public List<T> Data { get; set; }

        public uint Total { get; set; }

        public string Next { get; set; }

        public string Previous { get; set; }
    }
}
