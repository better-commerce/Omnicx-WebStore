(function () {
    'use strict';
    window.app.controller('globalCtrl', globalCtrl);
    globalCtrl.$inject = ['$scope', '$timeout', 'globalConfig', 'loader', '$http'];
    function globalCtrl($scope, $timeout, globalConfig, $http) {
        var gm = this;
        gm.model = {};
        gm.userLogin = userLogin;
        gm.registration = registration;
        gm.contactForm = contactForm;
        gm.currencySettings = currencySettings;
        gm.errorMessage == null
        gm.saving = false;
        gm.success = false;
        gm.basketResponse = [];
        gm.initBasket = initBasket;
        gm.addToBasket = addToBasket;
        gm.getPaymentMethods = getPaymentMethods;
        gm.basketResponse = [];
        gm.miniBasketSize = 3;
        gm.lineItemTotal = 0;
        gm.getShippingMethods = getShippingMethods;
        $scope.signin = false;
        $scope.register = false;
        $scope.global_login = false;
        gm.shippingMethods = [];
        gm.updateShipping = updateShipping;
        gm.applyPromoCode = applyPromoCode;
        gm.invalidpromo = false;
        gm.getallblogs = getallblogs;
        gm.getallblogsbycategory = getallblogsbycategory;
        gm.getBlogByCategory = getBlogByCategory;
        gm.getBlogByCategory = getBlogByCategory;
        gm.blogReponse = [];
        gm.initblogs = initblogs;
        gm.login = login;
        gm.globalLogin = globalLogin;
        gm.newsLetterSubscription = newsLetterSubscription;
        gm.emailinvalid = false;
        gm.subssuccess = false;
        gm.customerEmail = '';
        gm.showBasket = showBasket;
        gm.showAccount = showAccount;
        gm.alreadySubscribed = false;
        gm.removePromoCode = removePromoCode;
        gm.forgotPassword = forgotPassword;
        gm.emptyGuid = '00000000-0000-0000-0000-000000000000';
        gm.openQuickBasketModal = openQuickBasketModal;
        gm.nRows = nRows;
        gm.addProductsInBasket = addProductsInBasket;
        gm.generateTable = generateTable;
        gm.addProductsExcel = addProductsExcel;

        function contactForm(model) {
            $(".alertBlock").fadeIn();
            $http.post(globalConfig.setContactForm, { model: model })
             .success(function (data) {
                 gm.errorMessage = null;
                 gm.success = true;
             })
             .error(function (msg) {
                 gm.errorMessage = msg.errorMessages;
             })
             .finally(function () {
                 gm.saving = false;
                 $timeout(function () { $(".alertBlock").fadeOut(); }, 3000);
             });
        }

        function applyPromoCode(basketId, promoCode) {

            if ($.trim(promoCode) != "") {
                gm.model.Id = basketId;
                gm.invalidpromo = false;
                gm.showCustomMsg = false;
                gm.model.promoCode = promoCode;
                $http.post(globalConfig.applyPromoCode, gm.model).success(function (data) {
                    $scope.promoCode = null;
                    gm.invalidpromo = false;
                    gm.validpromo = true;
                    $('.promovalid').show(0).delay(2000).hide(0);
                    gm.basketResponse = data.result.basket;
                })
                .error(function (msg) {
                    if (msg.errorMessage != "false") {
                        gm.showCustomMsg = true;
                        gm.customMsg = msg.errorMessage;
                    }
                    gm.invalidpromo = true;
                    $('.promo').show(0).delay(2000).hide(0);
                })
                 .finally(function () {
                 });
            }
            else {
                gm.promonull = true;
                $('.promonull').show(0).delay(1200).hide(0);
            }
        }

        function initBasket() {
            $http.post(globalConfig.getBasketUrl)
             .success(function (data) {

                 gm.basketResponse = data;
                 var count = 0;
                 if (gm.basketResponse != null) {
                     if (gm.basketResponse.lineItems != null) {
                         angular.forEach(gm.basketResponse.lineItems, function (line) {
                             if (line.parentProductId == gm.emptyGuid) {
                                 count = count + line.qty;

                             }
                         });
                     }
                 }
                 gm.count = count;
             })
             .error(function (msg) {
                 // vm.errorMessage = msg.errorMessages;
             })
             .finally(function () {
                 // vm.saving = false;
                 //$("html, body").animate({ scrollTop: 0 }, "slow");
             });


        }

        function addToBasket(recordId, qty, displayOrder) {
            //-- checks if basket already have 12 items-------
            //if (vm.basketResponse.lineItems.length >= 12) {
            //    return false;
            //}
            if (displayOrder >= 0) {
                gm.displayOrder = displayOrder;
            }
            else {
                gm.displayOrder = gm.basketResponse.lineItems.length + 1;
            }

            $http.post(globalConfig.addToBasket, { "productId": recordId, "qty": qty, "displayOrder": gm.displayOrder })
              .success(function (data) {
                  //vm.success = true;
                  gm.basketResponse = data;
                  var count = 0;
                  if (gm.basketResponse != null) {
                      if (gm.basketResponse.lineItems != null) {
                          angular.forEach(gm.basketResponse.lineItems, function (line) {
                              if (line.parentProductId == gm.emptyGuid) {
                                  count = count + line.qty;

                              }
                          });
                          gm.count = count;
                      }
                  }

                  if (qty == 1) {
                      $("html, body").animate({ scrollTop: 0 }, "slow");
                      $('.cartopen').addClass('active');
                      $timeout(function () { $(".cartopen").removeClass("active"); }, 3000);
                  }
              })
              .error(function (msg) {
                  // vm.errorMessage = msg.errorMessages;
              })
              .finally(function () {
                  // vm.saving = false;
                  //$("html, body").animate({ scrollTop: 0 }, "slow");
              });
        };

        function userLogin(model) {
            gm.saving = false;
            gm.errorMessage = null;
            gm.success = false;
            $(".alertBlock").fadeIn();
            $http.post(globalConfig.signIn, model)
            .success(function (data) {
                if (data) {
                    $("#login-modal").modal('hide');
                    if ($scope.global_login)
                        window.location.reload();
                    else
                        window.location.href = window.location.origin + data.returnUrl;
                }
            })
            .error(function (msg) {
                gm.errorMessage = msg.errorMessages;
                $timeout(function () {
                    $(".alertBlock").fadeOut();
                }, 3000);

            })
            .finally(function () {
                gm.saving = false;
            });
        }

        function registration(model) {
            $scope.global_login = false;
            $scope.signin = false;
            $scope.register = true;
            gm.saving = false;
            gm.errorMessage = null;
            gm.success = false;
            $(".alertBlock").fadeIn();

            $http.post(globalConfig.register, model)
            .success(function (data) {
                if (data) {
                    window.location.href = "/MyAccount";
                }
            })
            .error(function (msg) {
                gm.errorMessage = msg.errorMessages;
            })
            .finally(function () {
                $timeout(function () { $(".alertBlock").fadeOut(); }, 3000);
            });
        }


        function getShippingMethods(countryCode) {

            $http.post(globalConfig.getShippingMethods, { 'countryCode': countryCode })
             .success(function (data) {
                 gm.basketResponse.shippingMethods = data;
             })
             .error(function (msg) {
                 // vm.errorMessage = msg.errorMessages;
             })
             .finally(function () {
                 // vm.saving = false;
                 //$("html, body").animate({ scrollTop: 0 }, "slow");
             });


        }

        function updateShipping(id) {
            $http.post(globalConfig.updateShipping, { id: gm.basketResponse.id, shippingId: id, nominatedDelivery: null })
            .success(function (data) {
                gm.basketResponse = data;
            })
            .error(function (msg) {
                // vm.errorMessage = msg.errorMessages;
            })
            .finally(function () {
                // vm.saving = false;
                //$("html, body").animate({ scrollTop: 0 }, "slow");
            });
        };

        function getallblogs(page) {
            location.href = 'GetAllBlogs?currentpage=' + page + ''
        }

        function getallblogsbycategory(page, category) {

            location.href = 'GetBlogByCategory?category=' + category + '&currentpage=' + page + ''
        }

        function getBlogByCategory(id, page) {
            $http.post(globalConfig.getBlogByCategory, { category: id, currentpage: page })
           .success(function (data) {

               gm.basketResponse = data;

           })
           .error(function (msg) {
               // vm.errorMessage = msg.errorMessages;
           })
           .finally(function () {
               // vm.saving = false;
               //$("html, body").animate({ scrollTop: 0 }, "slow");
           });


        }

        function getBlogByCategory(id, page) {

            $http.post(globalConfig.getBlogsbyCategory, { category: id, currentpage: page })
           .success(function (data) {

               gm.blogReponse = data;

           })
           .error(function (msg) {
               // vm.errorMessage = msg.errorMessages;
           })
           .finally(function () {
               // vm.saving = false;
               //$("html, body").animate({ scrollTop: 0 }, "slow");
           });


        }

        function initblogs(id) {
            $http.post(globalConfig.getallblogs, { id: id })
          .success(function (data) {

              gm.blogReponse = data;

          })
          .error(function (msg) {
              // vm.errorMessage = msg.errorMessages;
          })
          .finally(function () {
              // vm.saving = false;
              //$("html, body").animate({ scrollTop: 0 }, "slow");
          });
        }

        function currencySettings(value1, value2, value3) {
            var model = { 'Currency': value1, "Language": value2, "Country": value3 };
            $http.post(globalConfig.currencySettingUrl, model)
               .success(function () {
                   $('#currency-modal').modal('toggle');
                   window.location.reload();
               })
               .error(function (msg) {
               })
               .finally(function () {
               });
        };

        function getPaymentMethods() {
            $http.post(globalConfig.paymentMethodsUrl)
               .success(function (data) {
                   gm.paymentMethods = data;
               })
               .error(function (msg) {
               })
               .finally(function () {
               });
        };

        function login(model) {
            $scope.signin = true;
            $scope.register = false;
            $scope.global_login = false;
            gm.userLogin(model);
        }
        function globalLogin(model) {
            $scope.signin = false;
            $scope.register = false;
            $scope.global_login = true;
            gm.userLogin(model);
        }

        function newsLetterSubscription(email) {
            gm.emailinvalid = false;
            gm.subssuccess = false;
            var email_regex = /^[_a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$/;
            if (!email_regex.test(email.toLowerCase())) {
                gm.emailinvalid = true;
                $('.newsletteralert').show(0).delay(2000).hide(0);
            }
            if (email == "" || email == null) {
                gm.emailinvalid = true;
                $('.newsletteralert').show(0).delay(2000).hide(0);
            }
            if (gm.emailinvalid == false) {
                $http.post(globalConfig.newsLetterSubscription, { email: email })
                  .success(function (data) {
                      gm.customerEmail = '';
                      if (data == true) {
                          gm.subssuccess = true;
                          $('.newslettersuccess').show(0).delay(2000).hide(0);
                      }
                      else {
                          gm.alreadySubscribed = true;
                          $('.newsletteralready').show(0).delay(4000).hide(0);
                      }
                  })
                  .error(function (msg) {
                      gm.alreadySubscribed = true;
                      $('.newsletteralert').show(0).delay(2000).hide(0);
                  })
                  .finally(function () {

                  });
            }

        }

        function showBasket(value) {
            if (value)
                $('#shoppingCart').addClass('active');
            else
                $("#shoppingCart").removeClass("active");
        }

        function showAccount(value) {
            if (value)
                $('#accountDrop').addClass('active');
            else
                $("#accountDrop").removeClass("active");
        };

        function removePromoCode(id, promoCode) {
            $http.post(globalConfig.removePromoCode, { id: id, promoCode: promoCode })
                 .success(function (data) {
                     gm.basketResponse = data.result.basket;
                 })
                 .error(function (msg) {
                 })
                 .finally(function () {
                 });
        };

        function forgotPassword(model) {
            $http.post(globalConfig.forgotPassword, model)
                 .success(function (data) {
                     gm.errorMessage = null;
                     gm.isValid = data.isValid;
                     gm.isValiduser = !data.isValid;
                     $timeout(function () {
                         gm.isValiduser = false;
                         gm.isValid = false;
                         window.location.href = '/account/signin';
                     }, 3000);
                 })
                 .error(function (msg) {
                     gm.errorMessage = msg.errorMessages;
                     $timeout(function () {
                         gm.isValiduser = false;
                         $(".alertBlock").fadeOut();
                     }, 3000);
                 })
                 .finally(function () {
                 });
        };

        function openQuickBasketModal() {
            console.log("1");
            $("#AddToBasketModel").modal();
        };
        function nRows(num) {
            return new Array(num);
        }
        function addProductsInBasket(line) {
            gm.errorMessage = '';
            angular.forEach(line.stockCode, function (item, key) {
                if (item != undefined) {
                    angular.forEach(line.stockCode, function (i, k) {
                        if (i!=undefined && item == i && key != k) {
                            gm.errorMessage = "Duplicate stockCode entry in rows " + (parseInt(key, 10) + 1) + " and " + (parseInt(k, 10) + 1);
                        }
                    });
                }
            });
            if (gm.errorMessage) {
                $('.stockError').show(0).delay(3000).hide(0);
                return;
            }
            if (line.stockCode == undefined || line.qty == undefined) {
                gm.errorMessage = "Wrong Input format";
                $('.stockError').show(0).delay(3000).hide(0);
            }
            $scope.bulkOrder = [];
            angular.forEach(line.stockCode, function (value, key) {
                $scope.bulkOrder.push({ stockCode: line.stockCode[key], qty: line.qty[key], basketId: "" })
            });
            $http.post(globalConfig.bulkAddproduct, $scope.bulkOrder)
              .success(function (data) {
                  gm.basketResponse = data.result;
                  var count = 0;
                  if (gm.basketResponse != null) {
                      if (gm.basketResponse.lineItems != null) {
                          angular.forEach(gm.basketResponse.lineItems, function (line) {
                              if (line.parentProductId == gm.emptyGuid) {
                                  count = count + line.qty;
                              }
                          });
                          gm.count = count;
                      }
                  }
                  if (data.message) {
                      gm.errorMessage = data.message;
                      $('.stockError').show(0).delay(3000).hide(0);
                  }
                  else {
                      $("#AddToBasketModel").modal("hide");
                      $("html, body").animate({ scrollTop: 0 }, "slow");
                      $('.cartopen').addClass('active');
                      $timeout(function () { $(".cartopen").removeClass("active"); }, 3000);
                  }
              }).error(function () {
                  gm.errorMessage = "Wrong Input format";
                  $('.stockError').show(0).delay(3000).hide(0);
              });
        }
        function generateTable() {
            var data = $('textarea[name=excel_data]').val();
            var rows = data.split("\n");
            var table = $('<table />');
            for (var y in rows) {
                var cells = rows[y].split("\t");
                var row = $('<tr />');
                for (var x in cells) {
                    row.append('<td>' + cells[x] + '</td>');
                }
                table.append(row);
            }
            $('#excel_table').html(table);
        }

        function addProductsExcel(line) {
            gm.errorMessage = '';            
            $scope.bulkOrder = [];
            var rows = line.split("\n");
            angular.forEach(rows, function (value, key) {
                $scope.bulkOrder.push({ stockCode: rows[key].split(",")[0], qty: rows[key].split(",")[1], basketId: "" })
            });
            angular.forEach($scope.bulkOrder, function (item, key) {
                if (item != undefined) {
                    angular.forEach($scope.bulkOrder, function (i, k) {
                        if (i!= undefined && item.stockCode == i.stockCode && key != k) {
                            gm.errorMessage = "Duplicate stockCode added.";
                        }
                    });
                }
            });
            if (gm.errorMessage) {
                $('.stockError').show(0).delay(3000).hide(0);
                return;
            }
            $http.post(globalConfig.bulkAddproduct, $scope.bulkOrder)
               .success(function (data) {
                   gm.basketResponse = data.result;
                   var count = 0;
                   if (gm.basketResponse != null) {
                       if (gm.basketResponse.lineItems != null) {
                           angular.forEach(gm.basketResponse.lineItems, function (line) {
                               if (line.parentProductId == gm.emptyGuid) {
                                   count = count + line.qty;
                               }
                           });
                           gm.count = count;
                       }
                   }
                   //if (data.message.split(",").length > 1) {
                   //    gm.errorMessage = data.message.split(",")[1] + " " + "does not exist";
                   //    $('.stockError').show(0).delay(3000).hide(0);
                   //}
                   if (data.message) {
                       gm.errorMessage = data.message;
                       $('.stockError').show(0).delay(3000).hide(0);
                   }
                   else {
                       $("#AddToBasketModel").modal("hide");
                       $("html, body").animate({ scrollTop: 0 }, "slow");
                       $('.cartopen').addClass('active');
                       $timeout(function () { $(".cartopen").removeClass("active"); }, 3000);
                   }
               }).error(function () {
                   gm.errorMessage = "Wrong Input format";
                   $('.stockError').show(0).delay(3000).hide(0);
               });
        };
    };
})();