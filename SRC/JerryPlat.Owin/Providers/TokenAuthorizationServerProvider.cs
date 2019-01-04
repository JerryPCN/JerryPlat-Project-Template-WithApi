using JerryPlat.BLL.AdminManage;
using JerryPlat.BLL.CommonManage;
using JerryPlat.Models;
using JerryPlat.Utils.Helpers;
using JerryPlat.Utils.Models;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JerryPlat.Owin.Providers
{
    public class TokenAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId, clientSecret;
            context.TryGetBasicCredentials(out clientId, out clientSecret);

            OwinTokenHelper helper = new OwinTokenHelper();

            if(helper.Exist(clientId, clientSecret))
            {
                context.Validated();
            }            

            return Task.FromResult<object>(null);
        }

        /// <summary>
        ///  clientId, clientSecret认证 "grant_type": "client_credentials"
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            var oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
            oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, context.ClientId));
            var ticket = new AuthenticationTicket(oAuthIdentity, new AuthenticationProperties());
            context.Validated(ticket);
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 刷新token "grant_type", "refresh_token"
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["client_id"];
            var currentClient = context.ClientId;
            if (originalClient != currentClient)
            {
                context.Rejected();
                return Task.FromResult<object>(null);
            }

            var newId = new ClaimsIdentity(context.Ticket.Identity);
            newId.AddClaim(new Claim("newClaim", "refreshToken"));

            var newTickets = new AuthenticationTicket(newId, context.Ticket.Properties);
            context.Validated(newTickets);

            return Task.FromResult<object>(null);
        }
        
        /// <summary>
        /// 验证password "grant_type": "password"
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            AdminUserHelper helper = new AdminUserHelper();
            AdminUser adminUser = helper.Get(new LoginModel { UserName = context.UserName, Password = context.Password });
            if (adminUser == null)
            {
                LogHelper.Info($"invalid_grant:password, UserName:{context.UserName}, Password:{context.Password}, Invalid UserName and Password.");
                context.SetError("invalid_grant", "用户名或密码不正确。");
                return Task.FromResult<object>(null);
            }

            //此处通过token得到用户  可以进行判断权限
            var oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
            oAuthIdentity.AddClaim(new Claim(ClaimTypes.PrimarySid, adminUser.Id.ToString()));
            oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, adminUser.UserName));
            oAuthIdentity.AddClaim(new Claim(ClaimTypes.System, SiteType.Admin.ToString()));
            oAuthIdentity.AddClaim(new Claim(ClaimTypes.Role, adminUser.GroupId.ToString()));
            oAuthIdentity.AddClaim(new Claim(ClaimTypes.UserData, SerializationHelper.ToJson(adminUser)));

            var props = new AuthenticationProperties(new Dictionary<string, string>
            {
                { "client_id", context.ClientId }
            });
            var ticket = new AuthenticationTicket(oAuthIdentity, props);
            context.Validated(ticket);
            return Task.FromResult<object>(null);
        }
    }
}