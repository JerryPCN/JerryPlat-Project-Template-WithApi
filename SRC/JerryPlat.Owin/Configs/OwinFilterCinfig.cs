using System.Web.Http;

namespace JerryPlat.Owin
{
    public static class OwinFilterCinfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Configure Web API to use only bearer token authentication.
            //config.SuppressDefaultHostAuthentication();
            //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
        }
    }
}