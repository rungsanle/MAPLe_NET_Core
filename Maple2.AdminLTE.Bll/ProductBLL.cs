using Maple2.AdminLTE.Bel;
using Maple2.AdminLTE.Dal;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maple2.AdminLTE.Bll
{
    public class ProductBLL : IDisposable
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

        ~ProductBLL()
        {
            this.Dispose(false);
        }

        #endregion

        #region Private Member

        private bool IsDisposed = false;
        private AppConfiguration appSetting;
        private DbContextOptions<MasterDbContext> contextOptions;

        #endregion

        public ProductBLL()
        {
            appSetting = new AppConfiguration();

            contextOptions = new DbContextOptionsBuilder<MasterDbContext>()
                            .UseMySql(appSetting.ConnectionString)
                            .Options;
        }

        #region Method Member

        public async Task<List<M_Product>> GetProduct(int? id)
        {
            try
            {
                using (var context = new MasterDbContext(contextOptions))
                {
                    MySqlParameter[] sqlParams = new MySqlParameter[] {
                                             new MySqlParameter("strId", id)
                    };

                    var objList = await context.Query<M_ProductObj>().FromSql("call sp_product_get(?)", parameters: sqlParams).ToListAsync();

                    return objList.ConvertAll(p => new M_Product
                    {
                        Id = p.Id,
                        ProductCode = p.ProductCode,
                        ProductName = p.ProductName,
                        ProductNameRef = p.ProductNameRef,
                        ProductDesc = p.ProductDesc,
                        MaterialTypeId = p.MaterialTypeId,
                        MaterialType = p.MaterialType,
                        ProductionTypeId = p.ProductionTypeId,
                        ProductionType = p.ProductionType,
                        MachineId = p.MachineId,
                        Machine = p.Machine,
                        UnitId = p.UnitId,
                        Unit = p.Unit,
                        PackageStdQty = p.PackageStdQty,
                        SalesPrice1 = p.SalesPrice1,
                        SalesPrice2 = p.SalesPrice2,
                        SalesPrice3 = p.SalesPrice3,
                        SalesPrice4 = p.SalesPrice4,
                        SalesPrice5 = p.SalesPrice5,
                        GLSalesAccount = p.GLSalesAccount,
                        GLInventAccount = p.GLInventAccount,
                        GLCogsAccount = p.GLCogsAccount,
                        RevisionNo = p.RevisionNo,
                        WarehouseId = p.WarehouseId,
                        Warehouse = p.Warehouse,
                        LocationId = p.LocationId,
                        Location = p.Location,
                        ProductImagePath = p.ProductImagePath,
                        CompanyCode = p.CompanyCode,
                        Is_Active = p.Is_Active,
                        Created_Date = p.Created_Date,
                        Created_By = p.Created_By,
                        Updated_Date = p.Updated_Date,
                        Updated_By = p.Updated_By
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResultObject> InsertProduct(M_Product prod)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = prod };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strProductCode", prod.ProductCode),
                                    new MySqlParameter("strProductName", prod.ProductName),
                                    new MySqlParameter("strProductNameRef", prod.ProductNameRef),
                                    new MySqlParameter("strProductDesc", prod.ProductDesc),
                                    new MySqlParameter("strMaterialTypeId", prod.MaterialTypeId),
                                    new MySqlParameter("strProductionTypeId", prod.ProductionTypeId),
                                    new MySqlParameter("strMachineId", prod.MachineId),
                                    new MySqlParameter("strUnitId", prod.UnitId),
                                    new MySqlParameter("strPackageStdQty", prod.PackageStdQty),
                                    new MySqlParameter("strSalesPrice1", prod.SalesPrice1),
                                    new MySqlParameter("strSalesPrice2", prod.SalesPrice2),
                                    new MySqlParameter("strSalesPrice3", prod.SalesPrice3),
                                    new MySqlParameter("strSalesPrice4", prod.SalesPrice4),
                                    new MySqlParameter("strSalesPrice5", prod.SalesPrice5),
                                    new MySqlParameter("strGLSalesAccount", prod.GLSalesAccount),
                                    new MySqlParameter("strGLInventAccount", prod.GLInventAccount),
                                    new MySqlParameter("strGLCogsAccount", prod.GLCogsAccount),
                                    new MySqlParameter("strRevisionNo", prod.RevisionNo),
                                    new MySqlParameter("strWarehouseId", prod.WarehouseId),
                                    new MySqlParameter("strLocationId", prod.LocationId),
                                    new MySqlParameter("strProductImagePath", prod.ProductImagePath),
                                    new MySqlParameter("strCompanyCode", prod.CompanyCode),
                                    new MySqlParameter("strIs_Active", prod.Is_Active),
                                    new MySqlParameter("strCreated_By", prod.Created_By)
                        };

                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_product_insert(@`strId`, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

                        //new product after insert.
                        var newProd = context.Product.FromSql("SELECT * FROM m_product WHERE Id = @`strId`;").ToListAsync();
                        resultObj.ObjectValue = newProd.Result[0];

                        if (prod.ProdProcess.Count > 0)
                        {
                            foreach (M_Product_Process prodpro in prod.ProdProcess)
                            {
                                MySqlParameter[] sqlParams2 = new MySqlParameter[] {
                                        new MySqlParameter("strProductId", newProd.Result[0].Id),
                                        new MySqlParameter("strProcessId", prodpro.ProcessId),
                                        new MySqlParameter("strIs_Active", prodpro.Is_Active),
                                        new MySqlParameter("strUpdated_By", prodpro.Updated_By)
                                };

                                //Output Parameter no need to define. @`strId`
                                resultObj.RowAffected += await context.Database.ExecuteSqlCommandAsync("call sp_product_update_process(?, ?, ?, ?)", parameters: sqlParams2);
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

        public async Task<int> BulkInsertProduct(List<M_Product> lstProd)
        {
            int rowaffected = -1;

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (M_Product prod in lstProd)
                        {
                            MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strProductCode", prod.ProductCode),
                                    new MySqlParameter("strProductName", prod.ProductName),
                                    new MySqlParameter("strProductNameRef", prod.ProductNameRef),
                                    new MySqlParameter("strProductDesc", prod.ProductDesc),
                                    new MySqlParameter("strMaterialTypeId", prod.MaterialTypeId),
                                    new MySqlParameter("strProductionTypeId", prod.ProductionTypeId),
                                    new MySqlParameter("strMachineId", prod.MachineId),
                                    new MySqlParameter("strUnitId", prod.UnitId),
                                    new MySqlParameter("strPackageStdQty", prod.PackageStdQty),
                                    new MySqlParameter("strSalesPrice1", prod.SalesPrice1),
                                    new MySqlParameter("strSalesPrice2", prod.SalesPrice2),
                                    new MySqlParameter("strSalesPrice3", prod.SalesPrice3),
                                    new MySqlParameter("strSalesPrice4", prod.SalesPrice4),
                                    new MySqlParameter("strSalesPrice5", prod.SalesPrice5),
                                    new MySqlParameter("strGLSalesAccount", prod.GLSalesAccount),
                                    new MySqlParameter("strGLInventAccount", prod.GLInventAccount),
                                    new MySqlParameter("strGLCogsAccount", prod.GLCogsAccount),
                                    new MySqlParameter("strRevisionNo", prod.RevisionNo),
                                    new MySqlParameter("strWarehouseId", prod.WarehouseId),
                                    new MySqlParameter("strLocationId", prod.LocationId),
                                    new MySqlParameter("strCompanyCode", prod.CompanyCode),
                                    new MySqlParameter("strIs_Active", prod.Is_Active),
                                    new MySqlParameter("strCreated_By", prod.Created_By)
                            };

                            rowaffected += await context.Database.ExecuteSqlCommandAsync("call sp_product_upload(@`strId`, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);
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

        public async Task<ResultObject> UpdateProduct(M_Product prod)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = prod };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(prod).State = EntityState.Modified;

                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", prod.Id),
                                    new MySqlParameter("strProductCode", prod.ProductCode),
                                    new MySqlParameter("strProductName", prod.ProductName),
                                    new MySqlParameter("strProductNameRef", prod.ProductNameRef),
                                    new MySqlParameter("strProductDesc", prod.ProductDesc),
                                    new MySqlParameter("strMaterialTypeId", prod.MaterialTypeId),
                                    new MySqlParameter("strProductionTypeId", prod.ProductionTypeId),
                                    new MySqlParameter("strMachineId", prod.MachineId),
                                    new MySqlParameter("strUnitId", prod.UnitId),
                                    new MySqlParameter("strPackageStdQty", prod.PackageStdQty),
                                    new MySqlParameter("strSalesPrice1", prod.SalesPrice1),
                                    new MySqlParameter("strSalesPrice2", prod.SalesPrice2),
                                    new MySqlParameter("strSalesPrice3", prod.SalesPrice3),
                                    new MySqlParameter("strSalesPrice4", prod.SalesPrice4),
                                    new MySqlParameter("strSalesPrice5", prod.SalesPrice5),
                                    new MySqlParameter("strGLSalesAccount", prod.GLSalesAccount),
                                    new MySqlParameter("strGLInventAccount", prod.GLInventAccount),
                                    new MySqlParameter("strGLCogsAccount", prod.GLCogsAccount),
                                    new MySqlParameter("strRevisionNo", prod.RevisionNo),
                                    new MySqlParameter("strWarehouseId", prod.WarehouseId),
                                    new MySqlParameter("strLocationId", prod.LocationId),
                                    new MySqlParameter("strProductImagePath", prod.ProductImagePath),
                                    new MySqlParameter("strCompanyCode", prod.CompanyCode),
                                    new MySqlParameter("strIs_Active", prod.Is_Active),
                                    new MySqlParameter("strUpdated_By", prod.Updated_By)
                        };

                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_product_update(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

                        if (prod.ProdProcess.Count > 0)
                        {
                            foreach (M_Product_Process prodpro in prod.ProdProcess)
                            {
                                MySqlParameter[] sqlParams2 = new MySqlParameter[] {
                                        new MySqlParameter("strProductId", prodpro.ProductId),
                                        new MySqlParameter("strProcessId", prodpro.ProcessId),
                                        new MySqlParameter("strIs_Active", prodpro.Is_Active),
                                        new MySqlParameter("strUpdated_By", prodpro.Updated_By)
                                };

                                //Output Parameter no need to define. @`strId`
                                resultObj.RowAffected += await context.Database.ExecuteSqlCommandAsync("call sp_product_update_process(?, ?, ?, ?)", parameters: sqlParams2);
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

        public async Task<ResultObject> DeleteProduct(M_Product prod)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = prod };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", prod.Id),
                                    new MySqlParameter("strDelete_By", prod.Updated_By)
                        };

                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_product_delete( ?, ?)", parameters: sqlParams);

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

        public async Task<List<M_Product_Process>> GetProductProcess(int prodId)
        {
            try
            {
                using (var context = new MasterDbContext(contextOptions))
                {
                    MySqlParameter[] sqlParams = new MySqlParameter[] {
                                             new MySqlParameter("strId", prodId)
                    };

                    var objList = await context.Query<M_Product_ProcessObj>().FromSql("call sp_product_process_get(?)", parameters: sqlParams).ToListAsync();

                    return objList.ConvertAll(p => new M_Product_Process
                    {
                        Id = p.Id,
                        ProductId = p.ProductId,
                        ProcessId = p.ProcessId,
                        Is_Active = p.Is_Active,
                        ProcessSeq = p.ProcessSeq,
                        ProcessCode = p.ProcessCode,
                        ProcessName = p.ProcessName
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> InsertProductProcess(List<M_Product_Process> lstProdProcess)
        {
            int rowaffected = -1;

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (M_Product_Process prodpro in lstProdProcess)
                        {
                            MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strProductId", prodpro.ProductId),
                                    new MySqlParameter("strProcessId", prodpro.ProcessId),
                                    new MySqlParameter("strIs_Active", prodpro.Is_Active),
                                    new MySqlParameter("strUpdated_By", prodpro.Created_By)
                            };

                            rowaffected += await context.Database.ExecuteSqlCommandAsync("call sp_product_insert_process(?, ?, ?, ?)", parameters: sqlParams);
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

        public async Task<int> UpdateProductProcess(List<M_Product_Process> lstProdProcess)
        {
            int rowaffected = -1;

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (M_Product_Process prodpro in lstProdProcess)
                        {
                            MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strProductId", prodpro.ProductId),
                                    new MySqlParameter("strProcessId", prodpro.ProcessId),
                                    new MySqlParameter("strIs_Active", prodpro.Is_Active),
                                    new MySqlParameter("strUpdated_By", prodpro.Updated_By)
                            };

                            rowaffected += await context.Database.ExecuteSqlCommandAsync("call sp_product_update_process(?, ?, ?, ?)", parameters: sqlParams);
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

        #endregion
    }
}
