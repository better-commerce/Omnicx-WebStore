using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Omnicx.API.SDK.Helpers;
using Omnicx.WebStore.Models.Commerce;
using Omnicx.WebStore.Models.Common;

namespace Omnicx.API.SDK.Payments.MasterCard
{
   public class MasterCardApi
    {
        private readonly string _baseUrl = string.Empty;
        private readonly string _apiver = string.Empty;
        private readonly string _merchantId = string.Empty;
        private readonly string _username = string.Empty;
        private readonly string _password = string.Empty;
        private readonly string _signature = string.Empty;

        public MasterCardApi(string baseUrl, string apiver, string merchantid, string username, string password)
        {
            _baseUrl = baseUrl;
            _apiver = apiver;
            _merchantId = merchantid;
            _username = username;
            _password = password;
        }

       public MasterCardApi(PaymentMethodModel setting)
       {
           _baseUrl = setting.UseSandbox == "True" ? setting.TestUrl : setting.ProductionUrl;
           _apiver = setting.Version;
           _merchantId = setting.AccountCode;
           _username = setting.UserName;
           _password = setting.Password;
           _signature = setting.Signature;
       }
        public static string GetTimestamp(string signature,string orderRefNo,decimal amount,string currency )
        {
           var dt = DateTime.Now;
           string timestamp = String.Format("{0:yyyyMMddhhmmss}", dt);
           timestamp = Utils.GenerateSHA1Hash(timestamp + "." + signature + "." + orderRefNo + "." + Convert.ToInt32(amount * 100).ToString() + "." + currency, "");
           return timestamp;
        }

        public  Secure3DResponse Check3DSecureEnrollment(string sessionId, decimal amount, string currency, string secure3DauthUrl,
             string secure3DId)
       {
           var errorResponse = new ErrorResponse();
           var secure3D = SECURE3D_CheckEnrollment(sessionId, amount, currency, secure3DauthUrl, secure3DId, ref errorResponse);
            return secure3D;
       }

