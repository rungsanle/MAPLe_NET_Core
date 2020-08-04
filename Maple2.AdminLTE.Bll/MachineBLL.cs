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
    public class MachineBLL : IDisposable
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

        ~MachineBLL()
        {
            this.Dispose(false);
        }

        #endregion

        #region Private Member

        private bool IsDisposed = false;
        private AppConfiguration appSetting;
        private DbContextOptions contextOptions;

        #endregion

        public MachineBLL()
        {
            appSetting = new AppConfiguration();

            contextOptions = new DbContextOptionsBuilder()
                            .UseMySql(appSetting.ConnectionString)
                            .Options;
        }

        #region Method Member



        public async Task<List<M_Machine>> GetMachine(int? id)
        {
            try
            {
                using (var context = new MasterDbContext(contextOptions))
                {
                    MySqlParameter[] sqlParams = new MySqlParameter[] {
                                        new MySqlParameter("strId", id)
                    };

                    var objList = await context.Query<M_MachineObj>().FromSql("call sp_machine_get(?)", parameters: sqlParams).ToListAsync();

                    return objList.ConvertAll(mc => new M_Machine
                    {
                        Id = mc.Id,
                        MachineCode = mc.MachineCode,
                        MachineName = mc.MachineName,
                        MachineProdType = mc.MachineProdType,
                        MachineProdTypeName = mc.MachineProdTypeName,
                        MachineSize = mc.MachineSize,
                        MachineRemark = mc.MachineRemark,
                        CompanyCode = mc.CompanyCode,
                        Is_Active = mc.Is_Active,
                        Created_By = mc.Created_By,
                        Created_Date = mc.Created_Date,
                        Updated_By = mc.Updated_By,
                        Updated_Date = mc.Updated_Date
                    });

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<List<M_Machine>> GetMachineByProdType(int? prodTypeId)
        {
            try
            {
                using (var context = new MasterDbContext(contextOptions))
                {
                    MySqlParameter[] sqlParams = new MySqlParameter[] {
                                        new MySqlParameter("strProdTypeId", prodTypeId)
                    };

                    var objList = await context.Query<M_MachineObj>().FromSql("call sp_machine_get_byprodtype(?)", parameters: sqlParams).ToListAsync();

                    return objList.ConvertAll(mc => new M_Machine
                    {
                        Id = mc.Id,
                        MachineCode = mc.MachineCode,
                        MachineName = mc.MachineName,
                        MachineProdType = mc.MachineProdType,
                        MachineProdTypeName = mc.MachineProdTypeName,
                        MachineSize = mc.MachineSize,
                        MachineRemark = mc.MachineRemark,
                        CompanyCode = mc.CompanyCode,
                        Is_Active = mc.Is_Active,
                        Created_By = mc.Created_By,
                        Created_Date = mc.Created_Date,
                        Updated_By = mc.Updated_By,
                        Updated_Date = mc.Updated_Date
                    });

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<ResultObject> InsertMachine(M_Machine machine)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = machine };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strMachineCode", machine.MachineCode),
                                    new MySqlParameter("strMachineName", machine.MachineName),
                                    new MySqlParameter("strMachineProdType", machine.MachineProdType),
                                    new MySqlParameter("strMachineSize", machine.MachineSize),
                                    new MySqlParameter("strMachineRemark", machine.MachineRemark),
                                    new MySqlParameter("strCompanyCode", machine.CompanyCode),
                                    new MySqlParameter("strIs_Active", machine.Is_Active),
                                    new MySqlParameter("strCreated_By", machine.Created_By)
                        };

                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_machine_insert(@`strId`, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

                        //new department after insert.
                        var newMc = context.Machine.FromSql("SELECT * FROM m_machine WHERE Id = @`strId`;").ToListAsync();
                        resultObj.ObjectValue = newMc.Result[0];

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

        public async Task<ResultObject> UpdateMachine(M_Machine machine)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = machine };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(machine).State = EntityState.Modified;

                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", machine.Id),
                                    new MySqlParameter("strMachineCode", machine.MachineCode),
                                    new MySqlParameter("strMachineName", machine.MachineName),
                                    new MySqlParameter("strMachineProdType", machine.MachineProdType),
                                    new MySqlParameter("strMachineSize", machine.MachineSize),
                                    new MySqlParameter("strMachineRemark", machine.MachineRemark),
                                    new MySqlParameter("strCompanyCode", machine.CompanyCode),
                                    new MySqlParameter("strIs_Active", machine.Is_Active),
                                    new MySqlParameter("strCreated_By", machine.Updated_By)
                        };

                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_machine_update(?, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

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

        public async Task<ResultObject> DeleteMachine(M_Machine machine)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = machine };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", machine.Id),
                                    new MySqlParameter("strDelete_By", machine.Updated_By)
                        };


                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_machine_delete( ?, ?)", parameters: sqlParams);

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
