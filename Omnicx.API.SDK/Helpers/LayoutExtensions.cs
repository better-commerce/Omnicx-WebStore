using System.Web.Mvc;
using Omnicx.API.SDK.Entities;

namespace Omnicx.API.SDK.Helpers
{
     public static class LayoutExtensions
    {
        #region "Invoked from Main Layout View"
        //the methods in this region bascially RENDER the actual values based on whats populated / passed from the specific views 

        /// <summary>
        /// Generates the Page Title, invoked in the _LayoutHeadTag
        /// </summary>
        /// <param name="html"></param>
        /// <param name="addDefaultTitle"></param>
        /// <param name="part"></param>
        /// <returns></returns>
        public static MvcHtmlString Title(this HtmlHelper html, bool addDefaultTitle, string part = "")
         {
            var pageHeadBuilder = DependencyResolver.Current.GetService<IHeadTagBuilder>();
            html.AppendTitleParts(part);
            return MvcHtmlString.Create(html.Encode(pageHeadBuilder.GenerateTitle(addDefaultTitle)));
        }
        public static void AppendTitleParts(this HtmlHelper html, string part)
        {
            var pageHeadBuilder = DependencyResolver.Current.GetService<IHeadTagBuilder>();
            pageHeadBuilder.AppendTitleParts(" " + part);
        }
        public static MvcHtmlString MetaDescription(this HtmlHelper html, string part = "")
         {
            var pageHeadBuilder = DependencyResolver.Current.GetService<IHeadTagBuilder>();
            html.AppendMetaDescriptionParts(part);
            return MvcHtmlString.Create(html.Encode(pageHeadBuilder.GenerateMetaDescription()));
        }
        public static void AppendMetaDescriptionParts(this HtmlHelper html, string part)
        {
            var pageHeadBuilder = DependencyResolver.Current.GetService<IHeadTagBuilder>();
            pageHeadBuilder.AppendMetaDescriptionParts(part);
        }
        public static MvcHtmlString MetaTitle(this HtmlHelper html, string part = "")
        {
            var pageHeadBuilder = DependencyResolver.Current.GetService<IHeadTagBuilder>();
            html.AddMetaTitle(part);
            return MvcHtmlString.Create(html.Encode(pageHeadBuilder.GenerateMetaTitle()));
        }
        public static MvcHtmlString MetaKeywords(this HtmlHelper html, string part = "")
        {
            var pageHeadBuilder = DependencyResolver.Current.GetService<IHeadTagBuilder>();
            html.AppendMetaKeywordParts(part);
            return MvcHtmlString.Create(html.Encode(pageHeadBuilder.GenerateMetaKeywords()));
        }
        public static void AppendMetaKeywordParts(this HtmlHelper html, string part)
        {
            var pageHeadBuilder = DependencyResolver.Current.GetService<IHeadTagBuilder>();
            pageHeadBuilder.AppendMetaKeywordParts(part);
        }
        public static MvcHtmlString CanonicalUrl(this HtmlHelper html, string url = "")
        {
            var pageHeadBuilder = DependencyResolver.Current.GetService<IHeadTagBuilder>();
            html.AddCanonicalUrl(url);
            return MvcHtmlString.Create(pageHeadBuilder.GenerateCanonicalUrls());
        }
        public static MvcHtmlString DataLayer(this HtmlHelper html)
        {
            var pageHeadBuilder = DependencyResolver.Current.GetService<IHeadTagBuilder>();
            return MvcHtmlString.Create(pageHeadBuilder.GenerateDataLayer());
        }
        public static MvcHtmlString GetOmnilyticUrl(this HtmlHelper html)
        {
            var pageHeadBuilder = DependencyResolver.Current.GetService<IHeadTagBuilder>();
            return MvcHtmlString.Create(pageHeadBuilder.GetOmnilyticUrl());
        }
        public static MvcHtmlString GetOmnilyticId(this HtmlHelper html)
        {
            var pageHeadBuilder = DependencyResolver.Current.GetService<IHeadTagBuilder>();
            return MvcHtmlString.Create(pageHeadBuilder.GetOmnilyticId());
        }

        #endregion

        #region AddPartMethods

        //these "AddxxxxParts" methods are invoked from above methods & also the specific views - ProductDetail, BrandDetail to add the required parts to the final info taht needs to be rendered in the HTML
        public static void AddTitleParts(this HtmlHelper html, string part)
        {
            var pageHeadBuilder = DependencyResolver.Current.GetService<IHeadTagBuilder>();
            pageHeadBuilder.AddTitleParts(part);
        }
        public static void AddMetaKeywordsParts(this HtmlHelper html, string defaultKeywords = "")
        {
            var pageHeadBuilder = DependencyResolver.Current.GetService<IHeadTagBuilder>();
            pageHeadBuilder.AddMetaKeywordsParts(defaultKeywords);
        }
        public static void AddMetaTitle(this HtmlHelper html, string defaultTitle = "")
        {
            var pageHeadBuilder = DependencyResolver.Current.GetService<IHeadTagBuilder>();
            pageHeadBuilder.AddMetaTitleParts(defaultTitle);
        }
        public static void AddMetaDescriptionParts(this HtmlHelper html, string part)
        {
            var pageHeadBuilder = DependencyResolver.Current.GetService<IHeadTagBuilder>();
            pageHeadBuilder.AddMetaDescriptionParts(part);
        }
        public static void AddCanonicalUrl(this HtmlHelper html, string url)
        {
            var pageHeadBuilder = DependencyResolver.Current.GetService<IHeadTagBuilder>();
            pageHeadBuilder.AddCanonicalUrlParts(url);
        }
        #endregion
        public static void AddDataLayer(this HtmlHelper html, DataLayerObjectType dataLayerObjectType, object obj)
        {
            //TODO: write the logic to generate the datalayer 
            var pageHeadBuilder = DependencyResolver.Current.GetService<IHeadTagBuilder>();
            //pageHeadBuilder.AddCanonicalUrlParts(url);
        }

        
    }
}
