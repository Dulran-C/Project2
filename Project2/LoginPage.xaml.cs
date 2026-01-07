using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace Project2;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private void OnLoginClicked(object sender, EventArgs e)
    {
        string username = UsernameEntry.Text?.Trim();
        if (string.IsNullOrWhiteSpace(username))
        {
            DisplayAlert("Error", "Please enter a username", "OK");
            return;
        }

        Preferences.Set("CurrentUser", username);

        FavouritesService.LoadFavourites(username);
        ViewedMoviesService.LoadHistory(username);

        // Go to main tabs
        Application.Current.MainPage = new MainTabs();
    }
}
