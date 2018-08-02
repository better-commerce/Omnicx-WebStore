using System.Collections.Generic;

namespace Omnicx.WebStore.Models.Site
{
    public class MenuModel
    {
        public string Id { get; set; }

        public string ParentMenuId { get; set; }

        public string MenuType { get; set; }

        public string Hyperlink { get; set; }

        public int DisplayOrder { get; set; }

        public string DisplayText { get; set; }

        public string AltText { get; set; }

        public List<MenuModel> ChildrenMenu { get; set; }
    }

    public class MenuListModel
    {
        public List<MenuModel> MenuList { get; set; }
    }
}
