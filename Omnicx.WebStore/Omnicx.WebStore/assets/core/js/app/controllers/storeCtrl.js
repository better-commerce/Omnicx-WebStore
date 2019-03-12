(function () {
    'use strict';
    window.app.controller('storeCtrl', storeCtrl);
    storeCtrl.$inject = ['$scope', '$timeout', 'storeConfig', '$http', '$q', 'model'];

    function storeCtrl($scope, $timeout, storeConfig, $http, $q, model) {
        var sm = this;
        
        sm.getNearestStore = getNearestStore;
        sm.storeDetail = storeDetail;

        function getNearestStore() {
            $http.post(storeConfig.nearestStoreUrl, { postCode: sm.postCode }).success(function (resp) {
                sm.stores = resp;
                //pm.showStoresOnMap(pm.stores);
            });
        }

        function storeDetail() {
            console.log(model);
            sm.model = model;
        }
    };
})();