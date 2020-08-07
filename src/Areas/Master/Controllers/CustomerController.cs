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
    public class CustomerController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMemoryCache _cache;

        public CustomerController(IHostingEnvironment hostingEnvironment,
                                  IMemoryCache memoryCache,
                                  UserManager<ApplicationUser> userManager) : base(userManager, hostingEnvironment, memoryCache)
        {
            _hostingEnvironment = hostingEnvironment;
            _cache = memoryCache;
        }

        // GET: Master/Customer
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View());
        }

        // GET: Master/Customer
        public async Task<IActionResult> GetCustomer()
        {
            try
            {
                if (_cache.TryGetValue("CACHE_MASTER_CUSTOMER", out List<M_Customer> c_lstCust))
                {
                    return Json(new { data = c_lstCust });
                }

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300),
                    SlidingExpiration = TimeSpan.FromSeconds(60),
                    Priority = CacheItemPriority.NeverRemove
                };

                using (var custBll = new CustomerBLL())
                {
                    var lstCust = await custBll.GetCustomer(null);

                    _cache.Set("CACHE_MASTER_CUSTOMER", lstCust, options);

                    return Json(new { data = lstCust });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            //using (var custBll = new CustomerBLL())
            //{
            //    return Json(new { data = await custBll.GetCustomer(null) });
            //}
        }

        // GET: Master/Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {

                if (_cache.TryGetValue("CACHE_MASTER_CUSTOMER", out List<M_Customer> c_lstCust))
                {
                    var m_Customer = c_lstCust.Find(c => c.Id == id);

                    if (m_Customer == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Customer);
                }

                using (var custBll = new CustomerBLL())
                {
                    var lstCust = await custBll.GetCustomer(id);
                    var m_Customer = lstCust.First();

                    if (m_Customer == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Customer);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // GET: Master/Customer/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.CompCode = await base.CurrentUserComp();
            return await Task.Run(() => View());
        }

        // POST: Master/Customer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerCode,CustomerName,AddressL1,AddressL2,AddressL3,AddressL4,Telephone,Fax,CustomerEmail,CustomerContact,CreditTerm,PriceLevel,CustomerTaxId,Remark,CompanyCode,Id,Is_Active,Created_Date,Created_By,Updated_Date,Updated_By")] M_Customer m_Customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    m_Customer.Created_By = await base.CurrentUserId();

                    ResultObject resultObj;

                    try
                    {
                        using (var custBll = new CustomerBLL())
                        {
                            resultObj = await custBll.InsertCustomer(m_Customer);

                            _cache.Remove("CACHE_MASTER_CUSTOMER");
                        }

                        return Json(new { success = true, data = (M_Customer)resultObj.ObjectValue, message = "Customer Created." });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, data = m_Customer, message = ex.Message });
                    }
                }

                var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
                return Json(new { success = false, errors = err, data = m_Customer, message = "Created Faield" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }

        }

        // GET: Master/Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.CompCode = await base.CurrentUserComp();

            try
            {

                if (_cache.TryGetValue("CACHE_MASTER_CUSTOMER", out List<M_Customer> c_lstCust))
                {
                    var m_Customer = c_lstCust.Find(c => c.Id == id);

                    if (m_Customer == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Customer);
                }

                using (var custBll = new CustomerBLL())
                {
                    var lstCust = await custBll.GetCustomer(id);
                    var m_Customer = lstCust.First();

                    if (m_Customer == null)
                    {
                        return NotFound();
                    }



                    return PartialView(m_Customer);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // POST: Master/Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("CustomerCode,CustomerName,AddressL1,AddressL2,AddressL3,AddressL4,Telephone,Fax,CustomerEmail,CustomerContact,CreditTerm,PriceLevel,CustomerTaxId,Remark,CompanyCode,Id,Is_Active,Created_Date,Created_By,Updated_Date,Updated_By")] M_Customer m_Customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    m_Customer.Updated_By = await base.CurrentUserId();

                    ResultObject resultObj;

                    try
                    {
                        using (var custBll = new CustomerBLL())
                        {
                            resultObj = await custBll.UpdateCustomer(m_Customer);

                            _cache.Remove("CACHE_MASTER_CUSTOMER");
                        }

                        return Json(new { success = true, data = (M_Customer)resultObj.ObjectValue, message = "Customer Update." });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, data = m_Customer, message = ex.Message });
                    }
                }

                var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
                return Json(new { success = false, errors = err, data = m_Customer, message = "Update Failed" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }

        }

        // POST: Master/Customer/Delete/5
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
                if (_cache.TryGetValue("CACHE_MASTER_CUSTOMER", out List<M_Customer> c_lstCust))
                {
                    var m_Customer = c_lstCust.Find(c => c.Id == id);

                    if (m_Customer == null)
                    {
                        return NotFound();
                    }

                    m_Customer.Updated_By = await base.CurrentUserId();

                    using (var custBll = new CustomerBLL())
                    {
                        resultObj = await custBll.DeleteCustomer(m_Customer);

                        _cache.Remove("CACHE_MASTER_CUSTOMER");
                    }

                    return Json(new { success = true, data = (M_Customer)resultObj.ObjectValue, message = "Customer Deleted." });
                }

                using (var custBll = new CustomerBLL())
                {
                    var lstCust = await custBll.GetCustomer(id);
                    var m_Customer = lstCust.First();

                    if (m_Customer == null)
                    {
                        return NotFound();
                    }

                    m_Customer.Updated_By = await base.CurrentUserId();

                    resultObj = await custBll.DeleteCustomer(m_Customer);

                    _cache.Remove("CACHE_MASTER_CUSTOMER");
                }

                return Json(new { success = true, data = (M_Customer)resultObj.ObjectValue, message = "Customer Deleted." });
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
                string path = $"{this._hostingEnvironment.WebRootPath}\\uploads\\Customer\\";

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
        public async Task<IActionResult> UploadModelData(List<M_Customer> lstCust)
        {
            var uId = await base.CurrentUserId();

            lstCust.ForEach(m =>
            {
                m.Created_By = uId;
            });

            try
            {
                using (var custBll = new CustomerBLL())
                {
                    var rowaffected = await custBll.BulkInsertCustomer(lstCust);

                    _cache.Remove("CACHE_MASTER_CUSTOMER");
                }

                return Json(new { success = true, data = lstCust, message = "Import Success." });
            }
            catch (Exception ex)
            {
                return Json(new { success = true, data = lstCust, message = ex.Message });
            }
        }

    }
}
