using System;
using System.Collections.Generic;
using System.Linq;
using Maple2.AdminLTE.Bel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;

namespace Maple2.AdminLTE.Uil.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class RoleController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMemoryCache _cache;

        

        public RoleController(IHostingEnvironment hostingEnvironment,
                              RoleManager<ApplicationRole> roleManager,
                              IMemoryCache memoryCache)
        {
            _hostingEnvironment = hostingEnvironment;
            _roleManager = roleManager;
            _cache = memoryCache;
        }

        // GET: Administrator/Role
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View());
        }

        public async Task<IActionResult> GetApplicationRoles()
        {
            try
            {

                if (_cache.TryGetValue("CACHE_ADMINISTRATOR_APPROLE", out List<ApplicationRole> c_lstAppRole))
                {
                    return Json(new { data = c_lstAppRole });
                }

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300),
                    SlidingExpiration = TimeSpan.FromSeconds(60),
                    Priority = CacheItemPriority.NeverRemove
                };


                var lstAppRole = await _roleManager.Roles.ToListAsync();

                _cache.Set("CACHE_ADMINISTRATOR_APPROLE", lstAppRole, options);


                return Json(new { data = lstAppRole });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // GET: Administrator/Role/Create
        public async Task<IActionResult> Create()
        {
            return await Task.Run(() => View());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] ApplicationRole role)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    try
                    {
                        var result = await _roleManager.CreateAsync(role);

                        _cache.Remove("CACHE_ADMINISTRATOR_APPROLE");

                        return Json(new { success = result.Succeeded, data = role, message = "Role Created." });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { sussess = false, data = role, message = ex.Message });
                    }

                }

                var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
                return Json(new { success = false, errors = err, data = role, message = "Create Faield" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // GET: Administrator/Role/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {

                if (_cache.TryGetValue("CACHE_ADMINISTRATOR_APPROLE", out List<ApplicationRole> c_lstAppRole))
                {
                    var c_role = c_lstAppRole.Find(m => m.Id == id);

                    if (c_role == null)
                    {
                        return NotFound();
                    }

                    return PartialView(c_role);
                }

                var role = await _roleManager.FindByIdAsync(id);

                if (role == null)
                {
                    return NotFound();
                }

                return PartialView(role);

                
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // GET: Administrator/Role/Edit/xxxxx
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {

                if (_cache.TryGetValue("CACHE_ADMINISTRATOR_APPROLE", out List<ApplicationRole> c_lstAppRole))
                {
                    var c_role = c_lstAppRole.Find(m => m.Id == id);

                    if (c_role == null)
                    {
                        return NotFound();
                    }

                    return PartialView(c_role);
                }

                var role = await _roleManager.FindByIdAsync(id);

                if (role == null)
                {
                    return NotFound();
                }

                return PartialView(role);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Name")] ApplicationRole role)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    try
                    {
                        var roleExists = await _roleManager.FindByIdAsync(role.Id);

                        if (roleExists == null)
                        {
                            return BadRequest(new { success = false, message = "Role Not Exist!" });
                        }

                        roleExists.Name = role.Name;


                        var result = await _roleManager.UpdateAsync(roleExists);

                        _cache.Remove("CACHE_ADMINISTRATOR_APPROLE"); 

                        return Json(new { success = result.Succeeded, data = role, message = (result.Succeeded ? "Role Updated." : "Role Error Update!") });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { sussess = false, data = role, message = ex.Message });
                    }

                }

                var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
                return Json(new { success = false, errors = err, data = role, message = "Update Faield" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }

        }

        // POST: Administrator/User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            IdentityResult result;

            try
            {
                if (_cache.TryGetValue("CACHE_ADMINISTRATOR_APPROLE", out List<ApplicationRole> c_lstAppRole))
                {
                    var c_role = c_lstAppRole.Find(m => m.Id == id);

                    if (c_role == null)
                    {
                        return NotFound();
                    }

                    result = await _roleManager.DeleteAsync(c_role);

                    _cache.Remove("CACHE_ADMINISTRATOR_USER");

                    return Json(new { success = result.Succeeded, data = c_role, message = "Role Deleted." });

                }


                var role = await _roleManager.FindByIdAsync(id);

                if (role == null)
                {
                    return NotFound();
                }

                result = await _roleManager.DeleteAsync(role);

                return Json(new { success = result.Succeeded, data = role, message = "Role Deleted." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}