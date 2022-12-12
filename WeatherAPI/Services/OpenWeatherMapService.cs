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
        private static String APIKEY = "8a6ffc5fc9687f07fcfaba425c3bdc66";
        // https://api.openweathermap.org/data/2.5/weather?q=Olten&appid=8a6ffc5fc9687f07fcfaba425c3bdc66

        /// <summary>
        /// HTTPClients gets the Api from the URL
        /// </summary>
        private HttpClient HttpClient = new HttpClient();
        private String LinkStartWeather = "https://api.openweathermap.org/data/2.5/weather";
        private String LinkStartForecast = "http://api.openweathermap.org/data/2.5/forecast";

        public async Task<WeatherLocation> getWeatherFromCityName(String city)
        {
            String url = $"{LinkStartWeather}?q={city}&APPID={APIKEY}";
            HttpResponseMessage response = await HttpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<WeatherLocation>(responseContent);
        }

        public async Task<WeatherForecast> getWeatherForecastFromCityName(String city)
        {
            String url = $"{LinkStartForecast}?q={city}&APPID={APIKEY}";
            HttpResponseMessage response = await HttpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            try
            {
                return JsonConvert.DeserializeObject<WeatherForecast>(responseContent);
            }
            catch (DivideByZeroException)
            {
                WeatherForecast weatherForecast = new WeatherForecast();
                weatherForecast.Cod = 400;
                return weatherForecast;
            }
        }

        public async Task<String> getWeatherJsonAsString(String city)
        {
            String url = $"{LinkStartWeather}?q={city}&APPID={APIKEY}";
            HttpResponseMessage response = await HttpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
    }
}
