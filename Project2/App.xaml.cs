using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace Project2;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        string username = Preferences.Get("CurrentUser", null);
        bool isDarkMode = false;

        if (!string.IsNullOrEmpty(username))
        {
            isDarkMode = Preferences.Get($"{username}_DarkMode", false);
            Application.Current.UserAppTheme = isDarkMode ? AppTheme.Dark : AppTheme.Light;

            FavouritesService.LoadFavourites(username);
            ViewedMoviesService.LoadHistory(username);

            MainPage = new MainTabs();
        }
        else
        {
            Application.Current.UserAppTheme = AppTheme.Light;
            MainPage = new NavigationPage(new LoginPage());
        }
    }

    public static void Logout()
    {
        Preferences.Remove("CurrentUser");

        FavouritesService.Clear();
        ViewedMoviesService.Clear();

        Application.Current.MainPage = new NavigationPage(new LoginPage());
    }
}
