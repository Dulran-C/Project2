using Microsoft.Maui.Controls; // For Application, NavigationPage
using Microsoft.Maui.Storage;  // For Preferences

namespace Project2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Check if user is already logged in
            string username = Preferences.Get("CurrentUser", null);

            if (string.IsNullOrEmpty(username))
            {
                MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                MainPage = new NavigationPage(new MainPage());
            }
        }
    }
}
