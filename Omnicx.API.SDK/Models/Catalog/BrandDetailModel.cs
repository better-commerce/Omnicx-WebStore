using System.Collections.Generic;
using Omnicx.API.SDK.Models.Site;
using Omnicx.API.SDK.Models.Helpers;

namespace Omnicx.API.SDK.Models.Catalog
{
    public class BrandDetailModel : IHaveSeoInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LogoImageName { get; set; }
        public string ProductImage { get; set; }
        public string Description { get; set; }
        public string CarouselImage1 { get; set; }
        public string CarouselImage2 { get; set; }
        public string CarouselImage3 { get; set; }
        public string CarouselImage4 { get; set; }
        public string CarouselLink1 { get; set; }
        public string CarouselLink2 { get; set; }
        public string CarouselLink3 { get; set; }
        public string CarouselLink4 { get; set; }
        public string WidgetLink1 { get; set; }
        public string WidgetLink2 { get; set; }
        public string WidgetLink3 { get; set; }
        public string WidgetLink4 { get; set; }
        public string WidgetImage1 { get; set; }
        public string WidgetImage2 { get; set; }
        public string WidgetImage3 { get; set; }
        public string WidgetImage4 { get; set; }
        public string VideoId { get; set; }
        public string ParentManufacturerName { get; set; }
        public List<SubBrandModel> SubBrands { get; set; }
        public bool ShowLandingPage { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string CanonicalTags { get; set; }
        public PaginatedResult<ProductModel> ProductList { get; set; }
        public string Link { get; set; }
    }

    public class SubBrandModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public List<ProductModel> Products { get; set; }

    }
}