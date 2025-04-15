using MoodMusicAPI.Models;

namespace MoodMusicAPI.Services;

public interface ISpotifyService
{
    Task<List<MusicRecommendation>> GetItemAsync(MoodAnalysisResult moodAnalysis);
}