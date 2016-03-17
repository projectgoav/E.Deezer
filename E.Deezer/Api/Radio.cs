using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

using RestSharp.Deserializers;

namespace E.Deezer.Api
{
    public interface IRadio
    {
        uint Id { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        string ShareLink{ get; set; }

        //Methods
        Task<IEnumerable<ITrack>> GetTracks();

        string GetPicture(PictureSize aSize);
        bool HasPicture(PictureSize aSize);
    }

    internal class Radio : IRadio
    {
        public uint Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ShareLink { get; set; }

        //Pictures
        [DeserializeAs(Name = "cover_small")]
        private string SMPicture { get; set; }

        [DeserializeAs(Name = "cover_medium")]
        private string MDPicture { get; set; }

        [DeserializeAs(Name = "cover_big")]
        private string BGPicture { get; set; }


        public string GetPicture(PictureSize aSize)
        {
            switch (aSize)
            {
                case PictureSize.SMALL: { return string.IsNullOrEmpty(SMPicture) ? string.Empty : SMPicture; }
                case PictureSize.MEDIUM: { return string.IsNullOrEmpty(MDPicture) ? string.Empty : MDPicture; }
                case PictureSize.LARGE: { return string.IsNullOrEmpty(BGPicture) ? string.Empty : BGPicture; }
                default: { return string.Empty; }
            }
        }

        public bool HasPicture(PictureSize aSize)
        {
            switch (aSize)
            {
                case PictureSize.SMALL: { return string.IsNullOrEmpty(SMPicture); }
                case PictureSize.MEDIUM: { return string.IsNullOrEmpty(MDPicture); }
                case PictureSize.LARGE: { return string.IsNullOrEmpty(BGPicture); }
                default: { return false; }
            }
        }

        public Task<IEnumerable<ITrack>> GetTracks()
        {
            throw new NotImplementedException();
        }
    }
}
