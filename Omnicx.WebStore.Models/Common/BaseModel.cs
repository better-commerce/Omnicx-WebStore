namespace Omnicx.WebStore.Models.Common
{
    public class BaseModel
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalRecord { get; set; }
        public bool isB2BEnable { get; set; }
        public bool AllowReorder { get; set; }
    }
}
