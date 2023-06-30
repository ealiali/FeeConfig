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
                _logger.LogInformation("Strating Getting Fee Config V2");
                var feeConfig = await _feeDBContext.FeeConfigurations.FirstOrDefaultAsync();
                if (feeConfig != null)
                {
                    _logger.LogInformation("Strating Calculating Fee Config V2");
                    return Ok(new
                    {
                        Msg = "Success",
                        Code = 200,
                        Data = new
                        {
                            Amount = amount,
                            Fee = feeConfig.FlatValue * amount / 100
                        }
                    });
                }
                return Ok(new
                {
                    Msg = "Not Found Configuration For Fee",
                    Code = 401,
                    Data = new
                    {
                        Amount = amount,
                        Fee = 0
                    }
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException == null ? e.Message : e.InnerException.Message);

                return Ok(new
                {
                    Msg = "Error",
                    Code = 500,
                    Data = new
                    {
                        Amount = amount,
                        Fee = 0
                    }
                });
            }
        }
    }
}