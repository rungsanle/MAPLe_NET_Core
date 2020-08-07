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
using Microsoft.Extensions.Caching.Memory;
using Maple2.AdminLTE.Bll;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using Maple2.AdminLTE.Uil.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Maple2.AdminLTE.Uil.Areas.Master.Controllers
{
    [Area("Master")]
    [RequestFormLimits(ValueCountLimit = int.MaxValue)]
    public class MaterialController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMemoryCache _cache;

        public MaterialController(IHostingEnvironment hostingEnvironment,
                                  IMemoryCache memoryCache,
                                  UserManager<ApplicationUser> userManager) : base(userManager, hostingEnvironment, memoryCache)
        {
            _hostingEnvironment = hostingEnvironment;
            _cache = memoryCache;
        }

        // GET: Master/Material
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View());
        }

        // GET: Master/Material
        public async Task<IActionResult> GetMaterial()
        {
            try
            {
                if (_cache.TryGetValue("CACHE_MASTER_MATERIAL", out List<M_Material> c_lstMat))
                {
                    return Json(new { data = c_lstMat });
                }

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300),
                    SlidingExpiration = TimeSpan.FromSeconds(60),
                    Priority = CacheItemPriority.NeverRemove
                };

                using (var matBll = new MaterialBLL())
                {
                    var lstMat = await matBll.GetMaterial(null);

                    _cache.Set("CACHE_MASTER_MATERIAL", lstMat, options);

                    return Json(new { data = lstMat });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // GET: Master/Material/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                if (_cache.TryGetValue("CACHE_MASTER_MATERIAL", out List<M_Material> c_lstMat))
                {
                    var m_Material = c_lstMat.Find(m => m.Id == id);

                    if (m_Material == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Material);
                }


                using (var matBll = new MaterialBLL())
                {
                    var lstMat = await matBll.GetMaterial(id);
                    var m_Material = lstMat.First();

                    if (m_Material == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Material);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // GET: Master/Material/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.CompCode = await base.CurrentUserComp();
            return await Task.Run(() => View());
        }

        // POST: Master/Material/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaterialCode,MaterialName,MaterialDesc1,MaterialDesc2,RawMatTypeId,UnitId,PackageStdQty,WarehouseId,LocationId,CompanyCode,MaterialImagePath,Id,Is_Active,Created_Date,Created_By,Updated_Date,Updated_By")] M_Material m_Material)
        {
            if (ModelState.IsValid)
            {
                m_Material.Created_By = await base.CurrentUserId();

                ResultObject resultObj;

                try
                {
                    using (var matBll = new MaterialBLL())
                    {
                        resultObj = await matBll.InsertMaterial(m_Material);

                        _cache.Remove("CACHE_MASTER_MATERIAL");
                        _cache.Remove("CACHE_MASTER_MATERIAL_BYRAWTYPE");
                        
                    }

                    return Json(new { success = true, data = (M_Material)resultObj.ObjectValue, message = "Material Created." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, data = m_Material, message = ex.Message });
                }
            }

            var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            return Json(new { success = false, errors = err, data = m_Material, message = "Created Faield" });
        }

        [HttpPost]
        public IActionResult UploadMaterialImage(List<IFormFile> files)
        {
            //string fileName = string.Empty;
            try
            {
                string fileName = Request.Form["fileName"];

                var filesPath = $"{this._hostingEnvironment.WebRootPath}\\img\\materials\\";

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
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            //return this.Ok();
        }

        // GET: Master/Material/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.CompCode = await base.CurrentUserComp();

            try
            {

                if (_cache.TryGetValue("CACHE_MASTER_MATERIAL", out List<M_Material> c_lstMat))
                {
                    var m_Material = c_lstMat.Find(m => m.Id == id);

                    if (m_Material == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Material);
                }

                using (var matBll = new MaterialBLL())
                {
                    var lstMat = await matBll.GetMaterial(id);
                    var m_Material = lstMat.First();

                    if (m_Material == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Material);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // POST: Master/Material/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("MaterialCode,MaterialName,MaterialDesc1,MaterialDesc2,RawMatTypeId,UnitId,PackageStdQty,WarehouseId,LocationId,CompanyCode,MaterialImagePath,Id,Is_Active,Created_Date,Created_By,Updated_Date,Updated_By")] M_Material m_Material)
        {
            if (ModelState.IsValid)
            {
                m_Material.Updated_By = await base.CurrentUserId();

                ResultObject resultObj;

                try
                {
                    using (var matBll = new MaterialBLL())
                    {
                        resultObj = await matBll.UpdateMaterial(m_Material);

                        _cache.Remove("CACHE_MASTER_MATERIAL");
                        _cache.Remove("CACHE_MASTER_MATERIAL_BYRAWTYPE");
                    }

                    return Json(new { success = true, data = (M_Material)resultObj.ObjectValue, message = "Material Update." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, data = m_Material, message = ex.Message });
                }
            }

            var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            return Json(new { success = false, errors = err, data = m_Material, message = "Update Failed" });
            
        }

        // POST: Master/Material/Delete/5
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
                if (_cache.TryGetValue("CACHE_MASTER_MATERIAL", out List<M_Material> c_lstMat))
                {
                    var m_Material = c_lstMat.Find(m => m.Id == id);

                    if (m_Material == null)
                    {
                        return NotFound();
                    }

                    m_Material.Updated_By = await base.CurrentUserId();

                    using (var matBll = new MaterialBLL())
                    {
                        resultObj = await matBll.DeleteMaterial(m_Material);

                        _cache.Remove("CACHE_MASTER_MATERIAL");
                    }

                    return Json(new { success = true, data = (M_Material)resultObj.ObjectValue, message = "Material Deleted." });
                }

                using (var matBll = new MaterialBLL())
                {
                    var lstMat = await matBll.GetMaterial(id);
                    var m_Material = lstMat.First();

                    if (m_Material == null)
                    {
                        return NotFound();
                    }

                    m_Material.Updated_By = await base.CurrentUserId();

                    resultObj = await matBll.DeleteMaterial(m_Material);

                    _cache.Remove("CACHE_MASTER_MATERIAL");
                    _cache.Remove("CACHE_MASTER_MATERIAL_BYRAWTYPE");
                }

                return Json(new { success = true, data = (M_Material)resultObj.ObjectValue, message = "Material Deleted." });
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

                string path = $"{this._hostingEnvironment.WebRootPath}\\uploads\\Materials\\";

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
        public async Task<IActionResult> UploadModelData(List<M_Material> lstMat)
        {
            var uId = await base.CurrentUserId();
            lstMat.ForEach(m =>
            {
                m.Created_By = uId;
            });

            try
            {
                using (var matBll = new MaterialBLL())
                {
                    var rowaffected = await matBll.BulkInsertMaterial(lstMat);

                    _cache.Remove("CACHE_MASTER_MATERIAL");
                    _cache.Remove("CACHE_MASTER_MATERIAL_BYRAWTYPE");
                }

                return Json(new { success = true, data = lstMat, message = "Import Success." });
            }
            catch (Exception ex)
            {
                return Json(new { success = true, data = lstMat, message = ex.Message });
            }
        }
    }
}
