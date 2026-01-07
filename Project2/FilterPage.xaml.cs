using Microsoft.Maui.Controls;

namespace Project2;

public partial class FilterPage : ContentPage
{
    public Action? OnFilterApplied; // callback for MainPage

    public FilterPage()
    {
        InitializeComponent();
        GenrePicker.SelectedIndex = 0;
        RatingSlider.Value = MovieFilter.MinimumRating;
        DirectorEntry.Text = MovieFilter.DirectorSearch;
    }

    private async void OnApplyFilterClicked(object sender, EventArgs e)
    {
        MovieFilter.SelectedGenre = GenrePicker.SelectedItem?.ToString() ?? "All";
        MovieFilter.DirectorSearch = DirectorEntry.Text?.Trim() ?? "";
        MovieFilter.MinimumRating = RatingSlider.Value;

        // Notify MainPage to refresh its CollectionView
        OnFilterApplied?.Invoke();

        await Navigation.PopAsync(); // go back to MainPage
    }
}
