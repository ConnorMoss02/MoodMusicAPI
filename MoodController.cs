using Microsoft.AspNetCore.Mvc;

[Route("api/mood")]
[ApiController]
public class MoodController : ControllerBase
{
    [HttpPost("analyze")]
    public IActionResult AnalyzeMood([FromBody] MoodRequest request)
    {
        return Ok(new { mood = "happy", message = $"Detected mood from text: {request.Text}" });
    }
}

public class MoodRequest
{
    public string? Text { get; set; }
}