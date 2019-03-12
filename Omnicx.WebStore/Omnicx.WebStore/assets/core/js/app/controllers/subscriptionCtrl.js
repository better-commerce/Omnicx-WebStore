(function () {
    'use strict';
    window.app.controller('subscriptionCtrl', subscriptionCtrl);
    subscriptionCtrl.$inject = ['$scope', '$http', 'globalConfig', 'SUBSCRIPTION_ENUMS'];
    function subscriptionCtrl($scope, $http, globalConfig, SUBSCRIPTION_ENUMS) {
        var sm = this;
        //Variables
        sm.subscriptionPlan = null;
        //Methods
        sm.initSubscriptionPlan = initSubscriptionPlan;
        sm.addSubscriptionToBag = addSubscriptionToBag;
        function initSubscriptionPlan(productId, isSubscriptionEnabled) {

            if (isSubscriptionEnabled) {
                $http.get("/Subscription/GetSubscriptionPlan/?productId=" + productId).then(function (success) {
                    sm.subscriptionPlan = success.data.result;
                    //handling added when cart has subcription and normal product
                    if (sm.subscriptionPlan != null) {
                        if (sm.subscriptionPlan.planType == SUBSCRIPTION_ENUMS.SubscriptionPlanType.DynamicBundle) {
                            sm.dynamicBundleSubscriptionPlan = true;
                        }
                        else if (sm.subscriptionPlan.planType == SUBSCRIPTION_ENUMS.SubscriptionPlanType.Simple) {
                            sm.simpleSubcriptionPlan = true;
                        }
                    }
                    sm.subscriptionPlan.RecurringPayment = {
                        type: SUBSCRIPTION_ENUMS.UserPricingType.Recurring,
                        price: sm.subscriptionPlan.recurringFee
                    };
                    sm.subscriptionPlan.OneTimePayment = {
                        type: SUBSCRIPTION_ENUMS.UserPricingType.OneTime,
                        price: sm.subscriptionPlan.oneTimeFee
                    }
                }, function (error) { });
            }
        }
        function addSubscriptionToBag(productId, basketId, displayOrder, qty) {

            var model = {};
            //model when subscription is Simple Subscription
            if (sm.model != null && sm.model.selectedTerm.id != null && sm.model.selectedPricing != null) {

                model =
                    {
                        productId: productId, basketId: basketId,
                        subscriptionPlanId: sm.model.selectedTerm.subscriptionPlanId,
                        subscriptionTermId: sm.model.selectedTerm.id,
                        userSubscriptionPricing: sm.model.selectedPricing,
                        displayOrder: displayOrder
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

        function validateSubscription() {
            return true;
        }
    }   
})();