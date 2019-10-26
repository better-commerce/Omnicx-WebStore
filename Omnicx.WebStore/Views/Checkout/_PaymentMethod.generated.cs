﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using DevTrends.MvcDonutCaching;
    
    #line 14 "..\..\Views\Checkout\_PaymentMethod.cshtml"
    using Omnicx.API.SDK.Payments.Entities;
    
    #line default
    #line hidden
    using Omnicx.WebStore;
    using Omnicx.WebStore.Core;
    
    #line 13 "..\..\Views\Checkout\_PaymentMethod.cshtml"
    using Omnicx.WebStore.Models.Common;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Checkout/_PaymentMethod.cshtml")]
    public partial class _Views_Checkout__PaymentMethod_cshtml : Omnicx.WebStore.Core.Services.Infrastructure.CustomBaseViewPage<CheckoutViewModel>
    {
        public _Views_Checkout__PaymentMethod_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 1 "..\..\Views\Checkout\_PaymentMethod.cshtml"
  
/*
    Name: Payment Method
    Purpose: Show ALl Available Payment Methods
    Structure: /Views/Checkout/_PaymentMethod.cshtml
    Contains (Partial Views Used):
        d-/Views/Checkout/_BillingAddress.cshtml        (Select Billing Address)

    Contained In (Where we Use this View):
        a-/Views/Basket/OnePageCheckout.cshtml
    */

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 16 "..\..\Views\Checkout\_PaymentMethod.cshtml"
  
    var masterCard = Model.Checkout.PaymentOptions.FirstOrDefault(x => x.SystemName == PaymentMethodTypes.MasterCard.ToString());
    if (masterCard != null)
    {

            
            #line default
            #line hidden
WriteLiteral("        <script");

WriteAttribute("src", Tuple.Create(" src=\"", 719), Tuple.Create("\"", 847)
, Tuple.Create(Tuple.Create("", 725), Tuple.Create("https://eu-gateway.mastercard.com/form/version/", 725), true)
            
            #line 20 "..\..\Views\Checkout\_PaymentMethod.cshtml"
, Tuple.Create(Tuple.Create("", 772), Tuple.Create<System.Object, System.Int32>(masterCard.Version
            
            #line default
            #line hidden
, 772), false)
, Tuple.Create(Tuple.Create("", 791), Tuple.Create("/merchant/", 791), true)
            
            #line 20 "..\..\Views\Checkout\_PaymentMethod.cshtml"
                  , Tuple.Create(Tuple.Create("", 801), Tuple.Create<System.Object, System.Int32>(masterCard.AccountCode
            
            #line default
            #line hidden
, 801), false)
, Tuple.Create(Tuple.Create("", 824), Tuple.Create("/session.js?debug=false", 824), true)
);

WriteLiteral("></script>\r\n");

            
            #line 21 "..\..\Views\Checkout\_PaymentMethod.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<div");

WriteLiteral(" class=\"content\"");

WriteLiteral(" >\r\n    <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"col-sm-3 col-xs-6\"");

WriteLiteral(" ng-repeat=\"paymentMethod in ck.model.checkout.paymentOptions\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"paymentBox\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"methodImage\"");

WriteLiteral(">\r\n                    <img");

WriteAttribute("ng-src", Tuple.Create(" ng-src=\"", 1127), Tuple.Create("\"", 1195)
, Tuple.Create(Tuple.Create("", 1136), Tuple.Create<System.Object, System.Int32>(Href("~/assets/theme/ocx/images/{{paymentMethod.displayName}}.png")
, 1136), false)
);

WriteLiteral(" class=\"img-responsive\"");

WriteLiteral(" alt=\"{{paymentMethod.displayName}}\"");

WriteLiteral(" />\r\n                </div>\r\n                <div");

WriteLiteral(" class=\"methodName\"");

WriteLiteral(">\r\n                    <label");

WriteLiteral(" class=\"control control--radio\"");

WriteLiteral(">\r\n                        <span");

WriteLiteral(" ng-bind=\"paymentMethod.displayName\"");

WriteLiteral("></span>                        \r\n                        <input");

WriteLiteral(" id=\"payment\"");

WriteLiteral(" name=\"payment\"");

WriteLiteral(" type=\"radio\"");

WriteLiteral(" value=\"{{paymentMethod.slug}}\"");

WriteLiteral(" class=\"ng-pristine ng-valid\"");

WriteLiteral(" ng-click=\"ck.errors=false;ck.continue=false;ck.payment(paymentMethod);\"");

WriteLiteral(">\r\n                        <div");

WriteLiteral(" class=\"control__indicator\"");

WriteLiteral("></div>\r\n                    </label>\r\n                </div>\r\n            </div>" +
"\r\n        </div>\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">               \r\n        <div");

WriteLiteral(" class=\"col-sm-12\"");

WriteLiteral(">\r\n            <div ng-cloak");

WriteLiteral(" class=\"alert alert-danger\"");

WriteLiteral(" ng-show=\"ck.errors\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 44 "..\..\Views\Checkout\_PaymentMethod.cshtml"
           Write(LT("Checkout.Text.SelectPayment", "Select a Payment Method"));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n        </div>\r\n");

            
            #line 47 "..\..\Views\Checkout\_PaymentMethod.cshtml"
        
            
            #line default
            #line hidden
            
            #line 47 "..\..\Views\Checkout\_PaymentMethod.cshtml"
          
            foreach (var payment in Model.Checkout.PaymentOptions)
            {

            
            #line default
            #line hidden
WriteLiteral("                <div");

WriteLiteral(" class=\"col-sm-12 col-xs-12 img-rounded\"");

WriteAttribute("ng-show", Tuple.Create(" ng-show=\"", 2273), Tuple.Create("\"", 2350)
, Tuple.Create(Tuple.Create("", 2283), Tuple.Create("ck.model.checkout.selectedPayment.systemName==\'", 2283), true)
            
            #line 50 "..\..\Views\Checkout\_PaymentMethod.cshtml"
                                     , Tuple.Create(Tuple.Create("", 2330), Tuple.Create<System.Object, System.Int32>(payment.SystemName
            
            #line default
            #line hidden
, 2330), false)
, Tuple.Create(Tuple.Create("", 2349), Tuple.Create("\'", 2349), true)
);

WriteLiteral(">\r\n");

            
            #line 51 "..\..\Views\Checkout\_PaymentMethod.cshtml"
                   
            
            #line default
            #line hidden
            
            #line 51 "..\..\Views\Checkout\_PaymentMethod.cshtml"
                    if (payment.SystemName == PaymentMethodTypes.MasterCard.ToString())
                   {

            
            #line default
            #line hidden
WriteLiteral("                       <div");

WriteLiteral(" class=\"col-sm-8 col-xs-12 no-padding\"");

WriteLiteral(">\r\n                           <form");

WriteLiteral(" name=\"paymetForm\"");

WriteLiteral(" class=\"payment-box\"");

WriteLiteral(">\r\n                               <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n                                   <div");

WriteLiteral(" class=\"col-sm-12 col-xs-12\"");

WriteLiteral(">\r\n                                       <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                                           <label");

WriteLiteral(" class=\"mastercard-label\"");

WriteLiteral(">");

            
            #line 58 "..\..\Views\Checkout\_PaymentMethod.cshtml"
                                                                      Write(LT("Checkout.Text.CardNumber", "Card Number"));

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n                                           <span");

WriteLiteral(" class=\"icon-credit-card icon-abso\"");

WriteLiteral("></span>\r\n                                           <input");

WriteLiteral(" type=\"number\"");

WriteLiteral(" id=\"card-number\"");

WriteLiteral(" class=\"form-control mastercard-textbox\"");

WriteLiteral(" onkeyup=\"removeClass(this)\"");

WriteLiteral(" placeholder=\"Card Number\"");

WriteLiteral(" value=\"\"");

WriteLiteral(" required />\r\n                                           <span");

WriteLiteral(" id=\"card-numberError\"");

WriteLiteral(" style=\"margin-top:-10px; display:none;\"");

WriteLiteral(" class=\"help-block for-validated-control has-error margin-bottom-sm\"");

WriteLiteral(">");

            
            #line 61 "..\..\Views\Checkout\_PaymentMethod.cshtml"
                                                                                                                                                                              Write(LT("Checkout.Text.SelectPayment", "Select a Payment Method"));

            
            #line default
            #line hidden
WriteLiteral("Required</span>\r\n                                       </div>\r\n                 " +
"                  </div>\r\n                               </div>\r\n               " +
"                <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n                                   <div");

WriteLiteral(" class=\"col-sm-4 col-xs-6\"");

WriteLiteral(">\r\n                                       <div");

WriteLiteral(" class=\"form-group no-margin\"");

WriteLiteral(">\r\n                                           <label");

WriteLiteral(" class=\"mastercard-label\"");

WriteLiteral(">");

            
            #line 68 "..\..\Views\Checkout\_PaymentMethod.cshtml"
                                                                      Write(LT("Checkout.Text.ExpiryMonth", "Expiry Month"));

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n                                           <span");

WriteLiteral(" class=\"icon-calendar icon-abso\"");

WriteLiteral("></span>\r\n                                           <select");

WriteLiteral(" class=\"mastercard-textbox form-control\"");

WriteLiteral(" name=\"expiry-month\"");

WriteLiteral(" id=\"expiry-month\"");

WriteLiteral(" onchange=\"removeClass(this)\"");

WriteLiteral(">\r\n                                               <option");

WriteLiteral(" value=\"00\"");

WriteLiteral(">MM</option>\r\n                                               <option");

WriteLiteral(" value=\"01\"");

WriteLiteral(">01</option>\r\n                                               <option");

WriteLiteral(" value=\"02\"");

WriteLiteral(">02</option>\r\n                                               <option");

WriteLiteral(" value=\"03\"");

WriteLiteral(">03</option>\r\n                                               <option");

WriteLiteral(" value=\"04\"");

WriteLiteral(">04</option>\r\n                                               <option");

WriteLiteral(" value=\"05\"");

WriteLiteral(">05</option>\r\n                                               <option");

WriteLiteral(" value=\"06\"");

WriteLiteral(">06</option>\r\n                                               <option");

WriteLiteral(" value=\"07\"");

WriteLiteral(">07</option>\r\n                                               <option");

WriteLiteral(" value=\"08\"");

WriteLiteral(">08</option>\r\n                                               <option");

WriteLiteral(" value=\"09\"");

WriteLiteral(">09</option>\r\n                                               <option");

WriteLiteral(" value=\"10\"");

WriteLiteral(">10</option>\r\n                                               <option");

WriteLiteral(" value=\"11\"");

WriteLiteral(">11</option>\r\n                                               <option");

WriteLiteral(" value=\"12\"");

WriteLiteral(">12</option>\r\n                                           </select>\r\n             " +
"                              <span");

WriteLiteral(" id=\"expiry-monthError\"");

WriteLiteral(" style=\"margin-top:-5px; display:none;\"");

WriteLiteral(" class=\"help-block for-validated-control has-error margin-bottom-sm\"");

WriteLiteral(">");

            
            #line 85 "..\..\Views\Checkout\_PaymentMethod.cshtml"
                                                                                                                                                                              Write(LT("Checkout.Text.Required", "Required"));

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n                                       </div>\r\n                         " +
"          </div>\r\n                                   <div");

WriteLiteral(" class=\"col-sm-4 col-xs-6\"");

WriteLiteral(">\r\n                                       <div");

WriteLiteral(" class=\"form-group no-margin\"");

WriteLiteral(">\r\n                                           <label");

WriteLiteral(" class=\"mastercard-label\"");

WriteLiteral(">");

            
            #line 90 "..\..\Views\Checkout\_PaymentMethod.cshtml"
                                                                      Write(LT("Checkout.Text.ExpiryYear", "Expiry Year"));

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n                                           <span");

WriteLiteral(" class=\"icon-calendar icon-abso\"");

WriteLiteral("></span>\r\n                                           <select");

WriteLiteral(" name=\"expiry-year\"");

WriteLiteral(" id=\"expiry-year\"");

WriteLiteral(" class=\"col-xs-12 form-control\"");

WriteLiteral(" onchange=\"removeClass(this)\"");

WriteLiteral("></select>\r\n                                           <span");

WriteLiteral(" id=\"expiry-yearError\"");

WriteLiteral(" style=\"margin-top: -5px; display:none;float: left;width: 100%;\"");

WriteLiteral(" class=\"help-block for-validated-control has-error margin-bottom-sm\"");

WriteLiteral(">");

            
            #line 93 "..\..\Views\Checkout\_PaymentMethod.cshtml"
                                                                                                                                                                                                      Write(LT("Checkout.Text.Required", "Required"));

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n                                       </div>\r\n                         " +
"          </div>\r\n                                   <div");

WriteLiteral(" class=\"col-sm-4 col-xs-12\"");

WriteLiteral(" id=\"inputCvv\"");

WriteLiteral(">\r\n                                       <div");

WriteLiteral(" class=\"form-group no-margin\"");

WriteLiteral(">\r\n                                           <label");

WriteLiteral(" class=\"mastercard-label\"");

WriteLiteral(">");

            
            #line 98 "..\..\Views\Checkout\_PaymentMethod.cshtml"
                                                                      Write(LT("Checkout.Text.CVVNumber", "CVV Number"));

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n                                           <span");

WriteLiteral(" class=\"icon-credit-card icon-abso\"");

WriteLiteral("></span>\r\n                                           <input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" id=\"security-code\"");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" value=\"\"");

WriteLiteral(" maxlength=\"3\"");

WriteLiteral(" max=\"3\"");

WriteLiteral(" onkeyup=\"removeClass(this)\"");

WriteLiteral(" placeholder=\"CVV\"");

WriteLiteral(" required />\r\n                                           <span");

WriteLiteral(" style=\"display:none\"");

WriteLiteral(" class=\"help-block for-validated-control has-error\"");

WriteLiteral(">");

            
            #line 101 "..\..\Views\Checkout\_PaymentMethod.cshtml"
                                                                                                                    Write(LT("Checkout.Text.InvalidMissing", "Invalid or missing"));

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n                                       </div>                           " +
"            \r\n                                   </div>\r\n                       " +
"        </div>\r\n                               <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n                                   <div");

WriteLiteral(" class=\"col-sm-12 col-xs-12\"");

WriteLiteral(">\r\n                                       <div");

WriteLiteral(" class=\"form-group no-margin\"");

WriteLiteral(">\r\n                                           <span");

WriteLiteral(" class=\"icon-user icon-abso\"");

WriteLiteral("></span>\r\n                                           <label");

WriteLiteral(" class=\"mastercard-label\"");

WriteLiteral(">");

            
            #line 109 "..\..\Views\Checkout\_PaymentMethod.cshtml"
                                                                      Write(LT("Checkout.Text.CardholderName", "Cardholder Name"));

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n                                           <input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" id=\"name-on-card\"");

WriteLiteral(" class=\"form-control \"");

WriteLiteral(" onkeyup=\"removeClass(this)\"");

WriteLiteral(" placeholder=\"Cardholder Name\"");

WriteLiteral(" value=\"\"");

WriteLiteral(" required />\r\n                                           <input");

WriteLiteral(" id=\"save-token\"");

WriteLiteral(" style=\"display:none\"");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" checked=\"checked\"");

