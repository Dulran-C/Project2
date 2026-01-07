using Microsoft.Maui.Controls;

namespace Project2;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();

        // Initialize dark mode toggle
        string username = Preferences.Get("CurrentUser", null);
        if (!string.IsNullOrEmpty(username))
        {
            DarkModeSwitch.IsToggled = Preferences.Get($"{username}_DarkMode", false);
        }
    }

    private void DarkModeSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        string username = Preferences.Get("CurrentUser", null);
        if (!string.IsNullOrEmpty(username))
        {
            Preferences.Set($"{username}_DarkMode", e.Value);
            Application.Current.UserAppTheme = e.Value ? AppTheme.Dark : AppTheme.Light;
        }
    }

    private void OnLogoutClicked(object sender, EventArgs e)
    {
        App.Logout();
    }
}
