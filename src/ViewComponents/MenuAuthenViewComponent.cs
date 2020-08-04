using Maple2.AdminLTE.Bel;
using Maple2.AdminLTE.Bll;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maple2.AdminLTE.Uil.ViewComponents
{
    public class MenuAuthenViewComponent : ViewComponent
    {
        private readonly IMemoryCache _cache;

        public MenuAuthenViewComponent(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? userId)
        {
            var items = await GetMenuAuthenAsync(userId);
            return View("~/Views/Shared/_NavBar.cshtml", items);
        }

        private async Task<List<M_Menu>> GetMenuAuthenAsync(int? Id)
        {
            try
            {
                if (_cache.TryGetValue("CACHE_MENU_AUTHEN", out Task<List<M_Menu>> c_lstMenu))
                {
                    return await c_lstMenu;
                }

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300),
                    SlidingExpiration = TimeSpan.FromSeconds(60),
                    Priority = CacheItemPriority.Low
                };

                using (var menuBll = new MenuBLL())
                {
                    var lstMenu = await menuBll.GetMenuAuthen(Id);

                    _cache.Set("CACHE_MENU_AUTHEN", lstMenu, options);

                    //return Json(new { data = lstMenu });
                    return lstMenu;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
