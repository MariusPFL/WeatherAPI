using CodeBeautify;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WeatherAPI.Model;

namespace WeatherAPI.Services
{
    class OpenWeatherMapService
    {
        private static readonly String APIKEY = "8a6ffc5fc9687f07fcfaba425c3bdc66";

        /// <summary>
        /// HTTPClients gets the Api from the URL
        /// </summary>
        private HttpClient HttpClient = new HttpClient();
        private readonly String LinkStartWeather = "https://api.openweathermap.org/data/2.5/weather";
        private readonly String LinkStartForecast = "http://api.openweathermap.org/data/2.5/forecast";

        public async Task<WeatherLocation> GetWeatherFromCityName(String city)
        {
            String url = $"{LinkStartWeather}?q={city}&APPID={APIKEY}";
            HttpResponseMessage response = await HttpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<WeatherLocation>(responseContent);
        }

        public async Task<WeatherForecast> GetWeatherForecastFromCityName(String city)
        {
            String url = $"{LinkStartForecast}?q={city}&APPID={APIKEY}";
            HttpResponseMessage response = await HttpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            try
            {
                return JsonConvert.DeserializeObject<WeatherForecast>(responseContent);
            }
            catch (Exception)
            {
                WeatherForecast weatherForecast = new()
                {
                    Cod = 400
                };
                return weatherForecast;
            }
        }

        public async Task<String> GetWeatherJsonAsString(String city)
        {
            String url = $"{LinkStartWeather}?q={city}&APPID={APIKEY}";
            HttpResponseMessage response = await HttpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
    }
}
