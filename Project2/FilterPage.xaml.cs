namespace Project2;

public partial class FilterPage : ContentPage
{
    public FilterPage()
    {
        InitializeComponent();
        GenrePicker.SelectedIndex = 0;
    }

    private void OnApplyFilterClicked(object sender, EventArgs e)
    {
        MovieFilter.SelectedGenre = GenrePicker.SelectedItem?.ToString() ?? "All";
        MovieFilter.SelectedDirector = DirectorEntry.Text?.Trim() ?? "All";
        MovieFilter.MinimumRating = RatingSlider.Value;

        // Optional: show alert to confirm
        DisplayAlert("Filter Applied",
                     $"Genre: {MovieFilter.SelectedGenre}\n" +
                     $"Director: {MovieFilter.SelectedDirector}\n" +
                     $"Min Rating: {MovieFilter.MinimumRating:F1}",
                     "OK");
    }
}
