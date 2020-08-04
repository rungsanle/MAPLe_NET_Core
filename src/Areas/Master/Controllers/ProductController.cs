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
    public class ProductController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMemoryCache _cache;

        public ProductController(IHostingEnvironment hostingEnvironment,
                                 IMemoryCache memoryCache,
                                 UserManager<ApplicationUser> userManager) : base(userManager, memoryCache)
        {
            _hostingEnvironment = hostingEnvironment;
            _cache = memoryCache;
        }

        // GET: Master/Product
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View());
        }

        // GET: Master/Product
        public async Task<IActionResult> GetProduct()
        {
            try
            {
                if (_cache.TryGetValue("CACHE_MASTER_PRODUCT", out List<M_Product> c_lstProd))
                {
                    return Json(new { data = c_lstProd });
                }

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300),
                    SlidingExpiration = TimeSpan.FromSeconds(60),
                    Priority = CacheItemPriority.NeverRemove
                };

                using (var prodBll = new ProductBLL())
                {
                    var lstProd = await prodBll.GetProduct(null);

                    _cache.Set("CACHE_MASTER_PRODUCT", lstProd, options);

                    return Json(new { data = lstProd });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // GET: Master/Product Process
        public async Task<IActionResult> GetProductProcess(int id)
        {
            try
            {
                using (var prodBll = new ProductBLL())
                {
                    var lstProdProcess = await prodBll.GetProductProcess(id);

                    return Json(new { data = lstProdProcess });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // GET: Master/Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {

                if (_cache.TryGetValue("CACHE_MASTER_PRODUCT", out List<M_Product> c_lstProd))
                {
                    var m_Product = c_lstProd.Find(p => p.Id == id);

                    if (m_Product == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Product);
                }

                using (var prodBll = new ProductBLL())
                {
                    var lstProd = await prodBll.GetProduct(id);
                    var m_Product = lstProd.First();

                    if (m_Product == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Product);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // GET: Master/Product/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.CompCode = await base.CurrentUserComp();
            return await Task.Run(() => View());
        }

        // POST: Master/Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductCode,ProductName,ProductNameRef,ProductDesc,MaterialTypeId,ProductionTypeId,MachineId,UnitId,PackageStdQty,SalesPrice1,SalesPrice2,SalesPrice3,SalesPrice4,SalesPrice5,GLSalesAccount,GLInventAccount,GLCogsAccount,RevisionNo,WarehouseId,LocationId,CompanyCode,ProductImagePath,ProdProcess,Id,Is_Active,Created_Date,Created_By,Updated_Date,Updated_By")] M_Product m_Product)
        {
            if (ModelState.IsValid)
            {
                m_Product.Created_By = await base.CurrentUserId();

                ResultObject resultObj;

                try
                {
                    using (var prodBll = new ProductBLL())
                    {
                        resultObj = await prodBll.InsertProduct(m_Product);

                        _cache.Remove("CACHE_MASTER_PRODUCT");
                    }

                    return Json(new { success = true, data = (M_Product)resultObj.ObjectValue, message = "Product Created." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, data = m_Product, message = ex.Message });
                }
            }

            var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            return Json(new { success = false, errors = err, data = m_Product, message = "Created Faield" });
        }

        [HttpPost]
        public IActionResult UploadProductImage(List<IFormFile> files)
        {
            try
            {
                string fileName = Request.Form["fileName"];

                var filesPath = $"{this._hostingEnvironment.WebRootPath}\\img\\products\\";

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
                }

                return Json(new { success = true, data = fileName, message = files.Count + " Files Uploaded!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // GET: Master/Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.CompCode = await base.CurrentUserComp();

            try
            {

                if (_cache.TryGetValue("CACHE_MASTER_PRODUCT", out List<M_Product> c_lstProd))
                {
                    var m_Product = c_lstProd.Find(p => p.Id == id);

                    if (m_Product == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Product);
                }

                using (var prodBll = new ProductBLL())
                {
                    var lstProd = await prodBll.GetProduct(id);
                    var m_Product = lstProd.First();

                    if (m_Product == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_Product);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // POST: Master/Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ProductCode,ProductName,ProductNameRef,ProductDesc,MaterialTypeId,ProductionTypeId,MachineId,UnitId,PackageStdQty,SalesPrice1,SalesPrice2,SalesPrice3,SalesPrice4,SalesPrice5,GLSalesAccount,GLInventAccount,GLCogsAccount,RevisionNo,WarehouseId,LocationId,CompanyCode,ProductImagePath,ProdProcess,Id,Is_Active,Created_Date,Created_By,Updated_Date,Updated_By")] M_Product m_Product)
        {
            if (ModelState.IsValid)
            {
                m_Product.Updated_By = await base.CurrentUserId();

                ResultObject resultObj;

                try
                {
                    using (var prodBll = new ProductBLL())
                    {
                        resultObj = await prodBll.UpdateProduct(m_Product);

                        _cache.Remove("CACHE_MASTER_PRODUCT");
                    }

                    return Json(new { success = true, data = (M_Product)resultObj.ObjectValue, message = "Product Update." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, data = m_Product, message = ex.Message });
                }
            }

            var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            return Json(new { success = false, errors = err, data = m_Product, message = "Update Failed" });
        }

        // POST: Master/Product/Delete/5
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
                if (_cache.TryGetValue("CACHE_MASTER_PRODUCT", out List<M_Product> c_lstProd))
                {
                    var m_Product = c_lstProd.Find(p => p.Id == id);

                    if (m_Product == null)
                    {
                        return NotFound();
                    }

                    m_Product.Updated_By = await base.CurrentUserId();

                    using (var prodBll = new ProductBLL())
                    {
                        resultObj = await prodBll.DeleteProduct(m_Product);

                        _cache.Remove("CACHE_MASTER_PRODUCT");
                    }

                    return Json(new { success = true, data = (M_Product)resultObj.ObjectValue, message = "Product Deleted." });
                }

                using (var prodBll = new ProductBLL())
                {
                    var lstProd = await prodBll.GetProduct(id);
                    var m_Product = lstProd.First();

                    if (m_Product == null)
                    {
                        return NotFound();
                    }

                    m_Product.Updated_By = await base.CurrentUserId();

                    resultObj = await prodBll.DeleteProduct(m_Product);

                    _cache.Remove("CACHE_MASTER_PRODUCT");
                }

                return Json(new { success = true, data = (M_Product)resultObj.ObjectValue, message = "Product Deleted." });
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
                string path = $"{this._hostingEnvironment.WebRootPath}\\uploads\\Products\\";

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
        public async Task<IActionResult> UploadModelData(List<M_Product> lstProd)
        {
            var uId = await base.CurrentUserId();
            lstProd.ForEach(m =>
            {
                m.Created_By = uId;
            });

            try
            {
                using (var prodBll = new ProductBLL())
                {
                    var rowaffected = await prodBll.BulkInsertProduct(lstProd);

                    _cache.Remove("CACHE_MASTER_PRODUCT");
                }

                return Json(new { success = true, data = lstProd, message = "Import Success." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = lstProd, message = ex.Message });
            }
        }
    }
}
