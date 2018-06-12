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
    
    #line 12 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
    using Microsoft.Web.Mvc;
    
    #line default
    #line hidden
    
    #line 14 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
    using Omnicx.API.SDK.Models;
    
    #line default
    #line hidden
    using Omnicx.WebStore;
    using Omnicx.WebStore.Core;
    
    #line 13 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
    using Omnicx.WebStore.Core.Controllers;
    
    #line default
    #line hidden
    
    #line 15 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
    using Omnicx.WebStore.Core.Helpers;
    
    #line default
    #line hidden
    
    #line 16 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
    using Omnicx.WebStore.Framework.Helpers;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Blog/Layout1/BlogCategory.cshtml")]
    public partial class _Views_Blog_Layout1_BlogCategory_cshtml : Omnicx.WebStore.Core.Services.Infrastructure.CustomBaseViewPage<Omnicx.API.SDK.Models.Site.BlogDetailViewModel>
    {
        public _Views_Blog_Layout1_BlogCategory_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 1 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
  
/*
Name: Blog list of selected category
Purpose: All blogs of selected category
Structure: /Views/Blog/Layout1/BlogCategory.cshtml
Contains (Partial Views Used):
    a-/Views/Shared/Layout1/_LayoutBlog.cshtml
Contained In (Where we Use this View) :
    a-/Views/Shared/Layout1/Blog.cshtml
*/

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 18 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
  
    ViewBag.Title = @ViewBag.selectedGroup;
    Layout = "~/Views/Shared/Layout1/_LayoutBlog.cshtml";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 23 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
  /*New Design*/
            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 25 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
  /*Start Category Name*/
            
            #line default
            #line hidden
WriteLiteral("\r\n<style>\r\n    #landing-banner {\r\n        display: none !important;\r\n    }\r\n</sty" +
"le>\r\n<div");

WriteLiteral(" class=\"col-sm-12 margin-top-md\"");

WriteLiteral(">\r\n    <ul");

WriteLiteral(" class=\"breadcrumb\"");

WriteLiteral(">\r\n        <li><a");

WriteLiteral(" href=\"/\"");

WriteLiteral(">Home</a></li>\r\n        <li><a");

WriteAttribute("href", Tuple.Create(" href=\"", 974), Tuple.Create("\"", 1009)
            
            #line 34 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
, Tuple.Create(Tuple.Create("", 981), Tuple.Create<System.Object, System.Int32>(Url.Action("Blogs", "Blog")
            
            #line default
            #line hidden
, 981), false)
);

WriteLiteral(">");

            
            #line 34 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
                                              Write(LT("Blogs.Links.Blogs", "Blogs"));

            
            #line default
            #line hidden
WriteLiteral("</a></li>\r\n        <li>");

            
            #line 35 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
       Write(ViewBag.selectedGroup);

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n    </ul>\r\n</div>\r\n");

            
            #line 38 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
 if (@Model.BlogList.Results != null && @Model.BlogList.Results.Count > 0)
{

            
            #line default
            #line hidden
WriteLiteral("<div");

WriteLiteral(" class=\"row margin-top-md\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"col-sm-8 col-md-8 col-md-offset-2 col-sm-offset-2\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"row border-bottom-blog\"");

WriteLiteral(">\r\n            <h1");

WriteLiteral(" class=\"blog-category-landing-h1\"");

WriteLiteral(">");

            
            #line 43 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
                                            Write(Model.BlogList.Results[0].Category);

            
            #line default
            #line hidden
WriteLiteral("</h1>\r\n        </div>\r\n    </div>\r\n</div>\r\n");

            
            #line 47 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
}
else
{ 

            
            #line default
            #line hidden
WriteLiteral("<div");

WriteLiteral(" class=\"row margin-top-md\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"col-sm-8 col-md-8 col-md-offset-2 col-sm-offset-2\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"row border-bottom-blog\"");

WriteLiteral(">\r\n            <h3>No blogs exists for this category.</h3>\r\n        </div>\r\n    <" +
"/div>\r\n</div>\r\n");

            
            #line 57 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"

}

            
            #line default
            #line hidden
            
            #line 59 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
  /*Start Category Name*/
            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 61 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
  /*Start Category Blog listing*/
            
            #line default
            #line hidden
WriteLiteral("\r\n<div");

WriteLiteral(" class=\"row margin-top-md\"");

WriteLiteral(">\r\n");

            
            #line 63 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
    
            
            #line default
            #line hidden
            
            #line 63 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
     if(@Model.BlogList.Results != null && @Model.BlogList.Results.Count > 0)
    {
    foreach (var item in @Model.BlogList.Results)
    {

            
            #line default
            #line hidden
WriteLiteral("        <div");

WriteLiteral(" class=\"col-sm-8 col-md-8 col-md-offset-2 col-sm-offset-2\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"row border-bottom-blog\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"col-sm-4\"");

WriteLiteral(">\r\n");

            
            #line 70 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 70 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
                     if (item.BlogImage == null)
                    {

            
            #line default
            #line hidden
WriteLiteral("                         <a");

WriteAttribute("href", Tuple.Create(" href=\"", 2234), Tuple.Create("\"", 2298)
            
            #line 72 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
, Tuple.Create(Tuple.Create("", 2241), Tuple.Create<System.Object, System.Int32>(Url.Action("blogdetail", "blog", new { url = @item.URL})
            
            #line default
            #line hidden
, 2241), false)
);

WriteLiteral("><img");

WriteAttribute("ng-src", Tuple.Create(" ng-src=\"", 2304), Tuple.Create("\"", 2355)
, Tuple.Create(Tuple.Create("", 2313), Tuple.Create<System.Object, System.Int32>(Href("~/assets/theme/ocx/images/noimagefound.jpg")
, 2313), false)
);

WriteLiteral(" class=\"img-responsive\"");

WriteLiteral("></a>\r\n");

            
            #line 73 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
                    }
                    else
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <a");

WriteAttribute("href", Tuple.Create(" href=\"", 2484), Tuple.Create("\"", 2548)
            
            #line 76 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
, Tuple.Create(Tuple.Create("", 2491), Tuple.Create<System.Object, System.Int32>(Url.Action("blogdetail", "blog", new { url = @item.URL})
            
            #line default
            #line hidden
, 2491), false)
);

WriteLiteral("><img");

WriteAttribute("ng-src", Tuple.Create(" ng-src=\"", 2554), Tuple.Create("\"", 2578)
            
            #line 76 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
                         , Tuple.Create(Tuple.Create("", 2563), Tuple.Create<System.Object, System.Int32>(item.BlogImage
            
            #line default
            #line hidden
, 2563), false)
);

WriteLiteral(" class=\"img-responsive\"");

WriteLiteral(" onerror=\"this.src = \'/assets/theme/ocx/images/noimagefound.jpg\'\"");

WriteLiteral("></a>\r\n");

            
            #line 77 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                </div>\r\n                <div");

WriteLiteral(" class=\"col-sm-8\"");

WriteLiteral(">\r\n                    <h6");

WriteLiteral(" class=\"blog-h6\"");

WriteLiteral(">");

            
            #line 80 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
                                   Write(Model.BlogList.Results[0].Category);

            
            #line default
            #line hidden
WriteLiteral("</h6>\r\n                    <h4");

WriteLiteral(" class=\"blog-h4\"");

WriteLiteral(">\r\n                        <a");

WriteAttribute("href", Tuple.Create(" href=\"", 2911), Tuple.Create("\"", 2975)
            
            #line 82 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
, Tuple.Create(Tuple.Create("", 2918), Tuple.Create<System.Object, System.Int32>(Url.Action("blogdetail", "blog", new { url = @item.URL})
            
            #line default
            #line hidden
, 2918), false)
);

WriteLiteral(">");

            
            #line 82 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
                                                                                       Write(item.Title);

            
            #line default
            #line hidden
WriteLiteral("</a>\r\n                    </h4>\r\n                    <p");

WriteLiteral(" class=\"blog-intro\"");

WriteLiteral(">\r\n");

WriteLiteral("                        ");

            
            #line 85 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
                   Write(item.Abstract);

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </p>\r\n                    <p");

WriteLiteral(" class=\"blog-category\"");

WriteLiteral(">\r\n");

WriteLiteral("                        ");

            
            #line 88 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
                   Write(item.Days);

            
            #line default
            #line hidden
WriteLiteral(" days ago. By <a");

WriteAttribute("href", Tuple.Create(" href=\"", 3228), Tuple.Create("\"", 3252)
, Tuple.Create(Tuple.Create("", 3235), Tuple.Create("/", 3235), true)
            
            #line 88 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
, Tuple.Create(Tuple.Create("", 3236), Tuple.Create<System.Object, System.Int32>(item.EditorSlug
            
            #line default
            #line hidden
, 3236), false)
);

WriteLiteral(">");

            
            #line 88 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
                                                                       Write(item.Author);

            
            #line default
            #line hidden
WriteLiteral("</a>\r\n                    </p>\r\n                </div>\r\n            </div>\r\n\r\n\r\n " +
"       </div>\r\n");

            
            #line 95 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
    }
    }

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"col-sm-6 no-padding\"");

