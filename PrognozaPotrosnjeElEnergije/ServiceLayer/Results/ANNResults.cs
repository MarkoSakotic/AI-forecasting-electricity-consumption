using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Results
{
    public class ANNResults
    {
        private List<double> predictedValue;

        public List<double> PredictedValues { get => predictedValue; set => predictedValue = value; }
    }
}
