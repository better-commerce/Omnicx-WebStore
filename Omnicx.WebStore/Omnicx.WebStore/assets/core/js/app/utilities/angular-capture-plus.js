var app = angular.module('CapturePlus', []);
app.directive('capturePlus', function () {
    return {
        require: 'ngModel',
        restrict: 'A',
        link: function (scope, elem, attrs) {
            if (navigator.appName == 'Microsoft Internet Explorer') {
                elem[0].attachEvent('onchange', function () {
                    scope.$apply(function () {
                        var model = attrs.ngModel;
                        scope[model] = elem.val();
                    })
                });
            }
            else {
                elem.bind('change', function () {
                    scope.$apply(function () {
                        var model = attrs.ngModel;
                        scope[model] = elem.val();
                    })
                });
            }
        }
    }

});

app.factory('CapturePlus', function () {
    var capture = {
        CapturePlusCallback: function () { },
        CapturePlusError: function () { },
        CapturePlusStartTyping: function () { }
    }
    window.CapturePlusCallback = function (uid, response) {
        var captureFields = getAllElementsWithAttribute('capture-plus');
        for (var i = 0; i < captureFields.length; i++) {
            var field = document.getElementById(captureFields[i].id);
            if ("fireEvent" in field)
                field.fireEvent("onchange");
            else {
                var evt = document.createEvent("HTMLEvents");
                evt.initEvent("change", false, true);
                field.dispatchEvent(evt);
            }
        }
        capture.CapturePlusCallback(uid, response);
    }
    window.CapturePlusStartTyping = function (uid, response) {
        capture.CapturePlusStartTyping(uid, response);
    }
    window.CapturePlusError = function (uid, response) {
        capture.CapturePlusError(uid, response);
    }
    return capture;
});

function getAllElementsWithAttribute(attribute) {
    var matchingElements = [];
    var allElements = document.getElementsByTagName('*');
    for (var i = 0; i < allElements.length; i++) {
        if (allElements[i].attributes) {
            if (allElements[i].attributes[attribute]) {
                matchingElements.push(allElements[i]);
            }
        }
    }
    return matchingElements;
}
