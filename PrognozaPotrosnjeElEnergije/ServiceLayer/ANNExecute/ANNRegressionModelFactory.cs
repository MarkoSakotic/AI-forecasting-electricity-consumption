using Keras.Models;
using Numpy;
using ServiceLayer.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.ANNExecute
{
    public class ANNRegressionModelFactory
    {
        private ANNTrainingOptions trainingOptions;

        public ANNRegressionModelFactory(ANNTrainingOptions trainingOptions)
        {
            this.trainingOptions = trainingOptions;
        }

        public BaseModel GetModel()
        {
            var ann = new ANNRegression(this.trainingOptions);
            var model = ann.GetSequentialModel((NDarray)trainingOptions.NdPredictorData, (NDarray)trainingOptions.NdPredictedData);
            return model;
        }
    }
}
