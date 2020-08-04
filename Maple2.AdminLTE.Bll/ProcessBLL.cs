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
    public class ProcessBLL : IDisposable
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

        ~ProcessBLL()
        {
            this.Dispose(false);
        }

        #endregion

        #region Private Member

        private bool IsDisposed = false;
        private AppConfiguration appSetting;
        private DbContextOptions contextOptions;

        #endregion

        public ProcessBLL()
        {
            appSetting = new AppConfiguration();

            contextOptions = new DbContextOptionsBuilder()
                            .UseMySql(appSetting.ConnectionString)
                            .Options;
        }

        #region Method Member

        public async Task<List<M_Process>> GetProcess(int? id)
        {
            try
            {
                using (var context = new MasterDbContext(contextOptions))
                {
                    MySqlParameter[] sqlParams = new MySqlParameter[] {
                                             new MySqlParameter("strId", id)
                    };

                    return await context.Process.FromSql("call sp_process_get(?)", parameters: sqlParams).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResultObject> InsertProcess(M_Process process)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = process };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strProcessCode", process.ProcessCode),
                                    new MySqlParameter("strProcessName", process.ProcessName),
                                    new MySqlParameter("strProcessDesc", process.ProcessDesc),
                                    new MySqlParameter("strProcessSeq", process.ProcessSeq),
                                    new MySqlParameter("strCompanyCode", process.CompanyCode),
                                    new MySqlParameter("strIs_Active", process.Is_Active),
                                    new MySqlParameter("strCreated_By", process.Created_By)
                        };

                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_process_insert(@`strId`, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

                        //new department after insert.
                        var newDept = context.Process.FromSql("SELECT * FROM m_process WHERE Id = @`strId`;").ToListAsync();
                        resultObj.ObjectValue = newDept.Result[0];

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

        public async Task<ResultObject> UpdateDepartment(M_Department dept)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = dept };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(dept).State = EntityState.Modified;

                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", dept.Id),
                                    new MySqlParameter("strDeptCode", dept.DeptCode),
                                    new MySqlParameter("strDeptName", dept.DeptName),
                                    new MySqlParameter("strDeptDesc", dept.DeptDesc),
                                    new MySqlParameter("strCompanyCode", dept.CompanyCode),
                                    new MySqlParameter("strIs_Active", dept.Is_Active),
                                    new MySqlParameter("strCreated_By", dept.Updated_By)
                        };


                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_department_update(?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

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

        public async Task<ResultObject> UpdateProcess(M_Process process)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = process };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(process).State = EntityState.Modified;

                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", process.Id),
                                    new MySqlParameter("strProcessCode", process.ProcessCode),
                                    new MySqlParameter("strProcessName", process.ProcessName),
                                    new MySqlParameter("strProcessDesc", process.ProcessDesc),
                                    new MySqlParameter("strProcessSeq", process.ProcessSeq),
                                    new MySqlParameter("strCompanyCode", process.CompanyCode),
                                    new MySqlParameter("strIs_Active", process.Is_Active),
                                    new MySqlParameter("strCreated_By", process.Updated_By)
                        };


                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_process_update(?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

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
        
        public async Task<ResultObject> DeleteProcess(M_Process process)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = process };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", process.Id),
                                    new MySqlParameter("strDelete_By", process.Updated_By)
                        };


                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_process_delete( ?, ?)", parameters: sqlParams);

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
