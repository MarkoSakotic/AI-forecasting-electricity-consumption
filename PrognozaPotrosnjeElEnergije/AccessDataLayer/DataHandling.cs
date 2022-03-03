using DataLayer;
using DataLayer.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Data;
using System.Globalization;

namespace AccessDataLayer
{
    public class DataHandling
    {
        private WeatherConditionsDAO weatherConditionDao = new WeatherConditionsDAO();
        private List<double> airTemperatureList = new List<double>();
        private List<double> atmosphericPressureList = new List<double>();
        private List<double> pressureTendencyList = new List<double>();
        private List<double> relativeHumidityList = new List<double>();
        private List<double> pressureList = new List<double>();
        private List<double> cloudCoverList = new List<double>();
        private List<double> loadMWhList = new List<double>();
        private List<double> dewPointTemperatureList = new List<double>();
        private List<double> meanWindSpeedList = new List<double>();
        private List<double> hourList = new List<double>();
        private List<double> dayList = new List<double>();
        private List<double> monthList = new List<double>();

        public Weather WeatherCondition { get; set; }

        public DateAndLoad DateAndLoad { get; set; }


        /********************IMPORT SPECIFIC DATA FROM EXCEL AND WRITE IN DATABASE******************************************************/
        public void ImportFromExcel()
        {
            //weatherConditionDao.DeleteAll();

            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "Excell File |*.xlsx;*xlsx";

            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            Excel.Worksheet xlWorkSheet2;
            Excel.Range range;
            Excel.Range rangeSheet2;

            int rCnt;
            int rCnt2;
            int rw = 0;
            int rwSheet2 = 0;
            int cl = 0;
            string path = "";

            xlApp = new Excel.Application();
            if( openfile.ShowDialog() == DialogResult.OK)
            {
                 path = openfile.FileName + openfile.InitialDirectory;
            }

            xlWorkBook = xlApp.Workbooks.Open(path, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            xlWorkSheet2 = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(7);

            range = xlWorkSheet.UsedRange;
            rangeSheet2 = xlWorkSheet2.UsedRange;

            rw = range.Rows.Count;
            rwSheet2 = rangeSheet2.Count;
            cl = range.Columns.Count;
            List<Weather> weathers = new List<Weather>();
            List<DateAndLoad> dateAndLoad = new List<DateAndLoad>();

            double airTemperature;
            double atmosphericPressure;
            double pressureTendency;
            double relativeHumidity;
            double pressure;
            double cloudCover = 100;
            DateTime localTime = new DateTime();
            double loadMWh = 0;
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
            double loadMWhPrevious = 0;
            double dewPointTemperaturePrevious = 0;
            double meanWindSpeedPrevious = 0;
            double hourPrevious = 0;
            double monthPrevious = 0;
            double dayPrevious = 0;

            double airTemperatureMin= 0;
            double airTemperatureMax = 0;

            double atmosphericPressureMin = 0;
            double atmosphericPressureMax = 0;

            double pressureTendencyMin = 0;
            double pressureTendencyMax = 0;

            double relativeHumidityMin = 0;
            double relativeHumidityMax= 0;

            double pressureMin = 0;
            double pressureMax = 0;

            double cloudCoverMin = 0;
            double cloudCoverMax = 0;

            double loadMWhMin = 0;
            double loadMWhMax = 0;

            double dewPointTemperatureMin = 0;
            double dewPointTemperatureMax = 0;

            double meanWindSpeedMin = 0;
            double meanWindSpeedMax = 0;

            double hourMin = 0;
            double hourMax = 0;

            double dayMin = 0;
            double dayMax = 0;

            double monthMin = 0;
            double monthMax = 0;

            /********************LOAD LOAD, DATE, TIME******************************************************/

            for (rCnt2 = 2; rCnt2 <= 29305; rCnt2++)
            {
                var localTimeCell = (range.Cells[rCnt2, 1] as Excel.Range).Value2;

                var loadMWhCell = (rangeSheet2.Cells[rCnt2, 4] as Excel.Range).Value2;
                var loadDateCell = DateTime.FromOADate((rangeSheet2.Cells[rCnt2, 1] as Excel.Range).Value2);
                var loadTimeCell = (rangeSheet2.Cells[rCnt2, 2] as Excel.Range).Text;
                string[] niz = loadTimeCell.Split(':');
                TimeSpan ts = new TimeSpan(Int32.Parse(niz[0]), Int32.Parse(niz[1]), 0);


                if (loadMWhCell == null && loadDateCell == null && loadTimeCell == 0)
                {
                    break;
                }
                else
                {
                    DateAndLoad dal = new DateAndLoad(loadMWhCell, loadDateCell, ts);
                    dateAndLoad.Add(dal);
                }

            }


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
                    /********************LOAD******************************************************/
                    foreach (var item in dateAndLoad)
                    {
                        DateTime loadDateConverted = Convert.ToDateTime(item.AllDateCell);
                        TimeSpan loadTimeConverted = item.AllTimeCell;
                        DateTime localDataConv = Convert.ToDateTime(localTimeCell);

                        if (localDataConv.Year == loadDateConverted.Year && localDataConv.Day == loadDateConverted.Day && localDataConv.Month == loadDateConverted.Month && localDataConv.Hour == item.AllTimeCell.Hours)
                        {
                            loadMWh = item.AllLoadMWh;
                            loadMWhPrevious = item.AllLoadMWh;
                            break;
                        }

                    }

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
                    //pressureTendency = (range.Cells[rCnt, 5] as Excel.Range).Value2;
                    //var pressureTendencyCell = (range.Cells[rCnt, 5] as Excel.Range).Value2;
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
                    //pressure = (range.Cells[rCnt, 4] as Excel.Range).Value2;
                    //var pressureCell = (range.Cells[rCnt, 4] as Excel.Range).Value2;
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

                        string pom = localTimeConverted.ToString("dddd");
                        /********************TYPE OF DAY******************************************************/
                        if (String.Equals(localTimeConverted.ToString("dddd"), "subota"))
                        {
                            typeOfDay = 1;
                        }
                        else if(String.Equals(localTimeConverted.ToString("dddd"), "nedelja"))
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

                    /********************DEWPOINT TEMPERATURE******************************************************/
                    if (dewTemperatureCell != null)
                    {
                        dewPointTemperature = dewTemperatureCell;
                        dewPointTemperaturePrevious = dewTemperatureCell;
                    }
                    else
                    {
                        dewPointTemperature = dewPointTemperaturePrevious;
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
                    //string cloudCoverCell = Convert.ToString((range.Cells[rCnt, 11] as Excel.Range).Value2);
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

                    Weather w = new Weather(airTemperature, atmosphericPressure, pressureTendency, relativeHumidity, pressure, cloudCover, localTime, loadMWh, dewPointTemperature, meanWindSpeed, day, month, hour, typeOfDay);
                    
                    airTemperatureList.Add(airTemperature);
                    atmosphericPressureList.Add(atmosphericPressure);
                    pressureTendencyList.Add(pressureTendency);
                    relativeHumidityList.Add(relativeHumidity);
                    pressureList.Add(pressure);
                    cloudCoverList.Add(cloudCover);
                    loadMWhList.Add(loadMWh);
                    dewPointTemperatureList.Add(dewPointTemperature);
                    meanWindSpeedList.Add(meanWindSpeed);
                    hourList.Add(hour);
                    dayList.Add(day);
                    monthList.Add(month);

                    weathers.Add(w);

                    weatherConditionDao.Insert(w);
                }

            }

            airTemperatureMin = airTemperatureList.Min();
            airTemperatureMax = airTemperatureList.Max();

            atmosphericPressureMin = atmosphericPressureList.Min();
            atmosphericPressureMax = atmosphericPressureList.Max();

            pressureTendencyMin = pressureTendencyList.Min();
            pressureTendencyMax = pressureTendencyList.Max();

            relativeHumidityMin = relativeHumidityList.Min();
            relativeHumidityMax = relativeHumidityList.Max();

            pressureMin = pressureList.Min();
            pressureMax = pressureList.Max();

            cloudCoverMin = cloudCoverList.Min();
            cloudCoverMax = cloudCoverList.Max();

            loadMWhMin = loadMWhList.Min();
            loadMWhMax = loadMWhList.Max();

            dewPointTemperatureMin = dewPointTemperatureList.Min();
            dewPointTemperatureMax = dewPointTemperatureList.Max();

            meanWindSpeedMin = meanWindSpeedList.Min();
            meanWindSpeedMax = meanWindSpeedList.Max();

            hourMin = hourList.Min();
            hourMax = hourList.Max();

            dayMin = dayList.Min();
            dayMax = dayList.Max();

            monthMin = monthList.Min();
            monthMax = monthList.Max();

            /*foreach (var item in weathers)
            {
                item.AirTemperature = Normalize(item.AirTemperature, airTemperatureMax, airTemperatureMin);
                item.AtmosphericPressure = Normalize(item.AtmosphericPressure, atmosphericPressureMax, atmosphericPressureMin);
                item.PressureTendency = Normalize(item.PressureTendency, pressureTendencyMax, pressureTendencyMin);
                item.RelativeHumidity = Normalize(item.RelativeHumidity, relativeHumidityMax, relativeHumidityMin);
                item.Pressure = Normalize(item.Pressure, pressureMax, pressureMin);
                item.CloudCover = Normalize(item.CloudCover, cloudCoverMax, cloudCoverMin);
                item.LoadMWh = Normalize(item.LoadMWh, loadMWhMax, loadMWhMin);
                item.DewPointTemperature = Normalize(item.DewPointTemperature, dewPointTemperatureMax, dewPointTemperatureMin);

                weatherConditionDao.Insert(item);

            }
            */

            //xlWorkBook.Close(true, null, null);
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

        public double Denormalize(double normalize, double max, double min)
        {
            return (normalize * (max - min) + min);
        }

        public List<Weather> NormalizedValues()
        {
            List<Weather> weatherList = weatherConditionDao.GetList();
            List<Weather> normalizedValues = new List<Weather>();

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

            double loadMwhMax;
            double loadMwhMin;

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

            loadMwhMax = weatherList.Max(m => m.LoadMWh);
            loadMwhMin = weatherList.Min(m => m.LoadMWh);

            meanWindSpeedMax = weatherList.Max(m => m.MeanWindSpeed);
            meanWindSpeedMin = weatherList.Min(m => m.MeanWindSpeed);


            foreach (var item in weatherList)
            {
                item.AirTemperature = Normalize(item.AirTemperature, airTemperatureMax, airTemperatureMin);
                item.AtmosphericPressure = Normalize(item.AtmosphericPressure, athmosphericPressureMax, athmosphericPressureMin);
                item.Pressure = Normalize(item.Pressure, pressureMax, pressureMin);
                item.CloudCover = Normalize(item.CloudCover, cloudCoverMax, cloudCoverMin);
                item.PressureTendency = Normalize(item.PressureTendency, pressureTendencyMax, pressureTendencyMin);
                item.LoadMWh = Normalize(item.LoadMWh, loadMwhMax, loadMwhMin);
                item.Day = item.Day;
                item.Month = item.Month;
                item.Hour = item.Hour;
                item.MeanWindSpeed = Normalize(item.MeanWindSpeed, meanWindSpeedMax, meanWindSpeedMin);
                item.TypeOfDay = item.TypeOfDay;
                

                normalizedValues.Add(item);
            }

            return normalizedValues;
        }

        public List<double> DenormalizeLoad(List<double> normalizedValues)
        {
            List<double> result = new List<double>();
            List<Weather> weatherList = weatherConditionDao.GetList();

            double loadMax;
            double loadMin;

            loadMax = weatherList.Max(m => m.LoadMWh);
            loadMin = weatherList.Min(m => m.LoadMWh);

            foreach (var item in normalizedValues)
            {
                result.Add(Denormalize(item, loadMax, loadMin));
            }

            return result;
        }

    }
}