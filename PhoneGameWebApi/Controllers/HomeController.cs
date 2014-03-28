using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PhoneGameWebApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GoogleOAuth(string code)
        {
            ViewBag.Code = code;    
            return View();
        }
    }
}
