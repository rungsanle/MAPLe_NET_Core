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
using Microsoft.AspNetCore.Identity;
using Maple2.AdminLTE.Uil.Extensions;

namespace Maple2.AdminLTE.Uil.Areas.Master.Controllers
{
    [Area("Master")]
    public class ProductionTypeController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMemoryCache _cache;

        public ProductionTypeController(IHostingEnvironment hostingEnvironment,
                                        IMemoryCache memoryCache,
                                        UserManager<ApplicationUser> userManager) : base(userManager, hostingEnvironment, memoryCache)
        {
            _hostingEnvironment = hostingEnvironment;
            _cache = memoryCache;
        }

        // GET: Master/ProductionType
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View());
        }

        public async Task<IActionResult> GetProductionType()
        {
            try
            {
                if (_cache.TryGetValue("CACHE_MASTER_PRODUCTIONTYPE", out List<M_ProductionType> c_lstProdType))
                {
                    return Json(new { data = c_lstProdType });
                }

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300),
                    SlidingExpiration = TimeSpan.FromSeconds(60),
                    Priority = CacheItemPriority.NeverRemove
                };

                using (var prodTypeBll = new ProductionTypeBLL())
                {
                    var lstProdType = await prodTypeBll.GetProductionType(null);

                    _cache.Set("CACHE_MASTER_PRODUCTIONTYPE", lstProdType, options);

                    return Json(new { data = lstProdType });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }

            //using (var prodTypeBll = new ProductionTypeBLL())
            //{
            //    return Json(new { data = await prodTypeBll.GetProductionType(null) });
            //}
        }

        // GET: Master/ProductionType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {

                if (_cache.TryGetValue("CACHE_MASTER_PRODUCTIONTYPE", out List<M_ProductionType> c_lstProdType))
                {
                    var m_ProductionType = c_lstProdType.Find(p => p.Id == id);

                    if (m_ProductionType == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_ProductionType);
                }

                using (var prodTypeBll = new ProductionTypeBLL())
                {
                    var lstProdType = await prodTypeBll.GetProductionType(id);
                    var m_ProductionType = lstProdType.First();

                    if (m_ProductionType == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_ProductionType);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // GET: Master/ProductionType/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.CompCode = await base.CurrentUserComp();
            return await Task.Run(() => View());
        }

        // POST: Master/ProductionType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProdTypeCode,ProdTypeName,ProdTypeDesc,ProdTypeSeq,CompanyCode,Id,Is_Active,Created_Date,Created_By,Updated_Date,Updated_By")] M_ProductionType m_ProductionType)
        {
            if (ModelState.IsValid)
            {
                m_ProductionType.Created_By = await base.CurrentUserId();

                ResultObject resultObj;

                try
                {
                    using (var prodTypeBll = new ProductionTypeBLL())
                    {
                        resultObj = await prodTypeBll.InsertProductionType(m_ProductionType);

                        _cache.Remove("CACHE_MASTER_PRODUCTIONTYPE");
                    }

                    return Json(new { success = true, data = (M_ProductionType)resultObj.ObjectValue, message = "Production Type Created." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, data = m_ProductionType, message = ex.Message });
                }
            }

            var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            return Json(new { success = false, errors = err, data = m_ProductionType, message = "Created Faield" });
            
        }

        // GET: Master/ProductionType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.CompCode = await base.CurrentUserComp();

            try
            {

                if (_cache.TryGetValue("CACHE_MASTER_PRODUCTIONTYPE", out List<M_ProductionType> c_lstProdType))
                {
                    var m_ProductionType = c_lstProdType.Find(p => p.Id == id);

                    if (m_ProductionType == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_ProductionType);
                }

                using (var prodTypeBll = new ProductionTypeBLL())
                {
                    var lstProdType = await prodTypeBll.GetProductionType(id);
                    var m_ProductionType = lstProdType.First();

                    if (m_ProductionType == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_ProductionType);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // POST: Master/ProductionType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ProdTypeCode,ProdTypeName,ProdTypeDesc,ProdTypeSeq,CompanyCode,Id,Is_Active,Created_Date,Created_By,Updated_Date,Updated_By")] M_ProductionType m_ProductionType)
        {
            if (ModelState.IsValid)
            {
                m_ProductionType.Updated_By = await base.CurrentUserId();

                ResultObject resultObj;

                try
                {
                    using (var prodTypeBll = new ProductionTypeBLL())
                    {
                        resultObj = await prodTypeBll.UpdateProductionType(m_ProductionType);

                        _cache.Remove("CACHE_MASTER_PRODUCTIONTYPE");
                    }

                    return Json(new { success = true, data = (M_ProductionType)resultObj.ObjectValue, message = "Production Type Update." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, data = m_ProductionType, message = ex.Message });
                }
            }

            var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            return Json(new { success = false, errors = err, data = m_ProductionType, message = "Update Failed" });
            
        }

        // POST: Master/ProductionType/Delete/5
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
                if (_cache.TryGetValue("CACHE_MASTER_PRODUCTIONTYPE", out List<M_ProductionType> c_lstProdType))
                {
                    var m_ProductionType = c_lstProdType.Find(p => p.Id == id);

                    if (m_ProductionType == null)
                    {
                        return NotFound();
                    }

                    m_ProductionType.Updated_By = await base.CurrentUserId();

                    using (var prodTypeBll = new ProductionTypeBLL())
                    {
                        resultObj = await prodTypeBll.DeleteProductionType(m_ProductionType);

                        _cache.Remove("CACHE_MASTER_PRODUCTIONTYPE");
                    }

                    return Json(new { success = true, data = (M_ProductionType)resultObj.ObjectValue, message = "Production Type Deleted." });
                }

                using (var prodTypeBll = new ProductionTypeBLL())
                {
                    var lstProdType = await prodTypeBll.GetProductionType(id);
                    var m_ProductionType = lstProdType.First();

                    if (m_ProductionType == null)
                    {
                        return NotFound();
                    }

                    m_ProductionType.Updated_By = await base.CurrentUserId();await base.CurrentUserId();

                    resultObj = await prodTypeBll.DeleteProductionType(m_ProductionType);

                    _cache.Remove("CACHE_MASTER_PRODUCTIONTYPE");
                }

                return Json(new { success = true, data = (M_ProductionType)resultObj.ObjectValue, message = "Production Type Deleted." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }
    }
}
