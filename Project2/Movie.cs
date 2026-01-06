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

    public bool IsFavorite { get; set; }
}