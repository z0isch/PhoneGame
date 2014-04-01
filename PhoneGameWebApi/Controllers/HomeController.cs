using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using log4net;
using PhoneGameService;

namespace PhoneGameWebApi.Controllers
{
    public class HomeController : Controller
    {
        private static ILog _log = LogManager.GetLogger("HomeController");

        public ActionResult Index()
        {
            LogHelper.Begin(_log, "Index()");
            return View();
        }

        public ActionResult GoogleOAuth(string code)
        {
            ViewBag.Code = code;    
            return View();
        }
    }
}
