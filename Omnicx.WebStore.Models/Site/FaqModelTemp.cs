using System;
using System.Collections.Generic;

namespace Omnicx.WebStore.Models.Site
{
    [Serializable]
    public class FaqsCategoryModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    [Serializable]
    public class FaqsSubCategoryModel
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
    [Serializable]
    public class FaqsModel
    {
        public List<FaqsCategoryModel> FaqsCategory { get; set; }
        public List<FaqsSubCategoryModel> FaqsSubCategory { get; set; }
    }
}