WriteLiteral(" />\r\n                                           <span");

WriteLiteral(" id=\"name-on-cardError\"");

WriteLiteral(" style=\"margin-top:-5px; display:none;\"");

WriteLiteral(" class=\"help-block for-validated-control has-error\"");

WriteLiteral(">");

            
            #line 112 "..\..\Views\Checkout\_PaymentMethod.cshtml"
                                                                                                                                                             Write(LT("Checkout.Text.Required", "Required"));

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n                                       </div>\r\n                         " +
"          </div>\r\n                               </div>\r\n                       " +
"        <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n                                   <div");

WriteLiteral(" class=\"col-sm-12 col-xs-12\"");

WriteLiteral(">\r\n                                       <span");

WriteLiteral(" class=\"cvv-span\"");

WriteLiteral(">");

            
            #line 118 "..\..\Views\Checkout\_PaymentMethod.cshtml"
                                                         Write(LT("Checkout.Text.SignatureStrip", "*Find CVV on the back of your card on the signature strip."));

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n                                   </div>\r\n                             " +
"  </div>\r\n                           </form>\r\n                       </div>\r\n");

            
            #line 123 "..\..\Views\Checkout\_PaymentMethod.cshtml"
                   }

            
            #line default
            #line hidden
WriteLiteral("                    ");

            
            #line 124 "..\..\Views\Checkout\_PaymentMethod.cshtml"
                     if (payment.SystemName == PaymentMethodTypes.Givex.ToString())
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <div");

