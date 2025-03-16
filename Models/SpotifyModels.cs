namespace MoodMusicAPI.Models;

public class SpotifySettings
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}

public class SpotifyTokenResponse
{
    public string AccessToken { get; set; }
    public string TokenType { get; set; }
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
}

public class SpotifyArtist
{
    public string Id { get; set; }
    public string Name { get; set; }
}