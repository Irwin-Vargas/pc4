using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaluadorInteligente.Models
{
    public class DashboardViewModel
    {
        public SentimentInput SentimentInput { get; set; } = new();
        public RecommendationInput RecommendationInput { get; set; } = new();
    }
}
