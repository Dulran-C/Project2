using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System;

namespace Project2;

public partial class MovieDetailsPage : ContentPage
{
    private readonly Movie _movie;

    public MovieDetailsPage(Movie movie)
    {
        InitializeComponent();
        _movie = movie;

        BindingContext = _movie;

        MarkAsViewed();
    }

    private void MarkAsViewed()
    {
        string username = Preferences.Get("CurrentUser", null);
        if (username == null) return;

        if (!ViewedMoviesService.GetViewed().Exists(m => m.Title == _movie.Title))
        {
            ViewedMoviesService.AddViewed(_movie);
            ViewedMoviesService.SaveHistory(username);
        }
    }
}
