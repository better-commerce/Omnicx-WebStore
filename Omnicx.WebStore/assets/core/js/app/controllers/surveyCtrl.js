(function () {
    'use strict';
    window.app.controller('surveyCtrl', surveyCtrl);
    surveyCtrl.$inject = ['$scope', '$timeout', 'model', 'surveyConfig', '$http', '$q'];

    function surveyCtrl($scope, $timeout, model, surveyConfig, $http, $q) {
        var pm = this;
        pm.model = model;

        //public properties.
        pm.currentStep = 0;
        pm.currentQuesId = pm.model.questions[0].recordId;
        pm.canGoPrev = false;
        pm.canGoNext = true;
        pm.surveyCompleted = false;

        //public methods
        pm.moveNext = moveNext;
        pm.movePrev = movePrev;
        pm.submitSurvey = submitSurvey;
        pm.showRelevantProducts = showRelevantProducts;
        pm.addToBag = addToBag;
        pm.answers = [];
        pm.surveyResponse = {};
        function submitSurvey() {
            //this is where we submit the whole survey using AJAX call.
            pm.surveyCompleted = true;
            pm.answers = []; var multipleAnswers = "";
            var linkedStockCodes = [];
            //save answers in an array
            angular.forEach(pm.model.questions, function (value, key) {
                multipleAnswers = "";
                if (value.selectedOptionValue == "") {
                    if (value.inputOptions) {
                        //if one answer have multiple options, save them as comma-seprated string
                        angular.forEach(value.inputOptions, function (v, k) {
                            if (v.selectedOptionValue) {
                                linkedStockCodes = v.linkedStockCodes;
                                if (multipleAnswers == "") {
                                    multipleAnswers += v.optionValue;
                                }
                                else {
                                    multipleAnswers = multipleAnswers + "-" + v.optionValue;
                                }
                            }
                        });
                    }
                }
                if (multipleAnswers != "") {
                    pm.answers.push({
                        "questionId": value.recordId, "question": value.question, "selectedAnswer": multipleAnswers, "key": value.linkedAttributeCode, "linkedStockCodes": linkedStockCodes});
                }
                else {
                    pm.answers.push({ "questionId": value.recordId, "question": value.question, "selectedAnswer": value.selectedOptionValue, "key": value.linkedAttributeCode, "linkedStockCodes": linkedStockCodes});
                }
            });

            //save survey.
            $http.post("/Survey/SaveAnswerBulk", { surveyId: pm.model.recordId, answers: pm.answers }).then(function (response) {
                pm.surveyResponse = response.data;              
            }, function (error) { })


        }
        function showRelevantProducts() {           
            var filter = "";
            angular.forEach(pm.answers, function (ans) {              
                filter = filter + (filter === "" ? ans.key + ":" + ans.selectedAnswer : ";" + ans.key + ":" + ans.selectedAnswer );
            });
            filter = surveyConfig.searchFilterQryString + "=" + filter + "&" + surveyConfig.surveyQryString + "=true";
            window.location = surveyConfig.searchUrl + "?" + filter;
            //redirect the user to search result matching the criterias provided
        }
        function addToBag() {
            //save survey.
            $http.post(surveyConfig.addToBagUrl, pm.surveyResponse).then(function (response) {
                window.location.href = surveyConfig.basketUrl;
            }, function (error) { })
        }
        function moveNext() {
            if (pm.model.questions[pm.currentStep] == undefined) {
                enableDisablePrevNextBtns();
                return;
            }
            //increment the current step by 1 
            if (pm.model.questions[pm.currentStep].isMandatory && !pm.model.questions[pm.currentStep].selectedOptionValue) {
                $scope.isMandatory = true;
                return;
            }
            else {
                $scope.isMandatory = false;
                pm.currentStep = pm.currentStep + 1;
                if (pm.currentStep === pm.model.questions.length) {
                    pm.canGoNext = false;
                }
                else
                {
                    pm.currentQuesId = pm.model.questions[pm.currentStep].recordId;
                    enableDisablePrevNextBtns();
                }
                
            }
        }
        function movePrev() {
            pm.currentStep = pm.currentStep - 1;
            if (pm.currentStep === 0 || pm.currentStep <= pm.model.questions.length)
            {
                pm.canGoNext = true;
            }
            pm.currentQuesId = pm.model.questions[pm.currentStep].recordId;
            enableDisablePrevNextBtns();
        }
        function enableDisablePrevNextBtns() {
            //check if it does not exceed the number of questions
            if (pm.currentStep === pm.model.questions.length - 1) {
                pm.canGoNext = false;
            }
            if (pm.currentStep === 0) { pm.canGoPrev = false; }
            else { pm.canGoPrev = true; }
        }


    };
})();