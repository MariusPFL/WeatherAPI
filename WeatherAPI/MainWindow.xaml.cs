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


        /*
         * TODO 
         * 
         * Letzte Aktualisierung
         * 
        */ 

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            OpenWeatherMapService serviceProvider = new OpenWeatherMapService();
            String city = tbInputCityName.Text.Trim();
            if (city == "")
            {
                MessageBox.Show("Please type in a City!");
            }
            else
            {
                WeatherLocation weatherLocation = await serviceProvider.getWeatherFromCityName(city);

                //String test = await serviceProvider.getWeatherJsonAsString("Olten");
                //cityNameLabel.Content = test.Substring(test.IndexOf("country" + 3), 2);
                if (weatherLocation.Cod == 404)
                {
                    MessageBox.Show("City not found! 404");
                }
                else
                {
                    lblCityName.Content = $"Showing results for:  {weatherLocation.Name} {weatherLocation.Sys.Country} \t {weatherLocation.Coord.Lat} N : {weatherLocation.Coord.Lon} W";
                    lblWeatherAndTemp.Content = $"{weatherLocation.Main.Temp - 273 - 15}°C {weatherLocation.Weather[0].Description}";
                    
                    lblInfoSun.Content = $"Sun rises at: {CalculateTimeFromMillisecondsToActualTime(weatherLocation.Sys.Sunrise, weatherLocation.Timezone)} \n Sun sets at: {CalculateTimeFromMillisecondsToActualTime(weatherLocation.Sys.Sunset, weatherLocation.Timezone)}";





                    // Change Picture
                    String path = "C:\\Users\\mapf\\source\\repos\\WeatherAPI\\WeatherAPI\\Source\\";
                    switch (weatherLocation.Weather[0].Main)
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
                        case "Mist":
                            path += "cloudy.png";
                            break;
                        default:
                            path += "MainMenu.png";
                            break;
                    }
                    BitmapImage logo = new BitmapImage();
                    logo.BeginInit();
                    logo.UriSource = new Uri(path);
                    logo.EndInit();
                    Img.Source = logo;
                }
            }

        }

        public String CalculateTimeFromMillisecondsToActualTime(double milliSeconds, long Timezone)
        {
            milliSeconds /= 1000;
            milliSeconds -= Timezone;
            milliSeconds /= 3600;
            String hours = milliSeconds.ToString().Substring(0, 1);
            double minutes = Math.Round(Convert.ToDouble(milliSeconds.ToString().Substring(1, 2)) * 0.6);
            return $"{hours}:{minutes}";
        }
    }
}
