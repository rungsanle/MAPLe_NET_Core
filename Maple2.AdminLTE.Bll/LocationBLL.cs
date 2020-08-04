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
    public class LocationBLL : IDisposable
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

        ~LocationBLL()
        {
            this.Dispose(false);
        }

        #endregion

        #region Private Member

        private bool IsDisposed = false;
        private AppConfiguration appSetting;
        private DbContextOptions contextOptions;

        #endregion

        public LocationBLL()
        {
            appSetting = new AppConfiguration();

            contextOptions = new DbContextOptionsBuilder()
                            .UseMySql(appSetting.ConnectionString)
                            .Options;
        }

        #region Method Member

        public async Task<List<M_Location>> GetLocation(int? id)
        {
            try
            {
                using (var context = new MasterDbContext(contextOptions))
                {
                    MySqlParameter[] sqlParams = new MySqlParameter[] {
                                             new MySqlParameter("strId", id)
                    };

                    var objList = await context.Query<M_LocationObj>().FromSql("call sp_location_get(?)", parameters: sqlParams).ToListAsync();

                    return objList.ConvertAll(loc => new M_Location
                    {
                        Id = loc.Id,
                        LocationCode = loc.LocationCode,
                        LocationName = loc.LocationName,
                        LocationDesc = loc.LocationDesc,
                        WarehouseId = loc.WarehouseId,
                        WarehouseName = loc.WarehouseName,
                        CompanyCode = loc.CompanyCode,
                        Is_Active = loc.Is_Active,
                        Created_By = loc.Created_By,
                        Created_Date = loc.Created_Date,
                        Updated_By = loc.Updated_By,
                        Updated_Date = loc.Updated_Date
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResultObject> InsertLocation(M_Location loc)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = loc };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strLocationCode", loc.LocationCode),
                                    new MySqlParameter("strLocationName", loc.LocationName),
                                    new MySqlParameter("strLocationDesc", loc.LocationDesc),
                                    new MySqlParameter("strWarehouseId", loc.WarehouseId),
                                    new MySqlParameter("strCompanyCode", loc.CompanyCode),
                                    new MySqlParameter("strIs_Active", loc.Is_Active),
                                    new MySqlParameter("strCreated_By", loc.Created_By)
                        };

                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_location_insert(@`strId`, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

                        //new Location after insert.
                        var newLoc = context.Location.FromSql("SELECT * FROM m_location WHERE Id = @`strId`;").ToListAsync();
                        resultObj.ObjectValue = newLoc.Result[0];

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

        public async Task<ResultObject> UpdateLocation(M_Location loc)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = loc };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(loc).State = EntityState.Modified;

                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", loc.Id),
                                    new MySqlParameter("strLocationCode", loc.LocationCode),
                                    new MySqlParameter("strLocationName", loc.LocationName),
                                    new MySqlParameter("strLocationDesc", loc.LocationDesc),
                                    new MySqlParameter("strWarehouseId", loc.WarehouseId),
                                    new MySqlParameter("strCompanyCode", loc.CompanyCode),
                                    new MySqlParameter("strIs_Active", loc.Is_Active),
                                    new MySqlParameter("strCreated_By", loc.Updated_By)
                        };


                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_location_update(?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

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

        public async Task<ResultObject> DeleteLocation(M_Location loc)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = loc };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", loc.Id),
                                    new MySqlParameter("strDelete_By", loc.Updated_By)
                        };


                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_location_delete( ?, ?)", parameters: sqlParams);

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
