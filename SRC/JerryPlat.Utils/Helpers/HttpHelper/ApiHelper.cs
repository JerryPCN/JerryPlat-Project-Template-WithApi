using JerryPlat.Utils.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace JerryPlat.Utils.Helpers
{
    public class ApiHelper : HttpHelper
    {
        public static ApiHelper Instance = SingleInstanceHelper.GetInstance<ApiHelper>();

        protected readonly string[] AryTokenName = new string[] { "client_id" };
        protected string TokenType = "Bearer";

        public override string GetBaseUrl()
        {
            return WebConfigModel.Instance.ApiBaseUrl;
        }

        private string GetTokenId()
        {
            HttpCookie cookie = null;
            foreach (string tokenName in AryTokenName)
            {
                if (!CookieHelper.IsExistCookie(tokenName))
                {
                    continue;
                }
                cookie = CookieHelper.GetCookie(tokenName);
                if (cookie != null)
                {
                    return cookie.Value;
                }
            }
            return string.Empty;
        }

        protected override AuthenticationHeaderValue GetAuthenticationHeaderValue()
        {
            string strTokenId = GetTokenId();
            if (string.IsNullOrEmpty(strTokenId))
            {
                return null;
            }

            return GetAuthenticationHeaderValue(TokenType, strTokenId);
        }

        private string GetBaseTokenId()
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(WebConfigModel.Instance.OwinClientId
                + ":" + WebConfigModel.Instance.OwinClientSecret));
        }

        private AuthenticationHeaderValue GetBaseAuthenticationHeaderValue()
        {
            string strTokenId = GetBaseTokenId();
            if (string.IsNullOrEmpty(strTokenId))
            {
                return null;
            }

            return GetAuthenticationHeaderValue("Basic", strTokenId);
        }

        public TokenModel GetToken(LoginModel loginModel)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("grant_type", "password");
            parameters.Add("UserName", loginModel.UserName);
            parameters.Add("PassWord", loginModel.Password);

            TokenModel model = PostAsync<TokenModel, Dictionary<string, string>>("/Token", 
                parameters, 
                RequestType.Form, 
                GetBaseAuthenticationHeaderValue());
            return model;
        }

        public TokenModel GetRefreshToken(string strRefreshToken)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("grant_type", "refresh_token");
            parameters.Add("refresh_token", strRefreshToken);

            TokenModel model = PostAsync<TokenModel, Dictionary<string, string>>("/Token", 
                parameters, RequestType.Form, 
                GetBaseAuthenticationHeaderValue());
            return model;
        }
    }
}