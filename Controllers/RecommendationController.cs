using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EvaluadorInteligente.Models;
using EvaluadorInteligente.ML.Recommendation;


namespace EvaluadorInteligente.Controllers
{
    public class RecommendationController : Controller
    {
        private readonly RecommendationTrainer _trainer = new RecommendationTrainer();

        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult Index(RecommendationInput input)
        {
            if (!string.IsNullOrWhiteSpace(input.UserId))
            {
                input.RecommendedProducts = _trainer.GetRecommendations(input.UserId);
            }

            return View(input);
        }
    }
}