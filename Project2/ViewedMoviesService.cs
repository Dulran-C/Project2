using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project2
{
    public static class ViewedMoviesService
    {
        private static List<Movie> _viewedMovies = new();
        private static string _currentUser;

        public static void SetCurrentUser(string username)
        {
            _currentUser = username;
            LoadHistory(username).Wait();
        }

        public static void AddViewed(Movie movie)
        {
            if (!_viewedMovies.Exists(m => m.Title == movie.Title))
            {
                _viewedMovies.Add(movie);
                SaveHistory(_currentUser).Wait();
            }
        }

        public static List<Movie> GetViewedMovies() => _viewedMovies;

        public static async Task SaveHistory(string username)
        {
            if (string.IsNullOrEmpty(username)) return;

            string path = Path.Combine(FileSystem.AppDataDirectory, $"{username}_viewed.json");
            string json = JsonSerializer.Serialize(_viewedMovies);
            await File.WriteAllTextAsync(path, json);
        }

        public static async Task LoadHistory(string username)
        {
            if (string.IsNullOrEmpty(username)) return;

            _currentUser = username;
            string path = Path.Combine(FileSystem.AppDataDirectory, $"{username}_viewed.json");

            if (File.Exists(path))
            {
                string json = await File.ReadAllTextAsync(path);
                _viewedMovies = JsonSerializer.Deserialize<List<Movie>>(json) ?? new List<Movie>();
            }
            else
            {
                _viewedMovies = new List<Movie>();
            }
        }
    }
}
