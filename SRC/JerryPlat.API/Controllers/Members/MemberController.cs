using JerryPlat.BLL.CommonManage;
using JerryPlat.Models;
using JerryPlat.Utils.Models;
using System.Threading.Tasks;
using System.Web.Http;

namespace JerryPlat.API.Controllers
{
    public class MemberController : AdminAuthurizeBaseApiController<AdminMemberHelper, Member>
    {
        
    }
}