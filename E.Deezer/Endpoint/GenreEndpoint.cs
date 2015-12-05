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
        private DeezerClient iClient;


        public GenreEndpoint(DeezerClient aClient) {  iClient = aClient;  }
    }
}
