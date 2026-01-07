using Microsoft.Maui.Controls;
using System;

namespace Project2
{
    public partial class FilterPage : ContentPage
    {
        // Callback to refresh MainPage after applying filters
        public Action OnFilterApplied;

        public FilterPage()
        {
            InitializeComponent();

            // Set defaults
            GenrePicker.SelectedIndex = 0;
            RatingSlider.Value = MovieFilter.MinimumRating;
            DirectorEntry.Text = MovieFilter.DirectorSearch;

            // Update label dynamically
            RatingSlider.ValueChanged += (s, e) =>
            {
                RatingValueLabel.Text = $"Rating: {e.NewValue:F1}";
            };
        }

        private void OnApplyFilterClicked(object sender, EventArgs e)
        {
            // Update the filter static class
            MovieFilter.SelectedGenre = GenrePicker.SelectedItem?.ToString() ?? "All";
            MovieFilter.DirectorSearch = DirectorEntry.Text?.Trim() ?? "";
            MovieFilter.MinimumRating = RatingSlider.Value;

            // Call the callback to refresh MainPage
            OnFilterApplied?.Invoke();

            // Go back to MainPage
            Navigation.PopAsync();
        }
    }
}
