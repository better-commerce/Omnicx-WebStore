(function () {
    'use strict';
    window.app.controller('b2bCtrl', b2bCtrl);
    b2bCtrl.$inject = ['$scope', 'b2bConfig', '$http', '$timeout', 'model', 'globalConfig','scriptLoader'];

    function b2bCtrl($scope, b2bConfig, $http, $timeout, model, globalConfig, scriptLoader) {
        var b2b = this;
        b2b.model = model;
        b2b.saving = false;
        b2b.saveCustomerDetail = saveCustomerDetail;
        b2b.getQuoteDetail = getQuoteDetail;
        b2b.emptyGuid = '00000000-0000-0000-0000-000000000000';
        b2b.sendEmail = sendEmail;
        b2b.updateQuoteBasket = updateQuoteBasket;
        b2b.updateQtyAndAdd = updateQtyAndAdd;
        b2b.editQuoteInfo = editQuoteInfo;
        b2b.saveQuoteInfo = saveQuoteInfo;
        b2b.changeAddress = changeAddress;

         function initPCALookup() {
            if (globalConfig.pcaAccessCode != undefined && globalConfig.pcaAccessCode != '') {
                window.setTimeout(function () {
                    if (!b2b.defaultCountry) {
                        $http.post(globalConfig.getDefaultCountryUrl)
                            .success(function (country) { b2b.defaultCountry = country; });
                    }
                    // BillingAddress PCA Predict
                    var optionsBilling = {
                        key: globalConfig.pcaAccessCode,
                        countries: {
                            codeList: b2b.defaultCountry
                        }
                    };

                    var fieldsBilling = [
                        { element: 'b2b.quoteInfo.billingAddress.address1', field: 'Line1' },
                        { element: 'b2b.quoteInfo.billingAddress.address2', field: 'Line2', mode: pca.fieldMode.POPULATE },
                        { element: 'b2b.quoteInfo.billingAddress.city', field: 'City', mode: pca.fieldMode.POPULATE },
                        { element: 'b2b.quoteInfo.billingAddress.state', field: 'Province', mode: pca.fieldMode.POPULATE },
                        { element: 'b2b.quoteInfo.billingAddress.postCode', field: 'PostalCode' }
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

                    //ShippingAddress PCA Predict
                    var optionsShip = {
                        key: globalConfig.pcaAccessCode,
                        countries: {
                            codeList: b2b.defaultCountry
                        }
                    };
                    var fieldsShip = [
                        { element: 'b2b.quoteInfo.shippingAddress.address1', field: 'Line1' },
                        { element: 'b2b.quoteInfo.shippingAddress.address2', field: 'Line2', mode: pca.fieldMode.POPULATE },
                        { element: 'b2b.quoteInfo.shippingAddress.city', field: 'City', mode: pca.fieldMode.POPULATE },
                        { element: 'b2b.quoteInfo.shippingAddress.state', field: 'Province', mode: pca.fieldMode.POPULATE },
                        { element: 'b2b.quoteInfo.shippingAddress.postCode', field: 'PostalCode' }
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

                }, 1500);
            }
        }

        function saveCustomerDetail(model) {
            $scope.changepass = false;
            $scope.personalDetail = true;
            b2b.saving = true;
            b2b.errorMessage = null;
            b2b.success = false;
            $(".alertBlock").fadeIn();
            $http.post(b2bConfig.saveCustomerUrl, model)
                .success(function () {
                    b2b.success = true;
                    window.location.reload();
                })
                .error(function (msg) {
                    b2b.errorMessage = msg.errorMessages;
                })
                .finally(function () {
                    b2b.saving = false;
                    $timeout(function () { $(".alertBlock").fadeOut(); }, 3000);
                });
        };      

        function getQuoteDetail(quoteId,quoteNo,quoteName) {
            $http.post(b2bConfig.quoteDetailUrl, { quoteId : quoteId}).success(function (basket) {
                b2b.quoteDetail = basket;
                b2b.customQuoteNumber = quoteNo;
                b2b.quoteName = quoteName;
                $("#quoteDetailModal").modal();
            });
        }
        function sendEmail() {
            $http.post(b2bConfig.requestQuoteChangeUrl, { quoteNo: b2b.customQuoteNumber }).success(function (resp) {
                b2b.emailSent = resp;
                $('.emailalert').show(0).delay(2000).hide(0);
            });
        }

        function updateQuoteBasket(recordId, qty, displayOrder) {
            var prod = { "basketId": b2b.quoteDetail.id, "productId": recordId, "qty": qty, "displayOrder": (displayOrder >= 0) ? displayOrder : b2b.quoteDetail.lineItems.length + 1  };
            $http.post(b2bConfig.addToBasketUrl, prod)
                .success(function (data) {
                    b2b.quoteDetail = data;
                })
                .error(function (msg) {
                })
                .finally(function () {
                });
        }
        function updateQtyAndAdd(productId, newQty, oldQty, displayOrder) {
            b2b.updateQty = 0;
            if (newQty == oldQty || !oldQty) { return; }
            else {
                if (newQty > oldQty) {
                    b2b.updateQty = newQty - oldQty;
                } else {
                    b2b.updateQty = -(oldQty - newQty);
                }
                b2b.updateQuoteBasket(productId, b2b.updateQty, displayOrder);
            }
        };
        function editQuoteInfo() {
            b2b.quoteInfo = [];
            if (globalConfig.pcaAccessCode != undefined && globalConfig.pcaAccessCode != '') {
                scriptLoader.load("//services.postcodeanywhere.co.uk/css/captureplus-2.30.min.css?key=" + globalConfig.pcaAccessCode, "text/css", "stylesheet");
                scriptLoader.load("//services.postcodeanywhere.co.uk/js/captureplus-2.30.min.js?key=" + globalConfig.pcaAccessCode, "text/javascript", "");
            }
            window.setTimeout(function () {
                initPCALookup()
            }, 1500);
            $('#EditQuoteAddress').modal();
            b2b.quoteInfo.billingAddress = b2b.quoteDetail.billingAddress;
            b2b.quoteInfo.shippingAddress = b2b.quoteDetail.shippingAddress;
            b2b.quoteInfo.quoteName = b2b.quoteName;
            $http.post(b2bConfig.custGridUrl)
                .success(function (data) {
                    if (data != null && data.length > 0) {
                        b2b.userAddresses = data;
                    }
                })
        };
        function saveQuoteInfo(quoteInfo) {
            var quoteModel = {
                id: b2b.quoteDetail.id, billingAddress: quoteInfo.billingAddress, shippingAddress: quoteInfo.shippingAddress,
                customerId: b2b.quoteDetail.customerId, email: b2b.quoteDetail.email, quoteName: b2b.quoteInfo.quoteName, validUntil: b2b.quoteDetail.validUntil
            }
            $http.post(b2bConfig.createQuote, { quote: quoteModel }).success(function (basket) {
                location.reload();
            });
        }

        function changeAddress(changeFor, address) {     
            if (changeFor == 'delivery') {
                b2b.quoteInfo.shippingAddress = address;
            }
            if (changeFor == 'billing') {
                b2b.quoteInfo.billingAddress = address;
            }
        }
    }
})();