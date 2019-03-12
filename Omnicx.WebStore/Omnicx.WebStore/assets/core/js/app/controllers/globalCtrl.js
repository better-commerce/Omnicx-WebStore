(function () {
    'use strict';
    // Added constant for Bulk Order Messages
    window.app.constant('BULKORDER_CONSTANTS', {
        'STOCK_UNAVAILABLE': 'QP01', 'STOCK_AVAILABLE': 'QP02', 'SUCCESS': 'C001', 'POSTCODE': 'POSTCODE'
    });
    // Added constant for Tube cutting and Powder coating
    window.app.constant('SERVICE_CONSTANTS', {
        'CUTTING': 'Cutting', 'COATING': 'Coating'
    });
    // ADD CONSTANT FOR THEME DEFAULT IMAGE
    window.DEFAULT_IMAGE_URL = '/assets/theme/ocx/images/noimagefound.jpg';
    window.app.controller('globalCtrl', globalCtrl);
    globalCtrl.$inject = ['$scope', '$timeout', 'globalConfig', 'loader', '$http', 'CapturePlus', 'scriptLoader', 'BULKORDER_CONSTANTS', 'SERVICE_CONSTANTS', 'Recommendation'];
    function globalCtrl($scope, $timeout, globalConfig, $http, loader, CapturePlus, scriptLoader, BULKORDER_CONSTANTS, SERVICE_CONSTANTS, Recommendation) {
        var gm = this;
        gm.model = {};

        gm.errorMessage == null;
        gm.stockErrorMessage == null;
        gm.saving = false;
        gm.success = false;
        gm.basketResponse = null;
        gm.updateQtyAndAdd = updateQtyAndAdd;
        gm.basketExtraInfo = [];
        gm.miniBasketSize = 3;
        gm.lineItemTotal = 0;
        gm.shippingMethods = [];
        gm.invalidpromo = false;
        gm.blogReponse = [];
        gm.emailinvalid = false;
        gm.subssuccess = false;
        gm.customerEmail = '';
        gm.alreadySubscribed = false;
        gm.emptyGuid = '00000000-0000-0000-0000-000000000000';
        gm.openQuickBasketModal = openQuickBasketModal;
        gm.nRows = nRows;
        gm.userName = '';
        gm.incVat = ($.cookie('incVat') === undefined) ? false : ($.cookie('incVat') == 'true');
        gm.isChecked = !gm.incVat;
        $scope.signin = false;
        $scope.register = false;
        $scope.global_login = false;
        gm.basketData = null;
        gm.isPasswordPolicyMeet = isPasswordPolicyMeet;
        gm.SubscriptionCartEnabled = false;
        //methods
        gm.userLogin = userLogin;
        gm.registration = registration;
        gm.contactForm = contactForm;
        gm.currencySettings = currencySettings;
        gm.initBasket = initBasket;
        gm.showShippingGrid = showShippingGrid;
        gm.addToBasket = addToBasket;
        gm.getPaymentMethods = getPaymentMethods;
        gm.getShippingMethods = getShippingMethods;
        gm.updateShipping = updateShipping;
        gm.applyPromoCode = applyPromoCode;
        gm.getallblogs = getallblogs;
        gm.getallblogsbycategory = getallblogsbycategory;
        gm.getBlogByCategory = getBlogByCategory;
        gm.getBlogByCategory = getBlogByCategory;
        gm.initblogs = initblogs;
        gm.login = login;
        gm.globalLogin = globalLogin;
        gm.newsLetterSubscription = newsLetterSubscription;
        gm.showBasket = showBasket;
        gm.removePromoCode = removePromoCode;
        gm.forgotPassword = forgotPassword;
        gm.addProductsInBasket = addProductsInBasket;
        gm.addProductsExcel = addProductsExcel;
        gm.showCompanyForm = showCompanyForm;
        gm.registerCompanyRequest = registerCompanyRequest;
        gm.productPrice = productPrice;
        gm.socialSignIn = socialSignIn;
        gm.getSocialSettings = getSocialSettings;
        gm.getBillingCountries = getBillingCountries;
        gm.basketDetails = basketDetails;
        gm.hideBasketDetail = hideBasketDetail;
        gm.isTubeCutter = false;
        gm.checkForSpecificAttribute = checkForSpecificAttribute;
        gm.onTextFocus = onTextFocus;
        gm.checkPassword = checkPassword;
        gm.isPasswordValid = false;
        gm.returnUrl = '';
        var eventMethod = window.addEventListener ? "addEventListener" : "attachEvent";
        var eventer = window[eventMethod];
        var messageEvent = eventMethod == "attachEvent" ? "onmessage" : "message";
        gm.addQuoteToBasket = addQuoteToBasket;
        gm.removeQuoteBasket = removeQuoteBasket;
        gm.basketAction = basketAction;
        gm.cancelChangePostCode = cancelChangePostCode;
        gm.serializedData = serializedData;
        gm.formReset = formReset;
        gm.removeProductFromWishlist = removeProductFromWishlist;
        gm.initLookbooks = initLookbooks;
        gm.addLookbookToCart = addLookbookToCart;
        gm.fetchLookbookByGroup = fetchLookbookByGroup;
        gm.updateBasketQty = updateBasketQty;
        gm.getRecommendations = getRecommendations;
        gm.activeClass = '';
        gm.getAllcurrencyandCountries = getAllcurrencyandCountries;
        gm.currencies = [];
        gm.countries = [];
        gm.languageSettings = languageSettings;
        gm.isSubscriptionCart = isSubscriptionCart;
        gm.updateBasketSubscriptionInfo = updateBasketSubscriptionInfo;

        /*******below code is for recomendation**********/
        gm.recomendations = {};

        // data used to generate qty dropdown in basket 
        gm.basketQtyDropdown = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20];

        gm.guid = guid;

        //##################### NEED TO TRANSFER THIS FUNCTION TO COMMON.JS FILE ####################
        function guid() {
            function s4() {
                return Math.floor((1 + Math.random()) * 0x10000)
                    .toString(16)
                    .substring(1);
            }
            return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
        }
        //##################### NEED TO TRANSFER THIS FUNCTION TO COMMON.JS FILE ####################

        function initPCALookup() {
            if (globalConfig.pcaAccessCode != undefined && globalConfig.pcaAccessCode != '') {
                window.setTimeout(function () {
                    if (!gm.defaultCountry) {
                        $http.post(globalConfig.getDefaultCountryUrl)
                            .success(function (country) { gm.defaultCountry = country; });
                    }
                    // address PCA Predict
                    var optionsBilling = {
                        key: globalConfig.pcaAccessCode,
                        countries: {
                            codeList: gm.defaultCountry
                        }
                    };

                    var fieldsBilling = [
                        { element: 'gm.model.company.address1', field: 'Line1' },
                        { element: 'gm.model.company.address2', field: 'Line2', mode: pca.fieldMode.POPULATE },
                        { element: 'gm.model.company.city', field: 'City', mode: pca.fieldMode.POPULATE },
                        { element: 'gm.model.company.state', field: 'Province', mode: pca.fieldMode.POPULATE },
                        { element: 'gm.model.company.postCode', field: 'PostalCode' }
                    ];

                    var controlBilling = new pca.Address(fieldsBilling, optionsBilling);

                    controlBilling.listen('options', function (options) {
                        options.countries = options.countries || {};
                        options.countries.codesList = "GB" //document.getElementById('gm.model.countryCode').value;
                    });

                    controlBilling.listen('populate', function (address, variations) {
                        CapturePlusCallback();
                    });

                    controlBilling.load();
                }, 1500);
            }
        }

        function checkForSpecificAttribute(attributeCode, attributevalue) {
            var flag = false;
            if (gm.basketResponse.lineItems != null) {
                angular.forEach(gm.basketResponse.lineItems, function (line) {
                    var parsedJSON = eval('(' + line.attributesJson + ')');
                    var attr = parsedJSON.Attributes;
                    if (attr != null) {
                        for (i = 0; i < attr.length; i++) {
                            // this can be a string or null
                            var FieldCode = attr[i].FieldCode;
                            var FieldValue = attr[i].FieldValue;
                            if (FieldCode != null && FieldCode != "" && FieldCode == attributeCode) {
                                //line.customInfo1 = FieldCode + "" + FieldValue;
                                if (FieldValue != null && FieldValue == attributevalue) {
                                    flag = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (flag)
                        return flag;
                });
                return flag;
            }
        }

        function contactForm(model) {
            $(".alertBlock").fadeIn();
            $http.post(globalConfig.setContactForm, { model: model })
                .success(function (data) {
                    gm.errorMessage = null;
                    gm.success = true;
                })
                .error(function (msg) {
                    gm.errorMessage = msg.errorMessages;
                })
                .finally(function () {
                    gm.saving = false;
                    $timeout(function () { $(".alertBlock").fadeOut(); }, 3000);
                });
        }

        //#region Basket
        function applyPromoCode(basketId, promoCode) {

            if ($.trim(promoCode) != "") {
                gm.model.Id = basketId;
                gm.invalidpromo = false;
                gm.showCustomMsg = false;
                gm.model.promoCode = promoCode;
                $http.post(globalConfig.applyPromoCode, gm.model).success(function (data) {
                    $scope.promoCode = null;
                    gm.invalidpromo = false;
                    gm.validpromo = true;
                    $('.promovalid').show(0).delay(2000).hide(0);
                    var dataResult = gm.serializedData(data.result.basket);
                    gm.basketResponse = dataResult;
                    reCalculateLineitemsAndQty();
                })
                    .error(function (msg) {
                        if (msg.errorMessage != "false") {
                            gm.showCustomMsg = true;
                            gm.customMsg = msg.errorMessage;
                        }
                        gm.invalidpromo = true;
                        $('.promo').show(0).delay(2000).hide(0);
                    })
                    .finally(function () {
                    });
            }
            else {
                gm.promonull = true;
                $('.promonull').show(0).delay(1200).hide(0);
            }
        }
        function cancelChangePostCode() {
            gm.basketResponse.postCode = $.cookie(BULKORDER_CONSTANTS.POSTCODE);
        }
        //Method to show the shipping methods after entering the valid Post code
        function showShippingGrid(countryCode, basketId, postCode, appliedShippingId) {
            $.cookie(BULKORDER_CONSTANTS.POSTCODE, postCode, { path: '/' });
            //var postCode_regex = /^[a-zA-Z0-9_/-]+$/;
            if (postCode != null && postCode != "") {
                //if (postCode_regex.test(postCode)) {
                $http.post(globalConfig.getDeliverysByPostCode, { countryCode: countryCode, basketId: basketId, postCode: postCode, appliedShippingId: appliedShippingId })
                    .success(function (data) {
                        $scope.postCodeSelected = true;
                        var dataResult = gm.serializedData(data);
                        gm.basketResponse = dataResult;
                        gm.basketResponse.postCode = $.cookie(BULKORDER_CONSTANTS.POSTCODE);
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
        function initAddToBagQty() {
            if (gm.maximumAddToBasketLimit > 0) {
                gm.basketQtyDropdown = [];
                for (i = 1; i <= gm.maximumAddToBasketLimit; i++) {
                    gm.basketQtyDropdown.push(i);
                }
            }
        }
        function isSubscriptionCart() {
            if (gm.basketResponse.containsSubscription) {
                gm.SubscriptionCartEnabled = true;
                angular.forEach(gm.basketResponse.lineItems, function (line) {
                    if (!line.isSubscription) {
                        gm.SubscriptionCartEnabled = false;
                    }
                });
            }
        };
        function reCalculateLineitemsAndQty() {
            if (gm.basketResponse != null && gm.basketResponse != undefined) {

                angular.forEach(gm.basketResponse.lineItems, function (line) {
                    var json = eval('(' + line.attributesJson + ')');
                    line.slug = json.Slug;
                    if (isJSON(line.customInfo2)) {
                        line.customInfo2 = JSON.parse(line.customInfo2);
                        line.parentProductId = line.customInfo2.ParentProductId;
                    }
                    if (line.subscriptionJSON != null && line.subscriptionJSON != undefined && typeof (line.subscriptionJSON) != 'object' && line.subscriptionJSON != '') {
                        line.subscriptionPlan = JSON.parse(line.subscriptionJSON);
                    }
                    line.updatedqty = line.qty;
                });
                gm.count = gm.basketResponse.lineItemCount;

                if (gm.basketResponse.deliveryPlans != undefined) {

                    angular.forEach(gm.basketResponse.deliveryPlans, function (dp) {

                        angular.forEach(dp.lineItems, function (line) {
                            var json = eval('(' + line.attributesJson + ')');
                            line.slug = json.Slug;
                            if (isJSON(line.customInfo2)) {
                                line.customInfo2 = JSON.parse(line.customInfo2);
                                line.parentProductId = line.customInfo2.ParentProductId;
                            }
                            line.updatedqty = line.qty;
                        });

                    });
                }
            }
        };
        function initBasket(basket) {
            if (basket == null || basket == undefined) {
                if (gm.basketResponse == null || gm.basketResponse == undefined) {
                    $http.post(globalConfig.getBasketUrl)
                        .success(function (data) {
                            gm.basketResponse = data;
                            initAddToBagQty();
                            gm.postCode = $.cookie(BULKORDER_CONSTANTS.POSTCODE);
                            reCalculateLineitemsAndQty();
                            gm.isSubscriptionCart();
                        })
                        .error(function (msg) {
                            // vm.errorMessage = msg.errorMessages;
                        })
                        .finally(function () {
                            // vm.saving = false;
                            //$("html, body").animate({ scrollTop: 0 }, "slow");
                        });
                }

            } else {
                gm.basketResponse = basket;
                if (gm.basketResponse != null && gm.basketResponse != undefined) {
                    angular.forEach(gm.basketResponse.lineItems, function (line) {
                        if (isJSON(line.customInfo2)) {
                            line.customInfo2 = JSON.parse(line.customInfo2);
                            line.parentProductId = line.customInfo2.ParentProductId;
                        }
                        line.updatedqty = line.qty;
                    });
                    initAddToBagQty();
                    gm.count = gm.basketResponse.lineItemCount;
                    gm.postCode = $.cookie(BULKORDER_CONSTANTS.POSTCODE);
                }
            }
        }

        function addLookbookToCart(data) {
            var model = {};
            var qty = [];
            angular.forEach(data, function (data) {
                qty.push(1);
            });
            model = { 'stockCode': data, 'qty': qty }
            addProductsInBasket(model);
        }
        gm.maximumBasketItemError = false;
        function addToBasket(recordId, qty, displayOrder) {
            //-- checks if basket already have 12 items-------
            //if (vm.basketResponse.lineItems.length >= 12) {
            //    return false;
            //}
            if (displayOrder >= 0) {
                gm.displayOrder = displayOrder;
            }
            else {
                gm.displayOrder = gm.basketResponse.lineItems.length + 1;
            }
            //var basketId = gm.basketResponse != null ? gm.basketResponse.id : "";
            //$http.post(globalConfig.addToBasket, { "basketId": basketId, "productId": recordId, "qty": qty, "displayOrder": gm.displayOrder })
            var basketId = gm.basketResponse != null ? gm.basketResponse.id : "";
            var itemType = 0;
            var prodInBasket = null
            if (gm.basketResponse != null) {
                if (gm.basketResponse.lineItems != null) {
                    angular.forEach(gm.basketResponse.lineItems, function (line) {
                        if (line.productId == recordId) {
                            itemType = line.itemType;
                            // this line is used in omnilytics  incase of product removal from basket
                            prodInBasket = { 'name': line.name, basketId: basketId, 'stockCode': line.stockCode, 'stockCode': line.stockCode, 'price': line.price, 'manufacturer': line.manufacturer, 'category': line.category, 'qty': qty, 'image': line.image };
                        }

                    });
                }
            }
            initAddToBagQty();
            if (gm.maximumAddToBasketLimit < qty && qty > 0) {
                $(".wishdiv").fadeIn();
                gm.maximumBasketItemError = true;
                window.setTimeout(function () {
                    $(".wishdiv").fadeOut();
                    gm.maximumBasketItemError = false;
                }, 3000);
                return;
            }
            var prod = { "basketId": basketId, "productId": recordId, "qty": qty, "displayOrder": gm.displayOrder, "itemType": itemType };
            var eventData = { product: prodInBasket, basket: { id: null, lines: [], totalCost: null, totalItems: null, tax: null } };
            $http.post(globalConfig.addToBasket, prod)
                .success(function (data) {
                    if (data !=null && data == false) {
                        $(".wishdiv").fadeIn();
                                gm.maximumBasketItemError = true;
                                window.setTimeout(function () {
                                    $(".wishdiv").fadeOut();
                                    gm.maximumBasketItemError = false;
                                }, 3000);
                                return;                    
                    }
                    //vm.success = true;
                    if (data.messageCode == 'C002')
                        gm.errorMessage = data.message;
                    gm.basketResponse = data.result;
                    reCalculateLineitemsAndQty();
                    var count = 0;
                    if (gm.basketResponse != null) {
                        if (gm.basketResponse.lineItems != null) {
                            eventData.id = gm.basketResponse.id;
                            eventData.basket.totalItems = gm.basketResponse.lineItemCount;
                            eventData.basket.totalCost = gm.basketResponse.subTotal.raw.withoutTax;
                            eventData.basket.tax = gm.basketResponse.grandTotal.raw.tax;
                            angular.forEach(gm.basketResponse.lineItems, function (line) {
                                var li = { id: line.id, basketId: gm.basketResponse.id, stockCode: line.stockCode, name: line.name, qty: line.qty, price: line.price.raw.withoutTax, tax: line.price.raw.tax, manufacturer: line.Manufacture, 'img': line.image };
                                eventData.basket.lines.push(li);
                                if (line.parentProductId == gm.emptyGuid) {
                                    count = count + line.qty;
                                }
                                line.updatedqty = line.qty;
                                var json = eval('(' + line.attributesJson + ')');
                                line.slug = json.Slug;
                                if (line.productId.toLowerCase() == prod.productId.toLowerCase()) {
                                    var prodDetail = angular.copy(line);
                                    prodDetail.qty = qty /// set the actual qty that was requested to be aded instead of final  item qty
                                    prodDetail.basketId = basketId;
                                    eventData.product = prodDetail;
                                }
                                if (line.customInfo2) {
                                    line.customInfo2 = JSON.parse(line.customInfo2);
                                    line.parentProductId = line.customInfo2.ParentProductId;
                                }
                            });
                        }
                        gm.count = gm.basketResponse.lineItemCount;
                        window.setTimeout(function () { imgix.init({ force: true }); }, 1000);
                    }
                    window.setTimeout(function () {
                        gm.errorMessage = null;
                    }, 3000);
                    if (qty >= 1) {
                        var scroll = $(window).scrollTop();
                        if (scroll >= 200) {
                            $("#miniBasket").addClass("fix-to-top");
                        } else {
                            $('.cartopen').addClass('active');
                            $("#miniBasket").removeClass("fix-to-top");
                        }
                        //$("html, body").animate({ scrollTop: 0 }, "slow");
                        $('.cartopen').addClass('active');
                        $timeout(function () { $(".cartopen").removeClass("active"); }, 3000);
                    }
                    gm.removeProductFromWishlist(recordId);
                    PubSub.publish("addToCart", eventData);

                })
                .error(function (msg) {
                    // vm.errorMessage = msg.errorMessages;
                })
                .finally(function () {
                    // vm.saving = false;
                    //$("html, body").animate({ scrollTop: 0 }, "slow");
                });    
           
        };
        function removeProductFromWishlist(recordId) {
            $http.post(globalConfig.removeProductFromWishlist, { id: recordId })              
                .success(function (data) {
                    if(data != false)
                    PubSub.publish("wishListData", data);
                  //window.location.reload();
                })
                .error(function (msg) {
                })
                .finally(function () {
                });
        };

        function getShippingMethods(countryCode) {

            $http.post(globalConfig.getShippingMethods, { 'countryCode': countryCode })
                .success(function (data) {
                    gm.basketResponse.shippingMethods = data;
                })
                .error(function (msg) {
                    // vm.errorMessage = msg.errorMessages;
                })
                .finally(function () {
                    // vm.saving = false;
                    //$("html, body").animate({ scrollTop: 0 }, "slow");
                });


        }
        function updateQtyAndAdd(productId, newQty, oldQty, displayOrder) {
            gm.updateQty = 0;
            if (newQty == oldQty || !oldQty) {
                return gm.basketResponse;
            }
            else {
                if (newQty > oldQty) {
                    gm.updateQty = newQty - oldQty;
                } else {
                    gm.updateQty = -(oldQty - newQty);
                }
                gm.addToBasket(productId, gm.updateQty, displayOrder);
            }
        };

        function updateShipping(id) {
            var requestSource = window.location.pathname.substring(window.location.pathname.lastIndexOf('/') + 1);
            $http.post(globalConfig.updateShipping, { id: gm.basketResponse.id, shippingId: id, nominatedDelivery: null, requestSource: requestSource.toLowerCase() })
                .success(function (data) {
                    var dataResult = gm.serializedData(data.basket);
                    gm.basketResponse = dataResult;
                    reCalculateLineitemsAndQty();
                    angular.forEach(data.basket.shippingMethods, function (obj, key) {
                        if (obj.id == id) {
                            gm.basketResponse.isPriceOnRequest = obj.isPriceOnRequest;
                        }
                    });
                    angular.forEach(data.basket.lineItems, function (line) {
                        if (line.parentProductId == gm.emptyGuid) {
                            line.updatedqty = line.qty;
                        }
                    });
                })
                .error(function (msg) {
                    // vm.errorMessage = msg.errorMessages;
                })
                .finally(function () {
                    // vm.saving = false;
                    //$("html, body").animate({ scrollTop: 0 }, "slow");
                });
        };

        function removePromoCode(id, promoCode) {
            $http.post(globalConfig.removePromoCode, { id: id, promoCode: promoCode })
                .success(function (data) {
                    var dataResult = gm.serializedData(data.result.basket);
                    gm.basketResponse = dataResult;
                    reCalculateLineitemsAndQty();
                })
                .error(function (msg) {
                })
                .finally(function () {
                });
        };

        function addProductsInBasket(line) {
            gm.stockErrorMessage = '';
            gm.wrongFormatError = '';
            gm.basketMessage = '';
            gm.stockUnavailable = false;
            if (line.stockCode == undefined || line.qty == undefined || line == null) {
                gm.wrongFormatMessage = true;
                $('.wrongFormatError').show(0).delay(3000).hide(0);
            }
            angular.forEach(line.stockCode, function (item, key) {
                if (item != undefined) {
                    angular.forEach(line.stockCode, function (i, k) {
                        if (i != undefined && item == i && key != k) {
                            gm.stockErrorMessage = (parseInt(key, 10) + 1) + " & " + (parseInt(k, 10) + 1);
                        }
                    });
                }
                if (item == "") {
                    delete line.stockCode[key];
                    delete line.qty[key];
                }
            });
            //angular.forEach(line.stockCode, function (item, key) {
            //    if (item != undefined) {
            //        angular.forEach(line.stockCode, function (i, k) {
            //            if (i != undefined && item == i && key != k) {
            //                gm.stockErrorMessage = (parseInt(key, 10) + 1) + " & " + (parseInt(k, 10) + 1);
            //            }
            //        });
            //    }
            //    if (item == "") {
            //        delete line.stockCode[key];
            //        delete line.qty[key];
            //    }
            //});
            //if (gm.stockErrorMessage) {
            //    $('.stockError').show(0).delay(3000).hide(0);
            //    return;
            //}

            //Added quantity of same SKU

            $scope.bulkOrder = [];
            var isMatchFound = false;
            angular.forEach(line.stockCode, function (value, key) {
                isMatchFound = false;
                if ($scope.bulkOrder.length == 0)
                    $scope.bulkOrder.push({ stockCode: line.stockCode[key], qty: line.qty[key], basketId: "" })
                else {
                    angular.forEach($scope.bulkOrder, function (obj) {
                        if (obj.stockCode == value) {
                            isMatchFound = true;
                            var q = line.qty[key];
                            var r = obj.qty;
                            obj.qty = (parseInt(q) + parseInt(r));
                        }

                    });
                    if (isMatchFound != true)
                        $scope.bulkOrder.push({ stockCode: line.stockCode[key], qty: line.qty[key], basketId: "" })
                }
            });

            $http.post(globalConfig.bulkAddproduct, $scope.bulkOrder)
                .success(function (data) {
                    var dataResult = gm.serializedData(data.result);
                    if (data.messageCode == 'C002') {
                        gm.errorMessage = data.message;
                        $('.alert').show(0).delay(4000).hide(0);
                    }
                    gm.basketResponse = dataResult;
                    PubSub.publish("addToCartBulk", gm.basketResponse);
                    var count = 0;
                    if (gm.basketResponse != null) {
                        if (gm.basketResponse.lineItems != null) {
                            angular.forEach(gm.basketResponse.lineItems, function (line) {
                                if (line.parentProductId == gm.emptyGuid) {
                                    count = count + line.qty;
                                }
                                var json = eval('(' + line.attributesJson + ')');
                                line.slug = json.Slug;
                            });
                            gm.count = gm.basketResponse.lineItemCount;
                        }
                    }
                    $("#AddToBasketModel").modal("hide");
                    $("#bulkOrderMessage").modal();
                    gm.notFoundLength = data.message.split(",").length;
                    if (data.messageCode == BULKORDER_CONSTANTS.STOCK_UNAVAILABLE) {
                        gm.stockUnavailable = true; $('.stockUnavailable').show(0).delay(3000).hide(0);
                        gm.basketMessage = data.message;
                    }
                    if (data.message) {
                        if (data.messageCode == BULKORDER_CONSTANTS.STOCK_AVAILABLE) {
                            gm.basketMessage = data.message;// $('.basketMessage').show(0).delay(3000).hide(0);
                        }
                        if (data.message) {
                            //if (data.messageCode == BULKORDER_CONSTANTS.SUCCESS)
                            //    gm.basketMessage = data.message
                        }
                        else {
                            gm.basketMessage = data.message;
                            // $('.basketMessage').show(0).delay(3000).hide(0);
                        }
                        return;
                    }
                    //if (data.messageCode != "QP01" && data.messageCode != "QP02") {
                    //    $("#AddToBasketModel").modal("hide");
                    //    $("html, body").animate({ scrollTop: 0 }, "slow");
                    //    $('.cartopen').addClass('active');
                    //    $timeout(function () { $(".cartopen").removeClass("active"); }, 3000);
                    //}
                }).error(function () {
                    gm.wrongFormatMessage = true;
                    $('.wrongFormatError').show(0).delay(3000).hide(0);
                });
        }

        function addProductsExcel(line) {
            gm.errorMessage = '';
            gm.wrongFormatError = '';
            gm.basketMessage = '';
            gm.stockUnavailable = false;
            $scope.bulkOrder = [];
            var rows = line.split("\n");
            angular.forEach(rows, function (value, key) {
                $scope.bulkOrder.push({ stockCode: rows[key].split(",")[0], qty: rows[key].split(",")[1], basketId: "" })
            });
            angular.forEach($scope.bulkOrder, function (item, key) {
                if (item != undefined) {
                    angular.forEach($scope.bulkOrder, function (i, k) {
                        if (i != undefined && item.stockCode == i.stockCode && key != k) {
                            gm.errorMessage = " ";
                        }
                    });
                }
            });
            if (gm.errorMessage) {
                $('.stockError').show(0).delay(3000).hide(0);
                return;
            }
            $http.post(globalConfig.bulkAddproduct, $scope.bulkOrder)
                .success(function (data) {
                    var dataResult = gm.serializedData(data.result);
                    gm.basketResponse = dataResult;
                    var count = 0;
                    if (gm.basketResponse != null) {
                        if (gm.basketResponse.lineItems != null) {
                            angular.forEach(gm.basketResponse.lineItems, function (line) {
                                if (line.parentProductId == gm.emptyGuid) {
                                    count = count + line.qty;
                                }
                                var json = eval('(' + line.attributesJson + ')');
                                line.slug = json.Slug;
                            });
                            gm.count = gm.basketResponse.lineItemCount;
                        }
                    }
                    $("#AddToBasketModel").modal("hide");
                    $("#bulkOrderMessage").modal();
                    gm.notFoundLength = data.message.split(",").length;
                    if (data.messageCode == BULKORDER_CONSTANTS.STOCK_UNAVAILABLE) {
                        gm.stockUnavailable = true; $('.stockUnavailable').show(0).delay(3000).hide(0);
                    }
                    if (data.message) {
                        if (data.messageCode == BULKORDER_CONSTANTS.STOCK_AVAILABLE) {
                            gm.basketMessage = data.message;
                        }
                        if (data.message) {
                            if (data.messageCode == BULKORDER_CONSTANTS.SUCCESS)
                                gm.basketMessage = data.message
                        }
                        else {
                            gm.basketMessage = data.message;
                        }
                        return;
                    }
                }).error(function () {
                    gm.wrongFormatMessage = true;
                    $('.wrongFormatError').show(0).delay(3000).hide(0);
                });
        };

        //#endregion

        function userLogin(model) {
            gm.saving = false;
            gm.errorMessage = null;
            gm.success = false;
            $(".alertBlock").fadeIn();
            $http.post(globalConfig.signIn, model)
                .success(function (data) {
                    if (data) {
                        $timeout(function () {
                            gm.isChecked = $.cookie(model.username);
                        }, 3000);
                        $("#login-modal").modal('hide');
                        $.cookie('IsUserLoggedIn', true, { path: '/' });
                        if ($scope.global_login)
                            window.location.reload();
                        else
                            window.location.href = window.location.origin + data.returnUrl;
                    }
                })
                .error(function (msg) {
                    gm.errorMessage = msg.errorMessages;
                    $timeout(function () {
                        $(".alertBlock").fadeOut();
                    }, 3000);

                })
                .finally(function () {
                    gm.saving = false;
                });
        }
        function isPasswordPolicyMeet(flag, form, pwdId, cnfPwdId) {
            if (!flag)
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
        function checkPassword(form, pwdId, cnfPwdId) {
            if (!gm.myPlugin) {
                gm.myPlugin = $("input[id='" + pwdId + "']").password_strength();
            }
            var resp = false;
            $timeout(function () { resp = gm.myPlugin.metReq() }, 1500);
            $timeout(function () { gm.isPasswordPolicyMeet(resp, form, pwdId, cnfPwdId) }, 1500);
        }

        function registration(model) {
            $scope.global_login = false;
            $scope.signin = false;
            $scope.register = true;
            gm.saving = false;
            gm.errorMessage = null;
            gm.success = false;
            $(".alertBlock").fadeIn();
            if (!gm.isPasswordPolicyMeet)
                return false;

            $http.post(globalConfig.register, model)
                .success(function (data) {
                    if (data) {
                        $.cookie('IsUserLoggedIn', true, { path: '/' });
                        window.location.href = "/MyAccount";
                        if (!gm.model.registerViewModel.notifyNone) {
                            var newsletterModel = { Email: gm.model.registerViewModel.email, notifyEmail: gm.model.registerViewModel.notifyByEmail, notifySMS: gm.model.registerViewModel.notifyBySMS, notifyPost: gm.model.registerViewModel.notifyByPost, notifyNone: gm.model.registerViewModel.notifyNone };
                            $http.post(globalConfig.newsletter, newsletterModel).then(function (success) { }, function (error) { });
                        }
                       
                    }
                })
                .error(function (msg) {
                    gm.errorMessage = msg.errorMessages;
                })
                .finally(function () {
                    $timeout(function () { $(".alertBlock").fadeOut(); }, 3000);
                });
           
        }

        function login(model) {
            $scope.signin = true;
            $scope.register = false;
            $scope.global_login = false;
            gm.userLogin(model);
        }

        function globalLogin(model) {
            $scope.signin = false;
            $scope.register = false;
            $scope.global_login = true;
            gm.userLogin(model);
        }

        function forgotPassword(model) {
            $http.post(globalConfig.forgotPassword, model)
                .success(function (data) {
                    gm.errorMessage = null;
                    gm.isValid = data.isValid;
                    gm.isValiduser = !data.isValid;
                    $timeout(function () {
                        gm.isValiduser = false;
                        gm.isValid = false;
                        window.location.href = gm.returnUrl;
                    }, 3000);
                })
                .error(function (msg) {
                    gm.errorMessage = msg.errorMessages;
                    $timeout(function () {
                        gm.isValiduser = false;
                        $(".alertBlock").fadeOut();
                    }, 3000);
                })
                .finally(function () {
                });
        };

        function registerCompanyRequest(model) {
            if (model.country == undefined)
                model.country = gm.defaultCountry;
            $scope.changeForm.$setSubmitted();
            $scope.registrationAlert = true;
            gm.errorMessage = null;
            $(".alertBlock").fadeIn();
            if (!gm.isPasswordValid)
                return false;
            $http.post(globalConfig.companyRegisterUrl, model)
                .success(function (resp) {
                    if (resp.isValid) {
                        if (resp.message) { gm.accountCreated = true; $('.accountCreated').show(0).delay(3000).hide(0); }
                        else { gm.requestSuccess = true; $('.requestSuccess').show(0).delay(3000).hide(0); }
                        $timeout(function () { window.location.reload(); }, 5000);
                    };
                })
                .error(function (msg) {
                    gm.errorMessage = msg.errorMessages;
                })
                .finally(function () {
                    $timeout(function () { $(".alertBlock").fadeOut(); }, 3000);
                });
        }

        //#region blogs

        function getallblogs(page) {
            location.href = 'GetAllBlogs?currentpage=' + page + ''
        }

        function getallblogsbycategory(page, category) {

            location.href = 'GetBlogByCategory?category=' + category + '&currentpage=' + page + ''
        }

        function getBlogByCategory(id, page) {
            $http.post(globalConfig.getBlogByCategory, { category: id, currentpage: page })
                .success(function (data) {
                    var dataResult = gm.serializedData(data);
                    gm.basketResponse = dataResult;

                })
                .error(function (msg) {
                    // vm.errorMessage = msg.errorMessages;
                })
                .finally(function () {
                    // vm.saving = false;
                    //$("html, body").animate({ scrollTop: 0 }, "slow");
                });


        }

        function getBlogByCategory(id, page) {

            $http.post(globalConfig.getBlogsbyCategory, { category: id, currentpage: page })
                .success(function (data) {

                    gm.blogReponse = data;

                })
                .error(function (msg) {
                    // vm.errorMessage = msg.errorMessages;
                })
                .finally(function () {
                    // vm.saving = false;
                    //$("html, body").animate({ scrollTop: 0 }, "slow");
                });


        }

        function initblogs(id) {
            $http.post(globalConfig.getallblogs, { id: id })
                .success(function (data) {

                    gm.blogReponse = data;

                })
                .error(function (msg) {
                    // vm.errorMessage = msg.errorMessages;
                })
                .finally(function () {
                    // vm.saving = false;
                    //$("html, body").animate({ scrollTop: 0 }, "slow");
                });
        }

        //#endregion blogs

        function currencySettings(currencyCode) {
            var data = { currency: currencyCode};
            $http.post(globalConfig.currencySettingUrl, data)
                .success(function () {
                    window.location.reload();
                })
                .error(function (msg) {
                })
                .finally(function () {
                });
        };

        function getPaymentMethods() {
            $http.post(globalConfig.paymentMethodsUrl)
                .success(function (data) {
                    gm.paymentMethods = data;
                })
                .error(function (msg) {
                })
                .finally(function () {
                });
        };

        function newsLetterSubscription(email) {
            gm.emailinvalid = false;
            gm.subssuccess = false;
            var email_regex = /^[_a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$/;
            if (!email_regex.test(email.toLowerCase())) {
                gm.emailinvalid = true;
                $('.newsletteralert').show(0).delay(2000).hide(0);
            }
            if (email == "" || email == null) {
                gm.emailinvalid = true;
                $('.newsletteralert').show(0).delay(2000).hide(0);
            }
            if (gm.emailinvalid == false) {
                $http.post(globalConfig.newsLetterSubscription, { email: email, notifyNone: true })
                    .success(function (data) {
                        gm.customerEmail = '';
                        if (data == true) {
                            gm.subssuccess = true;
                            $('.newslettersuccess').show(0).delay(2000).hide(0);
                        }
                        else {
                            gm.alreadySubscribed = true;
                            $('.newsletteralready').show(0).delay(4000).hide(0);
                        }
                    })
                    .error(function (msg) {
                        gm.alreadySubscribed = true;
                        $('.newsletteralert').show(0).delay(2000).hide(0);
                    })
                    .finally(function () {

                    });
            }

        }

        function showBasket(value) {
            if (value)
                gm.activeClass='active';
            else
                gm.activeClass = '';
        }
        
        function openQuickBasketModal() {
            $("#AddToBasketModel").modal();
        };

        function nRows(num) {
            return new Array(num);
        }
        function showCompanyForm() {
            gm.myPlugin = null;
            getBillingCountries();
            if (globalConfig.pcaAccessCode != undefined && globalConfig.pcaAccessCode != '') {
                scriptLoader.load("//services.postcodeanywhere.co.uk/css/captureplus-2.30.min.css?key=" + globalConfig.pcaAccessCode, "text/css", "stylesheet");
                scriptLoader.load("//services.postcodeanywhere.co.uk/js/captureplus-2.30.min.js?key=" + globalConfig.pcaAccessCode, "text/javascript", "");
            }
            window.setTimeout(function () {
                initPCALookup()
            }, 1500);
            $("#CompanyRegistrationRequest").modal();
        };

        function productPrice(isChecked) {
            gm.incVat = (isChecked) ? false : true;
            $.cookie('incVat', gm.incVat, { path: '/' });
        };

        function socialSignIn(provider) {
            //$http.post(globalConfig.socialSignInUrl, {provider: provider}).success(function (data) {

            //});

            var Form = document.createElement('form');
            Form.setAttribute('method', 'post');
            Form.setAttribute('action', '/Account/SocialSignIn');
            var providertag = document.createElement('input');
            providertag.setAttribute('type', 'hidden');
            providertag.setAttribute('name', 'provider');
            providertag.setAttribute('value', provider);
            Form.appendChild(providertag);

            document.getElementsByTagName('body')[0].appendChild(Form);
            Form.submit();
        };

        function getSocialSettings() {
            $http.post("/Account/GetSocialSettings").then(function (success) { gm.model.socialSettings = success.data; }, function (error) { });
        }

        function getBillingCountries() {
            $http.post(globalConfig.getBillingCountriesUrl)
                .success(function (data) {
                    if (data.length > 0) {
                        gm.countries = data;
                    }
                })
                .error(function (msg) {

                })
                .finally(function () {

                });
        }

        function basketDetails(basketId) {
            gm.basketDetailView = true;
            gm.currentBasket = gm.baskets.find(function (basket) { return basket.id == basketId });
        }
        function hideBasketDetail() {
            gm.basketDetailView = false;
        }

        PubSub.subscribe('addToCart', function (eventData) {
            if (eventData != null && dataLayer && omnilytics) {
                if (eventData.product != null) {
                    var prod = eventData.product
                    var data = dataLayer[0];
                    var entity = { 'basketId': prod.basketId, 'name': prod.name, 'id': prod.productId, 'stockCode': prod.stockCode, 'price': prod.price.raw.withTax, 'brand': prod.manufacturer, 'category': prod.category, 'quantity': prod.qty, 'img': prod.image };
                    data["Entity"] = JSON.stringify(entity);
                    data["EntityId"] = prod.productId;
                    data["EntityName"] = prod.name;
                    data["EntityType"] = "Product";
                    data["BasketItems"] = JSON.stringify(eventData.basket.lines);
                    data["BasketItemCount"] = eventData.basket.lines.length ? eventData.basket.totalItems : 0;
                    data["BasketTotal"] = eventData.basket.totalCost;
                    data["Tax"] = eventData.basket.tax;
                    dataLayer[0] = data;
                    data["Action"] = "addToCart";
                    if (eventData.basket.lines.length == 0 || prod.qty < 1) {
                        data["EventType"] = "BasketItemRemoved";
                        omnilytics.emit('BasketItemRemoved', null);

                    } else {
                        data["EventType"] = "BasketItemAdded";
                        omnilytics.emit('BasketItemAdded', null);

                    }


                }
            }
        });
        function onTextFocus(event) {
            event.target.select();
        }
        function addQuoteToBasket(id, action) {
            if (gm.basketResponse != null && gm.basketResponse.lineItems.length != 0) {
                $("#quoteDetailModal").modal("hide"); $("#mergeBasketModal").modal();
                return;
            }
            gm.basketAction(id, action);
        }
        function basketAction(quoteId, action) {
            $http.post(globalConfig.addQuoteToBasketUrl, { basketId: quoteId, basketAction: action })
                .success(function (data) {
                    var dataResult = gm.serializedData(data);
                    gm.basketResponse = dataResult;
                    $("#quoteDetailModal").modal("hide");
                    $("#mergeBasketModal").modal("hide");
                    if (gm.basketResponse != null && gm.basketResponse != undefined) {
                        gm.count = gm.basketResponse.lineItemCount;
                        var scroll = $(window).scrollTop();
                        if (scroll >= 200) { $("#miniBasket").addClass("fix-to-top"); }
                        else { $("#miniBasket").removeClass("fix-to-top"); }
                        $('.cartopen').addClass('active'); $timeout(function () { $(".cartopen").removeClass("active"); $("#miniBasket").removeClass("fix-to-top"); }, 3000);
                    }
                }).finally(function () {
                });
        }
        function removeQuoteBasket() {
            $http.post(globalConfig.removeQuoteBasketUrl)
                .success(function (resp) {
                    location.reload();
                });
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
        function formReset(form) {
            if (form) {
                form.$setPristine();
                form.$setUntouched();
            }
        }   

        function initLookbooks(data) {
            gm.model = data;
            gm.allLooks = angular.copy(data);
        }
        function fetchLookbookByGroup() {
            if (gm.selectedGroup != undefined && gm.selectedGroup != '' && gm.selectedGroup != null) {
                if (gm.selectedGroup == "All") {
                    gm.model.DynamicLists = gm.allLooks.DynamicLists;
                }
                else {
                    gm.model.DynamicLists = gm.allLooks.DynamicLists.filter(i => i.DisplayGroupName == gm.selectedGroup);
                }
               
            }
           
        }
        function updateBasketQty(productId, newQty, oldQty, displayOrder) {
            gm.updateQty = 0;
            if (newQty == oldQty || !oldQty) {
                return gm.basketResponse;
            }
            else {
                if (newQty > oldQty) {
                    gm.updateQty = newQty - oldQty;
                } else {
                    gm.updateQty = -(oldQty - newQty);
                }
                gm.addToBasket(productId, gm.updateQty, displayOrder);
            }
        }

        function getRecommendations() {
            Recommendation.getRecommendation().then(function (resp) { 
                if (resp) {
                    gm.recomendations.products = resp;
                    $("#bubbleOption").modal();
                }
            });
            
        }
        function getAllcurrencyandCountries() {
            $http.post(globalConfig.getAllcurrencySetting)
                .success(function (data) {
                    if (data) {
                        gm.currencies = data.currencies;
                        gm.countries = data.countries;
                        gm.langCulture = data.languages;
                    }
                })
                .error(function (msg) {
                })
                .finally(function () {
                });
        };

        function languageSettings(langculture) {
            var data = { language: langculture };
            $http.post(globalConfig.languageSettingUrl, data)
                .success(function () {
                    window.location.reload();
                })
                .error(function (msg) {
                })
                .finally(function () {
                });
        };
        function updateBasketSubscriptionInfo(subscriptionModel) {
            if (subscriptionModel != null) {
                var userSetting = null;
                angular.forEach(gm.basketResponse.lineItems, function (lineItem) {
                    if (lineItem.isSubscription) {
                        //when trigger is Fixed
                        if (subscriptionModel.selectedTerm.id != null && subscriptionModel.selectedPricing != null) {
                            userSetting = {
                                productId: lineItem.productId,
                                subscriptionUserSettings: {
                                    subscriptionPlanId: subscriptionModel.selectedTerm.subscriptionPlanId,
                                    subscriptionTermId: subscriptionModel.selectedTerm.id,
                                    userPricingType: subscriptionModel.selectedPricing.type,
                                    subscriptionJson: JSON.stringify(subscriptionModel)
                                }
                            };
                        }
                        if (userSetting != null) {
                            $http.post("/basket/UpdateBasketSubscriptionInfo", {
                                basketId: gm.basketResponse.id,
                                model: userSetting
                            }).then(function (success) {
                                gm.initBasket(success.data.result);
                            }, function (error) {
                            });
                        }
                    }

                });
            }
        };
    };
})();
