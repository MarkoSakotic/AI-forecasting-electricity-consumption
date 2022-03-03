using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessDataLayer
{
    public class DateAndLoad
    {
        public double AllLoadMWh { get; set; }
        public DateTime AllDateCell { get; set; }
        public TimeSpan AllTimeCell { get; set; }

        public DateAndLoad()
        {

        }

        public DateAndLoad(double allLoadMWh, DateTime allDateCell, TimeSpan allTimeCell)
        {
            AllLoadMWh = allLoadMWh;
            AllDateCell = allDateCell;
            AllTimeCell = allTimeCell;
        }
    }
}
