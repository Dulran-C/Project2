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
            // keep JsonLabel for debugging but don't overwrite it with manifest names
            JsonLabel.Text = string.IsNullOrEmpty(_rawJson) ? "(no JSON loaded)" : _rawJson;

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            _allMovies = string.IsNullOrEmpty(json)
                ? new List<Movie>()
                : JsonSerializer.Deserialize<List<Movie>>(json, options) ?? new List<Movie>();

            // Debug: show count in title so you can see whether any items were loaded
            this.Title = $"Movies ({_allMovies.Count})";

            // Remove fallback sample item so UI only shows real data
            // If no movies loaded, leave the list empty and let the UI show nothing

            MoviesView.ItemsSource = _allMovies;
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
            .Where(m => !string.IsNullOrEmpty(m.Title) &&
                        m.Title.Contains(filter, StringComparison.OrdinalIgnoreCase))
            .ToList();

        MoviesView.ItemsSource = filtered;
    }
}
