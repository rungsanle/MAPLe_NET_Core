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
    public class CustomerBLL : IDisposable
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

        ~CustomerBLL()
        {
            this.Dispose(false);
        }

        #endregion

        #region Private Member

        private bool IsDisposed = false;
        private AppConfiguration appSetting;
        private DbContextOptions contextOptions;

        #endregion

        public CustomerBLL()
        {
            appSetting = new AppConfiguration();

            contextOptions = new DbContextOptionsBuilder()
                            .UseMySql(appSetting.ConnectionString)
                            .Options;
        }

        #region Method Member

        public async Task<List<M_Customer>> GetCustomer(int? id)
        {
            try
            {
                using (var context = new MasterDbContext(contextOptions))
                {
                    MySqlParameter[] sqlParams = new MySqlParameter[] {
                                             new MySqlParameter("strId", id)
                    };

                    return await context.Customer.FromSql("call sp_customer_get(?)", parameters: sqlParams).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<ResultObject> InsertCustomer(M_Customer cust)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = cust };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strCustomerCode", cust.CustomerCode),
                                    new MySqlParameter("strCustomerName", cust.CustomerName),
                                    new MySqlParameter("strAddressL1", cust.AddressL1),
                                    new MySqlParameter("strAddressL2", cust.AddressL2),
                                    new MySqlParameter("strAddressL3", cust.AddressL3),
                                    new MySqlParameter("strAddressL4", cust.AddressL4),
                                    new MySqlParameter("strTelephone", cust.Telephone),
                                    new MySqlParameter("strFax", cust.Fax),
                                    new MySqlParameter("strCustomerEmail", cust.CustomerEmail),
                                    new MySqlParameter("strCustomerContact", cust.CustomerContact),
                                    new MySqlParameter("strCreditTerm", cust.CreditTerm),
                                    new MySqlParameter("strPriceLevel", cust.PriceLevel),
                                    new MySqlParameter("strCustomerTaxId", cust.CustomerTaxId),
                                    new MySqlParameter("strRemark", cust.Remark),
                                    new MySqlParameter("strCompanyCode", cust.CompanyCode),
                                    new MySqlParameter("strIs_Active", cust.Is_Active),
                                    new MySqlParameter("strCreated_By", cust.Created_By)
                        };

                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_customer_insert(@`strId`, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

                        //new department after insert.
                        var newCust = context.Customer.FromSql("SELECT * FROM m_customer WHERE Id = @`strId`;").ToListAsync();
                        resultObj.ObjectValue = newCust.Result[0];

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

        public async Task<int> BulkInsertCustomer(List<M_Customer> lstCust)
        {
            int rowaffected = -1;

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (M_Customer cust in lstCust)
                        {
                            MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strCustomerCode", cust.CustomerCode),
                                    new MySqlParameter("strCustomerName", cust.CustomerName),
                                    new MySqlParameter("strAddressL1", cust.AddressL1),
                                    new MySqlParameter("strAddressL2", cust.AddressL2),
                                    new MySqlParameter("strAddressL3", cust.AddressL3),
                                    new MySqlParameter("strAddressL4", cust.AddressL4),
                                    new MySqlParameter("strTelephone", cust.Telephone),
                                    new MySqlParameter("strFax", cust.Fax),
                                    new MySqlParameter("strCustomerEmail", cust.CustomerEmail),
                                    new MySqlParameter("strCustomerContact", cust.CustomerContact),
                                    new MySqlParameter("strCreditTerm", cust.CreditTerm),
                                    new MySqlParameter("strPriceLevel", cust.PriceLevel),
                                    new MySqlParameter("strCustomerTaxId", cust.CustomerTaxId),
                                    new MySqlParameter("strRemark", cust.Remark),
                                    new MySqlParameter("strCompanyCode", cust.CompanyCode),
                                    new MySqlParameter("strIs_Active", cust.Is_Active),
                                    new MySqlParameter("strCreated_By", cust.Created_By)
                            };

                            rowaffected += await context.Database.ExecuteSqlCommandAsync("call sp_customer_upload(@`strId`, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);
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

        public async Task<ResultObject> UpdateCustomer(M_Customer cust)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = cust };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(cust).State = EntityState.Modified;

                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", cust.Id),
                                    new MySqlParameter("strCustomerCode", cust.CustomerCode),
                                    new MySqlParameter("strCustomerName", cust.CustomerName),
                                    new MySqlParameter("strAddressL1", cust.AddressL1),
                                    new MySqlParameter("strAddressL2", cust.AddressL2),
                                    new MySqlParameter("strAddressL3", cust.AddressL3),
                                    new MySqlParameter("strAddressL4", cust.AddressL4),
                                    new MySqlParameter("strTelephone", cust.Telephone),
                                    new MySqlParameter("strFax", cust.Fax),
                                    new MySqlParameter("strCustomerEmail", cust.CustomerEmail),
                                    new MySqlParameter("strCustomerContact", cust.CustomerContact),
                                    new MySqlParameter("strCreditTerm", cust.CreditTerm),
                                    new MySqlParameter("strPriceLevel", cust.PriceLevel),
                                    new MySqlParameter("strCustomerTaxId", cust.CustomerTaxId),
                                    new MySqlParameter("strRemark", cust.Remark),
                                    new MySqlParameter("strCompanyCode", cust.CompanyCode),
                                    new MySqlParameter("strIs_Active", cust.Is_Active),
                                    new MySqlParameter("strUpdated_By", cust.Updated_By)
                        };


                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_customer_update(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

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

        public async Task<ResultObject> DeleteCustomer(M_Customer cust)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = cust };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", cust.Id),
                                    new MySqlParameter("strDelete_By", cust.Updated_By)
                        };

                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_customer_delete( ?, ?)", parameters: sqlParams);

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
