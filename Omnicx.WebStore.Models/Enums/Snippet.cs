using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.WebStore.Models.Enums
{
    public enum SnippetContentTypes
    {
        Javascript = 1, Html = 2, Text = 3

    }
    public enum SnippetPlacements
    {

        Head,
        ErrorHead,
        BodyStartHtmlTagAfter,
        PageContainerAfter,
        HeaderMenuBefore,
        HeaderMenuAfter,
        FooterBefore,
        FooterAfter,
        BodyEndHtmlTagBefore,
        H1,
        ProductAndBrandDescription,
        LeftPanel,
        RightPanel,
        SiteWallpaper,
        SiteLogo,
        Child,
        OrderConfirmationAfterProgressBar,
        TopHead
    }
}
