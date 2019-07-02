using E.Deezer.Api;
using E.Deezer.Endpoint;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace E.Deezer.Tests.Regression.Endpoint
{
    [TestFixture]
    class BrowseEndpointTests
    {
        private static IBrowseEndpoint _browse;

        [OneTimeSetUp]
        public static void OneTimeSetUp()
        {
            _browse = DeezerSession.CreateNew().Browse;
        }

        [Test]
        public async Task GetAlbumById()
        {
            IAlbum album = await _browse.GetAlbumById(302127u);


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

            Assert.AreEqual(14, album.Tracks, nameof(album.Tracks));
        }

        [Test]
        public async Task GetArtistById()
        {
            IArtist artist = await _browse.GetArtistById(1);


            Assert.IsNotNull(artist, nameof(artist));
            Assert.AreEqual(1, artist.Id, nameof(artist.Id));
            Assert.AreEqual("The Beatles", artist.Name, nameof(artist.Name));
            Assert.AreEqual("https://www.deezer.com/artist/1", artist.Link, nameof(artist.Link));
            Assert.AreEqual(45, artist.AlbumCount, nameof(artist.AlbumCount));
            Assert.That(artist.Fans, Is.GreaterThan(1000000), nameof(artist.Fans));
            Assert.IsTrue(artist.HasSmartRadio, nameof(artist.HasSmartRadio));
        }

        [Test]
        public async Task GetPlaylistById()
        {
            IPlaylist playlist = await _browse.GetPlaylistById(300);


            Assert.IsNotNull(playlist, nameof(playlist));
            Assert.AreEqual(300, playlist.Id, nameof(playlist.Id));
            Assert.AreEqual("Hard-Fi", playlist.Title, nameof(playlist.Title));
            Assert.AreEqual("", playlist.Description, nameof(playlist.Description));
            Assert.AreEqual(1249, playlist.Duration, nameof(playlist.Duration));
            Assert.IsTrue(playlist.IsPublic, nameof(playlist.IsPublic));
            Assert.IsFalse(playlist.IsLovedTrack, nameof(playlist.IsLovedTrack));
            Assert.IsFalse(playlist.IsCollaborative, nameof(playlist.IsCollaborative));
            Assert.AreEqual(5, playlist.TrackCount, nameof(playlist.TrackCount));
            Assert.AreEqual(0, playlist.Fans, nameof(playlist.Fans));
            Assert.AreEqual("https://www.deezer.com/playlist/300", playlist.Link, nameof(playlist.Link));

            Assert.IsNotNull(playlist.Creator, nameof(playlist.Creator));
            Assert.IsNotNull(playlist.CreatorName, nameof(playlist.CreatorName));
            Assert.AreEqual(203, playlist.Creator.Id, "Creator.Id");
            Assert.AreEqual("anonymous", playlist.Creator.Username, "Creator.Username");
            Assert.IsNull(playlist.Creator.ShareLink, "Creator.ShareLink");
        }

        [Test]
        public async Task GetTrackById()
        {
            ITrack track = await _browse.GetTrackById(3135556);


            Assert.IsNotNull(track, nameof(track));
            Assert.AreEqual(3135556, track.Id, nameof(track.Id));
            Assert.AreEqual("Harder, Better, Faster, Stronger", track.Title, nameof(track.Title));
            Assert.AreEqual("Harder, Better, Faster, Stronger", track.ShortTitle, nameof(track.ShortTitle));
            Assert.AreEqual("GBDUW0000059", track.ISRC, nameof(track.ISRC));
            Assert.AreEqual("https://www.deezer.com/track/3135556", track.Link, nameof(track.Link));
            Assert.AreEqual(224, track.Duration, nameof(track.Duration));
            Assert.AreEqual(4, track.Number, nameof(track.Number));
            Assert.AreEqual(1, track.Disc, nameof(track.Disc));
            Assert.AreEqual(759175, track.Rank, nameof(track.Rank));
            Assert.AreEqual(new DateTime(2001, 03, 07), track.ReleaseDate, nameof(track.ReleaseDate));
            Assert.IsFalse(track.IsExplicit, nameof(track.IsExplicit));
            Assert.AreEqual("https://cdns-preview-d.dzcdn.net/stream/c-deda7fa9316d9e9e880d2c6207e92260-5.mp3", track.Preview, nameof(track.Preview));
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
        public async Task GetRadioById()
        {
            IRadio radio = await _browse.GetRadioById(6);


            Assert.IsNotNull(radio, nameof(radio));
            Assert.AreEqual(6, radio.Id, nameof(radio.Id));
            Assert.AreEqual("Elektronikus zene", radio.Title, nameof(radio.Title));
            Assert.AreEqual("Elektronikus zene", radio.Description, nameof(radio.Description));
        }

        [Test]
        public async Task GetUserById()
        {
            IUser user = await _browse.GetUserById(5);


            Assert.IsNotNull(user, nameof(user));
            Assert.AreEqual(5, user.Id, nameof(user.Id));
            Assert.AreEqual("Daniel Marhely", user.Name, nameof(user.Name));
            Assert.AreEqual("https://www.deezer.com/profile/5", user.Link, nameof(user.Link));
            Assert.AreEqual("JP", user.Country, nameof(user.Country));
        }
    }
}
