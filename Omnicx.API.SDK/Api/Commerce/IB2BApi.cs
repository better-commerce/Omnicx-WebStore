using Omnicx.WebStore.Models;
using Omnicx.WebStore.Models.B2B;
using Omnicx.WebStore.Models.Commerce;
using System.Collections.Generic;

namespace Omnicx.API.SDK.Api.Commerce
{
    public interface IB2BApi
    {
        ResponseModel<List<QuoteInfoModel>> GetQuotes(string userId);
        ResponseModel<List<CompanyUserModel>> GetUsers(string companyId);
        ResponseModel<bool> UpdateCompanyDetail(CompanyDetailModel model);
        //ResponseModel<T> GetUserdetailsById<T>(string userId);
        ResponseModel<CompanyDetailModel> GetCompanyDetail(string companyId);
        ResponseModel<bool> CreateQuote(QuoteInfoModel quote);
        ResponseModel<BasketModel> GetQuoteDetail(string quoteId);
        ResponseModel<QuoteInfoModel> ValidateQuotePayment(string link);
        ResponseModel<bool> RequestQuoteChange(string userId,string quoteNo);
        ResponseModel<BasketModel> GetQuoteBasket(string id,string action);
        bool RemoveQuoteBasket();
    }
}