WriteLiteral(" class=\"col-sm-12 col-xs-12 default-border\"");

WriteLiteral(">\r\n                            <div");

WriteLiteral(" class=\"col-lg-12 no-padding no-margin\"");

WriteLiteral(">\r\n                                <div");

WriteLiteral(" class=\"col-lg-12 no-padding\"");

WriteLiteral(">\r\n                                    <h5>");

            
            #line 129 "..\..\Views\Checkout\_PaymentMethod.cshtml"
                                   Write(LT("Checkout.Text.GiftCard", "Please enter your Gift Card details"));

            
            #line default
            #line hidden
WriteLiteral("</h5>\r\n                                    <hr>\r\n                                " +
"    <div");

WriteLiteral(" class=\"col-xs-12 col-lg-6\"");

WriteLiteral(">\r\n                                        <form");

WriteLiteral(" class=\"form-inline margin-bottom-sm ng-pristine ng-valid\"");

WriteLiteral(" ng-init=\"givex.amount=basket.remaingAmt\"");

WriteLiteral(">\r\n                                            <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                                                <div");

WriteLiteral(" class=\"input-group\"");

WriteLiteral(">\r\n                                                    <div");

WriteLiteral(" class=\"input-group-addon border-radius-none\"");

WriteLiteral(">");

            
            #line 135 "..\..\Views\Checkout\_PaymentMethod.cshtml"
                                                                                                 Write(payment.AccountCode);

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n                                                    <div");

