namespace Omnicx.API.SDK.Helpers
{
    public interface IHeadTagBuilder
    {
        string GenerateTitle(bool addDefaultTitle);
        string GenerateMetaTitle();
        string GenerateMetaDescription();
        string GenerateMetaKeywords();

        
        void AddCanonicalUrlParts(string part);
        string GenerateCanonicalUrls();
       
        void AddTitleParts(string part);
        
        void AddMetaDescriptionParts(string part);
        void AddMetaKeywordsParts(string part);
        void AddMetaTitleParts(string part);

        string GenerateDataLayer();
        string GetOmnilyticUrl();
        string GetOmnilyticId();
        void AddDataLayer(string key,object value, bool overWriteifExists=false);
        void AppendTitleParts(string part);
        void AppendMetaDescriptionParts(string part);
        void AppendMetaKeywordParts(string part);
    }
}
