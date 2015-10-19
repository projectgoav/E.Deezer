using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using E.Deezer.Api;

namespace E.Deezer.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            DeezerSession iSession = new DeezerSession("projectgoav", string.Empty, string.Empty, DeezerPermissions.Email);
            DeezerClient iClient = new DeezerClient(iSession);

            var T = iClient.GetInfos();

            T.Wait();


            Console.WriteLine("> Deezer Service Info");
            Console.WriteLine(string.Format("Country: {0}\nCountry ISO: {1}\nDeezer Available: {2}", T.Result.Country, T.Result.Iso, T.Result.IsAvailable));

            Console.WriteLine("\n\nPress any key to close...");
            Console.ReadLine(); 
        }
    }
}
