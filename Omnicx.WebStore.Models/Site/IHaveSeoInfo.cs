namespace Omnicx.WebStore.Models.Site
{
    public interface IHaveSeoInfo
    {

        string MetaTitle { get; set; }
        string MetaDescription { get; set; }

        string MetaKeywords { get; set; }
        string CanonicalTags { get; set; }
    }

}
