using System.Collections.Generic;
using Omnicx.WebStore.Models.Common;
using Omnicx.WebStore.Models.Site;
using System.ComponentModel.DataAnnotations;
using System;
using Omnicx.WebStore.Models.Infrastructure.Settings;

namespace Omnicx.WebStore.Models.Catalog
{
    [Serializable]
    public class ProductModel
    {
        
        #region basic properties

        public string Id { get; set; }
        public string RecordId { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Slug { get; set; }
        public string SeoName { get; set; }
        public string Brand { get; set; }
        public string SubBrand { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string StockCode { get; set; }
        public string Sku { get; set; }
        public string Barcode { get; set; }
        public Amount ListPrice { get; set; }
        public Amount Price { get; set; }

        public string Uom { get; set; }
        public string UomValue { get; set; }
        public IList<ProductAttributeModel> Attributes { get; set; }
        public VariantModel Variant { get; set; }
        public string AttributeSet { get; set; }
        public IList<ImageModel> Images { get; set; }

        public string Image { get; set; }
        public List<string> SortBy { get; set; }
        public FlagModel Flags { get; set; }
        public bool IsDiscontinued { get; set; }
        public IList<ProductModel> FreeProducts { get; set; }
        public bool InWishList { get; set; }
        public int CurrentStock { get; set; }
        public ClassficationForIndex Classification { get; set; }
        public string GroupName { get; set; }

        #endregion

    }
    [Serializable]
    public class GroupModel
    {
        public bool AllowGrouping { get; set; }
        public string GroupSeparator { get; set; }
        public string GroupCode { get; set; }
        public bool DisplayTitle { get; set; }
        public List<string> Groups { get; set; }

    }

    public class AutoSearchModel
    {
        public List<ProductModel> ProductResult { get; set; }
        public IList<BlogModel> BlogResult { get; set; }
    }

    public class ProductReviewAddModel
    {
        [Required]
        public string Title { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public bool IsRecommended { get; set; }
        public List<ProductReviewSection> ReviewSections { get; set; }
        public string UserId { get; set; }
        public KeyValuePair<string , string> AdditionalData { get; set; }

        public bool RemainAnonymous { get; set; }
        [Required]
        public string Nickname { get; set; }
        [Required]
        public string Location { get; set; }

        public int Age { get; set; }
        public string Gender { get; set; }

        public string UserEmail { get; set; }
    }


}