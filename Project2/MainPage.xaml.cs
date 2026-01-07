using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Dispatching;
using Microsoft.Maui.Storage;
using System.IO;
using System.Reflection;

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
                {
                    m.Emoji = string.IsNullOrEmpty(m.Emoji) ? "🎬" : m.Emoji;
                }

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    MoviesView.ItemsSource = _allMovies;
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

        // Event handlers wired from XAML
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e) => ApplyFilters();
        private void ShowFavouritesSwitch_Toggled(object sender, ToggledEventArgs e) => ApplyFilters();

        private async void OnOpenFilterClicked(object sender, EventArgs e)
        {
            var filterPage = new FilterPage();
            filterPage.OnFilterApplied = ApplyFilters;
            await Navigation.PushAsync(filterPage);
        }

        private void OnFavouriteClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var movie = (Movie)button.BindingContext;

            if (FavouritesService.IsFavourite(movie))
                FavouritesService.RemoveFromFavourites(movie);
            else
                FavouritesService.AddToFavourites(movie);

            ApplyFilters();
        }

        private void FavouriteButton_Loaded(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var movie = (Movie)button.BindingContext;
            button.Text = FavouritesService.IsFavourite(movie) ? "♥" : "♡";
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
    }
}
