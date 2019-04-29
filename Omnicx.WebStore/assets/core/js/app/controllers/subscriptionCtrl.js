(function () {
    'use strict';
    window.app.controller('subscriptionCtrl', subscriptionCtrl);
    //window.app.constant('SUBSCRIPTION_CONSTANTS', {
    //    'SubscriptionPlanPricingType': {
    //        'Flat': 'Flat',
    //        'Term': 'Term',
    //        'PerUnit': 'PerUnit'
    //    },
    //    'UserPricingType': {
    //        'None': "None",
    //        'OneTime': "OneTime",
    //        'Recurring': "Recurring"
    //    },
    //    'SubscriptionPlanType': {
    //        'Simple': "Simple",
    //        'FixedBundle': "FixedBundle",
    //        'DynamicBundle': "DynamicBundle"
    //    },
    //});
    subscriptionCtrl.$inject = ['$scope', '$http', 'globalConfig', 'SUBSCRIPTION_ENUMS', 'SUBSCRIPTION_CONSTANTS'];
    function subscriptionCtrl($scope, $http, globalConfig, SUBSCRIPTION_ENUMS, SUBSCRIPTION_CONSTANTS) {
        var sm = this;
        //Variables
        sm.subscriptionPlan = null;
        sm.subscriptionModel = {};
        //Methods
        sm.initSubscriptionPlan = initSubscriptionPlan;
        sm.addSubscriptionToBag = addSubscriptionToBag;
        sm.initSubscription = initSubscription;
        sm.addDynamicBundleToBag = addDynamicBundleToBag;
        function initSubscriptionPlan(productId, isSubscriptionEnabled) {
            if (isSubscriptionEnabled) {
                $http.get("/Subscription/GetSubscriptionPlan/?productId=" + productId).then(function (success) {
                    sm.subscriptionPlan = success.data.result;
                    //select default term and pricing. 
                    sm.subscriptionModel.selectedTerm = sm.subscriptionPlan.terms.find(i => i.isDefault);
                    sm.subscriptionModel.selectedPricing = SUBSCRIPTION_CONSTANTS.UserPricingType.Recurring;
                }, function (error) { });
            }

        }
        function initSubscription(type, productId) {
            if (type == SUBSCRIPTION_CONSTANTS.SubscriptionPlanType.DynamicBundle) {
                initSubscriptionPlan(productId, true);
            }
        }
        function addSubscriptionToBag(productId, basketId, displayOrder, qty) {

            var model = {};
            //Validation for user cannot add subscription products more than maximum quantity saved as maxQty in Subscription Plan
            var validCount = true;
            var subscriptionItemInCart = 0;
            var maxSubscriptionTerm = 0;
            angular.forEach(sm.subscriptionPlan.terms, function (term) {
                if (maxSubscriptionTerm < term.subscriptionTerm.duration) {
                    maxSubscriptionTerm = term.subscriptionTerm.duration;
                }
            });
            angular.forEach($scope.$parent.$parent.gm.basketResponse.lineItems, function (item) {
                if (item.isSubscription) {
                    subscriptionItemInCart = subscriptionItemInCart + 1;
                    if (subscriptionItemInCart >= sm.subscriptionPlan.maxQty || subscriptionItemInCart >= maxSubscriptionTerm) {
                        validCount = false;
                        $(".wishdiv").fadeIn();
                        $scope.$parent.$parent.gm.maximumBasketsubscriptionItemError = true;
                        window.setTimeout(function () {
                            $(".wishdiv").fadeOut();
                            $scope.$parent.$parent.gm.maximumBasketsubscriptionItemError = false;
                        }, 3000);
                        return false;
                    }
                }

            });
            //model when subscription is Simple Subscription
            if (sm.model != null && sm.model.selectedTerm.id != null && sm.model.selectedPricing != null) {

                model =
                    {
                        productId: productId, basketId: basketId,
                        subscriptionPlanId: sm.subscriptionModel.selectedTerm.subscriptionPlanId,
                        subscriptionTermId: sm.subscriptionModel.selectedTerm.id,
                        userSubscriptionPricing: sm.subscriptionModel.selectedPricing,
                        displayOrder: displayOrder,
                        qty: qty
                    };
            }
            //model when subscription is Dynamic Subscription
            else {
                model =
                    {
                        basketId: basketId,
                        productId: productId,
                        subscriptionPlanId: sm.subscriptionPlan.recordId,
                        qty: qty
                    };
            }
            if(validCount){
            $http.post(globalConfig.addToBasket, model)
                .success(function (data) {
                    //update basket
                    $scope.$parent.$parent.gm.initBasket(data.result);
                })
                .error(function (msg) {

                })
                .finally(function () {

                });
            }
        }

        function addDynamicBundleToBag(productId, basketId, displayOrder, qty) {
          
            $http.get("/Subscription/GetSubscriptionPlan/?productId=" + productId).then(function (success) {
                sm.subscriptionPlan = success.data.result;
                //select default term and pricing. 
                sm.subscriptionModel.selectedTerm = sm.subscriptionPlan.terms.find(i => i.isDefault);
                sm.subscriptionModel.selectedPricing = SUBSCRIPTION_CONSTANTS.UserPricingType.Recurring;
                addSubscriptionToBag(productId, basketId, displayOrder, qty);
            }, function (error) { });   
        };

        function validateSubscription() {
            return true;
        }
    }
})();