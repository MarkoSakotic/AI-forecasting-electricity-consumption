using DataLayer;
using DataLayer.DAO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace AccessDataLayer
{
    public class DataHandlingForForecast
    {
        private WeatherForecastDAO weatherConditionDao = new WeatherForecastDAO();
        private List<double> airTemperatureList = new List<double>();
        private List<double> atmosphericPressureList = new List<double>();
        private List<double> pressureTendencyList = new List<double>();
        private List<double> relativeHumidityList = new List<double>();
        private List<double> pressureList = new List<double>();
        private List<double> cloudCoverList = new List<double>();
        private List<double> dewPointTemperatureList = new List<double>();
        private List<double> meanWindSpeedList = new List<double>();
        private List<double> hourList = new List<double>();
        private List<double> dayList = new List<double>();
        private List<double> monthList = new List<double>();
        public WeatherForecast WeatherCondition { get; set; }

        public DateAndLoad DateAndLoad { get; set; }


        /********************IMPORT SPECIFIC DATA FROM EXCEL AND WRITE IN DATABASE******************************************************/
        public void ImportFromExcel()
        {
            weatherConditionDao.DeleteAll();

            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "Excell File |*.xlsx;*xlsx";

            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            Excel.Range range;

            int rCnt;
            int rw = 0;
            int cl = 0;
            string path = "";

            xlApp = new Excel.Application();
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                path = openfile.FileName + openfile.InitialDirectory;
            }

            xlWorkBook = xlApp.Workbooks.Open(path, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            range = xlWorkSheet.UsedRange;

            rw = range.Rows.Count;
            cl = range.Columns.Count;
            List<WeatherForecast> weathers = new List<WeatherForecast>();
            List<DateAndLoad> dateAndLoad = new List<DateAndLoad>();

            double airTemperature;
            double atmosphericPressure;
            double pressureTendency;
            double relativeHumidity;
            double pressure;
            double cloudCover = 100;
            DateTime localTime = new DateTime();
            double dewPointTemperature = 0;
            double meanWindSpeed = 0;
            double hour = 0;
            double month = 0;
            double day = 0;
            double typeOfDay = 0;

            double airTemperaturePrevious = 0;
            double atmosphericPressurePrevious = 0;
            double pressureTendencyPrevious = 0;
            double relativeHumidityPrevious = 0;
            double pressurePrevious = 0;
            double cloudCoverPrevious = 100;
            DateTime localTimePrevious = new DateTime();
            double dewPointTemperaturePrevious = 0;
            double meanWindSpeedPrevious = 0;
            double hourPrevious = 0;
            double monthPrevious = 0;
            double dayPrevious = 0;

            for (rCnt = 2; rCnt <= rw; rCnt++)
            {
                var airTemperatureCell = (range.Cells[rCnt, 2] as Excel.Range).Value2;
                var atmosphericPressureCell = (range.Cells[rCnt, 3] as Excel.Range).Value2;
                var pressureTendencyCell = (range.Cells[rCnt, 5] as Excel.Range).Value2;
                var relativeHumidityCell = (range.Cells[rCnt, 6] as Excel.Range).Value2;
                var pressureCell = (range.Cells[rCnt, 4] as Excel.Range).Value2;
                string cloudCoverCell = Convert.ToString((range.Cells[rCnt, 11] as Excel.Range).Value2);
                var localTimeCell = (range.Cells[rCnt, 1] as Excel.Range).Value2;
                var dewTemperatureCell = (range.Cells[rCnt, 23] as Excel.Range).Value2;
                var meanWindSpeedCell = (range.Cells[rCnt, 8] as Excel.Range).Value2;




                /********************Break if all cells are empty, end of excell document******************************************************/
                if (airTemperatureCell == null && atmosphericPressureCell == null && pressureTendencyCell == null && relativeHumidityCell == null && pressureCell == null && cloudCoverCell == null && dewTemperatureCell == null)
                {
                    break;
                }
                else
                {
                    /********************AIR TEMPERATURE******************************************************/
                    if (airTemperatureCell != null)
                    {
                        airTemperature = airTemperatureCell;
                        airTemperaturePrevious = airTemperatureCell;
                    }
                    else
                    {
                        airTemperature = airTemperaturePrevious;
                    }

                    /********************ATMOSPHERIC PRESSURE******************************************************/
                    if (atmosphericPressureCell != null)
                    {
                        atmosphericPressure = atmosphericPressureCell;
                        atmosphericPressurePrevious = atmosphericPressureCell;
                    }
                    else
                    {
                        atmosphericPressure = atmosphericPressurePrevious;
                    }

                    /********************PRESSURE TENDENCY******************************************************/
                    if (pressureTendencyCell != null)
                    {
                        pressureTendency = pressureTendencyCell;
                        pressureTendencyPrevious = pressureTendencyCell;
                    }
                    else
                    {
                        pressureTendency = pressureTendencyPrevious;
                    }

                    /********************RELATIVE HUMIDITY******************************************************/
                    //relativeHumidity = (range.Cells[rCnt, 6] as Excel.Range).Value2;
                    //var relativeHumidityCell = (range.Cells[rCnt, 6] as Excel.Range).Value2;
                    if (relativeHumidityCell != null)
                    {
                        relativeHumidity = relativeHumidityCell;
                        relativeHumidityPrevious = relativeHumidityCell;
                    }
                    else
                    {
                        relativeHumidity = relativeHumidityPrevious;
                    }

                    /********************PRESSURE******************************************************/
                    if (pressureCell != null)
                    {
                        pressure = pressureCell;
                        pressurePrevious = pressureCell;
                    }
                    else
                    {
                        pressure = pressurePrevious;
                    }


                    /********************LOCAL TIME******************************************************/
                    if (localTimeCell != null)
                    {
                        DateTime localTimeConverted = Convert.ToDateTime(localTimeCell);
                        localTime = localTimeConverted;
                        localTimePrevious = localTimeConverted;

                        hour = Convert.ToDouble(localTimeConverted.Hour);
                        month = Convert.ToDouble(localTimeConverted.Month);
                        day = Convert.ToDouble(localTimeConverted.Day);

                        hourPrevious = Convert.ToDouble(localTimeConverted.Hour);
                        monthPrevious = Convert.ToDouble(localTimeConverted.Month);
                        dayPrevious = Convert.ToDouble(localTimeConverted.Day);

                        /********************TYPE OF DAY******************************************************/
                        if (String.Equals(localTimeConverted.ToString("dddd"), "subota"))
                        {
                            typeOfDay = 1;
                        }
                        else if (String.Equals(localTimeConverted.ToString("dddd"), "nedelja"))
                        {
                            typeOfDay = 1;
                        }
                        else if (String.Equals(localTimeConverted.ToString("dddd"), "ponedeljak"))
                        {
                            typeOfDay = 2;
                        }
                        else
                        {
                            typeOfDay = 0;
                        }
                    }
                    else
                    {
                        localTime = localTimePrevious;

                        hour = hourPrevious;
                        month = monthPrevious;
                        day = dayPrevious;
                    }


                    /********************MEAN WIND SPEED******************************************************/
                    if (meanWindSpeedCell != null)
                    {
                        meanWindSpeed = meanWindSpeedCell;
                        meanWindSpeedPrevious = meanWindSpeedCell;
                    }
                    else
                    {
                        meanWindSpeed = meanWindSpeedPrevious;
                    }

                    /********************CLOUD COVER******************************************************/
                    string dash = "–";

                    string cloudCoverFirstPart;
                    if (cloudCoverCell != null)
                    {
                        if (cloudCoverCell.Contains(dash))
                        {
                            string[] cloudCoverSplit = cloudCoverCell.Split('–');
                            cloudCoverFirstPart = cloudCoverSplit[0];
                            cloudCover = Convert.ToDouble(cloudCoverFirstPart);
                            cloudCoverPrevious = Convert.ToDouble(cloudCoverFirstPart);
                        }

                        if (cloudCoverCell.Contains("no clouds"))
                        {
                            cloudCover = 0;
                            cloudCoverPrevious = 0;
                        }

                        if (cloudCoverCell.Contains("Sky obscured by fog and/or other meteorological phenomena"))
                        {
                            cloudCover = 100;
                            cloudCoverPrevious = 100;
                        }

                        if (cloudCoverCell.Contains("or more"))
                        {
                            string[] cloudCoverSplit = cloudCoverCell.Split(' ');
                            cloudCoverFirstPart = cloudCoverSplit[0];
                            cloudCover = Convert.ToDouble(cloudCoverFirstPart);
                            cloudCoverPrevious = Convert.ToDouble(cloudCoverFirstPart);
                        }

                        if (cloudCoverCell.Contains("%") && !cloudCoverCell.Contains(dash) && !cloudCoverCell.Contains("or more") && !cloudCoverCell.Contains("or less"))
                        {
                            string cloudCoverNumberPart = cloudCoverCell.Remove(cloudCoverCell.Length - 2, 1);
                            cloudCover = Convert.ToDouble(cloudCoverNumberPart);
                            cloudCoverPrevious = Convert.ToDouble(cloudCoverNumberPart);
                        }

                        if (cloudCoverCell.Contains("or less"))
                        {
                            string[] cloudCoverSplit = cloudCoverCell.Split(' ');
                            cloudCoverFirstPart = cloudCoverSplit[0];
                            string cloudCoverNumber = cloudCoverFirstPart.Remove(cloudCoverFirstPart.Length - 1, 1);
                            cloudCover = Convert.ToDouble(cloudCoverNumber);
                            cloudCoverPrevious = Convert.ToDouble(cloudCoverNumber);
                        }
                    }
                    else
                    {
                        cloudCover = cloudCoverPrevious;
                    }

                    WeatherForecast w = new WeatherForecast(airTemperature, atmosphericPressure, pressureTendency, relativeHumidity, pressure, cloudCover, localTime, meanWindSpeed, month, day, hour, typeOfDay);

                    airTemperatureList.Add(airTemperature);
                    atmosphericPressureList.Add(atmosphericPressure);
                    pressureTendencyList.Add(pressureTendency);
                    relativeHumidityList.Add(relativeHumidity);
                    pressureList.Add(pressure);
                    cloudCoverList.Add(cloudCover);
                    meanWindSpeedList.Add(meanWindSpeed);
                    hourList.Add(hour);
                    dayList.Add(day);
                    monthList.Add(month);

                    weathers.Add(w);

                    weatherConditionDao.Insert(w);
                }

            }

            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);
        }


        /********************Normalize data to 0-1 range******************************************************/
        public double Normalize(double val, double max, double min)
        {
            return (val - min) / (max - min);
        }

        /********************Denormalize data******************************************************/

        public float Denormalize(float normalize, float max, float min)
        {
            return (normalize * (max - min) + min);
        }

        public List<WeatherForecast> NormalizedValues()
        {
            List<WeatherForecast> weatherList = weatherConditionDao.GetList();
            List<WeatherForecast> normalizedValues = new List<WeatherForecast>();

            double airTemperatureMax;
            double airTemperatureMin;

            double athmosphericPressureMax;
            double athmosphericPressureMin;

            double pressureMax;
            double pressureMin;

            double cloudCoverMax;
            double cloudCoverMin;

            double pressureTendencyMax;
            double pressureTendencyMin;

            double meanWindSpeedMax;
            double meanWindSpeedMin;


            airTemperatureMax = weatherList.Max(m => m.AirTemperature);
            airTemperatureMin = weatherList.Min(m => m.AirTemperature);

            athmosphericPressureMax = weatherList.Max(m => m.AtmosphericPressure);
            athmosphericPressureMin = weatherList.Min(m => m.AtmosphericPressure);

            pressureMax = weatherList.Max(m => m.Pressure);
            pressureMin = weatherList.Min(m => m.Pressure);

            cloudCoverMax = weatherList.Max(m => m.CloudCover);
            cloudCoverMin = weatherList.Min(m => m.CloudCover);

            pressureTendencyMax = weatherList.Max(m => m.PressureTendency);
            pressureTendencyMin = weatherList.Min(m => m.PressureTendency);

            meanWindSpeedMax = weatherList.Max(m => m.MeanWindSpeed);
            meanWindSpeedMin = weatherList.Min(m => m.MeanWindSpeed);


            foreach (var item in weatherList)
            {
                item.AirTemperature = Normalize(item.AirTemperature, airTemperatureMax, airTemperatureMin);
                item.AtmosphericPressure = Normalize(item.AtmosphericPressure, athmosphericPressureMax, athmosphericPressureMin);
                item.Pressure = Normalize(item.Pressure, pressureMax, pressureMin);
                item.CloudCover = Normalize(item.CloudCover, cloudCoverMax, cloudCoverMin);
                item.PressureTendency = Normalize(item.PressureTendency, pressureTendencyMax, pressureTendencyMin);
                item.Day = item.Day;
                item.Month = item.Month;
                item.Hour = item.Hour;
                item.MeanWindSpeed = Normalize(item.MeanWindSpeed, meanWindSpeedMax, meanWindSpeedMin);
                item.TypeOfDay = item.TypeOfDay;


                normalizedValues.Add(item);
            }

            return normalizedValues;
        }


        public List<WeatherForecast> AllValues()
        {
            List<WeatherForecast> weatherList = weatherConditionDao.GetList();
            List<WeatherForecast> allValues = new List<WeatherForecast>();

            foreach (var item in weatherList)
            {
                item.AirTemperature = item.AirTemperature;
                item.AtmosphericPressure = item.AtmosphericPressure;
                item.Pressure = item.Pressure;
                item.CloudCover = item.CloudCover;
                item.PressureTendency = item.PressureTendency;
                item.Day = item.Day;
                item.Month = item.Month;
                item.Hour = item.Hour;
                item.MeanWindSpeed = item.MeanWindSpeed;
                item.TypeOfDay = item.TypeOfDay;


                allValues.Add(item);
            }

            return allValues;
        }
    }

}
