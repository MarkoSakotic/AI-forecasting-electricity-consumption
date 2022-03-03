using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DataLayer.DAO
{
    public class WeatherConditionsDAO : BaseRepo<Weather>
    {
        public void DeleteAll()
        {
            using (var db = new WeatherConditionsContainer())
            {
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE [WeatherSet]");
            }
        }


    }
}
