using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//See Setup Example
using E.Deezer;

namespace E.Deezer.Examples
{
    /// <summary>
    /// An example on how to search for data on Deezer
    /// </summary>
    public class Searching
    {
        public void Search()
        {
            //See Setup Example
            DeezerSession session = new DeezerSession(string.Empty, string.Empty, string.Empty, DeezerPermissions.BasicAccess);
            DeezerClient client = new DeezerClient(session);

            //The Search Query
            string SearchQuery = "<Insert Search Query>";

            //You can search Deezer for TRACKS / ARTISTS / ALBUMS / PLAYLISTS
            //Each has a method in the client with the signature - Search<ITEM>(SearchQuery)
            //Returns a paged result
            var searchTask = client.SearchArtists(SearchQuery);

            //Grab the result from the task
            //Contains the first page of results
            var items = searchTask.Result.Data;

            //We can get the total results found from the search
            var total = searchTask.Result.Total;

            //We can get the links to the next/previous pages in the search
            var nextPage = searchTask.Result.Next;
            var previousPage = searchTask.Result.Previous;

            //Alternatively we can get the pages directly
            //TODO NotYetImplemented
            var nextPageTask = searchTask.Result.GetNextPage();
            var previousPageTask = searchTask.Result.GetPreviousPage();
        }

    }
}
