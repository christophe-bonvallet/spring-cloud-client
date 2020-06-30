using System;

namespace Formation.SpringCloud.Client.Models
{
    public class WeatherForecastModel
    {
        public DateTime date { get; set; }
        public int temperatureC { get; set; }
        public int temperatureF { get; set; }
        public string summary { get; set; } 
    }
}