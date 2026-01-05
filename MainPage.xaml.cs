using System.Reflection;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;

namespace Project2;

public partial class MainPage : ContentPage
{
    string _rawJson = string.Empty;
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

            // 1) Try app package asset (preferred)
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("moviesemoji.json");
                using var reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                json = await reader.ReadToEndAsync();
            }
            catch
            {
                json = string.Empty;
            }

            // 2) Fallback: try embedded resource (if you accidentally added as EmbeddedResource)
            if (string.IsNullOrEmpty(json))
            {
                var fallbackAsm = Assembly.GetExecutingAssembly();
                var resourceName = fallbackAsm.GetManifestResourceNames()
                                      .FirstOrDefault(n => n.EndsWith("moviesemoji.json", StringComparison.OrdinalIgnoreCase));
                if (resourceName != null)
                {
                    using var stream = fallbackAsm.GetManifestResourceStream(resourceName);
                    if (stream != null)
                    {
                        using var reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                        json = await reader.ReadToEndAsync();
                    }
                }
            }

            _rawJson = json;
            // keep JsonLabel for debugging
            JsonLabel.Text = string.IsNullOrEmpty(_rawJson) ? "(no JSON loaded)" : _rawJson;

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            _allMovies = string.IsNullOrEmpty(json)
                ? new List<Movie>()
                : JsonSerializer.Deserialize<List<Movie>>(json, options) ?? new List<Movie>();

            // Debug: show count in title so you can see whether any items were loaded
            this.Title = $"Movies ({_allMovies.Count})";

            MoviesView.ItemsSource = _allMovies;

            // If no movies loaded, optionally show a debug alert
            if (_allMovies.Count == 0)
            {
                await DisplayAlert("JSON Debug", "No movies loaded!", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error loading JSON", ex.Message, "OK");
        }
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        string filter = e.NewTextValue?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(filter))
        {
            MoviesView.ItemsSource = _allMovies;
            return;
        }

        var filtered = _allMovies
            .Where(m => (!string.IsNullOrEmpty(m.Title) && m.Title.Contains(filter, StringComparison.OrdinalIgnoreCase))
                        || (!string.IsNullOrEmpty(m.Director) && m.Director.Contains(filter, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        MoviesView.ItemsSource = filtered;
    }

    private async void MoviesView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection == null || e.CurrentSelection.Count == 0)
            return;

        if (e.CurrentSelection.FirstOrDefault() is Movie selectedMovie)
        {
            try
            {
                // Navigate to details page passing the selected Movie via constructor
                var detailsPage = new MovieDetailsPage(selectedMovie);
                await Navigation.PushAsync(detailsPage);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Navigation Error", ex.Message, "OK");
            }
            finally
            {
                // Clear selection to prevent duplicate navigation
                MoviesView.SelectedItem = null;
            }
        }
    }
}
