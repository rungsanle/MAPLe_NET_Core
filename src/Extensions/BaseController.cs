using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Maple2.AdminLTE.Bel;
using Maple2.AdminLTE.Bll;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace Maple2.AdminLTE.Uil.Extensions
{
    public abstract partial class BaseController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMemoryCache _cache;

        private M_User CurrentUser { get; set; }

        public BaseController(UserManager<ApplicationUser> userManager,
                              IHostingEnvironment hostingEnvironment,
                              IMemoryCache memoryCache)
        {
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
            _cache = memoryCache;
        }

        
        public string JsReportFileRefPath
        { 
            get
            {
                return string.Format("file://{0}\\", _hostingEnvironment.WebRootPath);
            }
        }

        public async Task<int?> CurrentUserId()
        {
            if (_cache.TryGetValue("CACHE_BASECURRENT_USER", out M_User c_user))
            {
                return c_user.Id;
            }

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300),
                SlidingExpiration = TimeSpan.FromSeconds(60),
                Priority = CacheItemPriority.Low
            };

            M_User user;
            ClaimsPrincipal curUser = this.User;
            var appUser = await _userManager.GetUserAsync(curUser);

            using (var userBll = new UserBLL())
            {
                var lstUser = await userBll.GetUser(appUser.UserId);
                user = lstUser.First();
            }

            if (user != null)
            {
                _cache.Set("CACHE_BASECURRENT_USER", user, options);
                return user.Id;
            }
            else
            {
                _cache.Remove("CACHE_BASECURRENT_USER");
                return null;
            }

        }

        public async Task<string> CurrentUserComp()
        {
            if (_cache.TryGetValue("CACHE_BASECURRENT_USER", out M_User c_user))
            {
                return c_user.CompanyCode;
            }

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300),
                SlidingExpiration = TimeSpan.FromSeconds(60),
                Priority = CacheItemPriority.Low
            };

            M_User user;
            ClaimsPrincipal curUser = this.User;
            var appUser = await _userManager.GetUserAsync(curUser);
            using (var userBll = new UserBLL())
            {
                var lstUser = await userBll.GetUser(appUser.UserId);
                user = lstUser.First();
            }

            if (user != null)
            {
                _cache.Set("CACHE_BASECURRENT_USER", user, options);
                return user.CompanyCode;
            }
            else
            {
                _cache.Remove("CACHE_BASECURRENT_USER");
                return string.Empty;
            }

        }

    }
}