using Microsoft.Maui.Controls;
using System;

namespace Project2;

public partial class FilterPage : ContentPage
{
    public Action OnFilterApplied;

    public FilterPage()
    {
        InitializeComponent();
        GenrePicker.SelectedIndex = 0;
        RatingSlider.Value = MovieFilter.MinimumRating;
        DirectorEntry.Text = MovieFilter.DirectorSearch;

        RatingSlider.ValueChanged += (s, e) =>
        {
            RatingValueLabel.Text = $"Rating: {e.NewValue:F1}";
        };
    }

    private void OnApplyFilterClicked(object sender, EventArgs e)
    {
        MovieFilter.SelectedGenre = GenrePicker.SelectedItem?.ToString() ?? "All";
        MovieFilter.DirectorSearch = DirectorEntry.Text?.Trim() ?? "";
        MovieFilter.MinimumRating = RatingSlider.Value;

        OnFilterApplied?.Invoke();
        Navigation.PopAsync();
    }
}
