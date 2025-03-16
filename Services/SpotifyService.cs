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

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

        var requestUrl = $"https://api.spotify.com/v1/recommendations?" +
            $"target_valence={spotifyAttributes.Valence}&" +
            $"target_energy={spotifyAttributes.Energy}&" +
            $"target_tempo={spotifyAttributes.Tempo}&" +
            $"seed_genres={spotifyAttributes.PreferredGenres}";

        var response = await _httpClient.GetAsync(requestUrl);

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
        var tokenResult = await tokenResponse.Content.ReadFromJsonAsync<SpotifyTokenResponse>();
        _accessToken = tokenResult?.AccessToken;
    }
}