using JerryPlat.DAL;
using JerryPlat.Models;
using JerryPlat.Utils.Helpers;
using System.Linq;

namespace JerryPlat.BLL.CommonManage
{
    public class OwinTokenHelper : BaseHelper<OwinToken>
    {
        public bool Exist(string strClientId, string strClientSecret)
        {
            strClientSecret = EncryptHelper.Encrypt(strClientSecret);
            return GetDbSet<OwinToken>().Where(o =>
                    o.ClientId == strClientId
                    & o.ClientSecret == strClientSecret
                ).Any();
        }
    }
}
