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
    public class ProcessController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMemoryCache _cache;

        public ProcessController(IHostingEnvironment hostingEnvironment,
                                 IMemoryCache memoryCache,
                                 UserManager<ApplicationUser> userManager) : base(userManager, memoryCache)
        {
            _hostingEnvironment = hostingEnvironment;
            _cache = memoryCache;
        }

        // GET: Master/Process
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View());
        }

        public async Task<IActionResult> GetProcess()
        {
            try
            {
                if (_cache.TryGetValue("CACHE_MASTER_PROCESS", out List<M_Process> c_lstProc))
                {
                    return Json(new { data = c_lstProc });
                }

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300),
                    SlidingExpiration = TimeSpan.FromSeconds(60),
                    Priority = CacheItemPriority.NeverRemove
                };

                using (var processBll = new ProcessBLL())
                {
                    var lstProc = await processBll.GetProcess(null);

                    _cache.Set("CACHE_MASTER_PROCESS", lstProc, options);

                    return Json(new { data = lstProc });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }

            //using (var processBll = new ProcessBLL())
            //{
            //    return Json(new { data = await processBll.GetProcess(null) });
            //}
        }

        // GET: Master/Process/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {

                if (_cache.TryGetValue("CACHE_MASTER_PROCESS", out List<M_Process> c_lstProc))
                {
                    var m_Process = c_lstProc.Find(p => p.Id == id);

                    if (m_Process == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Process);
                }

                using (var processBll = new ProcessBLL())
                {
                    var lstProcess = await processBll.GetProcess(id);
                    var m_Process = lstProcess.First();

                    if (m_Process == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Process);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // GET: Master/Process/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.CompCode = await base.CurrentUserComp();
            return await Task.Run(() => View());
        }

        // POST: Master/Process/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProcessCode,ProcessName,ProcessDesc,ProcessSeq,CompanyCode,Id,Is_Active,Created_Date,Created_By,Updated_Date,Updated_By")] M_Process m_Process)
        {
            if (ModelState.IsValid)
            {
                m_Process.Created_By = await base.CurrentUserId();

                ResultObject resultObj;

                try
                {
                    using (var processBll = new ProcessBLL())
                    {
                        resultObj = await processBll.InsertProcess(m_Process);

                        _cache.Remove("CACHE_MASTER_PROCESS");
                    }

                    return Json(new { success = true, data = (M_Process)resultObj.ObjectValue, message = "Process Created." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, data = m_Process, message = ex.Message });
                }
            }

            var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            return Json(new { success = false, errors = err, data = m_Process, message = "Created Faield" });
        }

        // GET: Master/Process/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.CompCode = await base.CurrentUserComp();

            try
            {

                if (_cache.TryGetValue("CACHE_MASTER_PROCESS", out List<M_Process> c_lstProc))
                {
                    var m_Process = c_lstProc.Find(p => p.Id == id);

                    if (m_Process == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Process);
                }

                using (var processBll = new ProcessBLL())
                {
                    var lstProcess = await processBll.GetProcess(id);

                    var m_Process = lstProcess.First();

                    if (m_Process == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Process);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }

        }

        // POST: Master/Process/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ProcessCode,ProcessName,ProcessDesc,ProcessSeq,CompanyCode,Id,Is_Active,Created_Date,Created_By,Updated_Date,Updated_By")] M_Process m_Process)
        {
            if (ModelState.IsValid)
            {
                m_Process.Updated_By = await base.CurrentUserId();

                ResultObject resultObj;

                try
                {
                    using (var processBll = new ProcessBLL())
                    {
                        resultObj = await processBll.UpdateProcess(m_Process);

                        _cache.Remove("CACHE_MASTER_PROCESS");
                    }

                    return Json(new { success = true, data = (M_Process)resultObj.ObjectValue, message = "Process Update." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, data = m_Process, message = ex.Message });
                }
            }

            var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            return Json(new { success = false, errors = err, data = m_Process, message = "Update Failed" });
            
        }
        
        // POST: Master/Process/Delete/5
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
                if (_cache.TryGetValue("CACHE_MASTER_PROCESS", out List<M_Process> c_lstProc))
                {
                    var m_Process = c_lstProc.Find(p => p.Id == id);

                    if (m_Process == null)
                    {
                        return NotFound();
                    }

                    m_Process.Updated_By = await base.CurrentUserId();

                    using (var processBll = new ProcessBLL())
                    {
                        resultObj = await processBll.DeleteProcess(m_Process);

                        _cache.Remove("CACHE_MASTER_PROCESS");
                    }

                    return Json(new { success = true, data = (M_Process)resultObj.ObjectValue, message = "Process Deleted." });
                }

                using (var processBll = new ProcessBLL())
                {
                    var lstProcess = await processBll.GetProcess(id);

                    var m_Process = lstProcess.First();

                    if (m_Process == null)
                    {
                        return NotFound();
                    }

                    m_Process.Updated_By = await base.CurrentUserId();

                    resultObj = await processBll.DeleteProcess(m_Process);

                    _cache.Remove("CACHE_MASTER_PROCESS");
                }

                return Json(new { success = true, data = (M_Process)resultObj.ObjectValue, message = "Process Deleted." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
