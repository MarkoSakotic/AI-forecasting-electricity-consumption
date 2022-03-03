using AccessDataLayer;
using DataLayer;
using Keras.Models;
using Numpy;
using ServiceLayer.Options;
using ServiceLayer.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.ANNExecute
{
    public class ANNExecutor
    {
        DataHandling dh = new DataHandling();
        public ANNResults Run(ANNTrainingOptions trainingOptions)
        {
            ANNResults results = new ANNResults();
            FillOptions(trainingOptions);
            ANNRegressionModelFactory factory = new ANNRegressionModelFactory(trainingOptions);
            var model = factory.GetModel();
            SaveModelJson(model);
            var predictedValue = model.Predict(np.array(GetPredictorVariables(trainingOptions.PredictorVariablesTest, trainingOptions.PredictorVariablesTest.Count, trainingOptions.PredictorVariablesTest[0].Count))).astype(np.float32);
            results.PredictedValues = ToList(predictedValue);
            return results;
        }

        public ANNResults RunPredict(ANNTrainingOptions trainingOptions)
        {
            ANNResults results = new ANNResults();
            var model = LoadModel();
            var predictedValue = model.Predict(np.array(GetPredictorVariables(trainingOptions.PredictorVariablesTest, trainingOptions.PredictorVariablesTest.Count, trainingOptions.PredictorVariablesTest[0].Count))).astype(np.float32);
            results.PredictedValues = ToList(predictedValue);
            return results;
        }

        private List<double> ToList(NDarray arr)
        {
            List<double> list = new List<double>();
            for (int i = 0; i < arr.len; i++)
            {
                list.Add((double)arr[i][0]);
            }
            return list;
        }

        private void FillOptions(ANNTrainingOptions trainingOptions)
        {
            int inputDim = trainingOptions.PredictorVariablesTest[0].Count;
            trainingOptions.InputDim = inputDim;
            double[] predictedData = trainingOptions.PredictedVariablesTraining.ToArray();
            double[,] predictorData = GetPredictorVariables(trainingOptions.PredictorVariablesTraining, trainingOptions.PredictorVariablesTraining.Count, trainingOptions.PredictorVariablesTraining[0].Count);
            trainingOptions.NdPredictedData = np.array(predictedData);
            trainingOptions.NdPredictorData = np.array(predictorData);
        }

        private double[,] GetPredictorVariables(List<List<double>> predictorVariablesList, int x, int y)
        {
            double[,] predVariables = new double[x, y];
            for (int i = 0; i < predictorVariablesList.Count; i++)
            {
                for (int j = 0; j < predictorVariablesList[i].Count; j++)
                {
                    predVariables[i, j] = predictorVariablesList[i][j];
                }
            }
            return predVariables; //[26352, 5]
        }

        private double[] ToArray(List<double> valuesList)
        {
            double[] arr = new double[valuesList.Count];
            for (int i = 0; i < valuesList.Count; i++)
            {
                arr[i] = valuesList[i];
            }
            return arr;
        }

        private void SaveModelJson(BaseModel model)
        {
            string json = model.ToJson();
            File.WriteAllText("model.json", json);
            model.SaveWeight("model.h5");
        }

        private BaseModel LoadModel()
        {
            var loaded_model = Sequential.ModelFromJson(File.ReadAllText("model.json"));
            loaded_model.LoadWeight("model.h5");
            return loaded_model;
        }
    }
}
