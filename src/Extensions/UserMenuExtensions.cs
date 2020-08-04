using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc
{
    public static class UserMenuExtensions
    {
        public static bool IsMenuActive(this IHtmlHelper htmlHelper, string menuItemUrl)
        {
            var viewContext = htmlHelper.ViewContext;
            var currentPageUrl = viewContext.ViewData["ActiveMenu"] as string ?? viewContext.HttpContext.Request.Path;


            var menuItems = menuItemUrl.Split('/');

            var curItems = currentPageUrl.Split('/');

            if (menuItems.Length == 2)
            {
                return (menuItems[1] == curItems[1]);
            }
            else
            {
                return (currentPageUrl == menuItemUrl);
            }

            
            //return currentPageUrl.StartsWith(menuItemUrl, StringComparison.OrdinalIgnoreCase);
        }
    }
}
