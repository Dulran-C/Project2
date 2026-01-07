using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Maui.Storage;
using System.IO;
using System.Threading.Tasks;

namespace Project2;

public static class FavouritesService
{
    private static List<Movie> _favourites = new();

    public static IReadOnlyList<Movie> Favourites => _favourites;

    public static void AddToFavourites(Movie movie)
    {
        if (!_favourites.Any(m => m.Title == movie.Title))
            _favourites.Add(movie);
    }

    public static void RemoveFromFavourites(Movie movie)
    {
        var existing = _favourites.FirstOrDefault(m => m.Title == movie.Title);
        if (existing != null)
            _favourites.Remove(existing);
    }

    public static bool IsFavourite(Movie movie) =>
        _favourites.Any(m => m.Title == movie.Title);

    public static void Clear()
    {
        _favourites.Clear();
    }

    public static void LoadFavourites(string username)
    {
        string filePath = Path.Combine(FileSystem.AppDataDirectory, $"{username}_favourites.json");
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            _favourites = JsonSerializer.Deserialize<List<Movie>>(json) ?? new List<Movie>();
        }
    }

    public static void SaveFavourites(string username)
    {
        string filePath = Path.Combine(FileSystem.AppDataDirectory, $"{username}_favourites.json");
        var json = JsonSerializer.Serialize(_favourites);
        File.WriteAllText(filePath, json);
    }
}
