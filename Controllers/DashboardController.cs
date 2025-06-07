using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EvaluadorInteligente.Models;
using EvaluadorInteligente.ML;
using EvaluadorInteligente.ML.Recommendation;


namespace EvaluadorInteligente.Controllers
{
    public class DashboardController : Controller
    {
        private readonly SentimentTrainer _sentiment = new SentimentTrainer();
        private readonly RecommendationTrainer _recommender = new RecommendationTrainer();

        [HttpGet]
        public IActionResult Index()
        {
            return View(new DashboardViewModel());
        }

        [HttpPost]
        public IActionResult Analyze(DashboardViewModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.SentimentInput.Text))
            {
                var result = _sentiment.Predict(model.SentimentInput.Text);
                model.SentimentInput.Prediction = result.Prediction ? "Positivo" : "Negativo";
                model.SentimentInput.Score = result.Probability.ToString("P2");
            }
            return View("Index", model);
        }

        [HttpPost]
        public IActionResult Recommend(DashboardViewModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.RecommendationInput.UserId))
            {
                var recomendaciones = _recommender.GetRecommendations(model.RecommendationInput.UserId);
                model.RecommendationInput.RecommendedProducts = recomendaciones;

                if (!recomendaciones.Any())
                {
                    ModelState.AddModelError("Recommendation", "No se encontraron recomendaciones para este usuario.");
                }
            }

            return View("Index", model);
        }
    }
}