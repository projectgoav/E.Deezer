using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E.Deezer.Endpoint
{
    public interface IGenreEndpoint
    {

    }

    internal class GenreEndpoint : IGenreEndpoint
    {
        private DeezerClientV2 iClient;


        public GenreEndpoint(DeezerClientV2 aClient) {  iClient = aClient;  }
    }
}
