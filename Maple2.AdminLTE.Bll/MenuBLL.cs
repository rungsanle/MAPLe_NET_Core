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
    public class MenuBLL : IDisposable
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

        ~MenuBLL()
        {
            this.Dispose(false);
        }

        #endregion

        #region Private Member

        private bool IsDisposed = false;
        private AppConfiguration appSetting;
        private DbContextOptions contextOptions;

        #endregion

        public MenuBLL()
        {
            appSetting = new AppConfiguration();

            contextOptions = new DbContextOptionsBuilder()
                            .UseMySql(appSetting.ConnectionString)
                            .Options;
        }

        #region Method Member

        public async Task<List<M_Menu>> GetMenu(int? id)
        {
            try
            {
                using (var context = new MasterDbContext(contextOptions))
                {
                    MySqlParameter[] sqlParams = new MySqlParameter[] {
                                        new MySqlParameter("strId", id)
                    };

                    var objList = await context.Query<M_MenuObj>().FromSql("call sp_menu_get(?)", parameters: sqlParams).ToListAsync();

                    return objList.ConvertAll(menu => new M_Menu
                    {
                        Id = menu.Id,
                        nameOption = menu.nameOption,
                        controller = menu.controller,
                        action = menu.action,
                        imageClass = menu.imageClass,
                        status = menu.status,
                        isParent = menu.isParent,
                        parentId = menu.parentId,
                        parentName = menu.parentName,
                        area = menu.area,
                        menuseq = menu.menuseq,
                        Is_Active = menu.Is_Active,
                        Created_By = menu.Created_By,
                        Created_Date = menu.Created_Date,
                        Updated_By = menu.Updated_By,
                        Updated_Date = menu.Updated_Date
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<M_Menu>> GetMenuAuthen(int? userId)
        {
            try
            {
                using (var context = new MasterDbContext(contextOptions))
                {
                    MySqlParameter[] sqlParams = new MySqlParameter[] {
                                        new MySqlParameter("strUserId", userId)
                    };

                    return await context.NavBarMenu.FromSql("call sp_menuauthen_get(?)", parameters: sqlParams).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResultObject> InsertMenu(M_Menu menu)
        {
            //newId = null;
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = menu };


            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strnameOption", menu.nameOption),
                                    new MySqlParameter("strcontroller", menu.controller),
                                    new MySqlParameter("straction", menu.action),
                                    new MySqlParameter("strimageClass", menu.imageClass),
                                    new MySqlParameter("strstatus", menu.status),
                                    new MySqlParameter("strisParent", menu.isParent),
                                    new MySqlParameter("strparentId", menu.parentId),
                                    new MySqlParameter("strarea", menu.area),
                                    new MySqlParameter("strmenuseq", menu.menuseq),
                                    new MySqlParameter("strIs_Active", menu.Is_Active),
                                    new MySqlParameter("strCreated_By", menu.Created_By)
                        };

                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_menu_insert(@`strId`, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

                        //new department after insert.
                        var newMenu = context.NavBarMenu.FromSql("SELECT * FROM m_menu WHERE Id = @`strId`;").ToListAsync();
                        resultObj.ObjectValue = newMenu.Result[0];

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

        public async Task<ResultObject> UpdateMenu(M_Menu menu)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = menu };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Entry(menu).State = EntityState.Modified;

                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", menu.Id),
                                    new MySqlParameter("strnameOption", menu.nameOption),
                                    new MySqlParameter("strcontroller", menu.controller),
                                    new MySqlParameter("straction", menu.action),
                                    new MySqlParameter("strimageClass", menu.imageClass),
                                    new MySqlParameter("strstatus", menu.status),
                                    new MySqlParameter("strisParent", menu.isParent),
                                    new MySqlParameter("strparentId", menu.parentId),
                                    new MySqlParameter("strarea", menu.area),
                                    new MySqlParameter("strmenuseq", menu.menuseq),
                                    new MySqlParameter("strIs_Active", menu.Is_Active),
                                    new MySqlParameter("strCreated_By", menu.Updated_By)
                        };


                        //Output Parameter no need to define. @`strId`
                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_menu_update(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", parameters: sqlParams);

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

        public async Task<ResultObject> DeleteMenu(M_Menu menu)
        {
            var resultObj = new ResultObject { RowAffected = -1, ObjectValue = menu };

            using (var context = new MasterDbContext(contextOptions))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        MySqlParameter[] sqlParams = new MySqlParameter[] {
                                    new MySqlParameter("strId", menu.Id),
                                    new MySqlParameter("strDelete_By", menu.Updated_By)
                        };

                        resultObj.RowAffected = await context.Database.ExecuteSqlCommandAsync("call sp_menu_delete( ?, ?)", parameters: sqlParams);

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
