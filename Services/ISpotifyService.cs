using MoodMusicAPI.Models;

namespace MoodMusicAPI.Services;

public interface ISpotifyService
{
    Task<List<MusicRecommendation>> GetRecommendationsAsync(MoodAnalysisResult moodAnalysis);
}