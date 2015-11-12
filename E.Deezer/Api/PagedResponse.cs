using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

using RestSharp;

namespace E.Deezer.Api
{
    /// <summary>
    /// A Standard Deezer API response.
    /// </summary>
    /// <typeparam name="T">Type of data returned</typeparam>
    internal class DeezerFragment<T>
    {
        public List<T> Data { get; set; }
        public uint Total { get; set; }

        //TODO
        //ADD ERROR HERE AND CHECK FOR IT
    }

    /// <summary>
    /// An excerpt from a book
    /// </summary>
    /// <typeparam name="T">Type of content</typeparam>
    public interface IPage<T>
    {
        /// <summary>
        /// Data of this page
        /// </summary>
        List<T> Data { get; }

        /// <summary>
        /// Starting index of this book found in the page
        /// </summary>
        uint StartIndex { get; }
    }


    internal class Page<T> : IPage<T>
    {
        public List<T> Data { get; private set; }
        public uint StartIndex { get; private set; }

        public Page(uint aStart, IEnumerable<T> aItems)
        {
            Data = new List<T>();
            Data.AddRange(aItems);
            StartIndex = aStart;
        }
    }

    /// <summary>
    /// A large collection of results from the Deezer API
    /// </summary>
    /// <typeparam name="T">Type of these results</typeparam>
    public interface IBook<T>
    {
        /// <summary>
        /// Total number of results found within the book
        /// </summary>
        uint Total { get; }

        /// <summary>
        /// Read a certain number of items from the book
        /// </summary>
        /// <param name="aStart">Starting index</param>
        /// <param name="aCount">Ending index</param>
        /// <param name="aCallback">A callback to return a page from this book containing the requested items</param>
        void Read(uint aStart, uint aCount, Action<IPage<T>> aCallback);


        /// <summary>
        /// Read a certain number of items from the book.
        /// Same as read() but returns as a task, not via callbacl
        /// </summary>
        Task<IPage<T>> Read(uint aStart, uint aCount);
    }


    internal class Book<TSource, TDest> : IBook<TDest>
    {
        public uint Total {get; private set; }
        private Action<uint, uint, Action<IPage<TDest>>> iReadFunction;

        public Book(uint aTotal, Action<uint, uint, Action<IPage<TDest>>> aReadFunction)
        {
            Total = aTotal;
            iReadFunction = aReadFunction;
        }

        public void Read(uint aStart, uint aCount, Action<IPage<TDest>> aCallback )
        {
            iReadFunction(aStart, aCount, aCallback);
        }

        public Task<IPage<TDest>> Read(uint aStart, uint aCount)
        {
            return Task.Factory.StartNew<IPage<TDest>>(() =>
            {
                ManualResetEvent wh = new ManualResetEvent(false);
                IPage<TDest> pg = null;

                iReadFunction(aStart, aCount, (aPage) =>
                {
                    pg = aPage;
                    wh.Set();
                });
                wh.WaitOne();
                wh.Dispose();

                return (pg == null) ? new Page<TDest>(int.MaxValue, new List<TDest>()) : pg;
            });
        }
    }
}
