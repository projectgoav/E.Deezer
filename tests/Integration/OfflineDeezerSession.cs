using System;

namespace E.Deezer.Tests.Integration
{
    /// <summary>
    /// Class to initialize a session which
    /// will never call the actual Deezer API.
    /// </summary>
    class OfflineDeezerSession
    {
        private OfflineDeezerSession()
            : this(new OfflineMessageHandler()) { }

        private OfflineDeezerSession(OfflineMessageHandler messageHandler)
        {
            MessageHandler = messageHandler;
            Library = DeezerSession.CreateNew(MessageHandler);
        }

        /// <summary>
        /// Gets the messagehandler which responsibe to
        /// fake the Deezer API response.
        /// </summary>
        public OfflineMessageHandler MessageHandler { get; }

        /// <summary>
        /// Gets the actual API wrapper which will never
        /// call the live Deezer API.
        /// </summary>
        public Deezer Library { get; }

        /// <summary>
        /// Gets a session which can only work with
        /// endpoints that are not token protected.
        /// </summary>
        public static OfflineDeezerSession WithoutAuthentication()
            => new OfflineDeezerSession();

        /// <summary>
        /// Gets a session which can only work with
        /// endpoints that are token protected.
        /// </summary>
        /// <returns></returns>
        public static OfflineDeezerSession WithAuthentication()
        {
            var session = new OfflineDeezerSession(
                new OfflineAuthenticationMessageHandler());

            session.Library.Login("fake-token")
                .GetAwaiter().GetResult();

            return session;
        }
    }
}
