using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EvaluadorInteligente.ML;
using EvaluadorInteligente.Models;

namespace EvaluadorInteligente.Controllers
{
    public class SentimentController : Controller
    {
        private readonly SentimentTrainer trainer = new SentimentTrainer();

        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult Index(SentimentInput input)
        {
            if (!string.IsNullOrWhiteSpace(input.Text))
            {
                var result = trainer.Predict(input.Text);
                input.Prediction = result.Prediction ? "Positivo" : "Negativo";
                input.Score = result.Probability.ToString("P2");
            }

            return View(input);
        }
    }
}