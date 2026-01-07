using Microsoft.Maui.Controls;

namespace Project2;

public partial class MovieDetailsPage : ContentPage
{
    public MovieDetailsPage(Movie movie)
    {
        InitializeComponent();

        // Set the BindingContext so XAML bindings work
        BindingContext = movie;

        // Add a back button to toolbar (optional)
        ToolbarItems.Add(new ToolbarItem
        {
            Text = "Back",
            Command = new Command(async () =>
            {
                if (Navigation.NavigationStack.Count > 0)
                    await Navigation.PopAsync();
            })
        });
    }
}
