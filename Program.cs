using MoodMusicAPI.Services;
using MoodMusicAPI.Models;
using MoodMusicAPI.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173") // your Vite dev server
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

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

// Enable CORS here
app.UseCors();

app.UseHttpsRedirection();
app.UseDefaultFiles(); // enables index.html fallback
app.UseStaticFiles();  // serve from wwwroot

// Map endpoints
app.MapMoodEndpoints();
app.MapFallbackToFile("index.html");

app.Run();


