﻿using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               "Admin_Upload",
               "Admin/Upload/{folder}",
               new { controller = "Upload", action = "Index" },
               new string[] { "JerryPlat.Web.Areas.Admin.Controllers" }
           );

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "JerryPlat.Web.Areas.Admin.Controllers" }
            );
        }
    }
}