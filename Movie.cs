using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Project2;

    
    public partial class Movie
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("genre")]
        public List<string> Genre { get; set; } = new();

        [JsonPropertyName("director")]
        public string Director { get; set; } = string.Empty;

        [JsonPropertyName("rating")]
        public double Rating { get; set; }

        [JsonPropertyName("emoji")]
        public string Emoji { get; set; } = string.Empty;
    }
