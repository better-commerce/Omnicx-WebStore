(function () {
    'use strict';

    /**
     * Set Class When At Top
     */
   // var app = angular.module('app', []);
    angular.module('numbersOnly', []).directive('numbersOnly', function () {
       return {
           require: 'ngModel',
           link: function (scope, element, attr, ngModelCtrl) {              
               element.on('keydown', function (event) {
                   var $input = $(this);
                   var value = $input.val();
                   value = value.replace(/[^0-9]/g, '')
                   $input.val(value);
                   if (event.which == 64 || event.which == 16) {
                       // to allow numbers  
                       return false;
                   } else if (event.which >= 48 && event.which <= 57) {
                       // to allow numbers  
                       return true;
                   } else if (event.which >= 96 && event.which <= 105) {
                       // to allow numpad number  
                       return true;
                   } else if ([8, 13, 27, 37, 38, 39, 40].indexOf(event.which) > -1) {
                       // to allow backspace, enter, escape, arrows  
                       return true;
                   } else {
                       event.preventDefault();
                       // to stop others  
                       //alert("Sorry Only Numbers Allowed");  
                       return false;
                   } 
               });
               //ngModelCtrl.$parsers.push(fromUser);
           }
       };
   });
}());