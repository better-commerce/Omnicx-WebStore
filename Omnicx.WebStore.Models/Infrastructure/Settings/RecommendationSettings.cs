using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.WebStore.Models.Infrastructure.Settings
{
    [Serializable]
    public class RecommendationSettings
    {       
        public string RecommederKey { get; set; }
        public string AzureStorageConString { get; set; }
        public string ApiEndPoint { get; set; }
        public string BlobContainer { get; set; }

        public string ProductModelId { get; set; }
        public string HomeModelId { get; set; }
        public string BasketModelId { get; set; }

        public string PersonalisedModelId { get; set; }
        public string OrderModelId { get; set; }
        public string PromotionModelId { get; set; }
        public string NewForYouModelId { get; set; }
        public string PopularModelId { get; set; }

    }
}
