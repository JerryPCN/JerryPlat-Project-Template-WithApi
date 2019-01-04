using JerryPlat.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JerryPlat.Web.Controllers
{
    public class QrController : Controller
    {
        // GET: Qr
        public ActionResult Index(string code, int p = 9)
        {
            Bitmap bitMap = QrCodeHelper.Create(code, p, "");
            return File(TypeHelper.Bitmap2Byte(bitMap), "image/png");
        }
    }
}