using System.Collections.Generic;
using Omnicx.WebStore.Models.Site;


namespace Omnicx.WebStore.Models.Catalog
{
    public class MicrositeModel 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public bool IsActive { get; set; }
        public bool ShowInSiteMap { get; set; }
        public bool IncludeInSearch { get; set; }
        public bool DisplayInHeader { get; set; }
        public string Logo { get; set; }
        public bool IsPrimary { get; set; }
        public int Priority { get; set; }
        public int DisplayOrder { get; set; }

    
    }

    public class MicrositeDetailModel 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public bool IsActive { get; set; }
        public bool ShowInSiteMap { get; set; }
        public bool IncludeInSearch { get; set; }
        public bool DisplayInHeader { get; set; }
        public string Logo { get; set; }
        public bool IsPrimary { get; set; }
        public int Priority { get; set; }
        public int DisplayOrder { get; set; }
        public List<NavigationModel> Navigations { get; set; }
        public List<CollectionModel> Collections { get; set; }

    }
}