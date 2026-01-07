using System.Collections.Generic;

namespace Project2;

public static class FavouritesService
{
    private static List<Movie> Favourites = new();

    public static void LoadFavourites(string username)
    {
        Favourites = new List<Movie>(); // TODO: load per user
    }

    public static void Clear() => Favourites.Clear();

    public static void AddToFavourites(Movie m)
    {
        if (!Favourites.Contains(m))
            Favourites.Add(m);
    }

    public static void RemoveFromFavourites(Movie m)
    {
        if (Favourites.Contains(m))
            Favourites.Remove(m);
    }

    public static bool IsFavourite(Movie m) => Favourites.Contains(m);
}
