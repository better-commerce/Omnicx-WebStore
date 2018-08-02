using System;
using System.Collections.Generic;
using System.Linq;

namespace Omnicx.WebStore.Models.Catalog
{
    [Serializable]
    public class VariantModel
    {

        public VariantInputTypes VariantInputType { get; set; }
        public VariantDisplayInSearch ListType { get; set; }
        public bool IndependentProductUrl { get; set; }
        public bool DisplayInProductWidget { get; set; }
        public bool DisplayInProductDetail { get; set; }
        public List<VariantAttributeModel> Attributes { get; set; }
    }
    public enum VariantDisplayInSearch
    {
        Rollup = 1,
        Expanded = 2
    }

    public enum VariantInputTypes
    {
        Dropdown = 1,
        HorizontalList = 2
    }
    [Serializable]
    public class VariantAttributeModel
    {
        public string FieldName { get; set; }
        public string FieldCode { get; set; }
        public string InputType { get; set; }
        public List<VariantProductModel> Products { get; set; }
        public bool IsColorAttribute {
            get
            {
                if (Products == null) return false;
                if (
                    Products.Any(
                        x => x.FieldValue.ToLower().StartsWith("#") || x.FieldValue.ToLower().StartsWith("rgb{")))
                    return true;
                return false;
            }
            
        }
    }
    [Serializable]
    public class VariantProductModel
    {
        public string StockCode { get; set; }
        public Guid ProductId { get; set; }
        public bool IsDefault { get; set; }
        public string FieldValue { get; set; }
        
    }
}
