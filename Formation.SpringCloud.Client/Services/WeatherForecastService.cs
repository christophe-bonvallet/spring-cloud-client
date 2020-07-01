using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Formation.SpringCloud.Client.Models;
using Microsoft.Extensions.Logging;
using Steeltoe.Common.Discovery;

namespace Formation.SpringCloud.Client.Services
{
  public class WeatherForecastService: IWeatherForecastService  {
        private readonly ILogger<WeatherForecastService> _logger;
        private readonly DiscoveryHttpClientHandler _handler;
        
        public WeatherForecastService(ILogger<WeatherForecastService> logger, IDiscoveryClient client)
        {
            _logger = logger;
            _handler = new DiscoveryHttpClientHandler(client);
        }
        
        public async Task<IEnumerable<WeatherForecastModel>> GetWeatherForecast()
        {
            var client = new HttpClient(_handler, false);

            var response = await client.GetStringAsync("http://formation-springcloud-service/weatherforecast");
            _logger.LogWarning(response);
            var weatherForecastNext5 = JsonSerializer.Deserialize<List<WeatherForecastModel>>(response);

            return weatherForecastNext5;
        }
    }
}