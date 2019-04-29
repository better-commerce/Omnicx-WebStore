using System.Collections.Generic;
using Omnicx.WebStore.Models.Site;
using Omnicx.WebStore.Models;
using Omnicx.WebStore.Models.Common;

namespace Omnicx.API.SDK.Api.Site
{
    public interface IContentApi
    {
        //TODO: Replace MenuModel with Navigation Model - Vikram - 24Apr2017
        ResponseModel<NavigationModel>  GetMenuDetails();
        ResponseModel< List<FaqsCategoryModel>> GetFaqsCategories();
        ResponseModel<List<FaqsSubCategoryModel>>  GetFaqsSubCategories(int faqType);
        ResponseModel<bool> SendContactEmail(ContactModel contactForm);
    }
}