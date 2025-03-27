using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using MoodMusicAPI.Models;

namespace MoodMusicAPI.Services;

public class SpotifyService : ISpotifyService
{
    private readonly HttpClient _httpClient;
    private readonly SpotifySettings _settings;
    private string _accessToken;

    public SpotifyService(HttpClient httpClient, IOptions<SpotifySettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
    }

    public async Task<List<MusicRecommendation>> GetRecommendationsAsync(MoodAnalysisResult moodAnalysis)
    {
        await EnsureAccessTokenAsync();

        var spotifyAttributes = moodAnalysis.MusicAttributes;
        var recommendations = new List<MusicRecommendation>();
        
        var requestUrl = $"https://api.spotify.com/v1/recommendations" +
                         $"?seed_genres={spotifyAttributes.PreferredGenres}&" +
                         $"target_valence={spotifyAttributes.Valence}&" +
                         $"target_energy={spotifyAttributes.Energy}&" +
                         $"target_tempo={spotifyAttributes.Tempo}";
        
        Console.WriteLine($"Final Spotify Request URL: {requestUrl}");

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        Console.WriteLine($"Authorization header: {_httpClient.DefaultRequestHeaders.Authorization}");
        
        Console.WriteLine("Requesting recommendations from Spotify...");
        Console.WriteLine($"URL: {requestUrl}");
        Console.WriteLine($"Token: {_accessToken}");

        var response = await _httpClient.GetAsync(requestUrl);
        
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Spotify API error: {response.StatusCode} - {error}");
            // return new List<SpotifyTrack>();
        }

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<SpotifyRecommendationsResponse>();
            if (result?.Tracks != null)
            {
                recommendations.AddRange(result.Tracks.Select(track => new MusicRecommendation
                {
                    TrackName = track.Name,
                    ArtistName = track.Artists.FirstOrDefault()?.Name ?? "Unknown Artist",
                    SpotifyUri = track.Uri,
                    Attributes = spotifyAttributes,
                    RecommendationReason = $"This song matches your {moodAnalysis.PrimaryMood.ToLower()} mood with similar energy and tempo"
                }));
            }
        }

        return recommendations;
    }

    private async Task EnsureAccessTokenAsync()
    {
        if (!string.IsNullOrEmpty(_accessToken))
            return;
        
        Console.WriteLine("Getting new access token from Spotify...");
        Console.WriteLine("Client ID: " + _settings.ClientId);

        var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_settings.ClientId}:{_settings.ClientSecret}"));

        var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token")
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" }
            })
        };

        tokenRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", auth);
        
        var tokenResponse = await _httpClient.SendAsync(tokenRequest);
        
        if (!tokenResponse.IsSuccessStatusCode)
        {
            var errorBody = await tokenResponse.Content.ReadAsStringAsync();
            Console.WriteLine("Token request failed: " + errorBody);
            return;
        }
        
        var tokenResult = await tokenResponse.Content.ReadFromJsonAsync<SpotifyTokenResponse>();

        if (tokenResult == null || string.IsNullOrEmpty(tokenResult.AccessToken))
        {
            var raw = await tokenResponse.Content.ReadAsStringAsync();
            Console.WriteLine("Failed to get token from Spotify: " + raw);
        }
        
        _accessToken = tokenResult.AccessToken;
        Console.WriteLine("Got the token from Spotify: " + _accessToken);
    
    }
}