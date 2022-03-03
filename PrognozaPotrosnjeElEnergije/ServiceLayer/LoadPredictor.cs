using AccessDataLayer;
using DataLayer;
using ServiceLayer.ANNExecute;
using ServiceLayer.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class LoadPredictor
    {
        private const double frac = 0.9f;
        DataHandling dh = new DataHandling();

        /********************Prediction function, that returns the predicted values.******************************************************/
        public List<double> Predict(List<Weather> weathers)
        {
            
            var trainingData = GetTrainingData(weathers);
            var predictorData = trainingData.Item1;
            var predictedData = trainingData.Item2;

            ANNTrainingOptions trainingOptions = new ANNTrainingOptions();

            trainingOptions.PredictorVariablesTest = predictorData;
            ANNExecutor annExecutor = new ANNExecutor();

            var results = annExecutor.RunPredict(trainingOptions);

            List<double> pom = new List<double>();
            pom = dh.DenormalizeLoad(results.PredictedValues);

            return pom;

        }


        /********************Training function, that returns the difference between actual and predicted values******************************************************/
        public string Train(List<Weather> weathers)
        {
            var trainingData = GetTrainingData(weathers);
            var predictorData = trainingData.Item1;
            var predictedData = trainingData.Item2;

            int cnt = predictedData.Count;
            int trainCnt = (int)Math.Round((cnt * frac), 0);
            ANNTrainingOptions trainingOptions = new ANNTrainingOptions();

            trainingOptions.PredictorVariablesTraining = predictorData.Take(trainCnt).ToList();
            trainingOptions.PredictorVariablesTest = predictorData.Skip(trainCnt).ToList();
            trainingOptions.PredictedVariablesTraining = predictedData.Take(trainCnt).ToList(); 

            List<double> predictedVariablesTest = predictedData.Skip(trainCnt).ToList(); 

            ANNExecutor annExecutor = new ANNExecutor();

            var results = annExecutor.Run(trainingOptions);

            List<double> predictedList = new List<double>(); //predicted values
            List<double> realList = new List<double>(); //real values


            foreach (var item in predictedVariablesTest)
            {
                realList.Add(item);
            }

            foreach (var item in results.PredictedValues)
            {
                predictedList.Add(item);
            }

            List<double> predictedListDenormalize = new List<double>();
            List<double> realListDenormalize = new List<double>();


            predictedListDenormalize = dh.DenormalizeLoad(predictedList);
            realListDenormalize = dh.DenormalizeLoad(realList);
            //predictedListDenormalize = predictedList;
            //realListDenormalize = realList;


            double sqrDeviation = GetSquareDeviation(results.PredictedValues, predictedVariablesTest);
            double error = 0;
            for (int i = 0; i < results.PredictedValues.Count; i++)
            {
                error += Math.Abs(predictedListDenormalize[i] - realListDenormalize[i]) / realListDenormalize[i] * 100 / results.PredictedValues.Count;
            }

            //string res = "SQR Deviation: " + sqrDeviation.ToString();
            string res = error.ToString();
            return res;

        }

        private Tuple<List<List<double>>, List<double>> GetTrainingData(List<Weather> weathers)
        {
            List<List<double>> predictorData = new List<List<double>>();
            List<double> predictedData = new List<double>();
            foreach (Weather rowValues in weathers)
            {
                List<double> rowPredictorData = new List<double>();
                rowPredictorData.Add(rowValues.AirTemperature);
                rowPredictorData.Add(rowValues.AtmosphericPressure);
                rowPredictorData.Add(rowValues.CloudCover);
                rowPredictorData.Add(rowValues.PressureTendency);
                rowPredictorData.Add(rowValues.Pressure);
                rowPredictorData.Add(rowValues.Day);
                rowPredictorData.Add(rowValues.Hour);
                rowPredictorData.Add(rowValues.Month);
                rowPredictorData.Add(rowValues.MeanWindSpeed);
                rowPredictorData.Add(rowValues.TypeOfDay);

                predictedData.Add(rowValues.LoadMWh);
                predictorData.Add(rowPredictorData);

            }   
            return new Tuple<List<List<double>>, List<double>>(predictorData, predictedData);
            //predictor all data from excel, predcited - all data for load
        }

        public double GetSquareDeviation(List<double> l1, List<double> l2)
        {
            if (l1.Count != l2.Count)
            {
                throw new Exception("Different lenghts");
            }
            List<double> deviations = new List<double>();
            for (int i = 0; i < l1.Count; i++)
            {
                deviations.Add(Math.Pow((double)(new decimal(l1[i])) - (double)(new decimal(l2[i])), 2));
            }
            return Math.Sqrt(deviations.Average());
        }


    }
}
