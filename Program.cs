using MoodMusicAPI.Services;
using MoodMusicAPI.Models;
using MoodMusicAPI.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Spotify and application services
builder.Services.Configure<SpotifySettings>(builder.Configuration.GetSection("Spotify"));
builder.Services.AddHttpClient();
builder.Services.AddScoped<ISpotifyService, SpotifyService>();
builder.Services.AddScoped<IMoodAnalyzer, MoodAnalyzer>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Map endpoints
app.MapMoodEndpoints();

app.Run();


