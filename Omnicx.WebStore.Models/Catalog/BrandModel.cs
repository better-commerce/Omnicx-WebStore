using System;

namespace Omnicx.WebStore.Models.Catalog
{
    [Serializable]
    public class BrandModel
    {
        public string Id { get; set; }
        public string ManufacturerName { get; set; }
        public string LogoImageName { get; set; }
        public string ProductImage { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
    }
}