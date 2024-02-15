using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace intern
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Enable attribute routing
            config.MapHttpAttributeRoutes();

            // Default API route
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Custom route for RemoveDevice action
            config.Routes.MapHttpRoute(
                name: "RemoveDeviceApi",
                routeTemplate: "api/{controller}/RemoveDevice/{id}",
                defaults: new { id = RouteParameter.Optional, action = "RemoveDevice" }
            );
        }
    }
}
