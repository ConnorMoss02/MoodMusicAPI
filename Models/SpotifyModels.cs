using System.Text.Json.Serialization;

namespace MoodMusicAPI.Models;

public class SpotifySettings
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}

public class SpotifyTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }
    
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
}

public class SpotifyRecommendationsResponse
{
    public List<SpotifyTrack> Tracks { get; set; }
}

public class SpotifyTrack
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Uri { get; set; }
    public List<SpotifyArtist> Artists { get; set; }
    public SpotifyAlbum Album { get; set; }
    public int DurationMs { get; set; }
    public bool Explicit { get; set; }
    public int Popularity { get; set; }
    public string PreviewUrl { get; set; }
    public SpotifyExternalUrls ExternalUrls { get; set; }
    public List<string> AvailableMarkets { get; set; }
}

public class SpotifyArtist
{
    public string Id { get; set; }
    public string Name { get; set; }
    public SpotifyExternalUrls ExternalUrls { get; set; }
}

public class SpotifyAlbum
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Uri { get; set; }
    public List<SpotifyImage> Images { get; set; }
    public string ReleaseDate { get; set; }
}

public class SpotifyImage
{
    public string Url { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
}

public class SpotifyExternalUrls
{
    public string Spotify { get; set; }
}

public class SpotifyGenreResponse
{
    public List<string> Genres { get; set; }
}