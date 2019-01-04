using JerryPlat.Utils.Helpers;
using System.Collections.Generic;

namespace JerryPlat.Utils.Models
{
    public class SystemConfigModel
    {
        public static SystemConfigModel Instance;

        public static void Reset(Dictionary<string, string> keyValueList)
        {
            Instance = TypeHelper.InitModel<SystemConfigModel>(keyValueList);
        }

        public static void Reset(SystemConfigModel model)
        {
            Instance = model;
        }

        #region 微信登陆
        public bool IsUseWechatLogin { get; set; }
        #endregion

        #region 允许跨域访问域名
        public string AllowOriginSites { get; set; }
        #endregion

        #region 分享设置
        public string ShareTitle { get; set; }
        public string ShareContent { get; set; }
        #endregion 分享设置

        #region 数据库管理
        public string MasterDbConnStr { get; set; }
        public string DbName { get; set; }
        public string BackUpDbName { get; set; }
        #endregion
    }
}