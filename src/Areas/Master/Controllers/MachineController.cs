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
using jsreport.AspNetCore;
using Maple2.AdminLTE.Uil.Areas.Master.Models;
using jsreport.Types;
using Maple2.AdminLTE.Uil.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Maple2.AdminLTE.Uil.Areas.Master.Controllers
{
    [Area("Master")]
    public class MachineController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _config;
        private readonly IJsReportMVCService _jsReport;

        


        public MachineController(IHostingEnvironment hostingEnvironment,
                                 IMemoryCache memoryCache,
                                 IConfiguration config,
                                 IJsReportMVCService jsReport,
                                 UserManager<ApplicationUser> userManager) : base(userManager, memoryCache)
        {
            _hostingEnvironment = hostingEnvironment;
            _cache = memoryCache;
            _config = config;
            _jsReport = jsReport;
        }

        // GET: Master/Machine
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View());
        }

        public async Task<IActionResult> GetMachine()
        {
            try
            {
                //var iResult = await base.GetCurUserIdAsync();

                if (_cache.TryGetValue("CACHE_MASTER_MACHINE", out List<M_Machine> c_lstMac))
                {
                    return Json(new { data = c_lstMac });
                }

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300),
                    SlidingExpiration = TimeSpan.FromSeconds(60),
                    Priority = CacheItemPriority.NeverRemove
                };

                using (var mcBll = new MachineBLL())
                {
                    var lstMac = await mcBll.GetMachine(null);

                    _cache.Set("CACHE_MASTER_MACHINE", lstMac, options);

                    return Json(new { data = lstMac });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }

            //using (var mcBll = new MachineBLL())
            //{
            //    return Json(new { data = await mcBll.GetMachine(null) });
            //}
        }

        public async Task<IActionResult> GetMachineByProdType(int? id)
        {
            try
            {
                using (var mcBll = new MachineBLL())
                {
                    return Json(new { data = await mcBll.GetMachineByProdType(id) });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // GET: Master/Machine/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {

                if (_cache.TryGetValue("CACHE_MASTER_MACHINE", out List<M_Machine> c_lstMac))
                {
                    var m_Machine = c_lstMac.Find(m => m.Id == id);

                    if (m_Machine == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Machine);
                }

                using (var mcBll = new MachineBLL())
                {
                    var lstMc = await mcBll.GetMachine(id);
                    var m_Machine = lstMc.First();

                    if (m_Machine == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Machine);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // GET: Master/Machine/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.CompCode = await base.CurrentUserComp();

            return await Task.Run(() => View());
        }

        // POST: Master/Machine/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MachineCode,MachineName,MachineProdType,MachineSize,MachineRemark,CompanyCode,Id,Is_Active,Created_Date,Created_By,Updated_Date,Updated_By")] M_Machine m_Machine)
        {
            if (ModelState.IsValid)
            {
                m_Machine.Created_By = await base.CurrentUserId();

                ResultObject resultObj;

                try
                {
                    using (var mcBll = new MachineBLL())
                    {
                        resultObj = await mcBll.InsertMachine(m_Machine);

                        _cache.Remove("CACHE_MASTER_MACHINE");
                    }

                    return Json(new { success = true, data = (M_Machine)resultObj.ObjectValue, message = "Machine Created." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, data = m_Machine, message = ex.Message });
                }
            }

            var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            return Json(new { success = false, errors = err, data = m_Machine, message = "Created Faield" });

        }

        // GET: Master/Machine/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.CompCode = await base.CurrentUserComp();

            try
            {

                if (_cache.TryGetValue("CACHE_MASTER_MACHINE", out List<M_Machine> c_lstMac))
                {
                    var m_Machine = c_lstMac.Find(m => m.Id == id);

                    if (m_Machine == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Machine);
                }

                using (var mcBll = new MachineBLL())
                {
                    var lstMc = await mcBll.GetMachine(id);

                    var m_Machine = lstMc.First();

                    if (m_Machine == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Machine);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }

        }

        // POST: Master/Machine/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("MachineCode,MachineName,MachineProdType,MachineSize,MachineRemark,CompanyCode,Id,Is_Active,Created_Date,Created_By,Updated_Date,Updated_By")] M_Machine m_Machine)
        {
            if (ModelState.IsValid)
            {
                m_Machine.Updated_By = await base.CurrentUserId();

                ResultObject resultObj;

                try
                {
                    using (var mcBll = new MachineBLL())
                    {
                        resultObj = await mcBll.UpdateMachine(m_Machine);

                        _cache.Remove("CACHE_MASTER_MACHINE");
                    }

                    return Json(new { success = true, data = (M_Machine)resultObj.ObjectValue, message = "Machine Update." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, data = m_Machine, message = ex.Message });
                }
            }

            var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            return Json(new { success = false, errors = err, data = m_Machine, message = "Update Failed" });
            
        }

        // POST: Master/Machine/Delete/5
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
                if (_cache.TryGetValue("CACHE_MASTER_MACHINE", out List<M_Machine> c_lstMac))
                {
                    var m_Machine = c_lstMac.Find(m => m.Id == id);

                    if (m_Machine == null)
                    {
                        return NotFound();
                    }

                    m_Machine.Updated_By = await base.CurrentUserId();

                    using (var mcBll = new MachineBLL())
                    {
                        resultObj = await mcBll.DeleteMachine(m_Machine);

                        _cache.Remove("CACHE_MASTER_MACHINE");
                    }

                    return Json(new { success = true, data = (M_Machine)resultObj.ObjectValue, message = "Machine Deleted." });
                }

                using (var mcBll = new MachineBLL())
                {
                    var lstMc = await mcBll.GetMachine(id);

                    var m_Machine = lstMc.First();

                    if (m_Machine == null)
                    {
                        return NotFound();
                    }

                    m_Machine.Updated_By = await base.CurrentUserId();

                    resultObj = await mcBll.DeleteMachine(m_Machine);

                    _cache.Remove("CACHE_MASTER_MACHINE");
                }

                return Json(new { success = true, data = (M_Machine)resultObj.ObjectValue, message = "Machine Deleted." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            
        }

        public async Task<IActionResult> PrintModalData()
        {
            ViewBag.CompCode = await base.CurrentUserComp();
            return await Task.Run(() => PartialView());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MiddlewareFilter(typeof(JsReportPipeline))]
        public async Task<IActionResult> PrintMachine(List<M_Machine> lstSelMc)   //List<M_Machine> lstSelMc
        {
            try
            {

                List<MachineLabelModel> printMcLabel = lstSelMc.ConvertAll(mc => new MachineLabelModel(mc));

                if(printMcLabel != null)
                    ViewBag.ItemCount = printMcLabel.Count();

                var header = await _jsReport.RenderViewToStringAsync(HttpContext, RouteData, "HeaderReport", new { });
                var footer = await _jsReport.RenderViewToStringAsync(HttpContext, RouteData, "FooterReport", new { });

                HttpContext.JsReportFeature().Recipe(jsreport.Types.Recipe.ChromePdf)
                    .Configure((r) => r.Template.Chrome = new Chrome
                    {
                        DisplayHeaderFooter = true,
                        HeaderTemplate = header,
                        FooterTemplate = footer,
                        Landscape = true,
                        //Format = "A4",
                        //MarginTop = "0.7cm",
                        //MarginLeft = "0.7cm",
                        //MarginBottom = "0.7cm",
                        //MarginRight = "0.7cm"
                        Format = _config["ReportSetting:Format"],
                        MarginTop = _config["ReportSetting:MarginTop"],
                        MarginLeft = _config["ReportSetting:MarginLeft"],
                        MarginBottom = _config["ReportSetting:MarginBottom"],
                        MarginRight = _config["ReportSetting:MarginRight"]
                    });

                return await Task.Run(() => View(printMcLabel));
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

    }
}
