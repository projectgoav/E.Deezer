using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E.Deezer.Api
{
    public interface ITrack : IObjectWithImage
    {
        ulong Id { get; }
        uint Disc { get; }
        uint Rank { get; }
        float BPM { get; }
        float Gain { get; }
        string Link { get; }
        uint Number { get; }
        string ISRC { get; }
        string Title { get; }
        IAlbum Album { get; }
        uint Duration { get; }
        string Preview { get; }
        IArtist Artist { get; }
        bool IsExplicit { get; }
        string AlbumName { get; }
        DateTime TimeAdd { get; }
        string ShareLink { get; }
        string ShortTitle { get; }
        string ArtistName { get; }
        DateTime ReleaseDate { get; }
        ITrack AlternativeTrack { get; }
        IEnumerable<string> AvailableIn { get; }
        IEnumerable<IArtist> Contributors { get; }

        [Obsolete("Use of IsExplicit is encouraged")]
        bool Explicit { get; }

        Task<bool> AddTrackToFavorite();
        Task<bool> RemoveTrackFromFavorite();
    }

    internal class Track : ObjectWithImage, ITrack, IDeserializable<IDeezerClient>
    {
        public ulong Id { get; set; }

        public string Title { get; set; }

        public string Link { get; set; }

        public uint Duration { get; set; }

        public string Artwork { get; set; }

        public string Preview { get; set; }

        public float BPM { get; set; }

        public float Gain { get; set; }

        public uint Rank { get; set; }

        public string ISRC { get; set; }

        public IAlbum Album => AlbumInternal;

        public IArtist Artist => ArtistInternal;

        public DateTime TimeAdd => new DateTime(TimeAddInternal);

        public ITrack AlternativeTrack => AlternativeTrackInternal;

        public IEnumerable<string> AvailableIn => AvailableInInternal;

        public IEnumerable<IArtist> Contributors => ContributorInternal;

        public string ArtistName => ArtistInternal?.Name ?? string.Empty;

        public string AlbumName => AlbumInternal?.Title ?? string.Empty;

        [Obsolete("Use of IsExplicit is encouraged")]
        public bool Explicit => IsExplicit;

        [JsonProperty(PropertyName = "share")]
        public string ShareLink { get; set; }

        [JsonProperty(PropertyName = "explicit_lyrics")]
        public bool IsExplicit { get; set; }

        [JsonProperty(PropertyName = "title_short")]
        public string ShortTitle { get; set; }

        [JsonProperty(PropertyName = "time_add")]
        public long TimeAddInternal { get; set; }

        [JsonProperty(PropertyName = "track_position")]
        public uint Number { get; set; }

        [JsonProperty(PropertyName = "disk_number")]
        public uint Disc { get; set; }

        [JsonProperty(PropertyName = "release_date")]
        public DateTime ReleaseDate { get; set; }

        [JsonProperty(PropertyName = "artist")]
        public Artist ArtistInternal { get; set; }

        [JsonProperty(PropertyName = "album")]
        public Album AlbumInternal { get; set; }

        [JsonProperty(PropertyName = "available_countries")]
        public List<String> AvailableInInternal { get; set; }

        [JsonProperty(PropertyName = "alternative")]
        public Track AlternativeTrackInternal { get; set; }

        [JsonProperty(PropertyName = "contributors")]
        public List<Artist> ContributorInternal { get; set; }

        //IDeserializable
        public IDeezerClient Client { get; set; }

        public void Deserialize(IDeezerClient aClient)
        {
            Client = aClient;

            if (ArtistInternal != null)
            {
                ArtistInternal.Deserialize(aClient);
            }

            if (AlbumInternal != null)
            {
                AlbumInternal.Deserialize(aClient);
            }
        }

        //Tracks don't often come with their own images so if there is none, we can use that from the album in which it belongs.
        public override string GetPicture(PictureSize aSize)
        {
            string url = base.GetPicture(aSize);
            return (url == string.Empty) ? AlbumInternal.GetPicture(aSize) : url;
        }

        public override bool HasPicture(PictureSize aSize)
        {
            bool baseResult = base.HasPicture(aSize);
            return (baseResult) ? baseResult : AlbumInternal.HasPicture(aSize);
        }

        public Task<bool> AddTrackToFavorite()
            => Client.User.AddTrackToFavourite(Id);

        public Task<bool> RemoveTrackFromFavorite()
            => Client.User.RemoveTrackFromFavourite(Id);

        public override string ToString()
            => string.Format("E.Deezer: Track({0} - ({1}))", Title, ArtistName);
    }
}
