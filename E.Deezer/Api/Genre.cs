using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E.Deezer.Api
{
    /// <summary>
    /// A Deezer Genre Object
    /// </summary>
    public interface IGenre
    {
        /// <summary>
        /// Deezer library ID number
        /// </summary>
        uint Id { get; set; }

        /// <summary>
        /// Genre name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Link to genre's image
        /// </summary>
        string Picture { get; set; }
    }

    internal class Genre : IGenre
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }


        public override string ToString()
        {
            return string.Format("E.Deezer: Genre({0} ({1}))", Name, Id);
        }
    }
}
