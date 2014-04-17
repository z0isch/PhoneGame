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
            _log.Debug("Index()");
            return View();
        }

        public ActionResult GoogleOAuth(string code)
        {
            ViewBag.GoogleUrl =new Google().GetOAuthUrl();
            ViewBag.Code = code;    
            return View();
        }
    }
}
