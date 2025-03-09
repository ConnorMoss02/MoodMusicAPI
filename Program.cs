var builder = WebApplication.CreateBuilder(args);

// Add Swagger and API Explorer
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/api/mood/analyze", (MoodRequest request) =>
{
    return Results.Ok(new { mood = "happy", message = $"Detected mood from text: {request.Text}" });
})
    .WithName("AnalyzeMood")
    .WithTags("Mood Analysis")
    .Produces(200, typeof(object))
    .Accepts<MoodRequest>("application/json");

app.Run();


public class MoodRequest
{
    public string? Text { get; set; }
}
