using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E.Deezer.Api
{
    public interface IGenre
    {
        uint Id { get; set; }
        string Name { get; set; }
        string Picture { get; set; }
    }

    internal class Genre : IGenre
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }


        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Id);
        }
    }
}
