using AccessDataLayer;
using ServiceLayer;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FrontEndLayer.MVVM.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            DataHandling data = new DataHandling();

            data.ImportFromExcel();
        }

        private void btnTraining_Click(object sender, RoutedEventArgs e)
        {
            DataHandling dh = new DataHandling();
            LoadPredictor lp = new LoadPredictor();

            /* PART FOR TRAINING IN DEFINED TIME PERIOD
             
            List<Weather> newList = new List<Weather>();
            bool startDate = false;
            bool endDate = false;

            if(StartData.SelectedDate >= EndData.SelectedDate)
            {
                MessageBox.Show("Please enter valid input!", "Validation", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                foreach (var item in dh.NormalizedValues())
                {
                    if (item.LocalTime.Date >= StartData.SelectedDate )
                    {
                        startDate = true;
                    }
                    if (startDate == true)
                    {
                        if (item.LocalTime.Date <= EndData.SelectedDate)
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
            }
            */
            //lp.Predict();
            if (ResultTraining.Visibility == Visibility.Collapsed)
            {
                ResultTraining.Visibility = Visibility.Visible;
            }

            if (ResultTraining_Copy.Visibility == Visibility.Collapsed)
            {
                ResultTraining_Copy.Visibility = Visibility.Visible;
            }

            ResultTraining.Content = lp.Train(dh.NormalizedValues());
        }


        /********************Show or hide buttons, labels******************************************************/
        private void btnTraining_Copy_Click(object sender, RoutedEventArgs e)
        {
            btnTraining_Copy.Visibility = Visibility.Collapsed;
            btnOpen.Visibility = Visibility.Collapsed;

            if (btnTraining.Visibility == Visibility.Collapsed)
            {
                btnTraining.Visibility = Visibility.Visible;
            }

            if (StartData.Visibility == Visibility.Collapsed)
            {
                StartData.Visibility = Visibility.Visible;
            }

            if (EndData.Visibility == Visibility.Collapsed)
            {
                EndData.Visibility = Visibility.Visible;
            }

        }

    }
}
