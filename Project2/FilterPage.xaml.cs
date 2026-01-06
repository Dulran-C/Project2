namespace Project2;

public partial class FilterPage : ContentPage
{
    public FilterPage()
    {
        InitializeComponent();
        GenrePicker.SelectedIndex = 0;
    }

    private async void OnApplyFilterClicked(object sender, EventArgs e)
    {
        MovieFilter.SelectedGenre = GenrePicker.SelectedItem?.ToString() ?? "All";
        MovieFilter.MinimumRating = RatingSlider.Value;

        await DisplayAlert("Filter Applied",
            $"Genre: {MovieFilter.SelectedGenre}\nMin Rating: {MovieFilter.MinimumRating:F1}",
            "OK");
    }
}
