using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

using E.Deezer.Api;

namespace E.Deezer.TestConsole
{
    class Program
    {
        DeezerSession iSession;
        DeezerClient iClient;


        static void Main(string[] args)
        {
            Program me = new Program();

            me.iSession = new DeezerSession("projectgoav", string.Empty, string.Empty, DeezerPermissions.Email);
            me.iClient = new DeezerClient(me.iSession);

            me.Run();

            Console.WriteLine("\n\nPress any key to close...");
            Console.ReadLine(); 
        }


        public void Run()
        {
            //Sleep so we don't hammer API with calls and use up our limit
            var runtime = Task.Factory.StartNew(() =>
            {
                ServiceInfo();
                Thread.Sleep(250);
                AlbumStuff();
                Thread.Sleep(250);
                ArtistStuff();
            });

            runtime.Wait();
        }



        //Gets and prints out service info
        private void ServiceInfo()
        {
            var T = iClient.GetInfos();
            T.Wait();

            Console.WriteLine("> Deezer Service Info");
            Console.WriteLine(string.Format("Country: {0}\nCountry ISO: {1}\nDeezer Available: {2}", T.Result.Country, T.Result.Iso, T.Result.IsAvailable));
        }


        //Searches for a random album and gets its tracks
        private void AlbumStuff()
        {
            //Search + Album Info
            Console.WriteLine("> Performing a Search...");

            var T = iClient.SearchAlbums("Abba");
            var album = T.Result.Data.ElementAt(0);
            Console.WriteLine("> Getting track info");
            var albumTask = album.GetTracks();
            albumTask.Wait();

            Console.WriteLine(string.Format("> ALBUM: {0}", album.Title));

            foreach (var item in albumTask.Result.Data)
            {
                Console.WriteLine(string.Format("\t> {0} ({1}:{2})", item.Title, item.Duration / 60, item.Duration % 60));
            }
        }


        public void ArtistStuff()
        {
            string artistQuery = "Skillet";
            Console.WriteLine("> Performing Search...");
            var artist = iClient.SearchArtists(artistQuery).Result.Data.ElementAt(0);

            var topTracks = artist.GetTopTracks();
            var related = artist.GetRelated();
            var albums = artist.GetAlbums();
            //var tracklist = artist.GetTracklist(); TODO
            Task.WaitAll(topTracks, albums, related);

            Console.WriteLine(">Top Tracks...");

            foreach(var track in topTracks.Result.Data)
            {
                Console.WriteLine(string.Format("\t> {0} ({1}:{2})", track.Title, track.Duration / 60, track.Duration % 60));
            }

            Console.WriteLine(string.Format("{0} album(s) found.", albums.Result.Total));
            Console.WriteLine(string.Format("{0} related artist(s) found.", related.Result.Total));
            Console.WriteLine("> Got artist info");
        }


    }
}
