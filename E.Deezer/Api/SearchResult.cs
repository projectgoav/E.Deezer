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
        /// <returns>The next page, or null if not available</returns>
        Task<IPagedResponse<T>> GetNextPage();

        /// <summary>
        /// Gets the previous page, if available of the response
        /// </summary>
        /// <returns>The previous page, or null if not available</returns>
        Task<IPagedResponse<T>> GetPreviousPage();
    }

    internal class SearchResult<T> : IPagedResponse<T> where T : new()
    {
        public List<T> Data { get; set; }

        public uint Total { get; set; }

        public string Next { get; set; }

        public string Previous { get; set; }


        public Task<IPagedResponse<T>> GetNextPage()
        {
            throw new NotImplementedException();
        }

        public Task<IPagedResponse<T>> GetPreviousPage()
        {
            throw new NotImplementedException();
        }
    }
}
