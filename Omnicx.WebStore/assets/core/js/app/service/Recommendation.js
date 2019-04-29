(function () {
    'use strict';
    /* Service: AdminData
     * Defines the methods related to global data across the app
     */
    window.app.factory('Recommendation', function ($http, $q, BASE_URL) {       
        var factory = {};
        var RECENT_PRODUCT_COOKIE = '_rvp';
        return {
            getRecommendation: function () {
                var deferred = $q.defer();
                var recentViewedProducts = $.cookie(RECENT_PRODUCT_COOKIE);
                if (recentViewedProducts) {
                    var recentViewedProductList = recentViewedProducts.split(",");
                }
                var pagePath = window.location.pathname;
                $http.post(BASE_URL + '/Recomendation/GetItemRecommendations', { itemId: dataLayer[0].EntityId, recentViewedProductList: recentViewedProductList, pageCategory: dataLayer[0].PageCategory }).success(deferred.resolve).error(deferred.reject);
                return deferred.promise;  
            }
        }
    });
}());