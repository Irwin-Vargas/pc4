using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using System.IO;

namespace EvaluadorInteligente.ML.Recommendation
{
    public class ProductRating
    {
        [LoadColumn(0)] public string? UserId { get; set; }
        [LoadColumn(1)] public string? ProductId { get; set; }
        [LoadColumn(2)] public float Label { get; set; }
    }

    public class ProductPrediction
    {
        public float Score { get; set; }
    }

    public class RecommendationTrainer
    {
        private readonly string dataPath = "Data/ratings-data.csv";
        private readonly MLContext mlContext;
        private ITransformer model = null!;
        private IDataView trainingData = null!;

        public RecommendationTrainer()
        {
            mlContext = new MLContext();
            TrainModel();
        }

        private void TrainModel()
        {
            trainingData = mlContext.Data.LoadFromTextFile<ProductRating>(dataPath, hasHeader: true, separatorChar: ',');

            var pipeline = mlContext.Transforms.Conversion
                .MapValueToKey("UserIdEncoded", nameof(ProductRating.UserId))
                .Append(mlContext.Transforms.Conversion.MapValueToKey("ProductIdEncoded", nameof(ProductRating.ProductId)))
                .Append(mlContext.Recommendation().Trainers.MatrixFactorization(new MatrixFactorizationTrainer.Options
                {
                    MatrixColumnIndexColumnName = "UserIdEncoded",
                    MatrixRowIndexColumnName = "ProductIdEncoded",
                    LabelColumnName = nameof(ProductRating.Label),
                    LossFunction = MatrixFactorizationTrainer.LossFunctionType.SquareLossRegression,
                    NumberOfIterations = 20,
                    ApproximationRank = 100
                }));

            model = pipeline.Fit(trainingData);
        }

        public List<string> GetRecommendations(string userId, int top = 5)
        {
            // Validar si el userId existe en el dataset
            var users = mlContext.Data.CreateEnumerable<ProductRating>(trainingData, reuseRowObject: false)
                                      .Select(x => x.UserId!)
                                      .Distinct()
                                      .ToList();

            if (!users.Contains(userId))
            {
                return new List<string>(); // No hacer recomendaciones si el usuario no existe
            }

            var products = mlContext.Data.CreateEnumerable<ProductRating>(trainingData, reuseRowObject: false)
                                         .Select(x => x.ProductId!)
                                         .Distinct()
                                         .ToList();

            var predictionEngine = mlContext.Model.CreatePredictionEngine<ProductRating, ProductPrediction>(model);
            var predictions = new List<(string productId, float score)>();

            foreach (var productId in products)
            {
                var prediction = predictionEngine.Predict(new ProductRating
                {
                    UserId = userId,
                    ProductId = productId
                });

                predictions.Add((productId, prediction.Score));
            }

            return predictions.OrderByDescending(p => p.score)
                              .Take(top)
                              .Select(p => p.productId)
                              .ToList();
        }
    }
}
