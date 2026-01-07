using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System;

namespace Project2;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    private void OnLogoutClicked(object sender, EventArgs e)
    {
        App.Logout();
    }

    private void OnDarkModeToggled(object sender, ToggledEventArgs e)
    {
        string username = Preferences.Get("CurrentUser", null);
        if (!string.IsNullOrEmpty(username))
        {
            Preferences.Set($"{username}_DarkMode", e.Value);
        }

        Application.Current.UserAppTheme = e.Value ? AppTheme.Dark : AppTheme.Light;
    }
}
