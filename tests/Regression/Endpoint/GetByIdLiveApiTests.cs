using System;
using System.Linq;
using System.Threading.Tasks;

using System.Threading;
using System.Net.Http;

using NUnit.Framework;
using NUnit.Framework.Legacy;

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

            ClassicAssert.IsNotNull(album, nameof(album));
            ClassicAssert.AreEqual(302127, album.Id, nameof(album.Id));
            ClassicAssert.AreEqual("Discovery", album.Title, nameof(album.Title));
            ClassicAssert.AreEqual("724384960650", album.UPC, nameof(album.UPC));
            ClassicAssert.AreEqual("https://www.deezer.com/album/302127", album.Link, nameof(album.Link));

            ClassicAssert.AreEqual(113, album.GenreId, nameof(album.GenreId));
            ClassicAssert.IsNotNull(album.Genre, nameof(album.Genre));

            ClassicAssert.IsNotNull(album.Contributors, nameof(album.Contributors));

            ClassicAssert.IsNotNull(album.Artist, nameof(album.Artist));
            ClassicAssert.AreEqual(27, album.Artist.Id, "Artist.Id");
            ClassicAssert.AreEqual("Daft Punk", album.Artist.Name, "Artist.Name");

            ClassicAssert.AreEqual(14, album.TrackCount, nameof(album.TrackCount));
        }


        [Test]
        public void GetAlbumByIdWithNonExistingIdShouldThrowException()
        {
            var ex = ClassicAssert.Throws<AggregateException>(() => session.Albums.GetById(1u, CancellationToken.None)
                                                                        .Wait());

            ClassicAssert.That(ex.GetBaseException() is DeezerException);

            ClassicAssert.AreEqual(DeezerException.NOT_FOUND_MESSAGE, ex.GetBaseException()
                                                                 .Message);
        }


        [Test]
        public void GetArtistById()
        {
            IArtist artist = session.Artists.GetById(1, CancellationToken.None)
                                            .Result;


            ClassicAssert.IsNotNull(artist, nameof(artist));
            ClassicAssert.AreEqual(1, artist.Id, nameof(artist.Id));
            ClassicAssert.AreEqual("The Beatles", artist.Name, nameof(artist.Name));
            ClassicAssert.AreEqual("https://www.deezer.com/artist/1", artist.Link, nameof(artist.Link));
            ClassicAssert.That(artist.NumberOfAlbums, Is.AtLeast(20), nameof(artist.NumberOfAlbums));
            ClassicAssert.That(artist.NumberOfFans, Is.GreaterThan(1000000), nameof(artist.NumberOfFans));
            ClassicAssert.IsTrue(artist.HasSmartRadio, nameof(artist.HasSmartRadio));
        }

        [Test]
        public void GetArtistByIdWithNonExistingIdShouldThrowException()
        {
            var ex = ClassicAssert.Throws<AggregateException>(() => session.Artists.GetById(9999999u, CancellationToken.None)
                                                                            .Wait());


            ClassicAssert.That(ex.GetBaseException() is DeezerException);

            ClassicAssert.AreEqual(DeezerException.NOT_FOUND_MESSAGE, ex.GetBaseException()
                                                                 .Message);
        }


        [Test]
        public async Task GetPlaylistById()
        {
            IPlaylist playlist = await session.Playlists.GetById(300u, CancellationToken.None);


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

            ClassicAssert.IsNotNull(playlist.Creator, nameof(playlist.Creator));

            ClassicAssert.AreEqual("anonymous", playlist.Creator.Username, "Creator.Username");
        }

        [Test]
        public void GetPlaylistByIdWithNonExistingIdShouldThrowException()
        {
            var ex = ClassicAssert.Throws<AggregateException>(() => session.Playlists.GetById(1u, CancellationToken.None)
                                                                              .Wait());

            ClassicAssert.That(ex.GetBaseException() is DeezerException);

            ClassicAssert.AreEqual(DeezerException.NOT_FOUND_MESSAGE, ex.GetBaseException()
                                                                 .Message);
        }

        [Test]
        public void GetTrackById()
        {
            ITrack track = session.Tracks.GetById(3135556u, CancellationToken.None)
                                         .Result;


            ClassicAssert.IsNotNull(track, nameof(track));
            ClassicAssert.AreEqual(3135556, track.Id, nameof(track.Id));
            ClassicAssert.AreEqual("Harder, Better, Faster, Stronger", track.Title, nameof(track.Title));
            ClassicAssert.AreEqual("Harder, Better, Faster, Stronger", track.ShortTitle, nameof(track.ShortTitle));
            ClassicAssert.AreEqual("GBDUW0000059", track.ISRC, nameof(track.ISRC));
            ClassicAssert.AreEqual("https://www.deezer.com/track/3135556", track.Link, nameof(track.Link));
            ClassicAssert.AreEqual(224, track.Duration, nameof(track.Duration));
            ClassicAssert.AreEqual(4, track.TrackNumber, nameof(track.TrackNumber));
            ClassicAssert.AreEqual(1, track.DiscNumber, nameof(track.DiscNumber));

            ClassicAssert.That(track.Rank != uint.MaxValue, nameof(track.Rank));
            ClassicAssert.That(track.Rank != uint.MinValue, nameof(track.Rank));

            ClassicAssert.AreEqual(new DateTime(2001, 03, 07), track.ReleaseDate, nameof(track.ReleaseDate));
            ClassicAssert.IsFalse(track.IsExplicit, nameof(track.IsExplicit));
            ClassicAssert.AreEqual("https://cdns-preview-d.dzcdn.net/stream/c-deda7fa9316d9e9e880d2c6207e92260-5.mp3", track.PreviewLink, nameof(track.PreviewLink));
            ClassicAssert.AreEqual(123.4f, track.BPM, nameof(track.BPM));
            ClassicAssert.AreEqual(-12.4f, track.Gain, nameof(track.Gain));


            ClassicAssert.IsNotNull(track.AvailableIn, nameof(track.AvailableIn));
            var countries = track.AvailableIn.ToList();
            ClassicAssert.That(countries.Count(), Is.AtLeast(100), "AvailableIn.Count");


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

            ClassicAssert.NotNull(track.Artwork);

            var artworks = new string[]
            {
                track.Artwork.Small,
                track.Artwork.Medium,
                track.Artwork.Large,
                track.Artwork.ExtraLarge
            };

            ClassicAssert.That(artworks.Any(x => !string.IsNullOrEmpty(x)));
        }

        [Test]
        public void GetTrackByIdWithNonExistingIdShouldThrowException()
        {
            var ex = ClassicAssert.Throws<AggregateException>(() => session.Tracks.GetById(1u, CancellationToken.None)
                                                                           .Wait());
        
            ClassicAssert.That(ex.GetBaseException() is DeezerException);

            ClassicAssert.AreEqual(DeezerException.NOT_FOUND_MESSAGE, ex.GetBaseException()
                                                                 .Message);
        }


        [Test]
        public void GetRadioById()
        {
            IRadio radio = session.Radio.GetById(6u, CancellationToken.None)
                                        .Result;

            ClassicAssert.IsNotNull(radio, nameof(radio));
            ClassicAssert.AreEqual(6, radio.Id, nameof(radio.Id));

            ClassicAssert.That(!string.IsNullOrEmpty(radio.Title), nameof(radio.Title));
            ClassicAssert.That(!string.IsNullOrEmpty(radio.Description), nameof(radio.Description));

        }

        [Test]
        public void GetRadioByIdWithNonExistingIdShouldThrowException()
        {
            var ex = ClassicAssert.Throws<AggregateException>(() => session.Radio.GetById(1u, CancellationToken.None)
                                                                           .Wait());

            ClassicAssert.That(ex.GetBaseException() is DeezerException);

            ClassicAssert.AreEqual(DeezerException.NOT_FOUND_MESSAGE, ex.GetBaseException()
                                                                 .Message);
        }

        [Test]
        public void GetUserById()
        {
            IUserProfile user = session.User.GetById(5u, CancellationToken.None)    
                                            .Result;

            ClassicAssert.IsNotNull(user, nameof(user));
            ClassicAssert.AreEqual(5, user.Id, nameof(user.Id));
            ClassicAssert.AreEqual("Daniel Marhely", user.Username, nameof(user.Username));
            ClassicAssert.AreEqual("https://www.deezer.com/profile/5", user.Link, nameof(user.Link));
            ClassicAssert.AreEqual("JP", user.Country, nameof(user.Country));
        }

        [Test]
        public void GetUserByIdWithNonExistingIdShouldThrowException()
        {
            var ex = ClassicAssert.Throws<AggregateException>(() => session.User.GetById(1u, CancellationToken.None)
                                                                         .Wait());

            ClassicAssert.That(ex.GetBaseException() is DeezerException);

            ClassicAssert.AreEqual(DeezerException.NOT_FOUND_MESSAGE, ex.GetBaseException()
                                                                 .Message);
        }

    }
}
