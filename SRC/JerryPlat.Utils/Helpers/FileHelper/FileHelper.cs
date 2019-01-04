using System;
using System.IO;
using System.Web;

namespace JerryPlat.Utils.Helpers
{
    public static class FileHelper
    {
        public static bool IsValid(this HttpPostedFileBase file)
        {
            return true;
        }

        public static string SaveTo(this HttpPostedFileBase file, string strCategory = "")
        {
            if (file == null)
            {
                return "";
            }

            string strFilePath = file.FileName.GetPath(strCategory);
            file.SaveAs(strFilePath.ToMapPath());
            return strFilePath;
        }

        public static void Create(this string strPath)
        {
            IOHelper.CreateDir(strPath);
        }

        public static string ToMapPath(this string strPath)
        {
            return HttpContext.Current.Server.MapPath(strPath);
        }

        public static string GetPath(this string strName, string strCategory = "", string strExt = "")
        {
            if (string.IsNullOrEmpty(strCategory))
            {
                strCategory = "Upload/" + DateTime.Now.ToString("yyyy-MM-dd");
            }

            string strFilePath = "/File/" + strCategory;
            strFilePath.ToMapPath().Create();

            string strFullFilePath = strFilePath + "/" + strName;

            if (!string.IsNullOrEmpty(strExt) && !strFullFilePath.Contains("."))
            {
                strFullFilePath = strFullFilePath + "." + strExt;
            }

            return strFullFilePath;
        }
    }
}