using JerryPlat.Utils.Models;

namespace JerryPlat.Utils.Helpers
{
    public class ConstantHelper
    {
        #region Response Status

        public const string Ok = "Ok";
        public const string Error = "Error";
        public const string Invalid = "Invalid";
        public const string NotFound = "NotFound";
        public const string Existed = "Existed";
        public const string Logout = "Logout";

        public const string RefreshTokenFaild = "RefreshTokenFaild";

        #endregion Response Status


        #region Api
        public static readonly string ApiBaseUrl = WebConfigModel.Instance.ApiBaseUrl;
        #endregion Api

        #region Cookie Key

        public const string TokenType = "JerryPlat_TokenType";
        public const string Token = "JerryPlat_Token";
        public const string RefreshToken = "JerryPlat_RefreshToken";

        #endregion Cookie Key

        #region Script

        private static string _constantScript;

        public static string GetScript()
        {
            if (string.IsNullOrEmpty(_constantScript))
            {
                _constantScript = TypeHelper.GetScript<ConstantHelper>("constantHelper");
            }

            return _constantScript;
        }

        #endregion Script
    }
}