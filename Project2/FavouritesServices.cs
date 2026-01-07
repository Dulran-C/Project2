namespace Project2;

public static class FavouritesServices
{
    public static List<string> FavouriteTitles { get; set; } = new();

    public static void AddToFavourites(Movie movie)
    {
        if (!FavouriteTitles.Contains(movie.Title))
            FavouriteTitles.Add(movie.Title);
    }

    public static void RemoveFromFavourites(Movie movie)
    {
        if (FavouriteTitles.Contains(movie.Title))
            FavouriteTitles.Remove(movie.Title);
    }

    public static bool IsFavourite(Movie movie)
    {
        return FavouriteTitles.Contains(movie.Title);
    }
}
