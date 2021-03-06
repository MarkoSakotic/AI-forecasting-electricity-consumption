using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Options
{
    public class ANNTrainingOptions
    {

        private const int BATCH_SIZE_NUMBER = 16;
        private const int EPOCH_NUMBER = 300;
        private const string COST_FUNCTION = "mean_squared_error";
        private const string OPTIMIZER = "adam";
        private const string KERNEL_INITIALIZER = "normal";
        private const string ACTIVATION_FUNCTION = "sigmoid";
        private const int NUMBER_OF_HIDDEN_LAYERS = 2;
        private const int NUMBER_OF_NEURONS_IN_FIRST_HIDDEN_LAYER = 10;
        private const int NUMBER_OF_NEURONS_IN_OTHER_HIDDEN_LAYERS = 8;
        private const int VERBOSE = 2;
        private int inputDim = 0;

        private object ndPredictorData;
        private object ndPredictedData;

        private List<List<double>> predictorVariablesTraining = new List<List<double>>();
        private List<double> predictedVariablesTraining;
        private List<List<double>> predictorVariablesTest = new List<List<double>>();

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
        public int InputDim { get => inputDim; set => inputDim = value; }
        public List<List<double>> PredictorVariablesTraining { get => predictorVariablesTraining; set => predictorVariablesTraining = value; }
        public List<double> PredictedVariablesTraining { get => predictedVariablesTraining; set => predictedVariablesTraining = value; }
        public List<List<double>> PredictorVariablesTest { get => predictorVariablesTest; set => predictorVariablesTest = value; }
        public object NdPredictorData { get => ndPredictorData; set => ndPredictorData = value; }
        public object NdPredictedData { get => ndPredictedData; set => ndPredictedData = value; }

        public ANNTrainingOptions()
        {
            this.BatchSize = BATCH_SIZE_NUMBER;
            this.EpochNumber = EPOCH_NUMBER;
            this.CostFunction = COST_FUNCTION;
            this.Optimizer = OPTIMIZER;
            this.KernelInitializer = KERNEL_INITIALIZER;
            this.ActivationFunction = ACTIVATION_FUNCTION;
            this.NumberOfHiddenLayers = NUMBER_OF_HIDDEN_LAYERS;
            this.NumberOfNeuronsInFirstHiddenLayer = NUMBER_OF_NEURONS_IN_FIRST_HIDDEN_LAYER;
            this.NumberOfNeuronsInOtherHiddenLayers = NUMBER_OF_NEURONS_IN_OTHER_HIDDEN_LAYERS;
            this.Verbose = VERBOSE;
        }
    }
}
