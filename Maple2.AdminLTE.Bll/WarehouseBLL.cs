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
    public class WarehouseBLL : IDisposable
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

        ~WarehouseBLL()
        {
            this.Dispose(false);
        }

        #endregion

        #region Private Member

        private bool IsDisposed = false;
        private AppConfiguration appSetting;
        private DbContextOptions contextOptions;

        #endregion

        public WarehouseBLL()
        {
            appSetting = new AppConfiguration();

            contextOptions = new DbContextOptionsBuilder()
                            .UseMySql(appSetting.ConnectionString)
                            .Options;
        }

        #region Method Member

        public async Task<List<M_Warehouse>> GetWarehouse(int? id)
        {
            try
            {
                using (var context = new MasterDbContext(contextOptions))
                {
                    MySqlParameter[] sqlParams = new MySqlParameter[] {
                                        new MySqlParameter("strId", id)
                    };

                    return await context.Warehouse.FromSql("call sp_warehouse_get(?)", parameters: sqlParams).ToListAsync();
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<ResultObject> InsertWarehouse(M_Warehouse wh)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = wh };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strWarehouseCode", wh.WarehouseCode),
                                    new MySqlParameter("strWarehouseName", wh.WarehouseName),
                                    new MySqlParameter("strWarehouseDesc", wh.WarehouseDesc),
                                    new MySqlParameter("strCompanyCode", wh.CompanyCode),
                                    new MySqlParameter("strIs_Active", wh.Is_Active),
                                    new MySqlParameter("strCreated_By", wh.Created_By)
                        };

                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_warehouse_insert(@`strId`, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

                        //new Warehouse after insert.
                        var newWh = context.Warehouse.FromSql("SELECT * FROM m_warehouse WHERE Id = @`strId`;").ToListAsync();
                        resultObj.ObjectValue = newWh.Result[0];

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

        public async Task<ResultObject> UpdateWarehouse(M_Warehouse wh)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = wh };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(wh).State = EntityState.Modified;

                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", wh.Id),
                                    new MySqlParameter("strWarehouseCode", wh.WarehouseCode),
                                    new MySqlParameter("strWarehouseName", wh.WarehouseName),
                                    new MySqlParameter("strWarehouseDesc", wh.WarehouseDesc),
                                    new MySqlParameter("strCompanyCode", wh.CompanyCode),
                                    new MySqlParameter("strIs_Active", wh.Is_Active),
                                    new MySqlParameter("strCreated_By", wh.Updated_By)
                        };


                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_warehouse_update(?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

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

        public async Task<ResultObject> DeleteWarehouse(M_Warehouse wh)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = wh };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", wh.Id),
                                    new MySqlParameter("strDelete_By", wh.Updated_By)
                        };


                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_warehouse_delete( ?, ?)", parameters: sqlParams);

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
