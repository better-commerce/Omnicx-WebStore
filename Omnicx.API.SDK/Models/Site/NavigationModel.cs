using System;
using System.Collections.Generic;

namespace Omnicx.API.SDK.Models.Site
{
    [Serializable]
    public class NavigationModel 
    {
        public List<SubNavModel> Header { get; set; }
        public List<SubNavModel> Footer { get; set; }
    }
    [Serializable]
    public class SubNavModel
    {
        public string RecordId { get; set; }
        public string Caption { get; set; }
        public int NavPosition { get; set; }
        public int WidthPct { get; set; }
        public int AlignmentType { get; set; }
        public int DisplayOrder { get; set; }
        public int ColumnCount { get; set; }
        public bool IsTopNav { get; set; }
        public string HeaderContent { get; set; }
        public string FooterCotent { get; set; }
        public string HtmlAttrinutes { get; set; }
        public string Hyperlink { get; set; }
        public string Channels { get; set; }
        public List<NavBlockModel> NavBlocks { get; set; }
    }
    [Serializable]
    public class NavBlockModel
    {
        public string ItemIds { get; set; }
        public string BoxTitle { get; set; }
        public int NavBlockType { get; set; }
        public int WidthPct { get; set; }
        public int DisplayOrder { get; set; }
        public int ColumnCount { get; set; }
        public bool ShowImages { get; set; }
        public bool ShowViewAllLink { get; set; }
        public string ContentBody { get; set; }
        public List<NavItemsModel> NavItems { get; set; }
    }
    [Serializable]
    public class NavItemsModel
    {
        public string Caption { get; set; }
        public string ItemType { get; set; }
        public string ItemId { get; set; }
        public string ParentItemId { get; set; }
        public string ItemLink { get; set; }
        public string ItemImageSrc { get; set; }
        public string AlternateText { get; set; }
        public string Target { get; set; }
        public string HighlightItem { get; set; }
        public string HighlightCssClass { get; set; }
        public string DisplayOrder { get; set; }
    }

}