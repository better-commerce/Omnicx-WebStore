using System;
using System.Collections.Generic;
using Omnicx.WebStore.Models.Common;

namespace Omnicx.WebStore.Models.Catalog
{
    [Serializable]
    public class TypeAheadSearchModel
    {
        public List<EntityTypeAheadModel> Products { get; set; }
        public List<EntityTypeAheadModel> Categories { get; set; }
        public List<EntityTypeAheadModel> Blogs { get; set; }
        public List<EntityTypeAheadModel> Brands { get; set; }
    }
    [Serializable]
    public class EntityTypeAheadModel
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Slug { get; set; }
        public Amount Price { get; set; }
    }
}
