using System.Collections.Generic;
using Omnicx.WebStore.Models.Catalog;
using Omnicx.WebStore.Models.Commerce;
using Omnicx.WebStore.Models.Common;
using System;
using Omnicx.WebStore.Models;

namespace Omnicx.API.SDK.Api.Commerce
{
    public interface ICustomerApi
    {
        ResponseModel<CustomerModel>  Login(string userName, string password);
        ResponseModel<bool>  NewsletterSubscription(NewsletterModel newsletter);
        ResponseModel<T>  GetUserdetailsById<T>(string userId);
        ResponseModel<List<CustomerModel>>  GetUserdetailsByUserName(string username);
        ResponseModel<BoolResponse>  Register(CustomerModel model);
        ResponseModel<bool>  ChangePassword(string oldPassword, string newPassword, string userId);
        ResponseModel<bool>  UpdateCustomerDetail(string customerId, CustomerModel model);
        ResponseModel<AddressModel>  GetAddressById(string customerId, string id);
        ResponseModel<bool>  DeleteAddress(string customerId, string id);
        ResponseModel<bool> changeDefaultAddress(string customerId, string id);
        ResponseModel<bool>  AddProductToWishList(string id, Guid userId, bool flag);
        ResponseModel<List<ProductModel>>  GetWishlist(string customerId, bool flag);
        ResponseModel<bool>  RemoveWishList(string customerId, string recordId, bool flag);

        ResponseModel<bool>  UnSubscribeNewsletter(string customerId);
        ResponseModel<List<CustomerModel>> GetExistingUser(string email);

        //Address
        ResponseModel<List<T> > GetCustomerAddress<T>(string customerId);
        ResponseModel<bool>  SaveCustomerAddress(AddressModel model);
        ResponseModel<bool>  UpdateCustomerAddress(string customerId, AddressModel model);
        ResponseModel<BoolResponse> NoDefaultAddress(string customerId);
        //Return
        ResponseModel<ReturnModel>  ReturnRequest(string orderId);
        ResponseModel<BoolResponse> CreateReturn(ReturnModel returnModel);

        //Forgot Password
        ResponseModel<BoolResponse>  ForgotPassword(LoginModel user);
        ResponseModel<BoolResponse>  RecoverPassword(UserPasswordTokenModel user);
        ResponseModel<UserActivityListModel> UserActivity(string id, int currentPage, int pageSize);
        ResponseModel<BoolResponse> DeleteActivity(string id);
        ResponseModel<CustomerModel> AuthenticateGhostLogin(string id);

    }
}
