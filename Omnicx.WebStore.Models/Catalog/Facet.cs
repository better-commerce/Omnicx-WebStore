using Omnicx.WebStore.Models.Enums;
using System;


namespace Omnicx.WebStore.Models.Catalog
{
    [Serializable]
    public class Facet
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }

        public FacetType Type { get; set; }
    }
}