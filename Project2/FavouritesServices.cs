using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project2
{
    public static class FavouritesServices
    {
        // In-memory list of favourites for current user
        private static List<Movie> _favourites = new();

        private static string _currentUser;

        public static void SetCurrentUser(string username)
        {
            _currentUser = username;
            LoadFavourites(username).Wait();
        }

        public static bool IsFavourite(Movie movie)
        {
            return _favourites.Exists(m => m.Title == movie.Title);
        }

        public static void AddToFavourites(Movie movie)
        {
            if (!IsFavourite(movie))
            {
                _favourites.Add(movie);
                SaveFavourites(_currentUser).Wait();
            }
        }

        public static void RemoveFromFavourites(Movie movie)
        {
            _favourites.RemoveAll(m => m.Title == movie.Title);
            SaveFavourites(_currentUser).Wait();
        }

        public static List<Movie> GetFavourites() => _favourites;

        // Save favourites to local file
        public static async Task SaveFavourites(string username)
        {
            if (string.IsNullOrEmpty(username)) return;

            string path = Path.Combine(FileSystem.AppDataDirectory, $"{username}_favourites.json");
            string json = JsonSerializer.Serialize(_favourites);
            await File.WriteAllTextAsync(path, json);
        }

        // Load favourites from local file
        public static async Task LoadFavourites(string username)
        {
            if (string.IsNullOrEmpty(username)) return;

            _currentUser = username;
            string path = Path.Combine(FileSystem.AppDataDirectory, $"{username}_favourites.json");

            if (File.Exists(path))
            {
                string json = await File.ReadAllTextAsync(path);
                _favourites = JsonSerializer.Deserialize<List<Movie>>(json) ?? new List<Movie>();
            }
            else
            {
                _favourites = new List<Movie>();
            }
        }
    }
}
