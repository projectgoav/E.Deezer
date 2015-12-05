using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using E.Deezer.Endpoint;

namespace E.Deezer
{
    public interface IDeezerSession
    {
        IBrowseEndpoint Browse { get; }
        ISearchEndpoint Search { get; }

        //TODO
        //User Stuff
        //void Login(String aUsername);
        //void Logout();
    }

    public class DeezerSessionV2 : IDeezerSession
    {
        public IBrowseEndpoint Browse  { get { throw new NotImplementedException(); }  }
        public ISearchEndpoint Search {  get { throw new NotImplementedException(); }  }




        /// <summary>
        /// Starts a new session on the Deezer API.
        /// Setup internal workings of E.Deezer
        /// </summary>
        public static Deezer CreateNew()
        {
            return new Deezer(new DeezerSessionV2());
        }


    }
}
