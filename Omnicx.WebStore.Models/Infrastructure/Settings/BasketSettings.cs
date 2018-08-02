using System;

namespace Omnicx.WebStore.Models.Infrastructure.Settings
{
    [Serializable]
    public class BasketSettings : ISettings
    {
        /// <summary>
        /// Gets or sets a value indicating maximum number of items in the basket
        /// </summary>
        public int MaximumBasketItems { get; set; }
        public bool ShowWishlist { get; set; }
        /// <summary>
        /// Gets or sets a value indicating maximum number of items in the wishlist
        /// </summary>
        public int MaximumWishlistItems { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether "email a wishlist" feature is enabled
        /// </summary>
        public bool EmailWishlistEnabled { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to enabled "email a wishlist" for anonymous users.
        /// </summary>
        public bool AllowAnonymousUsersToEmailWishlist { get; set; }

        public bool EnableIsGiftORMe { get; set; }

        public bool RegistrationPrompt { get; set; }


    }
}
