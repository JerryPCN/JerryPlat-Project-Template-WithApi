using JerryPlat.BLL.CommonManage;
using JerryPlat.Models;
using JerryPlat.Models.Dto;
using JerryPlat.Office;
using JerryPlat.Utils.Helpers;
using JerryPlat.Utils.Models;
using JerryPlat.Web.Areas.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Admin.Controllers
{
    public class MemberController : AdminAuthurizeBaseController<AdminMemberHelper, Member>
    {
        public async Task<ActionResult> Export(SearchModel search)
        {
            var memberList = await _helper.GetTeamListAsync(search, o => o.JoinTime, false);
            return File(ExcelHelper.SaveExcelContent(memberList)
                        , ExcelHelper.Excel_ContentType
                        , ExcelHelper.GetExcelName("导出参赛人员"));
        }
    }
}