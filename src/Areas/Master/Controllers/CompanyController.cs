using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Maple2.AdminLTE.Bel;
using Maple2.AdminLTE.Dal;
using Maple2.AdminLTE.Bll;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.Extensions.Caching.Memory;
using Maple2.AdminLTE.Uil.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Maple2.AdminLTE.Uil.Areas.Master.Controllers
{
    [Area("Master")]
    public class CompanyController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMemoryCache _cache;

        public CompanyController(IHostingEnvironment hostingEnvironment,
                                 IMemoryCache memoryCache,
                                 UserManager<ApplicationUser> userManager) : base(userManager, memoryCache)
        {
            _hostingEnvironment = hostingEnvironment;
            _cache = memoryCache;
        }


        // GET: Master/Company
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View());
        }

        public async Task<IActionResult> GetCompany()
        {
            try
            {
                if (_cache.TryGetValue("CACHE_MASTER_COMPANY", out List<M_Company> c_lstComp))
                {
                    return Json(new { data = c_lstComp });
                }

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300),
                    SlidingExpiration = TimeSpan.FromSeconds(60),
                    Priority = CacheItemPriority.NeverRemove
                };

                using (var compBll = new CompanyBLL())
                {
                    var lstComp = await compBll.GetCompany(null);

                    _cache.Set("CACHE_MASTER_COMPANY", lstComp, options);

                    return Json(new { data = lstComp });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }

            //using (var compBll = new CompanyBLL())
            //{
            //    return Json(new { data = await compBll.GetCompany(null) });
            //}
        }

        // GET: Master/Company/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {

                if (_cache.TryGetValue("CACHE_MASTER_COMPANY", out List<M_Company> c_lstComp))
                {
                    var m_Company = c_lstComp.Find(c => c.Id == id);

                    if (m_Company == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Company);
                }


                using (var compBll = new CompanyBLL())
                {
                    var lstCompany = await compBll.GetCompany(id);
                    var m_Company = lstCompany.First();

                    if (m_Company == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Company);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // GET: Master/Company/Create
        public async Task<IActionResult> Create()
        {
            return await Task.Run(() => View());
        }

        // POST: Master/Company/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create([Bind("CompanyCode,CompanyName,CompanyLogoPath,AddressL1,AddressL2,AddressL3,AddressL4,Telephone,Fax,CompanyTaxId,Id,Is_Active,Created_Date,Created_By,Updated_Date,Updated_By")] M_Company m_Company)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        m_Company.Created_By = 1;

        //        try
        //        {
        //            using (var compBll = new CompanyBLL())
        //            {
        //                var rowaffected = compBll.InsertCompany(ref m_Company);
        //            }

        //            return Json(new { success = true, data = m_Company, message = "Company Created." });
        //        }
        //        catch (Exception ex)
        //        {
        //            return Json(new { success = false, data = m_Company, message = ex.Message });
        //        }
        //    }

        //    var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
        //    return Json(new { success = false, errors = err, data = m_Company, message = "Created Faield" });
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyCode,CompanyName,CompanyLogoPath,AddressL1,AddressL2,AddressL3,AddressL4,Telephone,Fax,CompanyTaxId,Id,Is_Active,Created_Date,Created_By,Updated_Date,Updated_By")] M_Company m_Company)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    m_Company.Created_By = await base.CurrentUserId();

                    ResultObject resultObj;

                    try
                    {
                        using (var compBll = new CompanyBLL())
                        {
                            resultObj = await compBll.InsertCompany(m_Company);

                            _cache.Remove("CACHE_MASTER_COMPANY");
                        }

                        return Json(new { success = true, data = (M_Company)resultObj.ObjectValue, message = "Company Created." });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, data = m_Company, message = ex.Message });
                    }
                }

                var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
                return Json(new { success = false, errors = err, data = m_Company, message = "Created Faield" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        //public async Task<IActionResult> UploadCompanyLogo(List<IFormFile> files)

        [HttpPost]
        public IActionResult UploadCompanyLogo(List<IFormFile> files)
        {
            //string fileName = string.Empty;
            try
            {

                string fileName = Request.Form["fileName"];

                var filesPath = $"{this._hostingEnvironment.WebRootPath}\\img\\compLogo\\";

                if (!Directory.Exists(filesPath))
                {
                    Directory.CreateDirectory(filesPath);
                }

                foreach (var file in files)
                {
                    var fullFilePath = Path.Combine(filesPath, fileName);

                    if (file.Length <= 0)
                    {
                        continue;
                    }

                    GlobalFunction.SaveThumbnails(0.5, file.OpenReadStream(), fullFilePath);

                    //using (var image = Image.FromStream(file.OpenReadStream(), true, true))
                    //{
                    //    int newWidth, newHeight;

                    //    if (image.Width > 100 || image.Height > 100)
                    //    {
                    //        newWidth = (int)(image.Width * 0.5);
                    //        newHeight = (int)(image.Height * 0.5);
                    //    }
                    //    else
                    //    {
                    //        newWidth = image.Width;
                    //        newHeight = image.Height;
                    //    }

                    //    var thumbnailImg = new Bitmap(newWidth, newHeight);
                    //    var thumbGraph = Graphics.FromImage(thumbnailImg);
                    //    thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                    //    thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                    //    thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    //    var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                    //    thumbGraph.DrawImage(image, imageRectangle);
                    //    thumbnailImg.Save(fullFilePath, image.RawFormat);
                    //}

                    //using (var stream = new FileStream(fullFilePath, FileMode.Create))
                    //{
                    //    await file.CopyToAsync(stream);
                    //}
                }

                return Json(new { success = true, data = fileName, message = files.Count + " Files Uploaded!" });
                //return this.Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        
        // GET: Master/Company/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {

                if (_cache.TryGetValue("CACHE_MASTER_COMPANY", out List<M_Company> c_lstComp))
                {
                    var m_Company = c_lstComp.Find(c => c.Id == id);

                    if (m_Company == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Company);
                }

                using (var compBll = new CompanyBLL())
                {
                    var lstCompany = await compBll.GetCompany(id);
                    var m_Company = lstCompany.First();

                    if (m_Company == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Company);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // POST: Master/Company/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("CompanyCode,CompanyName,CompanyLogoPath,AddressL1,AddressL2,AddressL3,AddressL4,Telephone,Fax,CompanyTaxId,Id,Is_Active,Created_Date,Created_By,Updated_Date,Updated_By")] M_Company m_Company)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    m_Company.Updated_By = await base.CurrentUserId();

                    ResultObject resultObj;

                    try
                    {
                        using (var compBll = new CompanyBLL())
                        {
                            resultObj = await compBll.UpdateCompany(m_Company);

                            _cache.Remove("CACHE_MASTER_COMPANY");
                        }

                        return Json(new { success = true, data = (M_Company)resultObj.ObjectValue, message = "Company Update." });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, data = m_Company, message = ex.Message });
                    }
                }

                var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
                return Json(new { success = false, errors = err, data = m_Company, message = "Update Failed" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // POST: Master/Company/Delete/5
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
                if (_cache.TryGetValue("CACHE_MASTER_COMPANY", out List<M_Company> c_lstComp))
                {
                    var m_Company = c_lstComp.Find(c => c.Id == id);

                    if (m_Company == null)
                    {
                        return NotFound();
                    }

                    m_Company.Updated_By = await base.CurrentUserId();

                    using (CompanyBLL compBll = new CompanyBLL())
                    {
                        resultObj = await compBll.DeleteCompany(m_Company);

                        _cache.Remove("CACHE_MASTER_COMPANY");
                    }

                    return Json(new { success = true, data = (M_Company)resultObj.ObjectValue, message = "Company Deleted." });
                }

                using (CompanyBLL compBll = new CompanyBLL())
                {
                    var lstCompany = await compBll.GetCompany(id);

                    var m_Company = lstCompany.First();

                    if (m_Company == null)
                    {
                        return NotFound();
                    }

                    m_Company.Updated_By = await base.CurrentUserId();

                    resultObj = await compBll.DeleteCompany(m_Company);

                    _cache.Remove("CACHE_MASTER_COMPANY");
                }

                return Json(new { success = true, data = (M_Company)resultObj.ObjectValue, message = "Company Deleted." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
