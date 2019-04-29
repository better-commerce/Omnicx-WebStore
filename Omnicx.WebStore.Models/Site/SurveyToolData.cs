using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.WebStore.Models.Site
{
   public  class SurveyToolData
    {
        public string FieldName { get; set; }
        public string FieldCode { get; set; }
        public string GroupName { get; set; }
        public List<KeyValuePair<string, string>> Options { get; set; }
    }
}
