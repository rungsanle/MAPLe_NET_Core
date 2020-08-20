using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maple2.AdminLTE.Bel;
using Maple2.AdminLTE.Bll;
using Maple2.AdminLTE.Uil.Areas.Transaction.Models;
using Maple2.AdminLTE.Uil.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Maple2.AdminLTE.Uil.Areas.Transaction.Controllers
{
    [Area("Transaction")]
    [RequestFormLimits(ValueCountLimit = int.MaxValue)]
    public class ArrivalController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMemoryCache _cache;

        public ArrivalController(IHostingEnvironment hostingEnvironment,
                                 IMemoryCache memoryCache,
                                 UserManager<ApplicationUser> userManager) : base(userManager, hostingEnvironment, memoryCache)
        {
            _hostingEnvironment = hostingEnvironment;
            _cache = memoryCache;
        }

        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View());
        }

        public async Task<IActionResult> GetSearchArrivalType()
        {
            try
            {
                if (_cache.TryGetValue("CACHE_MASTER_ARRIVALTYPE", out List<M_ArrivalType> c_lstArrType))
                {
                    return Json(new { data = c_lstArrType });
                }

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300),
                    SlidingExpiration = TimeSpan.FromSeconds(60),
                    Priority = CacheItemPriority.Low
                };

                using (var arrTypeBll = new ArrivalTypeBLL())
                {
                    var lstArrType = await arrTypeBll.GetArrivalType(null);

                    _cache.Set("CACHE_MASTER_ARRIVALTYPE", lstArrType, options);

                    return Json(new { data = lstArrType });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> GetSearchRawMaterialType()
        {
            try
            {
                if (_cache.TryGetValue("CACHE_MASTER_RAWMATTYPE", out List<M_RawMaterialType> c_lstRawMatType))
                {
                    return Json(new { data = c_lstRawMatType });
                }

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300),
                    SlidingExpiration = TimeSpan.FromSeconds(60),
                    Priority = CacheItemPriority.Low
                };

                using (var rawMatTypeBll = new RawMaterialTypeBLL())
                {
                    var lstRawMatType = await rawMatTypeBll.GetRawMaterialType(null);

                    _cache.Set("CACHE_MASTER_RAWMATTYPE", lstRawMatType, options);

                    return Json(new { data = lstRawMatType });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }

            //RawMaterial Type DbContext
            //using (var rawMatTypeBll = new RawMaterialTypeBLL())
            //{
            //    return Json(new { data = await rawMatTypeBll.GetRawMaterialType(null) });
            //}
        }

        public async Task<IActionResult> GetArrival()
        {
            try
            {
                using (var arrBll = new ArrivalBLL())
                {
                    var lstArr = await arrBll.GetArrival(null);

                    return Json(new { data = lstArr });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> GetSearchArrival([Bind("ArrivalNo,DocRefNo,ArrivalTypeId,RawMatTypeId,ArrivalDateF,ArrivalDateT,DocRefDateF,DocRefDateT")] AdvSearchArrivalModel advArrModel)
        {
            try
            {
                using (var arrBll = new ArrivalBLL(await base.CurrentUserComp()))
                {
                    var lstArr = await arrBll.GetArrival(advArrModel.ArrivalNo, advArrModel.DocRefNo, advArrModel.ArrivalTypeId, 
                        advArrModel.RawMatTypeId, advArrModel.ArrivalDateF, advArrModel.ArrivalDateT, advArrModel.DocRefDateF, advArrModel.DocRefDateT);

                    return Json(new { data = lstArr });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> GetArrivalDetail(int? arrId)
        {
            try
            {
                using (var arrBll = new ArrivalBLL())
                {
                    var lstArrDtl = await arrBll.GetArrivalDetails(arrId);

                    return Json(new { data = lstArrDtl });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {

                using (var arrBll = new ArrivalBLL())
                {
                    var lstArr = await arrBll.GetArrival(id);
                    var arr = lstArr.First();

                    if (arr == null)
                    {
                        return NotFound();
                    }

                    return PartialView(arr);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.CompCode = await base.CurrentUserComp();
            return await Task.Run(() => View());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ArrivalNo,ArrivalDate,RawMatTypeId,VendorId,ArrivalTypeId,PurchaseOrderNo,DocRefNo,DocRefDate,ArrivalRemark,CompanyCode,ArrivalDetails,Is_Active,Created_Date,Created_By,Updated_Date,Updated_By")] T_Arrival_Header t_Arrival_Header)
        {
            if (ModelState.IsValid)
            {
                t_Arrival_Header.Created_By = await base.CurrentUserId();

                ResultObject resultObj;

                try
                {
                    using (var arrBll = new ArrivalBLL())
                    {
                        resultObj = await arrBll.InsertArrival(t_Arrival_Header);
                    }

                    return Json(new { success = true, data = (T_Arrival_Header)resultObj.ObjectValue, message = "Arrival Created." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, data = t_Arrival_Header, message = ex.Message });
                }
            }

            var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            return Json(new { success = false, errors = err, data = t_Arrival_Header, message = "Created Faield" });
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.CompCode = await base.CurrentUserComp();

            try
            {
                using (var arrBll = new ArrivalBLL())
                {
                    var lstArr = await arrBll.GetArrival(id);
                    var arr = lstArr.First();

                    if (arr == null)
                    {
                        return NotFound();
                    }

                    return PartialView(arr);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,ArrivalNo,ArrivalDate,RawMatTypeId,VendorId,ArrivalTypeId,PurchaseOrderNo,DocRefNo,DocRefDate,ArrivalRemark,CompanyCode,ArrivalDetails,Is_Active,Created_Date,Created_By,Updated_Date,Updated_By")] T_Arrival_Header t_Arrival_Header)
        {
            if (ModelState.IsValid)
            {
                t_Arrival_Header.Updated_By = await base.CurrentUserId();

                ResultObject resultObj;

                try
                {
                    using (var arrBll = new ArrivalBLL())
                    {
                        resultObj = await arrBll.UpdateArrival(t_Arrival_Header);
                    }

                    return Json(new { success = true, data = (T_Arrival_Header)resultObj.ObjectValue, message = "Arrival Update." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, data = t_Arrival_Header, message = ex.Message });
                }
            }

            var err = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            return Json(new { success = false, errors = err, data = t_Arrival_Header, message = "Update Failed" });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ResultObject resultObj = null;

            try
            {

                using (var arrBll = new ArrivalBLL())
                {
                    var lstArr = await arrBll.GetArrival(id);
                    var arr = lstArr.First();

                    if (arr == null)
                    {
                        return NotFound();
                    }

                    arr.Updated_By = await base.CurrentUserId();

                    //resultObj = await arrBll.DeleteProduct(arr);

                }

                return Json(new { success = true, data = (T_Arrival_Header)resultObj.ObjectValue, message = "Arrival Deleted." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> GetVendor(string vcode)
        {
            try
            {
                if (_cache.TryGetValue("CACHE_MASTER_VENDOR", out List<M_Vendor> c_lstVend))
                {
                    if (string.IsNullOrEmpty(vcode))
                    {
                        return Json(new { success = true, data = c_lstVend, message = "Get Vendor Success" });
                    }
                    else
                    {
                        M_Vendor m_Vendor = c_lstVend.Find(v => v.VendorCode == vcode);

                        if (m_Vendor != null)
                        {
                            return Json(new { success = true, data = m_Vendor, message = "Get Vendor Success" });
                        }
                        else
                        {
                            return Json(new { success = false, data = string.Empty, message = "Vendor Not Found" });
                        }
                    }
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

                    if (string.IsNullOrEmpty(vcode))
                    {
                        return Json(new { success = true, data = lstVend, message = "Get Vendor Success" });
                    }
                    else
                    {
                        M_Vendor m_Vendor = lstVend.Find(v => v.VendorCode == vcode);

                        if (m_Vendor != null)
                        {
                            return Json(new { success = true, data = m_Vendor, message = "Get Vendor Success" });
                        }
                        else
                        {
                            return Json(new { success = false, data = string.Empty, message = "Vendor Not Found" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            
        }

        public async Task<IActionResult> GetMaterial(string vcode, int? rawMatTypeId, string companyCode)
        {
            try
            {
                object matTypeId = TempData.Peek("MatTypeId");

                if ((int?)matTypeId == rawMatTypeId)
                {
                    if (_cache.TryGetValue("CACHE_MASTER_MATERIAL_BYRAWTYPE", out List<M_Material> c_lstMat))
                    {
                        if (string.IsNullOrEmpty(vcode))
                        {
                            return Json(new { success = true, data = c_lstMat, message = "Get Material Success" });
                        }
                        else
                        {
                            M_Material m_Material = c_lstMat.Find(m => m.MaterialCode == vcode);

                            if (m_Material != null)
                            {
                                return Json(new { success = true, data = m_Material, message = "Get Material Success" });
                            }
                            else
                            {
                                return Json(new { success = false, data = string.Empty, message = "Material Not Found" });
                            }
                        }
                    }
                }

                TempData["MatTypeId"] = rawMatTypeId;
                TempData.Keep("MatTypeId");

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300),
                    SlidingExpiration = TimeSpan.FromSeconds(60),
                    Priority = CacheItemPriority.NeverRemove
                };

                using (var matBll = new MaterialBLL())
                {
                    var lstMat = await matBll.GetMaterialByRawType(rawMatTypeId, companyCode);

                    _cache.Set("CACHE_MASTER_MATERIAL_BYRAWTYPE", lstMat, options);

                    if (string.IsNullOrEmpty(vcode))
                    {
                        return Json(new { success = true, data = lstMat, message = "Get Material Success" });
                    }
                    else
                    {
                        M_Material m_Material = lstMat.Find(m => m.MaterialCode == vcode);

                        if (m_Material != null)
                        {
                            return Json(new { success = true, data = m_Material, message = "Get Material Success" });
                        }
                        else
                        {
                            return Json(new { success = false, data = string.Empty, message = "Material Not Found" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> GetLabelOption(int? arrId, int? lineNo)
        {
            try
            {
                using (var arrBll = new ArrivalBLL())
                {
                    var lstLabelOption = await arrBll.GetArrivalDetailSub(arrId, lineNo);

                    return Json(new { data = lstLabelOption });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateArrivalDetailSub(List<T_Arrival_Detail_Sub> lstArrDtlSub)
        {

            var arrId = lstArrDtlSub.FirstOrDefault().ArrivalId;
            var lineno = lstArrDtlSub.FirstOrDefault().DtlLineNo;

            var userId = await base.CurrentUserId();

            lstArrDtlSub.ForEach(u => {
                u.Created_By = userId;
            });

            ResultObject resultObj;

            try
            {
                using (var arrBll = new ArrivalBLL())
                {
                    resultObj = await arrBll.UpdateArrivalDetailSub(arrId, lineno, lstArrDtlSub);
                }

                return Json(new { success = true, data = lstArrDtlSub, message = "Update Success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = lstArrDtlSub, message = ex.Message });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateLabel(List<T_Arrival_Detail> lstArrDtl)
        {
            var userId = await base.CurrentUserId();
            ResultObject resultObj;

            try
            {
                using (var arrBll = new ArrivalBLL())
                {
                    resultObj = await arrBll.GenerateLabel(lstArrDtl, userId);
                }

                return Json(new { success = true, data = lstArrDtl, message = "Generate Success" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = lstArrDtl, message = ex.Message });
            }

        }
    }
}