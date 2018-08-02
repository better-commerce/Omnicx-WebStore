using System.Collections.Generic;
using Omnicx.WebStore.Models.Catalog;
using Omnicx.WebStore.Models;
namespace Omnicx.API.SDK.Api.Catalog
{
    public interface ICategoryApi
    {
        ResponseModel<List<CategoryModel>>  GetCategories();
        ResponseModel<CategoryModel>  GetCategory(string slug);
    }
}
