using System;
using System.Linq;
using System.Web.Mvc;
using Omnicx.API.SDK.Api.Catalog;
using Omnicx.WebStore.Models.Catalog;
using Omnicx.WebStore.Models.Helpers;

using Omnicx.API.SDK.Api.Infra;
using Omnicx.WebStore.Models.Keys;
using Omnicx.WebStore.Models.Enums;
namespace Omnicx.WebStore.Core.Helpers
{
    public static class SearchHelper
    {        
        public static PaginatedResult<ProductModel> GetPaginatedProducts(SearchRequestModel searchCriteria)
        {
            var sessionContext = DependencyResolver.Current.GetService<ISessionContext>();
            searchCriteria.CurrentPage = searchCriteria.CurrentPage == 0 ? 1 : searchCriteria.CurrentPage;
            searchCriteria.PageSize = searchCriteria.PageSize == 0 ? Convert.ToInt32(ConfigKeys.PageSize) : searchCriteria.PageSize;
            searchCriteria.SortBy = string.IsNullOrWhiteSpace(searchCriteria.SortBy) ? sessionContext?.CurrentSiteConfig?.SearchSettings?.DefaultSortBy : searchCriteria.SortBy;
            var productApi = DependencyResolver.Current.GetService<IProductApi>();
            var response = productApi.GetProducts(searchCriteria);
            var result = new PaginatedResult<ProductModel>();
            if (response != null && response.Result != null)
            {
                result = response.Result;
            }
            result.SearchCriteria = searchCriteria;
            if (result.Filters == null) return result;
            var validFilters = result.Filters.GroupBy(c => new { c.Name }).Select(c => c.First()).ToList();
            result.Filters = validFilters;
            // result.Filters = validFilters.Where(p => !validFilters.Any(p2 => p2.Name == p.Name && p.Items.Count == 0)).ToList();
            if (result.Filters != null)
            {
                foreach (var filter in result.Filters)
                {
                    foreach (var itm in filter.Items)
                    {
                        if (filter.Name != null && filter.Name.ToLower() == Constants.PRICE_FILTER && itm.From != null)
                        {
                            itm.PriceFilter = ((int)Convert.ToDecimal(itm.From));
                        }
                        if (searchCriteria.Filters != null)
                        {
                            itm.IsSelected = searchCriteria.Filters.Any(x => x.Key == filter.Key && x.Value == itm.Name);
                        }
                    }
                    filter.Items = filter.Items.GroupBy(x => x.Name).Select(x => x.FirstOrDefault()).ToList();
                    filter.Items = filter.Items.OrderByDescending(x => x.IsSelected).ThenBy(x => x.Name).ToList();
                }
            }
            return result;
        }
    }
}