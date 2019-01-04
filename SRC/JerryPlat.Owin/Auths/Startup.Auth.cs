using JerryPlat.Owin.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;

namespace JerryPlat.Owin
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }
        public static string PublicClientId { get; private set; }

        public void ConfigurationAuth(IAppBuilder app)
        {
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                //获取Token的路径
                TokenEndpointPath = new PathString("/Token"),
                Provider = new TokenAuthorizationServerProvider(),
                //AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(2),
                // In production mode set AllowInsecureHttp = false
#if DEBUG
                AllowInsecureHttp = true, //重要！！这里的设置包含整个流程通信环境是否启用ssl
#endif
                RefreshTokenProvider = new RefreshTokenAuthorizationProvider()
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}