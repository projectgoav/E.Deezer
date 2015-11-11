using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

using RestSharp;

namespace E.Deezer.Api
{
    internal class DeezerFragment<T>
    {
        public List<T> Data { get; set; }
        public uint Total { get; set; }
    }

    public interface IExcerpt<T>
    {
        List<T> Data { get; }
        uint StartIndex { get; }
    }

    internal class Excerpt<T> : IExcerpt<T>
    {
        public List<T> Data { get; private set; }
        public uint StartIndex { get; private set; }

        public Excerpt(uint aStart, IEnumerable<T> aItems)
        {
            Data = new List<T>();
            Data.AddRange(aItems);
            StartIndex = aStart;
        }
    }

    public interface IPage<T>
    {
        uint Total { get; }
        void Read(uint aStart, uint aEnd, Action<IExcerpt<T>> aCallback);
    }

    internal class Page<TSource, TDest> : IPage<TDest>
    {
        public uint Total {get; private set; }
        private Action<uint, uint, Action<IExcerpt<TDest>>> iReadFunction;

        public Page(uint aTotal, Action<uint, uint, Action<IExcerpt<TDest>>> aReadFunction)
        {
            Total = aTotal;
            iReadFunction = aReadFunction;
        }

        public void Read(uint aStart, uint aEnd, Action<IExcerpt<TDest>> aCallback )
        {
            iReadFunction(aStart, aEnd, aCallback);
        }
    }
}
