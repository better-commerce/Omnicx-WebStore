(function () {
    'use strict';
    window.app.controller('productCtrl', productCtrl);
    productCtrl.$inject = ['$scope', '$timeout', 'productConfig', 'model', '$http', '$q', 'loader'];

    function productCtrl($scope, $timeout, productConfig, model, $http, $q) {
        var pm = this;
        pm.model = model;
        pm.addToBag = false;
        pm.productResponse = {};
        pm.searchCriteria = {};
        pm.itemsPerPage = 12;
        pm.currentPage = 0;
        pm.productCount = 1;
        pm.productTosearch = productTosearch;
        pm.searchproductfilter = [];
        pm.initProducts = initProducts;
        pm.initCollectionProducts = initCollectionProducts;
        pm.searchProducts = searchProducts;
        pm.searchAddProduct = searchAddProduct;
        pm.clearAddProduct = clearAddProduct;
        //pm.variantDetail = variantDetail;
        pm.productDetail = productDetail;
        pm.productQty = productQty;
        pm.initBrandListing = initBrandListing;
        pm.filterBrandListing = filterBrandListing;
        pm.fetchSubBrandProductList = fetchSubBrandProductList;
        pm.filterBrands = filterBrands;
        pm.getUrl = getUrl;
        pm.paging = paging;
        pm.temp = 0;
        pm.addToWishlist = addToWishlist;
        pm.wishlistexistserror = false;
        pm.WishlistFilter = { id: '' };
        pm.wishlistsaved = false;
        pm.wishlisterror = false;
        pm.removeFilter = removeFilter;
        pm.selectionGroup = []
        pm.selectedKey = ""
        pm.item = [];
        $scope.noRecord = false;
        pm.subBrandProducts = [];
        pm.selectedRecord = '';
        pm.setRating = setRating;
        pm.removeChar = removeChar;
        pm.userLogin = userLogin;
        pm.registration = registration;
        pm.reviewAsGuest = reviewAsGuest;
        pm.cancel = cancel;
        pm.addReview = addReview;
        pm.configRating = configRating;
        pm.addMultipleToWishlist = addMultipleToWishlist;
        pm.addMultipleToBasket = addMultipleToBasket;
        pm.showAllbrand = false;
        pm.hideQickView = hideQickView;
        pm.subBrandPagination = subBrandPagination;
        pm.initProductVariant = initProductVariant;
        pm.getToWishlist = getToWishlist;
        pm.checkForWishlist = checkForWishlist;
        pm.getBasketRelatedProducts = getBasketRelatedProducts;
        pm.getAvailableAttributeValues = getAvailableAttributeValues;
        pm.relatedProducts = [];
        pm.GetDynamicReviewConfig = GetDynamicReviewConfig;
        pm.beginQuestionnaire = beginQuestionnaire;
        pm.submitSurvey = submitSurvey;
        pm.rotateImage = rotateImage;
        pm.degree = 0;
        pm.viewGridOrList = viewGridOrList;
        pm.getViewBycookie = false;
        pm.onTextFocus = onTextFocus;     
        pm.checkForSpecificAttributeinProductList = checkForSpecificAttributeinProductList;
        //pm.setVariantDetail = setVariantDetail;

        function getBasketRelatedProducts(basketId) {
            $http.post(productConfig.basketRelatedProducts + '/' + basketId).success(function (resp) {
                if (resp != null) {
                    pm.relatedProducts = resp;
                }
            })          
        };
        function checkForSpecificAttributeinProductList(attributeCode, attributevalue, grp, list) {
            var flag = false;
            if (list != null) {
                angular.forEach(list, function (line) {
                    if (line.groupName.includes(grp)) {
                        var attr = line.attributes
                        if (attr != null) {
                            for (i = 0; i < attr.length; i++) {
                                // this can be a string or null
                                var FieldCode = attr[i].key;
                                var FieldValue = attr[i].value;
                                if (FieldCode != null && FieldCode != "" && FieldCode == attributeCode) {
                                    //line.customInfo1 = FieldCode + "" + FieldValue;
                                    if (FieldValue != null && FieldValue == attributevalue) {
                                        flag = true;
                                        break;
                                    }
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


        function initProducts(responseModel) {
            pm.productResponse = responseModel;
            pm.searchCriteria = responseModel.searchCriteria;
            pm.itemsPerPage = pm.searchCriteria.pageSize;
            pm.currentPage = pm.searchCriteria.currentPage;
            pm.sortByList = responseModel.sortList;
            pm.searchCriteria.sortBy = responseModel.sortBy;
            //pm.getToWishlist(pm.productResponse.results);
            if ($.cookie("getView") === 'true' || $.cookie("getView") === undefined) 
                pm.getViewBycookie = true;
            else 
                pm.getViewBycookie = false;

            if (pm.productResponse.results == null || pm.productResponse.results.length == 0)
                $scope.noRecord = true;
        };
        function initCollectionProducts(responseModel) {
            pm.productResponse = responseModel.collectionResult;
            if (pm.productResponse == null || pm.productResponse.results == null)
                $scope.noRecord = true;
            if (pm.productResponse != null) {
                pm.searchCriteria = responseModel.collectionResult.searchCriteria;
                pm.itemsPerPage = pm.searchCriteria.pageSize;
                pm.currentPage = pm.searchCriteria.currentPage;
                pm.sortByList = responseModel.collectionResult.sortByList;

            }
        };
        function removeFilter(filter) {
            if (filter.freeText == undefined) {
                productTosearch({ key: filter.key, name: filter.name }, { name: filter.value, isSelected: false });
            } else {
                pm.searchCriteria.freeText = '';
                pm.searchAddProduct();
            }
        }
        function productTosearch(filter, item) {
            pm.selectedKey = '';
            if (item.isSelected) {
                pm.selectedKey = filter.key;
                pm.searchproductfilter.push({ "key": filter.key, "value": item.name, "isSelected": true, name: filter.name });
            }
            else {
                for (var i = 0; i < pm.searchproductfilter.length; i++) {
                    if (pm.searchproductfilter[i].key == filter.key && pm.searchproductfilter[i].value == item.name) {
                        pm.searchproductfilter.splice(i, 1);
                    }
                }
            }
            pm.selectionGroup = [];
            for (var i = 0; i < pm.searchproductfilter.length; i++) {
                if (pm.selectionGroup.indexOf(pm.searchproductfilter[i].name) == -1) {
                    pm.selectionGroup.push(pm.searchproductfilter[i].name);
                }
            }
            pm.searchAddProduct();
        };


        function searchAddProduct() {
            pm.searchfilter = { "currentPage": 1, "collectionId": pm.searchCriteria.collectionId, "pageSize": pm.searchCriteria.pageSize, "filters": pm.searchproductfilter, "freeText": pm.searchCriteria.freeText, "sortBy": pm.searchCriteria.sortBy, "allowFacet": "true", "categoryIds": pm.searchCriteria.categoryIds, "brandId": pm.searchCriteria.brandId, "facet": pm.searchCriteria.facet, "categoryId": pm.searchCriteria.categoryId };
            pm.searchProducts(pm.searchfilter);
        };


        function clearAddProduct(fieldId) {
            pm.selectedKey = '';
            pm.selectionGroup = [];
            $scope.fieldId = false;
            for (var i = 0; i < pm.searchproductfilter.length; i++) {
                if (pm.searchproductfilter[i].key == fieldId) {
                    pm.searchproductfilter.splice(i, 1);
                    i = i - 1;
                } else {
                    if (pm.selectionGroup.indexOf(pm.searchproductfilter[i].name) == -1) {
                        pm.selectionGroup.push(pm.searchproductfilter[i].name);
                    }
                }
            }

            pm.searchAddProduct();
        };

        function paging(pagination) {
            pm.searchCriteria = { "filters": pm.searchproductfilter, "currentPage": pagination.currentPage, "pageSize": pagination.pageSize, "categoryIds": pm.searchCriteria.categoryIds, "categoryId": pm.searchCriteria.categoryId };
            Products(pm.searchCriteria);
        };

        function productDetail(id) {
            $scope.productmodal = false;
            $http.post(productConfig.productUrl, { id: id })
                .success(function (data) {
                    pm.model = data;                    
                    pm.variants = data != null ? data.variant : null;
                    if (pm.variants != null) {
                        pm.initProductVariant(pm.variants);
                    }
                    if (data != null)
                        pm.productId = data.recordId;
                    $scope.productmodal = true;
                    pm.model.bulkQty = 1;
                    $timeout(function () { $("#qtyBox").focus().select(); $("#qtyBoxBundle").focus().select(); }, 2000);                   
                })
                .error(function (msg) {
                    pm.errorMessage = msg.errorMessages;
                })
                .finally(function () {
                    pm.saving = false;
                });
        };

        function searchProducts(searchFilter) {
            $http.post(productConfig.searchProductUrl, searchFilter)
                .success(function (data) {
                    pm.productResponse.results = data.results;
                    if (typeof (data.groups) !== undefined && data.groups !== null) {                    
                        pm.productResponse.productGroupModel.groups = data.groups;
                    }
                    angular.forEach(pm.productResponse.results, function (pro) {
                        pro.bulkQty = 1;
                    });
                    pm.productResponse.total = data.total;
                    pm.searchCriteria = data.searchCriteria;
                    pm.itemsPerPage = pm.searchCriteria.pageSize;
                    pm.currentPage = pm.searchCriteria.currentPage;
                    if (data.results.length == 0)
                        $scope.noRecord = true;
                    else
                        $scope.noRecord = false;
                    var filter = {};
                    angular.forEach(pm.productResponse.filters, function (value, i) {
                        if (pm.selectedKey == value.key) {
                            filter = value;
                        }
                    });
                    pm.productResponse.filters = data.filters;
                    //pm.productResponse.filters = [];
                    //angular.forEach(data.filters, function (value) {
                    //    if ( pm.selectedKey== value.key) 
                    //        pm.productResponse.filters.push(filter);
                    //    else
                    //        pm.productResponse.filters.push(value);

                    //});
                    //pm.selectedKey 
                    //pm.productResponse.filters = data.filters;
                    //if (pm.temp == 1 || pm.searchCriteria.filters == null) {
                    //    pm.productResponse.filters = data.filters;
                    //    pm.temp = 0;
                    //}
                })
                .error(function (msg) {

                })
                .finally(function () {
                });
        };

        function addToWishlist(id) {
            pm.WishlistFilter.id = id;
            $http.post(productConfig.addToWishlistUrl, pm.WishlistFilter).success(function (resp) {
                $(".wishdiv").fadeIn();
                if (resp) {
                    pm.wishlistsaved = true;
                }
                else {
                    pm.wishlistexistserror = true;
                    pm.wishlisterror = false;
                }
                $timeout(function () {
                    $(".wishdiv").fadeOut();
                    pm.wishlistsaved = false;
                    pm.wishlistexistserror = false;
                }, 2000);
            })
                .error(function (msg) {
                    $(".wishdiv").fadeIn();
                    pm.wishlisterror = true;
                    $timeout(function () {
                        $(".wishdiv").fadeOut();
                    }, 2000);
                })
                .finally(function () {
                    pm.getToWishlist(pm.productResponse.results);
                    pm.checkForWishlist();
                });
        };



        function initBrandListing() {
            $('#brandContainer').html($('#ALL').html());
            $('#brandContainer').quicksand($('#ALL').find('div[parent=1]'), { duration: 1, attribute: 'data-id', adjustHeight: 'dynamic' });
        };

        function filterBrandListing(letter) {
            if (letter == 'ALL') {
                pm.showAllbrand = true;
                $('#brandContainer').html($('#ALL').html());
                $('#brandContainer').quicksand($('#ALL').find('div[parent=1]'), { duration: 1, attribute: 'data-id', adjustHeight: 'dynamic' });
            }

            else {
                pm.showAllbrand = false;
                var test = $('div:contains("' + letter + '")');
                $('#brandContainer').quicksand($('#ALL').find('div[firstchar="' + letter + '"]'), { duration: 1, attribute: 'data-id', adjustHeight: 'dynamic' });
                pm.result = $('#ALL').find(test);
            }
        };

        function filterBrands(letter) {
            if (letter.length <= 1 && letter != null) return;
            pm.selectedRecord = letter;
            if (letter == '') {
                pm.showAllbrand = true;
                $('#brandContainer').html($('#ALL').html());
                $('#brandContainer').quicksand($('#ALL').find('div[parent=1]'), { duration: 1, attribute: 'data-id', adjustHeight: 'dynamic' });
            }

            else {
                pm.showAllbrand = false;
                if (letter == '#') { letter = ''; }
                letter = letter.toUpperCase();
                letter = letter.charAt(0).toLowerCase() + letter.slice(1);
                if (!isNaN(letter.charAt(0)) || letter.indexOf('+') !== -1) {
                    letter = '#' + letter;
                }
                var test = $('div:contains("' + letter + '")');
                $('#brandContainer').quicksand($('#ALL').find(test), { duration: 1, attribute: 'data-id', adjustHeight: 'dynamic' });
                pm.result = $('#ALL').find(test);
            }

        };
        function getUrl() {
            pm.url = window.location.origin;
        };

        function fetchSubBrandProductList(subBrand, parentBrand) {
            $http.post(productConfig.fetchSubBrandProducts, { id: parentBrand, subBrandId: subBrand }).success(function (resp) {
                pm.subBrandProducts = resp[0].products;
                pm.subBrandProducts.currentPage = 1;
                pm.subBrandProducts.pageSize = 12;
                pm.subBrandProducts.total = pm.subBrandProducts.length;
                pm.subBrandPaginated = pm.subBrandProducts.slice(0, pm.subBrandProducts.pageSize);
                //$timeout(function () { $(".wishdiv").fadeOut(); }, 2000);
            })
                .error(function (msg) {
                    //pm.wishlisterror = true;
                    //$timeout(function () {
                    //    $(".wishdiv").fadeOut();
                    //}, 2000);
                })
                .finally(function () {
                });

        };

        function subBrandPagination(subBrandProducts) {

            pm.subBrandPaginated = pm.subBrandProducts.slice((subBrandProducts.pageSize * subBrandProducts.currentPage) - subBrandProducts.pageSize, subBrandProducts.pageSize * subBrandProducts.currentPage);
        }

        function setRating(value, isActive) {
            pm.rate = [];
            if (isActive == 1) {
                for (var i = 1; i <= value; i++) {
                    pm.rate.push(i);
                }
            }
            else {
                for (var i = value; i < 5; i++) {
                    pm.rate.push(i);
                }
            }
            return pm.rate;

        };
        function removeChar(fielId, name) {
            var value = fielId.replace(/[^a-zA-Z0-9]/g, '') + name.replace(/[^a-zA-Z0-9]/g, '');
            return value;
        };

        function registration(model) {
            pm.errorMessage = null;
            pm.registerErrors = true;
            pm.loginErrors = false;
            $http.post(productConfig.register, model)
                .success(function (data) {
                    if (data) {
                        $("#productReviewLogin").modal('hide');
                        pm.writeReview = true;
                        pm.reviewAsGuest();
                    }
                })
                .error(function (msg) {
                    pm.errorMessage = msg.errorMessages;
                })
                .finally(function () {
                    $timeout(function () { $(".alertBlock").fadeOut(); }, 3000);
                });
        };

        function userLogin(model) {
            pm.errorMessage = null;
            pm.loginErrors = true;
            pm.registerErrors = false;
            $(".alertBlock").fadeIn();
            $http.post(productConfig.signIn, model)
                .success(function (data) {
                    if (data) {
                        $("#productReviewLogin").modal('hide');
                        pm.writeReview = true;
                        pm.reviewAsGuest();
                    }
                })
                .error(function (msg) {
                    pm.errorMessage = msg.errorMessages;
                    $timeout(function () {
                        $(".alertBlock").fadeOut();
                    }, 3000);

                })
                .finally(function () {
                });
        };

        function reviewAsGuest() {
            $('html,body').animate({ scrollTop: 980 }, 'slow');
            $("#collapse1").removeClass("in");
            $("#collapse3").addClass("in");
        };

        function cancel() {
            $('html,body').animate({ scrollTop: 300 }, 'slow');
            $("#collapse3").removeClass("in");
            $("#collapse1").addClass("in");
        };

        function addReview(id, model) {
            if ($scope.reviewForm.$invalid) {
                return;
            }
            model.reviewSections = pm.reviewSections;
            model.section = pm.reviewSections[0];
            $http.post(productConfig.addReview, { id: id, productReview: model })
                .success(function (data) {
                    $("#SuccessAlert").modal();
                    $scope.reviewForm.$setPristine();
                    $scope.reviewForm.$setUntouched();
                    pm.model.review = {};
                    pm.writeReview = false;
                    $timeout(function () {
                        $("#SuccessAlert").modal('hide');
                        $("#collapse3").removeClass("in");
                        $("#collapse1").addClass("in");
                    }, 2000);
                    $timeout(function () {
                        window.location.reload();
                    }, 3000);
                })
                .error(function (msg) {
                    //pm.errorMessage = msg.errorMessages;
                    //$timeout(function () {
                    //    $(".alertBlock").fadeOut();
                    //}, 3000);

                })
                .finally(function () {
                });
        };
        function configRating(value) {
            pm.trueRating = pm.setRating(value, 1);
            pm.falseRating = pm.setRating(5 - value, 1);
        };

        function GetDynamicReviewConfig() {
            $http.post(productConfig.reviewConfig).then(function (success) {
                pm.reviewSections = success.data;
            }, function (error) { });
        }

        function addMultipleToWishlist(products, isMultiple) {
            pm.modelList = [];
            if (isMultiple == 1) {
                pm.modelList.push({ "productId": products.productId, "qty": 0, "displayOrder": 0 });
            }
            else {
                for (var i = 0; i < products.length; i++) {
                    pm.modelList.push({ "productId": products[i].productId, "qty": 0, "displayOrder": 0 });
                }
            }
            $http.post(productConfig.basketToWishlist, pm.modelList).success(function (resp) {
                if (resp) {
                    pm.wishlistsaved = true;
                }
                else {
                    pm.wishlistexistserror = true;
                }
                $("html, body").animate({ scrollTop: 0 }, "slow");
                $timeout(function () { window.location.reload(); }, 3000);
            })
                .error(function (msg) {
                })
                .finally(function () {
                    pm.getToWishlist(pm.productResponse.results);
                });
        };
        function checkForWishlist() {
            $http.post(productConfig.getWishlist).success(function (resp) {
                if (resp != null && pm.productId != null) {
                    angular.forEach(resp, function (resp, key) {
                        if (pm.productId == resp.recordId) {
                            pm.model.inWishList = true;
                        }
                    });
                }
            })
              .finally(function () {
              });
        };

        function getToWishlist(products) {
            $http.post(productConfig.getWishlist).success(function (resp) {
                if (resp != null && products != null) {
                    angular.forEach(products, function (product, key) {
                        angular.forEach(resp, function (resp, key) {
                            if (product.id == resp.id) {
                                product.inWishList = true;
                            }
                        });
                    });
                }
            })
                .finally(function () {
                });
        };
        function addMultipleToBasket(products) {
            pm.modelList = [];
            for (var i = 0; i < products.length; i++) {
                pm.modelList.push({ "productId": products[i].recordId, "qty": 1, "displayOrder": 0 });
            }
            $http.post(productConfig.wishlistToBasket, pm.modelList).success(function (resp) {
                window.location.reload();
            })
                .error(function (msg) {
                })
                .finally(function () {
                });
        };

        function hideQickView() {
            $("#product-quick-view-modal").modal("hide");
        }

        //gets the possible attribute values for the selected attribute & its value. Uses the pm.model.variantProducts
        //sample call getAvailableAttributeValues('size', '6', products);      //=> {size: ['6'], color: ['black', 'blue']}
        function getAvailableAttributeValues(attrCode, attrValue, isSelected, independentUrl) {
            pm.addToBag = false;
            if (isSelected) { return; }
            var selectedValues = [];
            angular.forEach()
            //1. Set attributes available = false & selected = false for all 
            pm.model.variantProductsAttribute.forEach(function (attr) {
                attr.fieldValues.forEach(function (val) {
                    val.available = false;
                    if (attr.fieldCode == attrCode) {
                        if (val.fieldValue == attrValue) {
                            val.selected = true;
                        } else {
                            val.selected = false;
                        }
                    }
                    if (val.selected == true)
                        selectedValues.push(val.fieldValue);
                })
            });

            //2. pick up all the proudcts that match the attrCode & attrValue passed. 
            var availableAttrs = [];
            var matchedProd = [];
            pm.model.variantProducts.forEach(function (prod) {
                var matchedAttrAndValue = prod.variantAttributes.filter(function (pAttr) {
                    return pAttr.fieldCode === attrCode & pAttr.fieldValue === attrValue;
                })

                if (matchedAttrAndValue.length > 0) {

                    //console.log(MoviesCtrl(selectedValues, prod.variantAttributes));
                    //if (selectedValues.every(function (val) { return prod.variantAttributes.indexOf(val) >= 0; }))
                    //    matchedProd.push(prod);
                    var matchedAttribute = [];
                    prod.variantAttributes.forEach(function (pAttr) {
                        selectedValues.filter(function (pAttr1) {
                            pAttr.selected = pAttr1;
                            if (pAttr1 === pAttr.fieldValue) { matchedAttribute.push(pAttr1) } return matchedAttribute;
                        })
                        //TODO: check that the same attribute & value does not exist already in the array before pushing it in
                        availableAttrs.push(pAttr);
                    })
                    if (matchedAttribute.length == prod.variantAttributes.length)
                    { matchedProd.push(prod); }
                }
            });

            //3. pick up all the attributes + values (other than the clicked one) from products and set their isAvailable = true in MasterAttributes array
            pm.model.variantProductsAttribute.forEach(function (attr) {
                //the attribute passed in selection, shoudl have all the values as "avaialble" and clicked item to be "selected"
                if (attr.fieldCode === attrCode) {
                    attr.fieldValues.forEach(function (val) {
                        val.available = true;
                        if (val.fieldValue == attrValue) val.selected = true;
                        if (val.selected == true) {
                            if (val.available == true) {
                                attr.selectedValue = val.fieldValue;

                            }
                            else {
                                attr.selectedValue = "";
                                pm.addToBag = true;
                            }
                        }
                    });
                }
                else {
                    attr.fieldValues.forEach(function (val) {
                        if (availableAttrs.filter(function (availAttr) {
                            return availAttr.fieldCode === attr.fieldCode & availAttr.fieldValue === val.fieldValue;
                        }).length > 0) {
                            val.available = true;
                        }
                        if (val.selected == true) {
                            if (val.available == true) {
                                attr.selectedValue = val.fieldValue;

                            }
                            else {
                                attr.selectedValue = "";
                                pm.addToBag = true;
                            }
                        }
                    })
                }
            });

            if (matchedProd.length > 0) {
                $scope.productId = matchedProd[0].productId;
                $http.post(productConfig.productUrl + "/" + $scope.productId).success(function (resp) {
                    if (resp != null) {
                        if (independentUrl) {
                            window.location.href = "/" + resp.link;
                        }
                        else {
                            pm.productId = $scope.productId;

                            var attributes = pm.model.variantProductsAttribute;
                            var variantProducts = pm.model.variantProducts;
                            pm.model = resp;
                            pm.model.variantProductsAttribute = attributes;
                            pm.model.variantProducts = variantProducts;
                            pm.image = pm.model != null ? pm.model.image : null;
                        }
                    }
                });
            }
        }

        function initProductVariant() {
            pm.model.variantProductsAttribute.forEach(function (attr) {
                attr.fieldValues.forEach(function (val)
                { val.available = true; })
            });
            var selectedProduct = pm.model.variantProducts.filter(function (p) { return p.stockCode === pm.model.stockCode; })[0];
            if (selectedProduct != undefined) {
                selectedProduct.variantAttributes.forEach(function (pAttr) {

                    var attrFromMaster = pm.model.variantProductsAttribute.filter(function (attr) { return attr.fieldCode === pAttr.fieldCode })[0];
                    var matchedAttr = attrFromMaster.fieldValues.filter(function (fieldVal) { return fieldVal.fieldValue === pAttr.fieldValue })[0];
                    matchedAttr.selected = true;
                    attrFromMaster.selectedValue = matchedAttr.fieldValue;
                });
            }
        }

        function productQty(qty) {
            if (pm.productCount == 1 && qty == -1) { return; }
            pm.productCount = parseInt(pm.productCount) + qty;
        }

        function beginQuestionnaire(productId, questionnaireCode) {
            pm.model.survey = {};
            $http.post(productConfig.questionnaireCode, { questionnaireCode: questionnaireCode }).success(function (resp) {
                pm.model.survey = resp;
                $('#QualifyingQuestionnaire').modal('show');
            });
        }
        function viewGridOrList(gridorlist) {
            $.cookie("getView", gridorlist);
        }

        function submitSurvey(productId) {

            //this is where we submit the whole survey using AJAX call.
            pm.surveyCompleted = true;
            var answers = []; var multipleAnswers = "";
            pm.model.survey.result.wrongOptionCount = 0;
            //save answers in an array
            angular.forEach(pm.model.survey.result.questions, function (value, key) {
                multipleAnswers = "";
                if (value.selectedOptionValue == "" || value.showHelpText == true) {
                    pm.model.survey.result.wrongOptionCount = (pm.model.survey.result.wrongOptionCount + 1);
                }
                if (multipleAnswers != "") {
                    answers.push({ "questionId": value.recordId, "question": value.question, "selectedAnswer": multipleAnswers });
                }
                else {
                    answers.push({ "questionId": value.recordId, "question": value.question, "selectedAnswer": value.selectedOptionValue });
                }
            });

            //save survey.
            if (pm.model.survey.result.wrongOptionCount == 0) {
                $http.post("/Survey/SaveAnswerBulk", { surveyId: pm.model.survey.result.recordId, answers: answers, productId: productId }).then(function (success) {
                    $('#QualifyingQuestionnaire').modal('hide');
                    pm.model.canAddToBag = true;
                }, function (error) { })
            }
        }

        function rotateImage(deg) {
            if (pm.degree == 270)
            {
                $(".magnify-image").removeClass("rotate" + pm.degree);
                pm.degree = 0;
            }
            else {
                $(".magnify-image").removeClass("rotate" + pm.degree);
                pm.degree += deg;
                $(".magnify-image").addClass("rotate" + pm.degree);
            }
         
        }
        function onTextFocus(event) {
            event.target.select();
        }

    };
})();