WriteLiteral(" class=\"col-xs-12 no-padding\"");

WriteLiteral("><input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" class=\"form-control border-radius-none ng-pristine ng-valid\"");

WriteLiteral(" ng-model=\"ck.givexCardNo\"");

WriteAttribute("ng-blur", Tuple.Create(" ng-blur=\"", 10057), Tuple.Create("\"", 10156)
, Tuple.Create(Tuple.Create("", 10067), Tuple.Create("ck.model.checkout.selectedPayment.cardInfo.cardNo=\'", 10067), true)
            
            #line 136 "..\..\Views\Checkout\_PaymentMethod.cshtml"
                                                                                                                                                                           , Tuple.Create(Tuple.Create("", 10118), Tuple.Create<System.Object, System.Int32>(payment.AccountCode
            
            #line default
            #line hidden
, 10118), false)
, Tuple.Create(Tuple.Create("", 10138), Tuple.Create("\'", 10138), true)
, Tuple.Create(Tuple.Create(" ", 10139), Tuple.Create("+", 10140), true)
, Tuple.Create(Tuple.Create(" ", 10141), Tuple.Create("ck.givexCardNo", 10142), true)
);

WriteLiteral(" placeholder=\"CardNo\"");

WriteLiteral("></div>\r\n                                                </div>\r\n                " +
"                                <div");

