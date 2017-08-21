using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E.Deezer.Endpoint
{
    public interface IBrowseEndpoint
    {
        IGenreEndpoint Genre { get; }
        IChartsEndpoint Charts { get; }

        //TODO
        //IUserEndpoint User { get; }


    }

    internal class BrowseEndpoint : IBrowseEndpoint
    {
        private readonly IGenreEndpoint iGenre;
        private readonly IChartsEndpoint iCharts;

        private readonly DeezerClient iClient;

        public BrowseEndpoint(DeezerClient aClient)
        {
            iClient = aClient;

            iGenre = new GenreEndpoint(iClient);
            iCharts = new ChartsEndpoint(iClient);
        }


        public IGenreEndpoint Genre => iGenre;

        public IChartsEndpoint Charts => iCharts;
    }
}
