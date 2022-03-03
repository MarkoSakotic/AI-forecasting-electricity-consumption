using AccessDataLayer;
using DataLayer;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FrontEndLayer.MVVM.Views
{
    /// <summary>
    /// Interaction logic for DiscoveryView.xaml
    /// </summary>
    public partial class ForecastView : UserControl
    {
        public List<double> returnPredicted = new List<double>();

        List<Weather> listWeather = new List<Weather>();

        public ObservableCollection<DateTimeLoad> potrosnjaIDatumAll;

        private ObservableCollection<DateTimeLoad> potrosnjaIDatum;
        public ObservableCollection<DateTimeLoad> PotrosnjaIDatum { get => potrosnjaIDatum; set { potrosnjaIDatum = value; } }
        public ForecastView()
        {
            InitializeComponent();
            PotrosnjaIDatum = new ObservableCollection<DateTimeLoad>();
            potrosnjaIDatumAll = new ObservableCollection<DateTimeLoad>();

        }

        private void btnShowForecastValue_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < returnPredicted.Count; i++)
            {
                var firstElement = listWeather[i];
                var secondELement = returnPredicted[i];

                DateTimeLoad dtl = new DateTimeLoad();

                dtl.AllDateCelll = firstElement.LocalTime;
                dtl.AllLoadMWhh = secondELement;

                PotrosnjaIDatum.Add(dtl);
            }

            PotrosnjaIDatum.RemoveAt(PotrosnjaIDatum.Count - 1);

            datagrid1.ItemsSource = PotrosnjaIDatum;

            datagrid1.Visibility = Visibility.Visible;
        }

        private void btnExportToCSV_Click(object sender, RoutedEventArgs e)
        {
            string path = @"C:\Users\Marko\Desktop\NISProjekatOptimizacija\PrognozaPotrosnjeElEnergije\exportToCSV.csv";

            using (var w = new StreamWriter(path))
            {
                for (int i = 0; i < returnPredicted.Count; i++)
                {
                    var first = listWeather[i];
                    var second = returnPredicted[i];

                    var line = string.Format("{0},{1}", first.LocalTime, second);
                    w.WriteLine(line);
                    w.Flush();
                }
            }
        }

        private void btnTesting_Click(object sender, RoutedEventArgs e)
        {

            DataHandlingForForecast dh = new DataHandlingForForecast();
            LoadPredictor lp = new LoadPredictor();
            List<WeatherForecast> newList = new List<WeatherForecast>();


            if (ComboDay.SelectedIndex != -1 && StartDataTest.SelectedDate != null)
            {
                DataHandlingForForecast forecast = new DataHandlingForForecast();
                forecast.ImportFromExcel();

                DateTime EndDate = new DateTime();

                string s = ComboDay.SelectedItem.ToString();
                string[] splited = s.Split(':');
                string days = splited[1];
                int dayss = int.Parse(days);
                TimeSpan addDays = new TimeSpan(dayss, 0, 0, 0);

                bool startDate = false;
                bool endDate = false;

                EndDate = StartDataTest.SelectedDate.Value + addDays;

                foreach (var item in dh.NormalizedValues())
                {

                    if (item.LocalTime >= StartDataTest.SelectedDate)
                    {
                        startDate = true;
                    }
                    if (startDate == true)
                    {
                        if (item.LocalTime <= EndDate)
                        {
                            endDate = true;
                        }
                    }
                    if (startDate == true && endDate == true)
                    {
                        newList.Add(item);

                    }


                    startDate = false;
                    endDate = false;
                }

                foreach (var item in newList)
                {
                    Weather weather = new Weather();
                    weather.AirTemperature = item.AirTemperature;
                    weather.AtmosphericPressure = item.AtmosphericPressure;
                    weather.Pressure = item.PressureTendency;
                    weather.RelativeHumidity = item.RelativeHumidity;
                    weather.PressureTendency = item.PressureTendency;
                    weather.CloudCover = item.CloudCover;
                    weather.LocalTime = item.LocalTime;
                    weather.DewPointTemperature = item.DewPointTemperature;
                    weather.MeanWindSpeed = item.MeanWindSpeed;
                    weather.Day = item.Day;
                    weather.Month = item.Month;
                    weather.Hour = item.Hour;
                    weather.TypeOfDay = item.TypeOfDay;
                    weather.LoadMWh = 0;

                    listWeather.Add(weather);
                }

                returnPredicted = lp.Predict(listWeather);
                ShowForecastValue();

                btnExportToCSV.Visibility = Visibility.Visible;
                btnShowForecastValue.Visibility = Visibility.Visible;
                imageForecastView.Width = 253;
                imageForecastView.Height = 183;
                imageForecastView.Margin = new Thickness(447, 15, 0, 0);

                //ShowForecastValue();

            }
            else
            {
                MessageBox.Show("Please enter valid input!", "Validation", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        public ObservableCollection<DateTimeLoad> ShowForecastValue()
        {
            for (int i = 0; i < returnPredicted.Count; i++)
            {
                DateTimeLoad dtl = new DateTimeLoad();

                var firstElement = listWeather[i];
                var secondELement = returnPredicted[i];

                dtl.AllDateCelll = firstElement.LocalTime;
                dtl.AllLoadMWhh = secondELement;

                potrosnjaIDatumAll.Add(dtl);

            }
            potrosnjaIDatumAll.RemoveAt(potrosnjaIDatumAll.Count - 1);

            return potrosnjaIDatumAll;
        }
    }
}
