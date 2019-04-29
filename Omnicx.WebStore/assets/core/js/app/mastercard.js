//Start ===============================================Added methods for MasterCard Implementation===================================================
var sessionId = "";
var currency = 'GBP';
var notificationUrl = 'mastercard/notification';
var check3DSecureUrl = 'mastercard/check3Dsecure';
var authUrl = 'mastercard/authorize';
var cardVarifyUrl = 'mastercard/varify';
var month = null;
var year = null;
var amount = null;
var name = null;
var orderId = null;
var transId = null;
var basketId = null;
var secure3dId = null;
var saveToken = false;
var sessionResCount = 0;
var isDiscoveryOrder = false;
var ERRORSMASTER = {
    failedToVerify: 'Fail to Verify Card',
    failedToCallApi: 'Fail To Call API',
    sessionExpired: 'Session Expired',
    sessionError: ' Session Error',
    sessionUpdateFail: 'Session Update Fail',
    failToVerify3DSecure: ' FailTo Verify 3D',
    unknownError: ' Error'

};
var today = new Date();
yyyy = today.getFullYear();
yyyy = yyyy.toString().substr(2, 2);
inpYear = $('#expiry-year');
html = '<option value="00">' + 'YY' + '</option>';
for (var i = 0; i < 18; i++, yyyy++) {
    html = html + '<option>' + yyyy + '</option>';
};
inpYear.html(html);





if (typeof PaymentSession !== 'undefined') {
   
    PaymentSession.configure({
        fields: {
            // ATTACH HOSTED FIELDS TO YOUR PAYMENT PAGE
            cardNumber: "#card-number",
            securityCode: "#security-code",
            expiryMonth: "#expiry-month",
            expiryYear: "#expiry-year"
        },
        //SPECIFY YOUR MITIGATION OPTION HERE
        frameEmbeddingMitigation: ["javascript"],
        callbacks:
        {
            initialized: function (response) {
                //console.log(response)
                if (response.status === 'ok') {
                    //alert('dd');
                    // $("#logdiv").html("Initialised OK");
                    //$(".gw-proxy-cardNumber").css("width", "21px");
                    //$(".gw-proxy-cardNumber").css("height", "173px");
                    //$(".gw-proxy-securityCode").css("width", "12px");
                    //$(".gw-proxy-securityCode").css("height", "40px");
                }
                else {
                   
                    // $("#logdiv").html("Initialisation Failure.");
                }

                // HANDLE INITIALIZATION RESPONSE
            },
            formSessionUpdate: function (response) {
                sessionResCount = sessionResCount + 1;
                if (sessionResCount > 1) {
                    return;
                }
                // HANDLE RESPONSE FOR UPDATE SESSION
                //console.log(response);
                if (response.status) {
                    if ("ok" == response.status) {
                        $('#card-number').removeClass('ng-invalid-required');
                        $('#card-numberError').css("display", "none");
                        sessionId = response.session.id;
                        paysecure();
                    }
                    else if ("fields_in_error" == response.status) {
                        $(".alertDynamicBlock").fadeIn();
                      
                        resetForm();
                        if (response.errors.cardNumber) {
                            $('#card-number').addClass('ng-invalid-required');
                            showErrorMessage('There has been an issue confirming payment, please check and try again.');
                            //$("#payment-error").html('There has been an issue confirming payment, please check and try again.');
                            $('#card-numberError').css("display", "block");
                        }
                        else {
                            $('#card-numberError').css("display", "none");
                            $('#card-number').removeClass('ng-invalid-required');
                        }

                        if (response.errors.securityCode) {
                            $('#security-code').addClass('ng-invalid-required');
                             showErrorMessage('There has been an issue confirming payment, please check and try again.');
                        } else {
                            $('#security-code').css("display", "none");
                            $('#security-code').removeClass('ng-invalid-required');
                        }
                    }
                    else if ("request_timeout" == response.status) {
                        //showErrorMessage(sessionExpired);
                        // alerts.error(sessionExpired);
                        $('body').scrollTop(0);
                        window.location.href = window.location.href.replace("amp;", "");
                    }
                    else if ("system_error" == response.status) {
                        //showErrorMessage(sessionExpired);
                        $('body').scrollTop(0);
                        window.location.href = window.location.href.replace("amp;", "");
                    }
                }
                else {
                    showErrorMessage(sessionUpdateFail);
                    $('body').scrollTop(0);
                    window.location.href = window.location.href.replace("amp;", "");
                }
            }
        }
    });
}

function masterCardPay(ordId, bskId, transactionId, cur, sec3dId, orderAmount, responseUrl) {
    basketId = bskId;
    notificationUrl = responseUrl;
    sessionResCount = 0;
    // $("#logdiv").empty();
    validateForm();
    orderId = ordId;
    transId = transactionId;
    currency = cur;
    secure3dId = sec3dId;
    amount = orderAmount;
    //$(".btnOrderPayment").attr("disabled", "disabled");
    PaymentSession.updateSessionFromForm();

}


