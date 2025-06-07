using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaluadorInteligente.Models
{
    public class RecommendationInput
    {
        public string? UserId { get; set; }
        public List<string>? RecommendedProducts { get; set; }
    }
}