using System;

namespace Omnicx.API.SDK.Models.Helpers
{
    [Serializable]
    public class SearchFilter
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public bool IsSelected { get; set; }
    }
}