(function () {
    'use strict';
    /* Initialize the main module */
    var Xapp = angular.module('btApp', ['bw.paging', 'btAutoComplete', 'rzModule', 'CapturePlus','btRecommendation']);

    window.app = Xapp;
    Xapp.constant('BASE_URL', window.location.origin);

    window.app.config( function ($httpProvider) {
        $httpProvider.defaults.headers.common['X-Requested-With'] = 'XMLHttpRequest';

        $httpProvider.interceptors.push(function ($q) {
            return {
                request: function (config) {
                    return config || $q.when(config);
                },
                response: function (response) {

                    //update cookie time to 30 minutes at every request/activity. 
                    var sessionCookie = $.cookie('sid');
                    if (sessionCookie != null && sessionCookie != undefined)
                    {
                        $.removeCookie('sid');
                        $.cookie('sid', sessionCookie, { path: '/', expires: 30 / 1440 });
                    }
                    return response;
                },
                responseError: function (response) {
                    var responseInfo = {
                        status: response.status,
                        statusText: response.statusText,
                        headers: response.headers && response.headers(),
                        data: response.data,
                        config: response.config
                    };
                   
                    if (response.status === 500) {
                        console.log(response)
                    }
                    return $q.reject(response);
                }
            };
        });
    });
})();