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
            FavouritesService.LoadFavourites(username);
            ViewedMoviesService.LoadHistory(username);
            MainPage = new NavigationPage(new MainTabs());
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
