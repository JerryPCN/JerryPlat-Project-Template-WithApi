using JerryPlat.Utils.Helpers;

namespace JerryPlat.Utils.Models
{
    public class WebConfigModel
    {
        public static WebConfigModel Instance = TypeHelper.InitModel<WebConfigModel>(ConfigHelper.GetConfig);

        public string OwinClientId { get; set; }
        public string OwinClientSecret { get; set; }
        public string ApiBaseUrl { get; set; }
    }
}