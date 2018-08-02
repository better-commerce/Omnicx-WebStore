using System;

namespace Omnicx.WebStore.Models.Infrastructure.Settings
{
    [Serializable]
    public class CatalogSettings : ISettings
    {

        public bool DisplayOutOfStock { get; set; }
        
        /// <summary>
        /// Gets or set image url
        /// </summary>
        public string ImageUrl { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating product reviews must be approved
        /// </summary>
        public bool ProductReviewsMustBeApproved { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow anonymous users write product reviews.
        /// </summary>
        public bool AllowAnonymousUsersToReviewProduct { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether "Recently viewed products" feature is enabled
        /// </summary>
        public bool RecentlyViewedProductsEnabled { get; set; }

        /// <summary>
        /// Gets or sets a number of "Recently viewed products"
        /// </summary>
        public int RecentlyViewedProductsNumber { get; set; }

        /// <summary>
        /// Gets or sets a number of "Recently added products"
        /// </summary>
        public int RecentlyAddedProductsNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether "Recently added products" feature is enabled
        /// </summary>
        public bool RecentlyAddedProductsEnabled { get; set; }

        public bool NotifyMe { get; set; }

        public string EmailOutOfStock { get; set; }


        public bool EnableBlobStorage { get; set; }
        //public string BlobContainerName { get; set; }
        public string PrimaryBlobContainer { get; set; }
        public string SecondaryBlobContainer { get; set; }




    }
}
