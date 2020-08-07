using Maple2.AdminLTE.Bel;
using Maple2.AdminLTE.Bll;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maple2.AdminLTE.Uil.Services.Profile
{
    public class ProfileManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        IHttpContextAccessor _httpContextAccessor;

        private ApplicationUser _currentUser;
        private M_User _currentAuthenUser;

        public ProfileManager(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public ApplicationUser CurrentUser
        {
            get
            {
                if (_currentUser == null)
                    _currentUser = _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User).Result;

                return _currentUser;
            }
        }

        public bool IsHasPassword(ApplicationUser user)
        {
            return _userManager.HasPasswordAsync(user).Result;
        }

        public bool IsEmailConfirmed(ApplicationUser user)
        {
            return _userManager.IsEmailConfirmedAsync(user).Result;
        }

        public M_User CurrentAuthenUser
        {
            get
            {
                if (_currentAuthenUser == null)
                {
                    using (var userBll = new UserBLL())
                    {
                        var lstUser = userBll.GetUser(CurrentUser.UserId).Result;
                        _currentAuthenUser = lstUser.FirstOrDefault();
                    }
                }

                return _currentAuthenUser;
            }

        }
    }
}
