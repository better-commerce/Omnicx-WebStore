using System;
using System.Collections.Generic;

namespace Omnicx.API.SDK.Models.Infrastructure.Settings
{
    [Serializable]
    public class ProductReviewSection
    {
        //public string Id { get; set; }
        public string SectionName { get; set; }
        public List<SectionOption> Options { get; set; }
    }

    [Serializable]
    public class SectionOption
    {
        //public string Id { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
        //public string ParentItemValue { get; set; }

    }
}
