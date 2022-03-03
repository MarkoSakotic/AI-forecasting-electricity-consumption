using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AccessDataLayer
{
    public class DateTimeLoad : INotifyPropertyChanged
    {
        private double allLoadMWhh;
        private DateTime allDateCelll;
        public event PropertyChangedEventHandler PropertyChanged;

        public double AllLoadMWhh
        {
            get { return allLoadMWhh; }
            set
            {
                allLoadMWhh = value;
                OnPropertyChanged();
            }
        }
        public DateTime AllDateCelll
        {
            get { return allDateCelll; }
            set { allDateCelll = value;
                OnPropertyChanged();
            }
        }

        public DateTimeLoad()
        {

        }

        public DateTimeLoad(double allLoadMWhh, DateTime allDateCelll)
        {
            AllLoadMWhh = allLoadMWhh;
            AllDateCelll = allDateCelll;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