function paysecure() {
    month = $('#expiry-month').val();
    year = $('#expiry-year').val();
    name = $('#name-on-card').val();
    saveToken = true;

    // set the return URL for the 3dsecure page

    var secure3dauthUrl = notificationUrl + '?sessionId=' + sessionId +
                                                        "&month=" + encodeURIComponent(month) +
                                                        "&year=" + encodeURIComponent(year) +
                                                        "&amount=" + encodeURIComponent(amount) +
                                                        "&name=" + encodeURIComponent(name) +
                                                        "&orderId=" + encodeURIComponent(orderId) +
                                                        "&transId=" + encodeURIComponent(transId) +
                                                        "&secure3dId=" + encodeURIComponent(secure3dId) +
                                                        "&saveToken=" + encodeURIComponent(saveToken) +
                                                        "&currency=" + encodeURIComponent(currency) +
                                                        "&isDiscoveryOrder=" + encodeURIComponent(isDiscoveryOrder);

    // call the 3dsecure check
    //console.log(secure3dauthUrl);
    $(".dvloader").attr("style", "");
    $.ajax({
        type: "POST",
        url: baseUrl + check3DSecureUrl ,
        data: '{"sessionId": "' + sessionId + '"' +
            ', "amount": "' + amount + '"' +
            ', "currency": "' + currency + '"' +
            ', "secure3DauthUrl": "' + secure3dauthUrl + '"' +
            ', "secure3DId": "' + secure3dId + '"}',

        contentType: "application/json", // content type sent to server
        success: check3dsecure_enrollment_success,
        error: check3dsecure_enrollment_failure
    });
}

function check3dsecure_enrollment_success(data) {
    $(".dvloader").attr("style", "display:none");

    if (data === null) {
        // call authorize
        //varify(orderId, amount, transId, secure3dId, currency);
        authorize(year, month, amount, orderId, transId);
        return;
    }

    if (data.error != null || data.error != undefined) {
        //if (data.error.cause != null) {
        //    $("#logdiv").html(JSON.stringify(data));
        //}
        showErrorMessage(data.error);        
        //$('body').scrollTop(0);       
        return;
    }

    if (data.summaryStatus === "CARD_ENROLLED") {
        // redirect to paysecure if the card is enrolled
        //varify(orderId, amount, transId, secure3dId, currency);
        if (data.htmlBodyContent != null && data.htmlBodyContent != undefined) {
            var newDoc = document.open("text/html", "replace");
            newDoc.write(data.htmlBodyContent);
            newDoc.close();
        }
    }
    else
        // card is not enrolled or does not support 3Ds or the Auth server is down or not working correctly
        if (data.summaryStatus === "CARD_NOT_ENROLLED" || data.summaryStatus === "CARD_DOES_NOT_SUPPORT_3DS"
                || data.summaryStatus === "AUTHENTICATION_NOT_AVAILABLE" || data.summaryStatus === "AUTHENTICATION_ATTEMPTED") {
            // call authorize
            authorize(year, month, amount, orderId, transId);
            //varify(orderId, amount, transId, secure3dId, currency);
        }
        else {
            //varify(orderId, amount, transId, secure3dId, currency);
            //$("#logdiv").html("<br/>Unknown 3DSecure Card Status<br />");
            authorize(year, month, amount, orderId, transId);
        }
}

function check3dsecure_enrollment_failure() {
    $(".dvloader").attr("style", "display:none");
    showErrorMessage(failToVerify3DSecure);
    //alerts.error(failToVerify3DSecure);
    $('body').scrollTop(0);
}

function authorize(year, month, amount, orderId, transId) {
    $(".dvloader").attr("style", "");
    $.ajax({
        type: "POST",
        url: baseUrl + authUrl ,
        data: '{"sessionId": "' + sessionId + '"' +
            ', "year": "' + year + '"' +
            ', "month": "' + month + '"' +
            ', "amount": "' + amount + '"' +
            ', "orderId": "' + orderId + '"' +
            ', "transId": "' + transId + '"' +
            ', "currency": "' + currency + '"}',

        contentType: "application/json", // content type sent to server
        success: authorizeAPISuccess,
        error: authorizeAPIFailure
    });
}



function varify(orderId, amount, transId, secure3dId, cur) {
    $(".dvloader").attr("style", "");
    currency = cur;
    month = $('#expiry-month').val();
    year = $('#expiry-year').val();
    $.ajax({
        type: "POST",
        url: baseUrl + cardVarifyUrl ,
        data: '{"sessionId": "' + sessionId + '"' +
            ', "year": "' + year + '"' +
            ', "month": "' + month + '"' +
            ', "amount": "' + amount + '"' +
            ', "orderId": "' + orderId + '"' +
            ', "transId": "' + transId + '"' +
            ', "currency": "' + currency + '"' +
            ', "secure3DId": "' + secure3dId + '"}',

        contentType: "application/json", // content type sent to server
        success: verifyAAPISuccess,
        error: verifyAPIFailure
    });
}

