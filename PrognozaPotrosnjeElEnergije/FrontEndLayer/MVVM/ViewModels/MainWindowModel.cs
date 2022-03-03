using FrontEndLayer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndLayer.MVVM.ViewModels
{
    public class MainWindowModel : ObservableObject
    {
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand ForecastViewCommand { get; set; }

        public HomeViewModel HomeVM { get; set; }
        public ForecastViewModel ForecastVM { get; set; }


        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set 
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public MainWindowModel()
        {
            HomeVM = new HomeViewModel();
            ForecastVM = new ForecastViewModel();

            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o => 
            {
                CurrentView = HomeVM;
            });

            ForecastViewCommand = new RelayCommand(o =>
            {
                CurrentView = ForecastVM;
            });
        }
    }
}
