using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Omnicx.API.SDK.Api.Infra;


using Newtonsoft.Json;
using System.Web.Mvc;
using Omnicx.WebStore.Models.Keys;
using Omnicx.WebStore.Models.Enums;
using Omnicx.WebStore.Models.Site;

namespace Omnicx.API.SDK.Helpers
{
    public class HeadTagBuilder : IHeadTagBuilder
    {
        #region Fields
        private readonly List<string> _titleParts;
        private readonly List<string> _metaDescriptionParts;
        private readonly List<string> _metaKeywords;
        private readonly List<string> _metaTitle;
        private readonly List<string> _canonicalUrlParts;
        private readonly Dictionary<string, object> _dataLayer;
        private readonly List<string> _metaKeywordParts;
        private readonly IConfigApi _configRepository;
        public const string DomainKey = "DomainSettings-DomainName";

        #endregion

        #region Ctor
        public HeadTagBuilder(IConfigApi configRepository)
        {
            _titleParts = new List<string>();
            _metaDescriptionParts = new List<string>();
            _metaKeywords = new List<string>();
            _canonicalUrlParts = new List<string>();
            _metaTitle = new List<string>();
            _dataLayer = new Dictionary<string, object>();
            _metaKeywordParts = new List<string>();
            _configRepository = configRepository;
        }
        #endregion

        #region "Methods"
        public virtual string GenerateTitle(bool addDefaultTitle)
        {
            //TO DO:Need to manage with site Settings
            var sessionContext = DependencyResolver.Current.GetService<ISessionContext>();
            //var domainName = _configRepository.GetDefaultSetting(DomainKey);
            string result = sessionContext.CurrentSiteConfig?.SeoSettings?.DefaultTitle;

            // result= _titleParts.LastOrDefault();
            var specificTitle = string.Join(":", _titleParts.AsEnumerable().Reverse().ToArray());
            if (!String.IsNullOrEmpty(specificTitle.Trim()))
            {
                if (addDefaultTitle)
                {
                    //store name + page title
                    result = true ? string.Join(" ", result, specificTitle) : string.Join(" ", specificTitle, result);
                }
                else
                {
                    //page title only
                    result = specificTitle;
                }
            }

            //if the last character in the string is : then remove the extra colon 
            if (!string.IsNullOrEmpty(result))
            {
                if (result.Substring(result.Length - 2, 2).Contains(":"))
                {
                    result = result.Remove(result.Length - 2, 2);
                }
            }
            return result;

        }
        #region "Meta Tags"
        public virtual string GenerateMetaDescription()
        {
            //TO DO:Need to manage with site Settings
            string defaulteDescription = "";
            var metaDescription = string.Join(", ", _metaDescriptionParts.AsEnumerable().Reverse().ToArray());
            var result = !String.IsNullOrEmpty(metaDescription) ? metaDescription : defaulteDescription;
            return result;
        }
        public virtual string GenerateMetaKeywords()
        {
            //TO DO:Need to manage with site Settings
            var defaultKeywords = "";
            var metaKeywords = string.Join(", ", _metaKeywords.AsEnumerable().Reverse().ToArray());
            var result = !String.IsNullOrEmpty(metaKeywords) ? metaKeywords : defaultKeywords;
            return result;
        }
        public string GenerateMetaTitle()
        {
            var defaultMetaTitle = "";
            var metaTitle = string.Join(", ", _metaTitle.AsEnumerable().Reverse().ToArray());
            var result = !String.IsNullOrEmpty(metaTitle) ? metaTitle : defaultMetaTitle;
            return result;
        }

        #endregion

        public virtual void AddCanonicalUrlParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _canonicalUrlParts.Add(part);
        }

        public virtual string GenerateCanonicalUrls()
        {
            var result = new StringBuilder();
            foreach (var canonicalUrl in _canonicalUrlParts)
            {
                result.AppendFormat("<link rel=\"canonical\" href=\"{0}\" />", canonicalUrl);
                result.Append(Environment.NewLine);
            }
            return result.ToString();
        }

        public virtual void AddTitleParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _titleParts.Add(part);
        }
        public virtual void AppendTitleParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _titleParts.Insert(0, part);
        }
        public virtual void AddMetaKeywordsParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _metaKeywords.Add(part);
        }
        public virtual void AddMetaDescriptionParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _metaDescriptionParts.Add(part);
        }

        public virtual void AppendMetaDescriptionParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _metaDescriptionParts.Insert(0, part);
        }
        public void AddMetaTitleParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _metaTitle.Add(part);
        }

        public string GenerateDataLayer()
        {
            var emptyDl = "new []";
            var dataLayer = JsonConvert.SerializeObject(_dataLayer);
            var result = !String.IsNullOrEmpty(dataLayer) ? "[" + dataLayer + "]" : emptyDl;
            return result;
        }

        public void AddDataLayer(string key, object value, bool overWriteifExists = false)
        {
            if (string.IsNullOrEmpty(key) || _dataLayer.ContainsKey(key))
            {
                if (overWriteifExists)
                    _dataLayer.Remove(key);
                else
                    return;
            }
            _dataLayer.Add(key, value);
        }
        public bool DataLayerKeyExists(string key)
        {
           return _dataLayer.ContainsKey(key);
        }
        public virtual void AppendMetaKeywordParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _metaKeywordParts.Insert(0, part);
        }
        public string GetOmnilyticUrl()
        {
            return ConfigKeys.OmnilyticUrl?.ToString() + GetOmnilyticId();
        }
        public string GetOmnilyticId()
        {
            return ConfigKeys.OmnilyticId?.ToString();
        }
        public string GenerateGlobalSnippets(SnippetPlacements placement)
        {
            var sessionContext = DependencyResolver.Current.GetService<ISessionContext>();
            var osb = new StringBuilder();
            if (sessionContext.CurrentSiteConfig.Snippets != null)
            {
                foreach(var snippet in sessionContext.CurrentSiteConfig.Snippets.Where(x=>x.Placement== placement).ToList())
                {
                    osb.AppendLine(snippet.Content);
                }
            }
            if (System.Web.HttpContext.Current.Items[Constants.HTTP_CONTEXT_ITEM_SNIPPETS] != null)
            {
                var snippets = (List<SnippetModel>)System.Web.HttpContext.Current.Items[Constants.HTTP_CONTEXT_ITEM_SNIPPETS];
                foreach (var snippet in snippets.Where(x => x.Placement == placement).ToList())
                {
                    osb.AppendLine(snippet.Content);
                }
            }
            return osb.ToString();
        }
        public string GeneratePageSnippets(SnippetPlacements placement)
        {
            var osb = new StringBuilder();
            if (System.Web.HttpContext.Current.Items[Constants.HTTP_CONTEXT_ITEM_SNIPPETS] != null)
            {
                var snippets = (List<SnippetModel>)System.Web.HttpContext.Current.Items[Constants.HTTP_CONTEXT_ITEM_SNIPPETS];
                foreach (var snippet in snippets.Where(x => x.Placement == placement).ToList())
                {
                    osb.AppendLine(snippet.Content);
                }
            }
            return osb.ToString();
        }
        #endregion
    }
}
