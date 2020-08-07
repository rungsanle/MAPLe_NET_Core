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
using Microsoft.AspNetCore.Http;
using System.IO;
using Maple2.AdminLTE.Uil.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Identity;

namespace Maple2.AdminLTE.Uil.Areas.Master.Controllers
{
    [Area("Master")]
    [RequestFormLimits(ValueCountLimit = int.MaxValue)]
    public class VendorController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMemoryCache _cache;

        public VendorController(IHostingEnvironment hostingEnvironment,
                                IMemoryCache memoryCache,
                                UserManager<ApplicationUser> userManager) : base(userManager, hostingEnvironment, memoryCache)
        {
            _hostingEnvironment = hostingEnvironment;
            _cache = memoryCache;
        }

        // GET: Master/Vendor
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View());
        }

        // GET: Master/Vendor
        public async Task<IActionResult> GetVendor()
        {
            try
            {
                if (_cache.TryGetValue("CACHE_MASTER_VENDOR", out List<M_Vendor> c_lstVend))
                {
                    return Json(new { data = c_lstVend });
                }

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300),
                    SlidingExpiration = TimeSpan.FromSeconds(60),
                    Priority = CacheItemPriority.NeverRemove
                };

                using (var vendBll = new VendorBLL())
                {
                    var lstVend = await vendBll.GetVendor(null);

                    _cache.Set("CACHE_MASTER_VENDOR", lstVend, options);

                    return Json(new { data = lstVend });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }

            //using (var vendBll = new VendorBLL())
            //{
            //    return Json(new { data = await vendBll.GetVendor(null) });
            //}
        }

        // GET: Master/Vendor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {

                if (_cache.TryGetValue("CACHE_MASTER_VENDOR", out List<M_Vendor> c_lstVend))
                {
                    var m_Vendor = c_lstVend.Find(v => v.Id == id);

                    if (m_Vendor == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Vendor);
                }

                using (var vendBll = new VendorBLL())
                {
                    var lstVend = await vendBll.GetVendor(id);
                    var m_Vendor = lstVend.First();

                    if (m_Vendor == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Vendor);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // GET: Master/Vendor/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.CompCode = await base.CurrentUserComp();
            return await Task.Run(() => View());
        }

        // POST: Master/Vendor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VendorCode,VendorName,AddressL1,AddressL2,AddressL3,AddressL4,Telephone,Fax,VendorEmail,VendorContact,CreditTerm,PriceLevel,VendorTaxId,Remark,CompanyCode,Id,Is_Active,Created_Date,Created_By,Updated_Date,Updated_By")] M_Vendor m_Vendor)
        {
            if (ModelState.IsValid)
            {
                m_Vendor.Created_By = await base.CurrentUserId();

                ResultObject resultObj;

                try
                {
                    using (var vendBll = new VendorBLL())
                    {
                        resultObj = await vendBll.InsertVendor(m_Vendor);

                        _cache.Remove("CACHE_MASTER_VENDOR");
                    }

                    return Json(new { success = true, data = (M_Vendor)resultObj.ObjectValue, message = "Vendor Created." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, data = m_Vendor, message = ex.Message });
                }
            }

            var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            return Json(new { success = false, errors = err, data = m_Vendor, message = "Created Faield" });
            
        }

        // GET: Master/Vendor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.CompCode = await base.CurrentUserComp();

            try
            {

                if (_cache.TryGetValue("CACHE_MASTER_VENDOR", out List<M_Vendor> c_lstVend))
                {
                    var m_Vendor = c_lstVend.Find(v => v.Id == id);

                    if (m_Vendor == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Vendor);
                }

                using (var vendBll = new VendorBLL())
                {
                    var lstVend = await vendBll.GetVendor(id);
                    var m_Vendor = lstVend.First();

                    if (m_Vendor == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Vendor);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // POST: Master/Vendor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("VendorCode,VendorName,AddressL1,AddressL2,AddressL3,AddressL4,Telephone,Fax,VendorEmail,VendorContact,CreditTerm,PriceLevel,VendorTaxId,Remark,CompanyCode,Id,Is_Active,Created_Date,Created_By,Updated_Date,Updated_By")] M_Vendor m_Vendor)
        {
            if (ModelState.IsValid)
            {
                m_Vendor.Updated_By = await base.CurrentUserId();

                ResultObject resultObj;

                try
                {
                    using (var vendBll = new VendorBLL())
                    {
                        resultObj = await vendBll.UpdateVendor(m_Vendor);

                        _cache.Remove("CACHE_MASTER_VENDOR");
                    }

                    return Json(new { success = true, data = (M_Vendor)resultObj.ObjectValue, message = "Vendor Update." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, data = m_Vendor, message = ex.Message });
                }
            }

            var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            return Json(new { success = false, errors = err, data = m_Vendor, message = "Update Failed" });
            
        }

        // POST: Master/Vendor/Delete/5
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
                if (_cache.TryGetValue("CACHE_MASTER_VENDOR", out List<M_Vendor> c_lstVend))
                {
                    var m_Vendor = c_lstVend.Find(v => v.Id == id);

                    if (m_Vendor == null)
                    {
                        return NotFound();
                    }

                    m_Vendor.Updated_By = await base.CurrentUserId();

                    using (var vendBll = new VendorBLL())
                    {
                        resultObj = await vendBll.DeleteVendor(m_Vendor);

                        _cache.Remove("CACHE_MASTER_VENDOR");
                    }

                    return Json(new { success = true, data = (M_Vendor)resultObj.ObjectValue, message = "Vendor Deleted." });
                }

                using (var vendBll = new VendorBLL())
                {
                    var lstVend = await vendBll.GetVendor(id);
                    var m_Vendor = lstVend.First();

                    if (m_Vendor == null)
                    {
                        return NotFound();
                    }

                    m_Vendor.Updated_By = await base.CurrentUserId();

                    resultObj = await vendBll.DeleteVendor(m_Vendor);

                    _cache.Remove("CACHE_MASTER_VENDOR");
                }

                return Json(new { success = true, data = (M_Vendor)resultObj.ObjectValue, message = "Vendor Deleted." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> UploadData()
        {
            ViewBag.CompCode = await base.CurrentUserComp();
            return await Task.Run(() => PartialView());
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            string jsonData = string.Empty;
            string filePath = string.Empty;

            try
            {
                string path = $"{this._hostingEnvironment.WebRootPath}\\uploads\\Vendors\\";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                foreach (var file in files)
                {
                    filePath = Path.Combine(path, file.FileName);

                    if (file.Length <= 0)
                    {
                        continue;
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }


                    jsonData = GlobalFunction.ConvertCsvFileToJsonObject(filePath);
                }

                return Json(new { success = true, data = jsonData, message = files.Count + "Files Uploaded!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadModelData(List<M_Vendor> lstVend)
        {
            var uId = await base.CurrentUserId();
            lstVend.ForEach(m =>
            {
                m.Created_By = uId;
            });

            try
            {
                using (var vendBll = new VendorBLL())
                {
                    var rowaffected = await vendBll.BulkInsertVendor(lstVend);

                    _cache.Remove("CACHE_MASTER_VENDOR");
                }

                return Json(new { success = true, data = lstVend, message = "Import Success." });
            }
            catch (Exception ex)
            {
                return Json(new { success = true, data = lstVend, message = ex.Message });
            }
        }
    }
}
