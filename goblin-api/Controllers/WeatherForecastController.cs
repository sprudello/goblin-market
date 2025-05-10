using Microsoft.AspNetCore.Mvc;
using goblin_api.Models; // Assuming your models are in this namespace
using goblin_api.DTOs;   // Assuming your DTOs are in this namespace

namespace goblin_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Standard route: /api/WeatherForecast
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecastDto> Get()
        {
            return Enumerable.Range(1, 5).Select(index =>
            {
                var model = new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    Summaries[Random.Shared.Next(Summaries.Length)]
                );
                // Map Model to DTO
                return new WeatherForecastDto(model.Date, model.TemperatureC, model.Summary, model.TemperatureF);
            })
            .ToArray();
        }
    }
}
