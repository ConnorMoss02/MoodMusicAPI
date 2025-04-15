using MoodMusicAPI.Models;
using MoodMusicAPI.Services;

namespace MoodMusicAPI.Endpoints;

public static class MoodEndpoints
{
    public static void MapMoodEndpoints(this WebApplication app)
    {
        app.MapPost("/api/mood/analyze", (IMoodAnalyzer moodAnalyzer, MoodRequest request) =>
        {
            var moodAnalysis = moodAnalyzer.DetectMood(request.Text);
            return Results.Ok(new
            {
                analysis = moodAnalysis,
                message = $"Analyzed mood from text: {request.Text}"
            });
        })
        .WithName("AnalyzeMood")
        .WithTags("Mood Analysis")
        .Produces(200)
        .ProducesValidationProblem();

        app.MapPost("/api/mood/items", async(IMoodAnalyzer moodAnalyzer, ISpotifyService spotifyService, MoodRequest request) =>
        {
            var moodAnalysis = moodAnalyzer.DetectMood(request.Text);
            var recommendations = await spotifyService.GetItemAsync(moodAnalysis);

            return Results.Ok(new
            {
                mood = moodAnalysis,
                recommendations,
                message = $"Found music items based on your mood: {moodAnalysis.PrimaryMood}"
            });
        })
        .WithName("GetMoodBasedRecommendations")
        .WithTags("Music Recommendations")
        .Produces(200)
        .ProducesValidationProblem();
    }
}