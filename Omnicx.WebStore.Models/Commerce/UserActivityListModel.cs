using Omnicx.WebStore.Models.Common;
using System.Collections.Generic;

namespace Omnicx.WebStore.Models.Commerce
{
    public class UserActivityListModel: BaseModel
    {
        public List<UserActivityModel> Sessions { get; set; }
        public int TotalActivity { get; set; }
    }
}
