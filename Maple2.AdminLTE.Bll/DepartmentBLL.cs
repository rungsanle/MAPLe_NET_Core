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
    public class DepartmentBLL : IDisposable
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

        ~DepartmentBLL()
        {
            this.Dispose(false);
        }

        #endregion

        #region Private Member

        private bool IsDisposed = false;
        private AppConfiguration appSetting;
        private DbContextOptions contextOptions;

        #endregion

        public DepartmentBLL()
        {
            appSetting = new AppConfiguration();

            contextOptions = new DbContextOptionsBuilder()
                            .UseMySql(appSetting.ConnectionString)
                            .Options;
        }

        #region Method Member

        public async Task<List<M_Department>> GetDepartment(int? id)
        {
            try
            {
                using (var context = new MasterDbContext(contextOptions))
                {
                    MySqlParameter[] sqlParams = new MySqlParameter[] {
                                        new MySqlParameter("strId", id)
                    };

                    return await context.Department.FromSql("call sp_department_get(?)", parameters: sqlParams).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public int InsertDepartment(ref M_Department dept)
        //{
        //    //newId = null;
        //    int rowaffected = -1;

        //    using (var context = new MasterDbContext(contextOptions))
        //    {
        //        using (var transaction = context.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                MySqlParameter[] sqlParams = new MySqlParameter[] {
        //                            new MySqlParameter("strDeptCode", dept.DeptCode),
        //                            new MySqlParameter("strDeptName", dept.DeptName),
        //                            new MySqlParameter("strDeptDesc", dept.DeptDesc),
        //                            new MySqlParameter("strCompanyCode", dept.CompanyCode),
        //                            new MySqlParameter("strIs_Active", dept.Is_Active),
        //                            new MySqlParameter("strCreated_By", dept.Created_By)
        //                };

        //                rowaffected = context.Database.ExecuteSqlCommand("call sp_department_insert(@`strId`, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

        //                //new department after insert.
        //                var newDept = context.Department.FromSql("SELECT * FROM m_department WHERE Id = @`strId`;").ToListAsync();
        //                dept = newDept.Result[0];

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

        public async Task<ResultObject> InsertDepartment(M_Department dept)
        {
            //newId = null;
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = dept };
     

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strDeptCode", dept.DeptCode),
                                    new MySqlParameter("strDeptName", dept.DeptName),
                                    new MySqlParameter("strDeptDesc", dept.DeptDesc),
                                    new MySqlParameter("strCompanyCode", dept.CompanyCode),
                                    new MySqlParameter("strIs_Active", dept.Is_Active),
                                    new MySqlParameter("strCreated_By", dept.Created_By)
                        };

                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_department_insert(@`strId`, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

                        //new department after insert.
                        var newDept = context.Department.FromSql("SELECT * FROM m_department WHERE Id = @`strId`;").ToListAsync();
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

        //public int UpdateDepartment(M_Department dept)
        //{
        //    using (var context = new MasterDbContext(contextOptions))
        //    {
        //        using (var transaction = context.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                context.Entry(dept).State = EntityState.Modified;

        //                MySqlParameter[] sqlParams = new MySqlParameter[] {
        //                            new MySqlParameter("strId", dept.Id),
        //                            new MySqlParameter("strDeptCode", dept.DeptCode),
        //                            new MySqlParameter("strDeptName", dept.DeptName),
        //                            new MySqlParameter("strDeptDesc", dept.DeptDesc),
        //                            new MySqlParameter("strCompanyCode", dept.CompanyCode),
        //                            new MySqlParameter("strIs_Active", dept.Is_Active),
        //                            new MySqlParameter("strCreated_By", dept.Updated_By)
        //                };


        //                //Output Parameter no need to define. @`strId`
        //                int rowaffected = context.Database.ExecuteSqlCommand("call sp_department_update(?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

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

        public async Task<ResultObject> DeleteDepartment(M_Department dept)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = dept };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", dept.Id),
                                    new MySqlParameter("strDelete_By", dept.Updated_By)
                        };


                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_department_delete( ?, ?)", parameters: sqlParams);

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
