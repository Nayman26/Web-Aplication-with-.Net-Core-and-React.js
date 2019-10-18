using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class SampleController : Controller
    {
        string[] Summaries = { "sunny", "rainy", "snowy", "foggy", "cloudy" };
        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> Weather()
        {
            var rng = new Random();
            return Enumerable.Range(1, 4).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString(),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }
        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }
            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }
    }
}