function verifyAPIFailure(result) {
    showErrorMessage(failedToVerify);
    //alerts.error(failedToVerify);
    $('body').scrollTop(0);
    $(".dvloader").attr("style", "display:none");
}

function verifyAAPISuccess(result) {
    $(".dvloader").attr("style", "display:none");
    if (result.isValid) {
        authorize(year, month, amount, orderId, transId);
    } else {
        $('.btnOrderPayment').removeAttr('disabled');
        showErrorMessage(result.message);
        // alerts.error(result.message);
        $('body').scrollTop(0);
        resetForm();
    }

}

function authorizeAPIFailure(result) {
    $(".dvloader").attr("style", "display:none");
    showErrorMessage(failedToCallApi);
    //alerts.error(failedToCallApi);
    $('body').scrollTop(0);
    resetForm();
}

function authorizeAPISuccess(result) {
    $("#logdiv").html("");
    if (result != null && result.errorString != null) {
        if (result.errorString == '') {
            month = $('#expiry-month').val();
            year = $('#expiry-year').val();
            name = $('#name-on-card').val();
            saveToken = false;
            if ($("#save-token").prop('checked') === true) {
                saveToken = true;
            }

            // set the return URL for the 3dsecure page
            var postUrl =  notificationUrl  + '?sessionId=' + sessionId +
                //"&bid=" + encodeURIComponent(basketId) +
                "&month=" + encodeURIComponent(month) +
                "&year=" + encodeURIComponent(year) +
                "&amount=" + encodeURIComponent(amount) +
                "&name=" + encodeURIComponent(name) +
                "&orderId=" + encodeURIComponent(orderId) +
                "&transId=" + encodeURIComponent(transId) +
                "&secure3dId=" + encodeURIComponent(secure3dId) +
                "&saveToken=" + encodeURIComponent(saveToken) +
                "&currency=" + encodeURIComponent(currency);


            window.location.href = postUrl;
        } else {
            showErrorMessage(result.errorString.replace('_', ' '));

            //alerts.error(result.errorString.replace('_', ' '));
            $('body').scrollTop(0);
            resetForm();
        }
    }
    else {
        showErrorMessage(result.errorString);
        // alerts.error(unknownError);
        $('body').scrollTop(0);
        resetForm();
    }
}

function showErrorMessage(error) {    
    $(".divErrMsg").find("span").html(error);
    $(".divErrMsg").removeClass('hide');
    $("html, body").animate({ scrollTop: 0 }, "slow");
    setTimeout(function () { $(".alertDynamicBlock").fadeOut(); $(".divErrMsg").addClass('hide'); }, 10000);
    $(".dvloader").attr("style", "display:none");
}
function resetForm() {
    $('#expiry-month').val('00');
    $('#expiry-year').val('00');
    $('#name-on-card').val('');
    $('iframe').attr('src', function (i, val) { return val; });
}
function validateForm() {
    month = $('#expiry-month').val();
    year = $('#expiry-year').val();
    name = $('#name-on-card').val();


    var isMasterCardValidate = false;
    var isValidMonth = false;
    var isValidYear = false;
    var isValidName = false;

    if (month == '00') {
        $('#expiry-month').addClass('ng-invalid-required');
        $('#expiry-monthError').css("display", "block");
    }
    else {
        isValidMonth = true;
    }
    if (year == '00') {
        $('#expiry-year').addClass('ng-invalid-required');
        $('#expiry-yearError').css("display", "block");
    }
    else {
        isValidYear = true;
    }
    if (name == '') {
        $('#name-on-card').addClass('ng-invalid-required');
        $('#name-on-cardError').css("display", "block");
    }
    else {
        isValidName = true;
    }



    if (isValidMonth == true && isValidYear == true && isValidName == true) {
        isMasterCardValidate = true;
    }
    return isMasterCardValidate;
}

function removeClass(selector) {
    if (selector != undefined) {
        if (selector.id == 'expiry-year' || selector.id == 'expiry-month') {
            if ($('#' + selector.id).val() != '00') {
                $('#' + selector.id).removeClass('ng-invalid-required');
                $('#' + selector.id + 'Error').css("display", "none");
            }
        }
        else {
            if ($('#' + selector.id).val() != '') {
                $('#' + selector.id).removeClass('ng-invalid-required');
                $('#' + selector.id + 'Error').css("display", "none");
            }
        }
    }
}


function maxLengthCheck(object) {
    if (object.value.length > object.maxLength)
        object.value = object.value.slice(0, object.maxLength);
}
//End ===============================================Added methods for MasterCard Implementation===================================================