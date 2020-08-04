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
    public class RawMaterialTypeBLL : IDisposable
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

        ~RawMaterialTypeBLL()
        {
            this.Dispose(false);
        }

        #endregion

        #region Private Member

        private bool IsDisposed = false;
        private AppConfiguration appSetting;
        private DbContextOptions contextOptions;

        #endregion

        public RawMaterialTypeBLL()
        {
            appSetting = new AppConfiguration();

            contextOptions = new DbContextOptionsBuilder()
                            .UseMySql(appSetting.ConnectionString)
                            .Options;
        }

        #region Method Member

        public async Task<List<M_RawMaterialType>> GetRawMaterialType(int? id)
        {
            try
            {
                using (var context = new MasterDbContext(contextOptions))
                {
                    MySqlParameter[] sqlParams = new MySqlParameter[] {
                                             new MySqlParameter("strId", id)
                    };

                    return await context.RawMaterialType.FromSql("call sp_rawmaterialtype_get(?)", parameters: sqlParams).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResultObject> InsertRawMaterialType(M_RawMaterialType rawmatType)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = rawmatType };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strRawMatTypeCode", rawmatType.RawMatTypeCode),
                                    new MySqlParameter("strRawMatTypeName", rawmatType.RawMatTypeName),
                                    new MySqlParameter("strRawMatTypeDesc", rawmatType.RawMatTypeDesc),
                                    new MySqlParameter("strCompanyCode", rawmatType.CompanyCode),
                                    new MySqlParameter("strIs_Active", rawmatType.Is_Active),
                                    new MySqlParameter("strCreated_By", rawmatType.Created_By)
                        };

                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_rawmaterialtype_insert(@`strId`, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

                        //new department after insert.
                        var newDept = context.RawMaterialType.FromSql("SELECT * FROM m_rawmaterialtype WHERE Id = @`strId`;").ToListAsync();
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

        public async Task<ResultObject> UpdateRawMaterialType(M_RawMaterialType rawmatType)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = rawmatType };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(rawmatType).State = EntityState.Modified;

                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", rawmatType.Id),
                                    new MySqlParameter("strRawMatTypeCode", rawmatType.RawMatTypeCode),
                                    new MySqlParameter("strRawMatTypeName", rawmatType.RawMatTypeName),
                                    new MySqlParameter("strRawMatTypeDesc", rawmatType.RawMatTypeDesc),
                                    new MySqlParameter("strCompanyCode", rawmatType.CompanyCode),
                                    new MySqlParameter("strIs_Active", rawmatType.Is_Active),
                                    new MySqlParameter("strCreated_By", rawmatType.Updated_By)
                        };

                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_rawmaterialtype_update(?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

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

        public async Task<ResultObject> DeleteRawMaterialType(M_RawMaterialType rawmatType)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = rawmatType };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", rawmatType.Id),
                                    new MySqlParameter("strDelete_By", rawmatType.Updated_By)
                        };


                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_rawmaterialtype_delete( ?, ?)", parameters: sqlParams);

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
