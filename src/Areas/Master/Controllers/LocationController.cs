using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Maple2.AdminLTE.Bel;
using Maple2.AdminLTE.Dal;
using Microsoft.AspNetCore.Hosting;
using Maple2.AdminLTE.Bll;
using Microsoft.Extensions.Caching.Memory;
using Maple2.AdminLTE.Uil.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Maple2.AdminLTE.Uil.Areas.Master.Controllers
{
    [Area("Master")]
    public class LocationController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMemoryCache _cache;

        public LocationController(IHostingEnvironment hostingEnvironment,
                                  IMemoryCache memoryCache,
                                  UserManager<ApplicationUser> userManager) : base(userManager, memoryCache)
        {
            _hostingEnvironment = hostingEnvironment;
            _cache = memoryCache;
        }

        // GET: Master/Location
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View());
        }

        public async Task<IActionResult> GetLocation()
        {
            try
            {
                if (_cache.TryGetValue("CACHE_MASTER_LOCATION", out List<M_Location> c_lstLoc))
                {
                    return Json(new { data = c_lstLoc });
                }

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300),
                    SlidingExpiration = TimeSpan.FromSeconds(60),
                    Priority = CacheItemPriority.NeverRemove
                };

                using (var locationBll = new LocationBLL())
                {
                    var lstLoc = await locationBll.GetLocation(null);

                    _cache.Set("CACHE_MASTER_LOCATION", lstLoc, options);

                    return Json(new { data = lstLoc });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }

            //using (var locationBll = new LocationBLL())
            //{
            //    return Json(new { data = await locationBll.GetLocation(null) });
            //}
        }

        // GET: Master/Location/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {

                if (_cache.TryGetValue("CACHE_MASTER_LOCATION", out List<M_Location> c_lstLoc))
                {
                    var m_Location = c_lstLoc.Find(l => l.Id == id);

                    if (m_Location == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Location);
                }

                using (var locationBll = new LocationBLL())
                {
                    var lstLoc = await locationBll.GetLocation(id);
                    var m_Location = lstLoc.First();

                    if (m_Location == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Location);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // GET: Master/Location/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.CompCode = await base.CurrentUserComp();
            return await Task.Run(() => View());
        }

        // POST: Master/Location/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LocationCode,LocationName,LocationDesc,WarehouseId,CompanyCode,Id,Is_Active,Created_Date,Created_By,Updated_Date,Updated_By")] M_Location m_Location)
        {
            if (ModelState.IsValid)
            {
                m_Location.Created_By = await base.CurrentUserId();

                ResultObject resultObj;

                try
                {
                    using (var locationBll = new LocationBLL())
                    {
                        resultObj = await locationBll.InsertLocation(m_Location);

                        _cache.Remove("CACHE_MASTER_LOCATION");
                    }

                    return Json(new { success = true, data = (M_Location)resultObj.ObjectValue, message = "Location Created." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, data = m_Location, message = ex.Message });
                }
            }

            var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            return Json(new { success = false, errors = err, data = m_Location, message = "Created Faield" });
        }

        // GET: Master/Location/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.CompCode = "ALL*";

            try
            {

                if (_cache.TryGetValue("CACHE_MASTER_LOCATION", out List<M_Location> c_lstLoc))
                {
                    var m_Location = c_lstLoc.Find(l => l.Id == id);

                    if (m_Location == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Location);
                }

                using (var locationBll = new LocationBLL())
                {
                    var lstLoc = await locationBll.GetLocation(id);

                    var m_Location = lstLoc.First();

                    if (m_Location == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Location);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // POST: Master/Location/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("LocationCode,LocationName,LocationDesc,WarehouseId,CompanyCode,Id,Is_Active,Created_Date,Created_By,Updated_Date,Updated_By")] M_Location m_Location)
        {
            if (ModelState.IsValid)
            {
                m_Location.Updated_By = await base.CurrentUserId();

                ResultObject resultObj;

                try
                {
                    using (var locationBll = new LocationBLL())
                    {
                        resultObj = await locationBll.UpdateLocation(m_Location);

                        _cache.Remove("CACHE_MASTER_LOCATION");
                    }

                    return Json(new { success = true, data = (M_Location)resultObj.ObjectValue, message = "Location Update." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, data = m_Location, message = ex.Message });
                }
            }

            var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            return Json(new { success = false, errors = err, data = m_Location, message = "Update Failed" });
            
        }

        // POST: Master/Location/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ResultObject resultObj;

            try
            {
                if (_cache.TryGetValue("CACHE_MASTER_LOCATION", out List<M_Location> c_lstLoc))
                {
                    var m_Location = c_lstLoc.Find(l => l.Id == id);

                    if (m_Location == null)
                    {
                        return NotFound();
                    }

                    m_Location.Updated_By = await base.CurrentUserId();

                    using (var locationBll = new LocationBLL())
                    {
                        resultObj = await locationBll.DeleteLocation(m_Location);

                        _cache.Remove("CACHE_MASTER_LOCATION");
                    }

                    return Json(new { success = true, data = (M_Location)resultObj.ObjectValue, message = "Location Deleted." });
                }

                using (var locationBll = new LocationBLL())
                {
                    var lstLoc = await locationBll.GetLocation(id);

                    var m_Location = lstLoc.First();

                    if (m_Location == null)
                    {
                        return NotFound();
                    }

                    m_Location.Updated_By = await base.CurrentUserId();

                    resultObj = await locationBll.DeleteLocation(m_Location);

                    _cache.Remove("CACHE_MASTER_LOCATION");
                }

                return Json(new { success = true, data = (M_Location)resultObj.ObjectValue, message = "Location Deleted." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
