using System.Collections.Generic;
using Omnicx.API.SDK.Models.Catalog;
using Omnicx.API.SDK.Models.Commerce;
using Omnicx.API.SDK.Models.Common;
using System;
using Omnicx.API.SDK.Models;

namespace Omnicx.API.SDK.Api.Commerce
{
    public interface ICustomerApi
    {
        ResponseModel<CustomerModel>  Login(string userName, string password);
        ResponseModel<bool>  NewsletterSubscription(CustomerModel customer);
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
