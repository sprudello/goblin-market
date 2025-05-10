namespace goblin_api.DTOs
{
    public record WeatherForecastDto(DateOnly Date, int TemperatureC, string? Summary, int TemperatureF);
}
