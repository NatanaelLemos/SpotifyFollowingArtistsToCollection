using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyAddCollection
{
    class Program
    {
        const string token = "Enter here https://developer.spotify.com/web-api/console/get-album/ and Get OAuth Token";
        static void Main(string[] args)
        {
            List<ArtistItem> alreadyArtists = new List<ArtistItem>();
            string lastArtist = "";

            while (true)
            {
                var got = GetArtists(lastArtist);
                if (got.Count() == 0) break;

                alreadyArtists.AddRange(got);
                lastArtist = got.LastOrDefault().id;
            }

            List<Album> albums = new List<Album>();

            foreach (var artist in alreadyArtists)
            {
                albums.AddRange(GetAlbums(artist));
            }


            List<Album> savedAlbums = new List<Album>();

            foreach (var album in albums)
            {
                if (savedAlbums.Any(s => s.name.ToLower() == album.name.ToLower() && s.artistName.ToLower() == s.artistName.ToLower())) continue;

                SaveAlbum(album);
                savedAlbums.Add(album);
            }
        }

        private static IEnumerable<ArtistItem> GetArtists(string after)
        {
            var afterP = string.IsNullOrEmpty(after) ? "" : $"&after={after}";
            using (var getClient = new HttpClient())
            {
                getClient.DefaultRequestHeaders.Clear();
                getClient.DefaultRequestHeaders.Add("Accept", "application/json");
                getClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var result = getClient.GetAsync($"https://api.spotify.com/v1/me/following?type=artist{afterP}&limit=49").Result;
                var resultString = result.Content.ReadAsStringAsync().Result;

                var wrapper = JsonConvert.DeserializeObject<ArtistWrapper>(resultString);

                return wrapper.artists.items;
            }
        }

        private static IEnumerable<Album> GetAlbums(ArtistItem artist)
        {
            using (var getClient = new HttpClient())
            {
                getClient.DefaultRequestHeaders.Clear();
                getClient.DefaultRequestHeaders.Add("Accept", "application/json");
                getClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var result = getClient.GetAsync($"https://api.spotify.com/v1/artists/{artist.id}/albums?album_type=album").Result;
                var resultString = result.Content.ReadAsStringAsync().Result;

                var wrapper = JsonConvert.DeserializeObject<AlbumWrapper>(resultString);

                var items = wrapper.items;

                foreach (var item in items)
                {
                    item.artistName = artist.name;
                }

                return items;
            }
        }

        private static void SaveAlbum(Album album)
        {
            using (var putClient = new HttpClient())
            {
                putClient.DefaultRequestHeaders.Clear();
                putClient.DefaultRequestHeaders.Add("Accept", "application/json");
                putClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var result = putClient.PutAsync($"https://api.spotify.com/v1/me/albums?ids={album.id}", new StringContent("")).Result;
            }
        }
    }
}
