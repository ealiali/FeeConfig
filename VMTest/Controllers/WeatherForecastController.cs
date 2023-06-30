using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace VMTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly FeeDBContext _feeDBContext;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, FeeDBContext feeDBContext)
        {
            _logger = logger;
            _feeDBContext = feeDBContext;
        }

        [HttpGet("GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("GetFeeValue")]
        public async Task<ActionResult> GetFeeValue(decimal amount)
        {

            try
            {
                _logger.LogInformation("Strating Getting Fee Config");
                var feeConfig = await _feeDBContext.FeeConfigurations.FirstOrDefaultAsync();
                if (feeConfig != null)
                {
                    _logger.LogInformation("Strating Calculating Fee Config");
                    return Ok(feeConfig.FlatValue * amount / 100);
                }
                return NotFound("There is no configuation for fee");
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException == null ? e.Message : e.InnerException.Message);
                return StatusCode(500);
            }
        }
    }
}