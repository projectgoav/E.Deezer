namespace E.Deezer.Endpoints
{
    internal class Endpoints
    {
        public Endpoints(IDeezerClient client)
        {
            this.Albums = new AlbumEndpoint(client);
            this.Artists = new ArtistEndpoint(client);
            this.Charts = new ChartsEndpoint(client);
            this.Comments = new CommentsEndpoint(client);
            this.Genre = new GenreEndpoint(client);
            this.Playlists = new PlaylistsEndpoint(client);
            this.Radio = new RadioEndpoint(client);
            this.Track = new TrackEndpoint(client);
            this.User = new UserEndpoint(client);
            this.Search = new SearchEndpoint(client);
            this.Podcasts = new PodcastEndpoint(client);
            this.Episodes = new EpisodeEndpoint(client);
        }


        public IAlbumEndpoint Albums { get; }

        public IArtistEndpoint Artists { get; }

        public IChartsEndpoint Charts { get; }

        public ICommentsEndpoint Comments { get; }

        public IGenreEndpoint Genre { get; }

        public IPlaylistsEndpoint Playlists { get; }

        public IRadioEndpoint Radio { get; }

        public ITrackEndpoint Track { get; }

        public IUserEndpoint User { get; }

        public ISearchEndpoint Search { get; }

        public IPodcastEndpoint Podcasts { get; }

        public IEpisodeEndpoint Episodes { get; }
    }
}
