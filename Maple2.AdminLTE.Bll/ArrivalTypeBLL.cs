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
    public class ArrivalTypeBLL : IDisposable
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

        ~ArrivalTypeBLL()
        {
            this.Dispose(false);
        }

        #endregion

        #region Private Member

        private bool IsDisposed = false;
        private AppConfiguration appSetting;
        private DbContextOptions<MasterDbContext> contextOptions;

        #endregion

        public ArrivalTypeBLL()
        {
            appSetting = new AppConfiguration();

            contextOptions = new DbContextOptionsBuilder<MasterDbContext>()
                            .UseMySql(appSetting.ConnectionString)
                            .Options;
        }

        #region Method Member

        public async Task<List<M_ArrivalType>> GetArrivalType(int? id)
        {
            try
            {
                using (var context = new MasterDbContext(contextOptions))
                {
                    MySqlParameter[] sqlParams = new MySqlParameter[] {
                                             new MySqlParameter("strId", id)
                    };

                    return await context.ArrivalType.FromSql("call sp_arrivaltype_get(?)", parameters: sqlParams).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResultObject> InsertArrivalType(M_ArrivalType arrType)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = arrType };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strArrivalTypeCode", arrType.ArrivalTypeCode),
                                    new MySqlParameter("strArrivalTypeName", arrType.ArrivalTypeName),
                                    new MySqlParameter("strArrivalTypeDesc", arrType.ArrivalTypeDesc),
                                    new MySqlParameter("strCompanyCode", arrType.CompanyCode),
                                    new MySqlParameter("strIs_Active", arrType.Is_Active),
                                    new MySqlParameter("strCreated_By", arrType.Created_By)
                        };

                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_arrivaltype_insert(@`strId`, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

                        //new department after insert.
                        var newArrType = context.ArrivalType.FromSql("SELECT * FROM m_arrivaltype WHERE Id = @`strId`;").ToListAsync();
                        resultObj.ObjectValue = newArrType.Result[0];

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

        public async Task<ResultObject> UpdateArrivalType(M_ArrivalType arrType)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = arrType };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(arrType).State = EntityState.Modified;

                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", arrType.Id),
                                    new MySqlParameter("strArrivalTypeCode", arrType.ArrivalTypeCode),
                                    new MySqlParameter("strArrivalTypeName", arrType.ArrivalTypeName),
                                    new MySqlParameter("strArrivalTypeDesc", arrType.ArrivalTypeDesc),
                                    new MySqlParameter("strCompanyCode", arrType.CompanyCode),
                                    new MySqlParameter("strIs_Active", arrType.Is_Active),
                                    new MySqlParameter("strCreated_By", arrType.Updated_By)
                        };


                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_arrivaltype_update(?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

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

        public async Task<ResultObject> DeleteArrivalType(M_ArrivalType arrType)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = arrType };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", arrType.Id),
                                    new MySqlParameter("strDelete_By", arrType.Updated_By)
                        };


                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_arrivaltype_delete( ?, ?)", parameters: sqlParams);

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
