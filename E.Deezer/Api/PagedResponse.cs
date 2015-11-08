using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace E.Deezer.Api
{
    /// <summary>
    /// Deezer paginated response
    /// </summary>
    /// <typeparam name="T">Type of response</typeparam>
    public interface IPagedResponse<T>
    {
        /// <summary>
        /// List of items of response type
        /// </summary>
        List<T> Data { get; set; }

        /// <summary>
        /// Total number of responses available
        /// </summary>
        uint Total { get; set; }

        /// <summary>
        /// Link to next page of items
        /// </summary>
        string Next { get; set; }

        /// <summary>
        /// Link to previous page of items
        /// </summary>
        string Previous { get; set; }

        //Methods

        /// <summary>
        /// Gets the next page, if available of the response
        /// </summary>
        /// <returns>The next page or throws PageNotAvailableException if no page exists</returns>
        Task<IPagedResponse<T>> GetNextPage();

        /// <summary>
        /// Gets the previous page, if available of the response
        /// </summary>
        /// <returns>The previous page throws PageNotAvailableException if no page exists</returns>
        Task<IPagedResponse<T>> GetPreviousPage();
    }

    internal class PagedResponse<T> : IPagedResponse<T>
    {
        public List<T> Data { get; set; }
        public uint Total { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }

        private DeezerClient Client { get; set; }
        public void Deserialize(DeezerClient aClient) { Client = aClient; }

        public Task<IPagedResponse<T>> GetNextPage()
        {
            if(!string.IsNullOrEmpty(Next))
            {
                return Client.GetPage<T>(Next);
            }
            throw new PageNotAvailableException();
        }

        public Task<IPagedResponse<T>> GetPreviousPage()
        {
            if(!string.IsNullOrEmpty(Previous))
            {
                return Client.GetPage<T>(Previous);
            }
            throw new PageNotAvailableException();
        }


        public override string ToString()
        {
            return string.Format("E.Deezer: PR({0} - {1} item(s)", typeof(T).ToString(), Data.Count);
        }


        public static IPagedResponse<T> GetEmptyResponse<T>()
        {
            return new PagedResponse<T>()
            {
                Data = new List<T>(),
                Total = 0,
                Next = string.Empty,
                Previous = string.Empty,
            };
        }

    }

    /// <summary>
    /// Exception thrown when using PagedResponse Next() and Previous() functions if 
    /// no page is available.
    /// </summary>
    public class PageNotAvailableException : Exception
    {
        public PageNotAvailableException() : base("Requested page doesn't exist") { }
    }
}
