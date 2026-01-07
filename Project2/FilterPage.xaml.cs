namespace Project2;

public partial class FilterPage : ContentPage
{
    public FilterPage()
    {
        InitializeComponent();
        GenrePicker.SelectedIndex = 0;
        DirectorEntry.Text = MovieFilter.DirectorSearch;
        RatingSlider.Value = MovieFilter.MinimumRating;
    }

    private async void OnApplyFilterClicked(object sender, EventArgs e)
    {
        MovieFilter.SelectedGenre = GenrePicker.SelectedItem?.ToString() ?? "All";
        MovieFilter.DirectorSearch = DirectorEntry.Text?.Trim() ?? string.Empty;
        MovieFilter.MinimumRating = RatingSlider.Value;

        await Navigation.PopAsync(); // Go back to MainPage
    }
}