WriteLiteral(" ng-controller=\"blogCtrl as bm\"");

WriteLiteral(">\r\n            <ul");

WriteLiteral(" class=\"pull-right pagination\"");

WriteLiteral(">\r\n");

            
            #line 100 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
                
            
            #line default
            #line hidden
            
            #line 100 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
                 if (@Model.BlogList.Results.Count > 0)
                {
                    if (@Model.BlogList.Total > @Model.PageSize)
                    {
                        for (var i = 1; i <= Convert.ToDecimal(@Model.BlogList.Total / @Model.PageSize)+1; i++)
                        {

            
            #line default
            #line hidden
WriteLiteral("                            <li");

WriteLiteral(" class=\"ng-scope\"");

WriteLiteral(">\r\n                                <a");

WriteAttribute("href", Tuple.Create(" href=\"", 3911), Tuple.Create("\"", 3961)
, Tuple.Create(Tuple.Create("", 3918), Tuple.Create("/", 3918), true)
            
            #line 107 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
, Tuple.Create(Tuple.Create("", 3919), Tuple.Create<System.Object, System.Int32>(Model.BlogList.Results[0].CategorySlug
            
            #line default
            #line hidden
, 3919), false)
, Tuple.Create(Tuple.Create("", 3958), Tuple.Create("/", 3958), true)
            
            #line 107 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
  , Tuple.Create(Tuple.Create("", 3959), Tuple.Create<System.Object, System.Int32>(i
            
            #line default
            #line hidden
, 3959), false)
);

WriteLiteral(">");

            
            #line 107 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
                                                                                 Write(i);

            
            #line default
            #line hidden
WriteLiteral("</a>\r\n                            </li>\r\n");

            
            #line 109 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
                        }
                    }
                    if (@Model.BlogList.Total == @Model.PageSize)
                    {
                        for (var i = 1; i <= Convert.ToDecimal(@Model.BlogList.Total / @Model.PageSize); i++)
                        {

            
            #line default
            #line hidden
WriteLiteral("                            <li");

WriteLiteral(" class=\"ng-scope\"");

WriteLiteral(">\r\n                                <a");

WriteAttribute("href", Tuple.Create(" href=\"", 4369), Tuple.Create("\"", 4419)
, Tuple.Create(Tuple.Create("", 4376), Tuple.Create("/", 4376), true)
            
            #line 116 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
, Tuple.Create(Tuple.Create("", 4377), Tuple.Create<System.Object, System.Int32>(Model.BlogList.Results[0].CategorySlug
            
            #line default
            #line hidden
, 4377), false)
, Tuple.Create(Tuple.Create("", 4416), Tuple.Create("/", 4416), true)
            
            #line 116 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
  , Tuple.Create(Tuple.Create("", 4417), Tuple.Create<System.Object, System.Int32>(i
            
            #line default
            #line hidden
, 4417), false)
);

WriteLiteral(">");

            
            #line 116 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
                                                                                 Write(i);

            
            #line default
            #line hidden
WriteLiteral("</a>\r\n                            </li>\r\n");

            
            #line 118 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
                        }
                    }
                }
                
            
            #line default
            #line hidden
            
            #line 126 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
               

            
            #line default
            #line hidden
WriteLiteral("            </ul>\t\r\n\r\n        </div>\r\n    </div>\r\n\r\n</div>\r\n");

            
            #line 133 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
  /*End Category Blog listing*/
            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 135 "..\..\Views\Blog\Layout1\BlogCategory.cshtml"
  /*OLD CODE*/
            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

DefineSection("scripts", () => {

WriteLiteral("\r\n    <script>\r\n        window.app.constant(\'blogConfig\', {\r\n        });\r\n    </s" +
"cript>\r\n");

});

        }
    }
}
#pragma warning restore 1591