WriteLiteral(" class=\"input-group margin-top-sm\"");

WriteLiteral(">\r\n                                                    <div");

WriteLiteral(" class=\"col-xs-12 no-padding\"");

WriteLiteral("><input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" class=\"form-control border-radius-none ng-pristine ng-valid\"");

WriteLiteral(" ng-model=\"ck.model.checkout.selectedPayment.cardInfo.securityCode\"");

WriteLiteral(" placeholder=\"Security Code\"");

WriteLiteral("></div>\r\n                                                </div>\r\n                " +
"                                <div");

WriteLiteral(" class=\"input-group margin-top-sm\"");

WriteLiteral(">\r\n                                                    <div");

WriteLiteral(" class=\"col-xs-12 no-padding\"");

WriteLiteral("><input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" class=\"form-control border-radius-none ng-pristine ng-valid\"");

WriteLiteral(" ng-model=\"ck.model.checkout.selectedPayment.cardInfo.amount\"");

WriteLiteral(" placeholder=\"Amount\"");

WriteLiteral(" only-digits=\"\"");

WriteLiteral("></div>\r\n                                                </div>\r\n                " +
"                            </div>\r\n                                        </fo" +
"rm>\r\n\r\n                                    </div>\r\n                             " +
"       <div");

WriteLiteral(" class=\"col-xs-12 col-lg-6\"");

