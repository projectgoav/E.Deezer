using System;
using System.Linq;
using System.Threading.Tasks;

using System.Threading;
using System.Net.Http;

using NUnit.Framework;

using E.Deezer.Api;

namespace E.Deezer.Tests.Regression.Endpoint
{
#if LIVE_API_TEST
    [TestFixture]
#else
    [Ignore("Live API tests not enabled for this configuration")]
#endif
    public class GetByIdLiveApiTests : IDisposable
    {
        private readonly DeezerSession session;


        public GetByIdLiveApiTests()
        {
            this.session = new DeezerSession(new HttpClientHandler());
        }


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
            IAlbum album = this.session.Albums.GetById(302127u, CancellationToken.None)
                                              .Result;

            Assert.IsNotNull(album, nameof(album));
            Assert.AreEqual(302127, album.Id, nameof(album.Id));
            Assert.AreEqual("Discovery", album.Title, nameof(album.Title));
            Assert.AreEqual("724384960650", album.UPC, nameof(album.UPC));
            Assert.AreEqual("https://www.deezer.com/album/302127", album.Link, nameof(album.Link));

            Assert.AreEqual(113, album.GenreId, nameof(album.GenreId));
            Assert.IsNotNull(album.Genre, nameof(album.Genre));

            Assert.IsNotNull(album.Contributors, nameof(album.Contributors));

            Assert.IsNotNull(album.Artist, nameof(album.Artist));
            Assert.AreEqual(27, album.Artist.Id, "Artist.Id");
            Assert.AreEqual("Daft Punk", album.Artist.Name, "Artist.Name");

            Assert.AreEqual(14, album.TrackCount, nameof(album.TrackCount));
        }


        [Test]
        public void GetAlbumByIdWithNonExistingIdShouldThrowException()
        {
            var ex = Assert.Throws<AggregateException>(() => session.Albums.GetById(1u, CancellationToken.None)
                                                                        .Wait());

            Assert.That(ex.GetBaseException() is DeezerException);

            Assert.AreEqual(DeezerException.NOT_FOUND_MESSAGE, ex.GetBaseException()
                                                                 .Message);
        }


        [Test]
        public void GetArtistById()
        {
            IArtist artist = session.Artists.GetById(1, CancellationToken.None)
                                            .Result;


            Assert.IsNotNull(artist, nameof(artist));
            Assert.AreEqual(1, artist.Id, nameof(artist.Id));
            Assert.AreEqual("The Beatles", artist.Name, nameof(artist.Name));
            Assert.AreEqual("https://www.deezer.com/artist/1", artist.Link, nameof(artist.Link));
            Assert.That(artist.NumberOfAlbums, Is.AtLeast(20), nameof(artist.NumberOfAlbums));
            Assert.That(artist.NumberOfFans, Is.GreaterThan(1000000), nameof(artist.NumberOfFans));
            Assert.IsTrue(artist.HasSmartRadio, nameof(artist.HasSmartRadio));
        }

        [Test]
        public void GetArtistByIdWithNonExistingIdShouldThrowException()
        {
            var ex = Assert.Throws<AggregateException>(() => session.Artists.GetById(9999999u, CancellationToken.None)
                                                                            .Wait());


            Assert.That(ex.GetBaseException() is DeezerException);

            Assert.AreEqual(DeezerException.NOT_FOUND_MESSAGE, ex.GetBaseException()
                                                                 .Message);
        }


        [Test]
        public async Task GetPlaylistById()
        {
            IPlaylist playlist = await session.Playlists.GetById(300u, CancellationToken.None);


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

            Assert.IsNotNull(playlist.Creator, nameof(playlist.Creator));

            // FIX ME
            //Assert.IsNotNull(playlist.CreatorName, nameof(playlist.CreatorName));
            //Assert.IsNull(playlist.Creator.ShareLink, "Creator.ShareLink");

            Assert.AreEqual(203, playlist.Creator.Id, "Creator.Id");
            Assert.AreEqual("anonymous", playlist.Creator.Username, "Creator.Username");
        }

        [Test]
        public void GetPlaylistByIdWithNonExistingIdShouldThrowException()
        {
            var ex = Assert.Throws<AggregateException>(() => session.Playlists.GetById(1u, CancellationToken.None)
                                                                              .Wait());

            Assert.That(ex.GetBaseException() is DeezerException);

            Assert.AreEqual(DeezerException.NOT_FOUND_MESSAGE, ex.GetBaseException()
                                                                 .Message);
        }

