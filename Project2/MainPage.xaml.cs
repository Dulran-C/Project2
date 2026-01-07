using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching; // For MainThread
using Microsoft.Maui.Storage;     // For FileSystem


namespace Project2
{
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
            ApplyFilters(); // initial filter
        }

        private async Task LoadJsonAsync()
        {
            try
            {
                string json = await LoadJsonFileAsync("moviesemoji.json");

                if (string.IsNullOrEmpty(json))
                    return;

                _allMovies = await Task.Run(() =>
                {
                    var movies = JsonSerializer.Deserialize<List<Movie>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<Movie>();

                    foreach (var m in movies)
                    {
                        m.Emoji = StripLeadingQuestionMarks(m.Emoji);
                        m.Title = StripLeadingQuestionMarks(m.Title);
                        m.Director = StripLeadingQuestionMarks(m.Director);
                        if (m.Genre != null)
                            for (int i = 0; i < m.Genre.Count; i++)
                                m.Genre[i] = StripLeadingQuestionMarks(m.Genre[i]);
                    }

                    return movies;
                });

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    MoviesView.ItemsSource = _allMovies;
                    this.Title = $"Movies ({_allMovies.Count})";
                });
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
            catch
            {
                var asm = Assembly.GetExecutingAssembly();
                var resName = asm.GetManifestResourceNames().FirstOrDefault(n => n.EndsWith(filename));
                if (resName != null)
                {
                    using var stream = asm.GetManifestResourceStream(resName);
                    using var reader = new StreamReader(stream);
                    return await reader.ReadToEndAsync();
                }
            }
            return string.Empty;
        }

        private static string StripLeadingQuestionMarks(string s) =>
            string.IsNullOrEmpty(s) ? string.Empty : s.TrimStart('?', '\uFFFD').Trim();

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
                ViewedMoviesService.AddViewed(movie); // Make sure this service exists
                await Navigation.PushAsync(new MovieDetailsPage(movie));
                MoviesView.SelectedItem = null;
            }
        }

        private async void OnOpenFilterClicked(object sender, EventArgs e)
        {
            var filterPage = new FilterPage();
            filterPage.OnFilterApplied = ApplyFilters; // callback after filter
            await Navigation.PushAsync(filterPage);
        }
    }
}
