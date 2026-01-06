namespace Project2;

public static class FavouritesServices
{
    // Stores favourite movies globally
    public static List<Movie> Favourites { get; set; } = new();

    // Optional helper methods
    public static void AddToFavourites(Movie movie)
    {
        if (!Favourites.Contains(movie))
            Favourites.Add(movie);
    }

    public static void RemoveFromFavourites(Movie movie)
    {
        if (Favourites.Contains(movie))
            Favourites.Remove(movie);
    }

    public static bool IsFavourite(Movie movie)
    {
        return Favourites.Contains(movie);
    }
}