        [Test]
        public void GetTrackById()
        {
            ITrack track = session.Tracks.GetById(3135556u, CancellationToken.None)
                                         .Result;


            Assert.IsNotNull(track, nameof(track));
            Assert.AreEqual(3135556, track.Id, nameof(track.Id));
            Assert.AreEqual("Harder, Better, Faster, Stronger", track.Title, nameof(track.Title));
            Assert.AreEqual("Harder, Better, Faster, Stronger", track.ShortTitle, nameof(track.ShortTitle));
            Assert.AreEqual("GBDUW0000059", track.ISRC, nameof(track.ISRC));
            Assert.AreEqual("https://www.deezer.com/track/3135556", track.Link, nameof(track.Link));
            Assert.AreEqual(224, track.Duration, nameof(track.Duration));
            Assert.AreEqual(4, track.TrackNumber, nameof(track.TrackNumber));
            Assert.AreEqual(1, track.DiscNumber, nameof(track.DiscNumber));

            Assert.That(track.Rank != uint.MaxValue, nameof(track.Rank));
            Assert.That(track.Rank != uint.MinValue, nameof(track.Rank));

            Assert.AreEqual(new DateTime(2001, 03, 07), track.ReleaseDate, nameof(track.ReleaseDate));
            Assert.IsFalse(track.IsExplicit, nameof(track.IsExplicit));
            Assert.AreEqual("https://cdns-preview-d.dzcdn.net/stream/c-deda7fa9316d9e9e880d2c6207e92260-5.mp3", track.PreviewLink, nameof(track.PreviewLink));
            Assert.AreEqual(123.4f, track.BPM, nameof(track.BPM));
            Assert.AreEqual(-12.4f, track.Gain, nameof(track.Gain));

            // FIX ME
            /*
            Assert.IsNotNull(track.AvailableIn, nameof(track.AvailableIn));
            var countries = track.AvailableIn.ToList();
            Assert.AreEqual(209, countries.Count, "AvailableIn.Count");
            */

            //FIX ME
            /*
            Assert.IsNotNull(track.Contributors, nameof(track.Contributors));
            var contributors = track.Contributors.ToList();
            Assert.AreEqual(1, contributors.Count, "contributors.Count");
            Assert.AreEqual(27, contributors[0].Id, "contributors[0].Id");
            Assert.AreEqual("Daft Punk", contributors[0].Name, "contributors[0].Name");
            */

            Assert.IsNotNull(track.Artist, nameof(track.Artist));
            Assert.AreEqual(27, track.Artist.Id, "Artist.Id");
            Assert.AreEqual("Daft Punk", track.Artist.Name, "Artist.Name");

            Assert.IsNotNull(track.Album, nameof(track.Album));
            Assert.AreEqual(302127, track.Album.Id, "Album.Id");
            Assert.AreEqual("Discovery", track.Album.Title, "Album.Title");
            Assert.AreEqual("Discovery", track.AlbumName, nameof(track.AlbumName));

            Assert.NotNull(track.Artwork);

            var artworks = new string[]
            {
                track.Artwork.Small,
                track.Artwork.Medium,
                track.Artwork.Large,
                track.Artwork.ExtraLarge
            };

            Assert.That(artworks.Any(x => !string.IsNullOrEmpty(x)));
        }

        [Test]
        public void GetTrackByIdWithNonExistingIdShouldThrowException()
        {
            var ex = Assert.Throws<AggregateException>(() => session.Tracks.GetById(1u, CancellationToken.None)
                                                                           .Wait());
        
            Assert.That(ex.GetBaseException() is DeezerException);

            Assert.AreEqual(DeezerException.NOT_FOUND_MESSAGE, ex.GetBaseException()
                                                                 .Message);
        }


        [Test]
        public void GetRadioById()
        {
            IRadio radio = session.Radio.GetById(6u, CancellationToken.None)
                                        .Result;

            Assert.IsNotNull(radio, nameof(radio));
            Assert.AreEqual(6, radio.Id, nameof(radio.Id));

            Assert.That(!string.IsNullOrEmpty(radio.Title), nameof(radio.Title));
            Assert.That(!string.IsNullOrEmpty(radio.Description), nameof(radio.Description));

        }

        [Test]
        public void GetRadioByIdWithNonExistingIdShouldThrowException()
        {
            var ex = Assert.Throws<AggregateException>(() => session.Radio.GetById(1u, CancellationToken.None)
                                                                           .Wait());

            Assert.That(ex.GetBaseException() is DeezerException);

            Assert.AreEqual(DeezerException.NOT_FOUND_MESSAGE, ex.GetBaseException()
                                                                 .Message);
        }

        /* FIX ME: UserProfile .GetById support
        [Test]
        public void GetUserById()
        {
            IUserProfile user = session.User.GetUserById(5u, CancellationToken.None)    
                                            .Result;


            Assert.IsNotNull(user, nameof(user));
            Assert.AreEqual(5, user.Id, nameof(user.Id));
            Assert.AreEqual("Daniel Marhely", user.Username, nameof(user.Username));
            Assert.AreEqual("https://www.deezer.com/profile/5", user.ShareLink, nameof(user.ShareLink));
            Assert.AreEqual("JP", user.Country, nameof(user.Country));
        }

        [Test]
        public void GetUserByIdWithNonExistingIdShouldThrowException()
        {
            var ex = Assert.ThrowsAsync<DeezerException>(
                async () => await _browse.GetUserById(1u));

            Assert.AreEqual(DeezerException.NOT_FOUND_MESSAGE, ex.Message);
        }
        */
    }
}
