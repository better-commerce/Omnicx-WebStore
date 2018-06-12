using System.Collections.Generic;
using Omnicx.API.SDK.Models.Catalog;
using Omnicx.API.SDK.Models;
namespace Omnicx.API.SDK.Api.Catalog
{
   public interface ICollectionApi
   {
        ResponseModel<DynamicListModel>  GetCollectionBySlug(string slug);
        ResponseModel< List<DynamicListCollection>> GetCollectionList();
   }
}
