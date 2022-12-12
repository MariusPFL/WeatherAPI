using CodeBeautify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WeatherAPI.Pages
{
    /// <summary>
    /// Interaktionslogik für ForecastBox.xaml
    /// </summary>
    public partial class ForecastBox : Window
    {
        public ForecastBox(WeatherForecast weatherForecast)
        {
            InitializeComponent();
            for (int i = 0; i < weatherForecast.List.Length; i++)
            {
                if (weatherForecast.List[i].DtTxt.Hour == 12)
                {
                    Label label = new()
                    {
                        Content = $"\n\n\n{weatherForecast.List[i].DtTxt.DayOfWeek} \nWeather: {weatherForecast.List[i].Weather[0].Description} \tTemperature: {Math.Round(weatherForecast.List[i].Main.Temp - 273.15)} \n"
                    };
                    stackPanel.Children.Add(label);
                    Image image = new();
                    MainWindow.ChangePictureBasedOnWeatherStatus(image, weatherForecast.List[i].Weather[0].Main.ToString());
                    stackPanel.Children.Add(image);
                }
            }
        }
    }
}

