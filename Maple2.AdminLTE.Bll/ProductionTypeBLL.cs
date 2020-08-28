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
    public class ProductionTypeBLL : IDisposable
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

        ~ProductionTypeBLL()
        {
            this.Dispose(false);
        }

        #endregion

        #region Private Member

        private bool IsDisposed = false;
        private AppConfiguration appSetting;
        private DbContextOptions<MasterDbContext> contextOptions;

        #endregion

        public ProductionTypeBLL()
        {
            appSetting = new AppConfiguration();

            contextOptions = new DbContextOptionsBuilder<MasterDbContext>()
                            .UseMySql(appSetting.ConnectionString)
                            .Options;
        }

        #region Method Member

        public async Task<List<M_ProductionType>> GetProductionType(int? id)
        {
            try
            {
                using (var context = new MasterDbContext(contextOptions))
                {
                    MySqlParameter[] sqlParams = new MySqlParameter[] {
                                             new MySqlParameter("strId", id)
                    };

                    return await context.ProductionType.FromSql("call sp_productiontype_get(?)", parameters: sqlParams).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResultObject> InsertProductionType(M_ProductionType prodType)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = prodType };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strProdTypeCode", prodType.ProdTypeCode),
                                    new MySqlParameter("strProdTypeName", prodType.ProdTypeName),
                                    new MySqlParameter("strProdTypeDesc", prodType.ProdTypeDesc),
                                    new MySqlParameter("strProdTypeSeq", prodType.ProdTypeSeq),
                                    new MySqlParameter("strCompanyCode", prodType.CompanyCode),
                                    new MySqlParameter("strIs_Active", prodType.Is_Active),
                                    new MySqlParameter("strCreated_By", prodType.Created_By)
                        };

                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_productiontype_insert(@`strId`, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

                        //new department after insert.
                        var newProdType = context.ProductionType.FromSql("SELECT * FROM m_productiontype WHERE Id = @`strId`;").ToListAsync();
                        resultObj.ObjectValue = newProdType.Result[0];

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

        public async Task<ResultObject> UpdateProductionType(M_ProductionType prodType)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = prodType };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(prodType).State = EntityState.Modified;

                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", prodType.Id),
                                    new MySqlParameter("strProdTypeCode", prodType.ProdTypeCode),
                                    new MySqlParameter("strProdTypeName", prodType.ProdTypeName),
                                    new MySqlParameter("strProdTypeDesc", prodType.ProdTypeDesc),
                                    new MySqlParameter("strProdTypeSeq", prodType.ProdTypeSeq),
                                    new MySqlParameter("strCompanyCode", prodType.CompanyCode),
                                    new MySqlParameter("strIs_Active", prodType.Is_Active),
                                    new MySqlParameter("strCreated_By", prodType.Updated_By)
                        };


                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_productiontype_update(?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

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

        public async Task<ResultObject> DeleteProductionType(M_ProductionType prodType)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = prodType };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", prodType.Id),
                                    new MySqlParameter("strDelete_By", prodType.Updated_By)
                        };


                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_productiontype_delete( ?, ?)", parameters: sqlParams);

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
