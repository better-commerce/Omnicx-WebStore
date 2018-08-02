using System.Collections.Generic;
using Omnicx.WebStore.Models.Catalog;
using Omnicx.WebStore.Models;
namespace Omnicx.API.SDK.Api.Catalog
{
   public interface ICollectionApi
   {
        ResponseModel<DynamicListModel>  GetCollectionBySlug(string slug);
        ResponseModel< List<DynamicListCollection>> GetCollectionList();
   }
}