WriteLiteral(">\r\n                                        <div><a");

WriteLiteral(" href=\"\"");

WriteLiteral(" class=\"gift-card-back\"");

WriteLiteral("></a></div>\r\n                                    </div>\r\n                        " +
"        </div>\r\n                            </div>\r\n                        </di" +
"v>\r\n");

            
            #line 154 "..\..\Views\Checkout\_PaymentMethod.cshtml"

                    }

            
            #line default
            #line hidden
WriteLiteral("                    ");

            
            #line 156 "..\..\Views\Checkout\_PaymentMethod.cshtml"
                     if (payment.SystemName == PaymentMethodTypes.Klarna.ToString())
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n                            <div");

WriteLiteral(" class=\"col-sm-12 col-xs-12\"");

WriteLiteral(">\r\n                                <div");

WriteLiteral(" id=\"klarna_container\"");

WriteLiteral("></div>\r\n                            </div>\r\n                        </div>\r\n");

WriteLiteral("                        <script");

WriteLiteral(" src=\"https://x.klarnacdn.net/kp/lib/v1/api.js\"");

WriteLiteral(" async></script>\r\n");

            
            #line 164 "..\..\Views\Checkout\_PaymentMethod.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                            <div ng-cloak");

WriteLiteral(" class=\"alert alert-success successBlock\"");

WriteLiteral(" ng-show=\"ck.messageSuccess\"");

WriteLiteral(">\r\n");

WriteLiteral("                                ");

            
            #line 166 "..\..\Views\Checkout\_PaymentMethod.cshtml"
                           Write(LT("Checkout.Text.SelectPayment", "Select a Payment Method"));

            
            #line default
            #line hidden
            
            #line 166 "..\..\Views\Checkout\_PaymentMethod.cshtml"
                                                                                        Write(LT("Checkout.Text.PartialPayment", "Partial payment of amount"));

            
            #line default
            #line hidden
WriteLiteral(" <span");

WriteLiteral(" ng-bind=\"ck.partialAmount\"");

WriteLiteral("></span> ");

            
            #line 166 "..\..\Views\Checkout\_PaymentMethod.cshtml"
                                                                                                                                                                                                  Write(LT("Checkout.Text.BeenDone", "has been done. You can choose to complete your remaining payment using any other mode."));

            
            #line default
            #line hidden
WriteLiteral("\r\n                            </div>\r\n                            <div ng-cloak");

WriteLiteral(" class=\"alert alert-danger alertBlock\"");

WriteLiteral(" ng-show=\"ck.errorMessage\"");

