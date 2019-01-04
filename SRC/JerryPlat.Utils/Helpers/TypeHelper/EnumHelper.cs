using System;

namespace JerryPlat.Utils.Helpers
{
    /// <summary>
    /// Need to test
    /// </summary>
    public static class EnumHelper
    {
        public static T ToEnum<T>(int intValue)
        {
            //(T)intValue
            return (T)Enum.ToObject(typeof(T), intValue);
        }

        public static T ToEnum<T>(string strValue)
        {
            return (T)Enum.Parse(typeof(T), strValue);
        }   

        public static string GetName(Type enumType, int intValue)
        {
            return Enum.GetName(enumType, intValue);
        }
    }
}