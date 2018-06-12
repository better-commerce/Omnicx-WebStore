(function () {
    'use strict';

    /**
     * autocomplete
     * Autocomplete directive for AngularJS
     * By Daryl Rowland
     * https://github.com/darylrowland/angucomplete
     */
    var searchid = new Date().getTime();
    angular.module('btAutoComplete', [])
        .directive('btAutoComplete', function ($parse, $http, $sce, $timeout) {
            return {
                restrict: 'EA',
                scope: {
                    "id": "@id",
                    "placeholder": "@placeholder",
                    "selectedObject": "=selectedobject",
                    "url": "@url",
                    "dataField": "@datafield",
                    "titleField": "@titlefield",
                    "descriptionField": "@descriptionfield",
                    "imageField": "@imagefield",
                    "imageUri": "@imageuri",
                    "inputClass": "@inputclass",
                    "userPause": "@pause",
                    "localData": "=localdata",
                    "searchFields": "@searchfields",
                    "minLengthUser": "@minlength",
                    "matchClass": "@matchclass",
                    "returnkeyios": "@returnkeyios"
                },
                template: ' <div class="navbar-collapse collapse right">' +
                    '<form method="post" class="navbar-form" action="">' +
                        '<div class="autocomplete-holder">' +
            ' <div class="input-group">'+
                   '<input title="{{returnkeyios}}" name="txtAutoSearch' + searchid + '"  maxlength="60"  autocomplete="false"  ng-keyup="$event.keyCode == 13 && headerSearch()"  ng-model="searchStr" type="search" placeholder="{{placeholder}}" class="form-control search-textbox search-textbox-mobile" onmouseup="this.select();" ng-focus="resetHideResults()" ng-blur="hideResults()" autofocus>' +
                   '<span class="input-group-btn">'+
                    '<button type="button" class="btn btn-search top-search-button" ng-click="headerSearch()"><i class="fa fa-search"></i></button>' +
                    '</span>'+
              '</div>' +             
                            '<div id="{{id}}_dropdown" class="autocomplete-dropdown" ng-if="showDropdown">' +
                             '<div id="ex1_dropdown_arrow"></div>' +
                                '<div class="autocomplete-searching" ng-show="searching">Searching...</div>' +
                                '<div class="autocomplete-searching" ng-show="!searching && (!results || results.length == 0)">No results found</div>' +
                                '<div class="col-xs-12 col-sm-12 no-padding hidden-xs" ng-show="!searching && (!results || results.length != 0)"><h2 class="search-h2">Products</h2></div>' +
                                '<div class="col-xs-12 col-sm-12 no-padding visible-xs" ng-show="!searching && (!results || results.length != 0)"><h2 class="search-h2">Suggested Search Products</h2></div>' +
                                '<div class="col-sm-12 col-xs-12 no-padding">'+
                                '<div ng-repeat="result in results" class="col-sm-2 text-center result-min-180 no-padding hidden-xs" ng-if="$index<6">' +
                                        '<div class="col-sm-12 col-xs-12 no-padding">' +
                                        '<a href="/{{result.originalObject.slug}}">' +
                                                '<div ng-if="imageField" class="autocomplete-image-holder"><span>{{result.originalObject.detailUrl}}</span>' +
                                                    '<img ng-if="result.image && result.image != \'\'" ng-src="{{result.image}}" class="autocomplete-image"/>' +
                                                    '<div ng-if="!result.image && result.image != \'\'" class="autocomplete-image-default"></div>' +
                                                '</div>' +
                                                '<div class="search-detail">' +
                                                '<div class="autocomplete-title" ng-if="matchClass" ng-bind-html="result.title"></div>' +
                                                '<div class="autocomplete-title" ng-if="!matchClass">{{ result.title }}</div>' +
                                                '<div class="autocomplete-title"><b>{{result.originalObject.price.formatted.withTax }}</b></div>' +
                                                '</div>' +
                                              '</a>' +
                                         '</div>' +
                                         '</div>' +
                                          '<div ng-repeat="result in results" class="col-xs-12 no-padding visible-xs" ng-if="$index<6">' +
                                            '<div class="col-sm-12 col-xs-12 no-padding">' +
                                                '<a href="/{{result.originalObject.slug}}">' +
                                                    '<div class="col-xs-12 no-padding">' +
                                                        '<div class="autocomplete-title-result" ng-if="matchClass" ng-bind-html="result.title"></div>' +
                                                        '<div class="autocomplete-title-result" ng-if="!matchClass">{{ result.title }}</div>' +
                                                    '</div>' +
                                              '</a>' +
                                        '</div>' +
                                    '</div>' +
                                 '</div>' +
                                 '<div class="col-xs-12 col-sm-12 no-padding hidden-xs" ng-show="!searching && (!blogresults || blogresults.length != 0)"><h2 class="search-h2">Blogs</h2></div>' +
                                 '<div class="col-xs-12 col-sm-12 no-padding hidden-xs" ng-show="!searching && (!blogresults || blogresults.length != 0)">' +
                                        '<div class="col-xs-12 col-sm-6 no-padding" ng-repeat="result in blogresults">' +
                                            '<div class="search-blog-row">' +
                                                    '<div class="search-blog-image">' +
                                                            '<a href="/blogs/{{result.originalObject.slug}}"><img ng-src="{{result.originalObject.image}}" title="" alt="" /></a>' +
                                                    '</div>' +
                                                    '<div class="search-blog-name">' +
                                                        '<a href="/blogs/{{result.originalObject.slug}}"><h2 ng-bind="result.originalObject.name"></h2></a>' +
                                                        '<p ng-bind="result.originalObject.abstract"></p>' +
                                                    '</div>' +
                                            '</div>' +
                                        '</div>' +                                        
                                 '</div>' +
                             '</div>' +
                          '</div>' +
                        '</form>'+
                '</div>',

                link: function ($scope, elem, attrs) {
                    $scope.lastSearchTerm = null;
                    $scope.currentIndex = null;
                    $scope.justChanged = false;
                    $scope.searchTimer = null;
                    $scope.hideTimer = null;
                    $scope.searching = false;
                    $scope.pause = 500;
                    $scope.minLength =2;
                    $scope.searchStr = null;
                    $scope.returnkeyios = "Search";

                    if ($scope.minLengthUser && $scope.minLengthUser != "") {
                        $scope.minLength = $scope.minLengthUser;
                    };

                    if ($scope.userPause) {
                        $scope.pause = $scope.userPause;
                    };

                    $scope.isNewSearchNeeded = function (newTerm, oldTerm) {
                        return newTerm.length >= $scope.minLength && newTerm != oldTerm;
                    };
                    $scope.headerSearch = function () {

                        if ($scope.searchStr != null && $scope.searchStr != "") {
                            window.location = "//" + window.location.host + "/search?freeText=" + $scope.searchStr.toLowerCase();
                        }
                        //else {
                        //    window.location = "//" + window.location.host + "/search";
                        //}
                    };
                    $scope.processResults = function (responseData, str) {
                        if (responseData && responseData.length > 0) {
                            $scope.results = [];
                            var titleFields = [];
                            if ($scope.titleField && $scope.titleField != "") {
                                titleFields = $scope.titleField.split(",");
                            }
                            for (var i = 0; i < responseData.length; i++) {
                                var titleCode = [];
                                for (var t = 0; t < titleFields.length; t++) {
                                    titleCode.push(responseData[i][titleFields[t]]);
                                }
                                var description = "";
                                if ($scope.descriptionField) {
                                    description = responseData[i][$scope.descriptionField];
                                }
                                var imageUri = "";
                                if ($scope.imageUri) {
                                    imageUri = $scope.imageUri;
                                }
                                var image = "";
                                if ($scope.imageField) {
                                    image = responseData[i].image;
                                    //if (responseData[i].images!=null)
                                    //{
                                    //    if (responseData[i].images[0])
                                    //    image = imageUri + responseData[i].images[0][$scope.imageField];
                                    //}
                                }

                                var text = titleCode.join(' ');
                                if ($scope.matchClass) {
                                    var re = new RegExp(str, 'i');
                                    var strPart = text.match(re);
                                    if (strPart === null) {
                                        text = $sce.trustAsHtml(text);
                                    } else {
                                        text = $sce.trustAsHtml(text.replace(re, '<span class="' + $scope.matchClass + '">' + strPart[0] + '</span>'));
                                    }
                                }
                                var resultRow = {
                                    title: text,
                                    description: description,
                                    image: image,
                                    originalObject: responseData[i]
                                }
                                $scope.results[$scope.results.length] = resultRow;
                            }


                        } else {
                            $scope.results = [];
                        }
                        PubSub.publish("search", str, responseData);
                    };
                    PubSub.subscribe('search', function (searchKey, eventData) {
                        if (eventData != null && dataLayer && omnilytics) {
                            var data = dataLayer[0];
                            var entity = { 'FreeText': searchKey, 'ResultCount': eventData.length };
                            data["Entity"] = JSON.stringify(entity);
                            data["EntityId"] = searchKey;
                            data["EntityName"] = searchKey;
                            data["EntityType"] = "Search";
                            data["EventType"] = "Search";
                            data["Action"] = "search";
                            dataLayer[0] = data;
                            omnilytics.emit('Search', null);

                        }
                    });
                    $scope.searchTimerComplete = function (str) {
                        if (str.length >= $scope.minLength) {
                            if ($scope.localData) {
                                var searchFields = $scope.searchFields.split(",");

                                var matches = [];

                                for (var i = 0; i < $scope.localData.length; i++) {
                                    var match = false;

                                    for (var s = 0; s < searchFields.length; s++) {
                                        match = match || (typeof $scope.localData[i][searchFields[s]] === 'string' && typeof str === 'string' && $scope.localData[i][searchFields[s]].toLowerCase().indexOf(str.toLowerCase()) >= 0);
                                    }

                                    if (match) {
                                        matches[matches.length] = $scope.localData[i];
                                    }
                                }

                                $scope.searching = false;
                                $scope.processResults(matches, str);

                            } else {
                                $scope.searching = true;
                                $http.get($scope.url + str, {}).
                                    success(function (responseData, status, headers, config) {
                                        $scope.searching = false;
                                        $scope.processResults((($scope.dataField) ? responseData[$scope.dataField] : responseData.products), str);
                                        $scope.processblogResults((($scope.dataField) ? responseData[$scope.dataField] : responseData.blogs), str);
                                    }).
                                    error(function (data, status, headers, config) {
                                        console.log("error");
                                    });
                            }
                        }
                    };

                    $scope.hideResults = function () {
                        $scope.hideTimer = $timeout(function () {
                            $scope.showDropdown = false;
                        }, 500);
                    };

                    $scope.resetHideResults = function () {
                        if ($scope.hideTimer) {
                            $timeout.cancel($scope.hideTimer);
                        };
                    };

                    $scope.hoverRow = function (index) {
                        $scope.currentIndex = index;
                    };

                    $scope.keyPressed = function (event) {
                        if (!(event.which == 38 || event.which == 40 || event.which == 13)) {
                            if (!$scope.searchStr || $scope.searchStr == "") {
                                $scope.showDropdown = false;
                                $scope.lastSearchTerm = null;
                            } else if ($scope.isNewSearchNeeded($scope.searchStr, $scope.lastSearchTerm)) {
                                $scope.lastSearchTerm = $scope.searchStr;
                                $scope.showDropdown = true;
                                $scope.currentIndex = -1;
                                $scope.results = [];

                                if ($scope.searchTimer) {
                                    $timeout.cancel($scope.searchTimer);
                                }

                                //$scope.searching = true;
                                if ($scope.searching) {
                                    $scope.searchTimer = $timeout(function () {
                                        $scope.searchTimerComplete($scope.searchStr);
                                    }, 2000);
                                } else {
                                    $scope.searchTimerComplete($scope.searchStr);
                                }
                            }
                        } else {
                            event.preventDefault();
                        }
                    };

                    $scope.selectResult = function (result) {
                        window.location = result.originalObject.detailUrl;
                        if ($scope.matchClass) {
                            result.title = result.title.toString().replace(/(<([^>]+)>)/ig, '');
                        }
                        $scope.searchStr = $scope.lastSearchTerm = result.title;
                        $scope.selectedObject = result;

                        $scope.showDropdown = false;
                        $scope.results = [];
                    };

                    var inputField = elem.find('input');

                    inputField.on('keyup', $scope.keyPressed);

                    elem.on("keyup", function (event) {
                        if (event.which === 40) {
                            if ($scope.results && ($scope.currentIndex + 1) < $scope.results.length) {
                                $scope.currentIndex++;
                                if ($scope.results[$scope.currentIndex].originalObject.id == 0) {
                                    $scope.currentIndex++;
                                }
                                $scope.$apply();
                                event.preventDefault();
                                event.stopPropagation();
                            }

                            $scope.$apply();
                        } else if (event.which == 38) {
                            if ($scope.currentIndex >= 1) {
                                $scope.currentIndex--;
                                if ($scope.results[$scope.currentIndex].originalObject.id == 0) {
                                    $scope.currentIndex--;
                                }
                                $scope.$apply();
                                event.preventDefault();
                                event.stopPropagation();
                            }

                        } else if (event.which == 13) {
                            if ($scope.results && $scope.currentIndex >= 0 && $scope.currentIndex < $scope.results.length) {
                                $scope.selectResult($scope.results[$scope.currentIndex]);
                                $scope.$apply();
                                event.preventDefault();
                                event.stopPropagation();
                            } else {
                                $scope.results = [];
                                $scope.$apply();
                                event.preventDefault();
                                event.stopPropagation();
                            }

                        } else if (event.which == 27) {
                            $scope.results = [];
                            $scope.showDropdown = false;
                            $scope.$apply();
                        } else if (event.which == 8) {
                            $scope.selectedObject = null;
                            $scope.$apply();
                        }
                    });

                    $scope.processblogResults = function (responseData, str) {
                        
                        if (responseData && responseData.length > 0) {
                            $scope.blogresults = [];
                            var titleFields = [];
                            if ($scope.titleField && $scope.titleField != "") {
                                titleFields = $scope.titleField.split(",");
                            }
                            for (var i = 0; i < responseData.length; i++) {
                                var titleCode = [];
                                for (var t = 0; t < titleFields.length; t++) {
                                    titleCode.push(responseData[i][titleFields[t]]);
                                }
                                var description = "";
                                if ($scope.descriptionField) {
                                    description = responseData[i][$scope.descriptionField];
                                }
                                var imageUri = "";
                                if ($scope.imageUri) {
                                    imageUri = $scope.imageUri;
                                }
                                var image = "";
                                if ($scope.imageField) {
                                    if (responseData[i].image != null) {
                                        if (responseData[i].image)
                                            image = imageUri + responseData[i].image[$scope.imageField];
                                    }
                                }

                                var text = titleCode.join(' ');
                                if ($scope.matchClass) {
                                    var re = new RegExp(str, 'i');
                                    var strPart = text.match(re);
                                    if (strPart === null) {
                                        text = $sce.trustAsHtml(text);
                                    } else {
                                        text = $sce.trustAsHtml(text.replace(re, '<span class="' + $scope.matchClass + '">' + strPart[0] + '</span>'));
                                    }
                                }
                                var resultRow = {
                                    title: text,
                                    description: description,
                                    image: image,
                                    originalObject: responseData[i]
                                }
                                $scope.blogresults[$scope.blogresults.length] = resultRow;
                            }


                        } else {
                            $scope.blogresults = [];
                        }                       
                    };
                }
            };
        });

}());