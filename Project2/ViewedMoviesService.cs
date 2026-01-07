using System.Collections.Generic;

namespace Project2;

public static class ViewedMoviesService
{
    private static List<Movie> ViewedMovies = new();

    public static void LoadHistory(string username)
    {
        ViewedMovies = new List<Movie>(); // TODO: load per user
    }

    public static void Clear() => ViewedMovies.Clear();

    public static void AddViewed(Movie m)
    {
        if (!ViewedMovies.Contains(m))
            ViewedMovies.Add(m);
    }

    public static bool HasViewed(Movie m) => ViewedMovies.Contains(m);
}
