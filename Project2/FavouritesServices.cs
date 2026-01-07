using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Maui.Storage;

namespace Project2;

public static class FavouritesService
{
    private static List<Movie> _favourites = new();
    private static string _currentUser;

    public static void LoadFavourites(string username)
    {
        _currentUser = username;
        _favourites.Clear();

        string jsonPath = GetFilePath(username);
        if (File.Exists(jsonPath))
        {
            string json = File.ReadAllText(jsonPath);
            var movies = JsonSerializer.Deserialize<List<Movie>>(json);
            if (movies != null)
                _favourites = movies;
        }
    }

    public static void AddToFavourites(Movie movie)
    {
        if (_favourites.Any(m => m.Title == movie.Title)) return;
        _favourites.Add(movie);
        Save();
    }

    public static void RemoveFromFavourites(Movie movie)
    {
        _favourites.RemoveAll(m => m.Title == movie.Title);
        Save();
    }

    public static bool IsFavourite(Movie movie) => _favourites.Any(m => m.Title == movie.Title);

    public static void Clear()
    {
        _favourites.Clear();
        Save();
    }

    private static void Save()
    {
        if (string.IsNullOrEmpty(_currentUser)) return;

        string json = JsonSerializer.Serialize(_favourites, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(GetFilePath(_currentUser), json);
    }

    private static string GetFilePath(string username)
    {
        return Path.Combine(FileSystem.AppDataDirectory, $"{username}_favourites.json");
    }
}
