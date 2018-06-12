using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Omnicx.API.SDK.Payments.MasterCard
{
    public class Error
    {
        public string cause { get; set; }
        public string explanation { get; set; }
        public string field { get; set; }
        public string validationType { get; set; }
        public string supportCode { get; set; }
    }

    public class ErrorResponse
    {
        public Error error { get; set; }
        public string result { get; set; }
    }

    public class Expiry
    {
        public string month { get; set; }
        public string year { get; set; }
    }

    public enum MasterApiPaymentStatus
    {
        PAYMENT_SUCCESS = 0,
        PAYMENT_FAILURE = 1
    }

    public class PaymentResult
    {
        public MasterApiPaymentStatus result { get; set; }
        public char secure3dResult { get; set; }
        public string sessiondId { get; set; }
        public string errorString { get; set; }
        public string AVSResult { get; set; }
        public string AVSResultCode { get; set; }
    }

    public class Card
    {
        public string brand { get; set; }
        public Expiry expiry { get; set; }
        public string fundingMethod { get; set; }
        public string issuer { get; set; }
        public string localBrand { get; set; }
        public string number { get; set; }
        public string scheme { get; set; }
        public string securityCode { get; set; }
    }

    public class CardToken
    {
        public string brand { get; set; }
        public string expiry { get; set; }
        public string fundingMethod { get; set; }
        public string issuer { get; set; }
        public string localBrand { get; set; }
        public string number { get; set; }
        public string scheme { get; set; }
        public string securityCode { get; set; }
    }


    public class Ach
    {
        public string accountIdentifier { get; set; }
        public string accountType { get; set; }
        public string bankAccountHolder { get; set; }
        public string bankAccountNumber { get; set; }
        public string routingNumber { get; set; }
        public string secCode { get; set; }
    }

    public class GiftCard
    {
        public string brand { get; set; }
        public string localBrand { get; set; }
        public string number { get; set; }
        public string pin { get; set; }
        public string scheme { get; set; }
    }

    public class Provided
    {
        public Card card { get; set; }
        public Ach ach { get; set; }
        public GiftCard giftCard { get; set; }
    }

    public class SourceOfFunds
    {
        public string type { get; set; }
        public Provided provided { get; set; }
        public string token { get; set; }
        public string issuer { get; set; }
        public string number { get; set; }
        public string fundingMethod { get; set; }
        public string scheme { get; set; }
    }

    public class Session
    {
        public string id { get; set; }
        public string version { get; set; }
        public string updateStatus { get; set; }
    }

    public class PaymentPlan
    {
        public int numberOfDeferrals { get; set; }
        public int numberOfPayments { get; set; }
        public string supported { get; set; }
    }

    public class Constraints
    {
        public PaymentPlan paymentPlan { get; set; }
        public string correlationId { get; set; }
    }

    public class PosTerminal
    {
        public string attended { get; set; }
        public string cardHolderActivated { get; set; }
        public int entryMode { get; set; }
        public string inputCapability { get; set; }
        public string location { get; set; }
        public string name { get; set; }

    }

    public class ResponseControls
    {
        public string sensitiveData { get; set; }
    }

    public class Risk
    {
        public string bypassMerchantRiskRules { get; set; }
        public string custom { get; set; }
        public RiskResponse response { get; set; }
    }

    public class RiskResponse
    {
        public string gatewayCode { get; set; }
        public int totalScore { get; set; }
    }


    public class Shipping
    {
        public Address address { get; set; }
    }


    class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd";
        }
    }

    public class SomePropertyDecimalFormatConverter : DecimalFormatJsonConverter
    {
        public SomePropertyDecimalFormatConverter()
            : base(2)
        {
        }
    }

    public class DecimalFormatJsonConverter : JsonConverter
    {
        private readonly int _numberOfDecimals;

        public DecimalFormatJsonConverter(int numberOfDecimals)
        {
            _numberOfDecimals = numberOfDecimals;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var d = (decimal)value;
            decimal rounded = 0.00M;
            rounded = rounded + Math.Round(d, _numberOfDecimals);
            writer.WriteValue(string.Format("{0:0.00}", rounded));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(decimal);
        }
    }

    public class Order
    {
        //public bool acceptPartialAmount { get; set; }
        [JsonConverter(typeof(SomePropertyDecimalFormatConverter))]
        public decimal amount { get; set; }
        public string currency { get; set; }
        public string custom { get; set; }
        public string customerNote { get; set; }
        //[JsonConverter(typeof(CustomDateTimeConverter))]
        //public DateTime customerOrderDate { get; set; }
        public string customerReference { get; set; }
        public string description { get; set; }
        public Discount discount { get; set; }
        public Item[] item { get; set; }
        //public decimal itemAmount { get; set; }
        public string notificationUrl { get; set; }
        public string productSKU { get; set; }
        public string recurringPaymentAgreement { get; set; }
        public string reference { get; set; }
        public string requestorName { get; set; }
        //public decimal shippingAndHandlingAmount { get; set; }
        public SubMerchant subMerchant { get; set; }
        public Tax tax { get; set; }
        //public decimal taxAmount { get; set; }
        public string taxRegistrationId { get; set; }
        public string walletIndicator { get; set; }
        public string walletProvider { get; set; }
        public string status { get; set; }
        public string id { get; set; }
        public string creationTime { get; set; }
    }

    public class AuthenticationRedirect
    {
        public string responseUrl;
        public Simple simple { get; set; }
    }

    public class Secure3D
    {
        public AuthenticationRedirect authenticationRedirect { get; set; }
        public string summaryStatus { get; set; }
        public string xid { get; set; }
    }

    public class Simple
    {
        public string htmlBodyContents { get; set; }
    }

    public class Tax
    {
        public decimal amount { get; set; }
        public string type { get; set; }
    }

    public class SubMerchant
    {
        public Address addres { get; set; }
        public int bankIndustryCode { get; set; }
        public string email { get; set; }
        public string identifier { get; set; }
        public string phone { get; set; }
        public string registeredName { get; set; }
        public string tradingName { get; set; }
    }

    public class Discount
    {
        public decimal amount { get; set; }
        public string code { get; set; }
        public string description { get; set; }

    }

    public class OrderResponse
    {
        public decimal amount { get; set; }
        public string creationTime { get; set; }
        public string currency { get; set; }
        public string id { get; set; }
        public string status { get; set; }
        public double totalAuthorizedAmount { get; set; }
        public double totalCapturedAmount { get; set; }
        public double totalRefundedAmount { get; set; }
    }

    public class Item
    {
        public string brand { get; set; }
        public string category { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public string sku { get; set; }
        public decimal unitPrice { get; set; }
        public decimal unitTaxAmount { get; set; }
    }

    public class CardSecurityCode
    {
        public string acquirerCode { get; set; }
        public string gatewayCode { get; set; }
    }

    public class Response
    {
        public string acquirerCode { get; set; }
        public string acquirerMessage { get; set; }
        public CardSecurityCode cardSecurityCode { get; set; }
        public string gatewayCode { get; set; }
        public CardholderVerification cardholderVerification { get; set; }
    }

    public class CardholderVerification
    {
        public Avs avs { get; set; }
        public DetailedVerification[] detailedVerification { get; set; }
    }

    public class DetailedVerification
    {
        public string gatewayCode { get; set; }
        public string type { get; set; }
    }

    public class Avs
    {
        public string acquirerCode { get; set; }
        public string gatewayCode { get; set; }
    }

    public class Acquirer
    {
        public int batch { get; set; }
        public string id { get; set; }
        public string merchantId { get; set; }
    }

    public class Transaction
    {
        public Acquirer acquirer { get; set; }
        public decimal amount { get; set; }
        public string currency { get; set; }
        public string frequency { get; set; }
        public string id { get; set; }
        public string receipt { get; set; }
        public string source { get; set; }
        public string terminal { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public string authorizationCode { get; set; }
    }

    public class Billing
    {
        public Address address { get; set; }
    }

    public class Address
    {
        public string city { get; set; }
        public string company { get; set; }
        public string country { get; set; }
        public string postcodeZip { get; set; }
        public string stateProvince { get; set; }
        public string street { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string street2 { get; set; }
    }

    public class CurrencyConversion
    {
        public DateTime exchangeRateTime { get; set; }
        public decimal marginPercentage { get; set; }
        public decimal payerAmount { get; set; }
        public string payerCurrency { get; set; }
        public decimal payerExchangeRate { get; set; }
        public string provider { get; set; }
        public string providerReceipt { get; set; }
        public string requestId { get; set; }
        public string uptake { get; set; }
    }

    public class Customer
    {
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string mobilePhone { get; set; }
        public string phone { get; set; }
    }

    public class Device
    {
        public string ani { get; set; }
        public string aniCallType { get; set; }
        public string browser { get; set; }
        public string fingerprint { get; set; }
        public string hostname { get; set; }
        public string ipAddress { get; set; }
    }

    public class Usage
    {
        public DateTime lastUpdated { get; set; }
        public string lastUpdatedBy { get; set; }
        public DateTime lastUsed { get; set; }
    }

    public class Secure3DAuth
    {
        public int acsEci { get; set; }
        public string authenticationStatus { get; set; }
        public string authenticationToken { get; set; }
        public string enrollmentStatus { get; set; }
        public string xid { get; set; }
    }

    // -- Request and response Object below

    public class TokenRequest
    {
        public Session session { get; set; }
        public SourceOfFunds sourceOfFunds { get; set; }
    }

    public class TokenResponse
    {
        public string repositoryId { get; set; }
        public string result { get; set; }
        public string status { get; set; }
        public string token { get; set; }
        public string gatewayCode { get; set; }
        public string cardBrand { get; set; }
        public string cardExpiry { get; set; }
        public string cardFundingMethod { get; set; }
        public string cardIssuer { get; set; }
        public string cardNumber { get; set; }
        public string cardScheme { get; set; }
        public string cardType { get; set; }
        public string usageLastUpdated { get; set; }
        public string usageLastUpdatedBy { get; set; }
        public string usageLastUsed { get; set; }
        public string verificationStrategry { get; set; }
    }

    public class CaptureRequest
    {
        public string apiOperation { get; set; }
        public Transaction transaction { get; set; }
    }

    public class CaptureResponse
    {
        public OrderResponse order { get; set; }
        public Response response { get; set; }
        public string result { get; set; }
        public SourceOfFunds sourceOfFunds { get; set; }
        public Transaction transaction { get; set; }

    }

    public class TransRequest
    {
        [JsonProperty(PropertyName = "3DSecure")]
        public Secure3DAuth secure3d { get; set; }
        [JsonProperty(PropertyName = "3DSecureId")]
        public string secure3dId { get; set; }
        public string apiOperation { get; set; }
        public Billing billing { get; set; }
        public Constraints constraints { get; set; }
        public string correlationId { get; set; }
        public CurrencyConversion currencyConversion { get; set; }
        public Customer customer { get; set; }
        public Device device { get; set; }
        public Order order { get; set; }
        public string partnerSolutionId { get; set; }
        public PaymentPlan paymentPlan { get; set; }
        public PosTerminal posTerminal { get; set; }
        public ResponseControls responseControls { get; set; }
        public Risk risk { get; set; }
        public Session session { get; set; }
        public Shipping shipping { get; set; }
        public SourceOfFunds sourceOfFunds { get; set; }
        public Transaction transaction { get; set; }
        public string userId { get; set; }
    }

    public class TransResponse
    {
        [JsonProperty(PropertyName = "3DSecure")]
        public Secure3DAuth secure3d { get; set; }
        [JsonProperty(PropertyName = "3DSecureId")]
        public string secure3dId { get; set; }
        public Billing billing { get; set; }
        public CurrencyConversion currencyConversion { get; set; }
        public Customer customer { get; set; }
        public Device device { get; set; }
        public string merchant { get; set; }
        public string gatewayEntryPoint { get; set; }
        public OrderResponse order { get; set; }
        public Risk risk { get; set; }
        public Shipping shipping { get; set; }
        public Response response { get; set; }
        public string result { get; set; }
        public SourceOfFunds sourceOfFunds { get; set; }
        public string timeOfRecord { get; set; }
        public Transaction transaction { get; set; }
        public string userId { get; set; }
        public string version { get; set; }
    }

    public class Secure3DRequest
    {
        [JsonProperty(PropertyName = "3DSecure")]
        public Secure3D secure3d { get; set; }
        public Session session { get; set; }
        public string apiOperation { get; set; }
        public Order order { get; set; }
    }

    public class Secure3DResponse
    {
        public string htmlBodyContent { get; set; }
        public string summaryStatus { get; set; }
        public string id { get; set; }
        public string xid { get; set; }
        public string gatewayCode { get; set; }
    }

    public class RefundRequest
    {
        public string apiOperation { get; set; }
        public Transaction transaction { get; set; }
    }

    public class RefundResponse
    {
        public string merchant { get; set; }
        public OrderResponse order { get; set; }
        public Response response { get; set; }
        public string result { get; set; }
        public Transaction transaction { get; set; }
    }

    public class GatewayStatus
    {
        public string gatewayVersion { get; set; }
        public string status { get; set; }
    }

    public class PaymentOptionsRepsonse
    {
        public string correlationId { get; set; }
        public string merchant { get; set; }
        public PaymentTypes paymentTypes { get; set; }
        public string result { get; set; }
        public string transactionMode { get; set; }
    }

    public class PaymentTypes
    {
        public PTAch ach { get; set; }
        public PTBancanet bancanet { get; set; }
        public PTCard card { get; set; }
        public PTGiftCard giftCard { get; set; }
        public PTGiropay giropay { get; set; }
        public PTPaypal paypal { get; set; }
        public PTSofort sofort { get; set; }
    }

    public class PTAch
    {
        public PTCurrencies[] currencies { get; set; }
    }

    public class PTBancanet
    {
        public PTCurrencies[] currencies { get; set; }
        public PTTransactionSources[] transactionSources { get; set; }
    }

    public class PTCard
    {
        [JsonProperty(PropertyName = "3DSecureSchemes")]
        public PTSecure3DSchemes[] secure3dSchemes { get; set; }
        public PTCapabilities[] capabilities { get; set; }
        public PTCardTypes[] cardTypes { get; set; }
        public PTCurrencies[] currencies { get; set; }
        public PTCurrencyConversion currencyConversion { get; set; }
        public string enforceCardSecurityCodeEntry { get; set; }
        public PTPaymentPlans[] paymentPlans { get; set; }
        public PTTransactionSources[] transactionSources { get; set; }
        public PTWalletProvviders[] walletProviders { get; set; }
    }

    public class PTGiftCard
    {
        public PTCardTypes cardTypes { get; set; }
        public PTCurrencies[] currencies { get; set; }
        public PTTransactionSources[] transactionSources { get; set; }
    }

    public class PTGiropay
    {
        public PTCurrencies[] currencies { get; set; }
        public PTTransactionSources[] transactionSources { get; set; }
    }

    public class PTPaypal
    {
        public PTCurrencies[] currencies { get; set; }
        public PTTransactionSources[] transactionSources { get; set; }
    }

    public class PTSofort
    {
        public PTCurrencies[] currencies { get; set; }
        public PTTransactionSources[] transactionSources { get; set; }

    }

    public class PTCurrencies
    {
        public string currency { get; set; }
    }

    public class PTTransactionSources
    {
        public string transactionSource { get; set; }
    }

    public class PTSecure3DSchemes
    {
        [JsonProperty(PropertyName = "3DSecureScheme")]
        public string secure3DScheme { get; set; }
    }

    public class PTCapabilities
    {
        public string capability { get; set; }
    }

    public class PTCardTypes
    {
        public string cardType { get; set; }
    }

    public class PTCurrencyConversion
    {
        public string exchangeRateSource { get; set; }
        public DateTime exchangeRateTime { get; set; }
        public string gatewayCode { get; set; }
        public string marginPercentage { get; set; }
        public string offerText { get; set; }
        public decimal payerAmount { get; set; }
        public string payerCurrency { get; set; }
        public decimal payerExchangeRate { get; set; }
        public string provider { get; set; }
        public string providerCode { get; set; }
        public string providerReceipt { get; set; }
        public DateTime quoteExpiry { get; set; }
        public string requestId { get; set; }
    }

    public class PTPaymentPlans
    {
        public string endDate { get; set; }
        public PTMinimumOrderAmounts[] minimumOrderAmounts { get; set; }
        public string[] numberOfDeferrals { get; set; }
        public string[] numberOfPayments { get; set; }
        public bool payerInterest { get; set; }
        public string planId { get; set; }
        public string planName { get; set; }
        public string planTemplate { get; set; }
        public string startDate { get; set; }
    }

    public class PTMinimumOrderAmounts
    {
        public decimal amount { get; set; }
        public string currency { get; set; }
    }

    public class PTWalletProvviders
    {
        public string walletProvider { get; set; }
    }

    public class SessionResponse
    {
        public string merchant { get; set; }
        public Session session { get; set; }
        public SourceOfFunds sourceOfFunds { get; set; }
        public Wallet wallet { get; set; }
    }

    public class Wallet
    {
        public Masterpass masterpass { get; set; }
        public VisaCheckout visaCheckout { get; set; }
    }

    public class Masterpass
    {
        public string allowedCardTypes { get; set; }
        public string longAccessToken { get; set; }
        public string merchantCheckoutId { get; set; }
        public string originUrl { get; set; }
        public string payerAuthentication { get; set; }
        public string requestToken { get; set; }
        public string secondaryOriginUrl { get; set; }
    }

    public class VisaCheckout
    {
        public string cardArt { get; set; }
        public string cardBrand { get; set; }
    }
}
