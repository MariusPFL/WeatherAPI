using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.WebRequestMethods;
// using WeatherAPI.Model;
using Newtonsoft.Json;
using WeatherAPI.Model;
using WeatherAPI.Services;
using Microsoft.Win32;
using CodeBeautify;
using WeatherAPI.Pages;

// API KEY: 8a6ffc5fc9687f07fcfaba425c3bdc66
namespace WeatherAPI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        OpenWeatherMapService serviceProvider = new OpenWeatherMapService();
        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            String city = tbInputCityName.Text.Trim();
            if (IsInputFilled(city))
            {
                WeatherLocation weatherLocation = await serviceProvider.getWeatherFromCityName(city);
                if (WasApiCallSuccessfull(weatherLocation.Cod))
                {
                    lblCityName.Content = $"Showing results for:  {weatherLocation.Name} {weatherLocation.Sys.Country} \t {weatherLocation.Coord.Lat} N : {weatherLocation.Coord.Lon} W";
                    lblWeatherAndTemp.Content = $"{Math.Round(weatherLocation.Main.Temp - 273.15)}°C {weatherLocation.Weather[0].Description}";

                    lblInfoSun.Content = $"Sun rises at: {CalculateTimeFromMillisecondsToActualTime(weatherLocation.Sys.Sunrise)} \nSun sets at: {CalculateTimeFromMillisecondsToActualTime(weatherLocation.Sys.Sunset)}";

                    // Forecast schreibe Temperatur von den Nächsten Tagen und Kleines Icon von den

                    changePictureBasedOnWeatherStatus(Img, weatherLocation.Weather[0].Main);
                    // Change Picture
                    //String path = "C:\\Users\\mapf\\source\\repos\\WeatherAPI\\WeatherAPI\\Source\\";
                    //switch (weatherLocation.Weather[0].Main)
                    //{
                    //    case "Snow":
                    //        path += "snowflake.png";
                    //        break;
                    //    case "Clouds":
                    //        path += "cloudy.png";
                    //        break;
                    //    case "Clear":
                    //        path += "sun.png";
                    //        break;
                    //    case "Rain":
                    //        path += "rain.png";
                    //        break;
                    //    case "Drizzle":
                    //        path += "rain.png";
                    //        break;
                    //    case "Mist":
                    //        path += "foggy.png";
                    //        break;
                    //    case "Fog":
                    //        path += "foggy.png";
                    //        break;
                    //    default:
                    //        path += "MainMenu.png";
                    //        break;
                    //}
                    //BitmapImage logo = new BitmapImage();
                    //logo.BeginInit();
                    //logo.UriSource = new Uri(path);
                    //logo.EndInit();
                    //Img.Source = logo;

                    lblLastUpdate.Content = "Last Update: " + DateTime.Now;
                    wbMaps.Source = new Uri($"https://www.google.ch/maps/@{weatherLocation.Coord.Lat},{weatherLocation.Coord.Lon},10z?hl=de");
                    btnForecast.IsEnabled = true;
                }
            }
        }

        public static void changePictureBasedOnWeatherStatus(Image image, String weatherStatus)
        {
            String path = "C:\\Users\\mapf\\source\\repos\\WeatherAPI\\WeatherAPI\\Source\\";
            switch (weatherStatus)
            {
                case "Snow":
                    path += "snowflake.png";
                    break;
                case "Clouds":
                    path += "cloudy.png";
                    break;
                case "Clear":
                    path += "sun.png";
                    break;
                case "Rain":
                    path += "rain.png";
                    break;
                case "Drizzle":
                    path += "rain.png";
                    break;
                case "Mist":
                    path += "foggy.png";
                    break;
                case "Fog":
                    path += "foggy.png";
                    break;
                default:
                    path += "MainMenu.png";
                    break;
            }
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(path);
            logo.EndInit();
            image.Source = logo;
        }

        public Boolean WasApiCallSuccessfull(long code)
        {
            if (code == 404)
            {
                MessageBox.Show("City not found! 404");
                return false;
            }
            if (code == 400)
            {
                MessageBox.Show("Something went wrong! 400");
                return false;
            }
            return true;
        }

        public Boolean IsInputFilled(String inputCityName)
        {
            String city = inputCityName.Trim();
            if (city == "")
            {
                MessageBox.Show("Please type in a City!");
                return false;
            }
            return true;
        }

        public String CalculateTimeFromMillisecondsToActualTime(long UnixTimeStamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(UnixTimeStamp).DateTime.ToString("hh:mm");
        }


        private async void btnForecast_Click(object sender, RoutedEventArgs e)
        {
            String city = tbInputCityName.Text.Trim();
            if (IsInputFilled(city))
            {
                WeatherForecast weatherForecast = await serviceProvider.getWeatherForecastFromCityName(city);
                if (WasApiCallSuccessfull(weatherForecast.Cod))
                {
                    ForecastBox forecastBox = new ForecastBox(weatherForecast);
                    forecastBox.Show();
                    //String testeErgebenisse = "";
                    //for (int i = 0; i < weatherForecast.List.Length; i++)
                    //{
                    //    if (i % 8 == 0)
                    //    {
                    //        testeErgebenisse += $"{weatherForecast.List[i].DtTxt.DayOfWeek} \nWeather: {weatherForecast.List[i].Weather[0].Description} \tTemperature: {weatherForecast.List[i].Main.Temp} \n\n";
                    //    }
                    //}
                    //MessageBox.Show(testeErgebenisse, "The Forecast!");
                }

            }
        }

        private void tbInputCityName_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnForecast.IsEnabled = false;
        }
    }
}
