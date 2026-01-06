using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Project2
{
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
            if (value == null)
                return string.Empty;

            if (value is IEnumerable<string> genres)
            {
                var emojis = genres
                    .Where(g => !string.IsNullOrWhiteSpace(g))
                    .Select(g => Map.TryGetValue(g.Trim(), out var emoji) ? emoji : "🎬")
                    .Distinct();

                return string.Join(" ", emojis);
            }

            if (value is string s)
            {
                var parts = s.Split(',', ';');
                var emojis = parts
                    .Select(p => p.Trim())
                    .Where(p => p.Length > 0)
                    .Select(p => Map.TryGetValue(p, out var emoji) ? emoji : "🎬")
                    .Distinct();

                return string.Join(" ", emojis);
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}