WriteLiteral(">\r\n                                <span>");

            
            #line 169 "..\..\Views\Checkout\_PaymentMethod.cshtml"
                                 Write(LT("Checkout.Text.ConfirmingPayment", "There has been some issue confirming payment.Please check and try again."));

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n                            </div>\r\n                </div>\r\n");

            
            #line 172 "..\..\Views\Checkout\_PaymentMethod.cshtml"
            }
        
            
            #line default
            #line hidden
WriteLiteral("\r\n    </div>\r\n    <!-- /.row -->\r\n\r\n</div>\r\n\r\n<!-- /.content -->\r\n<div");

WriteLiteral(" ng-show=\"ck.hidebillingaddress\"");

WriteLiteral(">\r\n");

WriteLiteral("    ");

            
            #line 181 "..\..\Views\Checkout\_PaymentMethod.cshtml"
Write(Html.Partial("~/views/checkout/_billingaddress.cshtml", @Model.Checkout.BillingAddress, new ViewDataDictionary { { "prefix", "_payment" } }));

            
            #line default
            #line hidden
WriteLiteral("\r\n</div>\r\n\r\n<div");

WriteLiteral(" class=\"box-footer margin-top-lg\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"pull-left col-sm-4 col-xs-12 no-padding margin-bottom-sm\"");

WriteLiteral(" ng-show=\"ck.hideBillingAddress\"");

WriteLiteral(">\r\n        <button");

WriteLiteral(" type=\"button\"");

WriteLiteral(" class=\"animate btn-default\"");

WriteLiteral(" data-toggle=\"modal\"");

WriteLiteral(" data-target=\"#userAddress-modal\"");

WriteLiteral(" ng-click=\"ck.addressText=\'billing\'\"");

WriteLiteral(" ng-show=\"(ck.userAddresses.length>0) && ck.billingAddress\"");

WriteLiteral(">");

            
            #line 186 "..\..\Views\Checkout\_PaymentMethod.cshtml"
                                                                                                                                                                                                         Write(LT("Checkout.Text.ChangeBillingAddress", "Change Billing Address"));

            
            #line default
            #line hidden
WriteLiteral("</button>\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"pull-left col-sm-4 col-xs-5 no-padding-left margin-bottom-sm\"");

WriteLiteral(">\r\n        <inut");

WriteLiteral(" id=\"addressPanel\"");

WriteLiteral(" name=\"accordion-1\"");

WriteLiteral(" type=\"radio\"");

WriteLiteral(">\r\n        <label");

WriteLiteral(" class=\"animate btn-default btn-lg width-full\"");

WriteLiteral(" for=\"addressPanel\"");

WriteLiteral("><i");

WriteLiteral(" class=\"fa fa-caret-left\"");

WriteLiteral("></i> ");

            
            #line 190 "..\..\Views\Checkout\_PaymentMethod.cshtml"
                                                                                                            Write(LT("Checkout.Text.Back", "Back"));

            
            #line default
            #line hidden
WriteLiteral("</label></inut>\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"pull-right col-sm-3 col-xs-7 no-padding text-right\"");

