using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//See Setup Example
using E.Deezer;

namespace E.Deezer.Examples
{
    public class ServiceInformation
    {
        public void Infos()
        {
            //See Setup Example
            DeezerSession session = new DeezerSession(string.Empty, string.Empty, string.Empty, DeezerPermissions.BasicAccess);
            DeezerClient client = new DeezerClient(session);

            //We can get some service information from Deezer using GetInfos() method
            var serviceinfo = client.GetInfos();

            //From this we can get the current user's country
            //NOTE: Based on IP address
            var country = serviceinfo.Result.Country;
            var countryISO = serviceinfo.Result.Iso;

            //We can see if Deezer is available in this country
            var serviceAvailable = serviceinfo.Result.IsAvailable;
        }

    }
}
