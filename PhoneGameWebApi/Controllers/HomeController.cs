using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhoneGameWebApi.OAuthProviders;

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
            var g = new Google();
            //var token =  g.GetToken(code);
            //var p = g.GetPrincipal(token);

            //ViewBag.Token = token;
            ViewBag.Code = code;
            //ViewBag.GoogleID = p.Identity.Name;
            return View();
        }
    }
}
