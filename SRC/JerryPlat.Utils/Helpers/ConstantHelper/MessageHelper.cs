using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JerryPlat.Utils.Helpers
{
    public static class MessageHelper
    {
        public const string NoSession = "当前登陆已过期，请重新登陆。";

        public const string NoEdit = "当前记录不允许被编辑。";
        public const string NoDelete = "当前记录不允许被删除。";
        public const string NoDeleteList = "当前所选记录中存在不允许被删除的记录。";

        public const string NoPermissionToAccess = "您没有权限进行该操作，请联系系统管理人员。";
    }
}
