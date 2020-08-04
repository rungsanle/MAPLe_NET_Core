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
    public class CompanyBLL : IDisposable
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

        ~CompanyBLL()
        {
            this.Dispose(false);
        }

        #endregion

        #region Private Member

        private bool IsDisposed = false;
        private AppConfiguration appSetting;
        private DbContextOptions contextOptions;

        #endregion

        public CompanyBLL()
        {
            appSetting = new AppConfiguration();

            contextOptions = new DbContextOptionsBuilder()
                            .UseMySql(appSetting.ConnectionString)
                            .Options;
        }

        #region Method Member

        public async Task<List<M_Company>> GetCompany(int? id)
        {
            try
            {
                using (var context = new MasterDbContext(contextOptions))
                {
                    MySqlParameter[] sqlParams = new MySqlParameter[] {
                                             new MySqlParameter("strId", id)
                    };

                    return await context.Companies.FromSql("call sp_company_get(?)", parameters: sqlParams).ToListAsync();
                }

            
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public int InsertCompany(ref M_Company comp)
        //{
        //    using (var context = new MasterDbContext(contextOptions))
        //    {
        //        using (var transaction = context.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                MySqlParameter[] sqlParams = new MySqlParameter[] {
        //                            new MySqlParameter("strCompanyCode", comp.CompanyCode),
        //                            new MySqlParameter("strCompanyName", comp.CompanyName),
        //                            new MySqlParameter("strCompanyLogoPath", comp.CompanyLogoPath),
        //                            new MySqlParameter("strAddressL1", comp.AddressL1),
        //                            new MySqlParameter("strAddressL2", comp.AddressL2),
        //                            new MySqlParameter("strAddressL3", comp.AddressL3),
        //                            new MySqlParameter("strAddressL4", comp.AddressL4),
        //                            new MySqlParameter("strTelephone", comp.Telephone),
        //                            new MySqlParameter("strFax", comp.Fax),
        //                            new MySqlParameter("strCompanyTaxId", comp.CompanyTaxId),
        //                            new MySqlParameter("strIs_Active", comp.Is_Active),
        //                            new MySqlParameter("strCreated_By", comp.Created_By)
        //                };


        //                //Output Parameter no need to define. @`strId`
        //                int rowaffected = context.Database.ExecuteSqlCommand("call sp_company_insert(@`strId`, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

        //                var newComp = context.Companies.FromSql("SELECT * FROM m_company WHERE Id = @`strId`;").ToListAsync();
        //                comp = newComp.Result[0];

        //                transaction.Commit();

        //                return rowaffected;
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //                throw ex;
        //            }
        //        }
        //    }

        //}

        public async Task<ResultObject> InsertCompany(M_Company comp)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = comp };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strCompanyCode", comp.CompanyCode),
                                    new MySqlParameter("strCompanyName", comp.CompanyName),
                                    new MySqlParameter("strCompanyLogoPath", comp.CompanyLogoPath),
                                    new MySqlParameter("strAddressL1", comp.AddressL1),
                                    new MySqlParameter("strAddressL2", comp.AddressL2),
                                    new MySqlParameter("strAddressL3", comp.AddressL3),
                                    new MySqlParameter("strAddressL4", comp.AddressL4),
                                    new MySqlParameter("strTelephone", comp.Telephone),
                                    new MySqlParameter("strFax", comp.Fax),
                                    new MySqlParameter("strCompanyTaxId", comp.CompanyTaxId),
                                    new MySqlParameter("strIs_Active", comp.Is_Active),
                                    new MySqlParameter("strCreated_By", comp.Created_By)
                        };


                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_company_insert(@`strId`, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

                        var newComp = context.Companies.FromSql("SELECT * FROM m_company WHERE Id = @`strId`;").ToListAsync();
                        resultObj.ObjectValue = newComp.Result[0];

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

        //public int UpdateCompany(M_Company comp)
        //{
        //    using (var context = new MasterDbContext(contextOptions))
        //    {
        //        using (var transaction = context.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                context.Entry(comp).State = EntityState.Modified;

        //                MySqlParameter[] sqlParams = new MySqlParameter[] {
        //                            new MySqlParameter("strId", comp.Id),
        //                            new MySqlParameter("strCompanyCode", comp.CompanyCode),
        //                            new MySqlParameter("strCompanyName", comp.CompanyName),
        //                            new MySqlParameter("strCompanyLogoPath", comp.CompanyLogoPath),
        //                            new MySqlParameter("strAddressL1", comp.AddressL1),
        //                            new MySqlParameter("strAddressL2", comp.AddressL2),
        //                            new MySqlParameter("strAddressL3", comp.AddressL3),
        //                            new MySqlParameter("strAddressL4", comp.AddressL4),
        //                            new MySqlParameter("strTelephone", comp.Telephone),
        //                            new MySqlParameter("strFax", comp.Fax),
        //                            new MySqlParameter("strCompanyTaxId", comp.CompanyTaxId),
        //                            new MySqlParameter("strIs_Active", comp.Is_Active),
        //                            new MySqlParameter("strUpdated_By", comp.Updated_By)
        //                };


        //                //Output Parameter no need to define. @`strId`
        //                int rowaffected = context.Database.ExecuteSqlCommand("call sp_company_update(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

        //                transaction.Commit();

        //                return rowaffected;
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //                throw ex;
        //            }
        //        }
        //    }

        //}

        public async Task<ResultObject> UpdateCompany(M_Company comp)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = comp };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(comp).State = EntityState.Modified;

                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", comp.Id),
                                    new MySqlParameter("strCompanyCode", comp.CompanyCode),
                                    new MySqlParameter("strCompanyName", comp.CompanyName),
                                    new MySqlParameter("strCompanyLogoPath", comp.CompanyLogoPath),
                                    new MySqlParameter("strAddressL1", comp.AddressL1),
                                    new MySqlParameter("strAddressL2", comp.AddressL2),
                                    new MySqlParameter("strAddressL3", comp.AddressL3),
                                    new MySqlParameter("strAddressL4", comp.AddressL4),
                                    new MySqlParameter("strTelephone", comp.Telephone),
                                    new MySqlParameter("strFax", comp.Fax),
                                    new MySqlParameter("strCompanyTaxId", comp.CompanyTaxId),
                                    new MySqlParameter("strIs_Active", comp.Is_Active),
                                    new MySqlParameter("strUpdated_By", comp.Updated_By)
                        };


                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_company_update(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

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

        //public int DeleteCompany(M_Company comp)
        //{
        //    using (var context = new MasterDbContext(contextOptions))
        //    {
        //        using (var transaction = context.Database.BeginTransaction())
        //        {
        //            try
        //            {

        //                MySqlParameter[] sqlParams = new MySqlParameter[] {
        //                            new MySqlParameter("strId", comp.Id),
        //                            new MySqlParameter("strDelete_By", comp.Updated_By)
        //                };


        //                //Output Parameter no need to define. @`strId`
        //                int rowaffected = context.Database.ExecuteSqlCommand("call sp_company_delete( ?, ?)", parameters: sqlParams);

        //                transaction.Commit();

        //                return rowaffected;
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //                throw ex;
        //            }
        //        }
        //    }

        //}

        public async Task<ResultObject> DeleteCompany(M_Company comp)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = comp };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", comp.Id),
                                    new MySqlParameter("strDelete_By", comp.Updated_By)
                        };


                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_company_delete( ?, ?)", parameters: sqlParams);

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
