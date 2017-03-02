using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyAddCollection
{
    public class AlbumWrapper
    {
        public IEnumerable<Album> items { get; set; }
    }

    public class Album
    {
        public string artistName { get; set; }

        public string id { get; set; }
        public string name { get; set; }
    }
}
