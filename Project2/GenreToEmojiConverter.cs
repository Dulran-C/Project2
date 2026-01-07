using System.Globalization;
using Microsoft.Maui.Controls;

namespace Project2;

public class GenreToEmojiConverter : IValueConverter
{
    static readonly Dictionary<string, string> Map = new(StringComparer.OrdinalIgnoreCase)
    {
        { "Drama", "🎭" },
        { "Crime", "🕵️" },
        { "Action", "🔫" },
        { "Sci-Fi", "👽" },
        { "Science Fiction", "👽" },
        { "Romance", "❤️" },
        { "Comedy", "😂" },
        { "Horror", "👻" },
        { "Animation", "🎨" },
        { "Fantasy", "🧚‍♀️" },
        { "Biography", "👤" },
        { "History", "🏛️" },
        { "Mystery", "🕵️" },
        { "Thriller", "🔪" },
        { "Music", "🎵" },
        { "War", "⚔️" },
        { "Western", "🤠" },
        { "Family", "👪" },
        { "Adventure", "🧭" }
    };

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IEnumerable<string> genres)
        {
            return string.Join(" ",
                genres
                .Where(g => !string.IsNullOrWhiteSpace(g))
                .Select(g => Map.TryGetValue(g.Trim(), out var emoji) ? emoji : "🎬")
                .Distinct()
            );
        }

        return "🎬";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
