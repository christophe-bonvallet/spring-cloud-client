using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Formation.SpringCloud.Client.Models;
using Steeltoe.Common.Discovery;
using System.Net.Http;
using System.Text.Json;

namespace Formation.SpringCloud.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DiscoveryHttpClientHandler _handler;

        public HomeController(ILogger<HomeController> logger, IDiscoveryClient client)
        {
            _logger = logger;
            _handler = new DiscoveryHttpClientHandler(client);
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            var client = new HttpClient(_handler, false);

            var response = await client.GetStringAsync("http://formation-springcloud-service/weatherforecast");
            var weatherForecastNext5 = JsonSerializer.Deserialize<List<WeatherForecastModel>>(response);

            return View(weatherForecastNext5);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
