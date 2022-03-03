using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DAO
{
    public class WeatherForecastDAO : BaseRepo<WeatherForecast>
    {
        public void DeleteAll()
        {
            using (var db = new WeatherConditionsContainer())
            {
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE [WeatherForecastSet]");
            }
        }

    }
}
