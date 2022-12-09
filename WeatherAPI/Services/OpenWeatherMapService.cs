using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeatherAPI.Model;

namespace WeatherAPI.Services
{
    class OpenWeatherMapService
    {
        private static String APIKEY = "8a6ffc5fc9687f07fcfaba425c3bdc66";
        // https://api.openweathermap.org/data/2.5/weather?q=Olten&appid=8a6ffc5fc9687f07fcfaba425c3bdc66

        private HttpClient httpClient = new HttpClient();
        private String LinkStart = "https://api.openweathermap.org/data/2.5/weather";

        public async Task<WeatherLocation> getWeatherFromCityName(String city)
        {
            String url = $"{LinkStart}?q={city}&APPID={APIKEY}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<WeatherLocation>(responseContent);
        }

        public async Task<String> getWeatherJsonAsString(String city)
        {
            String url = $"{LinkStart}?q={city}&APPID={APIKEY}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
    }
}
