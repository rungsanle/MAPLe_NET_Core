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
    public class UserBLL : IDisposable
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

        ~UserBLL()
        {
            this.Dispose(false);
        }

        #endregion

        #region Private Member

        private bool IsDisposed = false;
        private AppConfiguration appSetting;
        private DbContextOptions<MasterDbContext> contextOptions;

        #endregion

        public UserBLL()
        {
            appSetting = new AppConfiguration();

            contextOptions = new DbContextOptionsBuilder<MasterDbContext>()
                            .UseMySql(appSetting.ConnectionString)
                            .Options;
        }

        #region Method Member

        public async Task<List<M_User>> GetUser(int? id)
        {
            try
            {
                using (var context = new MasterDbContext(contextOptions))
                {
                    MySqlParameter[] sqlParams = new MySqlParameter[] {
                                             new MySqlParameter("strId", id)
                    };

                    var objList = await context.Query<M_UserObj>().FromSql("call sp_user_get(?)", parameters: sqlParams).ToListAsync();

                    return objList.ConvertAll(user => new M_User {
                        Id = user.Id,
                        UserCode = user.UserCode,
                        UserName = user.UserName,
                        EmpCode = user.EmpCode,
                        DeptId = user.DeptId,
                        DeptName = user.DeptName,
                        Position = user.Position,
                        CompanyCode = user.CompanyCode,
                        aspnetuser_Id = user.aspnetuser_Id,
                        UserImagePath = user.UserImagePath,
                        Is_Active = user.Is_Active,
                        Created_By = user.Created_By,
                        Created_Date = user.Created_Date,
                        Updated_By = user.Updated_By,
                        Updated_Date = user.Updated_Date
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResultObject> InsertUser(M_User user)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = user };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strUserCode", user.UserCode),
                                    new MySqlParameter("strUserName", user.UserName),
                                    new MySqlParameter("strEmpCode", user.EmpCode),
                                    new MySqlParameter("strDeptId", user.DeptId),
                                    new MySqlParameter("strPosition", user.Position),
                                    new MySqlParameter("strCompanyCode", user.CompanyCode),
                                    new MySqlParameter("straspnetuser_Id", user.aspnetuser_Id),
                                    new MySqlParameter("strIs_Active", user.Is_Active),
                                    new MySqlParameter("strCreated_By", user.Created_By),
                                    new MySqlParameter("strUserImagePath", user.UserImagePath)
                        };

                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_user_insert(@`strId`, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

                        //new department after insert.
                        var newUser = context.User.FromSql("SELECT * FROM m_user WHERE Id = @`strId`;").ToListAsync();
                        resultObj.ObjectValue = newUser.Result[0];

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

        public async Task<ResultObject> UpdateUser(M_User user)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = user };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(user).State = EntityState.Modified;

                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", user.Id),
                                    new MySqlParameter("strUserCode", user.UserCode),
                                    new MySqlParameter("strUserName", user.UserName),
                                    new MySqlParameter("strEmpCode", user.EmpCode),
                                    new MySqlParameter("strDeptId", user.DeptId),
                                    new MySqlParameter("strPosition", user.Position),
                                    new MySqlParameter("strCompanyCode", user.CompanyCode),
                                    new MySqlParameter("straspnetuser_Id", user.aspnetuser_Id),
                                    new MySqlParameter("strIs_Active", user.Is_Active),
                                    new MySqlParameter("strUpdated_By", user.Updated_By),
                                    new MySqlParameter("strUserImagePath", user.UserImagePath)
                        };

                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_user_update(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

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

        public async Task<ResultObject> DeleteUser(M_User user)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = user };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", user.Id),
                                    new MySqlParameter("strDelete_By", user.Updated_By)
                        };

                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_user_delete( ?, ?)", parameters: sqlParams);

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



        public async Task<List<M_User>> GetSystemUser(string id)
        {
            try
            {
                using (var context = new MasterDbContext(contextOptions))
                {

                    MySqlParameter[] sqlParams = new MySqlParameter[] {
                                             new MySqlParameter("strId", id)
                    };

                    var objList = await context.Query<M_UserObj>().FromSql("call sp_systemuser_get(?)", parameters: sqlParams).ToListAsync();

                    return objList.ConvertAll(user => new M_User
                    {
                        Id = user.Id,
                        UserCode = user.UserCode,
                        UserName = user.UserName,
                        EmpCode = user.EmpCode,
                        DeptId = user.DeptId,
                        DeptName = user.DeptName,
                        Position = user.Position,
                        CompanyCode = user.CompanyCode,
                        aspnetuser_Id = user.aspnetuser_Id,
                        UserImagePath = user.UserImagePath,
                        CompanyLogoPath = user.CompanyLogoPath,
                        Is_Active = user.Is_Active,
                        Created_By = user.Created_By,
                        Created_Date = user.Created_Date,
                        Updated_By = user.Updated_By,
                        Updated_Date = user.Updated_Date
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int?> GetSystemUserId(string id)
        {
            try
            {
                using (var context = new MasterDbContext(contextOptions))
                {

                    MySqlParameter[] sqlParams = new MySqlParameter[] {
                                             new MySqlParameter("strId", id)
                    };

                    var userResult = await context.User.FromSql("call sp_systemuser_get(?)", parameters: sqlParams).AsNoTracking().FirstOrDefaultAsync();

                     return (userResult != null ? userResult.Id : (int?)null);

                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
