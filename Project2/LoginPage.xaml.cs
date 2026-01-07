using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System;

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

        // Load user-specific memory
        FavouritesService.LoadFavourites(username);
        ViewedMoviesService.LoadHistory(username);

        bool darkMode = Preferences.Get($"{username}_DarkMode", false);
        Application.Current.UserAppTheme = darkMode ? AppTheme.Dark : AppTheme.Light;

        Application.Current.MainPage = new MainTabs();
    }
}
