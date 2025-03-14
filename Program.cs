
// Creates new web application 
// "builder" holds all of the config settings and services (i.e. API requests)
var builder = WebApplication.CreateBuilder(args);

// Add Swagger and API Explorer
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Creates an instance of the web app
// "app" holds the actual API routes, middleware, and logic.
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Defines a POST request and returns a JSON response 
app.MapPost("/api/mood/analyze", (MoodRequest request) =>
{
    var detectedMood = DetectMood(request.Text);
    return Results.Ok(new { mood = detectedMood, message = $"Detected mood from text: {request.Text}" });
})
    .WithName("AnalyzeMood") // Names the endpoint
    .WithTags("Mood Analysis") // Groups endpoint under this name in Swagger
    .Produces(200, typeof(object))
    .Accepts<MoodRequest>("application/json"); // Specifies API expects JSON input

app.Run(); // Need this for app to start and "listen" for incoming API requests


string DetectMood(string text)
{
    var moodKeywords = new Dictionary<string, List<string>>
    {
        { "Happy", new List<string> { "happy", "excited", "joyful", "fun", "energetic", "laugh", "smile" } },
        { "Sad", new List<string> { "sad", "depressed", "heartbroken", "lonely", "upset", "down" } },
        { "Anxious", new List<string> { "anxious", "nervous", "worried", "stressed", "exams", "uncertain" } },
        { "Angry", new List<string> { "angry", "furious", "mad", "rage", "annoyed", "frustrated" } },
        { "Confident", new List<string> { "confident", "unstoppable", "determined", "strong" } }
    };

    text = text.ToLower(); // Cleans up input case-insensitivity

    var moodScores = new Dictionary<string, int>();

    foreach (var mood in moodKeywords.Keys)
    {
        moodScores[mood] = moodKeywords[mood].Count(word => text.Contains(word));
    }

    Console.WriteLine("Mood Scores:");
    foreach (var score in moodScores)
    {
        Console.WriteLine($"{score.Key}: {score.Value}");
    }


    var bestMatch = moodScores.OrderByDescending(m => m.Value).First();

    return bestMatch.Value > 0 ? bestMatch.Key : "neutral";
}

public class MoodRequest
{
    public string? Text { get; set; }
}
