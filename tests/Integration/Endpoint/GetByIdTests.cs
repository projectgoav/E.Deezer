using System;
using System.Linq;
using System.Threading;

using NUnit.Framework;

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


            Assert.IsNotNull(album, nameof(album));
            Assert.AreEqual(302127, album.Id, nameof(album.Id));
            Assert.AreEqual("Discovery", album.Title, nameof(album.Title));
            Assert.AreEqual("724384960650", album.UPC, nameof(album.UPC));
            Assert.AreEqual("https://www.deezer.com/album/302127", album.Link, nameof(album.Link));

            Assert.AreEqual(113, album.GenreId, nameof(album.GenreId));
            Assert.IsNotNull(album.Genre, nameof(album.Genre));
            var genres = album.Genre.ToList();
            Assert.AreEqual(1, genres.Count, "Genre.Count");
            Assert.AreEqual(113, genres[0].Id, "Genres[0].Id");
            Assert.AreEqual("Dance", genres[0].Name, "Genres[0].Name");

            Assert.IsNotNull(album.Contributors, nameof(album.Contributors));
            var contributors = album.Contributors.ToList();
            Assert.AreEqual(1, contributors.Count, "Contributors.Count");
            Assert.AreEqual(27, contributors[0].Id, "Contributors[0].Id");
            Assert.AreEqual("Daft Punk", contributors[0].Name, "Genres[0].Name");

            Assert.IsNotNull(album.Artist, nameof(album.Artist));
            Assert.AreEqual(27, album.Artist.Id, "Artist.Id");
            Assert.AreEqual("Daft Punk", album.Artist.Name, "Artist.Name");

            Assert.AreEqual(14, album.TrackCount, nameof(album.TrackCount));
        }


        [Test]
        public void GetArtistById()
        {
            handler.Content = base.GetServerResponse("artist");

            IArtist artist = session.Artists.GetById(DUMMY_ID, CancellationToken.None)
                                            .Result;

            Assert.IsNotNull(artist, nameof(artist));
            Assert.AreEqual(1, artist.Id, nameof(artist.Id));
            Assert.AreEqual("The Beatles", artist.Name, nameof(artist.Name));
            Assert.AreEqual("https://www.deezer.com/artist/1", artist.Link, nameof(artist.Link));
            Assert.AreEqual("https://www.deezer.com/artist/1?utm_source=deezer&utm_content=artist-1&utm_term=0_1562078771&utm_medium=web", artist.ShareLink, nameof(artist.ShareLink));
            Assert.AreEqual(45, artist.NumberOfAlbums, nameof(artist.NumberOfAlbums));
            Assert.AreEqual(5110265, artist.NumberOfFans, nameof(artist.NumberOfFans));
            Assert.IsTrue(artist.HasSmartRadio, nameof(artist.HasSmartRadio));
        }



        [Test]
        public void GetPlaylistById()
        {
            handler.Content = base.GetServerResponse("playlist");

            IPlaylist playlist = session.Playlists.GetById(DUMMY_ID, CancellationToken.None)
                                                  .Result;

            Assert.IsNotNull(playlist, nameof(playlist));
            Assert.AreEqual(300, playlist.Id, nameof(playlist.Id));
            Assert.AreEqual("Hard-Fi", playlist.Title, nameof(playlist.Title));
            Assert.AreEqual("", playlist.Description, nameof(playlist.Description));
            Assert.AreEqual(1249, playlist.Duration, nameof(playlist.Duration));
            Assert.IsTrue(playlist.IsPublic, nameof(playlist.IsPublic));
            Assert.IsFalse(playlist.IsLovedTrack, nameof(playlist.IsLovedTrack));
            Assert.IsFalse(playlist.IsCollaborative, nameof(playlist.IsCollaborative));
            Assert.AreEqual(5, playlist.NumberOfTracks, nameof(playlist.NumberOfTracks));
            Assert.AreEqual(0, playlist.NumberOfFans, nameof(playlist.NumberOfFans));
            Assert.AreEqual("https://www.deezer.com/playlist/300", playlist.Link, nameof(playlist.Link));
            Assert.AreEqual("https://www.deezer.com/playlist/300?utm_source=deezer&utm_content=playlist-300&utm_term=0_1562078987&utm_medium=web", playlist.ShareLink, nameof(playlist.ShareLink));

            Assert.IsNotNull(playlist.Creator, nameof(playlist.Creator));
            Assert.AreEqual(203, playlist.Creator.Id, "Creator.Id");
            Assert.AreEqual("anonymous", playlist.Creator.Username, "Creator.Username");
            Assert.IsNull(playlist.Creator.Link, "Creator.ShareLink");
        }

        [Test]
        public void GetTrackById()
        {
            handler.Content = base.GetServerResponse("track");

            ITrack track = session.Tracks.GetById(DUMMY_ID, CancellationToken.None)
                                        .Result;

            Assert.IsNotNull(track, nameof(track));
            Assert.AreEqual(3135556, track.Id, nameof(track.Id));
            Assert.AreEqual("Harder, Better, Faster, Stronger", track.Title, nameof(track.Title));
            Assert.AreEqual("Harder, Better, Faster, Stronger", track.ShortTitle, nameof(track.ShortTitle));
            Assert.AreEqual("GBDUW0000059", track.ISRC, nameof(track.ISRC));
            Assert.AreEqual("https://www.deezer.com/track/3135556", track.Link, nameof(track.Link));
            Assert.AreEqual("https://www.deezer.com/track/3135556?utm_source=deezer&utm_content=track-3135556&utm_term=0_1562079211&utm_medium=web", track.ShareLink, nameof(track.ShareLink));
            Assert.AreEqual(224, track.Duration, nameof(track.Duration));
            Assert.AreEqual(4, track.TrackNumber, nameof(track.TrackNumber));
            Assert.AreEqual(1, track.DiscNumber, nameof(track.DiscNumber));
            Assert.AreEqual(759175, track.Rank, nameof(track.Rank));
            Assert.AreEqual(new DateTime(2001, 03, 07), track.ReleaseDate, nameof(track.ReleaseDate));
            Assert.IsFalse(track.IsExplicit, nameof(track.IsExplicit));
            Assert.AreEqual("https://cdns-preview-d.dzcdn.net/stream/c-deda7fa9316d9e9e880d2c6207e92260-5.mp3", track.PreviewLink, nameof(track.PreviewLink));
            Assert.AreEqual(123.4f, track.BPM, nameof(track.BPM));
            Assert.AreEqual(-12.4f, track.Gain, nameof(track.Gain));


            Assert.IsNotNull(track.AvailableIn, nameof(track.AvailableIn));
            var countries = track.AvailableIn.ToList();
            Assert.AreEqual(209, countries.Count, "AvailableIn.Count");
   

            Assert.IsNotNull(track.Contributors, nameof(track.Contributors));
            var contributors = track.Contributors.ToList();
            Assert.AreEqual(1, contributors.Count, "contributors.Count");

            Assert.AreEqual(27, contributors[0].Id, "contributors[0].Id");
            Assert.AreEqual("Daft Punk", contributors[0].Name, "contributors[0].Name");

            Assert.IsNotNull(track.Artist, nameof(track.Artist));
            Assert.AreEqual(27, track.Artist.Id, "Artist.Id");
            Assert.AreEqual("Daft Punk", track.Artist.Name, "Artist.Name");

            Assert.IsNotNull(track.Album, nameof(track.Album));
            Assert.AreEqual(302127, track.Album.Id, "Album.Id");
            Assert.AreEqual("Discovery", track.Album.Title, "Album.Title");
            Assert.AreEqual("Discovery", track.AlbumName, nameof(track.AlbumName));
        }

        [Test]
        public void GetRadioById()
        {
            handler.Content = base.GetServerResponse("radio");

            IRadio radio = session.Radio.GetById(DUMMY_ID, CancellationToken.None)
                                        .Result;


            Assert.IsNotNull(radio, nameof(radio));
            Assert.AreEqual(6, radio.Id, nameof(radio.Id));
            Assert.AreEqual("Elektronikus zene", radio.Title, nameof(radio.Title));
            Assert.AreEqual("Elektronikus zene description", radio.Description, nameof(radio.Description));
            Assert.AreEqual("https://www.deezer.com/mixes/genre/6?utm_source=deezer&utm_content=mixes-genre-6&utm_term=0_1562079884&utm_medium=web", radio.ShareLink, nameof(radio.ShareLink));
        }

        
        [Test]
        public void GetUserById()
        {
            handler.Content = base.GetServerResponse("user");

            IUserProfile user = session.User.GetById(DUMMY_ID, CancellationToken.None)
                                            .Result;

            Assert.IsNotNull(user, nameof(user));
            Assert.AreEqual(5, user.Id, nameof(user.Id));
            Assert.AreEqual("Daniel Marhely", user.Username, nameof(user.Username));
            Assert.AreEqual("https://www.deezer.com/profile/5", user.Link, nameof(user.Link));
            Assert.AreEqual("JP", user.Country, nameof(user.Country));
        }

        [Test]
        public void GetPodcastById()
        {
            handler.Content = base.GetServerResponse("podcast");

            IPodcast podcast = session.Podcasts.GetById(DUMMY_ID, CancellationToken.None)
                                            .Result;

            Assert.IsNotNull(podcast, nameof(podcast));
            Assert.AreEqual(2888112, podcast.Id, nameof(podcast.Id));
            Assert.AreEqual("The Rest Is History", podcast.Title, nameof(podcast.Title));
            Assert.AreEqual("The Rest is History description.", podcast.Description, nameof(podcast.Description));
            Assert.AreEqual(3774, podcast.Fans, nameof(podcast.Fans));
            Assert.True(podcast.Available, nameof(podcast.Available));
            Assert.AreEqual("https://www.deezer.com/show/2888112", podcast.Link, nameof(podcast.Link));
        }

        [Test]
        public void GetEpisodeById()
        {
            handler.Content = base.GetServerResponse("episode");

            IEpisode episode = session.Episodes.GetById(DUMMY_ID, CancellationToken.None)
                                            .Result;

            Assert.IsNotNull(episode, nameof(episode));
            Assert.AreEqual(606723512, episode.Id, nameof(episode.Id));
            Assert.AreEqual(new DateTime(2024, 2, 26, 00, 10, 00), episode.ReleaseDate, nameof(episode.ReleaseDate));
            Assert.AreEqual("423. Carthage vs. Rome: The Wolf at the Gates (Part 3)", episode.Title, nameof(episode.Title));
            Assert.AreEqual("Episode 606723512 description", episode.Description, nameof(episode.Description));
            Assert.AreEqual(3089, episode.Duration, nameof(episode.Duration));
            Assert.True(episode.Available, nameof(episode.Available));
            Assert.AreEqual("https://www.deezer.com/episode/606723512", episode.Link, nameof(episode.Link));
        }
    }
}
