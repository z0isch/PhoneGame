﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PhoneGameWebApi
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        public static Dictionary<string, string> Users = new Dictionary<string, string>();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();


            GlobalConfiguration.Configure(WebApiConfig.Register);
            
            //Do not use for WebApi2
            //WebApiConfig.Register(GlobalConfiguration.Configuration);

            PhoneGameService.Logging.LoggingConfiguration.Initialize(Server.MapPath("bin/Log4NetConfig.xml"));

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}