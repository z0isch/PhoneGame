using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using PhoneGameWebApi.MessageHandlers;

namespace PhoneGameWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Trying out the new WebApi2 attribute routing http://www.asp.net/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            config.MessageHandlers.Add(new OAuthHandler());

            //config.Formatters.XmlFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));
            var xml = GlobalConfiguration.Configuration.Formatters.XmlFormatter;
            xml.UseXmlSerializer = true;

            /* These could be used for RPC style calls
             * 
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
             * 
             */

        }
    }
}
