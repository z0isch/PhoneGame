using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using log4net;
using PhoneGameService;
using PhoneGameService.Models.OAuthProviders;
using PhoneGameService.Services;

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
            ViewBag.GoogleUrl = OAuthService.GetOAuthUrl(new Google());
            ViewBag.Code = code;    
            return View();
        }
    }
}
