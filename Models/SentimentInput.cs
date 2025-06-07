using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaluadorInteligente.Models
{
    public class SentimentInput
    {
        public string? Text { get; set; } = "";
        public string? Prediction { get; set; } = "";
        public string? Score { get; set; } = "";
    }
}
