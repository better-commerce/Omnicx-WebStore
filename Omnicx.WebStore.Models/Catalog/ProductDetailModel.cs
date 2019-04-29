using System;
using System.Collections.Generic;

using Omnicx.WebStore.Models.Common;
using Omnicx.WebStore.Models.Helpers;
using Omnicx.WebStore.Models.Site;
using Omnicx.WebStore.Models.Infrastructure.Settings;
using Omnicx.WebStore.Models.Keys;
using Omnicx.WebStore.Models.Enums;

namespace Omnicx.WebStore.Models.Catalog
{
    [Serializable]
    public partial class ProductDetailModel : IHaveSeoInfo
    {
     

        #region basic properties
        
        public string Id { get; set; }
        public string RecordId { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }

        public string SeoName { get; set; }
        public string BrandId { get; set; }
        public string Brand { get; set; }
        public string SubBrand { get; set; }
        public string SubBrandId { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string StockCode { get; set; }
        public string Sku { get; set; }
        public string Barcode { get; set; }
        public Amount ListPrice { get; set; }
        public Amount Price { get; set; }

        public string Uom { get; set; }
        public string UomValue { get; set; }

        public string AttributeSet { get; set; }
        public bool SubscriptionEnabled { get; set; }
        public SubscriptionPlanType SubscriptionPlanType { get; set; }

        #endregion

        public VariantModel Variant { get; set; }

        public ClassficationModel Classification { get; set; }
        public List<ComponentProductsModel> ComponentProducts { get; set; }
        public bool SoldIndependently { get; set; }
        public DimensionsModel Dimensions { get; set; }
        public IList<ImageModel> Images { get; set; }
        public SeoInfoModel SeoInfo { get; set; }

        public FlagModel Flags { get; set; }
        public PreOrderModel PreOrder { get; set; }

        public IList<ProductAttributeModel> Attributes { get; set; }
        public IList<ProductReviewModel> Reviews { get; set; }
        public IList<ProductModel> RelatedProductList { get; set; }

        public string Image { get; set; }
        public string VideoId { get; set; }
        public int CurrentStock { get; set; }
        public List<FaqsCategoryModel> FaqsCategory { get; set; }
        public List<FaqsSubCategoryModel> FaqsSubCategory { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string CanonicalTags { get; set; }
        public bool IsDiscontinued { get; set; }
        public IList<ProductModel> FreeProducts { get; set; }
        public int Rating { get; set; }
        public List<VariantProductsModel> VariantProducts { get; set; }
        public List<VariantProductAttributes> VariantProductsAttribute { get; set; }
        public string BrandRecordId { get; set; }
        public string BrandSlug { get; set; }
        public bool InWishList { get; set; }
        public List<LocalizedSlugModel> BreadCrumbs { get; set; }
    }

    [Serializable]
    public class ClassficationModel
    {
        #region classficiation properties
        public string CategoryCode { get; set; }
        public string Category { get; set; }
        public string ItemTypeCode { get; set; }
        public string ItemType { get; set; }
        public string MainCategoryId { get; set; }
        public string MainCategoryName { get; set; }
        #endregion
    }
    [Serializable]
    public class ComponentProductsModel
    {
        #region componentProducts properties
        public string ProductId { get; set; }
        public string StockCode { get; set; }
        public string Qty { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Brand { get; set; }
        public string SubBrand { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public IList<ImageModel> Images { get; set; }
        #endregion
    }

    [Serializable]
    public class DimensionsModel
    {
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
    }
    [Serializable]
    public class ImageModel
    {
        public string Title { get; set; }
        public string Tag { get; set; }
        public string Image { get; set; }
        public ImageObjectTypes? ImageTypes { get; set; }
        public string Url { get; set; }
        public string Alt { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public string SubTitle { get; set; }
        public string LinkType { get; set; }
        public string ButtonText { get; set; }

    }
    [Serializable]
    public class SeoInfoModel
    {
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string CanonicalTags { get; set; }

    }
    [Serializable]
    public class FlagModel
    {
        public bool IsFreeDelivery { get; set; }
        public bool SellWithoutInventory { get; set; }
    }
    [Serializable]
    public class RelatedProductModel
    {
        public Guid RecordId { get; set; }
        public Guid ProductId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string StockCode { get; set; }
        public string RelatedTypeCode { get; set; }
        public string RelationDirectionCode { get; set; }
        public string Image { get; set; }
        public Amount ListPrice { get; set; }
        public Amount Price { get; set; }
    }
    [Serializable]
    public class PreOrderModel
    {
        public bool IsEnabled { get; set; }
        public string ShortMessage { get; set; }
        public string Message { get; set; }
        public DateTime LaunchDateTime { get; set; }
    }
    [Serializable]
    public class ProductAttributeModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Display { get; set; }
        public string Customkey
        {
            get
            {
                return Key + CharReplacement.Sperator + (string.IsNullOrEmpty(Value) ? "" : Value) + CharReplacement.Sperator + Display;

            }
        }
        public bool IsVariantAttribute { get; set; }
        public bool DisplayInProductDetail { get; set; }
    }
    [Serializable]
    public class ProductReviewModel
    {
        public string Title { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string PostedBy { get; set; }
        public List<ProductReviewSection> Sections { get; set; }
    }
    [Serializable]
    public class VariantProductsModel
    {
        public string StockCode { get; set; }
        public Guid ProductId { get; set; }
        public bool IsDefault { get; set; }
        public List<VariantAttributesModel> VariantAttributes { get; set; }
    }
    [Serializable]
    public class VariantAttributesModel
    {
        public string FieldName { get; set; }
        public string FieldCode { get; set; }
        public string FieldValue { get; set; }
        public bool Available { get; set; }
        public bool Selected { get; set; }
    }
    [Serializable]
    public class VariantProductAttributes
    {
        public string FieldName { get; set; }
        public string FieldCode { get; set; }
        public string InputType { get; set; }
        public bool IndependentProductUrl { get; set; }
        public bool DisplayInProductDetail { get; set; }
        public bool DisplayInProductWidget { get; set; }
        public List<VariantAttributesModel> FieldValues { get; set; }
    }
    [Serializable]
    public class ClassficationForIndex
    {
        #region classficiation properties
        public string CategoryId { get; set; }
        public string CategoryCode { get; set; }
        public string Category { get; set; }
        public string MainCategoryId { get; set; }
        public string MainCategoryName { get; set; }
        public string ItemTypeCode { get; set; }
        public string ItemType { get; set; }
        public string AttributeSet { get; set; }
        public string AttributeSetId { get; set; }
        #endregion
    }
    [Serializable]
    public class LocalizedSlugModel
    {
        public string SlugType { get; set; }
        public SlugModel Slug { get; set; }
    }
    [Serializable]
    public class SlugModel
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public SlugModel ChildSlug { get; set; }
    }
}