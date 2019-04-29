(function () {
    'use strict';
    window.app.controller('blogCtrl', blogCtrl);
    blogCtrl.$inject = ['$scope', '$http', 'blogConfig'];

    function blogCtrl($scope, $http, blogConfig) {
        var bm = this;
        bm.initCategory = initCategory;
        bm.categoryList = [];
        bm.initCategoryResults = initCategoryResults;
        bm.CategoryResults = [];
        bm.initEditorResults = initEditorResults;
        bm.geturl = geturl;
        bm.searchAgain = searchAgain;
        bm.currentPage = 4;
        bm.showMore = showMore;
        bm.results = [];
        function initCategory(categoryListModel) {
            bm.categoryList = categoryListModel;
        };
        function initCategoryResults(model) {
            bm.results = model;
            bm.currentPage = model.blogList.currentPage;
            bm.pages = model.blogList.pages;
            bm.total = model.blogList.total;
        };
        function initEditorResults(model) {
            bm.currentPage = model.currentPage;
            bm.pages = model.pages;
            bm.total = model.total;
        }
        function geturl() {
            bm.url = window.location.origin;
        };
        function searchAgain() {
            bm.freeText = true;
            bm.freeSearch = true;
        }
        function showMore() {
            bm.currentPage += 1 ;
            $http.post(blogConfig.showMoreUrl, { currentPage: bm.currentPage })
             .success(function (data) {
                 for (var i = 0; i < data.results.length; i++) {
                     bm.results.push(data.results[i]);
                 }                               
             })
             .error(function (msg) {
             });
        }
    }

})();