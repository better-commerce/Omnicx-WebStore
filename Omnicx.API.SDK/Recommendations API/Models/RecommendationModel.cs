using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.API.SDK.Recommendations_API.Models
{
    [Serializable]
    public class RecommendationModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int RecommendationMasterId { get; set; }
    }
}
