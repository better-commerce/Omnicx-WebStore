using System.Collections.Generic;
using Newtonsoft.Json;

using Omnicx.API.SDK.Models.Catalog;
using Omnicx.API.SDK.Models.Commerce;
using Omnicx.API.SDK.Models.Common;
using RestSharp;
using System;
using Omnicx.API.SDK.Models;

namespace Omnicx.API.SDK.Api.Commerce
{
    public class CustomerApi : ApiBase, ICustomerApi
    {
        public ResponseModel<CustomerModel> Login(string userName, string password)
        {
            var model = new LoginModel
            {
                Username = userName,
                Password = password
            };
            return CallApi<CustomerModel>(ApiUrls.Login, JsonConvert.SerializeObject(model), Method.POST);
        }

        public ResponseModel<T> GetUserdetailsById<T>(string userId)
        {
            return CallApi<T>(string.Format(ApiUrls.UserDetailId, userId), "");
        }
        public ResponseModel<List<CustomerModel>> GetUserdetailsByUserName(string username)
        {
            return CallApi<List<CustomerModel>>(ApiUrls.GetUserByName, JsonConvert.SerializeObject(new CustomerBasicModel { Email = username }), Method.POST);
        }
        public ResponseModel<List<CustomerModel>> GetExistingUser(string email)
        {
            return CallApi<List<CustomerModel>>(string.Format(ApiUrls.GetExistingUser,  email),"" , Method.POST);
        }
        public ResponseModel<BoolResponse> Register(CustomerModel model)
        {
            return CallApi<BoolResponse>(ApiUrls.RegisterUser, JsonConvert.SerializeObject(model), Method.POST);
        }

        public ResponseModel<bool> ChangePassword(string oldPassword, string newPassword, string userId)
        {
            var model = new Models.Common.ChangePasswordModel
            {
                OldPassword = oldPassword,
                NewPassword = newPassword,
                UserId = userId
            };
            return CallApi<bool>(string.Format(ApiUrls.ChangePassword, userId), JsonConvert.SerializeObject(model), Method.POST);
        }
        public ResponseModel<bool> UpdateCustomerDetail(string customerId, CustomerModel model)
        {
            return CallApi<bool>(string.Format(ApiUrls.UpdateCustomerDetail, customerId), JsonConvert.SerializeObject(model), Method.POST);
        }
        public ResponseModel<AddressModel> GetAddressById(string customerId, string id)
        {
            return CallApi<AddressModel>(string.Format(ApiUrls.GetAddressById, customerId, id), "");
        }
        public ResponseModel<bool> DeleteAddress(string customerId, string id)
        {
            return CallApi<bool>(string.Format(ApiUrls.DeleteAddress, customerId, id), "", Method.DELETE);
        }
        public ResponseModel<bool> changeDefaultAddress(string customerId, string id)
        {
            return CallApi<bool>(string.Format(ApiUrls.changeDefaultAddress, customerId, id), "", Method.POST);
        }
        public ResponseModel<bool> AddProductToWishList(string id, Guid userId, bool flag)
        {
            return CallApi<bool>(string.Format(ApiUrls.AddToWishlist,userId,id, flag), "", Method.POST);
        }
        public ResponseModel<bool> RemoveWishList(string customerId, string recordId, bool flag)
        {
            return CallApi<bool>(string.Format(ApiUrls.RemoveWishList, customerId, recordId, flag), "", Method.POST);
        }
        public ResponseModel<List<ProductModel>> GetWishlist(string customerId, bool flag)
        {
            return CallApi<List<ProductModel>>(string.Format(ApiUrls.GetWishlist, customerId, flag), "");
        }

        public ResponseModel<bool> NewsletterSubscription(CustomerModel customer)
        {
            return CallApi<bool>(string.Format(ApiUrls.NewsletterSubscription), JsonConvert.SerializeObject(customer), Method.POST);
        }

        public ResponseModel<bool> UnSubscribeNewsletter(string customerId)
        {
            return CallApi<bool>(string.Format(ApiUrls.UnSubscribeNewsletter, customerId), "", Method.POST);
        }

        #region "Address"
        public ResponseModel<List<T>> GetCustomerAddress<T>(string customerId)
        {
            return CallApi<List<T>>(string.Format(ApiUrls.GetAddress, customerId), "");
        }
        public ResponseModel<bool> SaveCustomerAddress(AddressModel model)
        {
            return CallApi<bool>(string.Format(ApiUrls.SaveAddress,model.CustomerId), JsonConvert.SerializeObject(model), Method.POST);
        }
        public ResponseModel<bool> UpdateCustomerAddress(string Id, AddressModel model)
        {
            return CallApi<bool>(string.Format(ApiUrls.UpdateCustomerAddress,model.CustomerId, Id), JsonConvert.SerializeObject(model), Method.POST);
        }
        #endregion

        //Return
        public ResponseModel<ReturnModel> ReturnRequest(string orderId)
        {
            return CallApi<ReturnModel>(string.Format(ApiUrls.ReturnRequest, orderId), "");
        }
        public ResponseModel<BoolResponse> CreateReturn(ReturnModel returnModel)
        {
          return  CallApi<BoolResponse>(string.Format(ApiUrls.CreateReturn), JsonConvert.SerializeObject(returnModel), Method.POST);
                     
        }

        //Forgot Password
        public ResponseModel<BoolResponse> ForgotPassword(LoginModel user)
        {
            return CallApi<BoolResponse>(string.Format(ApiUrls.ForgotPassword, user.Username), "", Method.POST);
        }
        public ResponseModel<BoolResponse> RecoverPassword(UserPasswordTokenModel user)
        {
            return  CallApi<BoolResponse>(string.Format(ApiUrls.ResetPassword), JsonConvert.SerializeObject(user), Method.POST);
        }
        public ResponseModel<UserActivityListModel> UserActivity(string id,int currentPage,int pageSize)
        {
            return CallApi<UserActivityListModel>(string.Format(ApiUrls.UserActivity,id,currentPage,pageSize), "", Method.GET);
        }
        public ResponseModel<BoolResponse> DeleteActivity(string id)
        {
            return CallApi<BoolResponse>(string.Format(ApiUrls.DeleteActivity, id), "", Method.GET);
        }
        public ResponseModel<CustomerModel> AuthenticateGhostLogin(string id)
        {
            return CallApi<CustomerModel>(string.Format(ApiUrls.GhostUserAuth, id), "", Method.POST);
        }
    }
}
