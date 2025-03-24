namespace MoodMusicAPI.Models;

public class MoodRequest
{
    public string? Text { get; set; }
}

public class MoodAnalysisResult
{
    public string PrimaryMood { get; set; }
    public double Intensity { get; set; }
    public Dictionary<string, double> MoodScores { get; set; }
    public SpotifyMusicAttributes MusicAttributes { get; set; }
}

public class SpotifyMusicAttributes
{
    public double Valence { get; set; }  // Musical positiveness
    public double Energy { get; set; }   // Energy level from 0.0 to 1.0
    public double Tempo { get; set; }    // Beats per minute
    public double Danceability { get; set; }
    public string PreferredGenres { get; set; }
}

public class MusicRecommendation
{
    public string TrackName { get; set; }
    public string ArtistName { get; set; }
    public string SpotifyUri { get; set; }
    public SpotifyMusicAttributes Attributes { get; set; }
    public string RecommendationReason { get; set; }
    
    // Additional track information
    public string AlbumName { get; set; }
    public string AlbumImageUrl { get; set; }
    public int DurationMs { get; set; }
    public bool Explicit { get; set; }
    public int Popularity { get; set; }
    public string PreviewUrl { get; set; }
    public string SpotifyUrl { get; set; }
    public string ReleaseDate { get; set; }
}