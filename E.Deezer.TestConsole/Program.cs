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

			me.iSession = new DeezerSession("RadioFeed.org", string.Empty, string.Empty, DeezerPermissions.Email);
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
				//ServiceInfo();
				Thread.Sleep(250);
				//UserInfo(1761649);
				Thread.Sleep(250);
				//GetLovedTracksNotInAnyPlaylist(1761649);
				Thread.Sleep(250);
				AlbumStuff();
				Thread.Sleep(250);
				//ArtistStuff();
			});

			runtime.Wait();
		}





		//Gets and prints out service info
		private void ServiceInfo()
		{
			var T = iClient.GetInfos();
            Await<IInfos>(T);

            if (!T.IsFaulted)
            {
                Console.WriteLine("> Deezer Service Info");
                Console.WriteLine(string.Format("Country: {0}\nCountry ISO: {1}\nDeezer Available: {2}", T.Result.Country, T.Result.Iso, T.Result.IsAvailable));
            }
		}

		//Gets and prints out specific user info
		private void UserInfo(int userId)
		{
			var T = iClient.GetUser(userId);
            Await<IUser>(T);

            if(!T.IsFaulted)
            {
			    IUser user = T.Result;
			    Console.WriteLine("> User info");
			    Console.WriteLine("Id: {0}\nName: {1}\nCountry: {2}\nLink: {3}", user.Id, user.Name, user.Country, user.Link);
            }
		}

		//Gets and prints out specific user info
		private void GetLovedTracksNotInAnyPlaylist(int userId)
		{
			var T = iClient.GetUser(userId);

            Await<IUser>(T);
            if (T.IsFaulted) { return; }

			IUser user = T.Result;
			var playlistsT = user.GetFavouritePlaylists(100);

            Await<IPage<IPlaylist>>(playlistsT);
            if (playlistsT.IsFaulted) { return; }

			var playlists = playlistsT.Result;
			Console.WriteLine("> PLAYLISTS");

			// write all playlists
			HashSet<int> tracksInPlaylists = new HashSet<int>();
			foreach (var item in playlists.Data)
			{
				Console.Write(string.Format("\t> {0}  by {1}. {2} track(s))", item.Title, item.CreatorName, item.NumTracks));
				if (item.IsLovedTrack)
				{
					// Ignore loved tracks playlist
					Console.Write(" LOVED TRACKS");
				}
				else
				{
					if (item.CreatorName == iSession.Username)
					{
						// add tracks id for this playlist
						var tracksTask = item.GetTracks(500);
                        Await<IPage<ITrack>>(tracksTask);
                        if (tracksTask.IsFaulted) { return; }

						var tracks = tracksTask.Result;

						tracksInPlaylists.UnionWith(tracks.Data.Select(t => t.Id));
					}
				}
				Console.WriteLine();
			}
			Console.WriteLine("{0} tracks in all user playlists.", tracksInPlaylists.Count);

			// Get loved tracks playlist
			HashSet<int> lovedTracksId = new HashSet<int>();
			var loved = playlists.Data.Where(p => p.IsLovedTrack).FirstOrDefault();
			if (loved != null)
			{
				var tracksTask = loved.GetTracks(500);
                Await<IPage<ITrack>>(tracksTask);
                if (tracksTask.IsFaulted) { return; }

				var tracks = tracksTask.Result;
				lovedTracksId.UnionWith(tracks.Data.Select(t => t.Id));

				lovedTracksId.ExceptWith(tracksInPlaylists);

				// get tracks in loved but not in any playlist
				var orphanTracks = tracks.Data.Where(t => lovedTracksId.Contains(t.Id)).ToList();

				Console.WriteLine("{0} tracks in Loved Tracks but not in any other playlist.", orphanTracks.Count);
			}


		}


		//Searches for a random album and gets its tracks
		private void AlbumStuff()
		{
			//Search + Album Info
			Console.WriteLine("> Performing a Search...");

			var T = iClient.SearchAlbums("Abba");

            Await<IPage<IAlbum>>(T);
            if (T.IsFaulted) { return; }

            var result = T.Result;

			Console.WriteLine("> Getting track info");

            System.Threading.ManualResetEvent wh = new ManualResetEvent(false);

            result.Read(0, 35, (aFragment) =>
            {
                int i = 0;
                foreach(var a in aFragment.Data)
                {
                    Console.WriteLine(string.Format("\t> {0} ({1})", a.Title, a.ArtistName));
                    if(i == 0)
                    {
                        a.GetTracks().ContinueWith((aTask) => Console.WriteLine("No Errors here...")).Wait();
                        i++; 
                    }
                }
                wh.Set();
            });

            wh.WaitOne(5000);
            wh.Reset();
		}


		public void ArtistStuff()
		{
			string artistQuery = "Skillet";
			Console.WriteLine("> Performing Search...");

            var aTask = iClient.SearchArtists(artistQuery);
            Await<IPage<IArtist>>(aTask);
            if (aTask.IsFaulted) { return; }

			var artist = aTask.Result.Data.ElementAt(0);

			var topTracks = artist.GetTopTracks();
			var related = artist.GetRelated();
			var albums = artist.GetAlbums();
			var tracklist = artist.GetTracklist();

            try
            {
			    Task.WaitAll(topTracks, albums, related, tracklist);
            }
            catch (AggregateException ex)
            {
                Exception e = ex.GetBaseException();
                Console.WriteLine("[EXCEPTION THROWN]");
                Console.WriteLine(e.GetType().ToString());
                Console.WriteLine(e.Message);
                return;
            }

			Console.WriteLine(">Top Tracks...");

			foreach (var track in topTracks.Result.Data)
			{
				Console.WriteLine(string.Format("\t> {0} ({1}:{2})", track.Title, track.Duration / 60, track.Duration % 60));
			}

			Console.WriteLine(string.Format("{0} album(s) found.", albums.Result.Total));
			Console.WriteLine(string.Format("{0} related artist(s) found.", related.Result.Total));
			Console.WriteLine(string.Format("{0} track(s) found.", tracklist.Result.Data.Count));
			Console.WriteLine("> Got artist info");
		}

        //Waits the tasks and prints any errors that might have been thrown by the tasks.
        private void Await<T>(Task<T> aTask)
        {
            try
            {
                aTask.Wait();
            }
            catch(AggregateException ex)
            {
                Exception e = ex.GetBaseException();
                Console.WriteLine("[EXCEPTION THROWN]");
                Console.WriteLine(e.GetType().ToString());
                Console.WriteLine(e.Message);
            }
        }

	}
}
