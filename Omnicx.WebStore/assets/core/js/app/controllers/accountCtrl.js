(function () {
    'use strict';
    window.app.controller('accountCtrl', accountCtrl);
    accountCtrl.$inject = ['$scope', 'accountConfig', '$http', '$timeout', 'model', 'CapturePlus','scriptLoader'];

    function accountCtrl($scope, accountConfig, $http, $timeout, model, CapturePlus, scriptLoader) {
        var am = this;
        am.model = model;
        am.saving = false;        
        am.saveCustomerDetail = saveCustomerDetail;
        am.changePassword = changePassword;
        am.getBillingCountries = getBillingCountries;
        am.custAddressGrid = custAddressGrid;
        am.editAddress = editAddress;
        $scope.changepass = false;
        $scope.personalDetail = false;
        am.getOrderHistory = getOrderHistory;
        am.ordersList = [];
        am.addNewAddress = addNewAddress;
        am.saveCustomerAddress = saveCustomerAddress;
        am.deleteAddress = deleteAddress;
        am.resetform = resetform;
        am.createReturn = createReturn;
        am.readyToReturn = false;
        am.setQuantity = setQuantity;
        am.returnProduct = returnProduct;
        am.recoverPassword = recoverPassword;
        am.getMyActivity = getMyActivity;
        am.deleteMyActivity = deleteMyActivity;
        am.orderConfirmation = orderConfirmation;
        am.showSurveyResponse = showSurveyResponse;
        am.reOrder = reOrder;
        am.basketUrl = "/basket/index";
        am.defaultAddress = [];
        am.changeDefaultAddress = changeDefaultAddress;
        am.addressDiv = false;
        am.isPasswordValid = false;
        //subscriptions fields.
        am.getSubscriptionHistory = getSubscriptionHistory;
        am.updateSubscriptionStatus = updateSubscriptionStatus;
        am.calculateRangeForPauseDuration = calculateRangeForPauseDuration;
        

        function initPCALookup() {
            if (accountConfig.pcaAccessCode != undefined && accountConfig.pcaAccessCode != '') {
                window.setTimeout(function () {
                    if (!am.defaultCountry) {
                        $http.post(accountConfig.getDefaultCountryUrl)
                            .success(function (country) { am.defaultCountry = country; });
                    }
                    // address PCA Predict
                    var optionsBilling = {
                        key: accountConfig.pcaAccessCode,
                        countries: {
                            codeList: am.defaultCountry
                        }
                    };
                    var fieldsBilling = [
                        { element: 'am.model.customerAddress.address1', field: 'Line1' },
                        { element: 'am.model.customerAddress.address2', field: 'Line2', mode: pca.fieldMode.POPULATE },
                        { element: 'am.model.customerAddress.city', field: 'City', mode: pca.fieldMode.POPULATE },
                        { element: 'am.model.customerAddress.state', field: 'Province', mode: pca.fieldMode.POPULATE },
                        { element: 'am.model.customerAddress.postCode', field: 'PostalCode' }
                    ];
                    var controlBilling = new pca.Address(fieldsBilling, optionsBilling);
                    controlBilling.listen('options', function (options) {
                        options.countries = options.countries || {};
                        options.countries.codesList = am.defaultCountry 
                    });

                    controlBilling.listen('populate', function (address, variations) {
                        CapturePlusCallback();
                    });

                    controlBilling.load();
                }, 2000);
            }
        }
        function showSurveyResponse(survey) {
            am.survey = survey;
            am.surveyAns = [];
            am.surveyAns.push(JSON.parse(survey.answers));
            console.log(am.surveyAns);
            $('#showeSurvey').modal();
        }
        function changeDefaultAddress(defaultAddress) {
            $http.post(accountConfig.changeDefaultAddressUrl, defaultAddress)
               .success(function (data) {
                   $(".addressDiv").fadeIn();
                   am.addressDiv = true;
                   $timeout(function () {
                       $(".addressDiv").fadeOut();
                       am.addressDiv = false;
                   }, 2000);
                   am.custAddressGrid();                   
               })
               .error(function (msg) {
                   //vm.errorMessage = msg.errorMessages;
               })
               .finally(function () {
                   // vm.saving = false;
                   // $("html, body").animate({ scrollTop: 0 }, "slow");
               });
        };
        function saveCustomerDetail(model) {
            $scope.changepass = false;
            $scope.personalDetail = true;
            am.saving = true;
            am.errorMessage = null;
            am.success = false;
            $(".alertBlock").fadeIn();

            $http.post(accountConfig.saveCustomerUrl,model)
               .success(function () {
                   am.success = true;
                   window.location.reload();
                   // custAddressGrid();
               })
               .error(function (msg) {
                   am.errorMessage = msg.errorMessages;
               })
               .finally(function () {
                   am.saving = false;
                   $timeout(function () { $(".alertBlock").fadeOut(); }, 3000);
                });
            var newsletterModel = { Email: am.model.customerDetail.email, notifyEmail: am.model.customerDetail.notifyByEmail, notifySMS: am.model.customerDetail.notifyBySMS, notifyPost: am.model.customerDetail.notifyByPost };
            $http.post(accountConfig.newsletter, newsletterModel).then(function (success) { }, function (error) { });
        };
        
        function changePassword(model) {
            $scope.changepass = true;
            $scope.personalDetail = false;
            am.saving = true;
            am.errorMessage = null;
            am.success = false;
            $(".alertBlock").fadeIn();
            if (!am.isPasswordValid) {
                $scope.changepass = false;
                return false;
            }
            $http.post(accountConfig.passwordChangeUrl,model)
               .success(function () {
                   am.success = true;
                   am.model.changePasswordViewModel = {};
                   $scope.changePasswordForm.$setPristine();
                   $scope.changePasswordForm.$setUntouched();
               })
               .error(function (msg) {
                   am.errorMessage = msg.errorMessages;
               })
               .finally(function () {
                   am.saving = false;
                   $timeout(function () { $(".alertBlock").fadeOut(); }, 3000);
               });
        };

        function getBillingCountries() {
            $http.post(accountConfig.getBillingCountriesUrl)
               .success(function (data) {
                   if (data.length > 0) {
                       am.model.customerAddress.billingCountries = data;
                   }
               })
               .error(function (msg) {

               })
               .finally(function () {

               });
        }
        function custAddressGrid() {
            am.showGrid = true;
            am.showAddressGrid = false;
            $http.post(accountConfig.custGridUrl)
               .success(function (data) {
                   if (data.length > 0) {
                       am.model = data;
                       $scope.addressTable = true;
                   }
                   else{
                       $scope.addressTable = false;
                   }
                   if (accountConfig.pcaAccessCode != undefined && accountConfig.pcaAccessCode != '') {
                       scriptLoader.load("//services.postcodeanywhere.co.uk/css/captureplus-2.30.min.css?key=" + accountConfig.pcaAccessCode, "text/css", "stylesheet");
                       scriptLoader.load("//services.postcodeanywhere.co.uk/js/captureplus-2.30.min.js?key=" + accountConfig.pcaAccessCode, "text/javascript", "");
                   }                  
               })
               .error(function (msg) {
                   am.errorMessage = msg.errorMessages;
               })
               .finally(function () {
                   $timeout(function () { $(".alertBlock").fadeOut(); }, 3000);
               });
        };
        function getOrderHistory() {
            $(".alertBlock").fadeOut();
            $http.post(accountConfig.getOrderHistoryUrl)
               .success(function (data) {
                   am.ordersList = data;
               })
               .error(function (msg) {
               })
               .finally(function () {
               });
        };
        function editAddress(model) {
            am.saving = false;
            am.errorMessage = null;
            am.showGrid = false;
            am.showAddressGrid = true;
            $http.post(accountConfig.addByIdUrl, model)
               .success(function (data) {
                   am.model.customerAddress = data;
                   am.getBillingCountries();
                   initPCALookup();
               })
               .error(function (msg) {
                   am.errorMessage = msg.errorMessages;
               })
               .finally(function () {
                   am.saving = false;
                   $("html, body").animate({ scrollTop: 0 }, "slow");
               });
        };

        function addNewAddress() {
            $("html, body").animate({ scrollTop: 0 }, "slow");
            am.saving = false;
            am.errorMessage = null;
            am.success = false;
            am.showGrid = false;
            am.showAddressGrid = true;
            if (am.model != undefined)
                am.model.customerAddress = {};
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            am.model.customerAddress.countryCode = "GB";
            $timeout(function () { $(".alertBlock").fadeOut(); }, 3000);
            am.getBillingCountries();
            initPCALookup();
        };

        function saveCustomerAddress(model) {
            $scope.myForm.$setSubmitted();
            am.saving = true;
            am.errorMessage = null;
            am.success = false;
            am.model.customerAddress.country = am.model.customerAddress.countryCode;
            $(".alertBlock").fadeIn();

            $http.post(accountConfig.saveCustAddrUrl, am.model.customerAddress)
               .success(function () {
                   am.success = true;
                   am.custAddressGrid();
               })
               .error(function (msg) {
                   am.errorMessage = msg.errorMessages;
               })
               .finally(function () {
                   am.saving = false;
                   $("html, body").animate({ scrollTop: 0 }, "slow");
                   $timeout(function () { $(".alertBlock").fadeOut(); }, 3000);
               });
        };

        function deleteAddress(model) {
            $http.post(accountConfig.deleteAddressUrl, model)
               .success(function (data) {
                   am.custAddressGrid();
               })
               .error(function (msg) {
                   //vm.errorMessage = msg.errorMessages;
               })
               .finally(function () {
                   // vm.saving = false;
                   // $("html, body").animate({ scrollTop: 0 }, "slow");
               });
        };

        function resetform() {
            if (am.model!=undefined)
            am.model.customerAddress = {};
            am.showAddressGrid = false;
            am.showGrid = true
        };

        function createReturn(model) {
            am.readyToReturn = true;
            for (var i = 0; i < model.lineItems.length; i++)
            {
                if (model.lineItems[i].availableQty > 0)
                {
                    if (model.lineItems[i].returnQtyRecd > 0) {
                        am.readyToReturn = false;
                    }
                }               
            }
            if (am.readyToReturn) {
                $("html, body").animate({ scrollTop: 0 }, "slow");
                am.readyToReturn = true;
                return;
            }
            if (model.reasonForReturnId == null || model.reasonForReturnId == "") {
                $scope.reasonForReturnId = true;
                am.errorMessage = true;
                return;
            }
            $scope.reasonForReturnId = false;
            if (model.requiredActionId == null || model.requiredActionId == "") {
                $scope.requiredActionId = true;
                am.errorMessage = true;
                return;
            }
            $scope.requiredActionId = false;
            if (model.comment == null || model.comment == "")
            {
                $scope.comment = true;
                am.errorMessage = true;
                return;
            }
            $scope.comment = false;
            am.errorMessage = false;
            $http.post(accountConfig.createReturnUrl, model)
              .success(function (data) {
                  $("html, body").animate({ scrollTop: 0 }, "slow");
                  am.success = true;
                  $timeout(function () {
                      am.success = false;
                  }, 2000);
                  $timeout(function () {
                      window.location.href = "/account/returnhistory";
                  }, 3000);                 
              })
              .error(function (msg) {
                  //vm.errorMessage = msg.errorMessages;
              })
              .finally(function () {
                  // vm.saving = false;
                  // $("html, body").animate({ scrollTop: 0 }, "slow");
              });            
        };

        function setQuantity(value) {
            am.Quantity = [];
            for (var i = 0; i <= value; i++) {
                am.Quantity.push(i);
            }
            return am.Quantity;
        };

        function returnProduct(productId,qty) {
            for (var i = 0; i < model.lineItems.length; i++) {
                if (model.lineItems[i].productId == productId) {
                    model.lineItems[i].returnQtyRecd = qty;
                }
            }
        };
        function recoverPassword(model) {
            $http.post(accountConfig.recoverPassword, model)
               .success(function (data) {
                   am.errorMessage = null;
                   if(data.isValid)
                       am.passwordChange = true;
                   else
                       am.tokeExpired = true;
                   $timeout(function () {
                       window.location.href = accountConfig.returnUrl;
                   }, 3000);
               })
               .error(function (msg) {
                   am.tokeExpired = false;
                   am.errorMessage = msg.errorMessages;
               })
               .finally(function () {
                   // vm.saving = false;
                   // $("html, body").animate({ scrollTop: 0 }, "slow");
               });
        };


        function getMyActivity(search) {
            $scope.changepass = false;
            $scope.personalDetail = true;
            am.saving = true;
            am.errorMessage = null;
            am.success = false;
            $(".alertBlock").fadeIn();

            $http.post(accountConfig.getMyActivity, search)
               .success(function (resp) {
                   console.log(resp);
                   am.activityList = resp;
               })
               .error(function (msg) {
                 
               })
               .finally(function () {
                  
               });
        };
        function deleteMyActivity() {
            $scope.changepass = false;
            $scope.personalDetail = true;
            am.saving = true;
            am.errorMessage = null;
            am.success = false;
            $(".alertBlock").fadeIn();

            $http.post(accountConfig.deleteMyActivity)
                .success(function (resp) {
                    console.log(resp);
                    am.activityList = null;
                })
                .error(function (msg) {

                })
                .finally(function () {

                });
        };
        function orderConfirmation(myForm) {
            document.myForm.submit();
        };

        function reOrder(orderId) {
            $http.post(accountConfig.reOrderUrl, { id: orderId })
                .success(function (data) {                   
                    if (data.messageCode == "QP01") {
                        am.orderUnavailable = true;
                        $('.orderUnavailable').show(0).delay(3000).hide(0);
                        return;
                    }
                    if (data.messageCode == "QP02") {
                        am.orderError = data.message;
                        $('.orderError').show(0).delay(3000).hide(0);
                    }
                    if (data.messageCode == "C001") {
                        am.orderSuccess = true
                        $('.orderSuccess').show(0).delay(3000).hide(0);
                    }
                    $timeout(function () { window.location.href = am.basketUrl; }, 3000);
                });
        };

        //get list of subscription for a customer.
        function getSubscriptionHistory() {
            $http.post(accountConfig.getSubscriptionHistoryUrl).success(function (resp) {
                am.subscriptionList = resp.result;
            }).error(function (resp) { }).finally(function (resp) { });
        }
        function updateSubscriptionStatus(seedOrderstatus) {
            var model = {
                seedOrderId: am.subscriptionDetail.Id,
                pauseDuration: am.subscriptionDetail.PauseDuration,
                autoRenew: am.subscriptionDetail.AutoRenewal
            }
            if (seedOrderstatus != null || seedOrderstatus != undefined) {
                model.status = seedOrderstatus
            }
            else {
                model.status = am.subscriptionDetail.Status
            }
            $http.post(accountConfig.updateSubscriptionStatus,{subscriptionUpdateStatus: model}).success(function (resp) {
                window.location.reload();
            }).error(function (resp) { }).finally(function (resp) { });
        };
        function calculateRangeForPauseDuration() {
            am.rangeOfPauseDuration = [];
            for (i = 1; i <= am.model.seedOrderDetail.maxPauseUnit; i++) {
                am.rangeOfPauseDuration.push(i);
            }
        };
       
    }
})();