        public BoolResponse CaptureNotification(ref PaymentModel payment,Billing billingAddress)
       {
           var response = new BoolResponse { IsValid = false, Message = "" };
           TransResponse authResponse = null;
           var request = HttpContext.Current.Request;
            string sessionId = "";
            try
            {
                if (request["orderId"] != null)
                {
                    string paRes = String.Empty;
                    sessionId = request.Params["sessionId"] ?? null;
                    string year = request.Params["year"] ?? null;
                    string month = request.Params["month"] ?? null;
                    string amount = request.Params["amount"] ?? null;
                    string name = request.Params["name"] ?? null;
                    string orderId = request.Params["orderId"] ?? null;
                    string transId = request.Params["transId"] ?? null;
                    string secure3dId = request.Params["secure3dId"] ?? null;
                    string currency = request.Params["currency"] ?? null;
                    if (request.Params["PaRes"] != null)
                    {
                        paRes = request.Params["PaRes"];
                    }

                    string orderNo = orderId;
                    string paymentId = orderNo.Split('-')[1];
                    orderNo = orderNo.Split('-')[0];
                    decimal decAmount = 0;

                    try
                    {
                        decAmount = decimal.Parse(amount);
                    }
                    catch (Exception amountEx)
                    {
                        response.Message = "Failed to parse amount:" + amountEx.Message;
                        return response;
                    }
                    Secure3DResponse secure3dresponse = null;
                    try
                    {
                        var errorResponse = new ErrorResponse();
                      if(!string.IsNullOrWhiteSpace(paRes))//Added check if "paRes" is null
                        {
                            secure3dresponse = SECURE3D_ProcessACSResult(paRes, secure3dId, ref errorResponse);
                        }
                        if (secure3dresponse != null)
                        {
                            payment.Secure3DResult = secure3dresponse.gatewayCode + " ";
                            if (errorResponse.error != null)
                            {
                                response.Message = errorResponse.error.explanation;
                            }
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        response.Message = "Exception in SECURE3D_ProcessACSResult():" + ex.Message;
                        return response;
                    }
                    if (secure3dresponse == null || secure3dresponse.summaryStatus == null)
                    {
                        var errorResponse = new ErrorResponse();
                        authResponse = TRANSACTION_Authorize(sessionId, null, year, month, decAmount, orderId, transId, currency,
                                                                             null, null, billingAddress, null, null, null, ref errorResponse);


                        //tokenId = "";// CreateToken(sessionId, mcard);
                        response.IsValid = true;
                        payment.PspSessionCookie = transId;
                        payment.CardHolderName = name;
                    }
                    if (secure3dresponse!=null && secure3dresponse.summaryStatus != null)
                    {
                        bool doAuth = false;

                        switch (secure3dresponse.summaryStatus)
                        {
                            case "CARD_NOT_ENROLLED":
                                doAuth = true;
                                break;
                            case "CARD_ENROLLED":
                                doAuth = true;
                                break;
                            case "AUTHENTICATION_NOT_AVAILABLE":
                                doAuth = true;
                                break;
                            case "AUTHENTICATION_SUCCESSFUL":
                                doAuth = true;
                                break;
                            case "AUTHENTICATION_FAILED":
                                doAuth = false;
                                break;
                            case "AUTHENTICATION_ATTEMPTED":
                                doAuth = true;
                                break;
                            case "CARD_DOES_NOT_SUPPORT_3DS":
                                doAuth = true;
                                break;
                        }
                        if (doAuth)
                        {
                            var errorResponse = new ErrorResponse();

                            // verify the card
                            authResponse = TRANSACTION_Authorize(sessionId, null, year, month, decAmount, orderId, transId, currency, secure3dId, null, billingAddress, null, null, null, ref errorResponse);
                            if (errorResponse.error != null)
                            {
                                // response.Message = response.Message + "<br/>ERROR:" + JToken.Parse(JsonConvert.SerializeObject(errorResponse)).ToString(Newtonsoft.Json.Formatting.Indented) + "<br/>";
                                response.Message = response.Message + "<br/>ERROR in VERIFY: " + errorResponse.error.explanation;
                                return response;
                            }
                            if (authResponse != null)
                            {
                                if (authResponse.order.status == "AUTHORIZED")
                                {
                                    //tokenId = "";// CreateToken(sessionId, mcard);
                                    response.IsValid = true;
                                }
                            }
                        }
                    }
                }
                if (response.IsValid)
                {
                    payment.IsValid = true;
                    payment.Status = 1; //Authrised
                    payment.Token = "";
                    payment.AuthCode = sessionId;

                    payment.Secure3DResult = payment.Secure3DResult+ "CARD NOT ENROLLED";

                    payment.LastUpdatedBy = "MasterCardNotification";

                    payment.IsValid = true;
                    payment.PspResponseCode = authResponse.response.gatewayCode;
                    payment.AvsResult = authResponse.response.cardholderVerification.avs.gatewayCode;
                    payment.CvcResult = "NA";
                    payment.IssuerCountry = "";
                    payment.CardNo = "";
                    payment.PaymentMethod = "";

                    if (authResponse.sourceOfFunds != null)
                    {
                        payment.IssuerCountry = authResponse.sourceOfFunds.issuer;
                        payment.CardNo = authResponse.sourceOfFunds.number;
                        payment.PaymentMethod = authResponse.sourceOfFunds.scheme ?? "" + ">" + authResponse.sourceOfFunds.fundingMethod ?? "";
                        if (authResponse.sourceOfFunds.provided != null)
                        {
                            if (authResponse.sourceOfFunds.provided.card != null)
                            {
                                //  payment.IssuerCountry = authResponse.sourceOfFunds.provided.card.issuer;
                                payment.CardNo = authResponse.sourceOfFunds.provided.card.number;
                                payment.PaymentMethod = authResponse.sourceOfFunds.provided.card.scheme ?? "" + ">" + authResponse.sourceOfFunds.provided.card.fundingMethod ?? "";
                                payment.CvcResult = authResponse.response.cardSecurityCode.gatewayCode;
                            }
                        }
                    }
                    if (authResponse.risk != null)
                    {
                        if (authResponse.risk.response != null)
                        {
                            payment.FraudScore = authResponse.risk.response.totalScore.ToString();
                            //  payment.AVSResult = authResponse.risk.response.gatewayCode;
                        }
                    }
                 
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
           
           return response;
       }
        public PaymentResult Authorize(string sessionId, string year, string month, decimal amount, string orderId,string transId, string currency, Billing billingAddress)
        {
            var payResult = new PaymentResult();

            var errorResponse = new ErrorResponse();
            var shippingAddress = new Shipping()
            {
                address = billingAddress.address
            };

            TransResponse authResponse = TRANSACTION_Authorize(sessionId, null, year, month, amount, orderId, transId, currency, null, null, billingAddress, null, null, null, ref errorResponse);

            if (errorResponse.error != null)
            {
                payResult.errorString = errorResponse.error.explanation;
            }

            if (authResponse != null)
            {
                if (authResponse.response.cardholderVerification != null) { 
                payResult.AVSResult = authResponse.response.cardholderVerification.avs.gatewayCode;
                payResult.AVSResultCode = authResponse.response.cardholderVerification.avs.acquirerCode;
                }
                if ((authResponse.response.cardSecurityCode == null) || (authResponse.response.cardSecurityCode.gatewayCode != "MATCH" && authResponse.result.ToUpper() == "SUCCESS"))
                {
                    var voidErrorResponse = new ErrorResponse();
                    var resp = TRANSACTION_Void(orderId, transId, Guid.NewGuid().ToString(), ref voidErrorResponse);
                    payResult.result = MasterApiPaymentStatus.PAYMENT_FAILURE;
                    payResult.sessiondId = sessionId;
                    payResult.errorString = "There has been an issue confirming payment, please check and try again";
                    return payResult;
                }
                if (authResponse.result.ToUpper() == "SUCCESS")
                {
                    payResult.result = MasterApiPaymentStatus.PAYMENT_SUCCESS;
                    payResult.sessiondId = sessionId;
                    payResult.errorString = "";
                }
                else
                {
                    payResult.result = MasterApiPaymentStatus.PAYMENT_FAILURE;
                    payResult.sessiondId = sessionId;
                    payResult.errorString = "There has been an issue confirming payment, please check and try again";
                }
            }
            else
            {
                payResult.errorString = "There has been an issue confirming payment, please check and try again";
               
            }
            return payResult;
        }
        public SessionResponse SESSION_Retrieve(string sessionId, ref ErrorResponse errorResponse)
        {
           
         //   Debug.Log(Debug.DEBUG_INFO, "MasterCardAPI.SESSION_Retrieve()");

            string wsUrl = _baseUrl + "/api/rest/version/" + _apiver + "/merchant/" + _merchantId + "/session/" + sessionId;

            // call the webservice
            string sessionJsonResponse = CallWS(wsUrl, null, "GET");

            GetErrorResponse(sessionJsonResponse, ref errorResponse);

            // serialise the response
            if (sessionJsonResponse != null)
            {
                return JsonConvert.DeserializeObject<SessionResponse>(sessionJsonResponse);
            }
            else
            {
                return null;
            }
        }

        public GatewayStatus GATEWAY_CheckGateway(ref ErrorResponse errorResponse)
        {
           // Debug.Log(Debug.DEBUG_INFO, "MasterCardAPI.GATEWAY_CheckGateWay()");

            string wsUrl = _baseUrl + "/api/rest/version/" + _apiver + "/information";

            // call the webservice
            string gatewayJsonResponse = CallWS(wsUrl, null, "GET");

            GetErrorResponse(gatewayJsonResponse, ref errorResponse);

            // serialise the response
            if (gatewayJsonResponse != null)
            {
                return JsonConvert.DeserializeObject<GatewayStatus>(gatewayJsonResponse);
            }
            else
            {
                return null;
            }
        }

        public PaymentOptionsRepsonse GATEWAY_PaymentOptionsInquiry(string sessionId, string tokenId, ref ErrorResponse errorResponse)
        {
           // Debug.Log(Debug.DEBUG_INFO, "MasterCardAPI.GATEWAY_PaymentOptionsInquiry()");

            string wsUri = _baseUrl + "/api/rest/version/" + _apiver + "/merchant/" + _merchantId + "/paymentOptionsInquiry";

            JObject jPaymentOptionsRequest = null;

            if (sessionId != null)
            {
                jPaymentOptionsRequest = new JObject(
                      new JProperty("session",
                      new JObject(
                              new JProperty("id", sessionId)
                              )));

            }

            if (tokenId != null)
            {
                jPaymentOptionsRequest = new JObject(
                      new JProperty("sourceOfFunds",
                      new JObject(
                              new JProperty("token", tokenId)
                              )));
            }

            string response = null;

            // something wrong with the spec here as it says use a http get but also json parameters?
            try
            {
                response = CallWS(wsUri, jPaymentOptionsRequest.ToString(), "GET");
            }
            catch (Exception e)
            {
                //Debug.Log(Debug.DEBUG_ERROR, "MasterCardAPI.GATEWAY_PaymentOptionsInquiry(): Exception in CallWS():" + e.Message);
                throw e;
            }

            GetErrorResponse(response, ref errorResponse);

            if (response != null)
            {
                return JsonConvert.DeserializeObject<PaymentOptionsRepsonse>(response);
            }
            else
            {
                return null;
            }
        }

        public Secure3DResponse SECURE3D_CheckEnrollment(string sessionId, decimal amount, string currency, string authenticationRedirectUrl, string secure3dId, ref ErrorResponse errorResponse)
        {
           // Debug.Log(Debug.DEBUG_INFO, "MasterCardAPI.SECURE3D_CheckEnrollment()");

            // reform the uri for 3DSecure enrollment check
            string uri3ds = _baseUrl + "/api/rest/version/" + _apiver + "/merchant/" + _merchantId + "/3DSecureId/" + secure3dId;

            Secure3DRequest secure3dRequest = new Secure3DRequest();

            Secure3D secure3d = new Secure3D();
            Order order = new Order();
            AuthenticationRedirect authenticationRedirect = new AuthenticationRedirect();
            Session session = new Session();

            // set the sessionId
            session.id = sessionId;
            secure3dRequest.session = session;

            // set the redirect URL for 3dSecure
            authenticationRedirect.responseUrl = authenticationRedirectUrl;

            // set the basic order details
            order.amount = amount;
            order.currency = currency;


            secure3dRequest.apiOperation = "CHECK_3DS_ENROLLMENT";
            secure3dRequest.order = order;
            secure3dRequest.secure3d = secure3d;
            secure3d.authenticationRedirect = authenticationRedirect;


            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new LowercaseContractResolver();
            settings.NullValueHandling = NullValueHandling.Ignore;

            // serialise the secure3d object
            string secure3dJsonRequest = JsonConvert.SerializeObject(secure3dRequest, Formatting.Indented, settings);

            // call the webservice
            string secure3dJsonResponse = CallWS(uri3ds, secure3dJsonRequest, "PUT");

            // check for any json error response
            GetErrorResponse(secure3dJsonResponse, ref errorResponse);

            // serialise the response
            if (secure3dJsonResponse != null)
            {
                JObject o = JObject.Parse(secure3dJsonResponse);
                Secure3DResponse response = new Secure3DResponse();
                try
                {
                    response.gatewayCode = o.SelectToken("response.3DSecure.gatewayCode").ToString();
                }
                catch (Exception)
                {
                   // Debug.Log(Debug.DEBUG_WARNING, "MasterCardAPI.SECURE3D_CheckEnrollment(): Error getting response.3DSecure.gatewayCode");
                }

                try
                {
                    response.htmlBodyContent = o.SelectToken("3DSecure.authenticationRedirect.simple.htmlBodyContent").ToString();
                }
                catch (Exception)
                {
                  //  Debug.Log(Debug.DEBUG_WARNING, "MasterCardAPI.SECURE3D_CheckEnrollment(): Error getting 3DSecure.authenticationRedirect.simple.htmlBodyContent");
                }

                try
                {
                    response.id = o.SelectToken("3DSecureId").ToString();
                }
                catch (Exception)
                {
                   // Debug.Log(Debug.DEBUG_WARNING, "MasterCardAPI.SECURE3D_CheckEnrollment(): Error getting 3DSecureId");
                }

                try
                {
                    response.summaryStatus = o.SelectToken("3DSecure.summaryStatus").ToString();
                }
                catch (Exception)
                {
                    //Debug.Log(Debug.DEBUG_WARNING, "MasterCardAPI.SECURE3D_CheckEnrollment(): Error getting 3DSecure.summaryStatus");
                }

                try
                {
                    response.xid = o.SelectToken("3DSecure.xid").ToString();
                }
                catch (Exception)
                {
                   // Debug.Log(Debug.DEBUG_WARNING, "MasterCardAPI.SECURE3D_CheckEnrollment(): Error getting 3DSecure.xid");
                }

                return response;
            }
            else
            {
                return null;
            }
        }

        public Secure3DResponse SECURE3D_Retrieve3DSResult(string secure3dId, ref ErrorResponse errorResponse)
        {
            //Debug.Log(Debug.DEBUG_INFO, "MasterCardAPI.SECURE3D_Retrieve3DSResult()");

            // reform the uri for 3DSecure enrollment check
            string uri3ds = _baseUrl + "/api/rest/version/" + _apiver + "/merchant/" + _merchantId + "/3DSecureId/" + secure3dId;

            // call the webservice
            string secure3dJsonResponse = CallWS(uri3ds, null, "GET");

            GetErrorResponse(secure3dJsonResponse, ref errorResponse);

            // serialise the response
            if (secure3dJsonResponse != null)
            {
                JObject o = JObject.Parse(secure3dJsonResponse);
                Secure3DResponse response = new Secure3DResponse();
                try
                {
                    response.gatewayCode = o.SelectToken("response.3DSecure.gatewayCode").ToString();
                    response.htmlBodyContent = o.SelectToken("3DSecure.authenticationRedirect.simple.htmlBodyContent").ToString();
                    response.id = o.SelectToken("3DSecureId").ToString();
                    response.summaryStatus = o.SelectToken("3DSecure.summaryStatus").ToString();
                    response.xid = o.SelectToken("3DSecure.xid").ToString();
                }
                catch (Exception secure3dex)
                {
                    // Debug.Log(Debug.DEBUG_ERROR, "MasterCardAPI.SECURE3D_Retrieve3DSResult(): Exception in JSON SelectToken:" + secure3dex.Message);
                    throw secure3dex;
                }

                return response;
            }
            else
            {
                return null;
            }
        }

        public Secure3DResponse SECURE3D_ProcessACSResult(string paRes, string secure3dId, ref ErrorResponse errorResponse)
        {
           // Debug.Log(Debug.DEBUG_INFO, "MasterCardAPI.SECURE3D_ProcessACSResult()");

            string uri3ds = _baseUrl + "/api/rest/version/" + _apiver + "/merchant/" + _merchantId + "/3DSecureId/" + secure3dId;

            JObject jProcessACSRequest = new JObject(
                new JProperty("apiOperation", "PROCESS_ACS_RESULT"),
                new JProperty("3DSecure",
                new JObject(
                        new JProperty("paRes", paRes)
                        )));

            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new LowercaseContractResolver();
            settings.NullValueHandling = NullValueHandling.Ignore;

            // serialise the secure3d object
            string secure3dJsonRequest = JsonConvert.SerializeObject(jProcessACSRequest, Formatting.Indented, settings);

            // call the webservice
            string secure3dJsonResponse = CallWS(uri3ds, secure3dJsonRequest, "POST");

            // see if we have an ErrorResponse object instead of a 3dsecure object
            GetErrorResponse(secure3dJsonResponse, ref errorResponse);

            // serialise the response
            if (secure3dJsonResponse != null)
            {
                JObject o = JObject.Parse(secure3dJsonResponse);
                Secure3DResponse response = new Secure3DResponse();
                try
                {
                    response.gatewayCode = o.SelectToken("response.3DSecure.gatewayCode").ToString();
                    response.htmlBodyContent = o.SelectToken("3DSecure.authenticationRedirect.simple.htmlBodyContent").ToString();
                    response.id = o.SelectToken("3DSecureId").ToString();
                    response.summaryStatus = o.SelectToken("3DSecure.summaryStatus").ToString();
                    response.xid = o.SelectToken("3DSecure.xid").ToString();
                }
                catch (Exception ex)
                {
                  //  Debug.Log(Debug.DEBUG_ERROR, "MasterCardAPI.SECURE3D_ProcessACSResult(): Exception in JSON SelectToken: " + ex.Message);
                    throw ex;
                }

                return response;
            }
            else
            {
            //    Debug.Log(Debug.DEBUG_ERROR, "MasterCardAPI.SECURE3D_ProcessACSResult(): Null response");
                return null;
            }
        }

        public TokenResponse TOKENISATION_RetrieveToken(string tokenId, ref ErrorResponse errorResponse)
        {
           // Debug.Log(Debug.DEBUG_INFO, "MasterCardAPI.TOKENISATION_RetrieveToken()");

            string wsUri = _baseUrl + "/api/rest/version/" + _apiver + "/merchant/" + _merchantId + "/token/" + tokenId;

            string response = null;

            try
            {
                response = CallWS(wsUri, null, "GET");
            }
            catch (Exception e)
            {
               // Debug.Log(Debug.DEBUG_ERROR, "TOKENISATION_RetrieveToken() Exception:" + e.Message + e.StackTrace);
                throw e;
            }

            GetErrorResponse(response, ref errorResponse);

            if (response != null)
            {
                JObject o = JObject.Parse(response);

                TokenResponse tokenResponse = new TokenResponse();
                try
                {
                    tokenResponse.repositoryId = o.SelectToken("repositoryId").ToString();
                    tokenResponse.result = o.SelectToken("result").ToString();
                    tokenResponse.status = o.SelectToken("status").ToString();
                    tokenResponse.token = o.SelectToken("token").ToString();
                    tokenResponse.cardBrand = o.SelectToken("sourceOfFunds.provided.card.brand").ToString();
                    tokenResponse.cardExpiry = o.SelectToken("sourceOfFunds.provided.card.expiry").ToString();
                    tokenResponse.cardFundingMethod = o.SelectToken("sourceOfFunds.provided.card.fundingMethod").ToString();
                    tokenResponse.cardIssuer = o.SelectToken("sourceOfFunds.provided.card.issuer").ToString();
                    tokenResponse.cardNumber = o.SelectToken("sourceOfFunds.provided.card.number").ToString();
                    tokenResponse.cardScheme = o.SelectToken("sourceOfFunds.provided.card.scheme").ToString();
                    tokenResponse.cardType = o.SelectToken("sourceOfFunds.type").ToString();
                    tokenResponse.usageLastUpdated = o.SelectToken("usage.lastUpdated").ToString();
                    tokenResponse.usageLastUpdatedBy = o.SelectToken("usage.lastUpdatedBy").ToString();
                    tokenResponse.usageLastUsed = o.SelectToken("usage.lastUsed").ToString();
                    tokenResponse.verificationStrategry = o.SelectToken("verificationStrategy").ToString();
                }
                catch (Exception ex)
                {
                    throw ex;
                    // Debug.Log(Debug.DEBUG_ERROR, "MasterCardAPI.TOKENISATION_RetrieveToken() Exception parsing response: " + ex.Message);
                }

                return tokenResponse;
            }
            else
            {
                return null;
            }
        }

        public TokenResponse TOKENISATION_CreateToken(string sessionId, string fundType, ref ErrorResponse errorResponse)
        {
         //   Debug.Log(Debug.DEBUG_INFO, "MasterCardAPI.TOKENISATION_CreateToken()");
            string wsUri = _baseUrl + "/api/rest/version/" + _apiver + "/merchant/" + _merchantId + "/token";

            TokenRequest tokenRequest = new TokenRequest();

            SourceOfFunds sourceOfFunds = new SourceOfFunds();
            // this can be ACH, CARD or GIF_CARD
            sourceOfFunds.type = fundType;

            Session session = new Session();
            session.id = sessionId;

            tokenRequest.session = session;
            tokenRequest.sourceOfFunds = sourceOfFunds;

            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new LowercaseContractResolver();
            settings.NullValueHandling = NullValueHandling.Ignore;

            // serialise json with lowercase property names and indented
            string tokenJson = JsonConvert.SerializeObject(tokenRequest, Formatting.Indented, settings);

            string response = null;

            try
            {
                response = CallWS(wsUri, tokenJson, "POST");
            }
            catch (Exception e)
            {
                //Debug.Log(Debug.DEBUG_ERROR, "TOKENISATION_CreateToken() Exception: " + e.Message + e.StackTrace);
                throw e;
            }

            GetErrorResponse(response, ref errorResponse);

            if (response != null)
            {
                JObject o = JObject.Parse(response);
                TokenResponse tokenResponse = new TokenResponse();
                tokenResponse.gatewayCode = o.SelectToken("response.gatewayCode").ToString();
                tokenResponse.result = o.SelectToken("result").ToString();
                tokenResponse.status = o.SelectToken("status").ToString();
                tokenResponse.token = o.SelectToken("token").ToString();
                return tokenResponse;
            }
            else
            {
                return null;
            }
        }

        public TokenResponse TOKENISATION_DeleteToken(string tokenId, ref ErrorResponse errorResponse)
        {
          //  Debug.Log(Debug.DEBUG_INFO, "MasterCardAPI.TOKENISATION_DeleteToken()");

            string wsUri = _baseUrl + "/api/rest/version/" + _apiver + "/merchant/" + _merchantId + "/token/" + tokenId;

            string response = null;

            try
            {
                response = CallWS(wsUri, null, "DELETE");
            }
            catch (Exception e)
            {
                //  Debug.Log(Debug.DEBUG_ERROR, "MasterCardAPI.TOKENISATION_DeleteToken(): Exception in CallWS():" + e.Message);
                throw e;
            }

            GetErrorResponse(response, ref errorResponse);

            if (response != null)
            {
                JObject o = JObject.Parse(response);
                TokenResponse tokenResponse = new TokenResponse();
                tokenResponse.result = o.SelectToken("result").ToString();
                return tokenResponse;
            }
            else
            {
                return null;
            }
        }

        public CaptureResponse TRANSACTION_Capture(string orderId, string transId, decimal amount, string currency, ref ErrorResponse errorResponse)
        {
         //   Debug.Log(Debug.DEBUG_INFO, "MasterCardAPI.TRANSACTION_Capture()");

            string wsUri = _baseUrl + "/api/rest/version/" + _apiver + "/merchant/" + _merchantId + "/order/" + orderId + "/transaction/" + transId;

            CaptureRequest capture = new CaptureRequest();
            capture.apiOperation = "CAPTURE";

            Transaction transaction = new Transaction();
            transaction.amount = amount;
            transaction.currency = currency;

            capture.transaction = transaction;

            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new LowercaseContractResolver();
            settings.NullValueHandling = NullValueHandling.Ignore;

            // serialise json with lowercase property names and indented
            string authJson = JsonConvert.SerializeObject(capture, Formatting.Indented, settings);

            string response = null;

            try
            {
                response = CallWS(wsUri, authJson, "PUT");
            }
            catch (Exception e)
            {
                //Debug.Log(Debug.DEBUG_ERROR, "TRANSACTION_Capture() Exception: " + e.Message + e.StackTrace);
                throw e;
            }

            GetErrorResponse(response, ref errorResponse);

            if (response != null)
            {
                return JsonConvert.DeserializeObject<CaptureResponse>(response);
            }
            else
            {
                return null;
            }
        }

        public TransResponse TRANSACTION_Void(string orderId, string transId, string targetTransId, ref ErrorResponse errorResponse)
        {
           // Debug.Log(Debug.DEBUG_INFO, "MasterCardAPI.TRANSACTION_Void()");

            string wsUri = _baseUrl + "/api/rest/version/" + _apiver + "/merchant/" + _merchantId + "/order/" + orderId + "/transaction/" + transId;

            string response = null;

            JObject jvoidTransRequest = new JObject(
            new JProperty("apiOperation", "VOID"),
            new JProperty("transaction",
            new JObject(
                new JProperty("targetTransactionId", targetTransId)
                )));

            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new LowercaseContractResolver();
            settings.NullValueHandling = NullValueHandling.Ignore;

            // serialise the secure3d object
            string jsonRequest = JsonConvert.SerializeObject(jvoidTransRequest, Formatting.Indented, settings);

            try
            {
                response = CallWS(wsUri, jsonRequest, "PUT");
            }
            catch (Exception e)
            {
                //Debug.Log(Debug.DEBUG_ERROR, "MasterCardAPI.TRANSACTION_Void(): Exception in CallWS()" + e.Message);
                throw e;
            }

            // see if we have an ErrorResponse object
            GetErrorResponse(response, ref errorResponse);

            if (response != null)
            {
                TransResponse authResponse = null;

                try
                {
                    authResponse = JsonConvert.DeserializeObject<TransResponse>(response);
                }
                catch (Exception authEx)
                {
                    //   Debug.Log(Debug.DEBUG_ERROR, "MasterCardAPI.TRANSACTION_Void(): Exception deserialising json response" + authEx.Message);
                    throw authEx;
                }

                return authResponse;
            }
            else
            {
                return null;
            }
        }

        public TransResponse TRANSACTION_UpdateAuthorization(string orderId, string transId, ref ErrorResponse errorResponse)
        {
           // Debug.Log(Debug.DEBUG_INFO, "MasterCardAPI.TRANSACTION_UpdateAuthorization()");
            return TRANSACTION_UpdateAuthorization(orderId, transId, -1, ref errorResponse);
        }

        public TransResponse TRANSACTION_UpdateAuthorization(string orderId, string transId, decimal amount, ref ErrorResponse errorResponse)
        {
           // Debug.Log(Debug.DEBUG_INFO, "MasterCardAPI.TRANSACTION_UpdateAuthorization()");

            string wsUri = _baseUrl + "/api/rest/version/" + _apiver + "/merchant/" + _merchantId + "/order/" + orderId + "/transaction/" + transId;

            JObject jUpdateAuth = null;
            if (amount > 0)
            {
                jUpdateAuth = new JObject(
                     new JProperty("apiOperation", "UPDATE_AUTHORIZATION"),
                     new JProperty("transaction",
                     new JObject(
                             new JProperty("amount", amount.ToString())
                             )));
            }
            else
            {
                jUpdateAuth = new JObject(
                     new JProperty("apiOperation", "UPDATE_AUTHORIZATION")
                     );
            }

            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new LowercaseContractResolver();
            settings.NullValueHandling = NullValueHandling.Ignore;

            // serialise the secure3d object
            string updateAuthJsonRequest = JsonConvert.SerializeObject(jUpdateAuth, Formatting.Indented, settings);

            // call the webservice
            string updateAuthJsonResponse = CallWS(wsUri, updateAuthJsonRequest, "PUT");

            // see if we have an ErrorResponse object instead of a 3dsecure object
            GetErrorResponse(updateAuthJsonResponse, ref errorResponse);

            // serialise the response
            if (updateAuthJsonResponse != null)
            {
                return JsonConvert.DeserializeObject<TransResponse>(updateAuthJsonResponse);
            }
            else
            {
              //  Debug.Log(Debug.DEBUG_ERROR, "MasterCardAPI.TRANSACTION_UpdateAuthorization(): Null response");
                return null;
            }
        }

        public TransResponse TRANSACTION_Verify(string orderId, string transId, TransRequest transRequest, ref ErrorResponse errorResponse)
        {
          //  Debug.Log(Debug.DEBUG_INFO, "MasterCardAPI.TRANSACTION_Verify()");

            string wsUrl = _baseUrl + "/api/rest/version/" + _apiver + "/merchant/" + _merchantId + "/order/" + orderId + "/transaction/" + transId;

            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new LowercaseContractResolver();
            settings.NullValueHandling = NullValueHandling.Ignore;

            string verifyJsonRequest = JsonConvert.SerializeObject(transRequest, Formatting.Indented, settings);

            string verifyJsonResponse = CallWS(wsUrl, verifyJsonRequest, "PUT");

            GetErrorResponse(verifyJsonResponse, ref errorResponse);

            if (verifyJsonResponse != null)
            {
                return JsonConvert.DeserializeObject<TransResponse>(verifyJsonResponse);
            }
            else
            {
              //  Debug.Log(Debug.DEBUG_ERROR, "MasterCardAPI.TRANSACTION_Verify(): Null response");
                return null;
            }
        }

        public TransResponse TRANSACTION_Verify(string sessionId, string token, string expiryYear,
                                string expiryMonth, decimal amount, string orderId,
                                string transId, string currency,
                                string secure3dId,
                                Customer customer, Billing billing,
                                Device device, Order order,
                                Shipping shipping, ref ErrorResponse errorResponse)
        {
            return TRANSACTION_Authorize_Pay("VERIFY", sessionId, token, expiryYear, expiryMonth, amount, orderId,
                                transId, currency, null, null, null, null, null, null, ref errorResponse);
        }

        public TransResponse TRANSACTION_Authorize(string sessionId, string token, string expiryYear,
                                    string expiryMonth, decimal amount, string orderId,
                                    string transId, string currency,
                                    string secure3dId,
                                    Customer customer, Billing billing,
                                    Device device, Order order,
                                    Shipping shipping, ref ErrorResponse errorResponse)
        {
          //  Debug.Log(Debug.DEBUG_INFO, "MasterCardAPI.TRANSACTION_Authorize()");
            return TRANSACTION_Authorize_Pay("AUTHORIZE", sessionId, token, expiryYear, expiryMonth, amount, orderId,
                                        transId, currency, secure3dId, customer, billing, device, order, shipping, ref errorResponse);
        }

        public TransResponse TRANSACTION_Pay(string sessionId, string token, string expiryYear,
                                    string expiryMonth, decimal amount, string orderId,
                                    string transId, string currency,
                                    string secure3dId,
                                    Customer customer, Billing billing,
                                    Device device, Order order,
                                    Shipping shipping, ref ErrorResponse errorResponse)
        {
            //Debug.Log(Debug.DEBUG_INFO, "MasterCardAPI.TRANSACTION_Pay()");
            return TRANSACTION_Authorize_Pay("PAY", sessionId, token, expiryYear, expiryMonth, amount, orderId,
                                        transId, currency, secure3dId, customer, billing, device, order, shipping, ref errorResponse);
        }

        public TransResponse TRANSACTION_Authorize(string sessionId, string token, string expiryYear,
                                    string expiryMonth, decimal amount, string orderId,
                                    string transId, string currency,
                                    string secure3dId, TransRequest transRequest, ref ErrorResponse errorResponse)
        {
          //  Debug.Log(Debug.DEBUG_INFO, "MasterCardAPI.TRANSACTION_Authorize()");
            return TRANSACTION_Authorize_Pay("AUTHORIZE", sessionId, token, expiryYear, expiryMonth, amount, orderId, transId, currency, secure3dId,
                                    transRequest.customer, transRequest.billing, transRequest.device, transRequest.order, transRequest.shipping, ref errorResponse);
        }


        private TransResponse TRANSACTION_Authorize_Pay(string apiOperation, string sessionId, string token, string expiryYear,
                                    string expiryMonth, decimal amount, string orderId,
                                    string transId, string currency,
                                    string secure3dId,
                                    Customer customer, Billing billing,
                                    Device device, Order order,
                                    Shipping shipping, ref ErrorResponse errorResponse)
        {
           // Debug.Log(Debug.DEBUG_INFO, "MasterCardAPI.TRANSACTION_Authorize_Pay()");

            string wsUri = _baseUrl + "/api/rest/version/" + _apiver + "/merchant/" + _merchantId + "/order/" + orderId + "/transaction/" + transId;

            TransRequest auth = new TransRequest();
            auth.apiOperation = apiOperation;

            if (secure3dId != null)
            {
                auth.secure3dId = secure3dId;
            }

            if (customer != null)
            {
                auth.customer = customer;
            }

            Session session = new Session();

            if (sessionId != null)
            {
                session.id = sessionId;
            }
            auth.session = session;

            Expiry expiry = new Expiry();
            expiry.month = expiryMonth;
            expiry.year = expiryYear;

            Card card = new Card();
            card.expiry = expiry;

            Provided provided = new Provided();
            provided.card = card;

            SourceOfFunds sourceOfFunds = new SourceOfFunds();
            sourceOfFunds.type = "CARD";
            sourceOfFunds.provided = provided;

            if (token != null)
            {
                sourceOfFunds.token = token;
            }

            auth.sourceOfFunds = sourceOfFunds;

            if (customer != null)
            {
                auth.customer = customer;
            }

            if (billing != null)
            {
                auth.billing = billing;
            }

            if (device != null)
            {
                auth.device = device;
            }

            if (shipping != null)
            {
                auth.shipping = shipping;
            }

            if (order == null)
            {
                Order _order = new Order();
                _order.amount = Math.Round(amount, 2);
                _order.currency = currency;
                auth.order = _order;
            }
            else
            {
                auth.order = order;
            }

            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new LowercaseContractResolver();
            settings.NullValueHandling = NullValueHandling.Ignore;

            // serialise json with lowercase property names and indented
            string authJson = null;

            try
            {
                authJson = JsonConvert.SerializeObject(auth, Formatting.Indented, settings);
            }
            catch (Exception ex)
            {
                // Debug.Log(Debug.DEBUG_ERROR, "TRANSACTION_Authorize_Pay() SerializeObject Exception: " + ex.Message);
                throw ex;
            }

            string response = null;

            try
            {
                response = CallWS(wsUri, authJson, "PUT");
            }
            catch (Exception e)
            {
                // Debug.Log(Debug.DEBUG_ERROR, "TRANSACTION_Authorize_Pay() CallWS Exception:" + e.Message);
                throw e;
            }

            // see if we have an ErrorResponse object instead of an authResponse object
            GetErrorResponse(response, ref errorResponse);

            if (errorResponse.error == null)
            {
                if (response != null)
                {
                    return JsonConvert.DeserializeObject<TransResponse>(response);
                }
                else
                {
                   // Debug.Log(Debug.DEBUG_ERROR, "TRANSACTION_Authorize_Pay() Null Response from DeserializeObject()");
                    return null;
                }
            }
            else
            {
               // Debug.Log(Debug.DEBUG_INFO, "TRANSACTION_Authorize_Pay() returning Null as errorResponse is valid");
                return null;
            }
        }

        private void GetErrorResponse(string response, ref ErrorResponse errorResponse)
        {
            try
            {
                // make sure the object is an error object
                JObject o = JObject.Parse(response);
                string cause = o.SelectToken("error.cause").ToString();

                // if we get this far then serialise the entire error object
                errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(response);
            }
            catch (Exception)
            {
              //  Debug.Log(Debug.DEBUG_INFO, "MasterCardAPI.GetErrorResponse(): no error object found");
            }
        }

        public TransResponse TRANSACTION_Retrieve(string transId, string orderId, ref ErrorResponse errorResponse)
        {
            //Debug.Log(Debug.DEBUG_INFO, "MasterCardAPI.TRANSACTION_Retrieve()");

            string wsUri = _baseUrl + "/api/rest/version/" + _apiver + "/merchant/" + _merchantId + "/order/" + orderId + "/transaction/" + transId;

            string response = null;

            try
            {
                response = CallWS(wsUri, null, "GET");
            }
            catch (Exception e)
            {
                //  Debug.Log(Debug.DEBUG_ERROR, "TRANSACTION_Retrieve(): Exception in CallWS()" + e.Message);
                throw e;
            }

            GetErrorResponse(response, ref errorResponse);

            if (response != null)
            {
                TransResponse authResponse = null;

                try
                {
                    authResponse = JsonConvert.DeserializeObject<TransResponse>(response);
                }
                catch (Exception authEx)
                {
                    //   Debug.Log(Debug.DEBUG_ERROR, "TRANSACTION_Retrieve(): Exception deserialising json response" + authEx.Message);
                    throw authEx;
                }

                return authResponse;
            }
            else
            {
                return null;
            }
        }

        public RefundResponse TRANSACTION_Refund(string orderId, string transId, decimal amount, string currency, ref ErrorResponse errorResponse)
        {
          //  Debug.Log(Debug.DEBUG_INFO, "MasterCardAPI.TRANSACTION_Refund()");

            string wsUri = _baseUrl + "/api/rest/version/" + _apiver + "/merchant/" + _merchantId + "/order/" + orderId + "/transaction/" + transId;

            RefundRequest refundRequest = new RefundRequest();
            refundRequest.apiOperation = "REFUND";

            Transaction transaction = new Transaction();
            transaction.amount = amount;
            transaction.currency = currency;
            refundRequest.transaction = transaction;

            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new LowercaseContractResolver();
            settings.NullValueHandling = NullValueHandling.Ignore;

            string refundJson = null;

            try
            {
                refundJson = JsonConvert.SerializeObject(refundRequest, Formatting.Indented, settings);
            }
            catch (Exception ex)
            {
                //  Debug.Log(Debug.DEBUG_ERROR, "TRANSACTION_Refund() SerializeObject Exception:" + ex.Message);
                throw ex;
            }

            string response = null;

            try
            {
                response = CallWS(wsUri, refundJson, "PUT");
            }
            catch (Exception e)
            {
                //  Debug.Log(Debug.DEBUG_ERROR, "TRANSACTION_Refund() CallWS Exception: " + e.Message);
                throw e;
            }

            GetErrorResponse(response, ref errorResponse);

            if (response != null)
            {
                return JsonConvert.DeserializeObject<RefundResponse>(response);
            }
            else
            {
             //   Debug.Log(Debug.DEBUG_ERROR, "TRANSACTION_Refund() Null Response from CalLWS()");
                return null;
            }
        }

        public string CallWS(string url, string jsonMessageBody, string verb)
        {
           // Debug.Log(Debug.DEBUG_INFO, "MasterCardAPI.CallWS() " + verb);

            if (jsonMessageBody != null)
            {
              //  Debug.Log(Debug.DEBUG_INFO, "SENDING: " + jsonMessageBody);
            }
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = verb;
            request.ContentType = "application/json";

            string credentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(_username + ":" + _password));
            request.Headers.Add("Authorization", "Basic " + credentials);

            // only need to send body data on PUT or POST
            if (verb == "PUT" || verb == "POST")
            {
                // write the body data
                try
                {
                    using (Stream reqStream = request.GetRequestStream())
                    {
                        reqStream.Write(System.Text.Encoding.UTF8.GetBytes(jsonMessageBody), 0,
                            System.Text.Encoding.UTF8.GetByteCount(jsonMessageBody));
                    }
                }
                catch (Exception e)
                {
                    //  Debug.Log(Debug.DEBUG_ERROR, "CallWS() Exception: " + e.Message + e.StackTrace);
                    throw e;
                }
            }

            HttpWebResponse response = null;

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                // the gateway will sometimes return an error result as a http 400 caught here
                if (ex.Response != null)
                {
                    response = (HttpWebResponse)ex.Response;
                }
                else
                {
                    return null;
                }
            }

            Stream respStream = response.GetResponseStream();

            StreamReader readStream = new StreamReader(respStream, System.Text.Encoding.UTF8);
            string result = readStream.ReadToEnd();
            response.Close();

            if (result != null)
              //  Debug.Log(Debug.DEBUG_INFO, "RETURN:" + JToken.Parse(result).ToString(Newtonsoft.Json.Formatting.Indented));

            if (result == null)
            {
              //  Debug.Log(Debug.DEBUG_ERROR, "MasterCardAPI.CallWS(): Null result");
            }

            return result;
        }

        public class LowercaseContractResolver : DefaultContractResolver
        {
            protected override string ResolvePropertyName(string str)
            {
                if (String.IsNullOrEmpty(str) || Char.IsLower(str, 0))
                    return str;

                return Char.ToLowerInvariant(str[0]) + str.Substring(1);
            }
        }
    }
}
