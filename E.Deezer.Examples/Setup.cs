using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Include this to use the Deezer API
//Remember to include E.Deezer as a reference
using E.Deezer;

namespace E.Deezer.Examples
{
    /// <summary>
    /// Details how to get a Deezer Client object to start using the Deezer
    /// API with .NET
    /// </summary>
    public class Setup
    {

        //Simple method getting us a Deezer Client object so we can use the API 
        public void SetupMethod()
        {
            //The username of the account you wish to browse the API with
            //NOTE: You don't need to specify this to BROWSE the API,
            //The username is only used when accessing user information, history or favourites.
            string Username = "<INSERT USERNAME HERE>";

            //Your application ID found at http://developers.deezer.com
            //NOTE: Since Deezer require a webwindow to authenticate users. E.Deezer currently doesn't
            //provide this feature and so this param is ignored.
            string ApplicationID = string.Empty;

            //Your application secret found at http://developers.deezer.com
            //NOTE: Since Deezer require a webwindow to authenticate users. E.Deezer currently doesn't
            //provide this feature and so this param is ignored.
            string ApplicationSecret = string.Empty;

            //Permissions that will be requested when authenticating users
            //See http://developers.deezer.com/api/permissions
            //NOTE: Since Deezer require a webwindow to authenticate users. E.Deezer currently doesn't
            //provide this feature and so this param is ignored.
            DeezerPermissions Permissions = DeezerPermissions.BasicAccess;


            //Create a DeezerSession
            //NOTE: This will be used to authenticate users. E.Deezer doesn't provide this feature yet.
            DeezerSession session = new DeezerSession(Username, ApplicationID, ApplicationSecret, Permissions);

            //We use this session to create a DeezerClient
            DeezerClient client = new DeezerClient(session);

            //All done :)
            //Use the DeezerClient object to get data from the Deezer API
        }

    }
}
