﻿using System.Web.Mvc;

namespace JerryPlat.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Redirect("/Admin/");
            //return View();
        }
    }
}