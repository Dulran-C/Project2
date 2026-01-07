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
        // Optional: implement dark mode
    }
}
