using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace E.Deezer.Api
{
    public interface IEditorial
    {
        uint Id { get; set; }
        string Name { get; set; }
        int Genre { get; set; }
        string Picture { get; set; }

        IPage<IAlbum> GetSelection();
        IPage<IAlbum> GetReleases();
    }

    internal class Editorial : IEditorial
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public int Genre { get; set; }
        public string Picture { get; set; }

        public IPage<IAlbum> GetSelection()
        {
            return null;
        }

        public IPage<IAlbum> GetReleases()
        {
            return null;
        }


        public override string ToString()
        {
            return Name;
        }

    }
}
