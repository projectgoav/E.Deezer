using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using RestSharp.Deserializers;

namespace E.Deezer.Api
{
    public interface IGenre : IObjectWithImage
    {

        uint Id { get; set; }
        string Name { get; set; }

        //METHODS
        Task<IEnumerable<IArtist>> GetArtists();
        Task<IEnumerable<IArtist>> GetArtists(uint aCount);
        Task<IEnumerable<IArtist>> GetArtists(uint aStart, uint aCount);

        Task<IEnumerable<IAlbum>> GetSelection();
        Task<IEnumerable<IAlbum>> GetSelection(uint aCount);
        Task<IEnumerable<IAlbum>> GetSelection(uint aStart, uint aCount);

        Task<IEnumerable<IAlbum>> GetReleases();
        Task<IEnumerable<IAlbum>> GetReleases(uint aCount);
        Task<IEnumerable<IAlbum>> GetReleases(uint aStart, uint aCount);


        //Charts!

        Task<IEnumerable<IAlbum>> GetAlbumChart();
        Task<IEnumerable<IAlbum>> GetAlbumChart(uint aCount);
        Task<IEnumerable<IAlbum>> GetAlbumChart(uint aStart, uint aCount);

        Task<IEnumerable<IArtist>> GetArtistChart();
        Task<IEnumerable<IArtist>> GetArtistChart(uint aCount);
        Task<IEnumerable<IArtist>> GetArtistChart(uint aStart, uint aCount);

        Task<IEnumerable<IPlaylist>> GetPlaylistChart();
        Task<IEnumerable<IPlaylist>> GetPlaylistChart(uint aCount);
        Task<IEnumerable<IPlaylist>> GetPlaylistChart(uint aStart, uint aCount);

        Task<IEnumerable<ITrack>> GetTrackChart();
        Task<IEnumerable<ITrack>> GetTrackChart(uint aCount);
        Task<IEnumerable<ITrack>> GetTrackChart(uint aStart, uint aCount);


        //TODO
        //Task<IBook<IPodcast>> GetPodcasts();

        //Task<IBook<IRadio>> GetRadios();

    }

    internal class Genre : ObjectWithImage, IGenre, IDeserializable<DeezerClient>
    {
        public uint Id { get; set; }
        public string Name { get; set; }

        public DeezerClient Client { get; set; }
        public void Deserialize(DeezerClient aClient) { Client = aClient; }

        //Methods

        public Task<IEnumerable<IArtist>> GetArtists() { return GetArtists(0, Client.ResultSize); }
        public Task<IEnumerable<IArtist>> GetArtists(uint aCount) { return GetArtists(0, aCount); }
        public Task<IEnumerable<IArtist>> GetArtists(uint aStart, uint aCount) { return Get<Artist, IArtist>("genre/{id}/artists", aStart, aCount); }

        public Task<IEnumerable<IAlbum>> GetSelection() { return GetSelection(0, Client.ResultSize); }
        public Task<IEnumerable<IAlbum>> GetSelection(uint aCount) { return GetSelection(0, aCount); }
        public Task<IEnumerable<IAlbum>> GetSelection(uint aStart, uint aCount) { return Get<Album, IAlbum>("editorial/{id}/selection", aStart, aCount); }

        public Task<IEnumerable<IAlbum>> GetReleases() { return GetReleases(0, Client.ResultSize); }
        public Task<IEnumerable<IAlbum>> GetReleases(uint aCount) { return GetReleases(0, aCount); }
        public Task<IEnumerable<IAlbum>> GetReleases(uint aStart, uint aCount) { return Get<Album, IAlbum>("editorial/{id}/releases", aStart, aCount); }

        //Charting

        public Task<IEnumerable<IAlbum>> GetAlbumChart() { return GetAlbumChart(0, Client.ResultSize); }
        public Task<IEnumerable<IAlbum>> GetAlbumChart(uint aCount) { return GetAlbumChart(0, aCount); }
        public Task<IEnumerable<IAlbum>> GetAlbumChart(uint aStart, uint aCount) { return Get<Album, IAlbum>("chart/{id}/albums", aStart, aCount); }

        public Task<IEnumerable<IArtist>> GetArtistChart() { return GetArtistChart(0, Client.ResultSize); }
        public Task<IEnumerable<IArtist>> GetArtistChart(uint aCount) { return GetArtistChart(0, aCount); }
        public Task<IEnumerable<IArtist>> GetArtistChart(uint aStart, uint aCount) { return Get<Artist, IArtist>("chart/{id}/artists", aStart, aCount); }

        public Task<IEnumerable<IPlaylist>> GetPlaylistChart() { return GetPlaylistChart(0, Client.ResultSize); }
        public Task<IEnumerable<IPlaylist>> GetPlaylistChart(uint aCount) { return GetPlaylistChart(0, aCount); }
        public Task<IEnumerable<IPlaylist>> GetPlaylistChart(uint aStart, uint aCount) { return Get<Playlist, IPlaylist>("chart/{id}/playlists", aStart, aCount); }

        public Task<IEnumerable<ITrack>> GetTrackChart() { return GetTrackChart(0, Client.ResultSize); }
        public Task<IEnumerable<ITrack>> GetTrackChart(uint aCount) { return GetTrackChart(0, aCount); }
        public Task<IEnumerable<ITrack>> GetTrackChart(uint aStart, uint aCount) { return Get<Track, ITrack>("chart/{id}/tracks", aStart, aCount); }


        //Internal wrapper around get for all genre methods :)
        private Task<IEnumerable<TDest>> Get<TSource, TDest>(string aMethod, uint aStart, uint aCount) where TSource : TDest, IDeserializable<DeezerClient>
        {
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", Id)
            };

            return Client.Get<TSource>(aMethod, parms, aStart, aCount).ContinueWith<IEnumerable<TDest>>((aTask) =>
            {
                return Client.Transform<TSource, TDest>(aTask.Result);
            }, Client.CancellationToken, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
        }



        public override string ToString()
        {
            return string.Format("E.Deezer: Genre({0} ({1}))", Name, Id);
        }
    }

}
