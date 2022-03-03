using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.ANNExecute
{
    public abstract class ANNBase
    {
        public int BatchSize { get; set; }
        public int EpochNumber { get; set; }
        public string CostFunction { get; set; }
        public string Optimizer { get; set; }
        public string KernelInitializer { get; set; }
        public string ActivationFunction { get; set; }
        public int NumberOfHiddenLayers { get; set; }
        public int NumberOfNeuronsInFirstHiddenLayer { get; set; }
        public int NumberOfNeuronsInOtherHiddenLayers { get; set; }
        public int Verbose { get; set; }

        public int InputDim { get; set; }

        public ANNBase()
        {

        }
    }
}
