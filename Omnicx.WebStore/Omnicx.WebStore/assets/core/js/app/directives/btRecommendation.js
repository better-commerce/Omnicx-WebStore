(function () {
    'use strict';   
    angular.module('btRecommendation', [])
        .directive('btRecommendation', function ($parse, $http, $sce, $timeout) {
            return {
                restrict: 'EA',
                scope: {
                    "itemId": "@itemid",
                    "modelId": "@modelid",
                    "type": "@type",
                    "title": "@title",
                    "noOfItems": "@noofitems",
                    "showModel": "@showmodel"
                },
                templateUrl: function (elem, attrs) {
                    return attrs.showmodel == "true" ? '/assets/core/js/app/directives/templates/recommendation.html': '/assets/core/js/app/directives/templates/recommendproducts.html'
                },                                                                                     
                link: function ($scope, elem, attrs) {
                    $scope.itemId = null;
                    $scope.title = null;                 
                    $scope.modelId = null;
                    $scope.type = elem.type; 
                    $scope.noOfItems = attrs.noofitems;
                    $scope.showModel = attrs.showmodel;
                    $scope.recommendProducts = null;                     
                    var RECENT_PRODUCT_COOKIE = '_rvp';
                    $scope.recommendTypes = { Home: "Home", Product: "Product", Basket: "Basket", Personalised: "Personalised", Promotion: "Promotion", Order:"Order", Promotion: "Promotion", NewForYou: "NewForYou", Popular: "Popular", RecentView:"RecentView" };
                   
                    $scope.getRecommendations = function (type) { 
                        $scope.type = type;
                        var recentViewedProductList = "";
                        if (type == $scope.recommendTypes.RecentView) {
                            var recentViewedProducts = $.cookie(RECENT_PRODUCT_COOKIE);
                            if (recentViewedProducts) {
                                var recentViewedProductList = recentViewedProducts.split(",");
                            } 
                        }                                              
                        $http.post('/Recomendation/GetItemRecommendations', { itemId: $scope.itemId, recentViewedProductList: recentViewedProductList, recommedType: $scope.type, modelId: $scope.modelId, noOfItems: $scope.noOfItems }
                        ).success(function (resp) {
                            console.log(resp);
                            $scope.recommendProducts = resp;
                            if ($scope.showModel=="true")
                                $("#bubbleOption").modal();
                            }).error(function (err) { console.log(err)});                       

                    }
                    if (attrs.showmodel == "false") {
                        $scope.getRecommendations(attrs.type);
                    }
                   
                }
            };
        });

}());