using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Moq;
using NUnit.Framework;

using E.Deezer.Api;

namespace E.Deezer.Tests.Api
{
    [TestFixture]
    public class TestChart
    {

        [Test]
        public void TestInternalsAreCorrectlyDeserialised()
        {
            var albums = new List<IAlbum>()
            {
                new Album(),
                new Album(),
                new Album(),
            };

            var artists = new List<IArtist>()
            {
                new Artist(),
                new Artist(),
                new Artist(),
                new Artist(),
            };

            var tracks = new List<ITrack>()
            {
                new Track(),
                new Track(),
            };

            var playlists = new List<IPlaylist>()
            {
                new Playlist(),
                new Playlist(),
            };


            var chart = new Chart(albums, artists, tracks, playlists);


            Assert.NotNull(chart);

            chart.Deserialize(new Mock<IDeezerClient>().Object);

            var obj = chart.Albums.Select(v => v as IDeserializable<IDeezerClient>)
                                  .Concat(chart.Artists.Select(v => v as IDeserializable<IDeezerClient>))
                                  .Concat(chart.Tracks.Select(v => v as IDeserializable<IDeezerClient>))
                                  .Concat(chart.Playlists.Select(v => v as IDeserializable<IDeezerClient>));

            foreach(var entry in obj)
            {
                Assert.NotNull(entry.Client);
            }
        }
    }
}
