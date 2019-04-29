(function () {
    'use strict';
    
    // ADD CONSTANT FOR THEME DEFAULT IMAGE
    window.DEFAULT_IMAGE_URL = '/assets/theme/ocx/images/noimagefound.jpg';
    window.app.controller('checkoutCtrl', checkoutCtrl);
    
    checkoutCtrl.$inject = ['$scope', 'checkoutConfig', 'globalConfig', '$http', 'model', '$timeout', 'loader', 'CapturePlus', 'SUBSCRIPTION_ENUMS', 'SUBSCRIPTION_CONSTANTS'];
    
    
    function checkoutCtrl($scope, checkoutConfig, globalConfig, $http, model, $timeout, loader, CapturePlus, SUBSCRIPTION_ENUMS, SUBSCRIPTION_CONSTANTS) {
        var ck = this;
        ck.model = model;
        ck.sameAsBillAddress = true;
        ck.login = login;
        ck.register = register;
        ck.continueToPayment = continueToPayment;
        ck.payment = payment;
        ck.ContinueAsGuest = ContinueAsGuest;
        ck.basket = model.checkout.basket;
        ck.paymentMethod = paymentMethod;
        ck.setShipping = setShipping;
        ck.placeOrder = placeOrder;
        ck.continuePlaceOrder = continuePlaceOrder;
        ck.custAddressGrid = custAddressGrid;
        ck.setShipAddress = setShipAddress;
        ck.continueToDelivery = continueToDelivery;
        ck.showDeliveryOption = false;
        ck.viewOrderDetail = viewOrderDetail;
        ck.ValidateDeliveryMethod = ValidateDeliveryMethod;
        ck.deleteAddress = deleteAddress;
        ck.changeAddress = changeAddress;
        ck.initMethod = initMethod;
        ck.continueToPayment_2 = continueToPayment_2;
        var temp = 0;
        ck.registration = registration;
        ck.addToBasket = addToBasket;
        ck.updateQtyAndAdd = updateQtyAndAdd;
        ck.setBalanceAmt = setBalanceAmt;
        ck.applyPromoCode = applyPromoCode;
        ck.removePromoCode = removePromoCode;
        ck.searchPhysicalStore = searchPhysicalStore;
        ck.getNominatedDelivery = getNominatedDelivery;
        ck.currentDate = ck.model.currentDate;
        ck.setStoreAddress = setStoreAddress;
        ck.selectNominatedDelivery = selectNominatedDelivery;
        ck.clickCollect = clickCollect;
        ck.logout = logout;
        ck.updateNominatedShipping = updateNominatedShipping;
        ck.validateGuestPassword = validateGuestPassword;
        ck.setPassword = setPassword;
        var startDate = "";
        ck.purchaseFor = purchaseFor;
        ck.model.checkout.giftOrMe = 0;
        ck.addProductToWishlist = addProductToWishlist;
        ck.removeProductToWishlist = removeProductToWishlist;
        ck.validateWishlist = validateWishlist;
        ck.validateLoginPassword = validateLoginPassword;
        ck.emptyGuid = '00000000-0000-0000-0000-000000000000';
        ck.addPersistentBasket = addPersistentBasket;
        ck.oldBasketPopup = oldBasketPopup;
        ck.createQuote = createQuote;
        ck.UserSelectedShippingEvent = [];
        ck.custInfoCheck = custInfoCheck;
        ck.isGuestUser = isGuestUser;
        ck.baskets = [];
        ck.getShippingMethod = getShippingMethod;
        ck.defaultShipping;
        ck.onTextFocus = onTextFocus;
        ck.alreadyRegistered = true;
        ck.checkPassword = checkPassword;
        ck.showShippingGrid = showShippingGrid;
        ck.wrongPostCode = false;
        ck.cookiepostCode = 'POSTCODE';
        ck.confirmPostCodeChange = confirmPostCodeChange;
        ck.isPostCodeDiff = false;
        ck.continue = false;
        ck.orderResp = '';
        ck.basketerror = null;
        ck.continueToSummery = continueToSummery;
        ck.getCurrentBasketData = getCurrentBasketData;
        //ck.getBasketSubcriptionSettings = getBasketSubcriptionSettings;
        ck.userSubscriptionSettings = {};
        ck.paymentLinkSend = false;
        //Subscription 
        ck.subscriptionModel = {};
        ck.initSubscriptionPlan = initSubscriptionPlan;



        //***********TEMPORARY METHOD FOR POWDER COATING **********************
        ck.reCalculateServiceCharge = reCalculateServiceCharge;
        ck.serializedData = serializedData;


        if (globalConfig.pcaAccessCode != undefined && globalConfig.pcaAccessCode != '') {
            window.setTimeout(function () {
                // Shipping address PCA Predict
                var optionsShip = {
                    key: globalConfig.pcaAccessCode,
                    countries: {
                        codesList: document.getElementById('ck.model.checkout.shippingAddress.countryCode').value
                    }
                };

                var fieldsShip = [
                    { element: 'ck.model.checkout.shippingAddress.address1', field: 'Line1' },
                    { element: 'ck.model.checkout.shippingAddress.address2', field: 'Line2', mode: pca.fieldMode.POPULATE },
                    { element: 'ck.model.checkout.shippingAddress.city', field: 'City', mode: pca.fieldMode.POPULATE },
                    { element: 'ck.model.checkout.shippingAddress.state', field: 'Province', mode: pca.fieldMode.POPULATE },
                    { element: 'ck.model.checkout.shippingAddress.postCode', field: 'PostalCode' }
                ];

                var controlShip = new pca.Address(fieldsShip, optionsShip);

                controlShip.listen('options', function (options) {
                    options.countries = options.countries || {};
                    options.countries.codesList = document.getElementById('ck.model.checkout.shippingAddress.countryCode').value;
                });

                controlShip.listen('populate', function (address, variations) {
                    CapturePlusCallback();
                });

                controlShip.load();

                // Billing address PCA Predict
                var optionsBilling = {
                    key: globalConfig.pcaAccessCode,
                    countries: {
                        codesList: document.getElementById('ck.model.checkout.billingAddress.countryCode').value
                    }
                };

                var fieldsBilling = [
                    { element: 'ck.model.checkout.billingAddress.address1', field: 'Line1' },
                    { element: 'ck.model.checkout.billingAddress.address2', field: 'Line2', mode: pca.fieldMode.POPULATE },
                    { element: 'ck.model.checkout.billingAddress.city', field: 'City', mode: pca.fieldMode.POPULATE },
                    { element: 'ck.model.checkout.billingAddress.state', field: 'Province', mode: pca.fieldMode.POPULATE },
                    { element: 'ck.model.checkout.billingAddress.postCode', field: 'PostalCode' }
                ];

                var controlBilling = new pca.Address(fieldsBilling, optionsBilling);

                controlBilling.listen('options', function (options) {
                    options.countries = options.countries || {};
                    options.countries.codesList = document.getElementById('ck.model.checkout.billingAddress.countryCode').value;
                });

                controlBilling.listen('populate', function (address, variations) {
                    CapturePlusCallback();
                });

                controlBilling.load();
            }, 5000);
        }

        function isGuestUser() {
            ck.isGuest = false;
            $("#userLogin").removeAttr('checked');
            $("#addressPanel").removeAttr('checked');
            $("#paymentPanel").removeAttr('checked');
            $("#addressPanel").prop('disabled', true);
            $("#productSummery").prop('disabled', true);
            $("#paymentPanel").prop('disabled', true);
        }

        function getShippingMethod(shippingMethod, selectedShippingId) {
            //  ck.defaultShipping = shippingMethod.id;
            if (!(!shippingMethod) && shippingMethod.id == selectedShippingId) {
                ck.selectedDelivery = shippingMethod;
            }
        }

        function custInfoCheck(form) {
            if (ck.model.checkout.customerId != null && ck.model.checkout.email != null) {
                ck.guestError = null;
                ck.GuestEmail = ck.model.checkout.email;
                ck.isGuest = true;
                ck.userPanel = false;
                ck.guestCheckout = false;
                ck.loginAccount = false;
                ck.createAccount = false;
                ck.deliverAddress = true;
                ck.updateEmail = true;
            }
            angular.forEach(ck.basket.lineItems, function (line) {
                if (line.isSubscription) {
                    ck.basketContainsSubscription = true;
                }
            });
        }

        function ContinueAsGuest(form) {
            if (!form.$invalid) {
                ck.model.checkout.customerId = null;
                $http.post(checkoutConfig.guestCheckout, {
                    email: ck.model.checkout.email,
                    basketId: ck.model.checkout.basketId,
                    customerId: ck.model.checkout.customerId
                })
                    .success(function (data) {
                        if (data.customerId) {
                            ck.guestError = null;
                            ck.GuestEmail = ck.model.checkout.email;
                            ck.model.checkout.companyId = data.companyId;
                            if (data.customerId != true) { ck.model.checkout.customerId = data.customerId; }
                            ck.model.checkout.stage = data.basketStage;
                            ck.isGuest = true;
                            var dataResult = ck.serializedData(data.basket);
                            ck.basket = dataResult;
                            ck.userPanel = false;
                            ck.guestCheckout = false;
                            ck.loginAccount = false;
                            ck.createAccount = false;
                            ck.deliverAddress = true;
                            ck.updateEmail = true;
                            initMethod();
                        }
                    })
                    .error(function (msg) {
                        ck.guestError = msg.errorMessages;
                    })
                    .finally(function () {
                        $timeout(function () { $(".alertBlock").fadeOut(); }, 3000);
                    });
                var newsletterModel = { Email: ck.model.checkout.email, notifyEmail: ck.model.checkout.notifyEmail, notifySMS: ck.model.checkout.notifySMS, notifyPost: ck.model.checkout.notifyPost };
                $http.post(checkoutConfig.newsletter, newsletterModel).then(function (success) { }, function (error) { });
            }
        }

        function login(loginModel) {

            ck.saving = false;
            ck.errorMessage = null;
            ck.success = false;
            $(".alertBlock").fadeIn();
            $http.post(checkoutConfig.signIn, loginModel)
                .success(function (data) {
                    if (data) {
                        ck.isGuest = false;
                        window.location.reload();
                        $("#paymentPanel").prop('checked', true);
                    }
                })
                .error(function (msg) {
                    ck.errorMessage = msg.errorMessages;
                    $timeout(function () {
                        ck.errorMessage = null;
                        $(".alertBlock").fadeOut();
                    }, 3000);

                })
                .finally(function () {
                    ck.saving = false;
                });
        };

        function setShipping(shipMethod) {
            //Add user selected shippings to UserSelectedShippingEvent . This Array maintains only one event for one country
            if (shipMethod == undefined || shipMethod == null) {
                var index = ck.basket.shippingMethods.findIndex(x => x.id == ck.defaultShipping)
                if (index > 0) {
                    shipMethod = ck.basket.shippingMethods[index];
                }
            }
            if (ck.UserSelectedShippingEvent.length == 0) {
                ck.UserSelectedShippingEvent.push(shipMethod);
            } else {
                ck.pushMethod = true;
                angular.forEach(ck.shipMethod, function (obj) {
                    if (obj.countryCode == shipMethod.countryCode && ck.pushMethod) {
                        var index = ck.UserSelectedShippingEvent.indexOf(obj);
                        ck.UserSelectedShippingEvent.splice(index, 1);
                        ck.UserSelectedShippingEvent.push(shipMethod);
                        ck.pushMethod = false;
                    }
                });
                if (ck.pushMethod) { ck.UserSelectedShippingEvent.push(shipMethod); }
            }

            temp = 1;
            ck.errors = false;
            ck.viewMoreStore = true;
            ck.showDates = false;
            ck.hideShippingAddress = false;
            ck.isClickAndCollect = false;
            ck.shippingSelected = true;
            ck.sameAsBillAddress = true;
            if (shipMethod != null) {
                if (shipMethod.type == checkoutConfig.shipClickAndCollect) {
                    ck.hideShippingAddress = true;
                    ck.isClickAndCollect = true;
                    ck.sameAsBillAddress = false;
                }

                ck.model.checkout.selectedShipping = shipMethod;
                if (shipMethod.isNominated == true) {
                    ck.currentDate = ck.model.currentDate;

                    $http.post(checkoutConfig.nominatedDelivery, { startDate: ck.currentDate, shipMethod: shipMethod }).success(function (dates) {
                        ck.nominatedDates = dates;
                        startDate = ck.nominatedDates[0].deliveryDate;
                        updateNominatedShipping(ck.basket.id, shipMethod.id, ck.nominatedDates[0]);

                        ck.showDates = true;
                    });
                }
                else {
                    var requestSource = window.location.pathname.substr(window.location.pathname.indexOf('/') + 1, window.location.pathname.lastIndexOf('/') - 1);
                    $http.post(globalConfig.updateShipping, { id: ck.basket.id, shippingId: shipMethod.id, nominatedDelivery: null, requestSource: requestSource.toLowerCase() })
                        .success(function (data) {
                            var dataResult = ck.serializedData(data.basket);
                            ck.basket = dataResult;
                            ck.setBalanceAmt();
                            ck.model.checkout.stage = data.basketStage;
                            ck.initMethod();
                        })
                        .error(function (msg) {
                            // vm.errorMessage = msg.errorMessages;
                        })
                        .finally(function () {

                        });
                }
            }
        };

        function continueToPayment(deliveryMethod) {

            if (temp == 0) {
                ck.errors = true;
                return;
            }
            ck.errors = false;
            if (deliveryMethod != null) {
                if (deliveryMethod.displayName != 'Click and Collect') {
                    ck.model.checkout.selectedShipping = deliveryMethod;
                } else {
                    ck.hideBillingAddress = false;
                }
            }
            ck.model.checkout.billingAddress = ck.model.checkout.shippingAddress;
            if (ck.isComplete == 1) {
                ck.showDeliveryOption = false;
                ck.orderDetail = true;
            }
            else {
                ck.showPaymentOption = true;
                ck.showDeliveryOption = false;
            }
        };

        function payment(paymentMethod) {
            if (paymentMethod.isBillingAddressRequired) {
                ck.hideBillingAddress = true;
            }
            else {
                ck.hideBillingAddress = false;
            }
            $scope.masterCard = false;
            $scope.givex = false;
            ck.model.checkout.selectedPayment = paymentMethod;
            ck.hideBillingAddress = paymentMethod.isBillingAddressRequired;
            ck.billingAddress = paymentMethod.isBillingAddressRequired;
            if (paymentMethod.displayName == 'MasterCard')
                $scope.masterCard = true;
            if (paymentMethod.displayName == 'Givex') $scope.givex = true;
        };



        function paymentMethod() {
            ck.model.checkout.storeAddress = '';
            if (ck.model.checkout.selectedShipping.type == checkoutConfig.shipClickAndCollect) {
                ck.model.checkout.storeAddress = ck.model.storeAddress;
                ck.model.checkout.storeAddress.externalRefId = ck.model.storeAddress.yourId;
                ck.model.checkout.shippingAddress = {};
            }
            if (ck.model.checkout.selectedShipping.displayName == 'Next Day Delivery') {
                ck.model.checkout.selectedShipping.nominatedDeliveryDate = ck.model.checkout.nominatedDeliveryDate;
            }

            var checkout = {
                basketId: ck.basket.id, billingAddress: ck.model.checkout.billingAddress, shippingAddress: ck.model.checkout.shippingAddress,
                customerId: ck.model.checkout.customerId, email: ck.model.checkout.email, selectedPayment: ck.model.checkout.selectedPayment,
                selectedShipping: ck.model.checkout.selectedShipping, storeAddress: ck.model.checkout.storeAddress, password: ck.model.checkout.password,
                giftOrMe: ck.model.checkout.giftOrMe, companyId: ck.model.checkout.companyId
            }
            if (ck.model.checkout.companyId != null && ck.model.checkout.companyId != ck.emptyGuid && ck.isGuest) {
                ck.error = true;
                ck.customErrorMessage = "Guest Checkout is not available for Company Account, Please Login to Continue.";
                $('.alertBlock').show(0).delay(6000).hide(0);
            }
            else {
                ck.partialAmount = ck.model.checkout.selectedPayment.cardInfo.amount;
                $http.post(checkoutConfig.converToOrder, checkout)
                    .success(function (data) {
                        if (data.success) {
                            if (data.balanceAmount != null) {
                                if (data.balanceAmount.raw.withTax > 0) {
                                    ck.model.checkout.balanceAmount = data.balanceAmount;
                                    ck.model.checkout.paidAmount = data.paidAmount;
                                    angular.forEach(ck.model.checkout.paymentOptions, function (pay) {
                                        pay.cardInfo.amount = data.balanceAmount.raw.withTax;
                                    });
                                    ck.model.checkout.selectedPayment.cardInfo = null;
                                    ck.showRemainingAmount = true;
                                    ck.messageSuccess = true;
                                    $('.successBlock').show(0).delay(8000).hide(0);
                                    return;
                                }
                            }
                            if (data.useAuthUrlToRedirect) {
                                window.location.href = data.authorizationTransactionUrl;
                            } else if (data.usePostForm) {
                                $(data.postForm).appendTo('body').submit();
                            } else if (ck.model.checkout.selectedPayment.systemName == checkoutConfig.klarna) {
                                ck.orderResp = data;
                                KlarnaPaymentInit(data.authorizationTransactionCode);
                                ck.continue = true;
                            } else {
                                masterCardPay(data.refOrderId, ck.basket.id, data.orderId, data.currencyCode, data.timeStamp, data.orderTotal, ck.model.checkout.selectedPayment.notificationUrl);
                            }

                        } else {
                            ck.errorMessage = true;
                            if (data.errors != undefined) {
                                ck.errorMessage = data.errors[0];
                            }
                            $('.alertBlock').show(0).delay(6000).hide(0);;
                        }

                    })
                    .error(function (msg) {
                        ck.errorMessage = msg.errorMessages;
                        $timeout(function () {
                            $(".alertBlock").fadeOut();
                        }, 3000);

                    })
                    .finally(function () {
                        ck.saving = false;
                    });
            }


        };


        function placeOrder(selectedPayment) {
            if (ck.isPostCodeDiff && ck.model.checkout.shippingAddress.postCode.replace(" ", "").toLowerCase() != ck.model.checkout.basket.postCode.replace(" ", "").toLowerCase()) {
                $("#postCodeAlert-modal").modal();
                return;
            }
            if (ck.isGuest == undefined) {
                $("html, body").animate({ scrollTop: 0 }, "slow");
            }
            else {
                if (ck.model.checkout.selectedPayment == null) {
                    ck.errors = true;
                }
                else {
                    ck.errors = false;
                    if (ck.model.checkout.selectedPayment.isBillingAddressRequired) {
                        if ($scope.billingForm != null) {
                            if ($scope.billingForm.$error.required != null) {
                                $scope.billingForm.$setSubmitted();
                                return;
                            }
                        }
                    }
                    ck.paymentMethod();
                }
            }
        };

        function continuePlaceOrder(selectedPayment) {
            if (ck.isPostCodeDiff && ck.model.checkout.shippingAddress.postCode.replace(" ", "").toLowerCase() != ck.model.checkout.basket.postCode.replace(" ", "").toLowerCase()) {
                $("#postCodeAlert-modal").modal();
                return;
            }
            if (ck.isGuest == undefined) {
                $("html, body").animate({ scrollTop: 0 }, "slow");
            } else {
                if (ck.model.checkout.selectedPayment == null) {
                    ck.errors = true;
                } else {
                    KlarnaAuthorize(ck.model.checkout, ck.orderResp);
                }
            }
        };

        function oldBasketPopup() {
            $http.post(checkoutConfig.savedBaskets).success(function (data) {
                if (data != null && data.result != null) {
                    ck.baskets = data.result;
                    //checks to prevent persistent basket popup
                    //condition 1: User does not have persistent basket.
                    //condition 2: User clicked through my saved basket, this is the only persistent basket.  
                    var index = ck.baskets.findIndex(x => x.id == ck.basket.id);
                    if (index >= 0) { ck.baskets.splice(index, 1); }
                    if (ck.baskets != null && ck.baskets != undefined && ck.baskets.length > 0) {
                        $("#basketAlert-modal").modal();
                    }
                }
            }).error(function () { })
        }

        function custAddressGrid() {

            if (ck.model.checkout.selectedShipping != null) {
                ck.model.checkout.shippingAddress.countryCode = ck.model.checkout.selectedShipping.countryCode;
            }

            ck.check = true;
            ck.orderDetail = false;
            ck.showDeliveryOption = false;
            $http.post(checkoutConfig.custGridUrl)
                .success(function (data) {
                    if (data != null && data.length > 0) {
                        ck.userAddresses = data;
                        //if basket is not a quote then continue doing same else ck.model.checkout.shippingAddress,BillingAddress remains what it comes
                        if (!ck.model.checkout.basket.isQuote) {
                            var index = data.findIndex(d => d.isDefault === true);
                            if (index >= 0) {
                                ck.model.checkout.shippingAddress = data[index];
                                //  ck.model.checkout.shippingAddress.postCode = ck.postCode;
                                if (ck.sameAsBillAddress)
                                    ck.model.checkout.billingAddress = ck.model.checkout.shippingAddress;
                            }
                            else {
                                if (ck.sameAsBillAddress)
                                {
                                    ck.model.checkout.billingAddress = ck.model.checkout.shippingAddress;
                                }
                                
                            }
                        } else {
                            ck.sameAsBillAddress = false;
                        }
                    }
                })
                .error(function (msg) {
                })
                .finally(function () {
                    $timeout(function () { $(".alertBlock").fadeOut(); }, 3000);
                });
        };


        function setShipAddress(value) {
            if (value) {
                ck.model.checkout.billingAddress = ck.model.checkout.shippingAddress;
            }
            else {
                ck.model.checkout.billingAddress = {
                    country: ck.model.checkout.billingAddress.country, countryCode: ck.model.checkout.billingAddress.countryCode, title: null
                };
            }
        }

        function continueToDelivery(address) {
            if (address.id != null) {
                ck.model.checkout.shippingAddress = address;
            }
            else {
                if ($scope.shippingForm != null) {
                    if ($scope.shippingForm.$invalid) {
                        return;
                    }
                }
            }
            if (ck.isComplete == 1) {
                ck.deliverAddress = false;
                ck.orderDetail = true;
            }
            else {
                ck.deliverAddress = false;
                ck.showDeliveryOption = true;
            }

        };

        function viewOrderDetail(value) {
            if (ck.model.checkout.selectedPayment == null) {
                ck.errors = true;
                return;
            }
            ck.errors = false;
            if (value) {
                if ($scope.billingForm != null) {
                    if ($scope.billingForm.$error.required != null) {
                        $scope.billingForm.$setSubmitted();
                        return;
                    }
                }
            }
            if (ck.isComplete == 1) {
                ck.showPaymentOption = false;
                ck.orderDetail = true;
            }
            else {
                ck.showPaymentOption = false;
                ck.orderDetail = true;
            }

        };

        function ValidateDeliveryMethod() {
            temp = 0;
            for (var i = 0; i < ck.basket.shippingMethods.length; i++) {
                if (ck.basket.shippingMethods[i].countryCode == ck.model.checkout.shippingAddress.countryCode) {
                    if (ck.basket.shippingMethods[i].id == ck.model.checkout.selectedShipping.id) {
                        ck.model.checkout.selectedShipping = ck.basket.shippingMethods[i];
                        temp = 1;
                    }
                }
            }
            //this code selects shipping method from user clicked events or select by default first method
            if (ck.model.checkout.selectedShipping.countryCode == ck.model.checkout.shippingAddress.countryCode) {
                ck.setShipping(ck.model.checkout.selectedShipping);
            } else {
                ck.getSelectedEvent = true;
                //checks from clicked events first , if found then set the found event as selected shipping and dont go to else loop
                angular.forEach(ck.UserSelectedShippingEvent, function (obj) {
                    if (obj.countryCode == ck.model.checkout.shippingAddress.countryCode && ck.getSelectedEvent) {
                        ck.selectedDelivery = obj;
                        ck.model.checkout.selectedShipping = obj;
                        ck.getSelectedEvent = false;
                        ck.setShipping(obj);
                    }
                });
                if (ck.getSelectedEvent) {
                    ck.setMethod = true;
                    //if no click events found of a user for a countryCode , then set first shipping method as selected
                    angular.forEach(ck.basket.shippingMethods, function (obj) {
                        if (obj.countryCode == ck.model.checkout.shippingAddress.countryCode && ck.setMethod) {
                            ck.model.checkout.selectedShipping = obj;
                            ck.setShipping(obj);
                            ck.setMethod = false;
                        }
                    });
                }
            }
        };

        function deleteAddress(model) {
            $http.post(checkoutConfig.deleteAddressUrl, model)
                .success(function (data) {
                    ck.custAddressGrid();
                })
                .error(function (msg) {
                })
                .finally(function () {
                });
        };
        function confirmPostCodeChange() {
            $("#userLogin").prop('checked', true);
            $("#paymentPanel").prop('checked', false);
            $("#addressPanel").prop('checked', false);
            $("#productSummery").prop('checked', false);
            ck.postCode = ck.model.checkout.shippingAddress.postCode;
            ck.showShippingGrid(ck.model.checkout.shippingAddress.countryCode, ck.model.checkout.basket.id, ck.postCode, ck.model.checkout.basket.shippingMethodId);
        }
        function continueToSummery(shippingForm, billingForm) {
            if (!ck.isClickAndCollect) {
                if (ck.isPostCodeDiff && ck.model.checkout.shippingAddress.postCode.replace(" ", "").toLowerCase() != ck.model.checkout.basket.postCode.replace(" ", "").toLowerCase()) {
                    $("#postCodeAlert-modal").modal();
                    return;
                }
                if (shippingForm != null) {
                    shippingForm.$setSubmitted();
                    if (shippingForm.$error.required != null) {
                        if (!ck.sameAsBillAddress && billingForm != null) {
                            if (billingForm.$error.required != null) {
                                billingForm.$setSubmitted();
                                return;
                            }
                        }
                        return;
                    }
                }

                if (shippingForm.$invalid) {
                    return
                }
            }

            if (ck.sameAsBillAddress == false && billingForm != null && billingForm.$invalid) {
                billingForm.$setSubmitted();
                return;
            }
            $scope.givex = false;
            //if (temp == 0) {
            //    ck.errors = true;
            //    return;
            //}
            ck.errors = false;
            ck.error1 = [];
            ck.error2 = [];
            if ($scope.guestForm != null) {
                ck.guest = $scope.guestForm.$invalid;
            }
            if ((!shippingForm.$invalid || ck.isClickAndCollect) && (!ck.guest)) {
                $("#productSummery").prop('checked', true);
                $("#addressPanel").removeAttr('checked');
                $("#userLogin").removeAttr('checked');
                $("#paymentPanel").removeAttr('checked');
                $("#productSummery").removeAttr('disabled');
                $("html, body").animate({ scrollTop: 0 }, "slow");
                if (ck.sameAsBillAddress)
                    ck.model.checkout.billingAddress = ck.model.checkout.shippingAddress;
            }
            else {
                $("#productSummery").prop('disabled', true);
                if ($scope.guestForm != null) {
                    if ($scope.guestForm.$error.required != null) {
                        $scope.guestForm.$setSubmitted();
                        $("html, body").animate({ scrollTop: 0 }, "slow");
                    }
                }
            }
            if (ck.model.storeAddress != null) {
                ck.model.checkout.storeAddress = ck.model.storeAddress;
                ck.model.checkout.storeAddress.externalRefId = ck.model.storeAddress.yourId;
            }
            else
                return false;
            $http.post(checkoutConfig.UpdateBasketDeliveryAddress, ck.model.checkout)
                .success(function (data) {
                    ck.model.checkout.stage = data.basketStage;
                    initMethod();
                    ck.postCode = ck.model.checkout.shippingAddress.postCode;
                })
                .error(function (msg) {
                })
        };

        function continueToPayment_2(value) {
            //$scope.shippingForm.$setSubmitted();
            //$scope.billingForm.$setSubmitted();
            if ((!value.$invalid) && (!ck.guest)) {
                $("#paymentPanel").prop('checked', true);
                $("#userLogin").removeAttr('checked');
                $("#addressPanel").removeAttr('checked');
                $("#productSummery").removeAttr('checked');
                $("#paymentPanel").removeAttr('disabled');
                $("html, body").animate({ scrollTop: 0 }, "slow");
            }
            else {
                $("#paymentPanel").prop('disabled', true);
                if ($scope.guestForm != null) {
                    if ($scope.guestForm.$error.required != null) {
                        $scope.guestForm.$setSubmitted();
                        $("html, body").animate({ scrollTop: 0 }, "slow");
                    }
                }
            }
        };

        function clickCollect() {
            if (ck.selectedDelivery.type == checkoutConfig.shipClickAndCollect) {
                ck.postCode = ck.model.storeAddress.postCode;
                if (!(ck.model.storeAddress)) {
                    return;
                }
                ck.model.checkout.storeAddress = ck.model.storeAddress;
                ck.model.checkout.shippingAddress = {};
                if (ck.model.storeAddress) {
                    $("#paymentPanel").prop('checked', true);
                    $("#userLogin").removeAttr('checked');
                    $("#addressPanel").removeAttr('checked');
                    $("#productSummery").removeAttr('checked');
                    $("#paymentPanel").removeAttr('disabled');
                    $("html, body").animate({ scrollTop: 0 }, "slow");
                }
                else {
                    $("#paymentPanel").prop('disabled', true);
                    if ($scope.guestForm != null) {
                        if ($scope.guestForm.$error.required != null) {
                            $scope.guestForm.$setSubmitted();
                            $("html, body").animate({ scrollTop: 0 }, "slow");
                        }
                    }
                }
            }
        };

        function changeAddress(changeFor, address) {
            ck.sameAsBillAddress = false;
            if (changeFor == 'delivery' && address != 'new') {
                ck.model.checkout.shippingAddress = address;
            }
            if (changeFor == 'billing' && address != 'new') {
                ck.model.checkout.billingAddress = address;
            }
            if (changeFor == 'delivery' && address == 'new') {
                ck.model.checkout.shippingAddress = { country: ck.model.checkout.shippingAddress.country, countryCode: ck.model.checkout.shippingAddress.countryCode, isDefault: 1 };
            }
            if (changeFor == 'billing' && address == 'new') {
                ck.model.checkout.billingAddress = { country: ck.model.checkout.billingAddress.country, countryCode: ck.model.checkout.billingAddress.countryCode, isDefault: 1 };
            }
        };
        function registration(model) {
            if (!ck.isPasswordValid) return;
            $http.post(checkoutConfig.register, model)
                .success(function (data) {
                    if (data) {
                        window.location.reload();
                    }
                })
                .error(function (msg) {
                    ck.errorMessage = msg.errorMessages;
                })
                .finally(function () {
                    $timeout(function () { $(".alertBlock").fadeOut(); }, 3000);
                });
        };
        function setBalanceAmt() {
            angular.forEach(ck.model.checkout.paymentOptions, function (pay) {
                pay.cardInfo.amount = ck.basket.grandTotal.raw.withTax - ck.model.checkout.paidAmount.raw.withTax;
            });
        };
        function addToBasket(recordId, qty, displayOrder) {
            if (displayOrder >= 0) {
                ck.displayOrder = displayOrder;
            }
            else {
                ck.displayOrder = ck.basket.lineItems.length + 1;
            }
            var itemType = 0;
            if (ck.basket != null) {
                if (ck.basket.lineItems != null) {
                    angular.forEach(ck.basket.lineItems, function (line) {
                        if (line.productId == recordId) {
                            itemType = line.itemType;
                        }
                    });
                }
            }
            $http.post(checkoutConfig.addToBasket, { "basketId": ck.basket.id, "productId": recordId, "qty": qty, "displayOrder": ck.displayOrder, "itemType": itemType })
                .success(function (data) {
                    //vm.success = true;
                    if (data.messageCode == 'C002') {
                        ck.basketerror = data.message;
                        $('.alert').show(0).delay(4000).hide(0);
                    }
                    var dataResult = ck.serializedData(data.result);
                    ck.basket = dataResult;
                    //ck.showShippingGrid(ck.model.shippingCountries[0].twoLetterIsoCode, ck.model.checkout.basket.id, ck.model.checkout.basket.postCode, ck.model.checkout.basket.shippingMethodId);
                    ck.setBalanceAmt();
                    //initSubscriptionPlan();
                    angular.forEach(ck.basket.lineItems, function (line) {
                        if (line.customInfo1 != '') {
                            reCalculateServiceCharge(line, recordId);
                        }
                        if (line.customInfo2) {
                            line.customInfo2 = JSON.parse(line.customInfo2);
                            line.parentProductId = line.customInfo2.ParentProductId;
                        }
                    })
                })
                .error(function (msg) {
                    // vm.errorMessage = msg.errorMessages;
                })
                .finally(function () {
                    // vm.saving = false;
                    //$("html, body").animate({ scrollTop: 0 }, "slow");
                });
        };

        function applyPromoCode(basketId, promoCode) {
            if ($.trim(promoCode) != "") {
                ck.invalidpromo = false;
                $http.post(globalConfig.applyPromoCode, { id: basketId, promoCode: promoCode }).success(function (data) {
                    $scope.promoCode = null;
                    ck.invalidpromo = false;
                    ck.validpromo = true;
                    $('.promovalid').show(0).delay(2000).hide(0);
                    var dataResult = ck.serializedData(data.result.basket);
                    ck.basket = dataResult;
                    ck.model.checkout.basket = dataResult;
                    ck.setBalanceAmt();
                })
                    .error(function (msg) {
                        ck.invalidpromo = true;
                        $('.promo').show(0).delay(2000).hide(0);
                    })
                    .finally(function () {
                    });
            }
            else {
                ck.promonull = true;
                $('.promonull').show(0).delay(1200).hide(0);
            }
        };

        function removePromoCode(id, promoCode) {
            $http.post(checkoutConfig.removePromoCode, { id: id, promoCode: promoCode })
                .success(function (data) {
                    var dataResult = ck.serializedData(data.result.basket);
                    ck.basket = dataResult;
                    ck.model.checkout.basket = dataResult;
                    ck.setBalanceAmt();
                })
                .error(function (msg) {
                })
                .finally(function () {
                });
        };

        function searchPhysicalStore(postCode) {
            $http.post(checkoutConfig.searchPhysicalStore, { id: ck.basket.id, postCode: postCode }).success(function (data) {
                ck.stores = data;
                var currentDate = new Date();
                // var m = ['Jan', 'Feb', 'Mar', 'Aprl', 'May', 'Jun', 'July', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

                for (var i = 0; i < data.length; i++) {
                    var day = currentDate.getDay();
                    if (data[i].openingHours != null) {
                        ck.slots = data[i].openingHours.split(',');
                        ck.currentDaySlot = ck.slots[day];
                        ck.slot = ck.currentDaySlot.split('-');
                        var startTime = ck.slot[0] + ":00";
                        var endTime = ck.slot[1] + ":00";
                        var interval = "120";
                        var timeSlots = [startTime];
                        for (var j = 0; j < 4; j++) {
                            startTime = addMinutes(startTime, interval);
                            timeSlots.push(startTime);
                        }
                        ck.stores[i].openingHours = timeSlots;
                    }

                }
            })
                .error(function (msg) {
                })
                .finally(function () {
                });
        };
        function addMinutes(time, minutes) {
            var date = new Date(new Date('01/01/2015 ' + time).getTime() + minutes * 60000);
            var tempTime = ((date.getHours().toString().length == 1) ? '0' + date.getHours() : date.getHours()) + ':' +
                ((date.getMinutes().toString().length == 1) ? '0' + date.getMinutes() : date.getMinutes()) + ':' +
                ((date.getSeconds().toString().length == 1) ? '0' + date.getSeconds() : date.getSeconds());
            return tempTime;
        }

        function getNominatedDelivery(delivery) {

            if (ck.nextWeekDate == null) { ck.nextWeekDate = new Date(ck.nominatedDates[0].deliveryDate); }
            if (delivery == 1) { ck.nextWeekDate.setDate(ck.nextWeekDate.getDate() + 7); }
            else {
                ck.nextWeekDate.setDate(ck.nextWeekDate.getDate() - 7);
                //var diff = ck.nextWeekDate.getDate() - new Date(ck.getNominatedDelivery[0]).getDate();
            }
            $http.post(checkoutConfig.nominatedDelivery, { startDate: ck.nextWeekDate }).success(function (dates) {
                ck.nominatedDates = dates;
                ck.showDates = true;
                if (startDate < ck.nominatedDates[0].deliveryDate) {
                    ck.prevDate = true;
                }
                else {
                    ck.prevDate = false;
                }
            });
        };

        function setStoreAddress(store) {
            ck.model.storeAddress = store;
        };

        function selectNominatedDelivery(date) {
            ck.model.checkout.nominatedDeliveryDate = date.deliveryDate;
            ck.model.checkout.nominatedDeliveryPrice = date.price.formatted.withTax;
            $http.post(globalConfig.updateShipping, { id: ck.basket.id, shippingId: ck.basket.shippingMethodId, nominatedDelivery: date })
                .success(function (data) {
                    var dataResult = ck.serializedData(data);
                    ck.basket = dataResult;
                    ck.selectedDayText = date.dayText;
                    ck.setBalanceAmt();
                })
                .error(function (msg) {
                })
                .finally(function () {
                });
        }

        function logout() {
            $http.post(checkoutConfig.logout)
                .success(function (data) {
                    window.location.reload();
                })
                .error(function (msg) {
                })
                .finally(function () {
                });
        };
        function updateNominatedShipping(id, shippingId, selectedDelivery) {

            $http.post(globalConfig.updateShipping, { id: id, shippingId: shippingId, nominatedDelivery: ck.nominatedDates[0] })
                .success(function (data) {
                    var dataResult = ck.serializedData(data);
                    ck.basket = dataResult;
                    ck.selectedDayText = ck.nominatedDates[0].dayText;
                    ck.setBalanceAmt();
                })
                .error(function (msg) {
                })
                .finally(function () {
                });
        }
        function validateGuestPassword(model) {
            model.email = ck.model.checkout.email;
            ck.model.checkout.password = ck.model.loginRegistration.registration.password;
            model.password = ck.model.loginRegistration.registration.password;
            model.confirmPassword = ck.model.loginRegistration.registration.confirmPassword;
            $http.post(checkoutConfig.validateGuestPassword, model)
                .success(function (data) {
                    ck.check = false;
                    ck.passwordError = null;
                    $("#guest-modal").modal('hide');
                    ck.placeOrder();
                })
                .error(function (msg) {
                    ck.passwordError = msg.errorMessages;
                })
                .finally(function () {
                });
        };
        function validateLoginPassword(model) {
            model.email = ck.model.checkout.email;
            ck.model.checkout.userName = ck.model.checkout.email;
            ck.model.checkout.password = ck.model.loginRegistration.login.password;
            $http.post(checkoutConfig.signIn, ck.model.checkout)
                .success(function (data) {
                    ck.check = false;
                    ck.passwordError = null;
                    $("#guest-modal").modal('hide');
                    ck.placeOrder();
                })
                .error(function (msg) {
                    ck.passwordError = msg.errorMessages;
                })
                .finally(function () {
                });
        };
        function setPassword() {
            if (ck.check) {
                $("#guest-modal").modal();
            }
            else {
                ck.placeOrder();
            }
        }
        function purchaseFor(purchase) {
            if (!purchase.me & !purchase.gift) { ck.model.checkout.giftOrMe = 0; }
            if (purchase.me & !purchase.gift) { ck.model.checkout.giftOrMe = 1; }
            if (!purchase.me & purchase.gift) { ck.model.checkout.giftOrMe = 2; }
            if (purchase.me & purchase.gift) { ck.model.checkout.giftOrMe = 3; }
        };

        function addProductToWishlist(item) {
            ck.addToBasket(item.productId, 0, item.displayOrder);
            $http.post(checkoutConfig.addProductToWishlist, { id: item.productId.toLowerCase() })
                .success(function (data) {
                    ck.model.checkout.wishlistProducts = data;
                })
                .error(function (msg) {
                })
                .finally(function () {
                });
        };

        function updateQtyAndAdd(productId, newQty, oldQty, displayOrder) {
            ck.updateQty = 0;
            if (newQty == oldQty || !oldQty) {
                return ck.basketResponse;
            }
            else {
                if (newQty > oldQty) {
                    ck.updateQty = newQty - oldQty;
                } else {
                    ck.updateQty = -(oldQty - newQty);
                }
                ck.addToBasket(productId, ck.updateQty, displayOrder);
            }
        };

        function removeProductToWishlist(item) {
            $http.post(checkoutConfig.removeWishList, { id: item.recordId.toLowerCase() })
                .success(function (data) {
                    ck.model.checkout.wishlistProducts = data;
                })
                .error(function (msg) {
                })
                .finally(function () {
                });
        };

        function validateWishlist() {
            ck.wishlisterror = true;
            $timeout(function () {
                ck.wishlisterror = false;
            }, 3000);
        };

        function addPersistentBasket(id, sourceBasketId) {
            $http.post(checkoutConfig.addPersistentBasket, { id: id, sourceBasketId: sourceBasketId })
                .success(function (data) {
                    window.location.reload();
                })
                .error(function (msg) {
                })
                .finally(function () {
                });
        };


        function initMethod() {
            ck.custAddressGrid();
            //ck.getCurrentBasketData();
            $("#userLogin").prop('checked', true);
            $("#paymentPanel").prop('checked', false);
            $("#addressPanel").prop('checked', false);
            if (ck.UserSelectedShippingEvent.length > 0) {
                ck.shippingSelected = true;
                if (ck.model.checkout.stage == 2) {
                    $("#userLogin").prop('checked', true);
                    $("html, body").animate({ scrollTop: 0 }, "slow");
                }
                if (ck.model.checkout.stage == 3) {
                    $("#addressPanel").prop('checked', true);
                    $("#userLogin").removeAttr('checked');
                    $("html, body").animate({ scrollTop: 0 }, "slow");
                }
                if (ck.model.checkout.stage == 4) {
                    $("#userLogin").removeAttr('checked');
                    $("#paymentPanel").removeAttr('disabled');
                    $("#addressPanel").removeAttr('checked');
                    $("#paymentPanel").prop('checked', false);
                    $("html, body").animate({ scrollTop: 0 }, "slow");
                }
            }
            if (!(!ck.model.checkout.basket.quoteStatus) && ck.model.checkout.basket.quoteStatus == checkoutConfig.paymentLinkSent) {
                $("#userLogin").removeAttr('checked');
                $("#addressPanel").removeAttr('checked');
                $("#productSummery").removeAttr('checked');
                $("#userLogin").attr('disabled', 'disabled');
                $("#addressPanel").attr('disabled', 'disabled');
                $("#productSummery").attr('disabled', 'disabled');
                $("#paymentPanel").prop('checked', true);
                $("html, body").animate({ scrollTop: 0 }, "slow");
                ck.showPaymentOption = true;
                ck.shippingSelected = true;
                ck.paymentLinkSend = true;
            }
            if (ck.isPostCodeDiff)
                ck.model.checkout.shippingAddress.postCode = ck.model.checkout.basket.postCode;
            ck.postCode = $.cookie(ck.cookiepostCode);
            initSubscriptionPlan();
        };

        function createQuote(quote) {
            if (quote.quoteName == null) { return; }
            var quoteModel = {
                id: ck.basket.id, billingAddress: ck.model.checkout.billingAddress, shippingAddress: ck.model.checkout.shippingAddress,
                customerId: ck.model.checkout.customerId, email: ck.model.checkout.email, purchaseOrderNo: quote.purchaseOrderNo, quoteName: quote.quoteName
            }
            $http.post(checkoutConfig.createQuote, { quote: quoteModel })
                .success(function (data) {
                    $("#createQuoteMessage").modal();
                    $('#quoteForm').modal('hide');
                    if (data.result) {
                        ck.customQuoteNo = data.message;
                    } else {
                        ck.customQuoteNo = false;
                    }
                })
                .error(function () {
                })
                .finally(function () {
                });
        };
        function onTextFocus(event) {
            event.target.select();
        }
        function checkPassword(form, pwdId, cnfPwdId) {
            if (!ck.myPlugin) { ck.myPlugin = $("input[id='" + pwdId + "']").password_strength(); }
            if (!ck.myPlugin.metReq())
                form[pwdId].$valid = false;
            else
                form[pwdId].$valid = true;
            if (form[pwdId].$modelValue != form[cnfPwdId].$modelValue)
                form[cnfPwdId].$valid = false;
            else
                form[cnfPwdId].$valid = true;
            if (form[pwdId].$valid && form[cnfPwdId].$valid)
                return true;
            else
                return false;
        }

        //Method to show the shipping methods after entering the valid Post code
        function showShippingGrid(countryCode, basketId, postCode, appliedShippingId) {
            //var postCode_regex = /^[a-zA-Z0-9_/-]+$/;
            $.cookie(ck.cookiepostCode, postCode, { path: '/' });
            ck.model.checkout.shippingAddress.postCode = postCode;
            if (postCode != null && postCode != "") {
                //if (postCode_regex.test(postCode)) {
                $http.post(checkoutConfig.getDeliverysByPostCode, { countryCode: countryCode, basketId: basketId, postCode: postCode, appliedShippingId: appliedShippingId })
                    .success(function (data) {
                        $scope.postCodeSelected = true;
                        console.log(data);
                        ck.model.checkout.basket = data;
                        var dataResult = ck.serializedData(data);
                        ck.basket = dataResult;
                        ////window.location.reload();
                    })
                    .error(function (msg) {
                    })
                    .finally(function () {
                    });

                //}
                //else {
                //    $scope.shippingSelected = false;
                //    $scope.postCodeSelected = false;
                //    ck.hideShippingAddress = true;
                //    ck.wrongPostCode = true;
                //    $timeout(function () { ck.wrongPostCode = false; }, 3000);

                //}
            }
        }

        function reCalculateServiceCharge(line, productId) {
            var customInfos = [];
            var promo;
            if (line.productId == productId) {
                var json = eval('(' + line.customInfo1 + ')');
                if (json.clampRal) {
                    json.clampCharges = parseInt(json.clampMultiplier) * line.additionalCharge.raw.withTax;
                    json.additionalCharge = json.clampCharges;
                    customInfos.push({ customInfo1: JSON.stringify(json), productId: productId, additionalCharge: json.clampCharges });
                    promo = 'POWDERCLAMP';
                }
                else if (json.tubeRal) {
                    json.tubeCharges = parseFloat(json.tubeMultiplier) * line.additionalCharge.raw.withTax;
                    json.additionalCharge = json.tubeCharges;
                    customInfos.push({ customInfo1: JSON.stringify(json), productId: productId, additionalCharge: json.tubeCharges });
                    promo = 'POWDERTUBE';
                }
            }
            $http.post("/basket/updateBasketInfo", { model: { basketId: gm.basketResponse.id, lineInfo: customInfos, customInfo1: promo } }).then(function (success) {
                gm.basketResponse = success.data.result;
                window.location.reload();
            }, function (error) { });
        }

        function serializedData(data) {
            if (data != null) {
                if (data.lineItems != null) {
                    angular.forEach(data.lineItems, function (line) {
                        if (isJSON(line.customInfo2)) {
                            line.customInfo2 = JSON.parse(line.customInfo2);
                            line.parentProductId = line.customInfo2.ParentProductId;
                        }
                    });
                }
            }
            return data;
        }
        function getCurrentBasketData() {
            $http.post(globalConfig.getBasketUrl).then(function (success) {
                if (success.data) {
                    ck.basket.lineItems = success.data.lineItems;
                }
                else if (success.data == null) {
                    ck.basket.lineItems = [];
                }
            }, function (error) { });
        }
        //function getBasketSubcriptionSettings() {

        //    angular.forEach(ck.basket.lineItems, function (items) {
        //        if (items.subscriptionUserSettings != null && items.subscriptionUserSettings.subscriptionJson != "") {
        //            var subscriptionPlan = JSON.parse(items.subscriptionUserSettings.subscriptionJson);
        //            ck.userSubscriptionSettings.selectedTerm = subscriptionPlan.Terms[0].SubscriptionTerm.Duration + " " + subscriptionPlan.Terms[0].SubscriptionTerm.IntervalType;
        //            ck.userSubscriptionSettings.paymentType = items.subscriptionUserSettings.userPricingType;
        //            //getting price according to subscription payment type
        //            if (ck.userSubscriptionSettings.paymentType == SUBSCRIPTION_ENUMS.UserPricingType.OneTime) {
        //                ck.userSubscriptionSettings.subscriptionPrice = subscriptionPlan.RecurringFee.CurrencySymbol + subscriptionPlan.OneTimeFee.Raw.WithTax;
        //            }
        //            else if (ck.userSubscriptionSettings.paymentType == SUBSCRIPTION_ENUMS.UserPricingType.Recurring) {
        //                ck.userSubscriptionSettings.subscriptionPrice = subscriptionPlan.RecurringFee.CurrencySymbol + subscriptionPlan.RecurringFee.Raw.WithTax;
        //            }
        //            //adding currency 

        //            //handling in case order trigger type is fixed
        //            if (subscriptionPlan.OrderTriggerType == SUBSCRIPTION_ENUMS.SubscriptionOrderTriggerType.FixedDay) {
        //                if (subscriptionPlan.OrderTriggerDayOfMonth) {
        //                    ck.userSubscriptionSettings.trigger = subscriptionPlan.OrderTriggerDayOfMonth + "Day of the Month";
        //                }
        //            }
        //            else if (subscriptionPlan.OrderTriggerType == SUBSCRIPTION_ENUMS.SubscriptionOrderTriggerType.Rolling) {
        //                if (subscriptionPlan.OrderTriggerDayOfMonth) {
        //                    ck.userSubscriptionSettings.trigger = subscriptionPlan.OrderTriggerDayOfMonth + "Day of the Month";
        //                }
        //            }
        //            else {
        //                //handling in case of user defined
        //            }
        //        }
        //    });
        //}

        //Subscription Methods.
        //Updates subscription plan on any changes in user subscription preference.
        function updateBasketSubscriptionInfo() {
            //Extract and build user subscription preference model.
            //get the specific line with subscription and product id.
            var subscriptionItem = ck.basket.lineItems.find(i => i.isSubscription);
            var userSetting = {
                subscriptionPlanId: ck.subscriptionPlan.RecordId,
                subscriptionTermId: ck.subscriptionModel.selectedTerm.Id,
                userPricingType: ck.subscriptionModel.selectedPricing
            };

            //Updated the user settings. 
            $http.post("/basket/UpdateBasketSubscriptionInfo", { basketId: ck.basket.id, productId: ck.emptyGuid, userSetting: userSetting }).then(function (success) {
                //ck.initBasket(success.data.result);
                ck.basket = success.data.result;
            }, function (error) { });
        }

        //Initialize subscription plan from item line
        function initSubscriptionPlan() {
            if (ck.basket != null && ck.basket.lineItems.length > 0) {
                //parse subscription plan for each subscription line items. 
                angular.forEach(ck.basket.lineItems, function (item) {
                    if (item.isSubscription && item.subscriptionUserSettings.subscriptionJson != null
                        && item.subscriptionUserSettings.subscriptionJson != undefined && typeof item.subscriptionUserSettings.subscriptionJson != 'object') {
                        item.subscriptionUserSettings.subscriptionPlan = JSON.parse(item.subscriptionUserSettings.subscriptionJson);
                    }
                });
                //Get subscription Item 
                var subscriptionItem = ck.basket.lineItems.find(i => i.isSubscription);
                if (subscriptionItem != null) {
                    //initilize basket level subscription plan
                    ck.subscriptionPlan = subscriptionItem.subscriptionUserSettings.subscriptionPlan;
                    //update subscription term dropdown on condition: 
                    //if basket contains more than term-duration products, then remove that term from drop down.
                    var noOfSubscriptionItemInCart = ck.basket.lineItems.filter(i => i.isSubscription).length;
                    ck.subscriptionPlan.Terms = ck.subscriptionPlan.Terms.filter(i => i.SubscriptionTerm.Duration >= noOfSubscriptionItemInCart);
                    if (ck.subscriptionPlan != null) {
                        //Extract subscription term.
                        if (subscriptionItem.subscriptionUserSettings.subscriptionTermId != null &&
                            subscriptionItem.subscriptionUserSettings.subscriptionTermId != ck.emptyGuid) {
                            //Extract user selected term from subscription line item. 
                            var selectedTerm = ck.subscriptionPlan.Terms.find(i => i.Id == subscriptionItem.subscriptionUserSettings.subscriptionTermId);
                            if (selectedTerm != null && selectedTerm != undefined) {
                                ck.subscriptionModel.selectedTerm = selectedTerm
                            } else {
                                ck.subscriptionModel.selectedTerm = ck.subscriptionPlan.Terms[0];
                            }
                            //ck.subscriptionModel.selectedTerm = ck.subscriptionPlan.Terms.find(i => i.Id == subscriptionItem.subscriptionUserSettings.subscriptionTermId);
                        }
                        else {
                            //Extract default subscription term. 
                            var selectedTerm = ck.subscriptionPlan.Terms.find(i => i.IsDefault);
                            if (selectedTerm != null && selectedTerm != undefined) {
                                ck.subscriptionModel.selectedTerm = selectedTerm;
                            } else {
                                ck.subscriptionModel.selectedTerm = ck.subscriptionPlan.Terms[0];
                            }
                            //ck.subscriptionModel.selectedTerm = ck.subscriptionPlan.Terms.find(i => i.IsDefault);
                        }
                        //Extract subscription pricing 
                        if (subscriptionItem.subscriptionUserSettings.userPricingType != null &&
                            subscriptionItem.subscriptionUserSettings.userPricingType != SUBSCRIPTION_CONSTANTS.UserPricingType.None) {
                            ck.subscriptionModel.selectedPricing = subscriptionItem.subscriptionUserSettings.userPricingType
                        }
                        else {
                            //Default pricing preference is recurring.
                            ck.subscriptionModel.selectedPricing = SUBSCRIPTION_CONSTANTS.UserPricingType.Recurring;
                        }
                        updateBasketSubscriptionInfo();
                    }
                }
            }
        }

    };

}());