(function () {
    'use strict';

    window.app.directive('surveyInputField', surveyInputField);
    function surveyInputField($compile) {
        var TEMPLATES = {
            TextInput:
            '<input type="text" class="form-control" ng-model="$parent.ques.selectedOptionValue" />',
            Multiline:
            '<textarea rows="4" cols"60" class="form-control" ng-model="$parent.ques.selectedOptionValue" />',
            RatingScale:
            '<angular-star-rating max="ques.rangeMaxNumber" value="$parent.ques.selectedOptionValue"  hover="true" is-readonly="false"></angular-star-rating>',
            SingleSlider: //http://angular-slider.github.io/angularjs-slider/
            ' <rzslider rz-slider-model="$parent.ques.selectedOptionValue" rz-slider-options="{floor:ques.rangeMinNumber,ceil:ques.rangeMaxNumber}"></rzslider>',
            DoubleSlider:
            ' <rzslider rz-slider-model="$parent.ques.selectedOptionValue" rz-slider-high="ques.rangeMaxNumber" rz-slider-options="{floor:ques.rangeMinNumber,ceil:ques.rangeMaxNumber}"></rzslider>',
            AsDropdown:
            '    <select' +
            '        name="{{ques.recordId}}"' +
            '        ng-model="$parent.ques.selectedOptionValue"' +
            '        class="form-control wizard"' +
            '        >' +
            '        <option ng-repeat="option in ques.inputOptions"  value="{{option.optionValue}}">{{option.optionText}}</option>' +
            '    </select>',
            AsText:
            '   <div class="col-sm-3" ng-repeat="option in ques.inputOptions track by $index">' +
            '       <div class="col-sm-12 col-xs-12 survey-options" ng-show="$parent.$parent.ques.inputDataType == \'OptionsSingleSelect\'">' +
            '           <div class="control-group">' +
            '               <label class="control control--radio">' +
            '                   <span class="label-survey">{{option.optionText}}</span>' +
            '                   <input type="radio" ng-value="option.optionValue" ng-click="option.stopAddToBag==true?$parent.$parent.ques.showHelpText=true:$parent.$parent.ques.showHelpText=false; $parent.$parent.ques.selectedOptionValue=option.optionValue;" name="singleSelect{{$parent.$parent.ques.recordId}}" ng-class="{\'btn btn-selected\':option.selected, \'btn\':!option.selected}">' +
            '                   <div class="control__indicator"></div>' +
            '               </label>' +
            '           </div>' +
            '       </div>' +
            '   </div>' +
            '    <ul class="pull-left">' +
            '        <li class="options-as-text " ng-repeat="option in ques.inputOptions track by $index">' +
            '            <button class="animate btn-default" ng-show="$parent.$parent.ques.inputDataType == \'OptionsMultipleSelect\'" ng-click="option.selected=!option.selected;$parent.$parent.ques.inputOptions[$index].selectedOptionValue = option.selected;$parent.$parent.ques.selectedOptionValue=option.optionValue" ng-class="{\'animate btn-success\':option.selected, \'animate btn-default\':!option.selected}">' +
            '            <span ng-class="{\'fa\':true, \'fa-check\':option.selected, \'fa-times\':!option.selected}"></span>' +
            '           {{option.optionText}}</button>' +
            '       </li>' +
            '    </ul>',
            AsImage:
            '    <ul>' +
            '        <li class="wizardImg-container" ng-repeat="option in ques.inputOptions track by $index" ng-click="option.selected=!option.selected; $parent.$parent.ques.inputOptions[$index].selectedOptionValue = option.selected;$parent.$parent.ques.selectedOptionValue=option.optionValue">' +
            '            <span ng-class="{\'wizardImg-label-selected\':option.selected}"><i ng-class="{\'fa fa-check\':option.selected, \'fa\':!option.selected}"></i> &nbsp; {{ option.optionText }} </span>' +
            '           <img ng-src="{{$parent.$parent.pm.model.imageBaseUrl}}{{option.imageUrl}}" alt="{{option.optionText}}" />' +
            '           <div class="wizadImg-inputs"> ' +
            '              <input ng-show="ques.inputDataType == \'OptionsMultiSelect\'" type="checkbox" value="option.optionValue" ng-model="$parent.$parent.ques.inputOptions[$index].selectedOptionValue" />' +
            '              <input ng-show="ques.inputDataType == \'OptionsSingleSelect\'" type="radio" value="option.optionValue" ng-model="$parent.$parent.ques.inputOptions[$index].selectedOptionValue" />' +
            '           </div>' +
            '        </li>' +
            '    </ul>',
            AsCarousel:
            '    <ul>' +
            '        <li class="options-as-text " ng-repeat="option in ques.inputOptions track by $index" ng-click="option.selected=!option.selected; $parent.$parent.ques.inputOptions[$index].selectedOptionValue = option.selected;$parent.$parent.ques.selectedOptionValue=option.optionValue">' +
            '            {{option.optionText}} ' +
            '           <img ng-src="{{option.imageUrl}}" alt="{{option.optionText}}" />' +
            '           <div> ' +
            '              <input ng-show="ques.inputDataType === \'OptionsMultiSelect\'" type="checkbox" value="option.optionValue" ng-model="$parent.ques.selectedOptionValue" />' +
            '              <input ng-show="ques.inputDataType === \'OptionsSingleSelect\'" type="radio" value="option.optionValue" ng-model="$parent.ques.selectedOptionValue" />' +
            '           </div>' +
            '        </li>' +
            '    </ul>',
        };

        return {
            restrict: 'E',
            scope: {
                question: "@question"
            },
            link: function (scope, element, attrs) {
                scope.ques = eval("(" + scope.question + ")");
                var tmpl = TEMPLATES[scope.ques.inputStyle];
                element.html(tmpl);

                $compile(element.contents())(scope);
            }
        }
    }
})();
