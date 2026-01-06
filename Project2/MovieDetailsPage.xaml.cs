using Microsoft.Maui.Controls;

namespace Project2;

public partial class MovieDetailsPage : ContentPage
{
    public MovieDetailsPage(Movie movie)
    {
        InitializeComponent();

        if (movie == null)
            return;

        EmojiLabel.Text = movie.Emoji;
        TitleLabel.Text = movie.Title;
        YearLabel.Text = $"Year: {movie.Year}";
        DirectorLabel.Text = $"Director: {movie.Director}";
        GenreLabel.Text = $"Genre: {string.Join(", ", movie.Genre ?? new System.Collections.Generic.List<string>())}";
        RatingLabel.Text = $"Rating: {movie.Rating}";
    }
}
