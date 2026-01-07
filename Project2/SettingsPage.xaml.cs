using Microsoft.Maui.Controls;

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
        if (e.Value)
            Application.Current.UserAppTheme = AppTheme.Dark;
        else
            Application.Current.UserAppTheme = AppTheme.Light;

        Preferences.Set("DarkMode", e.Value);

    }

}