WriteLiteral(">      \r\n");

            
            #line 193 "..\..\Views\Checkout\_PaymentMethod.cshtml"
        
            
            #line default
            #line hidden
            
            #line 193 "..\..\Views\Checkout\_PaymentMethod.cshtml"
         if (SessionContext.CurrentUser == null)
        {
            if (Model.RegistrationPrompt)
            {

            
            #line default
            #line hidden
WriteLiteral("                <button");

WriteLiteral(" class=\"animate btn-orange btn-lg\"");

WriteLiteral(" ng-hide=\"ck.continue\"");

WriteLiteral(" ng-click=\"ck.check=true;ck.setPassword()\"");

WriteLiteral(">\r\n");

WriteLiteral("                    ");

            
            #line 198 "..\..\Views\Checkout\_PaymentMethod.cshtml"
               Write(LT("Checkout.Label.OrderSummary", "PAY"));

            
            #line default
            #line hidden
WriteLiteral(" {{ck.basket.grandTotal.formatted.withTax}} <i");

WriteLiteral(" class=\"fa fa-caret-right\"");

WriteLiteral("></i>\r\n                </button>\r\n");

WriteLiteral("                <button");

WriteLiteral(" ng-click=\"ck.continuePlaceOrder()\"");

WriteLiteral(" ng-show=\"ck.continue\"");

WriteLiteral(" class=\"animate btn-orange btn-lg\"");

WriteLiteral(">\r\n");

WriteLiteral("                    ");

            
            #line 201 "..\..\Views\Checkout\_PaymentMethod.cshtml"
               Write(LT("Checkout.Label.OrderSummary", "Continue"));

            
            #line default
            #line hidden
WriteLiteral(" <i");

WriteLiteral(" class=\"fa fa-caret-right\"");

WriteLiteral("></i>\r\n                </button>\r\n");

            
            #line 203 "..\..\Views\Checkout\_PaymentMethod.cshtml"
            }
            else
            {

            
            #line default
            #line hidden
WriteLiteral("                <button");

WriteLiteral(" ng-click=\"ck.placeOrder()\"");

WriteLiteral(" ng-hide=\"ck.continue\"");

WriteLiteral(" class=\"animate btn-orange btn-lg\"");

WriteLiteral(">\r\n");

WriteLiteral("                    ");

            
            #line 207 "..\..\Views\Checkout\_PaymentMethod.cshtml"
               Write(LT("Checkout.Label.OrderSummary", "PAY"));

            
            #line default
            #line hidden
WriteLiteral(" {{ck.basket.grandTotal.formatted.withTax}}  <i");

WriteLiteral(" class=\"fa fa-caret-right\"");

WriteLiteral("></i>\r\n                </button>\r\n");

WriteLiteral("                <button");

WriteLiteral(" ng-click=\"ck.continuePlaceOrder()\"");

WriteLiteral(" ng-show=\"ck.continue\"");

WriteLiteral(" class=\"animate btn-orange btn-lg\"");

WriteLiteral(">\r\n");

WriteLiteral("                    ");

            
            #line 210 "..\..\Views\Checkout\_PaymentMethod.cshtml"
               Write(LT("Checkout.Label.OrderSummary", "Continue"));

            
            #line default
            #line hidden
WriteLiteral(" <i");

WriteLiteral(" class=\"fa fa-caret-right\"");

WriteLiteral("></i>\r\n                </button>\r\n");

            
            #line 212 "..\..\Views\Checkout\_PaymentMethod.cshtml"
            }
        }
        else
        {

            
            #line default
            #line hidden
WriteLiteral("            <button");

WriteLiteral(" ng-click=\"ck.placeOrder()\"");

WriteLiteral(" ng-hide=\"ck.continue\"");

WriteLiteral(" class=\"animate btn-orange btn-lg\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 217 "..\..\Views\Checkout\_PaymentMethod.cshtml"
           Write(LT("Checkout.Label.OrderSummary", "PAY"));

            
            #line default
            #line hidden
WriteLiteral(" {{ck.basket.grandTotal.formatted.withTax}}  <i");

WriteLiteral(" class=\"fa fa-caret-right\"");

WriteLiteral("></i>\r\n            </button>\r\n");

WriteLiteral("            <button");

WriteLiteral(" ng-click=\"ck.continuePlaceOrder()\"");

WriteLiteral(" ng-show=\"ck.continue\"");

WriteLiteral(" class=\"animate btn-orange btn-lg\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 220 "..\..\Views\Checkout\_PaymentMethod.cshtml"
           Write(LT("Checkout.Label.OrderSummary", "Continue"));

            
            #line default
            #line hidden
WriteLiteral(" <i");

WriteLiteral(" class=\"fa fa-caret-right\"");

WriteLiteral("></i>\r\n            </button>\r\n");

            
            #line 222 "..\..\Views\Checkout\_PaymentMethod.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n</div>");

        }
    }
}
#pragma warning restore 1591
