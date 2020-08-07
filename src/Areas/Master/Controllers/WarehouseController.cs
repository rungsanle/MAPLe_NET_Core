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
    public class WarehouseController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMemoryCache _cache;

        public WarehouseController(IHostingEnvironment hostingEnvironment,
                                   IMemoryCache memoryCache,
                                   UserManager<ApplicationUser> userManager) : base(userManager, hostingEnvironment, memoryCache)
        {
            _hostingEnvironment = hostingEnvironment;
            _cache = memoryCache;
        }

        // GET: Master/Warehouse
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View());
        }

        public async Task<IActionResult> GetWarehouse()
        {
            try
            {
                if (_cache.TryGetValue("CACHE_MASTER_WAREHOUSE", out List<M_Warehouse> c_lstWh))
                {
                    return Json(new { data = c_lstWh });
                }

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300),
                    SlidingExpiration = TimeSpan.FromSeconds(60),
                    Priority = CacheItemPriority.NeverRemove
                };

                using (var whBll = new WarehouseBLL())
                {
                    var lstWh = await whBll.GetWarehouse(null);

                    _cache.Set("CACHE_MASTER_WAREHOUSE", lstWh, options);

                    return Json(new { data = lstWh });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }

            //Warehouse DbContext
            //using (var whBll = new WarehouseBLL())
            //{
            //    return Json(new { data = await whBll.GetWarehouse(null) });
            //}
        }

        // GET: Master/Warehouse/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                if (_cache.TryGetValue("CACHE_MASTER_WAREHOUSE", out List<M_Warehouse> c_lstWh))
                {
                    var m_Warehouse = c_lstWh.Find(w => w.Id == id);

                    if (m_Warehouse == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Warehouse);
                }

                using (var whBll = new WarehouseBLL())
                {
                    var lstWh = await whBll.GetWarehouse(id);
                    var m_Warehouse = lstWh.First();

                    if (m_Warehouse == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Warehouse);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // GET: Master/Warehouse/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.CompCode = await base.CurrentUserComp();
            return await Task.Run(() => View());
        }

        // POST: Master/Warehouse/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WarehouseCode,WarehouseName,WarehouseDesc,CompanyCode,Id,Is_Active,Created_Date,Created_By,Updated_Date,Updated_By")] M_Warehouse m_Warehouse)
        {
            if (ModelState.IsValid)
            {
                m_Warehouse.Created_By = await base.CurrentUserId();

                ResultObject resultObj;

                try
                {
                    using (var whBll = new WarehouseBLL())
                    {
                        resultObj = await whBll.InsertWarehouse(m_Warehouse);

                        _cache.Remove("CACHE_MASTER_WAREHOUSE");
                    }

                    return Json(new { success = true, data = (M_Warehouse)resultObj.ObjectValue, message = "Warehouse Created." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, data = m_Warehouse, message = ex.Message });
                }
            }

            var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            return Json(new { success = false, errors = err, data = m_Warehouse, message = "Created Faield" });
            
        }

        // GET: Master/Warehouse/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.CompCode = base.CurrentUserComp();

            try
            {

                if (_cache.TryGetValue("CACHE_MASTER_WAREHOUSE", out List<M_Warehouse> c_lstWh))
                {
                    var m_Warehouse = c_lstWh.Find(w => w.Id == id);

                    if (m_Warehouse == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Warehouse);
                }

                using (var whBll = new WarehouseBLL())
                {
                    var lstWh = await whBll.GetWarehouse(id);

                    var m_Warehouse = lstWh.First();

                    if (m_Warehouse == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Warehouse);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }

        }

        // POST: Master/Warehouse/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("WarehouseCode,WarehouseName,WarehouseDesc,CompanyCode,Id,Is_Active,Created_Date,Created_By,Updated_Date,Updated_By")] M_Warehouse m_Warehouse)
        {
            if (ModelState.IsValid)
            {
                m_Warehouse.Updated_By = await base.CurrentUserId();

                ResultObject resultObj;

                try
                {
                    using (var whBll = new WarehouseBLL())
                    {
                        resultObj = await whBll.UpdateWarehouse(m_Warehouse);

                        _cache.Remove("CACHE_MASTER_WAREHOUSE");
                    }

                    return Json(new { success = true, data = (M_Warehouse)resultObj.ObjectValue, message = "Warehouse Update." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, data = m_Warehouse, message = ex.Message });
                }
            }

            var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            return Json(new { success = false, errors = err, data = m_Warehouse, message = "Update Failed" });
            
        }

        // POST: Master/Warehouse/Delete/5
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
                if (_cache.TryGetValue("CACHE_MASTER_WAREHOUSE", out List<M_Warehouse> c_lstWh))
                {
                    var m_Warehouse = c_lstWh.Find(w => w.Id == id);

                    if (m_Warehouse == null)
                    {
                        return NotFound();
                    }

                    m_Warehouse.Updated_By = await base.CurrentUserId();

                    using (var whBll = new WarehouseBLL())
                    {
                        resultObj = await whBll.DeleteWarehouse(m_Warehouse);

                        _cache.Remove("CACHE_MASTER_WAREHOUSE");
                    }

                    return Json(new { success = true, data = (M_Warehouse)resultObj.ObjectValue, message = "Warehouse Deleted." });
                }

                using (var whBll = new WarehouseBLL())
                {
                    var lstWh = await whBll.GetWarehouse(id);

                    var m_Warehouse = lstWh.First();

                    if (m_Warehouse == null)
                    {
                        return NotFound();
                    }

                    m_Warehouse.Updated_By = await base.CurrentUserId();

                    resultObj = await whBll.DeleteWarehouse(m_Warehouse);

                    _cache.Remove("CACHE_MASTER_WAREHOUSE");
                }

                return Json(new { success = true, data = (M_Warehouse)resultObj.ObjectValue, message = "Warehouse Deleted." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        
    }
}
