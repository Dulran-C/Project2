using Microsoft.Maui.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Project2;

public static class ViewedMoviesService
{
    private static List<Movie> _viewedMovies = new();

    public static void AddViewed(Movie movie)
    {
        if (!_viewedMovies.Any(m => m.Title == movie.Title))
            _viewedMovies.Add(movie);
    }

    public static List<Movie> GetViewed() => _viewedMovies;

    public static void Clear() => _viewedMovies.Clear();

    public static void LoadHistory(string username)
    {
        string json = Preferences.Get($"{username}_ViewedMovies", null);
        if (!string.IsNullOrEmpty(json))
        {
            _viewedMovies = JsonSerializer.Deserialize<List<Movie>>(json) ?? new List<Movie>();
        }
    }

    public static void SaveHistory(string username)
    {
        string json = JsonSerializer.Serialize(_viewedMovies);
        Preferences.Set($"{username}_ViewedMovies", json);
    }
}
