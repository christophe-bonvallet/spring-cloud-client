using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Formation.SpringCloud.Client.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Steeltoe.CircuitBreaker.Hystrix;

namespace Formation.SpringCloud.Client.Services
{
  public class GetWeatherForecastCommand : HystrixCommand<IEnumerable<WeatherForecastModel>>
  {
    private const string CACHE_KEY = "GetWeatherForecastCommand::CacheKey";
    private readonly ILogger<GetWeatherForecastCommand> _logger;
    private readonly IWeatherForecastService _weatherForecastService;
    private readonly IMemoryCache _cache;

    public GetWeatherForecastCommand(IHystrixCommandOptions commandOptions, 
        ILogger<GetWeatherForecastCommand> logger, 
        IWeatherForecastService weatherForecastService,
        IMemoryCache cache) : base(commandOptions)
    {
        _cache = cache;
        _weatherForecastService = weatherForecastService;
        _logger = logger;
    }

    public async Task<IEnumerable<WeatherForecastModel>> GetWeatherForecast()
    {
        return await ExecuteAsync();
    }

    protected override Task<IEnumerable<WeatherForecastModel>> RunAsync()
    {
        var result = _weatherForecastService.GetWeatherForecast();
        _logger.LogWarning(result.ToString());
        _cache.Set(CACHE_KEY, result);
        return result;
    }

    protected override Task<IEnumerable<WeatherForecastModel>> RunFallbackAsync()
    {
        IEnumerable<WeatherForecastModel> result;
        try {
            result = _cache.Get(this.CacheKey) as IEnumerable<WeatherForecastModel>;
            
        } catch {
            result = new List<WeatherForecastModel>()
            {
                new WeatherForecastModel()
                {
                    summary = "Service non-disponible pour le moment. Veuillez réesayer plus tard."
                }
            };
        }

        return Task.FromResult(result);
    }
  }
}