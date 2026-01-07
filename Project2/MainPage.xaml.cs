using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project2;

public partial class MainPage : ContentPage
{
    private List<Movie> _allMovies = new();

    public MainPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadJsonAsync();
        ApplyFilters();
    }

    private async Task LoadJsonAsync()
    {
        try
        {
            string json = await LoadJsonFileAsync("moviesemoji.json");
            if (string.IsNullOrEmpty(json)) return;

            _allMovies = JsonSerializer.Deserialize<List<Movie>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<Movie>();

            foreach (var m in _allMovies)
                m.Emoji = string.IsNullOrEmpty(m.Emoji) ? "🎬" : m.Emoji;

            MoviesView.ItemsSource = _allMovies;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async Task<string> LoadJsonFileAsync(string filename)
    {
        try
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(filename);
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }
        catch { }

        var candidatePaths = new List<string>
        {
            Path.Combine(AppContext.BaseDirectory ?? "", filename),
            Path.Combine(Environment.CurrentDirectory ?? "", filename),
            Path.Combine(FileSystem.AppDataDirectory, filename),
            Path.Combine(FileSystem.CacheDirectory, filename)
        }.Distinct();

        foreach (var p in candidatePaths)
        {
            try
            {
                if (File.Exists(p))
                    return await File.ReadAllTextAsync(p);
            }
            catch { }
        }

        try
        {
            var asm = Assembly.GetExecutingAssembly();
            var resName = asm.GetManifestResourceNames()
                             .FirstOrDefault(n => n.EndsWith(filename, StringComparison.OrdinalIgnoreCase));
            if (resName != null)
            {
                using var stream = asm.GetManifestResourceStream(resName);
                using var reader = new StreamReader(stream);
                return await reader.ReadToEndAsync();
            }
        }
        catch { }

        return string.Empty;
    }

    private void ApplyFilters()
    {
        var filtered = _allMovies
            .Where(m => (MovieFilter.SelectedGenre == "All" || m.Genre.Contains(MovieFilter.SelectedGenre)) &&
                        (string.IsNullOrWhiteSpace(MovieFilter.DirectorSearch) ||
                         m.Director.Contains(MovieFilter.DirectorSearch, StringComparison.OrdinalIgnoreCase)) &&
                        m.Rating >= MovieFilter.MinimumRating)
            .ToList();

        string search = SearchBarControl.Text?.Trim() ?? "";
        if (!string.IsNullOrWhiteSpace(search))
        {
            filtered = filtered
                .Where(m => m.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                            m.Director.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (ShowFavouritesSwitch.IsToggled)
            filtered = filtered.Where(FavouritesService.IsFavourite).ToList();

        MoviesView.ItemsSource = filtered;
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e) => ApplyFilters();

    private async void OnOpenFilterClicked(object sender, EventArgs e)
    {
        var filterPage = new FilterPage();
        filterPage.OnFilterApplied = ApplyFilters;
        await Navigation.PushAsync(filterPage);
    }

    private async void MoviesView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Movie movie)
        {
            ViewedMoviesService.AddViewed(movie);
            await Navigation.PushAsync(new MovieDetailsPage(movie));
            MoviesView.SelectedItem = null;
        }
    }

    private void ShowFavouritesSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        ApplyFilters();
    }

    private void OnFavouriteClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is Movie movie)
        {
            if (FavouritesService.IsFavourite(movie))
                FavouritesService.RemoveFromFavourites(movie);
            else
                FavouritesService.AddToFavourites(movie);

            // Update button text to show filled heart
            btn.Text = FavouritesService.IsFavourite(movie) ? "❤️" : "♡";

            ApplyFilters();
        }
    }
}
