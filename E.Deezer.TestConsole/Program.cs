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


            //Search + Album Info
            Console.WriteLine("> Performing a Search...");

            var TT = iClient.SearchAlbums("Abba");
            var album = TT.Result.Data.ElementAt(0);
            Console.WriteLine("> Getting track info");
            var albumTask = album.GetTracks();
            albumTask.Wait();

            Console.WriteLine(string.Format("> ALBUM: {0}", album.Title));

            foreach (var item in albumTask.Result.Data)
            {
                Console.WriteLine(string.Format("\t> {0} ({1}:{2})", item.Title, item.Duration / 60, item.Duration % 60));
            }


            Console.WriteLine("\n\nPress any key to close...");
            Console.ReadLine(); 
        }
    }
}
