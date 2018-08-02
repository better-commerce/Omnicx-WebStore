using System;
using System.Collections.Generic;
using Omnicx.WebStore.Models.Enums;
using Omnicx.WebStore.Models.Helpers;
using Omnicx.WebStore.Models.Keys;

namespace Omnicx.WebStore.Models.Site
{

    [Serializable]
    public class BlogGroupModel 
    {
        public int? Id { get; set; }
        public Guid? RecordId { get; set; }
        public BlogGroupType GroupType { get; set; }
        public string GroupName { get; set; }
        public int GroupCount { get; set; }
        public string Slug { get; set; }
        public string GroupImage { get; set; }
    }
    [Serializable]
    public class BlogGroups 
    {
        public BlogGroups()
        {
            Editors = new List<BlogGroupModel>();
            Categories = new List<BlogGroupModel>();
        }
        public List<BlogGroupModel> Editors{ get; set; }
        public List<BlogGroupModel> Categories { get; set; }
        public List<BlogGroupModel> BlogTypes { get; set; }
    }
    [Serializable]
    public class BlogModel : IHaveSeoInfo
    { 
        public Guid RecordId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Author { get; set; }
        public Guid AuthorId { get; set; }
        public string Category { get; set; }
        public string CategoryId { get; set; }
        public string Tag { get; set; }
        public string URL { get; set; }
        public bool IsPublish { get; set; }
        public string Abstract { get; set; }
        
        public string BlogImage { get; set; }

        public string EditorImage { get; set; }
        
        public string EditorBio { get; set; }
        public DateTime Created { get; set; }
        public int Days { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string CanonicalTags { get; set; }
        public string CategorySlug { get; set; }
        public string EditorSlug { get; set; }
    }
    [Serializable]
    public class BlogCategory 
    {
        public string CategoryName { get; set; }
        
    }
    [Serializable]
    public class BlogDetailViewModel 
    {
        public BlogModel Detail { get; set; }
        public List<BlogGroupModel> Editors { get; set; }
        public List<BlogGroupModel> Categories { get; set; }
        public PaginatedResult<BlogModel> BlogList { get; set; }
        public string FreeText { get; set; }
        public List<BlogGroupModel> BlogTypes { get; set; }
        public string Slug { get; set; }
        public int PageSize { get; set; }
    }
}