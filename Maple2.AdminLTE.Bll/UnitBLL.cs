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
    public class UnitBLL : IDisposable
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

        ~UnitBLL()
        {
            this.Dispose(false);
        }

        #endregion

        #region Private Member

        private bool IsDisposed = false;
        private AppConfiguration appSetting;
        private DbContextOptions contextOptions;

        #endregion

        public UnitBLL()
        {
            appSetting = new AppConfiguration();

            contextOptions = new DbContextOptionsBuilder()
                            .UseMySql(appSetting.ConnectionString)
                            .Options;
        }

        #region Method Member

        public async Task<List<M_Unit>> GetUnit(int? id)
        {
            try
            {
                using (var context = new MasterDbContext(contextOptions))
                {
                    MySqlParameter[] sqlParams = new MySqlParameter[] {
                                        new MySqlParameter("strId", id)
                    };

                    return await context.Unit.FromSql("call sp_unit_get(?)", parameters: sqlParams).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResultObject> InsertUnit(M_Unit unit)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = unit };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strUnitCode", unit.UnitCode),
                                    new MySqlParameter("strUnitName", unit.UnitName),
                                    new MySqlParameter("strUnitDesc", unit.UnitDesc),
                                    new MySqlParameter("strIs_Active", unit.Is_Active),
                                    new MySqlParameter("strCreated_By", unit.Created_By)
                        };

                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_unit_insert(@`strId`, ?, ?, ?, ?, ?)", parameters: sqlParams);

                        //new Unit after insert.
                        var newUnit = context.Unit.FromSql("SELECT * FROM m_unit WHERE Id = @`strId`;").ToListAsync();
                        resultObj.ObjectValue = newUnit.Result[0];

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

        public async Task<ResultObject> UpdateUnit(M_Unit unit)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = unit };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(unit).State = EntityState.Modified;

                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", unit.Id),
                                    new MySqlParameter("strUnitCode", unit.UnitCode),
                                    new MySqlParameter("strUnitName", unit.UnitName),
                                    new MySqlParameter("strUnitDesc", unit.UnitDesc),
                                    new MySqlParameter("strIs_Active", unit.Is_Active),
                                    new MySqlParameter("strCreated_By", unit.Updated_By)
                        };


                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_unit_update(?, ?, ?, ?, ?, ?)", parameters: sqlParams);

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

        public async Task<ResultObject> DeleteUnit(M_Unit unit)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = unit };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", unit.Id),
                                    new MySqlParameter("strDelete_By", unit.Updated_By)
                        };


                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_unit_delete( ?, ?)", parameters: sqlParams);

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
