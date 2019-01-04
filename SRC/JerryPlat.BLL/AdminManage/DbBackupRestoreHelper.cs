using JerryPlat.DAL;
using JerryPlat.Models;
using JerryPlat.Models.Dto;
using JerryPlat.Utils.Helpers;
using JerryPlat.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JerryPlat.BLL.AdminManage
{
    public class DbBackupRestoreHelper : AdminBaseSessionHelper
    {
        public async Task<PageData<DbBackupRestore>> GetPageListAsync<TKey>(SearchModel searchModel,
            Expression<Func<DbBackupRestore, TKey>> orderByKeySelector,
            PageParam pageParam,
            bool bIsAscOrder = true)
        {
            IQueryable<DbBackupRestore> dbBackupRestoreQuerable = GetDbSet<DbBackupRestore>();
            
            if (searchModel.StartTime.HasValue)
            {
                dbBackupRestoreQuerable = dbBackupRestoreQuerable.Where(o => o.CreateTime >= searchModel.StartTime.Value);
            }

            if (searchModel.EndTime.HasValue)
            {
                dbBackupRestoreQuerable = dbBackupRestoreQuerable.Where(o => o.CreateTime < searchModel.EndTime.Value);
            }

            return await PageHelper.GetPageDataAsync(dbBackupRestoreQuerable, orderByKeySelector, pageParam, bIsAscOrder, searchModel.Sort);
        }


        public async Task<string> BackUpAsync()
        {
            string strFilePath = ($"{SystemConfigModel.Instance.BackUpDbName}_{DateTime.Now.ToFormat("yyyyMMddhhmmss")}")
                                .GetPath("Database", "bak");
            string strResult = await DbHelper.BackUpAsync(SystemConfigModel.Instance.MasterDbConnStr
                , SystemConfigModel.Instance.DbName
                , strFilePath.ToMapPath());

            if (strResult == ConstantHelper.Ok)
            {
                DbBackupRestore model = new DbBackupRestore
                {
                    BackupPath = strFilePath
                };
                await SaveAsync(model);
            }

            return strResult;
        }

        public async Task<string> RestoreAsync(int id)
        {
            DbBackupRestore model = await GetByIdAsync<DbBackupRestore>(id);
            return await RestoreAsync(model);
        }

        public async Task<string> RestoreAsync(DbBackupRestore model)
        {
            string strResult = await DbHelper.RestoreAsync(SystemConfigModel.Instance.MasterDbConnStr
                , SystemConfigModel.Instance.DbName
                , model.BackupPath.ToMapPath());

            if (strResult == ConstantHelper.Ok)
            {
                DbContextHelper.Init();

                //Restore Db will kill the sysprocesses.
                ResetDbContext();

                if(!await ExistByIdAsync<DbBackupRestore>(model.Id))
                {
                    model.Id = 0;
                }
                
                model.RestoreCount = model.RestoreCount + 1;
                model.RestoreTime = DateTime.Now;
                await SaveAsync(model);
            }

            return strResult;
        }
    }
}
