using Omnicx.WebStore.Models.Enums;
using System;

namespace Omnicx.WebStore.Models.Site
{
    [Serializable]
    public class SnippetModel
    {
        public string Name { get; set; }
        public SnippetContentTypes Type { get; set; }
        public SnippetPlacements Placement { get; set; }
        public string Content { get; set; }       
    }
}