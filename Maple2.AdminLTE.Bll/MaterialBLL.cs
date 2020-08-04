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
    public class MaterialBLL : IDisposable
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

        ~MaterialBLL()
        {
            this.Dispose(false);
        }

        #endregion

        #region Private Member

        private bool IsDisposed = false;
        private AppConfiguration appSetting;
        private DbContextOptions contextOptions;

        #endregion

        public MaterialBLL()
        {
            appSetting = new AppConfiguration();

            contextOptions = new DbContextOptionsBuilder()
                            .UseMySql(appSetting.ConnectionString)
                            .Options;
        }

        #region Method Member

        public async Task<List<M_Material>> GetMaterial(int? id)
        {
            try
            {
                using (var context = new MasterDbContext(contextOptions))
                {
                    MySqlParameter[] sqlParams = new MySqlParameter[] {
                                                 new MySqlParameter("strId", id)
                    };

                    var objList = await context.Query<M_MaterialObj>().FromSql("call sp_material_get(?)", parameters: sqlParams).ToListAsync();

                    return objList.ConvertAll(m => new M_Material
                    {
                        Id = m.Id,
                        MaterialCode = m.MaterialCode,
                        MaterialName = m.MaterialName,
                        MaterialDesc1 = m.MaterialDesc1,
                        MaterialDesc2 = m.MaterialDesc2,
                        RawMatTypeId = m.RawMatTypeId,
                        RawMatType = m.RawMatType,
                        UnitId = m.UnitId,
                        Unit = m.Unit,
                        PackageStdQty = m.PackageStdQty,
                        WarehouseId = m.WarehouseId,
                        Warehouse = m.Warehouse,
                        LocationId = m.LocationId,
                        Location = m.Location,
                        MaterialImagePath = m.MaterialImagePath,
                        CompanyCode = m.CompanyCode,
                        Is_Active = m.Is_Active,
                        Created_Date = m.Created_Date,
                        Created_By = m.Created_By,
                        Updated_Date = m.Updated_Date,
                        Updated_By = m.Updated_By
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResultObject> InsertMaterial(M_Material mat)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = mat };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strMaterialCode", mat.MaterialCode),
                                    new MySqlParameter("strMaterialName", mat.MaterialName),
                                    new MySqlParameter("strMaterialDesc1", mat.MaterialDesc1),
                                    new MySqlParameter("strMaterialDesc2", mat.MaterialDesc2),
                                    new MySqlParameter("strRawMatTypeId", mat.RawMatTypeId),
                                    new MySqlParameter("strUnitId", mat.UnitId),
                                    new MySqlParameter("strPackageStdQty", mat.PackageStdQty),
                                    new MySqlParameter("strWarehouseId", mat.WarehouseId),
                                    new MySqlParameter("strLocationId", mat.LocationId),
                                    new MySqlParameter("strMaterialImagePath", mat.MaterialImagePath),
                                    new MySqlParameter("strCompanyCode", mat.CompanyCode),
                                    new MySqlParameter("strIs_Active", mat.Is_Active),
                                    new MySqlParameter("strCreated_By", mat.Created_By)
                        };

                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_material_insert(@`strId`, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

                        //new department after insert.
                        var newMat = context.Material.FromSql("SELECT * FROM m_material WHERE Id = @`strId`;").ToListAsync();
                        resultObj.ObjectValue = newMat.Result[0];

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

        public async Task<int> BulkInsertMaterial(List<M_Material> lstMat)
        {
            int rowaffected = -1;

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (M_Material mat in lstMat)
                        {
                            MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strMaterialCode", mat.MaterialCode),
                                    new MySqlParameter("strMaterialName", mat.MaterialName),
                                    new MySqlParameter("strMaterialDesc1", mat.MaterialDesc1),
                                    new MySqlParameter("strMaterialDesc2", mat.MaterialDesc2),
                                    new MySqlParameter("strRawMatTypeId", mat.RawMatTypeId),
                                    new MySqlParameter("strUnitId", mat.UnitId),
                                    new MySqlParameter("strPackageStdQty", mat.PackageStdQty),
                                    new MySqlParameter("strWarehouseId", mat.WarehouseId),
                                    new MySqlParameter("strLocationId", mat.LocationId),
                                    new MySqlParameter("strMaterialImagePath", mat.MaterialImagePath),
                                    new MySqlParameter("strCompanyCode", mat.CompanyCode),
                                    new MySqlParameter("strIs_Active", mat.Is_Active),
                                    new MySqlParameter("strCreated_By", mat.Created_By)
                            };

                            rowaffected += await context.Database.ExecuteSqlCommandAsync("call sp_material_upload(@`strId`, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);
                        }

                        transaction.Commit();

                        return rowaffected;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<ResultObject> UpdateMaterial(M_Material mat)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = mat };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(mat).State = EntityState.Modified;

                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", mat.Id),
                                    new MySqlParameter("strMaterialCode", mat.MaterialCode),
                                    new MySqlParameter("strMaterialName", mat.MaterialName),
                                    new MySqlParameter("strMaterialDesc1", mat.MaterialDesc1),
                                    new MySqlParameter("strMaterialDesc2", mat.MaterialDesc2),
                                    new MySqlParameter("strRawMatTypeId", mat.RawMatTypeId),
                                    new MySqlParameter("strUnitId", mat.UnitId),
                                    new MySqlParameter("strPackageStdQty", mat.PackageStdQty),
                                    new MySqlParameter("strWarehouseId", mat.WarehouseId),
                                    new MySqlParameter("strLocationId", mat.LocationId),
                                    new MySqlParameter("strMaterialImagePath", mat.MaterialImagePath),
                                    new MySqlParameter("strCompanyCode", mat.CompanyCode),
                                    new MySqlParameter("strIs_Active", mat.Is_Active),
                                    new MySqlParameter("strUpdated_By", mat.Updated_By)
                        };

                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_material_update(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

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

        public async Task<ResultObject> DeleteMaterial(M_Material mat)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = mat };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", mat.Id),
                                    new MySqlParameter("strDelete_By", mat.Updated_By)
                        };

                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_material_delete( ?, ?)", parameters: sqlParams);

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

        public async Task<List<M_Material>> GetMaterialByRawType(int? rawMatTypeId, string comp)
        {
            try
            {
                using (var context = new MasterDbContext(contextOptions))
                {
                    MySqlParameter[] sqlParams = new MySqlParameter[] {
                                             new MySqlParameter("strRawMatTypeId", rawMatTypeId),
                                             new MySqlParameter("strCompanyCode", comp)
                    };

                    var objList = await context.Query<M_MaterialObj>().FromSql("call sp_material_rawmattype_get(?, ?)", parameters: sqlParams).ToListAsync();

                    return objList.ConvertAll(m => new M_Material
                    {
                        Id = m.Id,
                        MaterialCode = m.MaterialCode,
                        MaterialName = m.MaterialName,
                        MaterialDesc1 = m.MaterialDesc1,
                        MaterialDesc2 = m.MaterialDesc2,
                        RawMatTypeId = m.RawMatTypeId,
                        RawMatType = m.RawMatType,
                        UnitId = m.UnitId,
                        Unit = m.Unit,
                        PackageStdQty = m.PackageStdQty,
                        WarehouseId = m.WarehouseId,
                        Warehouse = m.Warehouse,
                        LocationId = m.LocationId,
                        Location = m.Location,
                        MaterialImagePath = m.MaterialImagePath,
                        CompanyCode = m.CompanyCode,
                        Is_Active = m.Is_Active,
                        Created_Date = m.Created_Date,
                        Created_By = m.Created_By,
                        Updated_Date = m.Updated_Date,
                        Updated_By = m.Updated_By
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
