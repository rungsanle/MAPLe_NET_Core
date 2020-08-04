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
    public class MaterialTypeBLL : IDisposable
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
                    //db.Dispose();
                }
            }

            IsDisposed = true;
        }

        ~MaterialTypeBLL()
        {
            this.Dispose(false);
        }

        #endregion

        #region Private Member

        private bool IsDisposed = false;
        private AppConfiguration appSetting;
        private DbContextOptions contextOptions;

        #endregion

        public MaterialTypeBLL()
        {
            appSetting = new AppConfiguration();

            contextOptions = new DbContextOptionsBuilder()
                            .UseMySql(appSetting.ConnectionString)
                            .Options;
        }

        #region Method Member

        public async Task<ResultObject> DeleteDepartment(M_Department dept)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = dept };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", dept.Id),
                                    new MySqlParameter("strDelete_By", dept.Updated_By)
                        };


                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_department_delete( ?, ?)", parameters: sqlParams);

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

        public async Task<List<M_MaterialType>> GetMaterialType(int? id)
        {
            try
            {
                using (var context = new MasterDbContext(contextOptions))
                {
                    MySqlParameter[] sqlParams = new MySqlParameter[] {
                                             new MySqlParameter("strId", id)
                    };

                    return await context.MaterialType.FromSql("call sp_materialtype_get(?)", parameters: sqlParams).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResultObject> InsertMaterialType(M_MaterialType matType)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = matType };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strMatTypeCode", matType.MatTypeCode),
                                    new MySqlParameter("strMatTypeName", matType.MatTypeName),
                                    new MySqlParameter("strMatTypeDesc", matType.MatTypeDesc),
                                    new MySqlParameter("strCompanyCode", matType.CompanyCode),
                                    new MySqlParameter("strIs_Active", matType.Is_Active),
                                    new MySqlParameter("strCreated_By", matType.Created_By)
                        };

                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_materialtype_insert(@`strId`, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

                        //new department after insert.
                        var newDept = context.MaterialType.FromSql("SELECT * FROM m_materialtype WHERE Id = @`strId`;").ToListAsync();
                        resultObj.ObjectValue = newDept.Result[0];

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

        public async Task<int> BulkInsertMaterialType(List<M_MaterialType> lstMatType)
        {
            int rowaffected = -1;

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (M_MaterialType matType in lstMatType)
                        {
                            MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strMatTypeCode", matType.MatTypeCode),
                                    new MySqlParameter("strMatTypeName", matType.MatTypeName),
                                    new MySqlParameter("strMatTypeDesc", matType.MatTypeDesc),
                                    new MySqlParameter("strCompanyCode", matType.CompanyCode),
                                    new MySqlParameter("strIs_Active", matType.Is_Active),
                                    new MySqlParameter("strCreated_By", matType.Created_By)
                            };

                            rowaffected += await context.Database.ExecuteSqlCommandAsync("call sp_materialtype_insert(@`strId`, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);
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

        public async Task<ResultObject> UpdateMaterialType(M_MaterialType matType)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = matType };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(matType).State = EntityState.Modified;

                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", matType.Id),
                                    new MySqlParameter("strMatTypeCode", matType.MatTypeCode),
                                    new MySqlParameter("strMatTypeName", matType.MatTypeName),
                                    new MySqlParameter("strMatTypeDesc", matType.MatTypeDesc),
                                    new MySqlParameter("strCompanyCode", matType.CompanyCode),
                                    new MySqlParameter("strIs_Active", matType.Is_Active),
                                    new MySqlParameter("strCreated_By", matType.Updated_By)
                        };


                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_materialtype_update(?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

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

        public async Task<ResultObject> DeleteMaterialType(M_MaterialType matType)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = matType };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", matType.Id),
                                    new MySqlParameter("strDelete_By", matType.Updated_By)
                        };


                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_materialtype_delete( ?, ?)", parameters: sqlParams);

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
