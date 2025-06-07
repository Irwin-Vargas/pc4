using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.IO;

namespace EvaluadorInteligente.ML
{
    public class SentimentData
    {
        [LoadColumn(0)] public bool Label { get; set; }
        [LoadColumn(1)] public string Text { get; set; }
    }

    public class SentimentPrediction
    {
        [ColumnName("PredictedLabel")] public bool Prediction { get; set; }
        public float Probability { get; set; }
        public float Score { get; set; }
    }

    public class SentimentTrainer
    {
        private readonly string dataPath = "Data/sentiment-data.tsv";
        private readonly string modelPath = "ML/Models/sentiment-model.zip";
        private readonly MLContext mlContext;
        private PredictionEngine<SentimentData, SentimentPrediction> predictionEngine;

        public SentimentTrainer()
        {
            mlContext = new MLContext();

            if (File.Exists(modelPath))
            {
                var model = mlContext.Model.Load(modelPath, out _);
                predictionEngine = mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(model);
            }
            else
            {
                TrainModel();
            }
        }

        public void TrainModel()
        {
            var data = mlContext.Data.LoadFromTextFile<SentimentData>(dataPath, hasHeader: true);
            var pipeline = mlContext.Transforms.Text.FeaturizeText("Features", nameof(SentimentData.Text))
                .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression());

            var model = pipeline.Fit(data);
            mlContext.Model.Save(model, data.Schema, modelPath);
            predictionEngine = mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(model);
        }

        public SentimentPrediction Predict(string input)
        {
            return predictionEngine.Predict(new SentimentData { Text = input });
        }
    }
}
