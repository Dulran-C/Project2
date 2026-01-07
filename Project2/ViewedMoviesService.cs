using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace Project2;

public static class ViewedMoviesService
{
    private static List<Movie> _viewed = new();
    private static string _currentUser;

    // Called when a user logs in
    public static void LoadHistory(string username)
    {
        _currentUser = username;
        _viewed.Clear();

        string jsonPath = GetFilePath(username);
        if (File.Exists(jsonPath))
        {
            string json = File.ReadAllText(jsonPath);
            var movies = JsonSerializer.Deserialize<List<Movie>>(json);
            if (movies != null)
                _viewed = movies;
        }
    }

    public static void AddViewed(Movie movie)
    {
        if (_viewed.Any(m => m.Title == movie.Title)) return; // avoid duplicates
        _viewed.Add(movie);
        Save();
    }

    public static List<Movie> GetViewed() => _viewed;

    public static void Clear()
    {
        _viewed.Clear();
        Save();
    }

    private static void Save()
    {
        if (string.IsNullOrEmpty(_currentUser)) return;

        string json = JsonSerializer.Serialize(_viewed, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(GetFilePath(_currentUser), json);
    }

    private static string GetFilePath(string username)
    {
        return Path.Combine(FileSystem.AppDataDirectory, $"{username}_viewed.json");
    }
}
