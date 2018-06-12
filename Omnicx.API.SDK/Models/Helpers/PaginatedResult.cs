using System.Collections.Generic;
using Newtonsoft.Json;
using Omnicx.API.SDK.Models.Catalog;
using System;

namespace Omnicx.API.SDK.Models.Helpers
{
    [Serializable]
    public class PaginatedResult<T>
    {
        private SearchRequestModel _searchCriteria;
        private IList<SearchFilterModel> _filters;
        private IList<SearchFacetModel> _facets;
        public int Total { get; set; }
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
        public string PrevPageUrl { get; set; }
        public  string NextPageUrl { get; set; }

        public IList<T> Results { get; set; }

        [JsonIgnore]
        public IList<SearchFacetModel> Facets {
            get { return _facets ?? (_facets = new List<SearchFacetModel>()); }
            set { _facets = value; } }

      
        public IList<SearchFilterModel> Filters {
            get{return _filters ?? (_filters = new List<SearchFilterModel>());}
            set { _filters = value; } }

        public SearchRequestModel SearchCriteria
        { //empty objects are initialized to avoid throwing object reference error when referred in the views.
            get { return _searchCriteria ?? (_searchCriteria = new SearchRequestModel()); } 
            set { _searchCriteria = value; }
        }
        public IList<KeyValuePair<string, string>> SortList { get; set; }
        public string SortBy { get; set; }
        public List<ImageModel> Images { get; set; }
        public GroupModel ProductGroupModel { get; set; }
        public List<string> Groups { get; set; }

        public string CustomInfo1 { get; set; }
        public string CustomInfo2 { get; set; }
        public string CustomInfo3 { get; set; }
    }
}