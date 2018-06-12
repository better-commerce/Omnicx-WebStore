using System.Collections.Generic;

namespace Omnicx.API.SDK.Models.Site
{
    public class ZoneModel
    {
        public string Code { get; set; }

        public int DisplayOrder { get; set; }
        public List<WidgetModel> Widgets { get; set; }

     
    }
}