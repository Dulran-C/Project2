using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage; // For Preferences
using System;
using System.Threading.Tasks;

namespace Project2;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string username = UsernameEntry.Text?.Trim();
        Preferences.Set("CurrentUser", username);

        FavouritesServices.SetCurrentUser(username);
        ViewedMoviesService.SetCurrentUser(username);

        if (string.IsNullOrWhiteSpace(username))
        {
            await DisplayAlert("Error", "Please enter a username", "OK");
            return;
        }

        // Save username in Preferences for session persistence
        Preferences.Set("CurrentUser", username);

        

        // Navigate to main page inside a NavigationPage
        Application.Current.MainPage = new NavigationPage(new MainPage());
    }
}
