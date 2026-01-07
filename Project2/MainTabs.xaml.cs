namespace Project2;

public partial class MainTabs : TabbedPage
{
	public MainTabs()
	{
		InitializeComponent();

		Children.Clear();
		Children.Add(new NavigationPage(new MainPage()) { Title = "Movies" });
        Children.Add(new NavigationPage(new FilterPage()) { Title = "Filter" });
        Children.Add(new NavigationPage(new SettingsPage()) { Title = "Settings" });


    }


}