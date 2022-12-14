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
using Newtonsoft.Json;
using WeatherAPI.Model;
using WeatherAPI.Services;
using Microsoft.Win32;
using CodeBeautify;
using WeatherAPI.Pages;

namespace WeatherAPI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        OpenWeatherMapService ServiceProvider;
        public MainWindow()
        {
            InitializeComponent();
            ServiceProvider = new();
        }



        public static void ChangePictureBasedOnWeatherStatus(Image image, String weatherStatus)
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
                case "Haze":
                    path += "foggy.png";
                    break;
                default:
                    path += "MainMenu.png";
                    break;
            }
            BitmapImage logo = new();
            logo.BeginInit();
            logo.UriSource = new Uri(path);
            logo.EndInit();
            image.Source = logo;
        }

        public static Boolean WasApiCallSuccessfull(long code)
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

        public static Boolean IsInputFilled(String inputCityName)
        {
            String city = inputCityName.Trim();
            if (city == "")
            {
                MessageBox.Show("Please type in a City!");
                return false;
            }
            return true;
        }

        public static String CalculateTimeFromStamp(long UnixTimeStamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(UnixTimeStamp).DateTime.ToString("hh:mm");
        }

        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            String city = tbInputCityName.Text.Trim();
            if (IsInputFilled(city))
            {
                WeatherLocation weatherLocation = await ServiceProvider.GetWeatherFromCityName(city);
                if (WasApiCallSuccessfull(weatherLocation.Cod))
                {
                    lblCityName.Content = $"Showing results for:  {weatherLocation.Name} {weatherLocation.Sys.Country} \t {weatherLocation.Coord.Lat} N : {weatherLocation.Coord.Lon} W";
                    lblWeatherAndTemp.Content = $"{Math.Round(weatherLocation.Main.Temp - 273.15)}°C {weatherLocation.Weather[0].Description}";
                    lblInfoSun.Content = $"Sun rises at: {CalculateTimeFromStamp(weatherLocation.Sys.Sunrise)} \nSun sets at: {CalculateTimeFromStamp(weatherLocation.Sys.Sunset)}";
                    ChangePictureBasedOnWeatherStatus(Img, weatherLocation.Weather[0].Main);
                    lblLastUpdate.Content = "Last Update: " + DateTime.Now;
                    wbMaps.Source = new Uri($"https://www.google.ch/maps/@{weatherLocation.Coord.Lat},{weatherLocation.Coord.Lon},10z?hl=de");
                    btnForecast.IsEnabled = true;
                }
            }
        }

        private async void BtnForecast_Click(object sender, RoutedEventArgs e)
        {
            String city = tbInputCityName.Text.Trim();
            if (IsInputFilled(city))
            {
                WeatherForecast weatherForecast = await ServiceProvider.GetWeatherForecastFromCityName(city);
                if (WasApiCallSuccessfull(weatherForecast.Cod))
                {
                    ForecastBox forecastBox = new(weatherForecast);
                    forecastBox.Show();
                }
            }
        }

        private void TtbInputCityName_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnForecast.IsEnabled = false;
        }
    }
}