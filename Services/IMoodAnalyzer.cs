using MoodMusicAPI.Models;

namespace MoodMusicAPI.Services;

public interface IMoodAnalyzer
{
    MoodAnalysisResult DetectMood(string text);
}