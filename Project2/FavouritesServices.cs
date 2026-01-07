using Microsoft.Maui.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Project2;

public static class FavouritesService
{
    private static List<Movie> _favourites = new();

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

    public static bool IsFavourite(Movie movie)
    {
        return _favourites.Any(m => m.Title == movie.Title);
    }

    public static List<Movie> GetFavourites() => _favourites;

    public static void Clear()
    {
        _favourites.Clear();
    }

    // 🔹 USER MEMORY METHODS 🔹

    public static void LoadFavourites(string username)
    {
        string json = Preferences.Get($"{username}_Favourites", null);

        if (!string.IsNullOrEmpty(json))
        {
            _favourites = JsonSerializer.Deserialize<List<Movie>>(json)
                          ?? new List<Movie>();
        }
        else
        {
            _favourites = new List<Movie>();
        }
    }

    public static void SaveFavourites(string username)
    {
        string json = JsonSerializer.Serialize(_favourites);
        Preferences.Set($"{username}_Favourites", json);
    }
}
