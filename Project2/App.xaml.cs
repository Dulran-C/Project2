using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace Project2;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        string username = Preferences.Get("CurrentUser", null);

        if (string.IsNullOrEmpty(username))
        {
            MainPage = new NavigationPage(new LoginPage());
        }
        else
        {
            LoadUserData(username);
            MainPage = new MainTabs();
        }
    }

    private void LoadUserData(string username)
    {
        // Load favourites and viewed history
        FavouritesService.LoadFavourites(username);
        ViewedMoviesService.LoadHistory(username);

        // Load dark mode
        bool darkMode = Preferences.Get($"{username}_DarkMode", false);
        Application.Current.UserAppTheme = darkMode ? AppTheme.Dark : AppTheme.Light;
    }

    public static void Logout()
    {
        string username = Preferences.Get("CurrentUser", null);

        if (!string.IsNullOrEmpty(username))
        {
            FavouritesService.SaveFavourites(username);
            ViewedMoviesService.SaveHistory(username);
        }

        Preferences.Remove("CurrentUser");

        FavouritesService.Clear();
        ViewedMoviesService.Clear();

        Application.Current.MainPage = new NavigationPage(new LoginPage());
    }
}
