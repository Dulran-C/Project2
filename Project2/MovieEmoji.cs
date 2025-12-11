using System.Text.Json.Serialization;

public class MovieEmoji
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("year")]
    public int Year { get; set; }

    [JsonPropertyName("emoji")]
    public string Emoji { get; set; }

    [JsonPropertyName("genre")]
    public string Genre { get; set; }
}
