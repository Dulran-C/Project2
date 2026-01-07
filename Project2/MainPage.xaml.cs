using System.Reflection;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Project2;

public partial class MainPage : ContentPage
{
    List<Movie> _allMovies = new();

    public MainPage()
    {
        InitializeComponent();
        LoadJson();
    }

    private async void LoadJson()
    {
        try
        {
            string json = string.Empty;
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("moviesemoji.json");
                using var reader = new StreamReader(stream);
                json = await reader.ReadToEndAsync();
            }
            catch { json = string.Empty; }

            if (string.IsNullOrEmpty(json))
            {
                var asm = Assembly.GetExecutingAssembly();
                var resName = asm.GetManifestResourceNames().FirstOrDefault(n => n.EndsWith("moviesemoji.json"));
                if (resName != null)
                {
                    using var stream = asm.GetManifestResourceStream(resName);
                    using var reader = new StreamReader(stream);
                    json = await reader.ReadToEndAsync();
                }
            }

            _allMovies = string.IsNullOrEmpty(json)
                ? new List<Movie>()
                : JsonSerializer.Deserialize<List<Movie>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Movie>();

            MoviesView.ItemsSource = _allMovies;
            this.Title = $"Movies ({_allMovies.Count})";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void ApplyFilters()
    {
        var filtered = _allMovies
            .Where(m =>
                (MovieFilter.SelectedGenre == "All" || m.Genre.Contains(MovieFilter.SelectedGenre)) &&
                (string.IsNullOrWhiteSpace(MovieFilter.SelectedDirector) ||
                 MovieFilter.SelectedDirector.Equals("All", StringComparison.OrdinalIgnoreCase) ||
                 m.Director.Contains(MovieFilter.SelectedDirector, StringComparison.OrdinalIgnoreCase)) &&
                m.Rating >= MovieFilter.MinimumRating
            )
            .ToList();

        string search = SearchBarControl.Text?.Trim() ?? "";
        if (!string.IsNullOrWhiteSpace(search))
        {
            filtered = filtered.Where(m =>
                m.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                m.Director.Contains(search, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }

        if (ShowFavouritesSwitch.IsToggled)
            filtered = filtered.Where(FavouritesServices.IsFavourite).ToList();

        MoviesView.ItemsSource = filtered;
    }


    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e) => ApplyFilters();
    private void ShowFavouritesSwitch_Toggled(object sender, ToggledEventArgs e) => ApplyFilters();

    private void OnFavouriteClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var movie = (Movie)button.BindingContext;

        if (FavouritesServices.IsFavourite(movie))
        {
            FavouritesServices.RemoveFromFavourites(movie);
            button.Text = "♡";
        }
        else
        {
            FavouritesServices.AddToFavourites(movie);
            button.Text = "♥";
        }

        ApplyFilters();
    }

    private void FavouriteButton_Loaded(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var movie = (Movie)button.BindingContext;
        button.Text = FavouritesServices.IsFavourite(movie) ? "♥" : "♡";
    }

    private async void MoviesView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Movie movie)
        {
            await Navigation.PushAsync(new MovieDetailsPage(movie));
            MoviesView.SelectedItem = null;
        }
    }
}
