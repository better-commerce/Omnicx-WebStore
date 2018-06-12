using System.Collections.Generic;
using Omnicx.API.SDK.Models.Catalog;
using Omnicx.API.SDK.Models;
namespace Omnicx.API.SDK.Api.Catalog
{
    public interface ICategoryApi
    {
        ResponseModel<List<CategoryModel>>  GetCategories();
        ResponseModel<CategoryModel>  GetCategory(string slug);
    }
}
