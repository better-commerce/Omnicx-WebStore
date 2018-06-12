using System;
using Omnicx.API.SDK.Entities;

namespace Omnicx.API.SDK.Models.Helpers
{
    [Serializable]
    public class Facet
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }

        public FacetType Type { get; set; }
    }
}