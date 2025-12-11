using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using Microsoft.Maui.Storage;

public class MoviesViewModel : INotifyPropertyChanged
{
    public ObservableCollection<MovieEmoji> Movies { get; set; } = new();

    public MoviesViewModel()
    {
        _ = LoadJsonAsync();
    }

    private async Task LoadJsonAsync()
    {
        try
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("moviesemoji.json");
            using var reader = new StreamReader(stream);
            string json = await reader.ReadToEndAsync();

            var list = JsonSerializer.Deserialize<List<MovieEmoji>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (list != null)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Movies.Clear();
                    foreach (var m in list)
                        Movies.Add(m);
                });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading moviesemoji.json: {ex.Message}");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
