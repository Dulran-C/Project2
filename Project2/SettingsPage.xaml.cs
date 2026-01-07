namespace Project2;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
        ThemePicker.SelectedIndex = 0;
    }

    private void FontSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        Application.Current.Resources["FontSize"] = e.NewValue;
    }

    private void ThemePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        Application.Current.UserAppTheme = ThemePicker.SelectedIndex switch
        {
            1 => AppTheme.Light,
            2 => AppTheme.Dark,
            _ => AppTheme.Unspecified
        };
    }
}
