using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;


public record WallMessage(Guid Id, DateTime Created, string Creator, string MessageContent);

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
    }

    [HttpGet("", Name = "GetWeatherForecast")]
    public IEnumerable<WallMessage> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
