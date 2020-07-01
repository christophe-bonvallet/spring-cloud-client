using System.Collections.Generic;
using System.Threading.Tasks;
using Formation.SpringCloud.Client.Models;

namespace Formation.SpringCloud.Client.Services
{
  public interface IWeatherForecastService {
      Task<IEnumerable<WeatherForecastModel>> GetWeatherForecast();
  }
}