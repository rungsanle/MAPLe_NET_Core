using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maple2.AdminLTE.Bel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using Maple2.AdminLTE.Uil.Areas.Administrator.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Maple2.AdminLTE.Uil.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [RequestFormLimits(ValueCountLimit = int.MaxValue)]
    public class AppUserController : Controller
    {

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMemoryCache _cache;

        public AppUserController(IHostingEnvironment hostingEnvironment,
                                 UserManager<ApplicationUser> userManager,
                                 RoleManager<ApplicationRole> roleManager,
                                 IMemoryCache memoryCache)
        {
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _roleManager = roleManager;
            _cache = memoryCache;
        }

        // GET: Administrator/AppUser
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View());
        }

        public async Task<IActionResult> GetApplicationUser()
        {
            try
            {

                if (_cache.TryGetValue("CACHE_ADMINISTRATOR_APPUSER", out List<ApplicationUser> c_lstAppUser))
                {
                    return Json(new { data = c_lstAppUser });
                }

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300),
                    SlidingExpiration = TimeSpan.FromSeconds(60),
                    Priority = CacheItemPriority.NeverRemove
                };


                var lstAppUser = await _userManager.Users.ToListAsync();

                _cache.Set("CACHE_ADMINISTRATOR_APPUSER", lstAppUser, options);


                return Json(new { data = lstAppUser });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // GET: Administrator/AppUser/Register
        public async Task<IActionResult> Register()
        {
            return await Task.Run(() => View());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register([Bind("NameIdentifier, Email, Password, ConfirmPassword, IsAgree")] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var appUser = new ApplicationUser { UserName = model.NameIdentifier, Email = model.Email };
                    var result = await _userManager.CreateAsync(appUser, model.Password);

                    if (!result.Succeeded)
                    {
                        var errResult = result.Errors.Select(er => string.Format("{0}|{1}", er.Description.Substring(0, er.Description.IndexOf(" ")).Replace("Passwords", "Password"), er.Description));
                        //password does not meet standards
                        return Json(new { success = false, errors = errResult, data = model, message = "Update App. User Faield" });
                    }

                    return Json(new { success = result.Succeeded, data = model, message = "Register Success." });


                }
                catch (Exception ex)
                {
                    return Json(new { sussess = false, data = model, message = ex.Message });
                }
            }

            var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            return Json(new { success = false, errors = err, data = model, message = "Register Faield" });
        }

        // GET: Administrator/AppUser/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {

                //if (_cache.TryGetValue("CACHE_ADMINISTRATOR_APPUSER", out List<ApplicationUser> c_lstAppUser))
                //{
                //    var c_appUser = c_lstAppUser.Find(m => m.Id == id);

                //    if (c_appUser == null)
                //    {
                //        return NotFound();
                //    }

                //    return PartialView(c_appUser);
                //}

                var appUser = await _userManager.FindByIdAsync(id);
                if (appUser == null)
                {
                    return NotFound();
                }

                //Get roles for user
                var lstRole = await _userManager.GetRolesAsync(appUser);

                var appu = new AppUserViewModel()
                {
                    Id = appUser.Id,
                    UserName = appUser.UserName,
                    Email = appUser.Email,
                    Password = string.Empty,
                    PhoneNumber = appUser.PhoneNumber,
                    Roles = (List<string>)lstRole
                };

                

                return PartialView(appu);

            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // GET: Administrator/AppUser/Edit/5
        public async Task<IActionResult> Edit(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }

            try
            {
                var appUser = await _userManager.FindByIdAsync(Id);
                if (appUser == null)
                {
                    return NotFound();
                }

                //Get roles for user
                var lstRole = await _userManager.GetRolesAsync(appUser);

                var appu = new AppUserViewModel()
                {
                    Id = appUser.Id,
                    UserName = appUser.UserName,
                    Email = appUser.Email,
                    Password = string.Empty,
                    PhoneNumber = appUser.PhoneNumber,
                    Roles = (List<string>)lstRole
                };

                ViewBag.AddRole = new SelectList(await RolesUserIsNotIn(appUser.Id));

                return PartialView(appu);

            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAppUser([Bind("Id, UserName, Email, Password, PhoneNumber")] AppUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var appUser = await _userManager.FindByIdAsync(model.Id);
                    if (appUser == null)
                    {
                        return NotFound();
                    }

                    appUser.UserName = model.UserName;
                    appUser.Email = model.Email;
                    appUser.PhoneNumber = model.PhoneNumber;

                    // Lets check if the account needs to be unlocked
                    if (await _userManager.IsLockedOutAsync(appUser))
                    {
                        // Unlock user
                        await _userManager.ResetAccessFailedCountAsync(appUser);
                    }

                    var result = await _userManager.UpdateAsync(appUser);

                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        //user.PasswordHash = null;
                        //await UserManager.UpdateAsync(user);
                        var code = await _userManager.GeneratePasswordResetTokenAsync(appUser);
                        var result2 = await _userManager.ResetPasswordAsync(appUser, code, model.Password);

                        if (!result2.Succeeded)
                        {
                            var errResult = result2.Errors.Select(x => string.Format("Password|{0}", x));
                            //password does not meet standards
                            return Json(new { success = false, errors = errResult, data = model, message = "Update App. User Faield" });
                        }
                    }

                    return Json(new { success = result.Succeeded, data = model, message = "Update App. User Success." });
                }
                catch (Exception ex)
                {
                    return Json(new { sussess = false, data = model, message = ex.Message });
                }
            }

            var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            return Json(new { success = false, errors = err, data = model, message = "Update App. User Faield" });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAppUser(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }

            var appUser = await _userManager.FindByIdAsync(Id);
            if (appUser == null)
            {
                return NotFound();
            }

            try
            {
                await _userManager.RemoveFromRolesAsync(appUser, await _userManager.GetRolesAsync(appUser));
                await _userManager.UpdateAsync(appUser);
                await _userManager.DeleteAsync(appUser);

                return Json(new { success = true, data = appUser, message = "App User Deleted." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = appUser, message = ex.Message });
            }

        }

        private async Task<List<string>> RolesUserIsNotIn(string Id)
        {
            // Get roles the user is not in
            var colAllRoles = _roleManager.Roles.Select(x => x.Name).ToList();

            // Go get the roles for an individual
            var appUser = await _userManager.FindByIdAsync(Id);

            // If we could not find the user, throw an exception
            if (appUser == null)
            {
                throw new Exception("Could not find the User");
            }

            var colRolesForUser = await _userManager.GetRolesAsync(appUser);
            var colRolesUserInNotIn = (from objRole in colAllRoles
                                       where !colRolesForUser.Contains(objRole)
                                       select objRole).ToList();

            if (colRolesUserInNotIn.Count() == 0)
            {
                colRolesUserInNotIn.Add("No Roles Found");
            }

            return colRolesUserInNotIn;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAppUserRole(string Id, string RoleName)
        {
            if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(RoleName))
            {
                return BadRequest(new { success = false, message = "Value is Empty!" });
            }

            try
            {
                if (RoleName != "No Roles Found")
                {
                    var appUser = await _userManager.FindByIdAsync(Id);

                    // Put user in role
                    var result = await _userManager.AddToRoleAsync(appUser, RoleName);

                    var roles = new SelectList(await RolesUserIsNotIn(appUser.Id));


                    return Json(new { success = result.Succeeded, data = roles, message = "Role Updated." });
                }
                else
                {
                    return Json(new { success = false, data = RoleName, message = "Role Not Found" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = RoleName, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAppUserRole(string Id, string RoleName)
        {
            if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(RoleName))
            {
                return BadRequest(new { success = false, message = "Value is Empty!" });
            }

            try
            {
                var appUser = await _userManager.FindByIdAsync(Id);

                var resultRem = await _userManager.RemoveFromRoleAsync(appUser, RoleName);
                var resultUpd = await _userManager.UpdateAsync(appUser);

                var roles = new SelectList(await RolesUserIsNotIn(Id));

                return Json(new { success = resultRem.Succeeded, data = roles, message = "Role Updated." });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = RoleName, message = ex.Message });
            }
        }
    }
}