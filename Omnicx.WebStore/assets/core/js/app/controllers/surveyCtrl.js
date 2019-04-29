

(function () {
    'use strict';
    window.app.constant('SURVEY_TYPE', {
        'GuidedSearch': 'GuidedSearch', 'LeadCapture': 'LeadCapture', 'Forms': 'Forms', 'PersonalisationQuiz': 'PersonalisationQuiz'
    });
    window.app.controller('surveyCtrl', surveyCtrl);
    surveyCtrl.$inject = ['$scope', '$timeout', 'model', 'surveyConfig', '$http', '$q','SURVEY_TYPE'];

    function surveyCtrl($scope, $timeout, model, surveyConfig, $http, $q, SURVEY_TYPE) {
        var pm = this;
        pm.model = model;
       // console.log(pm.model);
        //public properties.
        pm.currentStep = 0;
        pm.currentQuesId = '';// pm.model.questions[0].recordId;
        pm.canGoPrev = false;
        pm.canGoNext = true;
        pm.surveyCompleted = false;
        pm.showSearchProducts = false;
        pm.showEmailInput = false;
        pm.showSucess = true;
        //public methods
        pm.moveNext = moveNext;
        pm.movePrev = movePrev;
        pm.submitSurvey = submitSurvey;
        pm.showRelevantProducts = showRelevantProducts;
        pm.saveSurveyResponse = saveSurveyResponse;
        pm.addToBag = addToBag;
        pm.answers = [];
        pm.userProductResponse = [];
        pm.surveyResponse = {};    
        pm.searchProducts = [];
        PubSub.subscribe('saveSurveyResponse', function (eventData) {
            var results = [];
            pm.surveyResponse = eventData;
            if (pm.model.surveyType != SURVEY_TYPE.PersonalisationQuiz) {
                $http.post(surveyConfig.saveAnswerUrl, { surveyId: pm.model.recordId, answer: JSON.stringify(eventData), email: pm.surveyEmail }).then(function (resp) {
                    if (!resp.data) {
                        pm.showSucess = false;
                        pm.showEmailInput = true;
                    } else {
                        pm.showSucess = true;
                        pm.redirectToSearch();
                    }

                }, function (error) { })
            } else {
                pm.showSucess = false;
                pm.showSearchProducts = true;
                pm.showRelevantProducts();
            }
            
        });
        function saveSurveyResponse() {
            $http.post(surveyConfig.saveAnswerUrl, { surveyId: pm.model.recordId, answer: JSON.stringify(pm.surveyResponse), email: pm.surveyEmail, searchResult: JSON.stringify(pm.userProductResponse) }).then(function (resp) {
                if (!resp.data) {      
                    pm.showSucess = false; 
                    pm.showEmailInput = true;
                } else {
                    pm.showSucess = true; 
                    pm.showEmailInput = false;
                    pm.showSearchProducts = false;
                    pm.redirectToSearch();
                }

            }, function (error) { })
        }
      
        function showRelevantProducts() {
            if (pm.model.surveyType == SURVEY_TYPE.PersonalisationQuiz)
            {
                var search = { "filters": [],pageSize:4,currentPage:1 };
                var values = Object.values(pm.surveyResponse);
                var filter = "";
                angular.forEach(values, function (ans) {
                    if (typeof (ans) == "string") {
                        if (ans.split("~").length == 2) {
                            search.filters.push({ "key": ans.split("~")[0], "value": ans.split("~")[1] });
                        } 
                        if (ans.split("~").length == 3) {
                            search.filters.push({ "key": ans.split("~")[0] + "~" + ans.split("~")[1], "value": ans.split("~")[2] });
                        } 
                    } else {
                        angular.forEach(ans, function (opt) {
                            if (opt.split("~").length == 2) {
                                search.filters.push({ "key":   opt.split("~")[0], "value": opt.split("~")[1] });
                            } 
                            if (opt.split("~").length == 3) {
                                search.filters.push({ "key": opt.split("~")[0] + "~" + opt.split("~")[2], "value": opt.split("~")[2] });
                            } 
                        }); 
                    }
                                      
                });                
                $http.post(surveyConfig.searchProductUrl, search).then(function (resp) {
                    $("#multiCarousal").carousel("pause").removeData();                 
                    pm.searchProducts = resp.data.results;
                    $("#multiCarousal").carousel(0);
                  

                }, function (error) { })
               
            }
           
           
        }
        pm.redirectToSearch = redirectToSearch;
        function redirectToSearch() {
            if (pm.model.surveyType == SURVEY_TYPE.GuidedSearch) {
                var values = Object.values(pm.surveyResponse);
                var filter = "";
                angular.forEach(values, function (ans) {
                    if (typeof (ans) == "string") {
                        if (ans.split("~").length == 2) {
                            filter = filter + (filter === "" ? ans.split("~")[0] + ":" + ans.split("~")[1] : ";" + ans.split("~")[0] + ":" + ans.split("~")[1]);
                        }                      
                    } else {
                        angular.forEach(ans, function (opt) {
                            if (opt.split("~").length == 2) {                              
                                filter = filter + (filter === "" ? opt.split("~")[0] + ":" + opt.split("~")[1] : ";" + opt.split("~")[0] + ":" + opt.split("~")[1]);
                            }
                        }); 
                    }
                    
                });
                filter = surveyConfig.searchFilterQryString + "=" + filter + "&" + surveyConfig.surveyQryString + "=true";
                if (pm.model.surveyType == SURVEY_TYPE.GuidedSearch) {
                    window.location = surveyConfig.searchUrl + "?" + filter;
                }

            }
        }
        pm.saveSurveyResult = saveSurveyResult;
        function saveSurveyResult() {
            pm.userProductResponse = [];
            angular.forEach(pm.searchProducts, function (prod) {
                var product = { id: prod.recordId, stockCode: prod.stockCode, rating: prod.rating}
                pm.userProductResponse.push(product);
            });    
            pm.showSearchProducts = false;
            pm.saveSurveyResponse();
        }
        pm.retakeSurvey = retakeSurvey;
        function retakeSurvey() {
            window.location = window.location;
        }
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
                                    multipleAnswers += v.optionValue + ',' + v.optionText;
                                }
                                else {
                                    multipleAnswers = multipleAnswers + "," + v.optionValue + ',' + v.optionText;
                                }
                            }
                        });
                    }
                }
                if (multipleAnswers != "") {
                    pm.answers.push({
                        "questionId": value.recordId, "question": value.question, "selectedAnswer": multipleAnswers, "key": value.linkedAttributeCode, "linkedStockCodes": linkedStockCodes
                    });
                }
                else {
                    var ans = value.selectedOptionValue;
                    if (value.inputOptions != null && value.inputOptions != undefined) {
                        angular.forEach(value.inputOptions, function (opt) {
                            if (value.selectedOptionValue == opt.optionValue) {
                                ans = ans + ',' + opt.optionText;
                                //pm.answers.push({ "questionId": value.recordId, "question": value.question, "selectedAnswer": opt.optionText, "key": value.linkedAttributeCode, "linkedStockCodes": linkedStockCodes });
                            }
                        });
                    }
                    pm.answers.push({ "questionId": value.recordId, "question": value.question, "selectedAnswer": ans, "key": value.linkedAttributeCode, "linkedStockCodes": linkedStockCodes });
                    //pm.answers.push({ "questionId": value.recordId, "question": value.question, "selectedAnswer": value.selectedOptionValue, "key": value.linkedAttributeCode, "linkedStockCodes": linkedStockCodes });
                }
            });

            //save survey.
            $http.post("/Survey/SaveAnswerBulk", { surveyId: pm.model.recordId, answers: pm.answers }).then(function (response) {
                pm.surveyResponse = response.data;
            }, function (error) { })


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