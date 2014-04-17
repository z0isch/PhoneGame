using System;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace PhoneGameWebApi
{
    public class UseXmlFormatterAttribute : Attribute, IControllerConfiguration
    {
        public void Initialize(HttpControllerSettings controllerSettings, HttpControllerDescriptor controllerDescriptor)
        {
            controllerSettings.Formatters.Clear();
            controllerSettings.Formatters.Add(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
        }
    }
}