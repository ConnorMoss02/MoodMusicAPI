using MoodMusicAPI.Models;

namespace MoodMusicAPI.Services;

public class MoodAnalyzer : IMoodAnalyzer
{
    private readonly Dictionary<string, (List<string> keywords, SpotifyMusicAttributes attributes)> _moodKeywords;

    public MoodAnalyzer()
    {
        _moodKeywords = new Dictionary<string, (List<string> keywords, SpotifyMusicAttributes attributes)>
        {
            { "Happy", (
                new List<string> { "happy", "excited", "joyful", "fun", "energetic", "laugh", "smile", "great", "wonderful" },
                new SpotifyMusicAttributes {
                    Valence = 0.8,
                    Energy = 0.7,
                    Tempo = 120,
                    Danceability = 0.7,
                    PreferredGenres = "pop,happy,dance"
                })
            },
            { "Sad", (
                new List<string> { "sad", "depressed", "heartbroken", "lonely", "upset", "down", "blue", "hurt", "pain" },
                new SpotifyMusicAttributes {
                    Valence = 0.2,
                    Energy = 0.3,
                    Tempo = 80,
                    Danceability = 0.3,
                    PreferredGenres = "sad,acoustic,ambient"
                })
            },
            { "Anxious", (
                new List<string> { "anxious", "nervous", "worried", "stressed", "exams", "uncertain", "fear", "panic" },
                new SpotifyMusicAttributes {
                    Valence = 0.4,
                    Energy = 0.6,
                    Tempo = 100,
                    Danceability = 0.4,
                    PreferredGenres = "chill,ambient,instrumental"
                })
            },
            { "Motivated", (
                new List<string> { "motivated", "inspired", "determined", "strong", "powerful", "confident", "unstoppable" },
                new SpotifyMusicAttributes {
                    Valence = 0.7,
                    Energy = 0.8,
                    Tempo = 130,
                    Danceability = 0.6,
                    PreferredGenres = "workout,pop,rock"
                })
            },
            { "Peaceful", (
                new List<string> { "peaceful", "calm", "relaxed", "serene", "tranquil", "quiet", "gentle" },
                new SpotifyMusicAttributes {
                    Valence = 0.6,
                    Energy = 0.2,
                    Tempo = 70,
                    Danceability = 0.3,
                    PreferredGenres = "ambient,classical,meditation"
                })
            }
        };
    }

    public MoodAnalysisResult DetectMood(string text)
    {
        text = text.ToLower();
        var words = text.Split(new[] { ' ', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

        var moodScores = new Dictionary<string, double>();
        var totalWords = words.Length;

        // Initialize scores
        foreach (var mood in _moodKeywords.Keys)
        {
            moodScores[mood] = 0;
        }

        // Calculate mood scores with word proximity weighting
        foreach (var word in words)
        {
            foreach (var mood in _moodKeywords)
            {
                if (mood.Value.keywords.Any(k => word.Contains(k)))
                {
                    moodScores[mood.Key] += 1.0;
                }
            }
        }

        // Normalize scores
        foreach (var mood in moodScores.Keys.ToList())
        {
            moodScores[mood] = moodScores[mood] / totalWords;
        }

        var bestMatch = moodScores.OrderByDescending(m => m.Value).First();
        var intensity = bestMatch.Value;

        // If no clear mood is detected, return neutral
        if (intensity == 0)
        {
            return new MoodAnalysisResult
            {
                PrimaryMood = "Neutral",
                Intensity = 0,
                MoodScores = moodScores,
                MusicAttributes = new SpotifyMusicAttributes
                {
                    Valence = 0.5,
                    Energy = 0.5,
                    Tempo = 100,
                    Danceability = 0.5,
                    PreferredGenres = "pop,indie,alternative"
                }
            };
        }

        return new MoodAnalysisResult
        {
            PrimaryMood = bestMatch.Key,
            Intensity = intensity,
            MoodScores = moodScores,
            MusicAttributes = _moodKeywords[bestMatch.Key].attributes
        };
    }
}