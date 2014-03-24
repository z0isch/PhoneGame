using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace PhoneGameWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Route Catches the GET PUT DELETE typical REST based interactions (add more if needed)
            config.Routes.MapHttpRoute("API Default", "api/{controller}/{id}",
                new { id = RouteParameter.Optional },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Get, HttpMethod.Put, HttpMethod.Delete) }
                );

            //This allows POSTs to the RPC Style methods http://api/controller/action
            config.Routes.MapHttpRoute("API RPC Style", "api/{controller}/{action}",
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) });

            //Finally this allows POST to typeical REST post address http://api/controller/
            config.Routes.MapHttpRoute("API Default 2", "api/{controller}/{action}",
                new { action = "Post" },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) });
        }
    }
}
