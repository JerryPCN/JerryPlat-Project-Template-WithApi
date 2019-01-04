using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JerryPlat.Utils.Helpers
{
    public static class DbHelper
    {
        public static async Task<string> BackUpAsync(string strMasterConnStr, string strDbName, string strBackUpDbName)
        {
            using (DbContext context = new DbContext(strMasterConnStr))
            {
                try
                {
                    await context.Database.ExecuteSqlCommandAsync(TransactionalBehavior.DoNotEnsureTransaction
                        , $"backup database {strDbName} to disk='{strBackUpDbName}';");
                    return ConstantHelper.Ok;
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                    return ex.Message;
                }
            }
        }
        
        public static async Task<string> KillSysProcessAsync(string strMasterConnStr, string strDbName)
        {
            using (DbContext context = new DbContext(strMasterConnStr))
            {
                try
                {
                    var sysProcesses = context.Database.SqlQuery<short>($"select spid from master..sysprocesses where dbid=db_id('{strDbName}');").ToList();
                    foreach (var sysProcess in sysProcesses)
                    {
                        await context.Database.ExecuteSqlCommandAsync(TransactionalBehavior.DoNotEnsureTransaction
                        , $"kill {sysProcess};");
                    }

                    return ConstantHelper.Ok;
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                    return ex.Message;
                }
            }
        }

        public static async Task<string> RestoreAsync(string strMasterConnStr, string strDbName, string strBackUpDbName)
        {
            string strResult = await KillSysProcessAsync(strMasterConnStr, strDbName);
            if (strResult != ConstantHelper.Ok)
            {
                return strResult;
            }

            using (DbContext context = new DbContext(strMasterConnStr))
            {
                try
                {
                    await context.Database.ExecuteSqlCommandAsync(TransactionalBehavior.DoNotEnsureTransaction
                        , $"backup log {strDbName} to disk='{strBackUpDbName}' with NORECOVERY;restore database {strDbName} from disk='{strBackUpDbName}';");
                    return ConstantHelper.Ok;
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                    return ex.Message;
                }
            }
        }
    }
}
