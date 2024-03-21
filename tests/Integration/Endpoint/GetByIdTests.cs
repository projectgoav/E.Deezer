using System;
using System.Linq;
using System.Threading;

using NUnit.Framework;
using NUnit.Framework.Legacy;

using E.Deezer.Api;

namespace E.Deezer.Tests.Integration.Endpoint
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class GetByIdTests : TestClassBase, IDisposable
    {
        private static readonly uint DUMMY_ID = 0;

        private readonly DeezerSession session;
        private readonly OfflineMessageHandler handler;

        public GetByIdTests()
            : base("BrowseEndpoint")
        {
            this.handler = new OfflineMessageHandler();
            this.session = new DeezerSession(this.handler);
        }


        // IDisposable
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.session.Dispose();
            }
        }

        [Test]
        public void GetAlbumById()
        {
            handler.Content = base.GetServerResponse("album");

            IAlbum album = session.Albums.GetById(DUMMY_ID, CancellationToken.None)
                                         .Result;


            ClassicAssert.IsNotNull(album, nameof(album));
            ClassicAssert.AreEqual(302127, album.Id, nameof(album.Id));
            ClassicAssert.AreEqual("Discovery", album.Title, nameof(album.Title));
            ClassicAssert.AreEqual("724384960650", album.UPC, nameof(album.UPC));
            ClassicAssert.AreEqual("https://www.deezer.com/album/302127", album.Link, nameof(album.Link));

            ClassicAssert.AreEqual(113, album.GenreId, nameof(album.GenreId));
            ClassicAssert.IsNotNull(album.Genre, nameof(album.Genre));
            var genres = album.Genre.ToList();
            ClassicAssert.AreEqual(1, genres.Count, "Genre.Count");
            ClassicAssert.AreEqual(113, genres[0].Id, "Genres[0].Id");
            ClassicAssert.AreEqual("Dance", genres[0].Name, "Genres[0].Name");

            ClassicAssert.IsNotNull(album.Contributors, nameof(album.Contributors));
            var contributors = album.Contributors.ToList();
            ClassicAssert.AreEqual(1, contributors.Count, "Contributors.Count");
            ClassicAssert.AreEqual(27, contributors[0].Id, "Contributors[0].Id");
            ClassicAssert.AreEqual("Daft Punk", contributors[0].Name, "Genres[0].Name");

            ClassicAssert.IsNotNull(album.Artist, nameof(album.Artist));
            ClassicAssert.AreEqual(27, album.Artist.Id, "Artist.Id");
            ClassicAssert.AreEqual("Daft Punk", album.Artist.Name, "Artist.Name");

            ClassicAssert.AreEqual(14, album.TrackCount, nameof(album.TrackCount));
        }


        [Test]
        public void GetArtistById()
        {
            handler.Content = base.GetServerResponse("artist");

            IArtist artist = session.Artists.GetById(DUMMY_ID, CancellationToken.None)
                                            .Result;

            ClassicAssert.IsNotNull(artist, nameof(artist));
            ClassicAssert.AreEqual(1, artist.Id, nameof(artist.Id));
            ClassicAssert.AreEqual("The Beatles", artist.Name, nameof(artist.Name));
            ClassicAssert.AreEqual("https://www.deezer.com/artist/1", artist.Link, nameof(artist.Link));
            ClassicAssert.AreEqual("https://www.deezer.com/artist/1?utm_source=deezer&utm_content=artist-1&utm_term=0_1562078771&utm_medium=web", artist.ShareLink, nameof(artist.ShareLink));
            ClassicAssert.AreEqual(45, artist.NumberOfAlbums, nameof(artist.NumberOfAlbums));
            ClassicAssert.AreEqual(5110265, artist.NumberOfFans, nameof(artist.NumberOfFans));
            ClassicAssert.IsTrue(artist.HasSmartRadio, nameof(artist.HasSmartRadio));
        }



        [Test]
        public void GetPlaylistById()
        {
            handler.Content = base.GetServerResponse("playlist");

            IPlaylist playlist = session.Playlists.GetById(DUMMY_ID, CancellationToken.None)
                                                  .Result;

            ClassicAssert.IsNotNull(playlist, nameof(playlist));
            ClassicAssert.AreEqual(300, playlist.Id, nameof(playlist.Id));
            ClassicAssert.AreEqual("Hard-Fi", playlist.Title, nameof(playlist.Title));
            ClassicAssert.AreEqual("", playlist.Description, nameof(playlist.Description));
            ClassicAssert.AreEqual(1249, playlist.Duration, nameof(playlist.Duration));
            ClassicAssert.IsTrue(playlist.IsPublic, nameof(playlist.IsPublic));
            ClassicAssert.IsFalse(playlist.IsLovedTrack, nameof(playlist.IsLovedTrack));
            ClassicAssert.IsFalse(playlist.IsCollaborative, nameof(playlist.IsCollaborative));
            ClassicAssert.AreEqual(5, playlist.NumberOfTracks, nameof(playlist.NumberOfTracks));
            ClassicAssert.AreEqual(0, playlist.NumberOfFans, nameof(playlist.NumberOfFans));
            ClassicAssert.AreEqual("https://www.deezer.com/playlist/300", playlist.Link, nameof(playlist.Link));
            ClassicAssert.AreEqual("https://www.deezer.com/playlist/300?utm_source=deezer&utm_content=playlist-300&utm_term=0_1562078987&utm_medium=web", playlist.ShareLink, nameof(playlist.ShareLink));

            ClassicAssert.IsNotNull(playlist.Creator, nameof(playlist.Creator));
            ClassicAssert.AreEqual(203, playlist.Creator.Id, "Creator.Id");
            ClassicAssert.AreEqual("anonymous", playlist.Creator.Username, "Creator.Username");
            ClassicAssert.IsNull(playlist.Creator.Link, "Creator.ShareLink");
        }

        [Test]
        public void GetTrackById()
        {
            handler.Content = base.GetServerResponse("track");

            ITrack track = session.Tracks.GetById(DUMMY_ID, CancellationToken.None)
                                        .Result;

            ClassicAssert.IsNotNull(track, nameof(track));
            ClassicAssert.AreEqual(3135556, track.Id, nameof(track.Id));
            ClassicAssert.AreEqual("Harder, Better, Faster, Stronger", track.Title, nameof(track.Title));
            ClassicAssert.AreEqual("Harder, Better, Faster, Stronger", track.ShortTitle, nameof(track.ShortTitle));
            ClassicAssert.AreEqual("GBDUW0000059", track.ISRC, nameof(track.ISRC));
            ClassicAssert.AreEqual("https://www.deezer.com/track/3135556", track.Link, nameof(track.Link));
            ClassicAssert.AreEqual("https://www.deezer.com/track/3135556?utm_source=deezer&utm_content=track-3135556&utm_term=0_1562079211&utm_medium=web", track.ShareLink, nameof(track.ShareLink));
            ClassicAssert.AreEqual(224, track.Duration, nameof(track.Duration));
            ClassicAssert.AreEqual(4, track.TrackNumber, nameof(track.TrackNumber));
            ClassicAssert.AreEqual(1, track.DiscNumber, nameof(track.DiscNumber));
            ClassicAssert.AreEqual(759175, track.Rank, nameof(track.Rank));
            ClassicAssert.AreEqual(new DateTime(2001, 03, 07), track.ReleaseDate, nameof(track.ReleaseDate));
            ClassicAssert.IsFalse(track.IsExplicit, nameof(track.IsExplicit));
            ClassicAssert.AreEqual("https://cdns-preview-d.dzcdn.net/stream/c-deda7fa9316d9e9e880d2c6207e92260-5.mp3", track.PreviewLink, nameof(track.PreviewLink));
            ClassicAssert.AreEqual(123.4f, track.BPM, nameof(track.BPM));
            ClassicAssert.AreEqual(-12.4f, track.Gain, nameof(track.Gain));


            ClassicAssert.IsNotNull(track.AvailableIn, nameof(track.AvailableIn));
            var countries = track.AvailableIn.ToList();
            ClassicAssert.AreEqual(209, countries.Count, "AvailableIn.Count");
   

            ClassicAssert.IsNotNull(track.Contributors, nameof(track.Contributors));
            var contributors = track.Contributors.ToList();
            ClassicAssert.AreEqual(1, contributors.Count, "contributors.Count");

            ClassicAssert.AreEqual(27, contributors[0].Id, "contributors[0].Id");
            ClassicAssert.AreEqual("Daft Punk", contributors[0].Name, "contributors[0].Name");

            ClassicAssert.IsNotNull(track.Artist, nameof(track.Artist));
            ClassicAssert.AreEqual(27, track.Artist.Id, "Artist.Id");
            ClassicAssert.AreEqual("Daft Punk", track.Artist.Name, "Artist.Name");

            ClassicAssert.IsNotNull(track.Album, nameof(track.Album));
            ClassicAssert.AreEqual(302127, track.Album.Id, "Album.Id");
            ClassicAssert.AreEqual("Discovery", track.Album.Title, "Album.Title");
            ClassicAssert.AreEqual("Discovery", track.AlbumName, nameof(track.AlbumName));
        }

        [Test]
        public void GetRadioById()
        {
            handler.Content = base.GetServerResponse("radio");

            IRadio radio = session.Radio.GetById(DUMMY_ID, CancellationToken.None)
                                        .Result;


            ClassicAssert.IsNotNull(radio, nameof(radio));
            ClassicAssert.AreEqual(6, radio.Id, nameof(radio.Id));
            ClassicAssert.AreEqual("Elektronikus zene", radio.Title, nameof(radio.Title));
            ClassicAssert.AreEqual("Elektronikus zene description", radio.Description, nameof(radio.Description));
            ClassicAssert.AreEqual("https://www.deezer.com/mixes/genre/6?utm_source=deezer&utm_content=mixes-genre-6&utm_term=0_1562079884&utm_medium=web", radio.ShareLink, nameof(radio.ShareLink));
        }

        
        [Test]
        public void GetUserById()
        {
            handler.Content = base.GetServerResponse("user");

            IUserProfile user = session.User.GetById(DUMMY_ID, CancellationToken.None)
                                            .Result;

            ClassicAssert.IsNotNull(user, nameof(user));
            ClassicAssert.AreEqual(5, user.Id, nameof(user.Id));
            ClassicAssert.AreEqual("Daniel Marhely", user.Username, nameof(user.Username));
            ClassicAssert.AreEqual("https://www.deezer.com/profile/5", user.Link, nameof(user.Link));
            ClassicAssert.AreEqual("JP", user.Country, nameof(user.Country));
        }

        [Test]
        public void GetPodcastById()
        {
            handler.Content = base.GetServerResponse("podcast");

            IPodcast podcast = session.Podcasts.GetById(DUMMY_ID, CancellationToken.None)
                                            .Result;

            ClassicAssert.IsNotNull(podcast, nameof(podcast));
            ClassicAssert.AreEqual(2888112, podcast.Id, nameof(podcast.Id));
            ClassicAssert.AreEqual("The Rest Is History", podcast.Title, nameof(podcast.Title));
            ClassicAssert.AreEqual("The Rest is History description.", podcast.Description, nameof(podcast.Description));
            ClassicAssert.AreEqual(3774, podcast.Fans, nameof(podcast.Fans));
            ClassicAssert.True(podcast.Available, nameof(podcast.Available));
            ClassicAssert.AreEqual("https://www.deezer.com/show/2888112", podcast.Link, nameof(podcast.Link));
        }

        [Test]
        public void GetEpisodeById()
        {
            handler.Content = base.GetServerResponse("episode");

            IEpisode episode = session.Episodes.GetById(DUMMY_ID, CancellationToken.None)
                                            .Result;

            ClassicAssert.IsNotNull(episode, nameof(episode));
            ClassicAssert.AreEqual(606723512, episode.Id, nameof(episode.Id));
            ClassicAssert.AreEqual(new DateTime(2024, 2, 26, 00, 10, 00), episode.ReleaseDate, nameof(episode.ReleaseDate));
            ClassicAssert.AreEqual("423. Carthage vs. Rome: The Wolf at the Gates (Part 3)", episode.Title, nameof(episode.Title));
            ClassicAssert.AreEqual("Episode 606723512 description", episode.Description, nameof(episode.Description));
            ClassicAssert.AreEqual(3089, episode.Duration, nameof(episode.Duration));
            ClassicAssert.True(episode.Available, nameof(episode.Available));
            ClassicAssert.AreEqual("https://www.deezer.com/episode/606723512", episode.Link, nameof(episode.Link));
        }
    }
}
