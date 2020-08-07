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
using Maple2.AdminLTE.Uil.Extensions;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Identity;
using jsreport.AspNetCore;
using jsreport.Types;
using Maple2.AdminLTE.Uil.Areas.Administrator.Models;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Extensions.Configuration;

namespace Maple2.AdminLTE.Uil.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [RequestFormLimits(ValueCountLimit = int.MaxValue)]
    public class UserController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _config;
        private readonly IJsReportMVCService _jsReport;

        //private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IHostingEnvironment hostingEnvironment,
                              IMemoryCache memoryCache,
                              IConfiguration config,
                              IJsReportMVCService jsReport,
                              UserManager<ApplicationUser> userManager) : base(userManager, hostingEnvironment, memoryCache)
        {
            _hostingEnvironment = hostingEnvironment;
            _cache = memoryCache;
            _config = config;
            _jsReport = jsReport;
        }

        // GET: Administrator/User
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View());
        }

        public async Task<IActionResult> GetUser()
        {
            try
            {
                if (_cache.TryGetValue("CACHE_ADMINISTRATOR_USER", out List<M_User> c_lstUser))
                {
                    return Json(new { data = c_lstUser });
                }

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300),
                    SlidingExpiration = TimeSpan.FromSeconds(60),
                    Priority = CacheItemPriority.NeverRemove
                };

                using (var userBll = new UserBLL())
                {
                    var lstUser = await userBll.GetUser(null);

                    _cache.Set("CACHE_ADMINISTRATOR_USER", lstUser, options);

                    return Json(new { data = lstUser });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // GET: Administrator/User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {

                if (_cache.TryGetValue("CACHE_ADMINISTRATOR_USER", out List<M_User> c_lstUser))
                {
                    var m_User = c_lstUser.Find(m => m.Id == id);

                    if (m_User == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_User);
                }

                using (var userBll = new UserBLL())
                {
                    var lstUser = await userBll.GetUser(id);
                    var m_User = lstUser.First();

                    if (m_User == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_User);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // GET: Administrator/User/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.CompCode = await base.CurrentUserComp();
            return await Task.Run(() => View());
        }

        // POST: Administrator/User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserCode,UserName,EmpCode,DeptId,Position,CompanyCode,aspnetuser_Id,UserImagePath,Id,Is_Active,Created_Date,Created_By,Updated_Date,Updated_By")] M_User m_User)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    m_User.Created_By = await base.CurrentUserId();

                    ResultObject resultObj;

                    try
                    {
                        using (var userBll = new UserBLL())
                        {
                            resultObj = await userBll.InsertUser(m_User);

                            _cache.Remove("CACHE_ADMINISTRATOR_USER");
                        }

                        return Json(new { success = true, data = (M_User)resultObj.ObjectValue, message = "User Created." });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, data = m_User, message = ex.Message });
                    }
                }

                var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
                return Json(new { success = false, errors = err, data = m_User, message = "Created Faield" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        public async Task<IActionResult> Register()
        {
            return await Task.Run(() => RedirectToPage("/Account/Register", new { area = string.Empty }));
        }

        [HttpPost]
        public IActionResult UploadUserImage(List<IFormFile> files)
        {
            //string fileName = string.Empty;
            try
            {
                string fileName = Request.Form["fileName"];

                var filesPath = $"{this._hostingEnvironment.WebRootPath}\\img\\users\\";

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
                }

                return Json(new { success = true, data = fileName, message = files.Count + " Files Uploaded!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // GET: Administrator/User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.CompCode = await base.CurrentUserComp();

            try
            {

                if (_cache.TryGetValue("CACHE_ADMINISTRATOR_USER", out List<M_User> c_lstUser))
                {
                    var m_User = c_lstUser.Find(m => m.Id == id);

                    if (m_User == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_User);
                }

                using (var userBll = new UserBLL())
                {
                    var lstUser = await userBll.GetUser(id);
                    var m_User = lstUser.First();

                    if (m_User == null)
                    {
                        return NotFound();
                    }

                    return PartialView(m_User);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }

        }

        // POST: Administrator/User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("UserCode,UserName,EmpCode,DeptId,Position,CompanyCode,aspnetuser_Id,UserImagePath,Id,Is_Active,Created_Date,Created_By,Updated_Date,Updated_By")] M_User m_User)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    m_User.Updated_By = await base.CurrentUserId();

                    ResultObject resultObj;

                    try
                    {
                        using (var userBll = new UserBLL())
                        {
                            resultObj = await userBll.UpdateUser(m_User);

                            _cache.Remove("CACHE_ADMINISTRATOR_USER");
                        }

                        return Json(new { success = true, data = (M_User)resultObj.ObjectValue, message = "User Update." });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, data = m_User, message = ex.Message });
                    }
                }

                var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
                return Json(new { success = false, errors = err, data = m_User, message = "Update Failed" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }

        }

        // POST: Administrator/User/Delete/5
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
                if (_cache.TryGetValue("CACHE_ADMINISTRATOR_USER", out List<M_User> c_lstUser))
                {
                    var m_User = c_lstUser.Find(m => m.Id == id);

                    if (m_User == null)
                    {
                        return NotFound();
                    }

                    m_User.Updated_By = 1;

                    using (var userBll = new UserBLL())
                    {
                        resultObj = await userBll.DeleteUser(m_User);

                        _cache.Remove("CACHE_ADMINISTRATOR_USER");
                    }

                    return Json(new { success = true, data = (M_User)resultObj.ObjectValue, message = "User Deleted." });
                }

                using (var userBll = new UserBLL())
                {
                    var lstUser = await userBll.GetUser(id);
                    var m_User = lstUser.First();

                    if (m_User == null)
                    {
                        return NotFound();
                    }

                    m_User.Updated_By = await base.CurrentUserId();

                    resultObj = await userBll.DeleteUser(m_User);

                    _cache.Remove("CACHE_ADMINISTRATOR_USER");
                }

                return Json(new { success = true, data = (M_User)resultObj.ObjectValue, message = "User Deleted." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> GetMenuAuthen()
        {
            try
            {
                if (_cache.TryGetValue("CACHE_MENU_AUTHEN", out List<M_Menu> c_lstMenu))
                {
                    //return Json(new { data = c_lstMenu });
                    return PartialView("~/Views/Shared/_NavBar.cshtml", c_lstMenu);
                }

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300),
                    SlidingExpiration = TimeSpan.FromSeconds(60),
                    Priority = CacheItemPriority.Low
                };

                using (var menuBll = new MenuBLL())
                {
                    var lstMenu = await menuBll.GetMenuAuthen(await base.CurrentUserId());

                    _cache.Set("CACHE_MENU_AUTHEN", lstMenu, options);

                    //return Json(new { data = lstMenu });
                    return PartialView("~/Views/Shared/_NavBar.cshtml", lstMenu);
                }
            }
            catch (Exception ex)
            {
                string excLog = ex.Message;
                return BadRequest();
            }
        }

        //public async Task<IActionResult> GetApplicationUser()
        //{
        //    try
        //    {

        //        if (_cache.TryGetValue("CACHE_ADMINISTRATOR_APPUSER", out List<ApplicationUser> c_lstAppUser))
        //        {
        //            return Json(new { data = c_lstAppUser });
        //        }

        //        MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
        //        {
        //            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300),
        //            SlidingExpiration = TimeSpan.FromSeconds(60),
        //            Priority = CacheItemPriority.NeverRemove
        //        };


        //        var lstAppUser = await _userManager.Users.ToListAsync();

        //        _cache.Set("CACHE_ADMINISTRATOR_APPUSER", lstAppUser, options);


        //        return Json(new { data = lstAppUser });
        //    }
        //    catch(Exception ex)
        //    {
        //        return BadRequest(new { success = false, message = ex.Message });
        //    }
        //}

        [MiddlewareFilter(typeof(JsReportPipeline))]
        public async Task<IActionResult> Invoice()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(InvoiceModel.Example().Number, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);

                using (Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    bitMap.Save(ms, ImageFormat.Png);
                    ViewBag.QRCodeImage = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                }
            }

            HttpContext.JsReportFeature().Recipe(jsreport.Types.Recipe.ChromePdf)
                .Configure((r) => r.Template.Chrome = new Chrome
                {
                    HeaderTemplate = null,
                    DisplayHeaderFooter = false,
                    Format = _config["ReportSetting:Format"],
                    MarginTop = _config["ReportSetting:MarginTop"],
                    MarginLeft = _config["ReportSetting:MarginLeft"],
                    MarginBottom = _config["ReportSetting:MarginBottom"],
                    MarginRight = _config["ReportSetting:MarginRight"]
                });
            return await Task.Run(() => View(InvoiceModel.Example()));
        }

        //[MiddlewareFilter(typeof(JsReportPipeline))]
        //public IActionResult Invoice()
        //{
        //    ViewBag.FileRefPath = base.JsReportFileRefPath;
        //    HttpContext.JsReportFeature().Recipe(jsreport.Types.Recipe.ChromePdf)
        //        .Configure((r) => r.Template.Chrome = new Chrome
        //        {
        //            HeaderTemplate = null,
        //            DisplayHeaderFooter = false,
        //            Format = "A4",
        //            MarginTop = "1cm",
        //            MarginLeft = "1cm",
        //            MarginBottom = "1cm",
        //            MarginRight = "1cm"
        //        });
        //    return View(InvoiceModel.Example());
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MiddlewareFilter(typeof(JsReportPipeline))]
        public async Task<IActionResult> ProductCard()
        {
            try
            {
                ViewBag.FileRefPath = base.JsReportFileRefPath;
                ViewBag.ItemCount = 10;

                var header = await _jsReport.RenderViewToStringAsync(HttpContext, RouteData, "HeaderReport", new { });
                var footer = await _jsReport.RenderViewToStringAsync(HttpContext, RouteData, "FooterReport", new { });

                HttpContext.JsReportFeature().Recipe(jsreport.Types.Recipe.ChromePdf)
                    .Configure((r) => r.Template.Chrome = new Chrome
                    {
                        DisplayHeaderFooter = true,
                        HeaderTemplate = header,
                        FooterTemplate = footer,
                        Format = _config["ReportSetting:Format"],
                        MarginTop = "0.7cm",
                        MarginLeft = _config["ReportSetting:MarginLeft"],
                        MarginBottom = _config["ReportSetting:MarginBottom"],
                        MarginRight = _config["ReportSetting:MarginRight"]
                    });

                return await Task.Run(() => View(ProductCardModel.Example()));
            }
            catch(Exception ex)
            {
                string excLog = ex.Message;
                return BadRequest();
            }
        }

        [MiddlewareFilter(typeof(JsReportPipeline))]
        public async Task<IActionResult> ProductCard_Label()
        {
            ViewBag.FileRefPath = base.JsReportFileRefPath;

            HttpContext.JsReportFeature().Recipe(jsreport.Types.Recipe.ChromePdf)
                .Configure((r) => r.Template.Chrome = new Chrome
                {
                    DisplayHeaderFooter = false,
                    HeaderTemplate = null,
                    FooterTemplate = null,
                    Width = "8cm",
                    Height = "5.5cm",
                    MarginTop = "0.5cm",
                    MarginLeft = "0.5cm",
                    MarginBottom = "0.5cm",
                    MarginRight = "0.5cm"
                });
            return await Task.Run(() => View(ProductCardModel.Example()));
        }




    }
}
