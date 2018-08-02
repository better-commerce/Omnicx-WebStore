using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Omnicx.API.SDK.Payments.Entities;
using Omnicx.WebStore.Models.Commerce;
using Omnicx.WebStore.Models.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Omnicx.API.SDK.Payments.Klarna
{
    public class KlarnaApi
    {
        private readonly string _baseUrl = string.Empty;
        private readonly string _username = string.Empty;
        private readonly string _password = string.Empty;

        public KlarnaApi(string baseUrl, string username, string password)
        {
            _baseUrl = baseUrl;
            _username = username;
            _password = password;
        }

        public KlarnaApi(PaymentMethodModel setting)
        {
            _baseUrl = setting.UseSandbox == "True" ? setting.TestUrl : setting.ProductionUrl;
            _username = setting.UserName;
            _password = setting.Password;
        }

        public TokenResponse CreateSession(ProcessPaymentRequest processPaymentRequest)
        {
            //Create a new credit session
            string sessionUri = _baseUrl + "/payments/v1/sessions";

            TokenRequest tokenRequest = new TokenRequest
            {
                Purchase_country = processPaymentRequest.Order.BillingAddress.CountryCode,
                Purchase_currency = processPaymentRequest.CurrencyCode,
                Locale = processPaymentRequest.LanuguageCode,
                Order_amount = Convert.ToInt32(processPaymentRequest.OrderTotal * 100),
                Order_tax_amount = 0,
                Order_lines = processPaymentRequest.Order.Items.Select(p => new OrderLines
                {
                    Name = p.Name,
                    Reference = p.StockCode,
                    Quantity = p.Qty,
                    Unit_price = Convert.ToInt32(p.Price.Raw.WithTax * 100),
                    Total_amount = Convert.ToInt32(p.TotalPrice.Raw.WithTax * 100),
                    Type = LineType.physical.ToString(),
                    Quantity_unit = "pcs",
                    Tax_rate = (int)p.Price.Raw.Tax,
                    Total_discount_amount = 0,
                    Total_tax_amount = (Convert.ToInt32(p.TotalPrice.Raw.WithTax * 100) / (Convert.ToInt32(p.TotalPrice.Raw.WithTax * 100) + (int)p.Price.Raw.Tax))
                }).ToList()
            };

            var shippingLine = new OrderLines
            {
                Name = "Shipping",
                Reference = "Shipping",
                Quantity = 1,
                Unit_price = Convert.ToInt32(processPaymentRequest.Order.ShippingCharge.Raw.WithTax * 100),
                Total_amount = Convert.ToInt32(processPaymentRequest.Order.ShippingCharge.Raw.WithTax * 100),
                Type = LineType.shipping_fee.ToString(),
                Tax_rate = 0,
                Total_tax_amount = 0
            };

            tokenRequest.Order_lines.Add(shippingLine);

            var discountLine = new OrderLines
            {
                Name = "Discount",
                Reference = "Discount",
                Quantity = 1,
                Unit_price = -Convert.ToInt32(processPaymentRequest.Order.Discount.Raw.WithTax * 100),
                Total_amount = -Convert.ToInt32(processPaymentRequest.Order.Discount.Raw.WithTax * 100),
                Type = LineType.discount.ToString(),
                Tax_rate = 0,
                Total_tax_amount = 0
            };

            tokenRequest.Order_lines.Add(discountLine);

            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new LowercaseContractResolver();
            settings.NullValueHandling = NullValueHandling.Ignore;

            // serialise json with lowercase property names and indented
            string tokenJson = JsonConvert.SerializeObject(tokenRequest, Formatting.Indented, settings);

            string response = null;
            var tokenResp = new TokenResponse();

            try
            {
                response = CallWS(sessionUri, tokenJson, "POST");
                // Parse JSON into dynamic object, convenient!
                JObject results = JObject.Parse(response);

                tokenResp.SessionId = Convert.ToString(results[ResponseVariable.SessionId]);
                tokenResp.ClientToken = Convert.ToString(results[ResponseVariable.ClientToken]);
            }
            catch (Exception e)
            {
                throw e;
            }
            return tokenResp;
        }

        public void ReadExistingSession(string sessionId)
        {
            //Read existing credit session
            string sessionUri = _baseUrl + "/payments/v1/sessions/" + sessionId;
            string response = null;

            try
            {
                response = CallWS(sessionUri, "", "GET");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public TokenResponse UpdateSession(ProcessPaymentRequest processPaymentRequest, string sessionId, string clientToken)
        {
            //Update credit session
            string sessionUri = _baseUrl + "/payments/v1/sessions/" + sessionId;

            TokenRequest tokenRequest = new TokenRequest
            {
                Purchase_country = processPaymentRequest.Order.BillingAddress.CountryCode,
                Purchase_currency = processPaymentRequest.CurrencyCode,
                Locale = processPaymentRequest.LanuguageCode,
                Order_amount = Convert.ToInt32(processPaymentRequest.OrderTotal * 100),
                Order_tax_amount = 0,
                Order_lines = processPaymentRequest.Order.Items.Select(p => new OrderLines
                {
                    Name = p.Name,
                    Reference = p.StockCode,
                    Quantity = p.Qty,
                    Unit_price = Convert.ToInt32(p.Price.Raw.WithTax * 100),
                    Total_amount = Convert.ToInt32(p.TotalPrice.Raw.WithTax * 100),
                    Type = LineType.physical.ToString(),
                    Quantity_unit = "pcs",
                    Tax_rate = (int)p.Price.Raw.Tax,
                    Total_discount_amount = 0,
                    Total_tax_amount = (Convert.ToInt32(p.TotalPrice.Raw.WithTax * 100) / (Convert.ToInt32(p.TotalPrice.Raw.WithTax * 100) + (int)p.Price.Raw.Tax))
                }).ToList()
            };

              var shippingLine = new OrderLines
            {
                Name = "Shipping",
                Reference = "Shipping",
                Quantity = 1,
                Unit_price = Convert.ToInt32(processPaymentRequest.Order.ShippingCharge.Raw.WithTax * 100),
                Total_amount = Convert.ToInt32(processPaymentRequest.Order.ShippingCharge.Raw.WithTax * 100),
                Type = LineType.shipping_fee.ToString(),
                Tax_rate = 0,
                Total_tax_amount = 0
            };

            tokenRequest.Order_lines.Add(shippingLine);

            var discountLine = new OrderLines
            {
                Name = "Discount",
                Reference = "Discount",
                Quantity = 1,
                Unit_price = -Convert.ToInt32(processPaymentRequest.Order.Discount.Raw.WithTax * 100),
                Total_amount = -Convert.ToInt32(processPaymentRequest.Order.Discount.Raw.WithTax * 100),
                Type = LineType.discount.ToString(),
                Tax_rate = 0,
                Total_tax_amount = 0
            };

            tokenRequest.Order_lines.Add(discountLine);

            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new LowercaseContractResolver();
            settings.NullValueHandling = NullValueHandling.Ignore;

            // serialise json with lowercase property names and indented
            string tokenJson = JsonConvert.SerializeObject(tokenRequest, Formatting.Indented, settings);

            string response = null;
            var tokenResp = new TokenResponse();

            try
            {
                response = CallWS(sessionUri, tokenJson, "POST");
            }
            catch (Exception e)
            {
                throw e;
            }

            tokenResp.SessionId = sessionId;
            tokenResp.ClientToken = clientToken;

            return tokenResp;
        }

        public void CancelExistingAuthorization(string authorizationToken)
        {
            //Cancel an existing authorization
            string authorizationUri = _baseUrl + "/payments/v1/authorizations/" + authorizationToken;
            string response = null;

            try
            {
                response = CallWS(authorizationUri, "", "DELETE");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public TokenResponse GenerateConsumerToken(CheckoutModel processPaymentRequest, string authorizationToken)
        {
            //Generate a consumer token
            string tokenUri = _baseUrl + "/payments/v1/authorizations/" + authorizationToken + "/customer-token";

            TokenRequest tokenRequest = new TokenRequest
            {
                Purchase_country = processPaymentRequest.BillingAddress.CountryCode,
                Purchase_currency = processPaymentRequest.CurrencyCode,
                Locale = processPaymentRequest.LanuguageCode,
                Description = "Create Order",
                Intended_use = "SUBSCRIPTION",
            };

            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new LowercaseContractResolver();
            settings.NullValueHandling = NullValueHandling.Ignore;

            // serialise json with lowercase property names and indented
            string tokenJson = JsonConvert.SerializeObject(tokenRequest, Formatting.Indented, settings);

            string response = null;
            var tokenResp = new TokenResponse();

            try
            {
                response = CallWS(tokenUri, tokenJson, "POST");
                // Parse JSON into dynamic object, convenient!
                JObject results = JObject.Parse(response);

                tokenResp.TokenId = Convert.ToString(results[ResponseVariable.TokenId]);
                tokenResp.RedirectUrl = Convert.ToString(results[ResponseVariable.RedirectUrl]);
            }
            catch (Exception e)
            {
                throw e;
            }
            return tokenResp;
        }

        public OrderResponse CreateOrder(CheckoutModel processPaymentRequest, string authorizationToken, string orderId, string paymentId)
        {
            //Create a new credit session
            string createOrderUri = _baseUrl + "/payments/v1/authorizations/" + authorizationToken + "/order";
            
            TokenRequest tokenRequest = new TokenRequest
            {
                Purchase_country = processPaymentRequest.BillingAddress.CountryCode,
                Purchase_currency = processPaymentRequest.CurrencyCode,
                Locale = processPaymentRequest.LanuguageCode,
                Billing_address = new Address
                {
                    Given_name = processPaymentRequest.BillingAddress.FirstName,
                    Family_name = processPaymentRequest.BillingAddress.LastName,
                    Email = processPaymentRequest.Email,
                    Title = processPaymentRequest.BillingAddress.Title,
                    Street_address = processPaymentRequest.BillingAddress.Address1,
                    Street_address2 = processPaymentRequest.BillingAddress.Address2,
                    Postal_code = processPaymentRequest.BillingAddress.PostCode,
                    City = processPaymentRequest.BillingAddress.City,
                    Region = processPaymentRequest.BillingAddress.State,
                    Phone = processPaymentRequest.BillingAddress.PhoneNo,
                    Country = processPaymentRequest.BillingAddress.CountryCode
                },
                Shipping_address = new Address
                {
                    Given_name = processPaymentRequest.ShippingAddress.FirstName,
                    Family_name = processPaymentRequest.ShippingAddress.LastName,
                    Email = processPaymentRequest.Email,
                    Title = processPaymentRequest.ShippingAddress.Title,
                    Street_address = processPaymentRequest.ShippingAddress.Address1,
                    Street_address2 = processPaymentRequest.ShippingAddress.Address2,
                    Postal_code = processPaymentRequest.ShippingAddress.PostCode,
                    City = processPaymentRequest.ShippingAddress.City,
                    Region = processPaymentRequest.ShippingAddress.State,
                    Phone = processPaymentRequest.ShippingAddress.PhoneNo,
                    Country = processPaymentRequest.ShippingAddress.CountryCode
                },
                Order_amount = Convert.ToInt32(processPaymentRequest.BalanceAmount.Raw.WithTax * 100),
                Order_tax_amount = 0,
                Order_lines = processPaymentRequest.Basket.LineItems.Select(p => new OrderLines
                {
                    Name = p.Name,
                    Reference = p.StockCode,
                    Quantity = p.Qty,
                    Unit_price = Convert.ToInt32(p.Price.Raw.WithTax * 100),
                    Total_amount = Convert.ToInt32(p.Price.Raw.WithTax * 100),
                    Type = LineType.physical.ToString(),
                    Quantity_unit = "pcs",
                    Tax_rate = (int)p.Price.Raw.Tax,
                    Total_discount_amount = 0,
                    Total_tax_amount = (Convert.ToInt32(p.Price.Raw.WithTax * 100) / (Convert.ToInt32(p.Price.Raw.WithTax * 100) + (int)p.Price.Raw.Tax))
                }).ToList(),
                Merchant_reference1 = orderId
            };
            var shippingLine = new OrderLines
            {
                Name = "Shipping",
                Reference = "Shipping",
                Quantity = 1,
                Unit_price = Convert.ToInt32(processPaymentRequest.Basket.ShippingCharge.Raw.WithTax * 100),
                Total_amount = Convert.ToInt32(processPaymentRequest.Basket.ShippingCharge.Raw.WithTax * 100),
                Type = LineType.shipping_fee.ToString(),
                Tax_rate = 0,
                Total_tax_amount = 0
            };
            tokenRequest.Order_lines.Add(shippingLine);
            var discountLine = new OrderLines
            {
                Name = "Discount",
                Reference = "Discount",
                Quantity = 1,
                Unit_price = -Convert.ToInt32(processPaymentRequest.Basket.Discount.Raw.WithTax * 100),
                Total_amount = -Convert.ToInt32(processPaymentRequest.Basket.Discount.Raw.WithTax * 100),
                Type = LineType.discount.ToString(),
                Tax_rate = 0,
                Total_tax_amount = 0
            };
            tokenRequest.Order_lines.Add(discountLine);
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new LowercaseContractResolver();
            settings.NullValueHandling = NullValueHandling.Ignore;

            // serialise json with lowercase property names and indented
            string tokenJson = JsonConvert.SerializeObject(tokenRequest, Formatting.Indented, settings);

            string response = null;
            var orderResp = new OrderResponse();

            try
            {
                response = CallWS(createOrderUri, tokenJson, "POST");
                // Parse JSON into dynamic object, convenient!
                JObject results = JObject.Parse(response);

                orderResp.OrderId = Convert.ToString(results[ResponseVariable.OrderId]);
                orderResp.RedirectUrl = Convert.ToString(results[ResponseVariable.RedirectUrl]);
                orderResp.FraudStatus = Convert.ToString(results[ResponseVariable.FraudStatus]);
                orderResp.PaymentId = paymentId;
                orderResp.OrderAmount = Convert.ToInt32(processPaymentRequest.BalanceAmount.Raw.WithTax * 100);
                orderResp.RefOrderId = orderId;
            }
            catch (Exception e)
            {
                throw e;
            }
            return orderResp;
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
                    return null;
                }
            }

            HttpWebResponse response = null;

            try
            {
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
