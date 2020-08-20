using Maple2.AdminLTE.Bel;
using Maple2.AdminLTE.Dal;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maple2.AdminLTE.Bll
{
    public class ArrivalBLL : IDisposable
    {
        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool Disposing)
        {
            if (!IsDisposed)
            {
                if (Disposing)
                {

                }
            }

            IsDisposed = true;
        }

        ~ArrivalBLL()
        {
            this.Dispose(false);
        }

        #endregion

        #region Private Member

        private bool IsDisposed = false;
        private AppConfiguration appSetting;
        private DbContextOptions contextOptions;

        private string compCode = string.Empty;

        #endregion

        public ArrivalBLL()
        {
            appSetting = new AppConfiguration();

            contextOptions = new DbContextOptionsBuilder()
                            .UseMySql(appSetting.ConnectionString)
                            .Options;
        }

        public ArrivalBLL(string company_code)
        {
            appSetting = new AppConfiguration();

            contextOptions = new DbContextOptionsBuilder()
                            .UseMySql(appSetting.ConnectionString)
                            .Options;

            this.compCode = company_code;
        }



        #region Method Member

        public async Task<List<T_Arrival_Header>> GetArrival(int? id)
        {
            try
            {
                using (var context = new TransactionDbContext(contextOptions))
                {
                    MySqlParameter[] sqlParams = new MySqlParameter[] {
                                             new MySqlParameter("strId", id)
                    };

                    var objList = await context.Query<T_Arrival_HeaderObj>().FromSql("call sp_arrival_hdr_get(?)", parameters: sqlParams).ToListAsync();

                    return objList.ConvertAll(a => new T_Arrival_Header
                    {
                        Id = a.Id,
                        ArrivalNo = a.ArrivalNo,
                        ArrivalDate = a.ArrivalDate,
                        RawMatTypeId = a.RawMatTypeId,
                        RawMatTypeName = a.RawMatTypeName,
                        VendorId = a.VendorId,
                        VendorCode = a.VendorCode,
                        VendorName = a.VendorName,
                        VendorAddress = a.VendorAddress,
                        ArrivalTypeId = a.ArrivalTypeId,
                        ArrivalTypeName = a.ArrivalTypeName,
                        PurchaseOrderNo = a.PurchaseOrderNo,
                        DocRefNo = a.DocRefNo,
                        DocRefDate = a.DocRefDate,
                        ArrivalRemark = a.ArrivalRemark,
                        CompanyCode = a.CompanyCode,
                        Is_Active = a.Is_Active,
                        Created_Date = a.Created_Date,
                        Created_By = a.Created_By,
                        Updated_Date = a.Updated_Date,
                        Updated_By = a.Updated_By
                    });
                }
                   
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<T_Arrival_Header>> GetArrival(string arrno, string docno, int? arrType, int? rawMatType, DateTime? arrdateF, DateTime? arrdateT, DateTime? docdateF, DateTime? docdateT)
        {
            try
            {
                using (var context = new TransactionDbContext(contextOptions))
                {
                    MySqlParameter[] sqlParams = new MySqlParameter[] {
                                             new MySqlParameter("strArrivalNo", arrno),
                                             new MySqlParameter("strDocumentNo", docno),
                                             new MySqlParameter("strArrivalTypeId", arrType),
                                             new MySqlParameter("strRawMatTypeId", rawMatType),
                                             new MySqlParameter("strArrivalDateF", arrdateF),
                                             new MySqlParameter("strArrivalDateT", arrdateT),
                                             new MySqlParameter("strDocRefDateF", docdateF),
                                             new MySqlParameter("strDocRefDateT", docdateT),
                                             new MySqlParameter("strCompanyCode", this.compCode),

                    };

                    var objList = await context.Query<T_Arrival_HeaderObj>().FromSql("call sp_arrival_hdr_gets(?, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams).ToListAsync();

                    return objList.ConvertAll(a => new T_Arrival_Header
                    {
                        Id = a.Id,
                        ArrivalNo = a.ArrivalNo,
                        ArrivalDate = a.ArrivalDate,
                        RawMatTypeId = a.RawMatTypeId,
                        RawMatTypeName = a.RawMatTypeName,
                        VendorId = a.VendorId,
                        VendorCode = a.VendorCode,
                        VendorName = a.VendorName,
                        VendorAddress = a.VendorAddress,
                        ArrivalTypeId = a.ArrivalTypeId,
                        ArrivalTypeName = a.ArrivalTypeName,
                        PurchaseOrderNo = a.PurchaseOrderNo,
                        DocRefNo = a.DocRefNo,
                        DocRefDate = a.DocRefDate,
                        ArrivalRemark = a.ArrivalRemark,
                        CompanyCode = a.CompanyCode,
                        Is_Active = a.Is_Active,
                        Created_Date = a.Created_Date,
                        Created_By = a.Created_By,
                        Updated_Date = a.Updated_Date,
                        Updated_By = a.Updated_By
                    });
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResultObject> InsertArrival(T_Arrival_Header arr)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = arr };

            using (var context = new TransactionDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    //new MySqlParameter("strArrivalNo", arr.ArrivalNo),
                                    new MySqlParameter("strArrivalDate", arr.ArrivalDate),
                                    new MySqlParameter("strRawMatTypeId", arr.RawMatTypeId),
                                    new MySqlParameter("strVendorId", arr.VendorId),
                                    new MySqlParameter("strArrivalTypeId", arr.ArrivalTypeId),
                                    new MySqlParameter("strPurchaseOrderNo", arr.PurchaseOrderNo),
                                    new MySqlParameter("strDocRefNo", arr.DocRefNo),
                                    new MySqlParameter("strDocRefDate", arr.DocRefDate),
                                    new MySqlParameter("strArrivalRemark", arr.ArrivalRemark),
                                    new MySqlParameter("strCompanyCode", arr.CompanyCode),
                                    new MySqlParameter("strIs_Active", arr.Is_Active),
                                    new MySqlParameter("strCreated_By", arr.Created_By)
                        };

                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_arrival_hdr_insert(@`strId`, @`strArrivalNo`, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

                        //new product after insert.
                        var newArr = context.ArrivalHdr.FromSql("SELECT * FROM t_arrival_hdr WHERE Id = @`strId`;").ToListAsync();
                        resultObj.ObjectValue = newArr.Result[0];

                        if (arr.ArrivalDetails.Count > 0)
                        {
                            MySqlParameter[] sqlParams2;

                            foreach (T_Arrival_Detail arrdtl in arr.ArrivalDetails)
                            {
                                switch (arrdtl.RecordFlag)
                                {
                                    case 0:
                                        break;
                                    case 1:
                                        break;
                                    case 2:

                                        sqlParams2 = new MySqlParameter[] {
                                            new MySqlParameter("strArrivalId", newArr.Result[0].Id),
                                            new MySqlParameter("strLineNo", arrdtl.LineNo),
                                            new MySqlParameter("strPoLineNo", arrdtl.PoLineNo),
                                            new MySqlParameter("strMaterialId", arrdtl.MaterialId),
                                            new MySqlParameter("strMaterialCode", arrdtl.MaterialCode),
                                            new MySqlParameter("strMaterialName", arrdtl.MaterialName),
                                            new MySqlParameter("strMaterialDesc", arrdtl.MaterialDesc),
                                            new MySqlParameter("strOrderQty", arrdtl.OrderQty),
                                            new MySqlParameter("strRecvQty", arrdtl.RecvQty),
                                            new MySqlParameter("strLotNo", arrdtl.LotNo),
                                            new MySqlParameter("strLotDate", arrdtl.LotDate),
                                            new MySqlParameter("strDetailRemark", arrdtl.DetailRemark),
                                            new MySqlParameter("strGenLabelStatus", arrdtl.GenLabelStatus),
                                            new MySqlParameter("strNoOfLabel", arrdtl.NoOfLabel),
                                            new MySqlParameter("strCompanyCode", arrdtl.CompanyCode),
                                            new MySqlParameter("strIs_Active", arrdtl.Is_Active),
                                            new MySqlParameter("strCreated_By", arrdtl.Created_By)
                                        };

                                        resultObj.RowAffected += await context.Database.ExecuteSqlCommandAsync("call sp_arrival_dtl_insert(@`strId`, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams2);

                                        break;
                                    default:
                                        break;
                                            
                                }
                            }
                        }

                        transaction.Commit();

                        return resultObj;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            
        }

        public async Task<ResultObject> UpdateArrival(T_Arrival_Header arr)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = arr };

            using (var context = new TransactionDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(arr).State = EntityState.Modified;

                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", arr.Id),
                                    new MySqlParameter("strArrivalNo", arr.ArrivalNo),
                                    new MySqlParameter("strArrivalDate", arr.ArrivalDate),
                                    new MySqlParameter("strRawMatTypeId", arr.RawMatTypeId),
                                    new MySqlParameter("strVendorId", arr.VendorId),
                                    new MySqlParameter("strArrivalTypeId", arr.ArrivalTypeId),
                                    new MySqlParameter("strPurchaseOrderNo", arr.PurchaseOrderNo),
                                    new MySqlParameter("strDocRefNo", arr.DocRefNo),
                                    new MySqlParameter("strDocRefDate", arr.DocRefDate),
                                    new MySqlParameter("strArrivalRemark", arr.ArrivalRemark),
                                    new MySqlParameter("strCompanyCode", arr.CompanyCode),
                                    new MySqlParameter("strIs_Active", arr.Is_Active),
                                    new MySqlParameter("strUpdated_By", arr.Updated_By)
                        };

                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_arrival_hdr_update(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

                        if (arr.ArrivalDetails.Count > 0)
                        {
                            MySqlParameter[] sqlParams2;

                            foreach (T_Arrival_Detail arrdtl in arr.ArrivalDetails)
                            {
                                switch (arrdtl.RecordFlag)
                                {
                                    case 0: //delete row

                                        sqlParams2 = new MySqlParameter[] {
                                            new MySqlParameter("strId", arrdtl.Id),
                                            new MySqlParameter("strArrivalId", arrdtl.ArrivalId),
                                            new MySqlParameter("strLineNo", arrdtl.LineNo),
                                            new MySqlParameter("strPoLineNo", arrdtl.PoLineNo),
                                            new MySqlParameter("strMaterialId", arrdtl.MaterialId),
                                            new MySqlParameter("strCompanyCode", arrdtl.CompanyCode),
                                            new MySqlParameter("strDelete_By", arrdtl.Created_By)
                                        };

                                        resultObj.RowAffected += await context.Database.ExecuteSqlCommandAsync("call sp_arrival_dtl_delete(?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams2);

                                        break;
                                    case 1: //no change (only update line no)

                                        sqlParams2 = new MySqlParameter[] {
                                            new MySqlParameter("strId", arrdtl.Id),
                                            new MySqlParameter("strLineNo", arrdtl.LineNo)
                                        };

                                        resultObj.RowAffected += await context.Database.ExecuteSqlCommandAsync("call sp_arrival_dtl_updateline(?, ?)", parameters: sqlParams2);

                                        break;
                                    case 2: //new row

                                        sqlParams2 = new MySqlParameter[] {
                                            new MySqlParameter("strArrivalId", arr.Id),
                                            new MySqlParameter("strLineNo", arrdtl.LineNo),
                                            new MySqlParameter("strPoLineNo", arrdtl.PoLineNo),
                                            new MySqlParameter("strMaterialId", arrdtl.MaterialId),
                                            new MySqlParameter("strMaterialCode", arrdtl.MaterialCode),
                                            new MySqlParameter("strMaterialName", arrdtl.MaterialName),
                                            new MySqlParameter("strMaterialDesc", arrdtl.MaterialDesc),
                                            new MySqlParameter("strOrderQty", arrdtl.OrderQty),
                                            new MySqlParameter("strRecvQty", arrdtl.RecvQty),
                                            new MySqlParameter("strLotNo", arrdtl.LotNo),
                                            new MySqlParameter("strLotDate", arrdtl.LotDate),
                                            new MySqlParameter("strDetailRemark", arrdtl.DetailRemark),
                                            new MySqlParameter("strGenLabelStatus", arrdtl.GenLabelStatus),
                                            new MySqlParameter("strNoOfLabel", arrdtl.NoOfLabel),
                                            new MySqlParameter("strCompanyCode", arrdtl.CompanyCode),
                                            new MySqlParameter("strIs_Active", arrdtl.Is_Active),
                                            new MySqlParameter("strCreated_By", arr.Updated_By)
                                        };

                                        resultObj.RowAffected += await context.Database.ExecuteSqlCommandAsync("call sp_arrival_dtl_insert(@`strId`, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams2);

                                        break;
                                    case 3: //Edit Qty

                                        sqlParams2 = new MySqlParameter[] {
                                            new MySqlParameter("strId", arrdtl.Id),
                                            new MySqlParameter("strArrivalId", arrdtl.ArrivalId),
                                            new MySqlParameter("strLineNo", arrdtl.LineNo),
                                            new MySqlParameter("strPoLineNo", arrdtl.PoLineNo),
                                            new MySqlParameter("strMaterialId", arrdtl.MaterialId),
                                            new MySqlParameter("strOrderQty", arrdtl.OrderQty),
                                            new MySqlParameter("strCompanyCode", arrdtl.CompanyCode),
                                            new MySqlParameter("strCreated_By", arrdtl.Created_By)
                                        };

                                        resultObj.RowAffected += await context.Database.ExecuteSqlCommandAsync("call sp_arrival_dtl_update(?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams2);

                                        break;
                                    default:
                                        break;

                                }
                            }
                        }

                        transaction.Commit();

                        return resultObj;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

        }

        public async Task<List<T_Arrival_Detail>> GetArrivalDetails(int? arrId)
        {
            try
            {
                using (var context = new TransactionDbContext(contextOptions))
                {
                    MySqlParameter[] sqlParams = new MySqlParameter[] {
                        new MySqlParameter("strArrivalId", arrId)
                    };

                    var objList = await context.Query<T_Arrival_DetailObj>().FromSql("call sp_arrival_dtl_get(?)", parameters: sqlParams).ToListAsync();

                    return objList.ConvertAll(a => new T_Arrival_Detail
                    {
                        Id = a.Id,
                        ArrivalId = a.ArrivalId,
                        LineNo = a.LineNo,
                        PoLineNo = a.PoLineNo,
                        MaterialId = a.MaterialId,
                        MaterialCode = a.MaterialCode,
                        MaterialName = a.MaterialName,
                        MaterialDesc = a.MaterialDesc,
                        OrderQty = a.OrderQty,
                        RecvQty = a.RecvQty,
                        LotNo = a.LotNo,
                        LotDate = a.LotDate,
                        DetailRemark = a.DetailRemark,
                        GenLabelStatus = a.GenLabelStatus,
                        NoOfLabel = a.NoOfLabel,
                        CompanyCode = a.CompanyCode,
                        RecordFlag = a.RecordFlag,
                        Is_Active = a.Is_Active,
                        Created_Date = a.Created_Date,
                        Created_By = a.Created_By,
                        Updated_Date = a.Updated_Date,
                        Updated_By = a.Updated_By,
                        PackageStdQty = a.PackageStdQty
                    });
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<T_Arrival_Detail_Sub>> GetArrivalDetailSub(int? arrDtlId, int? arrDtlLine)
        {
            try
            {
                using (var context = new TransactionDbContext(contextOptions))
                {
                    MySqlParameter[] sqlParams = new MySqlParameter[] {
                                             new MySqlParameter("strArrivalId", arrDtlId),
                                             new MySqlParameter("strLineNo", arrDtlLine)
                    };

                    var objList = await context.Query<T_Arrival_Detail_SubObj>().FromSql("call sp_arrival_dtl_sub_get(?, ?)", parameters: sqlParams).ToListAsync();

                    return objList.ConvertAll(a => new T_Arrival_Detail_Sub
                    {
                        Id = a.Id,
                        ArrivalId = a.ArrivalId,
                        DtlLineNo = a.DtlLineNo,
                        SubLineNo = a.SubLineNo,
                        MaterialId = a.MaterialId,
                        NoOfLabel = a.NoOfLabel,
                        LabelQty = a.LabelQty,
                        TotalQty = a.TotalQty,
                        SubDetail = a.SubDetail,
                        CompanyCode = a.CompanyCode,
                        RecordFlag = a.RecordFlag,
                        Is_Active = a.Is_Active,
                        Created_Date = a.Created_Date,
                        Created_By = a.Created_By,
                        Updated_Date = a.Updated_Date,
                        Updated_By = a.Updated_By
                    });
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResultObject> UpdateArrivalDetailSub(int? arrId, int? arrDtlLine, List<T_Arrival_Detail_Sub> arrDtlSubs)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = arrDtlSubs };

            using (var context = new TransactionDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams2;

                        foreach (T_Arrival_Detail_Sub arrdtlsub in arrDtlSubs)
                        {
                            switch (arrdtlsub.RecordFlag)
                            {
                                case 0: //delete row

                                    sqlParams2 = new MySqlParameter[] {
                                        new MySqlParameter("strId", arrdtlsub.Id),
                                        new MySqlParameter("strArrivalId", arrdtlsub.ArrivalId),
                                        new MySqlParameter("strDtlLineNo", arrdtlsub.DtlLineNo),
                                        new MySqlParameter("strSubLineNo", arrdtlsub.SubLineNo),
                                        new MySqlParameter("strMaterialId", arrdtlsub.MaterialId),
                                        new MySqlParameter("strLabelQty", arrdtlsub.LabelQty),
                                        new MySqlParameter("strNoOfLabel", arrdtlsub.NoOfLabel),
                                        new MySqlParameter("strSubDetail", arrdtlsub.SubDetail),
                                        new MySqlParameter("strCompanyCode", arrdtlsub.CompanyCode),
                                        new MySqlParameter("strDelete_By", arrdtlsub.Created_By)
                                    };

                                    resultObj.RowAffected += await context.Database.ExecuteSqlCommandAsync("call sp_arrival_dtl_sub_delete(?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams2);

                                    break;
                                case 1: //no change
                                    break;
                                case 2: //new row

                                    sqlParams2 = new MySqlParameter[] {
                                        new MySqlParameter("strArrivalId", arrdtlsub.ArrivalId),
                                        new MySqlParameter("strDtlLineNo", arrdtlsub.DtlLineNo),
                                        new MySqlParameter("strSubLineNo", arrdtlsub.SubLineNo),
                                        new MySqlParameter("strMaterialId", arrdtlsub.MaterialId),
                                        new MySqlParameter("strLabelQty", arrdtlsub.LabelQty),
                                        new MySqlParameter("strNoOfLabel", arrdtlsub.NoOfLabel),
                                        new MySqlParameter("strSubDetail", arrdtlsub.SubDetail),
                                        new MySqlParameter("strCompanyCode", arrdtlsub.CompanyCode),
                                        new MySqlParameter("strIs_Active", arrdtlsub.Is_Active),
                                        new MySqlParameter("strCreated_By", arrdtlsub.Created_By)
                                    };

                                    resultObj.RowAffected += await context.Database.ExecuteSqlCommandAsync("call sp_arrival_dtl_sub_insert(@`strId`, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams2);

                                    break;
                                case 3: //Edit Qty

                                    sqlParams2 = new MySqlParameter[] {
                                        new MySqlParameter("strId", arrdtlsub.Id),
                                        new MySqlParameter("strArrivalId", arrdtlsub.ArrivalId),
                                        new MySqlParameter("strDtlLineNo", arrdtlsub.DtlLineNo),
                                        new MySqlParameter("strSubLineNo", arrdtlsub.SubLineNo),
                                        new MySqlParameter("strMaterialId", arrdtlsub.MaterialId),
                                        new MySqlParameter("strLabelQty", arrdtlsub.LabelQty),
                                        new MySqlParameter("strNoOfLabel", arrdtlsub.NoOfLabel),
                                        new MySqlParameter("strSubDetail", arrdtlsub.SubDetail),
                                        new MySqlParameter("strCompanyCode", arrdtlsub.CompanyCode),
                                        new MySqlParameter("strIs_Active", arrdtlsub.Is_Active),
                                        new MySqlParameter("strUpdated_By", arrdtlsub.Created_By)
                                    };

                                    resultObj.RowAffected += await context.Database.ExecuteSqlCommandAsync("call sp_arrival_dtl_sub_update(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams2);

                                    break;
                                default:
                                    break;

                            }
                        }

                        //3. UPDATE ARRIVAL DTL LINE NO (RERUNNING).
                        MySqlParameter[] sqlParams3 = new MySqlParameter[] {
                                    new MySqlParameter("strArrivalId", arrId),
                                    new MySqlParameter("strDtlLineNo", arrDtlLine)
                        };

                        resultObj.RowAffected += await context.Database.ExecuteSqlCommandAsync("call sp_arrival_dtl_sub_reline(?, ?)", parameters: sqlParams3);

                        transaction.Commit();

                        return resultObj;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

        }

        public async Task<ResultObject> GenerateLabel(List<T_Arrival_Detail> lstArrDtl, int? userid)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = lstArrDtl };

            using (var context = new TransactionDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams;

                        foreach (T_Arrival_Detail arrDtl in lstArrDtl)
                        {
                            sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strArrivalId", arrDtl.ArrivalId),
                                    new MySqlParameter("strLineNo", arrDtl.LineNo),
                                    new MySqlParameter("strMaterialId", arrDtl.MaterialId),
                                    new MySqlParameter("strCreated_By", userid)
                            };

                            resultObj.RowAffected += await context.Database.ExecuteSqlCommandAsync("call sp_generate_material_card(?, ?, ?, ?)", parameters: sqlParams);
                        }

                        transaction.Commit();

                        return resultObj;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

        }

        #endregion
    }
}
