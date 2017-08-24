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

        IUserEndpoint CurrentUser { get; }


    }

    internal class BrowseEndpoint : IBrowseEndpoint
    {
        private readonly IGenreEndpoint iGenre;
        private readonly IChartsEndpoint iCharts;
        private readonly IUserEndpoint iUserEndpoint;

        private readonly DeezerClient iClient;

        public BrowseEndpoint(DeezerClient aClient)
        {
            iClient = aClient;

            iGenre = new GenreEndpoint(iClient);
            iCharts = new ChartsEndpoint(iClient);
            iUserEndpoint = new UserEndpoint(aClient);
        }


        public IGenreEndpoint Genre => iGenre;

        public IChartsEndpoint Charts => iCharts;

        public IUserEndpoint CurrentUser
        {
            get
            {
                if(!iClient.IsAuthenticated)
                {
                    throw new NotLoggedInException();
                }

                return iUserEndpoint;
            }
        }
    }
}
