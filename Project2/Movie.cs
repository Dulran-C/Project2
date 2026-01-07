using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Project2;

public class Movie
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("year")]
    public int Year { get; set; }

    [JsonPropertyName("genre")]
    public List<string> Genre { get; set; } = new();

    [JsonPropertyName("director")]
    public string Director { get; set; }

    [JsonPropertyName("rating")]
    public double Rating { get; set; }

    [JsonPropertyName("emoji")]
    public string Emoji { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } // new!

    public bool IsViewed { get; set; } = false;

    // Optional helper to display genre as a string
    [JsonIgnore]
    public string GenreString => Genre != null ? string.Join(", ", Genre) : "N/A";
}
