(function () {
    'use strict';
    window.app.factory('loader', ['$http', '$rootScope', function ($http, $rootScope) {

        angular.element(document).ready(function () {
            angular.element(".dvloader").hide();
        });

        $http.defaults.transformRequest.push(function (data) {
            angular.element(".dvloader").show();
            return data;
        });
        $http.defaults.transformResponse.push(function (data) {
            angular.element(".dvloader").hide();
            return data;
        })
        return $http;
    }]);
}());
