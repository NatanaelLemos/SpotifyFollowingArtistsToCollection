using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyAddCollection
{
    public class ArtistWrapper
    {
        public Artist artists { get; set; }
    }

    public class Artist
    {
        public IEnumerable<ArtistItem> items { get; set; }
    }

    public class ArtistItem
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}
