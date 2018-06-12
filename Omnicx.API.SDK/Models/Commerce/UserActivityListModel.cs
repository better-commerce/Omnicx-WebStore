using Omnicx.API.SDK.Models.Common;
using System.Collections.Generic;

namespace Omnicx.API.SDK.Models.Commerce
{
    public class UserActivityListModel: BaseModel
    {
        public List<UserActivityModel> Sessions { get; set; }
        public int TotalActivity { get; set; }
    }
}
