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
    public class VendorBLL : IDisposable
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

        ~VendorBLL()
        {
            this.Dispose(false);
        }

        #endregion

        #region Private Member

        private bool IsDisposed = false;
        private AppConfiguration appSetting;
        private DbContextOptions contextOptions;

        #endregion

        public VendorBLL()
        {
            appSetting = new AppConfiguration();

            contextOptions = new DbContextOptionsBuilder()
                            .UseMySql(appSetting.ConnectionString)
                            .Options;
        }

        #region Method Member

        public async Task<List<M_Vendor>> GetVendor(int? id)
        {
            try
            {
                using (var context = new MasterDbContext(contextOptions))
                {
                    MySqlParameter[] sqlParams = new MySqlParameter[] {
                                             new MySqlParameter("strId", id)
                    };

                    return await context.Vendor.FromSql("call sp_vendor_get(?)", parameters: sqlParams).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResultObject> InsertVendor(M_Vendor vendor)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = vendor };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strVendorCode", vendor.VendorCode),
                                    new MySqlParameter("strVendorName", vendor.VendorName),
                                    new MySqlParameter("strAddressL1", vendor.AddressL1),
                                    new MySqlParameter("strAddressL2", vendor.AddressL2),
                                    new MySqlParameter("strAddressL3", vendor.AddressL3),
                                    new MySqlParameter("strAddressL4", vendor.AddressL4),
                                    new MySqlParameter("strTelephone", vendor.Telephone),
                                    new MySqlParameter("strFax", vendor.Fax),
                                    new MySqlParameter("strVendorEmail", vendor.VendorEmail),
                                    new MySqlParameter("strVendorContact", vendor.VendorContact),
                                    new MySqlParameter("strCreditTerm", vendor.CreditTerm),
                                    new MySqlParameter("strPriceLevel", vendor.PriceLevel),
                                    new MySqlParameter("strVendorTaxId", vendor.VendorTaxId),
                                    new MySqlParameter("strRemark", vendor.Remark),
                                    new MySqlParameter("strCompanyCode", vendor.CompanyCode),
                                    new MySqlParameter("strIs_Active", vendor.Is_Active),
                                    new MySqlParameter("strCreated_By", vendor.Created_By)
                        };

                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_vendor_insert(@`strId`, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

                        //new department after insert.
                        var newVendor = context.Vendor.FromSql("SELECT * FROM m_vendor WHERE Id = @`strId`;").ToListAsync();
                        resultObj.ObjectValue = newVendor.Result[0];

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

        public async Task<int> BulkInsertVendor(List<M_Vendor> lstVend)
        {
            int rowaffected = -1;

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (M_Vendor vendor in lstVend)
                        {
                            MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strVendorCode", vendor.VendorCode),
                                    new MySqlParameter("strVendorName", vendor.VendorName),
                                    new MySqlParameter("strAddressL1", vendor.AddressL1),
                                    new MySqlParameter("strAddressL2", vendor.AddressL2),
                                    new MySqlParameter("strAddressL3", vendor.AddressL3),
                                    new MySqlParameter("strAddressL4", vendor.AddressL4),
                                    new MySqlParameter("strTelephone", vendor.Telephone),
                                    new MySqlParameter("strFax", vendor.Fax),
                                    new MySqlParameter("strVendorEmail", vendor.VendorEmail),
                                    new MySqlParameter("strVendorContact", vendor.VendorContact),
                                    new MySqlParameter("strCreditTerm", vendor.CreditTerm),
                                    new MySqlParameter("strPriceLevel", vendor.PriceLevel),
                                    new MySqlParameter("strVendorTaxId", vendor.VendorTaxId),
                                    new MySqlParameter("strRemark", vendor.Remark),
                                    new MySqlParameter("strCompanyCode", vendor.CompanyCode),
                                    new MySqlParameter("strIs_Active", vendor.Is_Active),
                                    new MySqlParameter("strCreated_By", vendor.Created_By)
                            };

                            rowaffected += await context.Database.ExecuteSqlCommandAsync("call sp_vendor_upload(@`strId`, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);
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

        public async Task<ResultObject> UpdateVendor(M_Vendor vendor)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = vendor };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(vendor).State = EntityState.Modified;

                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", vendor.Id),
                                    new MySqlParameter("strVendorCode", vendor.VendorCode),
                                    new MySqlParameter("strVendorName", vendor.VendorName),
                                    new MySqlParameter("strAddressL1", vendor.AddressL1),
                                    new MySqlParameter("strAddressL2", vendor.AddressL2),
                                    new MySqlParameter("strAddressL3", vendor.AddressL3),
                                    new MySqlParameter("strAddressL4", vendor.AddressL4),
                                    new MySqlParameter("strTelephone", vendor.Telephone),
                                    new MySqlParameter("strFax", vendor.Fax),
                                    new MySqlParameter("strVendorEmail", vendor.VendorEmail),
                                    new MySqlParameter("strVendorContact", vendor.VendorContact),
                                    new MySqlParameter("strCreditTerm", vendor.CreditTerm),
                                    new MySqlParameter("strPriceLevel", vendor.PriceLevel),
                                    new MySqlParameter("strVendorTaxId", vendor.VendorTaxId),
                                    new MySqlParameter("strRemark", vendor.Remark),
                                    new MySqlParameter("strCompanyCode", vendor.CompanyCode),
                                    new MySqlParameter("strIs_Active", vendor.Is_Active),
                                    new MySqlParameter("strUpdated_By", vendor.Updated_By)
                        };


                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_vendor_update(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

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

        public async Task<ResultObject> DeleteVendor(M_Vendor vendor)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = vendor };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", vendor.Id),
                                    new MySqlParameter("strDelete_By", vendor.Updated_By)
                        };

                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_vendor_delete( ?, ?)", parameters: sqlParams);

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
