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
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("moviesemoji.json");
                using var reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                json = await reader.ReadToEndAsync();
            }
            catch { json = string.Empty; }

            if (string.IsNullOrEmpty(json))
            {
                var asm = Assembly.GetExecutingAssembly();
                var resourceName = asm.GetManifestResourceNames()
                                      .FirstOrDefault(n => n.EndsWith("moviesemoji.json", System.StringComparison.OrdinalIgnoreCase));
                if (resourceName != null)
                {
                    using var stream = asm.GetManifestResourceStream(resourceName);
                    if (stream != null)
                    {
                        using var reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                        json = await reader.ReadToEndAsync();
                    }
                }
            }

            _rawJson = json;
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _allMovies = string.IsNullOrEmpty(json)
                ? new List<Movie>()
                : JsonSerializer.Deserialize<List<Movie>>(json, options) ?? new List<Movie>();

            foreach (var m in _allMovies)
            {
                m.Emoji = StripLeadingQuestionMarks(m.Emoji);
                m.Title = StripLeadingQuestionMarks(m.Title);
                m.Director = StripLeadingQuestionMarks(m.Director);
                if (m.Genre != null)
                    for (int i = 0; i < m.Genre.Count; i++)
                        m.Genre[i] = StripLeadingQuestionMarks(m.Genre[i]);
            }

            MoviesView.ItemsSource = _allMovies;
            this.Title = $"Movies ({_allMovies.Count})";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error loading JSON", ex.Message, "OK");
        }
    }

    private static string StripLeadingQuestionMarks(string s)
    {
        if (string.IsNullOrEmpty(s)) return string.Empty;
        return s.TrimStart('?', '\uFFFD').Trim();
    }

    private void ApplyFilters()
    {
        var filtered = _allMovies
            .Where(m => (MovieFilter.SelectedGenre == "All" || m.Genre.Contains(MovieFilter.SelectedGenre)) &&
                        m.Rating >= MovieFilter.MinimumRating)
            .ToList();

        string search = SearchBarControl.Text?.Trim() ?? "";
        if (!string.IsNullOrWhiteSpace(search))
        {
            filtered = filtered.Where(m =>
                (!string.IsNullOrEmpty(m.Title) && m.Title.Contains(search, System.StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(m.Director) && m.Director.Contains(search, System.StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }

        if (ShowFavouritesSwitch.IsToggled)
            filtered = filtered.Where(m => FavouritesServices.IsFavourite(m)).ToList();

        MoviesView.ItemsSource = filtered;
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        ApplyFilters();
    }

    private void ShowFavouritesSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        ApplyFilters();
    }

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

    private async void MoviesView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection == null || e.CurrentSelection.Count == 0) return;

        if (e.CurrentSelection.FirstOrDefault() is Movie selectedMovie)
        {
            try
            {
                var detailsPage = new MovieDetailsPage(selectedMovie);
                await Navigation.PushAsync(detailsPage);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Navigation Error", ex.Message, "OK");
            }
            finally
            {
                MoviesView.SelectedItem = null;
            }
        }
    }
}
